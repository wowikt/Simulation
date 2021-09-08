program LoadingVis;

uses
  Forms,
  ULoadingVis in 'ULoadingVis.pas' {frLoadingVis},
  ULoading in 'ULoading.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrLoadingVis, frLoadingVis);
  Application.Run;
end.
