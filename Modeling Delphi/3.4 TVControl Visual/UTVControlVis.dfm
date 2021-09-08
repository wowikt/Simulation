object frTVControl: TfrTVControl
  Left = 192
  Top = 114
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1050#1086#1085#1090#1088#1086#1083#1100' '#1090#1077#1083#1077#1074#1080#1079#1086#1088#1086#1074
  ClientHeight = 649
  ClientWidth = 706
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
    Width = 181
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100' '#1087#1088#1086#1074#1077#1088#1082#1080
  end
  object Label2: TLabel
    Left = 216
    Top = 16
    Width = 92
    Height = 25
    Caption = #1055#1088#1086#1074#1077#1088#1082#1072
  end
  object Label3: TLabel
    Left = 16
    Top = 128
    Width = 106
    Height = 25
    Caption = #1047#1072#1074#1077#1088#1096#1077#1085#1086
  end
  object Label4: TLabel
    Left = 136
    Top = 128
    Width = 162
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1074' '#1089#1080#1089#1090#1077#1084#1077
  end
  object Label5: TLabel
    Left = 320
    Top = 128
    Width = 147
    Height = 25
    Caption = #1058#1077#1082#1091#1097#1077#1077' '#1074#1088#1077#1084#1103
  end
  object Label6: TLabel
    Left = 320
    Top = 16
    Width = 190
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100' '#1085#1072#1089#1090#1088#1086#1081#1082#1080
  end
  object Label7: TLabel
    Left = 520
    Top = 16
    Width = 101
    Height = 25
    Caption = #1053#1072#1089#1090#1088#1086#1081#1082#1072
  end
  object lbInspectionQueue: TLabel
    Left = 32
    Top = 56
    Width = 153
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbAdjustmentQueue: TLabel
    Left = 344
    Top = 56
    Width = 153
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbInspection: TLabel
    Left = 208
    Top = 56
    Width = 105
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbAdjustment: TLabel
    Left = 520
    Top = 56
    Width = 105
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbInspectionBack: TLabel
    Left = 176
    Top = 96
    Width = 200
    Height = 25
    AutoSize = False
    Color = clNavy
    ParentColor = False
  end
  object lbInspectionUsage: TLabel
    Left = 176
    Top = 96
    Width = 200
    Height = 25
    AutoSize = False
    Color = clRed
    ParentColor = False
  end
  object lbAdjustmentBack: TLabel
    Left = 488
    Top = 96
    Width = 200
    Height = 25
    AutoSize = False
    Color = clNavy
    ParentColor = False
  end
  object lbAdjustmentUsage: TLabel
    Left = 488
    Top = 96
    Width = 200
    Height = 25
    AutoSize = False
    Color = clRed
    ParentColor = False
  end
  object lbCompleted: TLabel
    Left = 64
    Top = 168
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbTimeInSystem: TLabel
    Left = 208
    Top = 168
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbSimTime: TLabel
    Left = 384
    Top = 168
    Width = 11
    Height = 25
    Caption = '0'
  end
  object Label8: TLabel
    Left = 16
    Top = 208
    Width = 161
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1080#1084#1080#1090#1072#1094#1080#1080
  end
  object edSimulationTime: TEdit
    Left = 192
    Top = 208
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '480'
  end
  object btStart: TButton
    Left = 344
    Top = 208
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 1
    OnClick = btStartClick
  end
  object pcStat: TPageControl
    Left = 16
    Top = 256
    Width = 673
    Height = 377
    ActivePage = tsStat
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    object tsStat: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072
      object sgTime: TStringGrid
        Left = 8
        Top = 8
        Width = 649
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 0
      end
      object sgService: TStringGrid
        Left = 8
        Top = 104
        Width = 649
        Height = 97
        ColCount = 9
        RowCount = 3
        TabOrder = 1
      end
      object sgQueue: TStringGrid
        Left = 8
        Top = 216
        Width = 649
        Height = 120
        ColCount = 6
        RowCount = 4
        TabOrder = 2
      end
    end
  end
  object tmTVControl: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmTVControlTimer
    Left = 416
    Top = 96
  end
end
