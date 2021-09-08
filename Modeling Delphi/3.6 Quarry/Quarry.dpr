program Quarry;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UQuarry in 'UQuarry.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  qry : TQuarry;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndExcavator := TRandom.Create;
  rndMill := TRandom.Create;
  qry := TQuarry.Create;
  SwitchTo(qry);
  with qry do
  begin
    // ����� ���������� �� ��������� ������������
    for i := 0 to ExcavatorCount - 1 do
    begin
      WriteStat('Excavator ' + IntToStr(i) + ' statistics:', ExcavatorStat[i]);
      WriteLn;
    end;
    // ����� ���������� �� ��������� ������������
    WriteStat('Mill statistics:', MillStat);
    WriteLn;
    // ����� ���������� �� ����������� ����������
    WriteStat('Return statistics:', ReturnStat);
    WriteLn;
    // ����� ���������� �� �������� � ������������
    for i := 0 to ExcavatorCount - 1 do
    begin
      WriteStat('Excavator queue ' + IntToStr(i) + ' statistics: ',
          ExcavatorQueue[i]);
      WriteLn;
    end;
    // ����� ���������� �� ������� � ������������
    WriteStat('Mill queue statistics: ', MillQueue);
    WriteLn;
    // ����� ���������� �� ��������� �������
    WriteStat('Calendar statistics: ', Calendar);
  end;
  ReadLn;
end.