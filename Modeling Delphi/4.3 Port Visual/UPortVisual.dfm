object frPortVisual: TfrPortVisual
  Left = 192
  Top = 114
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1052#1086#1076#1077#1083#1100' '#1087#1086#1088#1090#1072
  ClientHeight = 465
  ClientWidth = 862
  Color = clBtnFace
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -21
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 25
  object Label1: TLabel
    Left = 16
    Top = 16
    Width = 186
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100' '#1086#1078#1080#1076#1072#1085#1080#1103
  end
  object Label2: TLabel
    Left = 216
    Top = 16
    Width = 66
    Height = 25
    Caption = #1041#1091#1082#1089#1080#1088
  end
  object Label3: TLabel
    Left = 296
    Top = 16
    Width = 87
    Height = 25
    Caption = #1055#1086#1075#1088#1091#1079#1082#1072
  end
  object Label4: TLabel
    Left = 440
    Top = 16
    Width = 60
    Height = 25
    Caption = #1042#1088#1077#1084#1103
  end
  object Label5: TLabel
    Left = 440
    Top = 56
    Width = 105
    Height = 25
    Caption = #1055#1086#1075#1088#1091#1078#1077#1085#1086
  end
  object Label6: TLabel
    Left = 48
    Top = 96
    Width = 66
    Height = 25
    Caption = #1064#1090#1086#1088#1084
  end
  object lbQueue: TLabel
    Left = 16
    Top = 56
    Width = 185
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbTug: TLabel
    Left = 216
    Top = 56
    Width = 65
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbLoading: TLabel
    Left = 296
    Top = 56
    Width = 89
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbSimTime: TLabel
    Left = 560
    Top = 16
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbLoaded: TLabel
    Left = 560
    Top = 56
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbStorm: TLabel
    Left = 128
    Top = 96
    Width = 89
    Height = 25
    Alignment = taCenter
    AutoSize = False
    Caption = '______'
  end
  object Label7: TLabel
    Left = 16
    Top = 136
    Width = 225
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1084#1086#1076#1077#1083#1080#1088#1086#1074#1072#1085#1080#1103':'
  end
  object shLoadingBack: TShape
    Left = 264
    Top = 96
    Width = 200
    Height = 25
    Brush.Color = clNavy
  end
  object shLoading: TShape
    Left = 264
    Top = 96
    Width = 200
    Height = 25
    Brush.Color = clRed
  end
  object btStart: TButton
    Left = 392
    Top = 136
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 0
    OnClick = btStartClick
  end
  object edModelingTime: TEdit
    Left = 248
    Top = 136
    Width = 121
    Height = 33
    TabOrder = 1
    Text = '8760'
  end
  object pgStat: TPageControl
    Left = 16
    Top = 184
    Width = 833
    Height = 265
    ActivePage = tsStat1
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    MultiLine = True
    ParentFont = False
    TabOrder = 2
    object tsStat1: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072' 1'
      object sgTimeStat: TStringGrid
        Left = 8
        Top = 8
        Width = 809
        Height = 129
        ColCount = 6
        TabOrder = 0
      end
    end
    object tsStat2: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072' 2'
      ImageIndex = 1
      object sgResStat: TStringGrid
        Left = 8
        Top = 8
        Width = 809
        Height = 97
        ColCount = 10
        RowCount = 3
        TabOrder = 0
      end
      object sgQueueStat: TStringGrid
        Left = 8
        Top = 120
        Width = 809
        Height = 105
        ColCount = 6
        RowCount = 4
        TabOrder = 1
      end
    end
  end
  object tmPort: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmPortTimer
    Left = 488
    Top = 144
  end
end
