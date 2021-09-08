program Bank;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UBank in 'UBank.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  bnk : TBankSimulation;

// ������������� ������� ����� � ����� ��������

begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndClient := TRandom.Create;
  rndCashman := TRandom.Create;
  MaxClientCount := 100;
  MaxQueueLength := 10;

  // ������ �������������
  bnk := TBankSimulation.Create;
  SwitchTo(bnk);
  with bnk do
  begin
    // ����� ����������
    WriteLn('Finished at ', SimTime : 6 : 3);
    WriteLn;
    WriteStat('Clients in bank time:', InBankTime);
    WriteLn;
    WriteStat('Cash usage:', CashStat);
    WriteLn;
    WriteStat('Clients queue:', Queue);
    WriteLn;
    WriteStat('System calendar:', Calendar);
    WriteLn;
    WriteLn('Not waited: ', NotWaited);
    WriteLn('Not serviced: ', NotServiced);
    WriteLn;
    WriteHist('In bank time histogram:', InBankHist);
  end;

  bnk.Free;
  ReadLn;
end.
