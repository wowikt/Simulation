program ShopMultiVis;

uses
  Forms,
  UShopMultiVis in 'UShopMultiVis.pas' {frShopMultiVis},
  UShop in 'UShop.pas',
  USimulation in '..\USimulation\USimulation.pas';

{$R *.res}

begin
  Application.Initialize;
  Application.CreateForm(TfrShopMultiVis, frShopMultiVis);
  Application.Run;
end.
