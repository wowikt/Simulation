program SavingsVisual;

uses
  Forms,
  USavingsVis in 'USavingsVis.pas' {frSavings},
  USavings in 'USavings.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrSavings, frSavings);
  Application.Run;
end.
