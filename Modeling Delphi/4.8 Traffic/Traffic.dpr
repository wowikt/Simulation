program Traffic;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UTraffic in 'UTraffic.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  trf : TTraffic;
begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndCar := TRandom.Create;
  trf := TTraffic.Create;
  SwitchTo(trf);
  with trf do
  begin
    WriteStat('Wait time left to right statistics:', WaitStat[dirLeftRight]);
    WriteLn;
    WriteStat('Wait time right to left statistics:', WaitStat[dirRightLeft]);
    WriteLn;
    WriteStat('Gate left to right statistics:', LightGate[dirLeftRight]);
    WriteLn;
    WriteStat('Gate right to left statistics:', LightGate[dirRightLeft]);
    WriteLn;
    WriteStat('Resource left to right statistics:', LightRes[dirLeftRight]);
    WriteLn;
    WriteStat('Resource right to left statistics:', LightRes[dirRightLeft]);
    WriteLn;
    WriteStat('Gate left to right queue statistics:',
        LightGate[dirLeftRight].Queue);
    WriteLn;
    WriteStat('Gate right to left queue statistics:',
        LightGate[dirRightLeft].Queue);
    WriteLn;
    WriteStat('Resource left to right queue statistics:',
        LightRes[dirLeftRight].Queue[0]);
    WriteLn;
    WriteStat('Resource right to left queue statistics:',
        LightRes[dirRightLeft].Queue[0]);
    WriteLn;
    WriteStat('Calendar statistics:', Calendar);
  end;
  ReadLn;
end.
