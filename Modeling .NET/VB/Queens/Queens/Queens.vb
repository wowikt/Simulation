Imports Simulation

Module Queens

    Sub Main()
        Dim QRun As QueensRun = New QueensRun
        SimGlobal.SwitchTo(QRun)
        QRun.Finish()
        Console.WriteLine("Готово.")
        Console.ReadLine()
    End Sub

End Module
