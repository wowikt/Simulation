program Pert;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UPert in 'UPert.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  prt : TPert;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndArc := TRandom.Create;
  for i := 1 to NodeCount - 1 do
    NodeStat[i] := TStatistics.Create;
  for i := 1 to RunCount do
  begin
    prt := TPert.Create;
    SwitchTo(prt);
    prt.Free;
  end;
  for i := 1 to NodeCount - 1 do
  begin
    WriteStat('Node ' + IntToStr(i) + ' statistics:', NodeStat[i]);
    WriteLn;
    NodeStat[i].Free;
  end;
  ReadLn;
end.
