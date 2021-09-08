program Bank;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Simulation,
  Fibers,
  UBank in 'UBank.pas';

var
  bnk : TBankSimulation;

// Ìîäåëèðîâàíèå î÷åðåäè áàíêà ñ îäíèì êàññèðîì

begin
  try
    { TODO -oUser -cConsole Main : Insert code here }
    rndClient := Simulation.Random.Create;
    rndCashman := Simulation.Random.Create;
    MaxClientCount := 100;

    // Çàïóñê ìîäåëèðîâàíèÿ
    bnk := TBankSimulation.Create;
    bnk.Start;
    with bnk do
    begin
      // Âûâîä ñòàòèñòèêè
      WriteLn('Имитация закончена в ', SimTime : 6 : 3);
      WriteLn;
      WriteLn(InBankTime);
      WriteLn;
      WriteLn(CashStat);
      WriteLn;
      WriteLn(Queue.Statistics);
      WriteLn;
      WriteLn(Calendar.Statistics);
      WriteLn;
      WriteLn('Обслужено без ожидания: ', NotWaited);
      WriteLn;
      WriteLn(InBankHist);
    end;

    bnk.Finish;
    ReadLn;
  except
    on E:Exception do
      Writeln(E.Classname, ': ', E.Message);
  end;
end.
