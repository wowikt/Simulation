object frShopVisual: TfrShopVisual
  Left = 254
  Top = 180
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1052#1072#1075#1072#1079#1080#1085' '#1089#1072#1084#1086#1086#1073#1089#1083#1091#1078#1080#1074#1072#1085#1080#1103
  ClientHeight = 650
  ClientWidth = 715
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
    Left = 40
    Top = 16
    Width = 133
    Height = 25
    Caption = #1058#1086#1088#1075#1086#1074#1099#1081' '#1079#1072#1083
  end
  object Label2: TLabel
    Left = 248
    Top = 16
    Width = 83
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100
  end
  object Label3: TLabel
    Left = 368
    Top = 16
    Width = 55
    Height = 25
    Caption = #1050#1072#1089#1089#1072
  end
  object Label4: TLabel
    Left = 16
    Top = 136
    Width = 108
    Height = 25
    Caption = #1054#1073#1089#1083#1091#1078#1077#1085#1086
  end
  object Label5: TLabel
    Left = 408
    Top = 128
    Width = 162
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1074' '#1089#1080#1089#1090#1077#1084#1077
  end
  object Label6: TLabel
    Left = 448
    Top = 16
    Width = 147
    Height = 25
    Caption = #1058#1077#1082#1091#1097#1077#1077' '#1074#1088#1077#1084#1103
  end
  object lbShopping: TLabel
    Left = 16
    Top = 56
    Width = 185
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbQueue: TLabel
    Left = 216
    Top = 56
    Width = 137
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbCash: TLabel
    Left = 368
    Top = 56
    Width = 49
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbServiced: TLabel
    Left = 144
    Top = 136
    Width = 105
    Height = 25
    Alignment = taCenter
    AutoSize = False
    Caption = '0'
  end
  object lbTimeInSystem: TLabel
    Left = 408
    Top = 168
    Width = 161
    Height = 25
    Alignment = taCenter
    AutoSize = False
    Caption = '0'
  end
  object lbSimTime: TLabel
    Left = 608
    Top = 16
    Width = 89
    Height = 25
    Alignment = taCenter
    AutoSize = False
    Caption = '0'
  end
  object Label7: TLabel
    Left = 16
    Top = 216
    Width = 225
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1084#1086#1076#1077#1083#1080#1088#1086#1074#1072#1085#1080#1103':'
  end
  object Label8: TLabel
    Left = 136
    Top = 96
    Width = 111
    Height = 25
    Caption = #1087#1086#1082#1091#1087#1072#1090#1077#1083#1080
  end
  object lbCanceledCust: TLabel
    Left = 192
    Top = 176
    Width = 11
    Height = 25
    Caption = '0'
  end
  object Label9: TLabel
    Left = 264
    Top = 96
    Width = 77
    Height = 25
    Caption = #1087#1086#1082#1091#1087#1082#1080
  end
  object lbCanceledBuys: TLabel
    Left = 296
    Top = 176
    Width = 11
    Height = 25
    Caption = '0'
  end
  object shCashBack: TShape
    Left = 432
    Top = 56
    Width = 200
    Height = 25
    Brush.Color = clNavy
  end
  object shCash: TShape
    Left = 432
    Top = 56
    Width = 200
    Height = 25
    Brush.Color = clRed
  end
  object Label10: TLabel
    Left = 16
    Top = 176
    Width = 71
    Height = 25
    Caption = #1054#1090#1082#1072#1079#1099
  end
  object lbTotalBuys: TLabel
    Left = 264
    Top = 136
    Width = 73
    Height = 25
    Alignment = taCenter
    AutoSize = False
    Caption = '0'
  end
  object edSimulationTime: TEdit
    Left = 248
    Top = 216
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '480'
  end
  object btStart: TButton
    Left = 384
    Top = 216
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 1
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 264
    Width = 681
    Height = 369
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
      object sgCash: TStringGrid
        Left = 8
        Top = 8
        Width = 657
        Height = 73
        ColCount = 9
        RowCount = 2
        TabOrder = 0
      end
      object sgTime: TStringGrid
        Left = 8
        Top = 168
        Width = 657
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 1
      end
      object sgShopping: TStringGrid
        Left = 8
        Top = 96
        Width = 657
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 2
      end
      object sgQueue: TStringGrid
        Left = 8
        Top = 240
        Width = 657
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 3
      end
    end
  end
  object tmShop: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmShopTimer
    Left = 512
    Top = 216
  end
end
