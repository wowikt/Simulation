program BankVis;

uses
  Forms,
  UBankVis in 'UBankVis.pas' {frBank},
  UBank in 'UBank.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrBank, frBank);
  Application.Run;
end.
