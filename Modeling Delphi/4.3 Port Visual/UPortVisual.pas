unit UPortVisual;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, Grids, ComCtrls;

type
  TfrPortVisual = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    Label4: TLabel;
    Label5: TLabel;
    Label6: TLabel;
    lbQueue: TLabel;
    lbTug: TLabel;
    lbLoading: TLabel;
    lbSimTime: TLabel;
    lbLoaded: TLabel;
    lbStorm: TLabel;
    tmPort: TTimer;
    btStart: TButton;
    Label7: TLabel;
    edModelingTime: TEdit;
    pgStat: TPageControl;
    tsStat1: TTabSheet;
    tsStat2: TTabSheet;
    sgTimeStat: TStringGrid;
    sgResStat: TStringGrid;
    sgQueueStat: TStringGrid;
    shLoadingBack: TShape;
    shLoading: TShape;
    procedure btStartClick(Sender: TObject);
    procedure tmPortTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frPortVisual: TfrPortVisual;

implementation
uses UPort, USimulation;

{$R *.dfm}

var
  prt : TPort = nil;
  Res : array [0 .. 1] of TResource;
  Que : array [0 .. 2] of TList;

procedure TfrPortVisual.btStartClick(Sender: TObject);
begin
  if tmPort.Enabled then
    tmPort.Enabled := False
  else
  begin
    tmPort.Enabled := True;
    SimulationTime := StrToFloat(edModelingTime.Text);
    prt.Free;
    prt := TPort.Create;
    Res[0] := prt.Berth;
    Res[1] := prt.Tug;
    Que[0] := prt.Tug.Queue[0];
    Que[1] := prt.Tug.Queue[1];
    Que[2] := prt.Calendar;
    SwitchTo(prt);
  end;
end;

procedure TfrPortVisual.tmPortTimer(Sender: TObject);
var
  i : Integer;
  Loaded : Integer;
begin
  prt.StopStat;
  lbSimTime.Caption := FloatToStr(prt.SimTime);
  Loaded := 0;
  for i := 0 to TypeCount - 1 do
    Loaded := Loaded + prt.TimeStat[i].Count;
  lbLoaded.Caption := IntToStr(Loaded);
  lbQueue.Caption := Chars(prt.Tug.Queue[0].Size, '*');
  // ?????????? ???????, ??????????? ? ???????,
  //   ???????? ??? ?? ??????????? ? ???????????, ?? ????????? ???????????
  lbLoading.Caption := Chars(prt.Berth.Busy - prt.Tug.Queue[1].Size, '*') +
      Chars(prt.Tug.Queue[1].Size, 'o');
  // ?????????? ????????? ???????
  case prt.TugState of
  tsFree :
    lbTug.Caption := '';
  tsArrive :
    lbTug.Caption := '===>';
  tsDepart :
    lbTug.Caption := '<===';
  end;
  // ?????????? ????????? ??????
  if prt.Tug.Capacity > 0 then
    lbStorm.Caption := '______'
  else
    lbStorm.Caption := '^^^^^';
  shLoading.Width := Round(prt.Berth.BusyStat.Mean * shLoadingBack.Width /
      BerthCount);
  ShowStat(sgTimeStat, ['??? 0', '??? 1', '??? 2', '??? 3'], prt.TimeStat);
  ShowStat(sgResStat, ['???????', '??????'], Res);
  ShowStat(sgQueueStat, ['????????????', '???????????', '?????????'], Que);
  if prt.Terminated then
    tmPort.Enabled := False
  else
    RunSimulation(prt);
end;

procedure TfrPortVisual.FormCreate(Sender: TObject);
begin
  rndTanker := TRandom.Create;
  rndStorm := TRandom.Create;
  sgQueueStat.ColWidths[0] := 120;
  sgQueueStat.ColWidths[1] := 70;
  sgQueueStat.ColWidths[2] := 100;
  sgQueueStat.ColWidths[3] := 80;
  sgQueueStat.ColWidths[4] := 60;
  sgQueueStat.ColWidths[5] := 80;
  sgResStat.ColWidths[0] := 80;
  sgResStat.ColWidths[1] := 85;
  sgResStat.ColWidths[2] := 90;
  sgResStat.ColWidths[3] := 100;
  sgResStat.ColWidths[4] := 110;
  sgResStat.ColWidths[5] := 120;
  sgResStat.ColWidths[6] := 100;
  sgResStat.ColWidths[7] := 75;
  sgResStat.ColWidths[8] := 85;
  sgResStat.ColWidths[9] := 90;
  sgTimeStat.ColWidths[0] := 100;
  sgTimeStat.ColWidths[1] := 70;
  sgTimeStat.ColWidths[2] := 100;
  sgTimeStat.ColWidths[3] := 75;
  sgTimeStat.ColWidths[4] := 80;
  sgTimeStat.ColWidths[5] := 95;
end;

end.
