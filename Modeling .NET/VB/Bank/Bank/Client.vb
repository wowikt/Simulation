Imports Simulation

Public Class Client
    Inherits Process

    Protected Overrides Sub Execute()
        Dim bsPar As BankSimulation = CType(Parent, BankSimulation)
        bsPar.Cash.Activate()
        Wait(bsPar.Queue)
        GoFinished()
    End Sub
End Class
