#define _WIN32_WINNT 0x400

#using <mscorlib.dll>
#include <windows.h>
#include <winnt.h>
#include <mscoree.h>

#if defined(Yield)
#undef Yield
#endif

#define CORHOST

namespace Fibers {

typedef System::Runtime::InteropServices::GCHandle GCHandle;

VOID CALLBACK unmanaged_fiberproc(PVOID pvoid);

private ref struct StopFiber {};

enum FiberStateEnum {
	FiberCreated, FiberRunning, FiberStopPending, FiberStopped
};

#pragma unmanaged

#if defined(CORHOST)
ICorRuntimeHost *corhost;

void initialize_corhost() {
	CorBindToCurrentRuntime(0, CLSID_CorRuntimeHost,
		IID_ICorRuntimeHost, (void**) &corhost);
}

#endif

void CorSwitchToFiber(void *fiber) {
#if defined(CORHOST)
	DWORD *cookie;
	corhost->SwitchOutLogicalThreadState(&cookie);
#endif
	SwitchToFiber(fiber);
#if defined(CORHOST)
	corhost->SwitchInLogicalThreadState(cookie);
#endif
}

#pragma managed

public delegate System::Object^ Coroutine();

public ref class Fiber abstract : public System::IDisposable {
public:
#if defined(CORHOST)
	static Fiber() { initialize_corhost(); }
#endif

	Fiber() : retval(nullptr), state(FiberCreated) {
		void *objptr = (void*) GCHandle::ToIntPtr(GCHandle::Alloc(this));
		fiber = CreateFiber(0, unmanaged_fiberproc, objptr);
	}


	property bool IsRunning {
		bool get()
		{
			return state != FiberStopped;
		}
	}

	operator Coroutine^ () {
		return gcnew Coroutine(this, &Fiber::Resume);
	}

	System::Object^ Resume() {
		if(!fiber || state == FiberStopped)
			return nullptr;
		initialize_thread();
		void *current = GetCurrentFiber();
		if(fiber == current)
			return nullptr;
		previousfiber = current;
		//System::Console::WriteLine("\nFIBERS.DLL: Resume: Prev fiber = {0}, new fiber = {1} at {2}", (int) previousfiber, (int) fiber, this);
		CorSwitchToFiber(fiber);
		return retval;
	}

	~Fiber() {
		if(fiber) {
			if(state  == FiberRunning) {
				initialize_thread();
				void *current = GetCurrentFiber();
				if(fiber == current)
					return;
				previousfiber = current;
				state = FiberStopPending;
				CorSwitchToFiber(fiber);
			} else if(state == FiberCreated) {
				state = FiberStopped;
			}
			DeleteFiber(fiber);
			fiber = 0;
		}
	}
protected:
	virtual void Run() = 0;
	void Yield(System::Object ^obj) {
		retval = obj;
		//System::Console::WriteLine("\nFIBERS.DLL: Yield: Prev fiber = {0}, new fiber = {1} at {2}", (int) previousfiber, (int) fiber, this);
		CorSwitchToFiber(previousfiber);
		if(state == FiberStopPending)
			throw gcnew StopFiber;
	}
private:
	[System::ThreadStatic] static bool thread_is_fiber;

	void *fiber, *previousfiber;
	FiberStateEnum state;
	System::Object ^retval;

	static void initialize_thread() {
		if(!thread_is_fiber) {
			ConvertThreadToFiber(0);
			thread_is_fiber = true;
		}
	}
internal:
	void* main() {
		state = FiberRunning;
		try {
			Run();
		} catch(System::Object ^x) {
			//System::Console::Error->WriteLine("\nFIBERS.DLL: main Caught {0}", x);
		}
		state = FiberStopped;
		retval = nullptr;
		return previousfiber;
	}
};

void* fibermain(void* objptr) {
	System::IntPtr ptr = (System::IntPtr) objptr;
	GCHandle g = GCHandle::FromIntPtr(ptr);
	Fiber ^fiber = static_cast<Fiber^>(g.Target);
	g.Free();
	return fiber->main();
}

#pragma unmanaged

VOID CALLBACK unmanaged_fiberproc(PVOID objptr) {
#if defined(CORHOST)
	corhost->CreateLogicalThreadState();
#endif
	void *previousfiber = fibermain(objptr);
#if defined(CORHOST)
	corhost->DeleteLogicalThreadState();
#endif
	SwitchToFiber(previousfiber);
}

} // namespace fibers
