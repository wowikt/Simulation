unit UBankVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls;

type
  TfrBank = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    lbQueue: TLabel;
    lbCashman: TLabel;
    lbServiced: TLabel;
    lbSimTime: TLabel;
    btStart: TButton;
    tmBank: TTimer;
    Label5: TLabel;
    edClientCount: TEdit;
    pgStats: TPageControl;
    shStats: TTabSheet;
    shHistogram: TTabSheet;
    sgTimeStat: TStringGrid;
    sgCashStat: TStringGrid;
    sgQueueStat: TStringGrid;
    dgHistogram: TDrawGrid;
    lbCashBack: TLabel;
    lbCashBusy: TLabel;
    Label6: TLabel;
    lbNoWait: TLabel;
    Label7: TLabel;
    edQueueLength: TEdit;
    Label8: TLabel;
    lbNotServiced: TLabel;
    procedure btStartClick(Sender: TObject);
    procedure tmBankTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure dgHistogramDrawCell(Sender: TObject; ACol, ARow: Integer;
      Rect: TRect; State: TGridDrawState);
    procedure dgHistogramTopLeftChanged(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frBank: TfrBank;

implementation
uses UBank, USimulation;

{$R *.dfm}

var
  bnk : TBankSimulation;
  Queue : array [0 .. 1] of TList;
  LastFinished : Integer = 0;

procedure TfrBank.btStartClick(Sender: TObject);
begin
  if tmBank.Enabled then
  begin
    tmBank.Enabled := False;
  end
  else
  begin
    MaxClientCount := StrToInt(edClientCount.Text);
    MaxQueueLength := StrToInt(edQueueLength.Text);
    tmBank.Enabled := True;
    bnk.Free;
    bnk := TBankSimulation.Create;
    Queue[0] := bnk.Queue;
    Queue[1] := bnk.Calendar;
    LastFinished := 0;
    dgHistogram.RowCount := bnk.InBankHist.IntervalCount + 3;
    SwitchTo(bnk);
  end;
end;

procedure TfrBank.tmBankTimer(Sender: TObject);
begin
  bnk.StopStat;
  lbQueue.Caption := Chars(bnk.Queue.Size, '*');
  if bnk.CashStat.Running = 0 then
    lbCashman.Caption := ''
  else
    lbCashman.Caption := '(' + Chars(bnk.CashStat.Running, '*') + ')';
  lbSimTime.Caption := Format('%1.0f', [bnk.SimTime]);
  lbServiced.Caption := IntToStr(bnk.InBankTime.Count);
  lbNotServiced.Caption := IntToStr(bnk.NotServiced);
  lbNoWait.Caption := IntToStr(bnk.NotWaited);
  ShowStat(sgCashStat, ['Касса'], bnk.CashStat);
  ShowStat(sgQueueStat, ['Очередь', 'Календарь'], Queue);
  ShowStat(sgTimeStat, ['Время в банке'], bnk.InBankTime);
  if bnk.InBankHist.TotalCount > LastFinished then
  begin
    LastFinished := bnk.InBankHist.TotalCount;
    dgHistogram.Repaint;
  end;
  lbCashBusy.Width :=
      Round(lbCashBack.Width * bnk.CashStat.Mean / bnk.CashStat.Devices);
  if bnk.Terminated then
  begin
    tmBank.Enabled := False;
  end
  else
    RunSimulation(bnk);
end;

procedure TfrBank.FormCreate(Sender: TObject);
begin
  rndClient := TRandom.Create;
  rndCashman := TRandom.Create;
  sgQueueStat.ColWidths[0] := 90;
  sgQueueStat.ColWidths[1] := 70;
  sgQueueStat.ColWidths[2] := 100;
  sgQueueStat.ColWidths[3] := 80;
  sgQueueStat.ColWidths[4] := 60;
  sgQueueStat.ColWidths[5] := 80;
  sgCashStat.ColWidths[0] := 90;
  sgCashStat.ColWidths[1] := 95;
  sgCashStat.ColWidths[2] := 70;
  sgCashStat.ColWidths[3] := 100;
  sgCashStat.ColWidths[4] := 60;
  sgCashStat.ColWidths[5] := 145;
  sgCashStat.ColWidths[6] := 115;
  sgCashStat.ColWidths[7] := 105;
  sgCashStat.ColWidths[8] := 90;
  sgTimeStat.ColWidths[0] := 130;
  sgTimeStat.ColWidths[1] := 70;
  sgTimeStat.ColWidths[2] := 100;
  sgTimeStat.ColWidths[3] := 75;
  sgTimeStat.ColWidths[4] := 80;
  sgTimeStat.ColWidths[5] := 95;
  dgHistogram.ColWidths[0] := 50;
  dgHistogram.ColWidths[1] := 50;
  dgHistogram.ColWidths[2] := 80;
  dgHistogram.ColWidths[3] := 75;
  dgHistogram.ColWidths[4] := 100;
  dgHistogram.ColWidths[5] := 600;
end;

procedure TfrBank.dgHistogramDrawCell(Sender: TObject; ACol, ARow: Integer;
  Rect: TRect; State: TGridDrawState);
begin
  if bnk <> nil then
    DrawHistCell(dgHistogram, ACol, ARow, Rect, bnk.InBankHist);
end;

procedure TfrBank.dgHistogramTopLeftChanged(Sender: TObject);
begin
  if bnk <> nil then
    dgHistogram.Repaint;
end;

end.
