program FlowLineVis;

uses
  Forms,
  UFlowLineVis in 'UFlowLineVis.pas' {frFlowLineVis},
  UFlowLine in 'UFlowLine.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrFlowLineVis, frFlowLineVis);
  Application.Run;
end.
