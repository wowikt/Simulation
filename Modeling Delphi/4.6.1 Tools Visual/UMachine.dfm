object frMachine: TfrMachine
  Left = 177
  Top = 98
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1057#1090#1072#1085#1086#1082' '#1089' '#1087#1086#1083#1086#1084#1082#1072#1084#1080
  ClientHeight = 417
  ClientWidth = 698
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
  object lbQueue: TLabel
    Left = 16
    Top = 48
    Width = 169
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
  object Label2: TLabel
    Left = 200
    Top = 16
    Width = 67
    Height = 25
    Caption = #1057#1090#1072#1085#1086#1082
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbState: TLabel
    Left = 200
    Top = 48
    Width = 65
    Height = 25
    Alignment = taCenter
    AutoSize = False
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object Label4: TLabel
    Left = 280
    Top = 16
    Width = 109
    Height = 25
    Caption = #1042#1099#1087#1086#1083#1085#1077#1085#1086
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbCompleted: TLabel
    Left = 320
    Top = 48
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
  object Label6: TLabel
    Left = 400
    Top = 16
    Width = 60
    Height = 25
    Caption = #1042#1088#1077#1084#1103
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
  end
  object lbTime: TLabel
    Left = 408
    Top = 48
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
  object Label1: TLabel
    Left = 16
    Top = 16
    Width = 170
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100' '#1079#1072#1076#1072#1085#1080#1081
  end
  object Label3: TLabel
    Left = 16
    Top = 88
    Width = 161
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1080#1084#1080#1090#1072#1094#1080#1080
  end
  object btStart: TButton
    Left = 352
    Top = 88
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -21
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 0
    OnClick = btStartClick
  end
  object edSimulationTime: TEdit
    Left = 192
    Top = 88
    Width = 121
    Height = 33
    TabOrder = 1
    Text = '500'
  end
  object pgStat: TPageControl
    Left = 16
    Top = 136
    Width = 665
    Height = 265
    ActivePage = tsTime
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    object tsTime: TTabSheet
      Caption = #1042#1088#1077#1084#1103' '#1080' '#1088#1077#1089#1091#1088#1089
      object sgTime: TStringGrid
        Left = 8
        Top = 8
        Width = 641
        Height = 129
        ColCount = 6
        TabOrder = 0
      end
      object sgResource: TStringGrid
        Left = 8
        Top = 144
        Width = 641
        Height = 73
        ColCount = 10
        RowCount = 2
        TabOrder = 1
      end
    end
    object tsAction: TTabSheet
      Caption = #1044#1077#1081#1089#1090#1074#1080#1103' '#1080' '#1086#1095#1077#1088#1077#1076#1080
      ImageIndex = 1
      object sgAction: TStringGrid
        Left = 8
        Top = 8
        Width = 641
        Height = 105
        ColCount = 6
        RowCount = 4
        TabOrder = 0
        ColWidths = (
          64
          67
          64
          64
          64
          64)
      end
      object sgQueue: TStringGrid
        Left = 8
        Top = 120
        Width = 641
        Height = 105
        ColCount = 6
        RowCount = 4
        TabOrder = 1
      end
    end
  end
  object tmTools: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmToolsTimer
    Left = 488
    Top = 48
  end
end
