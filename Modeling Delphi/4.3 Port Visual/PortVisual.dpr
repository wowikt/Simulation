program PortVisual;

uses
  Forms,
  UPortVisual in 'UPortVisual.pas' {frPortVisual},
  UPort in 'UPort.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrPortVisual, frPortVisual);
  Application.Run;
end.
