unit UMachine;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls;

type
  TfrMachine = class(TForm)
    lbQueue: TLabel;
    Label2: TLabel;
    lbState: TLabel;
    Label4: TLabel;
    lbCompleted: TLabel;
    Label6: TLabel;
    lbTime: TLabel;
    btStart: TButton;
    tmTools: TTimer;
    Label1: TLabel;
    Label3: TLabel;
    edSimulationTime: TEdit;
    pgStat: TPageControl;
    tsTime: TTabSheet;
    sgTime: TStringGrid;
    sgResource: TStringGrid;
    tsAction: TTabSheet;
    sgAction: TStringGrid;
    sgQueue: TStringGrid;
    procedure btStartClick(Sender: TObject);
    procedure tmToolsTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frMachine: TfrMachine;

implementation
uses UTools, USimulation;

{$R *.dfm}

var
  tls : TTools;
  TimeStat : array [0 .. 3] of TStatistics;
  ActionStat : array [0 .. 2] of TActionStatistics;
  QueueStat : array [0 .. 2] of TList;

procedure TfrMachine.btStartClick(Sender: TObject);
begin
  if tmTools.Enabled then
    tmTools.Enabled := False
  else
  begin
    tmTools.Enabled := True;
    SimulationTime := StrToFloat(edSimulationTime.Text);
    tls.Free;
    tls := TTools.Create;
    TimeStat[0] := tls.TimeStat;
    TimeStat[1] := tls.PrepTimeStat;
    TimeStat[2] := tls.OperTimeStat;
    TimeStat[3] := tls.RepairTimeStat;
    ActionStat[0] := tls.PrepStat;
    ActionStat[1] := tls.OperStat;
    ActionStat[2] := tls.RepairStat;
    QueueStat[0] := tls.Tool.Queue[0];
    QueueStat[1] := tls.Tool.Queue[1];
    QueueStat[2] := tls.Calendar;
    SwitchTo(tls);
  end;
end;

procedure TfrMachine.tmToolsTimer(Sender: TObject);
begin
  tls.StopStat;
  lbCompleted.Caption := IntToStr(tls.TimeStat.Count);
  // Отображение очереди с обозначением приостановленного задания
  if tls.Tool.PreemptedProcs(DetailQueueIndex) > 0 then
    lbQueue.Caption :=
        Chars(tls.Tool.Queue[DetailQueueIndex].Size - 1, '*') + 'o'
  else
    lbQueue.Caption := Chars(tls.Tool.Queue[DetailQueueIndex].Size, '*');
  // Отображение состояния станка
  case tls.State of
  msFree :
    lbState.Caption := '';
  msPreparing:
    lbState.Caption := '(.)';
  msWorking :
    lbState.Caption := '(*)';
  msRepaired:
    lbState.Caption := '[X]';
  end;
  lbTime.Caption := Format('%5.1f', [tls.SimTime]);
  ShowStat(sgTime, ['Время в системе', 'Время наладки',
      'Время основного действия', 'Время ремонт'], TimeStat);
  ShowStat(sgAction, ['Наладка', 'Основное действие', 'Ремонт'], ActionStat);
  ShowStat(sgResource, ['Станок'], tls.Tool);
  ShowStat(sgQueue, ['Очередь ремонта', 'Очередь заданий', 'Календарь'],
      QueueStat);
  if tls.Terminated then
  begin
    tmTools.Enabled := False;
  end
  else
    RunSimulation(tls);
end;

procedure TfrMachine.FormCreate(Sender: TObject);
begin
  rndDetail := TRandom.Create;
  rndFaults := TRandom.Create;
  sgQueue.ColWidths[0] := 140;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgResource.ColWidths[0] := 60;
  sgResource.ColWidths[1] := 85;
  sgResource.ColWidths[2] := 90;
  sgResource.ColWidths[3] := 100;
  sgResource.ColWidths[4] := 110;
  sgResource.ColWidths[5] := 120;
  sgResource.ColWidths[6] := 100;
  sgResource.ColWidths[7] := 75;
  sgResource.ColWidths[8] := 85;
  sgResource.ColWidths[9] := 90;
  sgTime.ColWidths[0] := 210;
  sgTime.ColWidths[1] := 70;
  sgTime.ColWidths[2] := 100;
  sgTime.ColWidths[3] := 75;
  sgTime.ColWidths[4] := 80;
  sgTime.ColWidths[5] := 95;
  sgAction.ColWidths[0] := 150;
  sgAction.ColWidths[1] := 70;
  sgAction.ColWidths[2] := 100;
  sgAction.ColWidths[3] := 80;
  sgAction.ColWidths[4] := 60;
  sgAction.ColWidths[5] := 90;
end;

end.
