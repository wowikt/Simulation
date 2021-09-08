program Savings;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  USavings in 'USavings.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  svg : TSavings;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndCustomer := TRandom.Create;
  svg := TSavings.Create;
  SwitchTo(svg);
  with svg do
  begin
    WriteLn('Simulation stopped at ', SimTime : 6 : 2);
    WriteLn('Statistics last cleared at ', LastCleared : 6 : 2);
    WriteLn;
    WriteStat('Lost sales statistics:', LostSalesStat);
    WriteLn;
    WriteStat('Safety stock statistics:', SafetyStockStat);
    WriteLn;
    WriteStat('Inventory position statistics:', InvPosStat);
    WriteLn;
    WriteStat('Radio resource statistics:', Radios);
    WriteLn;
    WriteStat('Order waiting statistics:', Radios.Queue[0]);
    WriteLn;
    WriteStat('Calendar statistics:', Calendar);
  end;
  ReadLn;
end.
