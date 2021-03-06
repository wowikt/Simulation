unit ULoadingVis;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, ExtCtrls, XPMan, Grids, ComCtrls;

type
  TfrLoadingVis = class(TForm)
    Label1: TLabel;
    Label2: TLabel;
    Label3: TLabel;
    lbHeapsQueue: TLabel;
    lbLoadersQueue: TLabel;
    lbTrucksQueue: TLabel;
    lb: TLabel;
    lbSimTime: TLabel;
    lbLoader0: TLabel;
    lbLoader1: TLabel;
    tmLoading: TTimer;
    Label4: TLabel;
    edModelingTime: TEdit;
    btStart: TButton;
    lbForward: TLabel;
    lbUnload: TLabel;
    lbBack: TLabel;
    Label8: TLabel;
    Label9: TLabel;
    pgStats: TPageControl;
    tsStat: TTabSheet;
    sgService: TStringGrid;
    sgQueue: TStringGrid;
    shLoader0Back: TShape;
    shLoader0Usage: TShape;
    shLoader1Back: TShape;
    shLoader1Usage: TShape;
    Label5: TLabel;
    Label6: TLabel;
    Label7: TLabel;
    Label11: TLabel;
    Label12: TLabel;
    lbLoads0: TLabel;
    lbLoads1: TLabel;
    procedure btStartClick(Sender: TObject);
    procedure tmLoadingTimer(Sender: TObject);
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frLoadingVis: TfrLoadingVis;

implementation
uses ULoading, USimulation;

{$R *.dfm}

var
  ldg : TLoading = nil;
  Queues : array [0 .. 3] of TList;

procedure TfrLoadingVis.btStartClick(Sender: TObject);
begin
  if tmLoading.Enabled then
    tmLoading.Enabled := False
  else
  begin
    ModelingTime := StrToFloat(edModelingTime.Text);
    ldg.Free;
    ldg := TLoading.Create;
    Queues[0] := ldg.HeapQueue;
    Queues[1] := ldg.LoadersQueue;
    Queues[2] := ldg.TrucksQueue;
    Queues[3] := ldg.Calendar;
    tmLoading.Enabled := True;
    SwitchTo(ldg);
  end;
end;

procedure TfrLoadingVis.tmLoadingTimer(Sender: TObject);
begin
  ldg.StopStat;
  lbSimTime.Caption := Format('%.1f', [ldg.SimTime]);
  lbHeapsQueue.Caption := Chars(ldg.HeapQueue.Size, 'o');
  lbLoadersQueue.Caption := Chars(ldg.LoadersQueue.Size, '>');
  lbTrucksQueue.Caption := Chars(ldg.TrucksQueue.Size, '*');
  if ldg.LoadersStat[0].Running > 0 then
    lbLoader0.Caption := '(*)'
  else
    lbLoader0.Caption := '';
  if ldg.LoadersStat[1].Running > 0 then
    lbLoader1.Caption := '(*)'
  else
    lbLoader1.Caption := '';
  lbForward.Caption := Chars(ldg.ForwardCount, '*');
  lbUnload.Caption := Chars(ldg.UnloadCount, '*');
  lbBack.Caption := Chars(ldg.BackCount, '*');
  lbLoads0.Caption := IntToStr(ldg.LoadersStat[0].Finished);
  lbLoads1.Caption := IntToStr(ldg.LoadersStat[1].Finished);
  shLoader0Usage.Width := Round(ldg.LoadersStat[0].Mean * shLoader0Back.Width);
  shLoader1Usage.Width := Round(ldg.LoadersStat[1].Mean * shLoader1Back.Width);
  ShowStat(sgService, ['????????? 0', '????????? 1'], ldg.LoadersStat);
  ShowStat(sgQueue, ['????', '??????????', '?????????', '?????????'], Queues);
  if ldg.Terminated then
    tmLoading.Enabled := False
  else
    RunSimulation(ldg);
end;

procedure TfrLoadingVis.FormCreate(Sender: TObject);
begin
  rndBulldozer := TRandom.Create;
  rndLoader := TRandom.Create;
  rndTruck := TRandom.Create;
  sgQueue.ColWidths[0] := 100;
  sgQueue.ColWidths[1] := 70;
  sgQueue.ColWidths[2] := 100;
  sgQueue.ColWidths[3] := 80;
  sgQueue.ColWidths[4] := 60;
  sgQueue.ColWidths[5] := 80;
  sgService.ColWidths[0] := 100;
  sgService.ColWidths[1] := 95;
  sgService.ColWidths[2] := 70;
  sgService.ColWidths[3] := 100;
  sgService.ColWidths[4] := 60;
  sgService.ColWidths[5] := 145;
  sgService.ColWidths[6] := 115;
  sgService.ColWidths[7] := 105;
  sgService.ColWidths[8] := 90;
end;

end.
