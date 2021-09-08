unit Queen;

interface
uses Simulation;

type
  TQueen = class(Coroutine)
  strict protected
    { Private Declarations }
    procedure Run; override;
  public
    Col : Byte;
    Row : Byte;
    constructor Create(ACol : Byte);
  end;

implementation
uses Board;

{ TQueen }

constructor TQueen.Create(ACol : Byte);
begin
  inherited Create;
  Col := ACol;
  Row := 0;
end;

procedure TQueen.Run;
begin
  inherited;
  while True do
  begin
    if TBoard.IsFree(Col, Row) then
    begin
      TBoard.MakeOccupied(Col, Row);
      if Col < QueensCount - 1 then
        TBoard.Queens[Col + 1].SwitchTo
      else
        TBoard.Remember;
      TBoard.MakeFree(Col, Row);
    end;
    Inc(Row);
    if Row = QueensCount then
    begin
      Row := 0;
      if Col = 0 then
        Exit
      else
        TBoard.Queens[Col - 1].SwitchTo;
    end;
  end;
end;

end.
