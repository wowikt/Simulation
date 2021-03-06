program Bank;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UBank in 'UBank.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  bnk : TBankSimulation;

// ????????????? ??????? ????? ? ????? ????????
begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndClient := TRandom.Create;
  rndCashman := TRandom.Create;
  MaxClientCount := 100;

  // ?????? ?????????????
  bnk := TBankSimulation.Create;
  SwitchTo(bnk);
  with bnk do
  begin
    // ????? ??????????
    WriteLn('Finished at ', bnk.SimTime : 6 : 3);
    WriteLn;
    WriteStat('Clients in bank time:', InBankTime);
    WriteLn;
    WriteStat('Cash usage:', Cashman);
    WriteLn;
    WriteStat('Clients queue:', Cashman.Queue[0]);
    WriteLn;
    WriteStat('System calendar:', Calendar);
    WriteLn;
    WriteLn('Not waited: ', NotWaited);
    WriteLn;
    WriteHist('In bank time histogram:', InBankHist);
  end;

  bnk.Free;
  ReadLn;
end.
