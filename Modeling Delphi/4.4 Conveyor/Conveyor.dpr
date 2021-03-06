program Conveyor;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UConveyor in 'UConveyor.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  cnv : TConveyor;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndDetail := TRandom.Create;
  cnv := TConveyor.Create;
  SwitchTo(cnv);
  with cnv do
  begin
    for i := 0 to MachineCount - 1 do
    begin
      WriteStat('Machine ' + IntToStr(i) + ' statistics:', MachRes[i]);
      WriteLn;
    end;
    WriteStat('Time in system: ', TimeStat);
    WriteLn;
    WriteStat('Activity statistics:', ActStat);
    WriteLn;
    WriteStat('Details on conveyor statistics:', ConvStat);
    WriteLn;
    WriteStat('Calendar statistics:', Calendar);
  end;
  ReadLn;
end.
