program Traffic;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UTraffic in 'UTraffic.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  // ������ ��������
  trf : TTraffic;
  // ���������� �� �������� ������� ��������
  GlWaitStat : array [TDirection] of TStatistics;
  // ��������� ������ �������� ��������
  MinLeftTime : Double = 57;
  MinRightTime : Double = 42;
  LeftTimeStep : Double = 2;
  RightTimeStep : Double = 2;
  LeftStepCount : Integer = 4;
  RightStepCount : Integer = 4;
  // ���������� �������� � �����
  RunCount : Integer = 400;
  // ��������
  i, iLeft, iRight : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndCar := TRandom.Create;
  GlWaitStat[dirLeftRight] := TStatistics.Create;
  GlWaitStat[dirRightLeft] := TStatistics.Create;
  // ������� ���������� ��������
  for iLeft := 0 to LeftStepCount - 1 do
    for iRight := 0 to RightStepCount - 1 do
    begin
      // ������ ������� �������� ����������
      OpenTime[dirLeftRight] := MinLeftTime + iLeft * LeftTimeStep;
      OpenTime[dirRightLeft] := MinRightTime + iRight * RightTimeStep;
      // �������� ����������
      GlWaitStat[dirLeftRight].Clear;
      GlWaitStat[dirRightLeft].Clear;
      // ��������� ����� ��������
      for i := 1 to RunCount do
      begin
        // ������� �������� � ��������� ��
        trf := TTraffic.Create;
        SwitchTo(trf);
        // ������� ����������
        GlWaitStat[dirLeftRight].AddData(trf.WaitStat[dirLeftRight].Mean);
        GlWaitStat[dirRightLeft].AddData(trf.WaitStat[dirRightLeft].Mean);
        // ������� ��������
        trf.Free;
        // ���������� ��� ����������
        if i mod 10 = 0 then
          Write('.');
      end;
      // ���������� ���������� ����� ��������
      WriteLn;
      WriteLn('Left open time = ', OpenTime[dirLeftRight] : 4 : 0,
          ', right open time = ', OpenTime[dirRightLeft] : 4 : 0);
      WriteLn('Left wait time = ', GlWaitStat[dirLeftRight].Mean : 8 : 4,
          ' +- ', GlWaitStat[dirLeftRight].Deviation : 6 : 4);
      WriteLn('Right wait time = ', GlWaitStat[dirRightLeft].Mean : 8 : 4,
          ' +- ', GlWaitStat[dirRightLeft].Deviation : 6 : 4);
      WriteLn;
    end;
  // ��������� ������
  GlWaitStat[dirLeftRight].Free;
  GlWaitStat[dirRightLeft].Free;
  WriteLn('Done.');
  ReadLn;
end.

