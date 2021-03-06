program FlowLine;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UFlowLine in 'UFlowLine.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  sim : TFlowLineSimulation;

// ???????????? ????????? ???????????? ? ????? ???????? ???????
begin
  { TODO -oUser -cConsole Main : Insert code here }
  rndPiece := TRandom.Create;
  rndWorker1 := TRandom.Create;
  rndWorker2 := TRandom.Create;
  
  sim := TFlowLineSimulation.Create;
  SwitchTo(sim);
  with sim do
  begin
    WriteStat('Time in system: ', TimeInSystem);
    WriteLn;
    WriteStat('Time between balks:', Balks);
    WriteLn;
    WriteStat('Worker 1:', Stat1);
    WriteLn;
    WriteStat('Worker 2:', Stat2);
    WriteLn;
    WriteStat('Queue 1:', Queue1);
    WriteLn;
    WriteStat('Queue 2:', Queue2);
    WriteLn;
    WriteStat('Calendar:', Calendar);
    WriteLn;
    WriteHist('Time in system histogram:', TimeHist);
    Free;
  end;
  ReadLn;
end.
