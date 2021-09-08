program ConveyorVisual;

uses
  Forms,
  UConveyorVis in 'UConveyorVis.pas' {frConveyor},
  UConveyor in 'UConveyor.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrConveyor, frConveyor);
  Application.Run;
end.
