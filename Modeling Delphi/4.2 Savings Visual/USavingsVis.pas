unit USavingsVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls, XPMan;

type
  TfrSavings = class(TForm)
    Label1: TLabel;
    lbQueue: TLabel;
    shAvailableBack: TShape;
    Label2: TLabel;
    Label3: TLabel;
    shRadioCountBack: TShape;
    shAvailable: TShape;
    shRadioCount: TShape;
    Label4: TLabel;
    lbTotalCount: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    edPreSimulation: TEdit;
    edMainSimulation: TEdit;
    btStart: TButton;
    tmSavings: TTimer;
    Label7: TLabel;
    lbSimTime: TLabel;
    Label8: TLabel;
    lbNoBuys: TLabel;
    pgStat: TPageControl;
    tsStat1: TTabSheet;
    tsStat2: TTabSheet;
    sgLostSales: TStringGrid;
    sgSafetyStock: TStringGrid;
    sgInvPosition: TStringGrid;
    sgRadio: TStringGrid;
    sgQueue: TStringGrid;
    XPManifest1: TXPManifest;
    procedure btStartClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure tmSavingsTimer(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frSavings: TfrSavings;

implementation
uses USavings, USimulation;

{$R *.dfm}

var
  svg : TSavings = nil;
  Queues : array [0 .. 1] of TList;
  Stats : array [0 .. 1] of TStatistics;

procedure TfrSavings.btStartClick(Sender: TObject);
begin
  if tmSavings.Enabled then
    tmSavings.Enabled := False
  else
  begin
    svg.Free;
    svg := TSavings.Create;
    Queues[0] := svg.Radios.Queue[0];
    Queues[1] := svg.Calendar;
    Stats[0] := svg.SafetyStockStat;
    Stats[1] := svg.WaitStat;
    tmSavings.Enabled := True;
  end;
end;

procedure TfrSavings.FormCreate(Sender: TObject);
begin
  rndCustomer := TRandom.Create;
  rndOrder := TRandom.Create;
  sgQueue.ColWidths[0] := 150;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgSafetyStock.ColWidths[0] := 130;
  sgSafetyStock.ColWidths[1] := 70;
  sgSafetyStock.ColWidths[2] := 100;
  sgSafetyStock.ColWidths[3] := 75;
  sgSafetyStock.ColWidths[4] := 80;
  sgSafetyStock.ColWidths[5] := 95;
  sgLostSales.ColWidths[0] := 150;
  sgLostSales.ColWidths[1] := 70;
  sgLostSales.ColWidths[2] := 100;
  sgLostSales.ColWidths[3] := 75;
  sgLostSales.ColWidths[4] := 80;
  sgLostSales.ColWidths[5] := 95;
  sgInvPosition.ColWidths[0] := 160;
  sgInvPosition.ColWidths[1] := 70;
  sgInvPosition.ColWidths[2] := 100;
  sgInvPosition.ColWidths[3] := 75;
  sgInvPosition.ColWidths[4] := 80;
  sgInvPosition.ColWidths[5] := 80;
  sgInvPosition.ColWidths[6] := 65;
  sgRadio.ColWidths[0] := 170;
  sgRadio.ColWidths[1] := 85;
  sgRadio.ColWidths[2] := 90;
  sgRadio.ColWidths[3] := 100;
  sgRadio.ColWidths[4] := 110;
  sgRadio.ColWidths[5] := 120;
  sgRadio.ColWidths[6] := 100;
  sgRadio.ColWidths[7] := 75;
  sgRadio.ColWidths[8] := 85;
  sgRadio.ColWidths[9] := 90;
end;

procedure TfrSavings.tmSavingsTimer(Sender: TObject);
begin
  svg.StopStat;
  lbSimTime.Caption := Format('%.1f', [svg.SimTime]);
  if svg.CurrentAvailable > 0 then
  begin
    shAvailable.Width := Round(svg.CurrentAvailable *
        shAvailableBack.Width / InvPosition);
    shAvailable.Left := shAvailableBack.Left;
    if svg.CurrentAvailable <= OrderLimit then
      shAvailable.Brush.Color := clYellow
    else
      shAvailable.Brush.Color := clLime;
  end
  else
  begin
    shAvailable.Width := Round(-svg.CurrentAvailable *
        shAvailableBack.Width / InvPosition);
    shAvailable.Left := shAvailableBack.Left - shAvailable.Width;
    shAvailable.Brush.Color := clRed;
  end;
  shRadioCount.Width := Round(svg.Radios.Available *
      shRadioCountBack.Width / InvPosition);
  lbQueue.Caption := Chars(svg.Radios.Queue[0].Size, '*');
  lbTotalCount.Caption := IntToStr(svg.BuysCount);
  lbNoBuys.Caption := IntToStr(svg.LostSalesStat.Count + 1);
  ShowStat(sgInvPosition, ['??????? ??????????'], svg.InvPosStat);
  ShowStat(sgLostSales, ['?????? ?? ???????'], svg.LostSalesStat);
  ShowStat(sgSafetyStock, ['????????? ?????', '???????? ??????'], Stats);
  ShowStat(sgQueue, ['??????? ????????', '?????????'], Queues);
  ShowStat(sgRadio, ['?????????? ?? ??????'], svg.Radios);
  if svg.Terminated then
    tmSavings.Enabled := False
  else
    RunSimulation(svg);
end;

end.
