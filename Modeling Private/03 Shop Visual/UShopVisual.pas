unit UShopVisual;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls;

type
  TfrShopVisual = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    lbShopping: TLabel;
    lbQueue: TLabel;
    lbCash: TLabel;
    lbServiced: TLabel;
    lbTimeInSystem: TLabel;
    lbSimTime: TLabel;
    Label7: TLabel;
    edSimulationTime: TEdit;
    btStart: TButton;
    tmShop: TTimer;
    pgStat: TPageControl;
    tsStat: TTabSheet;
    sgCash: TStringGrid;
    sgTime: TStringGrid;
    sgShopping: TStringGrid;
    sgQueue: TStringGrid;
    procedure btStartClick(Sender: TObject);
    procedure tmShopTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frShopVisual: TfrShopVisual;

implementation
uses UShop, USimulation;

{$R *.dfm}

var
  shp : TShop = nil;
  que : array [0 .. 1] of TList;

procedure TfrShopVisual.btStartClick(Sender: TObject);
begin
  if tmShop.Enabled then
  begin
    tmShop.Enabled := False;
  end
  else
  begin
    shp.Free;
    SimulationTime := StrToFloat(edSimulationTime.Text);
    shp := TShop.Create;
    que[0] := shp.Queue;
    que[1] := shp.Calendar;
    tmShop.Enabled := True;
  end;
end;

procedure TfrShopVisual.tmShopTimer(Sender: TObject);
begin
  shp.StopStat;
  lbSimTime.Caption := Format('%.1f', [shp.SimTime]);
  lbTimeInSystem.Caption := Format('%.3f', [shp.TimeStat.Mean]);
  lbShopping.Caption := Chars(shp.PeopleStat.Running, '*');
  lbQueue.Caption := Chars(shp.Queue.Size, '*');
  ShowStat(sgQueue, ['Очередь', 'Календарь'], que);
  ShowStat(sgShopping, ['Торговый зал'], shp.PeopleStat);
  ShowStat(sgTime, ['Время в системе'], shp.TimeStat);
  ShowStat(sgCash, ['Касса'], shp.CashStat);
  if shp.CashStat.Running > 0 then
    lbCash.Caption := '(*)'
  else
    lbCash.Caption := '';
  lbServiced.Caption := IntToStr(shp.TimeStat.Count);
  if shp.Terminated then
  begin
    tmShop.Enabled := False;
  end
  else
  begin
    RunSimulation(shp);
  end;
end;

procedure TfrShopVisual.FormCreate(Sender: TObject);
begin
  rndCust := TRandom.Create;
  rndService := TRandom.Create;
  sgQueue.ColWidths[0] := 90;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgCash.ColWidths[0] := 90;
  sgCash.ColWidths[1] := 95;
  sgCash.ColWidths[2] := 70;
  sgCash.ColWidths[3] := 100;
  sgCash.ColWidths[4] := 60;
  sgCash.ColWidths[5] := 145;
  sgCash.ColWidths[6] := 115;
  sgCash.ColWidths[7] := 105;
  sgCash.ColWidths[8] := 90;
  sgTime.ColWidths[0] := 130;
  sgTime.ColWidths[1] := 70;
  sgTime.ColWidths[2] := 100;
  sgTime.ColWidths[3] := 75;
  sgTime.ColWidths[4] := 80;
  sgTime.ColWidths[5] := 95;
  sgShopping.ColWidths[0] := 110;
  sgShopping.ColWidths[1] := 70;
  sgShopping.ColWidths[2] := 100;
  sgShopping.ColWidths[3] := 80;
  sgShopping.ColWidths[4] := 60;
  sgShopping.ColWidths[5] := 90;
end;

end.
