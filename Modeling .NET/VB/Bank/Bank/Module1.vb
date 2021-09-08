Imports Simulation

Module Module1

    Sub Main()
        BankSimulation.RandCashman = New Simulation.Random
        BankSimulation.RandClient = New Simulation.Random

        Dim bs As New BankSimulation

        bs.Start()

        Console.WriteLine("Имитация завершена в момент {0,6:0.000}", bs.SimTime())
        Console.WriteLine()
        Console.WriteLine(bs.InBankStat)
        Console.WriteLine()
        Console.WriteLine(bs.CashStat)
        Console.WriteLine()
        Console.WriteLine(bs.Queue.Statistics())
        Console.WriteLine()
        Console.WriteLine(bs.Calendar.Statistics())
        Console.WriteLine()
        Console.WriteLine("Обслужено без ожидания: {0}", bs.NotWaited)
        Console.WriteLine()
        Console.WriteLine(bs.InBankHist)

        bs.Finish()
        Console.WriteLine("Готово")
        Console.ReadLine()
    End Sub

End Module
