program PRandom;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  USimulation in '..\USimulation\USimulation.pas';

var
  rnd : TRandom;
  hst : THistogram;
  st : TStatistics;
  RndVal : Double;
  i : Integer;

begin
  { TODO -oUser -cConsole Main : Insert code here }
  Randomize;
  rnd := TRandom.Create;
  hst := TUniformHistogram.Create(0, 0.2, 10);
  st := TStatistics.Create;
  for i := 1 to 10000 do
  begin
    RndVal := rnd.Exponential(0.4);
    hst.AddData(RndVal);
    st.AddData(RndVal);
  end;

  WriteStat('Values:', st);
  WriteHist('Histogram:', hst);
  ReadLn;
end.
