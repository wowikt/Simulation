Public Class Board
    Public Const QueenCount = 5
    Shared Rows() As Boolean = New Boolean(QueenCount - 1) {}
    Shared DiagsUp() As Boolean = New Boolean(2 * QueenCount - 2) {}
    Shared DiagsDown() As Boolean = New Boolean(2 * QueenCount - 2) {}
    Public Shared Queens() As Queen = New Queen(QueenCount - 1) {}

    Public Shared Function IsFree(ByVal Col As Byte, ByVal Row As Byte) As Boolean
        Return Not Rows(Row) And Not DiagsUp(Col + QueenCount - Row - 1) And Not DiagsDown(Col + Row)
    End Function

    Public Shared Sub MakeOccupied(ByVal Col As Byte, ByVal Row As Byte)
        Rows(Row) = True
        DiagsUp(Col + QueenCount - Row - 1) = True
        DiagsDown(Col + Row) = True
    End Sub

    Public Shared Sub MakeFree(ByVal Col As Byte, ByVal Row As Byte)
        Rows(Row) = False
        DiagsUp(Col + QueenCount - Row - 1) = False
        DiagsDown(Col + Row) = False
    End Sub

    Public Shared Sub Remember()
        Dim i As Integer
        For i = 0 To QueenCount - 1
            Console.Write("{0} ", Queens(i).Row)
        Next
        Console.WriteLine()
    End Sub
End Class


