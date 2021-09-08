program Tools;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UTools in 'UTools.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  tls : TTools;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndDetail := TRandom.Create;
  rndFaults := TRandom.Create;
  tls := TTools.Create;
  SwitchTo(tls);
  with tls do
  begin
    WriteStat('Time in system statistics:', TimeStat);
    WriteLn;
    WriteStat('Preparation time statistics:', PrepTimeStat);
    WriteLn;
    WriteStat('Operation time statistics:', OperTimeStat);
    WriteLn;
    WriteStat('Repair time statistics:', RepairTimeStat);
    WriteLn;
    WriteStat('Preparation statitics:', PrepStat);
    WriteLn;
    WriteStat('Operation statitics:', OperStat);
    WriteLn;
    WriteStat('Repair statitics:', RepairStat);
    WriteLn;
    WriteStat('Tool resource statitics:', Tool);
    WriteLn;
    WriteStat('Repair queue statistics:', Tool.Queue[0]);
    WriteLn;
    WriteStat('Details queue statistics:', Tool.Queue[1]);
    WriteLn;
    WriteStat('Calendar statistics:', Calendar);
  end;
  tls.Free;
  ReadLn;
end.
