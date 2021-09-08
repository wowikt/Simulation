unit UTVControlVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, ComCtrls;

type
  TfrTVControl = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    lbInspectionQueue: TLabel;
    lbAdjustmentQueue: TLabel;
    lbInspection: TLabel;
    lbAdjustment: TLabel;
    lbInspectionBack: TLabel;
    lbInspectionUsage: TLabel;
    lbAdjustmentBack: TLabel;
    lbAdjustmentUsage: TLabel;
    lbCompleted: TLabel;
    lbTimeInSystem: TLabel;
    lbSimTime: TLabel;
    tmTVControl: TTimer;
    Label8: TLabel;
    edSimulationTime: TEdit;
    btStart: TButton;
    pcStat: TPageControl;
    tsStat: TTabSheet;
    sgTime: TStringGrid;
    sgService: TStringGrid;
    sgQueue: TStringGrid;
    procedure btStartClick(Sender: TObject);
    procedure tmTVControlTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frTVControl: TfrTVControl;

implementation
uses UTVControl, USimulation;

{$R *.dfm}

var
  tvc : TTVControl = nil;
  srv : array [0 .. 1] of TServiceStatistics;
  que : array [0 .. 2] of TList;
  stat : array [0 .. 1] of TStatistics;

procedure TfrTVControl.btStartClick(Sender: TObject);
begin
  if tmTVControl.Enabled then
  begin
    tmTVControl.Enabled := False;
  end
  else
  begin
    tvc.Free;
    tmTVControl.Enabled := True;
    SimulationTime := StrToFloat(edSimulationTime.Text);
    tvc := TTVControl.Create;
    srv[0] := tvc.InspectorsStat;
    srv[1] := tvc.AdjustmentStat;
    que[0] := tvc.InspectionQueue;
    que[1] := tvc.AdjustmentQueue;
    que[2] := tvc.Calendar;
    stat[0] := tvc.TimeInSystemStat;
    stat[1] := tvc.AdjustmentCountStat;
    SwitchTo(tvc);
  end;
end;

procedure TfrTVControl.tmTVControlTimer(Sender: TObject);
begin
  tvc.StopStat;
  lbInspectionQueue.Caption := Chars(tvc.InspectionQueue.Size, '*');
  lbAdjustmentQueue.Caption := Chars(tvc.AdjustmentQueue.Size, '*');
  if tvc.InspectorsStat.Running > 0 then
    lbInspection.Caption := '(' + Chars(tvc.InspectorsStat.Running, '*') + ')'
  else
    lbInspection.Caption := '';
  if tvc.AdjustmentStat.Running > 0 then
    lbAdjustment.Caption := '(*)'
  else
    lbAdjustment.Caption := '';
  lbSimTime.Caption := Format('%.1f', [tvc.SimTime]);
  lbTimeInSystem.Caption := Format('%.3f', [tvc.TimeInSystemStat.Mean]);
  lbCompleted.Caption := IntToStr(tvc.TimeInSystemStat.Count);
  lbInspectionUsage.Width :=
      Round(tvc.InspectorsStat.Mean * lbInspectionBack.Width /
      tvc.InspectorsStat.Devices);
  lbAdjustmentUsage.Width :=
      Round(tvc.AdjustmentStat.Mean * lbAdjustmentBack.Width /
      tvc.AdjustmentStat.Devices);
  ShowStat(sgTime, ['Время в системе', 'Число настроек'], stat);
  ShowStat(sgService, ['Проверка', 'Настройка'], srv);
  ShowStat(sgQueue,
      ['Очередь на проверку', 'Очередь на настройку', 'Календарь'], que);
  if tvc.Terminated then
    tmTVControl.Enabled := False
  else
    RunSimulation(tvc);
end;

procedure TfrTVControl.FormCreate(Sender: TObject);
begin
  rndTVSet := TRandom.Create;
  rndInspector := TRandom.Create;
  rndAdjuster := TRandom.Create;
  sgQueue.ColWidths[0] := 170;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgService.ColWidths[0] := 90;
  sgService.ColWidths[1] := 95;
  sgService.ColWidths[2] := 70;
  sgService.ColWidths[3] := 100;
  sgService.ColWidths[4] := 60;
  sgService.ColWidths[5] := 145;
  sgService.ColWidths[6] := 115;
  sgService.ColWidths[7] := 105;
  sgService.ColWidths[8] := 90;
  sgTime.ColWidths[0] := 130;
  sgTime.ColWidths[1] := 70;
  sgTime.ColWidths[2] := 100;
  sgTime.ColWidths[3] := 75;
  sgTime.ColWidths[4] := 80;
  sgTime.ColWidths[5] := 95;
end;

end.
