unit UConveyorVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, ComCtrls;

type
  TfrConveyor = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    lbDev0: TLabel;
    lbDev1: TLabel;
    lbDev2: TLabel;
    lbDev3: TLabel;
    lbDev4: TLabel;
    lbQue0: TLabel;
    lbQue1: TLabel;
    lbQue2: TLabel;
    lbQue3: TLabel;
    lbQue4: TLabel;
    lbCompl0: TLabel;
    lbCompl1: TLabel;
    lbCompl2: TLabel;
    lbCompl3: TLabel;
    lbCompl4: TLabel;
    tmConveyor: TTimer;
    btStart: TButton;
    Label6: TLabel;
    lbCompleted: TLabel;
    Label8: TLabel;
    lbTimeInSystem: TLabel;
    Label7: TLabel;
    lbTime: TLabel;
    shBack0: TShape;
    shBusy0: TShape;
    shBack1: TShape;
    shBusy1: TShape;
    shBack2: TShape;
    shBusy2: TShape;
    shBack3: TShape;
    shBusy3: TShape;
    shBack4: TShape;
    shBusy4: TShape;
    pgStat: TPageControl;
    tsDevStat: TTabSheet;
    tsStat: TTabSheet;
    sgDevices: TStringGrid;
    sgAction: TStringGrid;
    sgTime: TStringGrid;
    sgQueue: TStringGrid;
    procedure btStartClick(Sender: TObject);
    procedure tmConveyorTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frConveyor: TfrConveyor;

implementation
uses UConveyor, USimulation;


{$R *.dfm}

var
  cnv : TConveyor = nil;
  ActionStat : array [0 .. 1] of TActionStatistics;

procedure TfrConveyor.btStartClick(Sender: TObject);
begin
  if tmConveyor.Enabled then
    tmConveyor.Enabled := False
  else
  begin
    tmConveyor.Enabled := True;
    cnv.Free;
    cnv := TConveyor.Create;
    ActionStat[0] := cnv.ActStat;
    ActionStat[1] := cnv.ConvStat;
    SwitchTo(cnv);
  end;
end;

procedure TfrConveyor.tmConveyorTimer(Sender: TObject);
begin
  with cnv do
  begin
    StopStat;
    lbCompl0.Caption := IntToStr(Completed[0]);
    lbCompl1.Caption := IntToStr(Completed[1]);
    lbCompl2.Caption := IntToStr(Completed[2]);
    lbCompl3.Caption := IntToStr(Completed[3]);
    lbCompl4.Caption := IntToStr(Completed[4]);
    if MachRes[0].Available = 0 then
      lbDev0.Caption := '(*)'
    else
      lbDev0.Caption := '';
    if MachRes[1].Available = 0 then
      lbDev1.Caption := '(*)'
    else
      lbDev1.Caption := '';
    if MachRes[2].Available = 0 then
      lbDev2.Caption := '(*)'
    else
      lbDev2.Caption := '';
    if MachRes[3].Available = 0 then
      lbDev3.Caption := '(*)'
    else
      lbDev3.Caption := '';
    if MachRes[4].Available = 0 then
      lbDev4.Caption := '(*)'
    else
      lbDev4.Caption := '';
    lbQue0.Caption := Chars(ConvCount[0], '*');
    lbQue1.Caption := Chars(ConvCount[1], '*');
    lbQue2.Caption := Chars(ConvCount[2], '*');
    lbQue3.Caption := Chars(ConvCount[3], '*');
    lbQue4.Caption := Chars(ConvCount[4], '*');
    lbCompleted.Caption := IntToStr(ActStat.Finished);
    lbTimeInSystem.Caption := Format('%5.2f', [TimeStat.Mean]);
    lbTime.Caption := Format('%.0f', [SimTime]);
    shBusy0.Width := Trunc(MachRes[0].BusyStat.Mean * shBack0.Width);
    shBusy1.Width := Trunc(MachRes[1].BusyStat.Mean * shBack1.Width);
    shBusy2.Width := Trunc(MachRes[2].BusyStat.Mean * shBack2.Width);
    shBusy3.Width := Trunc(MachRes[3].BusyStat.Mean * shBack3.Width);
    shBusy4.Width := Trunc(MachRes[4].BusyStat.Mean * shBack3.Width);
    ShowStat(sgDevices, ['Устройство 0', 'Устройство 1', 'Устройство 2',
        'Устройство 3', 'Устройство 4'], MachRes);
    ShowStat(sgQueue, ['Календарь'], Calendar);
    ShowStat(sgTime, ['Время в системе'], TimeStat);
    ShowStat(sgAction, ['Загруженность устройств', 'Детали на конвейере'],
        ActionStat);
  end;
  if cnv.Terminated then
    tmConveyor.Enabled := False
  else
    RunSimulation(cnv);
end;

procedure TfrConveyor.FormCreate(Sender: TObject);
begin
  rndDetail := TRandom.Create;
  sgQueue.ColWidths[0] := 90;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgDevices.ColWidths[0] := 110;
  sgDevices.ColWidths[1] := 85;
  sgDevices.ColWidths[2] := 90;
  sgDevices.ColWidths[3] := 100;
  sgDevices.ColWidths[4] := 110;
  sgDevices.ColWidths[5] := 120;
  sgDevices.ColWidths[6] := 100;
  sgDevices.ColWidths[7] := 75;
  sgDevices.ColWidths[8] := 85;
  sgDevices.ColWidths[9] := 90;
  sgTime.ColWidths[0] := 130;
  sgTime.ColWidths[1] := 70;
  sgTime.ColWidths[2] := 100;
  sgTime.ColWidths[3] := 75;
  sgTime.ColWidths[4] := 80;
  sgTime.ColWidths[5] := 95;
  sgAction.ColWidths[0] := 200;
  sgAction.ColWidths[1] := 70;
  sgAction.ColWidths[2] := 100;
  sgAction.ColWidths[3] := 80;
  sgAction.ColWidths[4] := 60;
  sgAction.ColWidths[5] := 90;
end;

end.

