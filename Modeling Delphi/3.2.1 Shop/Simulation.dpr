program Simulation;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UShop in 'UShop.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  Shop : TShop;
begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndCust := TRandom.Create;
  rndService := TRandom.Create;

  // Создать симуляцию
  Shop := TShop.Create;
  // Запуск
  SwitchTo(Shop);
  with Shop do
  begin
    WriteStat('Cash statistics:', CashStat);
    WriteLn;
    WriteStat('In system time statistics:', TimeStat);
    WriteLn;
    WriteStat('People in shop statistics:', PeopleStat);
    WriteLn;
    WriteStat('Customers queue statistics:', Queue);
    WriteLn;
    WriteStat('Calendar statistics:', Calendar);
  end;
  Shop.Free;
  ReadLn;
end.
