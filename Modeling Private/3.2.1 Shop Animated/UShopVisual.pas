unit UShopVisual;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls, UShop, USimulation;

type
  TfrShopVisual = class(TForm)
    Label6: TLabel;
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
    tsGraph: TTabSheet;
    imAnimation: TImage;
    tsModel: TTabSheet;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    lbShopping: TLabel;
    lbQueue: TLabel;
    lbCash: TLabel;
    lbServiced: TLabel;
    lbTimeInSystem: TLabel;
    procedure btStartClick(Sender: TObject);
    procedure tmShopTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
    procedure ShowAnimation;
    procedure DrawCustomer(cust : TCustomer);
    procedure ClearCustomer(cust : TCustomer);
    procedure DrawCash;
  public
    { Public declarations }
  end;

var
  frShopVisual: TfrShopVisual;

implementation

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
    MinX := 0;
    MinY := 0;
    MaxX := imAnimation.Width - SpriteX;
    MaxY := imAnimation.Height;
    StartX := MinX;
    StartY := rndCust.NextInt(MinY, imAnimation.Height - SpriteY);
    CashX := imAnimation.Width - SpriteX;
    CashY := rndCust.NextInt(MinY, imAnimation.Height - SpriteY);
    SimulationTime := StrToFloat(edSimulationTime.Text);
    shp := TShop.Create;
    que[0] := shp.Queue;
    que[1] := shp.Calendar;
    tmShop.Enabled := True;
    with imAnimation.Canvas do
    begin
      Brush.Color := clWhite;
      Pen.Style := psClear;
      Rectangle(0, 0, Width, Height);
      Pen.Style := psSolid;
      Brush.Color := Color;
    end;
  end;
end;

procedure TfrShopVisual.tmShopTimer(Sender: TObject);
begin
  shp.StopStat;
  lbSimTime.Caption := TimeToStr(shp.SimTime / 1440 + 1 / 3);
  lbTimeInSystem.Caption := Format('%.3f', [shp.TimeStat.Mean]);
  lbShopping.Caption := Chars(shp.PeopleStat.Running, '*');
  lbQueue.Caption := Chars(shp.Queue.Size, '*');
  ShowStat(sgQueue, ['Очередь', 'Календарь'], que);
  ShowStat(sgShopping, ['Торговый зал'], shp.PeopleStat);
  ShowStat(sgTime, ['Время в системе'], shp.TimeStat);
  ShowStat(sgCash, ['Касса'], shp.CashStat);
  ShowAnimation;
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
  MinX := 0;
  MinY := 0;
  MaxX := imAnimation.Width - SpriteX;
  MaxY := imAnimation.Height;
  StartX := MinX;
  StartY := rndCust.NextInt(MinY, imAnimation.Height - SpriteY);
  CashX := imAnimation.Width - SpriteX;
  CashY := rndCust.NextInt(MinY, imAnimation.Height - SpriteY);
end;

procedure TfrShopVisual.ClearCustomer(cust: TCustomer);
begin
  with imAnimation.Canvas do
  begin
    Pen.Style := psClear;
    Brush.Color := clWhite;
    Rectangle(Round(cust.X), Round(cust.Y), Round(cust.X + SpriteX) + 1,
        Round(cust.Y + SpriteY) + 1);
    Brush.Color := Color;
    Pen.Style := psSolid;
  end;
end;

procedure TfrShopVisual.DrawCustomer(cust: TCustomer);
begin
  with imAnimation.Canvas do
  begin
    Brush.Color := cust.Color;
    Rectangle(Round(cust.X), Round(cust.Y), Round(cust.X + SpriteX),
        Round(cust.Y + SpriteY));
    Brush.Color := cust.BuysColor;
    Pen.Color := not (cust.Color xor cust.BuysColor) and $00FFFFFF;
    Pen.Width := 2;
    Rectangle(Round(cust.X + BuysLeft), Round(cust.Y + BuysBottom -
        cust.CurrentBuys * BuysMaxHeight / MaxBuysCount),
        Round(cust.X + BuysRight), Round(cust.Y + BuysBottom));
    Pen.Color := clBlack;
    Pen.Width := 1;
    Brush.Color := Color;
  end;
end;

procedure TfrShopVisual.ShowAnimation;
var
  cust : TCustomer;
  TargetX, TargetY : Integer;
  dx, dy : Double;
begin
  cust := shp.InShop.First as TCustomer;
  while cust <> nil do
  begin
    ClearCustomer(cust);
    cust := cust.Next as TCustomer;
  end;
  cust := shp.Queue.First as TCustomer;
  while cust <> nil do
  begin
    ClearCustomer(cust);
    cust := cust.Next as TCustomer;
  end;
  if shp.CashCustomer <> nil then
    ClearCustomer(shp.CashCustomer);
  TargetX := CashX - (shp.Queue.Size + 1) * SpriteStep;
  TargetY := CashY;
  cust := shp.InShop.First as TCustomer;
  while cust <> nil do
  begin
    dx := (TargetX - cust.X) * VisTimeStep / (cust.EventTime - shp.SimTime);
    dy := (TargetY - cust.Y) * VisTimeStep / (cust.EventTime - shp.SimTime);
    dx := rndCust.Normal(dx, DeviationX + Abs(dx));
    dy := rndCust.Normal(dy, DeviationY + Abs(dy));
    dx := Max(cust.dx - MaxDDX, Min(cust.dx + MaxDDX, (cust.dx * 3 + dx) / 4));
    dy := Max(cust.dy - MaxDDY, Min(cust.dy + MaxDDY, (cust.dy * 3 + dy) / 4));
    cust.X := Max(MinX, Min(MaxX - SpriteX, cust.X + dx));
    cust.Y := Max(MinY, Min(MaxY - SpriteY, cust.Y + dy));
    cust.dx := dx;
    cust.dy := dy;
    cust.CurrentBuys := cust.BuysCount * (shp.SimTime - cust.StartingTime) /
        (cust.EventTime - cust.StartingTime);
    DrawCustomer(cust);
    cust := cust.Next as TCustomer;
  end;
  TargetX := CashX - SpriteStep;
  cust := shp.Queue.First as TCustomer;
  while cust <> nil do
  begin
    cust.X := TargetX;
    cust.Y := TargetY;
    TargetX := TargetX - SpriteStep;
    DrawCustomer(cust);
    cust := cust.Next as TCustomer;
  end;
  DrawCash;
  if shp.CashCustomer <> nil then
  begin
    shp.CashCustomer.X := CashX;
    shp.CashCustomer.Y := CashY;
    shp.CashCustomer.CurrentBuys := shp.CashCustomer.BuysCount *
        (shp.Cash.EventTime - shp.SimTime) / (shp.Cash.EventTime - shp.CashStart);
    DrawCustomer(shp.CashCustomer);
  end;
end;

procedure TfrShopVisual.DrawCash;
begin
  with imAnimation.Canvas do
  begin
    Brush.Color := clFuchsia;
    Rectangle(CashX - 2, CashY - 2, CashX + SpriteX + 2, CashY + SpriteY + 2);
    Brush.Color := Color;
  end;
end;

end.
