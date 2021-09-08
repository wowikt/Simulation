program Queens;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Simulation,
  Queen in 'Queen.pas',
  QueensRun in 'QueensRun.pas',
  Board in 'Board.pas';

var
  QRun : TQueensRun;

begin
  try
    { TODO -oUser -cConsole Main : Insert code here }
    QRun := TQueensRun.Create;
    QRun.SwitchTo;
    QRun.Finish;
    Console.WriteLine('Готово');
    Console.ReadLine;
  except
    on E:Exception do
      Writeln(E.Classname, ': ', E.Message);
  end;
end.
