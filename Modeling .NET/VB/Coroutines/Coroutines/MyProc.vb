Imports Simulation

Public Class MyProc
    Inherits Coroutine
    Public Name As String
    Public NNext As MyProc

    Public Sub New(aName as String)
        Name = aName
    End Sub

    Protected Overrides Sub Run()
        MyBase.Run()
        Dim i As Integer
        For i = 0 To 4
            Console.WriteLine("Сопрограмма {0}: {1}", Name, i)
            SwitchTo(NNext)
        Next
    End Sub
End Class
