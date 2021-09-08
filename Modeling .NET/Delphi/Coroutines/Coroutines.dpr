program Coroutines;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Simulation,
  MyProc in 'MyProc.pas';

var
  corA, corB, corC : TMyProc;

begin
  try
    { TODO -oUser -cConsole Main : Insert code here }
    corA := TMyProc.Create('A');
    corB := TMyProc.Create('B');
    corC := TMyProc.Create('C');
    corA.Next := corB;
    corB.Next := corC;
    corC.Next := corA;
    Console.WriteLine('Готово. Нажми Enter.');
    Console.ReadLine;
    corA.SwitchTo;
    corA.Finish;
    corB.Finish;
    corC.Finish;
    Console.WriteLine('Выполнено.');
    Console.ReadLine;
  except
    on E:Exception do
      Writeln(E.Classname, ': ', E.Message);
  end;
end.
