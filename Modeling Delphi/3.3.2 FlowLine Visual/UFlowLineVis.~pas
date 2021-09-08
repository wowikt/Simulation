unit UFlowLineVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls;

type
  TfrFlowLineVis = class(TForm)
    Timer1: TTimer;
    lbLen1: TLabel;
    lbLen2: TLabel;
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    lbBusy1: TLabel;
    lbBusy2: TLabel;
    Label5: TLabel;
    lbServiced: TLabel;
    Label6: TLabel;
    lbBalked: TLabel;
    Label7: TLabel;
    lbTime: TLabel;
    Label9: TLabel;
    edQueue1Size: TEdit;
    btStart: TButton;
    pgStat: TPageControl;
    tsStat: TTabSheet;
    tsHistogram: TTabSheet;
    sgStat: TStringGrid;
    sgBalks: TStringGrid;
    sgWorkers: TStringGrid;
    sgQueues: TStringGrid;
    dgHistogram: TDrawGrid;
    shWorker2Back: TShape;
    shBusyStat2: TShape;
    shWorker1Back: TShape;
    shBusyStat1: TShape;
    shBlockStat: TShape;
    procedure FormCreate(Sender: TObject);
    procedure Timer1Timer(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure dgHistogramDrawCell(Sender: TObject; ACol, ARow: Integer;
      Rect: TRect; State: TGridDrawState);
    procedure dgHistogramTopLeftChanged(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frFlowLineVis: TfrFlowLineVis;

implementation
uses UFlowLine, USimulation;

{$R *.dfm}

var
  sim : TFlowLineSimulation;
  WrkStat : array [0 .. 1] of TServiceStatistics;
  QueStat : array [0 .. 2] of TList;
  LastServiced : Integer = 0;

procedure TfrFlowLineVis.FormCreate(Sender: TObject);
begin
  rndPiece := TRandom.Create;
  rndWorker1 := TRandom.Create;
  rndWorker2 := TRandom.Create;
  sgQueues.ColWidths[0] := 90;
  sgQueues.ColWidths[1] := 70;
  sgQueues.ColWidths[2] := 100;
  sgQueues.ColWidths[3] := 80;
  sgQueues.ColWidths[4] := 60;
  sgQueues.ColWidths[5] := 80;
  sgWorkers.ColWidths[0] := 90;
  sgWorkers.ColWidths[1] := 95;
  sgWorkers.ColWidths[2] := 70;
  sgWorkers.ColWidths[3] := 100;
  sgWorkers.ColWidths[4] := 60;
  sgWorkers.ColWidths[5] := 145;
  sgWorkers.ColWidths[6] := 115;
  sgWorkers.ColWidths[7] := 105;
  sgWorkers.ColWidths[8] := 90;
  sgBalks.ColWidths[0] := 130;
  sgBalks.ColWidths[1] := 70;
  sgBalks.ColWidths[2] := 100;
  sgBalks.ColWidths[3] := 75;
  sgBalks.ColWidths[4] := 80;
  sgBalks.ColWidths[5] := 95;
  sgStat.ColWidths[0] := 130;
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

procedure TfrFlowLineVis.Timer1Timer(Sender: TObject);
begin
  sim.Queue1.StopStat;
  sim.Queue2.StopStat;
  sim.Stat1.StopStat(sim.SimTime);
  sim.Stat2.StopStat(sim.SimTime);
  lbLen1.Caption := Chars(sim.Queue1.Size, '*');
  lbLen2.Caption := Chars(sim.Queue2.Size, '*');
  if sim.Stat1.Running > 0 then
    lbBusy1.Caption := '(*)'
  else if sim.Stat1.Blocked > 0 then
    lbBusy1.Caption := ' *||'
  else
    lbBusy1.Caption := '';
  if sim.Worker2.Idle then
    lbBusy2.Caption := ''
  else
    lbBusy2.Caption := '(*)';
  shBusyStat1.Width := Round(sim.Stat1.Mean * shWorker1Back.Width);
  shBusyStat2.Width := Round(sim.Stat2.Mean * shWorker2Back.Width);
  shBlockStat.Left := shBusyStat1.Left + shBusyStat1.Width;
  shBlockStat.Width := Round(sim.Stat1.MeanBlockage * shWorker1Back.Width);
  lbServiced.Caption := IntToStr(sim.TimeInSystem.Count);
  lbBalked.Caption := IntToStr(sim.Balks.Count + 1);
  lbTime.Caption := Format('%5.1f', [sim.SimTime]);
  ShowStat(sgStat, ['Время в системе'], sim.TimeInSystem);
  ShowStat(sgBalks, ['Между отказами'], sim.Balks);
  ShowStat(sgWorkers, ['Рабочий 1', 'Рабочий 2'], WrkStat);
  ShowStat(sgQueues, ['Очередь 1', 'Очередь 2', 'Календарь'], QueStat);
  if sim.TimeInSystem.Count > LastServiced then
  begin
    dgHistogram.Repaint;
    LastServiced := sim.TimeInSystem.Count;
  end;
  if not sim.Terminated then
    RunSimulation(sim)
  else
  begin
    Timer1.Enabled := False;
    sim.Free;
  end;
end;

procedure TfrFlowLineVis.btStartClick(Sender: TObject);
begin
  if Timer1.Enabled then
  begin
    btStart.Caption := 'Пуск';
    Timer1.Enabled := False;
    sim.Free;
  end
  else
  begin
    Queue1Size := StrToInt(edQueue1Size.Text);
    if Queue1Size < 0 then
      Queue1Size := 0
    else if Queue1Size > 6 then
      Queue1Size := 6;
    Queue2Size := 6 - Queue1Size;
    Timer1.Enabled := True;
    btStart.Caption := 'Стоп';
    sim := TFlowLineSimulation.Create;
    WrkStat[0] := sim.Stat1;
    WrkStat[1] := sim.Stat2;
    QueStat[0] := sim.Queue1;
    QueStat[1] := sim.Queue2;
    QueStat[2] := sim.Calendar;
    dgHistogram.RowCount := sim.TimeHist.IntervalCount + 3;
    SwitchTo(sim);
  end;
end;

procedure TfrFlowLineVis.dgHistogramDrawCell(Sender: TObject; ACol, ARow: Integer;
  Rect: TRect; State: TGridDrawState);
begin
  if sim <> nil then
    DrawHistCell(dgHistogram, ACol, ARow, Rect, sim.TimeHist);
end;

procedure TfrFlowLineVis.dgHistogramTopLeftChanged(Sender: TObject);
begin
  if sim <> nil then
    dgHistogram.Repaint;
end;

end.
