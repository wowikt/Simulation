unit UQueen;

interface
uses USimulation;

type
  TQueen = class(TRunningObject)
  public
    Col, Row : Byte;
    constructor Create(ACol : Byte);
  protected
    procedure Execute; override;
  end;

  TQueensSimulation = class(TRunningObject)
  protected
    procedure Execute; override;
  end;

implementation

const
  N = 5;

var
  Queens : array [1 .. N] of TQueen;

  Rows : array [1 .. N] of Boolean;
  DiagsUp : array [-N + 1 .. N - 1] of Boolean;
  DiagsDown : array [2 .. 2 * N] of Boolean;

function IsFree(Col, Row : Byte) : Boolean;
begin
  Result := not Rows[Row] and not DiagsUp[Col - Row] and
      not DiagsDown[Col + Row];
end;

procedure MakeOccupied(Col, Row : Byte);
begin
  Rows[Row] := True;
  DiagsUp[Col - Row] := True;
  DiagsDown[Col + Row] := True;
end;

procedure MakeFree(Col, Row : Byte);
begin
  Rows[Row] := False;
  DiagsUp[Col - Row] := False;
  DiagsDown[Col + Row] := False;
end;

procedure Remember;
var
  i : Integer;
begin
  for i := 1 to N do
    Write(Queens[i].Row, ' ');
  WriteLn;
end;

{ TQueen }

constructor TQueen.Create(ACol : Byte);
begin
  Col := ACol;
  Row := 1;
  inherited Create;
end;

procedure TQueen.Execute;
begin
  Detach;
  while True do
  begin
    if IsFree(Col, Row) then
    begin
      MakeOccupied(Col, Row);
      if Col < N then
      begin
        SwitchTo(Queens[Col + 1]);
      end
      else
        Remember;
      MakeFree(Col, Row);
    end;
    Inc(Row);
    if Row > N then
    begin
      Row := 1;
      if Col = 1 then
        Detach
      else
        SwitchTo(Queens[Col - 1]);
    end;
  end;
end;

{ TQueensSimulation }

procedure TQueensSimulation.Execute;
var
  i : Integer;
begin
  Detach;
  for i := 1 to N do
    Queens[i] := TQueen.Create(i);
  SwitchTo(Queens[1]);
  for i :=1 to N do
    Queens[i].Free;
  Detach;
end;

end.
