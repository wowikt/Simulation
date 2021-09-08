unit UCoroutine;

interface
uses USimulation;

type
  TCoroutine = class(TRunningObject)
  public
    NextCor : TRunningObject;
    Name : string;
  protected
    procedure Execute; override;
  end;

implementation

{ TCoroutine }

procedure TCoroutine.Execute;
var
  i : Integer;
begin
  Detach;
  for i := 1 to 5 do
  begin
    WriteLn('Coroutine ', Name, '; i = ', i);
    SwitchTo(NextCor);
  end;
  Detach;
end;

end.
