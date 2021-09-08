program Queens;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UQueen in 'UQueen.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  Sim : TQueensSimulation;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  Sim := TQueensSimulation.Create;
  SwitchTo(Sim);
  Sim.Free;
  WriteLn('Done');
  ReadLn;
end.
