unit UPertVisual;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, Grids, ComCtrls, StdCtrls;

type
  TfrPert = class(TForm)
    Label1: TLabel;
    edRunCount: TEdit;
    btStart: TButton;
    pgStat: TPageControl;
    tsStat: TTabSheet;
    tsHist1: TTabSheet;
    tsHist2: TTabSheet;
    tsHist3: TTabSheet;
    tsHist4: TTabSheet;
    tsHist5: TTabSheet;
    sgStat: TStringGrid;
    dgHist1: TDrawGrid;
    dgHist2: TDrawGrid;
    dgHist3: TDrawGrid;
    dgHist4: TDrawGrid;
    dgHist5: TDrawGrid;
    procedure FormCreate(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure dgHist1DrawCell(Sender: TObject; ACol, ARow: Integer;
      Rect: TRect; State: TGridDrawState);
    procedure dgHist1TopLeftChanged(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frPert: TfrPert;

implementation
uses UPert, USimulation;

{$R *.dfm}

var
  Stat : array of TStatistics;
  StatHeaders : array of string;

procedure TfrPert.FormCreate(Sender: TObject);
var
  i : Integer;
begin
  rndArc := TRandom.Create;
  SetLength(Stat, NodeCount - 1);
  SetLength(StatHeaders, NodeCount - 1);
  for i := 1 to NodeCount - 1 do
  begin
    NodeStat[i] := TStatistics.Create;
    NodeHist[i] :=
        TUniformHistogram.Create(HistMin[i], HistStep[i], HistStepCount[i]);
    Stat[i - 1] := NodeStat[i];
    StatHeaders[i - 1] := 'Узел ' + IntToStr(i);
  end;
  sgStat.ColWidths[0] := 130;
  sgStat.ColWidths[1] := 70;
  sgStat.ColWidths[2] := 100;
  sgStat.ColWidths[3] := 75;
  sgStat.ColWidths[4] := 80;
  sgStat.ColWidths[5] := 95;
  dgHist1.ColWidths[0] := 50;
  dgHist1.ColWidths[1] := 50;
  dgHist1.ColWidths[2] := 80;
  dgHist1.ColWidths[3] := 75;
  dgHist1.ColWidths[4] := 100;
  dgHist1.ColWidths[5] := 600;
  dgHist2.ColWidths[0] := 50;
  dgHist2.ColWidths[1] := 50;
  dgHist2.ColWidths[2] := 80;
  dgHist2.ColWidths[3] := 75;
  dgHist2.ColWidths[4] := 100;
  dgHist2.ColWidths[5] := 600;
  dgHist3.ColWidths[0] := 50;
  dgHist3.ColWidths[1] := 50;
  dgHist3.ColWidths[2] := 80;
  dgHist3.ColWidths[3] := 75;
  dgHist3.ColWidths[4] := 100;
  dgHist3.ColWidths[5] := 600;
  dgHist4.ColWidths[0] := 50;
  dgHist4.ColWidths[1] := 50;
  dgHist4.ColWidths[2] := 80;
  dgHist4.ColWidths[3] := 75;
  dgHist4.ColWidths[4] := 100;
  dgHist4.ColWidths[5] := 600;
  dgHist5.ColWidths[0] := 50;
  dgHist5.ColWidths[1] := 50;
  dgHist5.ColWidths[2] := 80;
  dgHist5.ColWidths[3] := 75;
  dgHist5.ColWidths[4] := 100;
  dgHist5.ColWidths[5] := 600;
  dgHist1.RowCount := HistStepCount[1] + 3;
  dgHist2.RowCount := HistStepCount[2] + 3;
  dgHist3.RowCount := HistStepCount[3] + 3;
  dgHist4.RowCount := HistStepCount[4] + 3;
  dgHist5.RowCount := HistStepCount[5] + 3;
end;

procedure TfrPert.btStartClick(Sender: TObject);
var
  i : Integer;
  prt : TPert;
begin
  RunCount := StrToInt(edRunCount.Text);
  for i := 1 to NodeCount - 1 do
  begin
    NodeStat[i].Clear;
    NodeHist[i].Clear;
  end;
  for i := 1 to RunCount do
  begin
    prt := TPert.Create;
    SwitchTo(prt);
    prt.Free;
  end;
  ShowStat(sgStat, StatHeaders, Stat);
  dgHist1.Repaint;
  dgHist2.Repaint;
  dgHist3.Repaint;
  dgHist4.Repaint;
  dgHist5.Repaint;
end;

procedure TfrPert.dgHist1DrawCell(Sender: TObject; ACol, ARow: Integer;
  Rect: TRect; State: TGridDrawState);
begin
  DrawHistCell(Sender as TDrawGrid, ACol, ARow, Rect,
      NodeHist[(Sender as TDrawGrid).Tag]);
end;

procedure TfrPert.dgHist1TopLeftChanged(Sender: TObject);
begin
  (Sender as TDrawGrid).Repaint;
end;

end.
