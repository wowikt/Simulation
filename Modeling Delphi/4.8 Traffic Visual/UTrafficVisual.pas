unit UTrafficVisual;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, ExtCtrls, StdCtrls, Grids, ComCtrls;

type
  TfrTrafficVisual = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    lbQueue1: TLabel;
    lbQueue2: TLabel;
    Label7: TLabel;
    edTime1: TEdit;
    edTime2: TEdit;
    btStart: TButton;
    tmSim: TTimer;
    Label3: TLabel;
    lbSimTime: TLabel;
    Label4: TLabel;
    lbWait1: TLabel;
    lbWait2: TLabel;
    shLightLeft: TShape;
    shLightRight: TShape;
    pgStat: TPageControl;
    tsStat: TTabSheet;
    tsQueue: TTabSheet;
    sgWaitStat: TStringGrid;
    sgGate: TStringGrid;
    sgResource: TStringGrid;
    sgQueue: TStringGrid;
    procedure FormCreate(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure tmSimTimer(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frTrafficVisual: TfrTrafficVisual;

implementation
uses UTraffic, USimulation;

{$R *.dfm}

var
  trf : TTraffic;
  Que : array [0 .. 4] of TList;
  
procedure TfrTrafficVisual.FormCreate(Sender: TObject);
begin
  rndCar := TRandom.Create;
  sgQueue.ColWidths[0] := 100;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgResource.ColWidths[0] := 100;
  sgResource.ColWidths[1] := 85;
  sgResource.ColWidths[2] := 90;
  sgResource.ColWidths[3] := 100;
  sgResource.ColWidths[4] := 110;
  sgResource.ColWidths[5] := 120;
  sgResource.ColWidths[6] := 100;
  sgResource.ColWidths[7] := 75;
  sgResource.ColWidths[8] := 85;
  sgResource.ColWidths[9] := 90;
  sgWaitStat.ColWidths[0] := 130;
  sgWaitStat.ColWidths[1] := 70;
  sgWaitStat.ColWidths[2] := 100;
  sgWaitStat.ColWidths[3] := 75;
  sgWaitStat.ColWidths[4] := 80;
  sgWaitStat.ColWidths[5] := 95;
  sgGate.ColWidths[0] := 125;
  sgGate.ColWidths[1] := 70;
  sgGate.ColWidths[2] := 65;
end;

procedure TfrTrafficVisual.btStartClick(Sender: TObject);
begin
  if tmSim.Enabled then
  begin
    btStart.Caption := 'Пуск';
    tmSim.Enabled := False;
  end
  else
  begin
    tmSim.Enabled := True;
    btStart.Caption := 'Стоп';
    trf.Free;
    OpenTime[dirLeftRight] := StrToInt(edTime1.Text);
    OpenTime[dirRightLeft] := StrToInt(edTime2.Text);
    trf := TTraffic.Create;
    Que[0] := trf.LightRes[dirLeftRight].Queue[0];
    Que[1] := trf.LightRes[dirRightLeft].Queue[0];
    Que[2] := trf.LightGate[dirLeftRight].Queue;
    Que[3] := trf.LightGate[dirRightLeft].Queue;
    Que[4] := trf.Calendar;
    SwitchTo(trf);
  end;
end;

procedure TfrTrafficVisual.tmSimTimer(Sender: TObject);
begin
  trf.StopStat;
  lbSimTime.Caption := FloatToStr(trf.SimTime);
  // Длина каждой очереди является суммой длин очередей к ресурсу и затвору
  lbQueue1.Caption := Chars(trf.LightRes[dirLeftRight].Queue[0].Size +
      trf.LightGate[dirLeftRight].Queue.Size, '*');
  lbQueue2.Caption := Chars(trf.LightRes[dirRightLeft].Queue[0].Size +
      trf.LightGate[dirRightLeft].Queue.Size, '*');
  // Отобразить состояние затвора посредством изменения цвета фигур
  if trf.LightGate[dirLeftRight].State then
    shLightLeft.Brush.Color := clGreen
  else
    shLightLeft.Brush.Color := clRed;
  if trf.LightGate[dirRightLeft].State then
    shLightRight.Brush.Color := clGreen
  else
    shLightRight.Brush.Color := clRed;
  ShowStat(sgWaitStat, ['Ожидание ==>', 'Ожидание <=='], trf.WaitStat);
  ShowStat(sgGate, ['Светофор ==>', 'Светофор <=='], trf.LightGate);
  ShowStat(sgResource, ['Ресурс ==>', 'Ресурс <=='], trf.LightRes);
  ShowStat(sgQueue, ['Ресурс ==>', 'Ресурс <==', 'Затвор ==>', 'Затвор <==',
      'Календарь'], Que);
  lbWait1.Caption := Format('%6.2f', [trf.WaitStat[dirLeftRight].Mean]);
  lbWait2.Caption := Format('%6.2f', [trf.WaitStat[dirRightLeft].Mean]);
  if trf.Terminated then
    tmSim.Enabled := False
  else
    RunSimulation(trf);
end;

end.
