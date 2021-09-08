program TrafficVisual;

uses
  Forms,
  UTrafficVisual in 'UTrafficVisual.pas' {frTrafficVisual},
  UTraffic in 'UTraffic.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrTrafficVisual, frTrafficVisual);
  Application.Run;
end.
