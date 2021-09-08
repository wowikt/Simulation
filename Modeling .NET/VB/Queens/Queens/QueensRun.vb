Imports Simulation

Public Class QueensRun
    Inherits Coroutine

    Protected Overrides Sub Run()
        Dim i As Byte
        For i = 0 To Board.QueenCount - 1
            Board.Queens(i) = New Queen(i)
        Next
        SwitchTo(Board.Queens(0))
        For i = 0 To Board.QueenCount - 1
            Board.Queens(i).Finish()
        Next
        Detach()
    End Sub
End Class

