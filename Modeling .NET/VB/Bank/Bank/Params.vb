Imports Simulation

''' <summary>
''' Класс Params содержит параметры имитации
''' </summary>
''' <remarks></remarks>
Public Class Params
    ''' <summary>
    ''' Нижняя граница гистограммы
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared HistMin As Double = 2

    ''' <summary>
    ''' Величина шага гистограммы
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared HistStep As Double = 1

    ''' <summary>
    ''' Количество шагов гистограммы
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared HistStepCount As Integer = 20

    ''' <summary>
    ''' Максимальное время обслуживания клиента кассиром
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MaxCashTime As Double = 6

    ''' <summary>
    ''' Количество обслуживаемых клиентов
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MaxClientCount As Integer = 100

    ''' <summary>
    ''' Средний интервал прибытия клиентов
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MeanClientInterval As Double = 5

    ''' <summary>
    ''' Минимальное время обслуживания клиента кассиром
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared MinCashTime As Double = 2
End Class
