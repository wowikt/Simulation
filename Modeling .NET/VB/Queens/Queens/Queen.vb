Imports Simulation

Public Class Queen
    Inherits Coroutine

    Public Row As Byte
    Public Col As Byte

    Public Sub New(ACol as Byte)
        Col = ACol
        Row = 0
    End Sub

    Protected Overrides Sub Run()
        Do
            If Board.IsFree(Col, Row) Then
                Board.MakeOccupied(Col, Row)
                If Col < Board.QueenCount - 1 Then
                    SwitchTo(Board.Queens(Col + 1))
                Else
                    Board.Remember()
                End If
                Board.MakeFree(Col, Row)
            End If
            Row = Row + 1
            If Row = Board.QueenCount Then
                Row = 0
                If Col = 0 Then
                    Exit Sub
                Else
                    SwitchTo(Board.Queens(Col - 1))
                End If
            End If
        Loop
    End Sub
End Class

