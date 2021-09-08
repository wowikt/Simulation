program TVControl;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UTVControl in 'UTVControl.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  tvc : TTVControl;

// ������ �������� ����� �� �������� �����������
begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndTVSet := TRandom.Create;
  rndInspector := TRandom.Create;
  rndAdjuster := TRandom.Create;
  // �������� ������ � ������
  tvc := TTVControl.Create;
  SwitchTo(tvc);
  with tvc do
  begin
    // ����� ����������:
    // ����� ���������� ����������� � �������
    WriteStat('Time in system:', TimeInSystemStat);
    WriteLn;
    // ��������� �����������
    WriteStat('Inspectors:', InspectorsStat);
    WriteLn;
    // ��������� �����������
    WriteStat('Adjustment:', AdjustmentStat);
    WriteLn;
    // ������� ��������
    WriteStat('Inspection queue:', InspectionQueue);
    WriteLn;
    // ������� ���������
    WriteStat('Adjustment queue:', AdjustmentQueue);
    WriteLn;
    // ������� �������
    WriteStat('Calendar:', Calendar);
  end;
  tvc.Free;
  ReadLn;
end.