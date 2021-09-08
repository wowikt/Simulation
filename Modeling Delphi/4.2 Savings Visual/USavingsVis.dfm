object frSavings: TfrSavings
  Left = 192
  Top = 114
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1059#1087#1088#1072#1074#1083#1077#1085#1080#1077' '#1079#1072#1087#1072#1089#1072#1084#1080
  ClientHeight = 546
  ClientWidth = 755
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
  object lbQueue: TLabel
    Left = 16
    Top = 56
    Width = 185
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object shAvailableBack: TShape
    Left = 232
    Top = 56
    Width = 200
    Height = 25
    Brush.Color = clNavy
  end
  object Label2: TLabel
    Left = 232
    Top = 16
    Width = 194
    Height = 25
    Caption = #1059#1095#1077#1090#1085#1086#1077' '#1082#1086#1083#1080#1095#1077#1089#1090#1074#1086
  end
  object Label3: TLabel
    Left = 232
    Top = 96
    Width = 186
    Height = 25
    Caption = #1053#1072#1083#1080#1095#1080#1077' '#1085#1072' '#1089#1082#1083#1072#1076#1077
  end
  object shRadioCountBack: TShape
    Left = 232
    Top = 136
    Width = 200
    Height = 25
    Brush.Color = clNavy
  end
  object shAvailable: TShape
    Left = 232
    Top = 56
    Width = 200
    Height = 25
    Brush.Color = clRed
  end
  object shRadioCount: TShape
    Left = 232
    Top = 136
    Width = 200
    Height = 25
    Brush.Color = clRed
  end
  object Label4: TLabel
    Left = 448
    Top = 16
    Width = 128
    Height = 25
    Caption = #1055#1088#1080#1086#1073#1088#1077#1090#1077#1085#1086
  end
  object lbTotalCount: TLabel
    Left = 608
    Top = 16
    Width = 11
    Height = 25
    Caption = '0'
  end
  object Label5: TLabel
    Left = 16
    Top = 176
    Width = 252
    Height = 25
    Caption = #1055#1088#1077#1076#1074#1072#1088#1080#1090#1077#1083#1100#1085#1099#1081' '#1087#1088#1086#1075#1086#1085
  end
  object Label6: TLabel
    Left = 104
    Top = 224
    Width = 168
    Height = 25
    Caption = #1054#1089#1085#1086#1074#1085#1086#1081' '#1087#1088#1086#1075#1086#1085
  end
  object Label7: TLabel
    Left = 448
    Top = 96
    Width = 147
    Height = 25
    Caption = #1058#1077#1082#1091#1097#1077#1077' '#1074#1088#1077#1084#1103
  end
  object lbSimTime: TLabel
    Left = 608
    Top = 96
    Width = 11
    Height = 25
    Caption = '0'
  end
  object Label8: TLabel
    Left = 448
    Top = 56
    Width = 71
    Height = 25
    Caption = #1054#1090#1082#1072#1079#1099
  end
  object lbNoBuys: TLabel
    Left = 608
    Top = 56
    Width = 11
    Height = 25
    Caption = '0'
  end
  object edPreSimulation: TEdit
    Left = 288
    Top = 176
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '52'
  end
  object edMainSimulation: TEdit
    Left = 288
    Top = 224
    Width = 121
    Height = 33
    TabOrder = 1
    Text = '260'
  end
  object btStart: TButton
    Left = 424
    Top = 176
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 2
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 272
    Width = 721
    Height = 257
    ActivePage = tsStat1
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    object tsStat1: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072' 1'
      object sgLostSales: TStringGrid
        Left = 8
        Top = 8
        Width = 697
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 0
      end
      object sgSafetyStock: TStringGrid
        Left = 8
        Top = 72
        Width = 697
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 1
      end
      object sgInvPosition: TStringGrid
        Left = 8
        Top = 160
        Width = 697
        Height = 57
        ColCount = 7
        RowCount = 2
        TabOrder = 2
      end
    end
    object tsStat2: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072' 2'
      ImageIndex = 1
      object sgRadio: TStringGrid
        Left = 8
        Top = 8
        Width = 697
        Height = 73
        ColCount = 10
        RowCount = 2
        TabOrder = 0
      end
      object sgQueue: TStringGrid
        Left = 8
        Top = 88
        Width = 697
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 1
      end
    end
  end
  object tmSavings: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmSavingsTimer
    Left = 32
    Top = 128
  end
  object XPManifest1: TXPManifest
    Left = 536
    Top = 152
  end
end
