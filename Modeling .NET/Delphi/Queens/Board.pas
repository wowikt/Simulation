unit Board;

interface
uses Simulation, Queen;

const QueensCount = 5;

type
  TBoard = class
  private
    class var Rows : array [0 .. QueensCount - 1] of Boolean;
    class var DiagsUp : array [0 .. 2 * QueensCount - 1] of Boolean;
    class var DiagsDown : array [0 .. 2 * QueensCount - 1] of Boolean;
  public
    class var Queens : array [0 .. QueensCount - 1] of TQueen;
    class function IsFree(Col, Row : Byte) : Boolean;
    class procedure MakeOccupied(Col, Row : Byte);
    class procedure MakeFree(Col, Row : Byte);
    class procedure Remember;
  end;

implementation

{ TBoard }

class function TBoard.IsFree(Col, Row: Byte): Boolean;
begin
  Result := not Rows[Row] and not DiagsUp[Col + QueensCount - Row - 1] and
      not DiagsDown[Col + Row];
end;

class procedure TBoard.MakeOccupied(Col, Row: Byte);
begin
  Rows[Row] := True;
  DiagsUp[Col + QueensCount - Row - 1] := True;
  DiagsDown[Col + Row] := True;
end;

class procedure TBoard.MakeFree(Col, Row: Byte);
begin
  Rows[Row] := False;
  DiagsUp[Col + QueensCount - Row - 1] := False;
  DiagsDown[Col + Row] := False;
end;

class procedure TBoard.Remember;
var
  i : Integer;
begin
  for i := 0 to QueensCount - 1 do
    Console.Write('{0} ', [Queens[i].Row]);
  Console.WriteLine;
end;

end.
