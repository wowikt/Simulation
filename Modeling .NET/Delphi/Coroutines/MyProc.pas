unit MyProc;

interface
uses Simulation;

type
  TMyProc = class(Coroutine)
  protected
    { Private Declarations }
    procedure Run; override;
  public
    Name : string;
    Next : TMyProc;
    constructor Create(AName : string);
  end;

implementation

constructor TMyProc.Create(AName : string);
begin
  inherited Create;
  // TODO: Add any constructor code here
  Name := AName;
end;

procedure TMyProc.Run;
var
  i : Integer;
begin
  inherited;
  for i := 0 to 4 do
  begin
    Console.WriteLine('Сопрограмма {0}: {1}', [Name, i]);
    Next.SwitchTo;
  end;
end;

end.
