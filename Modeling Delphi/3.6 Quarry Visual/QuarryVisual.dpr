program QuarryVisual;

uses
  Forms,
  UQuarryVis in 'UQuarryVis.pas' {frQuarryVis},
  UQuarry in 'UQuarry.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrQuarryVis, frQuarryVis);
  Application.Run;
end.
