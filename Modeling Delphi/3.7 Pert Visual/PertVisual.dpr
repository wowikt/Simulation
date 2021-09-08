program PertVisual;

uses
  Forms,
  UPertVisual in 'UPertVisual.pas' {frPert},
  UPert in 'UPert.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrPert, frPert);
  Application.Run;
end.
