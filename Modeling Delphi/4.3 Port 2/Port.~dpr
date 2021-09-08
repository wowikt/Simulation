program Port;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UPort in 'UPort.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  prt : TPort;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndStorm := TRandom.Create;
  rndTanker := TRandom.Create;
  prt := TPort.Create;
  SwitchTo(prt);
  with prt do
  begin
    for i := 0 to TypeCount - 1 do
    begin
      WriteStat('Type ' + IntToStr(i) + ' in port time:',
          TimeStat[i]);
      WriteLn;
    end;
    WriteStat('Berth resource:', Berth);
    WriteLn;
    WriteStat('Tug resource:', Tug);
    WriteLn;
    WriteStat('Berth queue:', Berth.Queue[0]);
    WriteLn;
    WriteStat('Tug for arrival queue:', Tug.Queue[0]);
    WriteLn;
    WriteStat('Tug for depart queue:', Tug.Queue[1]);
    WriteLn;
    WriteStat('Calendar:', Calendar);
  end;
  ReadLn;
end.
