program Coroutines;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  UCoroutine in 'UCoroutine.pas',
  USimulation in '..\USimulation\USimulation.pas';

var
  corA, corB, corC : TCoroutine;
begin
  { TODO -oUser -cConsole Main : Insert code here }
  corA := TCoroutine.Create;
  corB := TCoroutine.Create;
  corC := TCoroutine.Create;
  corA.Name := 'A';
  corB.Name := 'B';
  corC.Name := 'C';
  corA.NextCor := corB;
  corB.NextCor := corC;
  corC.NextCor := corA;
  SwitchTo(corA);
  ReadLn;
end.
