program ToolsVis;

uses
  Forms,
  UMachine in 'UMachine.pas' {frMachine},
  UTools in 'UTools.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrMachine, frMachine);
  Application.Run;
end.
