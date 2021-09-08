Imports Simulation

Public Class Cashman
    Inherits Process

    Protected Overrides Sub Execute()
        Dim bsPar As BankSimulation = CType(Parent, BankSimulation)
        Do
            Do While bsPar.Queue.Empty
                Passivate()
            Loop

            Dim clt As Client = CType(bsPar.Queue.First, Client)
            clt.StartRunning()

            If clt.StartingTime = SimTime() Then
                bsPar.NotWaited = bsPar.NotWaited + 1
            End If

            bsPar.CashStat.Start(SimTime)
            Hold(BankSimulation.RandCashman.Uniform(Params.MinCashTime, _
                                                    Params.MaxCashTime))
            bsPar.CashStat.Finish(SimTime)

            Dim inBankTime As Double = SimTime() - clt.StartingTime
            bsPar.InBankHist.AddData(inBankTime)
            bsPar.InBankStat.AddData(inBankTime)
            clt.Activate()

            If bsPar.CashStat.Finished = Params.MaxClientCount Then
                bsPar.Activate()
            End If
        Loop
    End Sub
End Class
