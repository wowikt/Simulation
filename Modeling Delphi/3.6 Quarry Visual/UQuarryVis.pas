unit UQuarryVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, ComCtrls;

type
  TfrQuarryVis = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    lbExcavatorQueue0: TLabel;
    lbExcavatorQueue1: TLabel;
    lbExcavatorQueue2: TLabel;
    lbExcavator0: TLabel;
    lbExcavator1: TLabel;
    lbExcavator2: TLabel;
    lbMillQueue: TLabel;
    lbMill: TLabel;
    lbBackTrip: TLabel;
    Label7: TLabel;
    edSimulationTime: TEdit;
    btStart: TButton;
    tmQuarry: TTimer;
    Label8: TLabel;
    lbSimTime: TLabel;
    lbExcavator0Completed: TLabel;
    lbExcavator2Completed: TLabel;
    lbExcavator1Completed: TLabel;
    shExcavator0Back: TShape;
    shExcavator0: TShape;
    shExcavator1Back: TShape;
    shExcavator1: TShape;
    shExcavator2Back: TShape;
    shExcavator2: TShape;
    lbMillCompleted: TLabel;
    shMillBack: TShape;
    shMill: TShape;
    lbForwardTrip0: TLabel;
    lbForwardTrip1: TLabel;
    lbForwardTrip2: TLabel;
    pgStat: TPageControl;
    tsActions: TTabSheet;
    tsQueues: TTabSheet;
    sgService: TStringGrid;
    sgAction: TStringGrid;
    sgQueue: TStringGrid;
    Label9: TLabel;
    Label10: TLabel;
    lbHeavyCount: TLabel;
    lbLightCount: TLabel;
    Label11: TLabel;
    lbDelivered: TLabel;
    procedure FormCreate(Sender: TObject);
    procedure btStartClick(Sender: TObject);
    procedure tmQuarryTimer(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frQuarryVis: TfrQuarryVis;

implementation
uses UQuarry, USimulation;

{$R *.dfm}

var
  qry : TQuarry = nil;
  Services : array of TServiceStatistics;
  Queues : array of TList;

procedure TfrQuarryVis.FormCreate(Sender: TObject);
begin
  rndExcavator := TRandom.Create;
  rndMill := TRandom.Create;
  sgQueue.ColWidths[0] := 120;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgService.ColWidths[0] := 120;
  sgService.ColWidths[1] := 95;
  sgService.ColWidths[2] := 70;
  sgService.ColWidths[3] := 100;
  sgService.ColWidths[4] := 60;
  sgService.ColWidths[5] := 145;
  sgService.ColWidths[6] := 115;
  sgService.ColWidths[7] := 105;
  sgService.ColWidths[8] := 90;
  sgAction.ColWidths[0] := 90;
  sgAction.ColWidths[1] := 70;
  sgAction.ColWidths[2] := 100;
  sgAction.ColWidths[3] := 80;
  sgAction.ColWidths[4] := 60;
  sgAction.ColWidths[5] := 90;
end;

procedure TfrQuarryVis.btStartClick(Sender: TObject);
begin
  if tmQuarry.Enabled then
    tmQuarry.Enabled := False
  else
  begin
    SimulationTime := StrToFloat(edSimulationTime.Text);
    qry.Free;
    qry := TQuarry.Create;
    SetLength(Services, 4);
    Services[0] := qry.ExcavatorStat[0];
    Services[1] := qry.ExcavatorStat[1];
    Services[2] := qry.ExcavatorStat[2];
    Services[3] := qry.MillStat;
    SetLength(Queues, 5);
    Queues[0] := qry.ExcavatorQueue[0];
    Queues[1] := qry.ExcavatorQueue[1];
    Queues[2] := qry.ExcavatorQueue[2];
    Queues[3] := qry.MillQueue;
    Queues[4] := qry.Calendar;
    tmQuarry.Enabled := True;
    SwitchTo(qry);
  end;
end;

// ??????????? ??????? ?????????? ? ?????????????? ?????? ????????
//   ??? ?????????? ??????? ????
function TruckChars(Queue : TList; HeavyChar, LightChar : Char) : string;
var
  Res : string;
  trk : TTruck;
begin
  Res := '';
  trk := Queue.First as TTruck;
  // ??????? ?????????? ? ???????
  while trk <> nil do
  begin
    // ???????? ? ?????? ????????? ??????
    if trk.Tonnage > 20 then
      Res := HeavyChar + Res
    else
      Res := LightChar + Res;
    trk := trk.Next as TTruck;
  end;
  Result := Res;
end;

procedure TfrQuarryVis.tmQuarryTimer(Sender: TObject);
begin
  qry.StopStat;
  lbSimTime.Caption := Format('%.1f', [qry.SimTime]);
  lbExcavatorQueue0.Caption := TruckChars(qry.ExcavatorQueue[0], 'O', 'o');
  lbExcavatorQueue1.Caption := TruckChars(qry.ExcavatorQueue[1], 'O', 'o');
  lbExcavatorQueue2.Caption := TruckChars(qry.ExcavatorQueue[2], 'O', 'o');
  if qry.ExcavatorStat[0].Running > 0 then
    lbExcavator0.Caption := '(*)'
  else
    lbExcavator0.Caption := '';
  if qry.ExcavatorStat[1].Running > 0 then
    lbExcavator1.Caption := '(*)'
  else
    lbExcavator1.Caption := '';
  if qry.ExcavatorStat[2].Running > 0 then
    lbExcavator2.Caption := '(*)'
  else
    lbExcavator2.Caption := '';
  lbMillQueue.Caption := TruckChars(qry.MillQueue, 'O', 'o');
  if qry.MillStat.Running > 0 then
    lbMill.Caption := '(*)'
  else
    lbMill.Caption := '';
  lbBackTrip.Caption := Chars(qry.ReturnStat.Running, '*');
  lbExcavator0Completed.Caption := IntToStr(qry.ExcavatorStat[0].Finished);
  lbExcavator1Completed.Caption := IntToStr(qry.ExcavatorStat[1].Finished);
  lbExcavator2Completed.Caption := IntToStr(qry.ExcavatorStat[2].Finished);
  shExcavator0.Width :=
      Round(qry.ExcavatorStat[0].Mean * shExcavator0Back.Width);
  shExcavator1.Width :=
      Round(qry.ExcavatorStat[1].Mean * shExcavator1Back.Width);
  shExcavator2.Width :=
      Round(qry.ExcavatorStat[2].Mean * shExcavator2Back.Width);
  lbMillCompleted.Caption := IntToStr(qry.MillStat.Finished);
  shMill.Width := Round(qry.MillStat.Mean * shMillBack.Width);
  lbForwardTrip0.Caption := Chars(qry.ForwardTrip[0], '*');
  lbForwardTrip1.Caption := Chars(qry.ForwardTrip[1], '*');
  lbForwardTrip2.Caption := Chars(qry.ForwardTrip[2], '*');
  lbHeavyCount.Caption := IntToStr(qry.HeavyCount);
  lbLightCount.Caption := IntToStr(qry.LightCount);
  lbDelivered.Caption := FloatToStr(qry.LightCount * LightTonnage +
      qry.HeavyCount * HeavyTonnage);
  ShowStat(sgService, ['?????????? 0', '?????????? 1', '?????????? 2',
      '????????????'], Services);
  ShowStat(sgAction, ['???????'], qry.ReturnStat);
  ShowStat(sgQueue, ['?????????? 0', '?????????? 1', '?????????? 2',
      '????????????', '?????????'], Queues);
  if qry.Terminated then
    tmQuarry.Enabled := False
  else
    RunSimulation(qry);
end;

end.

