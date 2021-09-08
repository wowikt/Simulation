Imports Simulation

''' <summary>
''' Класс BankSimulation имитирует работу банка
''' </summary>
''' <remarks></remarks>
Public Class BankSimulation
    Inherits SimProc

    ''' <summary>
    ''' Статистика по загруженности кассира
    ''' </summary>
    ''' <remarks></remarks>
    Public CashStat As ServiceStatistics

    ''' <summary>
    ''' Гистограмма по времени нахождения клиентов в банке
    ''' </summary>
    ''' <remarks></remarks>
    Public InBankHist As Histogram

    ''' <summary>
    ''' Статистика по времени нахождения клиентов в банке
    ''' </summary>
    ''' <remarks></remarks>
    Public InBankStat As Statistics

    ''' <summary>
    ''' Количество клиентов, обслуженных без ожидания
    ''' </summary>
    ''' <remarks></remarks>
    Public NotWaited As Integer

    ''' <summary>
    ''' Очередь клиентов, ожидающих обслуживания
    ''' </summary>
    ''' <remarks></remarks>
    Public Queue As List

    ''' <summary>
    ''' Кассир
    ''' </summary>
    ''' <remarks></remarks>
    Friend Cash As Cashman

    ''' <summary>
    ''' Генератор клиентов
    ''' </summary>
    ''' <remarks></remarks>
    Friend Generator As ClientGenerator

    ''' <summary>
    ''' Генератор случайных чисел, управляющий работой кассира
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared RandCashman As Simulation.Random

    ''' <summary>
    ''' Генератор случайных чисел, управляющий прибытием клиентов
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared RandClient As Simulation.Random

    ''' <summary>
    ''' основной алгоритм работы имитации
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Execute()
        Generator.Activate()
        Passivate()
    End Sub

    ''' <summary>
    ''' Создание инфраструктуры имитации
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Init()
        MyBase.Init()
        Cash = New Cashman
        Generator = New ClientGenerator
        Queue = New List("Очередь ожидания")
        InBankStat = New Statistics("Время нахождения клиентов в системе")
        InBankHist = New Histogram(Params.HistMin, Params.HistStep, Params.HistStepCount, _
                                   "Время нахождения клиентов в системе")
        CashStat = New ServiceStatistics("Занятость кассира")
        NotWaited = 0
    End Sub

    ''' <summary>
    ''' Завершение работы имитации
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub Finish()
        Queue.Finish()
        Cash.Finish()
        Generator.Finish()
        MyBase.Finish()
    End Sub

    ''' <summary>
    ''' Коррекция статистики к текущему времени
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub StopStat()
        MyBase.StopStat()
        CashStat.StopStat()
        Queue.StopStat()
    End Sub
End Class
