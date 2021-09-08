object frShopVisual: TfrShopVisual
  Left = 207
  Top = 199
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1052#1072#1075#1072#1079#1080#1085' '#1089#1072#1084#1086#1086#1073#1089#1083#1091#1078#1080#1074#1072#1085#1080#1103
  ClientHeight = 530
  ClientWidth = 858
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
  object Label6: TLabel
    Left = 480
    Top = 16
    Width = 147
    Height = 25
    Caption = #1058#1077#1082#1091#1097#1077#1077' '#1074#1088#1077#1084#1103
  end
  object lbSimTime: TLabel
    Left = 648
    Top = 16
    Width = 145
    Height = 25
    AutoSize = False
    Caption = '0'
  end
  object Label7: TLabel
    Left = 16
    Top = 16
    Width = 225
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1084#1086#1076#1077#1083#1080#1088#1086#1074#1072#1085#1080#1103':'
  end
  object edSimulationTime: TEdit
    Left = 248
    Top = 16
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '840'
  end
  object btStart: TButton
    Left = 384
    Top = 16
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    Default = True
    TabOrder = 1
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 64
    Width = 825
    Height = 457
    ActivePage = tsModel
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    object tsModel: TTabSheet
      Caption = #1052#1086#1076#1077#1083#1100
      ImageIndex = 2
      object Label1: TLabel
        Left = 16
        Top = 16
        Width = 133
        Height = 25
        Caption = #1058#1086#1088#1075#1086#1074#1099#1081' '#1079#1072#1083
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
      object Label2: TLabel
        Left = 160
        Top = 16
        Width = 83
        Height = 25
        Caption = #1054#1095#1077#1088#1077#1076#1100
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
      object Label3: TLabel
        Left = 256
        Top = 16
        Width = 55
        Height = 25
        Caption = #1050#1072#1089#1089#1072
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
      object Label4: TLabel
        Left = 320
        Top = 16
        Width = 108
        Height = 25
        Caption = #1054#1073#1089#1083#1091#1078#1077#1085#1086
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
      object Label5: TLabel
        Left = 440
        Top = 16
        Width = 162
        Height = 25
        Caption = #1042#1088#1077#1084#1103' '#1074' '#1089#1080#1089#1090#1077#1084#1077
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
      object lbShopping: TLabel
        Left = 16
        Top = 56
        Width = 129
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
      object lbQueue: TLabel
        Left = 160
        Top = 56
        Width = 81
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
      object lbCash: TLabel
        Left = 256
        Top = 56
        Width = 49
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
      object lbServiced: TLabel
        Left = 352
        Top = 56
        Width = 105
        Height = 25
        Alignment = taCenter
        AutoSize = False
        Caption = '0'
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
      object lbTimeInSystem: TLabel
        Left = 440
        Top = 56
        Width = 161
        Height = 25
        Alignment = taCenter
        AutoSize = False
        Caption = '0'
        Font.Charset = RUSSIAN_CHARSET
        Font.Color = clWindowText
        Font.Height = -21
        Font.Name = 'Tahoma'
        Font.Style = []
        ParentFont = False
      end
    end
    object tsGraph: TTabSheet
      Caption = #1040#1085#1080#1084#1072#1094#1080#1103
      ImageIndex = 1
      object imAnimation: TImage
        Left = 0
        Top = 0
        Width = 817
        Height = 417
      end
    end
    object tsStat: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072
      object sgCash: TStringGrid
        Left = 8
        Top = 8
        Width = 793
        Height = 73
        ColCount = 9
        RowCount = 2
        TabOrder = 0
      end
      object sgTime: TStringGrid
        Left = 8
        Top = 168
        Width = 793
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 1
      end
      object sgShopping: TStringGrid
        Left = 8
        Top = 96
        Width = 793
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 2
      end
      object sgQueue: TStringGrid
        Left = 8
        Top = 240
        Width = 793
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 3
      end
    end
  end
  object tmShop: TTimer
    Enabled = False
    Interval = 40
    OnTimer = tmShopTimer
    Left = 472
    Top = 16
  end
end
