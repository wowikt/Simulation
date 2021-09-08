unit QueensRun;

interface
uses Simulation;

type
  TQueensRun = class(Coroutine)
  strict protected
    procedure Run; override;
  end;

implementation
uses Queen, Board;

{ TQueensRun }

procedure TQueensRun.Run;
var
  i : Byte;
begin
  inherited;
  for i := 0 to QueensCount - 1 do
    TBoard.Queens[i] := TQueen.Create(i);
  TBoard.Queens[0].SwitchTo;
  for i := 0 to QueensCount - 1 do
    TBoard.Queens[i].Finish;
end;

end.
