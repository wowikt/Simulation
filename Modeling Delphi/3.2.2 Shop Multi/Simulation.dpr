program Simulation;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UShop in 'UShop.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  Shop : TShop;
  CashUsageStat : TStatistics;
  TimeStat : TStatistics;
  InShopStat : TStatistics;
  InShopMaxStat : TStatistics;
  MaxQueueLenStat : TStatistics;
  WaitStat : TStatistics;
  TimeHist : TUniformHistogram;
  RunsCount : Integer = 400;
  HistMin : Double = 8;
  HistStep : Double = 1;
  HistStepCount : Integer = 20;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndCust := TRandom.Create;
  rndService := TRandom.Create;
  CashUsageStat := TStatistics.Create;
  TimeStat := TStatistics.Create;
  InShopStat := TStatistics.Create;
  InShopMaxStat := TStatistics.Create;
  MaxQueueLenStat := TStatistics.Create;
  WaitStat := TStatistics.Create;
  TimeHist := TUniformHistogram.Create(HistMin, HistStep, HistStepCount);

  for i := 1 to RunsCount do
  begin
    // ??????? ????????
    Shop := TShop.Create;
    // ??????
    SwitchTo(Shop);
    // ???? ??????????
    CashUsageStat.AddData(Shop.CashStat.Mean);
    TimeStat.AddData(Shop.TimeStat.Mean);
    InShopStat.AddData(Shop.PeopleStat.Mean);
    InShopMaxStat.AddData(Shop.PeopleStat.Max);
    MaxQueueLenStat.AddData(Shop.Queue.LengthStat.Max);
    WaitStat.AddData(Shop.Queue.WaitStat.Mean);
    TimeHist.AddData(Shop.TimeStat.Mean);

    Shop.Free;
    if i mod 10 = 0 then
      Write('.');
  end;
  WriteLn;
  WriteStat('Cash usage statistics:', CashUsageStat);
  WriteLn;
  WriteStat('In system time statistics:', TimeStat);
  WriteLn;
  WriteStat('People in shop statistics:', InShopStat);
  WriteLn;
  WriteStat('Max people in shop statistics:', InShopMaxStat);
  WriteLn;
  WriteStat('Max queue length statistics:', MaxQueueLenStat);
  WriteLn;
  WriteStat('Waiting time statistics:', WaitStat);
  WriteLn;
  WriteHist('Mean in system time histogram:', TimeHist);
  ReadLn;
end.
