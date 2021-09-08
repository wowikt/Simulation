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
    // Вывод статистики по занятости экскаваторов
    for i := 0 to ExcavatorCount - 1 do
    begin
      WriteStat('Excavator ' + IntToStr(i) + ' statistics:', ExcavatorStat[i]);
      WriteLn;
    end;
    // Вывод статистики по занятости измельчителя
    WriteStat('Mill statistics:', MillStat);
    WriteLn;
    // Вывод статистики по возвращению самосвалов
    WriteStat('Return statistics:', ReturnStat);
    WriteLn;
    // Вывод статистики по очередям к экскаваторам
    for i := 0 to ExcavatorCount - 1 do
    begin
      WriteStat('Excavator queue ' + IntToStr(i) + ' statistics: ',
          ExcavatorQueue[i]);
      WriteLn;
    end;
    // Вывод статистики по очереди к измельчителю
    WriteStat('Mill queue statistics: ', MillQueue);
    WriteLn;
    // Вывод статистики по календарю событий
    WriteStat('Calendar statistics: ', Calendar);
  end;
  ReadLn;
end.
