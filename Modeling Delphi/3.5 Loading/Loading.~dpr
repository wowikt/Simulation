program Loading;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  USimulation in '..\USimulation\USimulation.pas',
  ULoading in 'ULoading.pas';

var
  ldg : TLoading;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndBulldozer := TRandom.Create;
  rndLoader := TRandom.Create;
  rndTruck := TRandom.Create;
  // Создать процесс моделирования и запустить его
  ldg := TLoading.Create;
  SwitchTo(ldg);
  with ldg do
  begin
    // Статистика по занятости каждого погрузчика
    //   в отдельности
    for i := 0 to LoadersCount - 1 do
    begin
      WriteStat('Loader ' + IntToStr(i) + ' statistics:', LoadersStat[i]);
      WriteLn;
    end;
    // Статистика по кучам
    WriteStat('Heaps queue:', HeapQueue);
    WriteLn;
    // Статистика по очереди самосвалов
    WriteStat('Trucks queue:', TrucksQueue);
    WriteLn;
    // Статистика по очереди погрузчиков
    WriteStat('Loaders queue:', LoadersQueue);
    WriteLn;
    // Статистика по системному календарю
    WriteStat('Calendar:', Calendar);
  end;
  ldg.Free;
  ReadLn;
end.
