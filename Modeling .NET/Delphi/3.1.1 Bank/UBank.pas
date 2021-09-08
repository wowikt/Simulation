unit UBank;

interface
uses Simulation;

type
  // Êëàññ TClient - ïðîöåññ, ìîäåëèðóþùèé êëèåíòà áàíêà
  TClient = class(Process)
  protected
    procedure Execute; override;
  end;

  // Êëàññ TClientGenerator - ïðîöåññ, ïîðîæäàþùèé êëèåíòîâ áàíêà
  TClientGenerator = class(Process)
  protected
    procedure Execute; override;
  end;

  // Êëàññ TCashman - ïðîöåññ, ìîäåëèðóþùèé ðàáîòó êàññèðà
  TCashman = class(Process)
  protected
    procedure Execute; override;
  end;

  // Êëàññ TBankSimulation - ìîäåëèðîâàíèå ðàáîòû áàíêà
  TBankSimulation = class(SimProc)
  public
    // Êàññèð
    Cash : TCashman;
    // Ãåíåðàòîð êëèåíòîâ
    Generator : TClientGenerator;
    // Î÷åðåäü îæèäàíèÿ
    Queue : List;
    // Ñòàòèñòèêà è ãèñòîãðàììà ïî âðåìåíè ïðåáûâàíèÿ
    //   êëèåíòîâ â áàíêå
    InBankTime : Statistics;
    InBankHist : Histogram;
    // Ñòàòèñòèêà ïî çàíÿòîñòè êàññèðà
    CashStat : ServiceStatistics;
    // Ñ÷åò÷èê êëèåíòîâ, îáñëóæåííûõ áåç îæèäàíèÿ
    NotWaited : Integer;
    procedure Finish; override;
    procedure StopStat; override;
  protected
    procedure Execute; override;
    procedure Init; override;
  end;

var
  // Äàò÷èêè ñëó÷àéíûõ ÷èñåë
  rndClient : Simulation.Random;
  rndCashman : Simulation.Random;
  // Êîëè÷åñòâî îáñëóæèâàåìûõ êëèåíòîâ
  MaxClientCount : Integer = 100;
  // Ñðåäíèé èíòåðâàë ìåæäó ïðèáûòèåì êëèåíòîâ
  MeanClientInterval : Double = 5;
  // Ãðàíèöû âðåìåíè îáñëóæèâàíèÿ
  MinCashTime : Double = 2;
  MaxCashTime : Double = 6;
  // Ïàðàìåòðû ãèñòîãðàììû
  HistMin : Double = 2;
  HistStep : Double = 2;
  HistStepCount : Integer = 20;

implementation

{ TClient }

procedure TClient.Execute;
var
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;
  // Àêòèâèðîâàòü êàññó
  par.Cash.Activate;
  // Âñòàòü â î÷åðåäü è æäàòü îáñëóæèâàíèÿ
  Wait(par.Queue);
  // Âñòàòü â î÷åðåäü çàâåðøåííûõ ïðîöåññîâ
  GoFinished;
end;

{ TClientGenerator }

procedure TClientGenerator.Execute;
var
  i : Integer;
begin
  for i := 1 to MaxClientCount do
  begin
    ClearFinished;
    TClient.Create.Activate;
    Hold(rndClient.Exponential(MeanClientInterval));
  end;
end;

{ TCashman }

procedure TCashman.Execute;
var
  Client : TClient;
  InTime : Double;
  par : TBankSimulation;
begin
  par := Parent as TBankSimulation;
  while True do
  begin
    // Åñëè î÷åðåäü ïóñòà, æäàòü ïðèáûòèÿ êëèåíòà
    while par.Queue.Empty do
      Passivate;

    // Èçâëå÷ü ïåðâîãî êëèåíòà èç î÷åðåäè
    Client := par.Queue.First as TClient;
    Client.StartRunning;

    // Åñëè êëèåíò íå æäàë, ó÷åñòü åãî
    if Client.StartingTime = SimTime then
      Inc(par.NotWaited);

    // Âûïîëíèòü îáñëóæèâàíèå
    par.CashStat.Start(SimTime);
    Hold(rndCashman.Uniform(MinCashTime, MaxCashTime));
    par.CashStat.Finish(SimTime);

    // Ó÷åñòü ïîëíîå âðåìÿ ïðåáûâàíèÿ â áàíêå
    InTime := SimTime - Client.StartingTime;
    par.InBankTime.AddData(InTime);
    par.InBankHist.AddData(InTime);

    // Âîçîáíîâèòü êëèåíòà, äàâ åìó âîçìîæíîñòü
    //   çàêîí÷èòü ðàáîòó
    Client.Activate;

    // Åñëè âñå êëèåíòû îáñëóæåíû, çàâåðøèòü ðàáîòó
    if par.CashStat.Finished = MaxClientCount then
      par.Activate;
  end;
end;

{ TBankSimulation }

procedure TBankSimulation.Finish;
begin
  Cash.Free;
  Generator.Free;
  InBankTime.Free;
  InBankHist.Free;
  CashStat.Free;
  Queue.Free;
  inherited;
end;

procedure TBankSimulation.Init;
begin
  inherited;
  Queue := List.Create('Очередь ожидания');
  Cash := TCashman.Create;
  Generator := TClientGenerator.Create;
  InBankTime := Statistics.Create('Статистика по времени пребывания в банке');
  CashStat := ServiceStatistics.Create('Статистика по занятости кассира');
  InBankHist := Histogram.Create(HistMin, HistStep, HistStepCount,
      'Гистограмма по времени пребывания в банке');
  NotWaited := 0;
end;

procedure TBankSimulation.Execute;
begin
  // Çàïóñòèòü ïðîöåññ ñîçäàíèÿ êëèåíòîâ
  Generator.Activate;
  // Æäàòü êîíöà èìèòàöèè
  Passivate;
  StopStat;
end;

procedure TBankSimulation.StopStat;
begin
  inherited;
  Queue.StopStat(SimTime);
  CashStat.StopStat(SimTime);
end;

end.
