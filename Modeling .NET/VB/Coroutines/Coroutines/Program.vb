Imports Simulation

Module Program

    Sub Main()
        Dim corA As MyProc = New MyProc("A")
        Dim corB As MyProc = New MyProc("B")
        Dim corC As MyProc = New MyProc("C")
        corA.NNext = corB
        corB.NNext = corC
        corC.NNext = corA
        Console.WriteLine("Готово. Нажми Enter.")
        Console.ReadLine()
        SimGlobal.SwitchTo(corA)
        corA.Finish()
        corB.Finish()
        corC.Finish()
        Console.WriteLine("Выполнено.")
        Console.ReadLine()

    End Sub

End Module
