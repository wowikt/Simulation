Imports Simulation

Public Class ClientGenerator
    Inherits Process

    Protected Overrides Sub Execute()
        Dim i As Integer
        For i = 0 To Params.MaxClientCount - 1
            ClearFinished()
            Dim clt As New Client
            clt.Activate()
            Hold(BankSimulation.RandClient.Exponential(Params.MeanClientInterval))
        Next
    End Sub
End Class
