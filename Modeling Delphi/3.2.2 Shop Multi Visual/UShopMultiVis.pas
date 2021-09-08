unit UShopMultiVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, Grids, ComCtrls, StdCtrls;

type
  TfrShopMultiVis = class(TForm)
    Label1: TLabel;
    edSimulationTime: TEdit;
    Label2: TLabel;
    edRunsCount: TEdit;
    btStart: TButton;
    pgStat: TPageControl;
    tsStat: TTabSheet;
    tsHist: TTabSheet;
    sgStat: TStringGrid;
    dgHistogram: TDrawGrid;
    tmShop: TTimer;
    Label3: TLabel;
    lbRuns: TLabel;
    procedure btStartClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure tmShopTimer(Sender: TObject);
    procedure dgHistogramDrawCell(Sender: TObject; ACol, ARow: Integer;
      Rect: TRect; State: TGridDrawState);
    procedure dgHistogramTopLeftChanged(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frShopMultiVis: TfrShopMultiVis;

implementation
uses UShop, USimulation;

{$R *.dfm}
var
  Shop : TShop = nil;
  CashUsageStat : TStatistics;
  TimeStat : TStatistics;
  InShopStat : TStatistics;
  InShopMaxStat : TStatistics;
  MaxQueueLenStat : TStatistics;
  WaitStat : TStatistics;
  TimeHist : TUniformHistogram;
  MaxRunsCount : Integer = 400;
  HistMin : Double = 10;
  HistStep : Double = 1;
  HistStepCount : Integer = 20;
  RunNum : Integer;
  Stat : array [0 .. 5] of TStatistics;

procedure TfrShopMultiVis.btStartClick(Sender: TObject);
begin
  if tmShop.Enabled then
  begin
    tmShop.Enabled := False;
  end
  else
  begin
    CashUsageStat.Clear;
    TimeStat.Clear;
    InShopStat.Clear;
    InShopMaxStat.Clear;
    MaxQueueLenStat.Clear;
    WaitStat.Clear;
    TimeHist.Clear;
    SimulationTime := StrToFloat(edSimulationTime.Text);
    MaxRunsCount := StrToInt(edRunsCount.Text);
    RunNum := 0;
    dgHistogram.RowCount := TimeHist.IntervalCount + 3;
    tmShop.Enabled := True;
  end;
end;

procedure TfrShopMultiVis.FormCreate(Sender: TObject);
begin
  rndCust := TRandom.Create;
  rndService := TRandom.Create;
  CashUsageStat := TStatistics.Create;
  TimeStat := TStatistics.Create;
  InShopStat := TStatistics.Create;
  InShopMaxStat := TStatistics.Create;
  MaxQueueLenStat := TStatistics.Create;
  WaitStat := TStatistics.Create;
  Stat[0] := CashUsageStat;
  Stat[1] := TimeStat;
  Stat[2] := InShopStat;
  Stat[3] := InShopMaxStat;
  Stat[4] := MaxQueueLenStat;
  Stat[5] := WaitStat;
  TimeHist := TUniformHistogram.Create(HistMin, HistStep, HistStepCount);
  sgStat.ColWidths[0] := 380;
  sgStat.ColWidths[1] := 70;
  sgStat.ColWidths[2] := 100;
  sgStat.ColWidths[3] := 75;
  sgStat.ColWidths[4] := 80;
  sgStat.ColWidths[5] := 95;
  dgHistogram.ColWidths[0] := 50;
  dgHistogram.ColWidths[1] := 50;
  dgHistogram.ColWidths[2] := 80;
  dgHistogram.ColWidths[3] := 75;
  dgHistogram.ColWidths[4] := 100;
  dgHistogram.ColWidths[5] := 600;
end;

procedure TfrShopMultiVis.tmShopTimer(Sender: TObject);
begin
  // Создать симуляцию
  Shop := TShop.Create;
  // Запуск
  SwitchTo(Shop);
  // Сбор статистики
  CashUsageStat.AddData(Shop.CashStat.Mean);
  TimeStat.AddData(Shop.TimeStat.Mean);
  InShopStat.AddData(Shop.PeopleStat.Mean);
  InShopMaxStat.AddData(Shop.PeopleStat.Max);
  MaxQueueLenStat.AddData(Shop.Queue.LengthStat.Max);
  WaitStat.AddData(Shop.Queue.WaitStat.Mean);
  TimeHist.AddData(Shop.TimeStat.Mean);
  Shop.Free;
  Inc(RunNum);
  lbRuns.Caption := IntToStr(RunNum);
  ShowStat(sgStat,
      ['Загруженность кассира', 'Среднее время в системе',
      'Среднее число покупателей в торговом зале',
      'Максимальное число покупателей в торговом зале',
      'Максимальная длина очереди', 'Среднее время ожидания в очереди'], Stat);
  dgHistogram.Repaint;
  if RunNum = MaxRunsCount then
    tmShop.Enabled := False;
end;

procedure TfrShopMultiVis.dgHistogramDrawCell(Sender: TObject; ACol,
  ARow: Integer; Rect: TRect; State: TGridDrawState);
begin
  DrawHistCell(dgHistogram, ACol, ARow, Rect, TimeHist);
end;

procedure TfrShopMultiVis.dgHistogramTopLeftChanged(Sender: TObject);
begin
  dgHistogram.Repaint;
end;

end.
