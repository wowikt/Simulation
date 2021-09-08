object frTrafficVisual: TfrTrafficVisual
  Left = 192
  Top = 114
  Width = 691
  Height = 557
  Caption = #1059#1095#1072#1089#1090#1086#1082' '#1086#1076#1085#1086#1089#1090#1086#1088#1086#1085#1085#1077#1075#1086' '#1076#1074#1080#1078#1077#1085#1080#1103
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object Label1: TLabel
    Left = 152
    Top = 16
    Width = 101
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100' 1'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label2: TLabel
    Left = 384
    Top = 16
    Width = 101
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100' 2'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbQueue1: TLabel
    Left = 16
    Top = 56
    Width = 233
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbQueue2: TLabel
    Left = 376
    Top = 56
    Width = 233
    Height = 25
    AutoSize = False
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label7: TLabel
    Left = 40
    Top = 96
    Width = 183
    Height = 25
    Caption = #1042#1088#1077#1084#1077#1085#1072' '#1079#1072#1076#1077#1088#1078#1082#1080
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label3: TLabel
    Left = 400
    Top = 144
    Width = 82
    Height = 25
    Caption = #1042#1088#1077#1084#1103' ='
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbSimTime: TLabel
    Left = 496
    Top = 144
    Width = 11
    Height = 25
    Caption = '0'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 40
    Top = 144
    Width = 186
    Height = 25
    Caption = #1042#1088#1077#1084#1077#1085#1072' '#1086#1078#1080#1076#1072#1085#1080#1103
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbWait1: TLabel
    Left = 240
    Top = 144
    Width = 11
    Height = 25
    Caption = '0'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbWait2: TLabel
    Left = 320
    Top = 144
    Width = 11
    Height = 25
    Caption = '0'
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object shLightLeft: TShape
    Left = 264
    Top = 56
    Width = 25
    Height = 25
    Brush.Color = clRed
    Shape = stCircle
  end
  object shLightRight: TShape
    Left = 336
    Top = 56
    Width = 25
    Height = 25
    Brush.Color = clRed
    Shape = stCircle
  end
  object edTime1: TEdit
    Left = 240
    Top = 96
    Width = 65
    Height = 33
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    Text = '60'
  end
  object edTime2: TEdit
    Left = 320
    Top = 96
    Width = 65
    Height = 33
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 1
    Text = '45'
  end
  object btStart: TButton
    Left = 400
    Top = 96
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 184
    Width = 649
    Height = 321
    ActivePage = tsStat
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    object tsStat: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072
      object sgWaitStat: TStringGrid
        Left = 8
        Top = 8
        Width = 625
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 0
      end
      object sgGate: TStringGrid
        Left = 8
        Top = 96
        Width = 625
        Height = 81
        ColCount = 3
        RowCount = 3
        TabOrder = 1
      end
      object sgResource: TStringGrid
        Left = 8
        Top = 184
        Width = 625
        Height = 97
        ColCount = 10
        RowCount = 3
        TabOrder = 2
      end
    end
    object tsQueue: TTabSheet
      Caption = #1054#1095#1077#1088#1077#1076#1080
      ImageIndex = 1
      object sgQueue: TStringGrid
        Left = 0
        Top = 8
        Width = 625
        Height = 161
        ColCount = 6
        RowCount = 6
        TabOrder = 0
      end
    end
  end
  object tmSim: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmSimTimer
    Left = 288
    Top = 8
  end
end
