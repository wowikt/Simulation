program VConstrolVis;

uses
  Forms,
  UTVControlVis in 'UTVControlVis.pas' {frTVControl},
  UTVControl in 'UTVControl.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrTVControl, frTVControl);
  Application.Run;
end.
