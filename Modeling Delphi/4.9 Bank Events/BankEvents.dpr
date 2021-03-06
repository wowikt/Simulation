program BankEvents;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UBank in 'UBank.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  bnk : TBank;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndClient := TRandom.Create;
  bnk := TBank.Create;
  SwitchTo(bnk);
  with bnk do
  begin
    WriteStat('Time between balks statistics:', BalksStat);
    WriteLn;
    WriteStat('Time between departures statistics:', DepartStat);
    WriteLn;
    for i := 0 to CashCount - 1 do
    begin
      WriteStat('Cash ' + IntToStr(i) + ' statistics:', CashmenStat[i]);
      WriteLn;
    end;
    WriteStat('Clients in bank count statistics:', ClientStat);
    WriteLn;
    WriteStat('Clients in bank time statistics:', TimeStat);
    WriteLn;
    for i := 0 to CashCount - 1 do
    begin
      WriteStat('Queue ' + IntToStr(i) + ' statistics:', Queue[i]);
      WriteLn;
    end;
    WriteStat('Calendar statistics:', Calendar);
    WriteLn;
    WriteLn('Balked ', (BalksStat.Count + 1) / IncomeCount * 100 : 5 : 3,
        '% of clients');
    WriteLn;
    WriteLn('Jerk count = ', JerkCount);
  end;
  bnk.Free;
  ReadLn;
end.
