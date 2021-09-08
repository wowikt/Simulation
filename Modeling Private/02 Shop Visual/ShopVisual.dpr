program ShopVisual;

uses
  Forms,
  UShopVisual in 'UShopVisual.pas' {frShopVisual},
  UShop in 'UShop.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrShopVisual, frShopVisual);
  Application.Run;
end.
