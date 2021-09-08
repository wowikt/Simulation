program Garage;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UGarage in 'UGarage.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  gar : TGarage;
  Intensity : Double = 0.5;

// ������������� ������ ������� ������������ ������������ �����������
begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndCar := TRandom.Create;
  rndService := TRandom.Create;

  // ��������� �������� ������
  ModelingTime := 1440;
  MeanServiceTime := 10.5;
  DeviationServiceTime := 1.35;
  CarsPerTimeUnit := Intensity;
  ParkingPlaces := 6;

  WriteLn('Car service simulation');
  WriteLn('Work time ', ModelingTime : 10 : 2);
  WriteLn('Service parameters:');
  WriteLn('Mean servcie time ', MeanServiceTime : 8 : 2);
  WriteLn('Deviation of service time ', DeviationServiceTime : 8 : 2);
  WriteLn('Places on parking ', ParkingPlaces : 5);

  while True do
  begin
    // �������� ���������
    gar := TGarage.Create;

    CarsPerTimeUnit := Intensity;
    WriteLn('Flow intensity ', CarsPerTimeUnit : 5 : 4);

    // ������ ���������
    SwitchTo(gar);

    // ����� �����������
    WriteLn;
    WriteLn('Simulation results:');
    WriteLn('Car income ', CarCount : 6);
    WriteLn('Serviced ', gar.BrigadeStat.Finished : 6);
    WriteLn('Not waited ', NoWaitCount : 6);
    WriteLn;
    with gar do
    begin
      WriteStat('Brigade usage:', BrigadeStat);
      WriteLn;
      WriteStat('Parking:', Parking);
    end;
    WriteLn;
    ReadLn;

    // ���� ��������� �� ����� 80% ��������� �����������,
    //   ��������� �������������
    if gar.BrigadeStat.Finished / CarCount >= 0.9 then
      Break;
    gar.Free;

    // ��������� ������������� �������� �� 10%
    Intensity := Intensity * 0.9;
  end;

  gar.Free;
  WriteLn;
  WriteLn('Found intensity ', Intensity : 10 : 4);
  ReadLn;
end.
