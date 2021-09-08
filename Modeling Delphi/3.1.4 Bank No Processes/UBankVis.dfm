object frBank: TfrBank
  Left = 192
  Top = 114
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1054#1095#1077#1088#1077#1076#1100' '#1074' '#1073#1072#1085#1082#1077
  ClientHeight = 651
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
    Width = 83
    Height = 25
    Caption = #1054#1095#1077#1088#1077#1076#1100
  end
  object Label2: TLabel
    Left = 136
    Top = 16
    Width = 68
    Height = 25
    Caption = #1050#1072#1089#1089#1080#1088
  end
  object Label3: TLabel
    Left = 232
    Top = 16
    Width = 108
    Height = 25
    Caption = #1054#1073#1089#1083#1091#1078#1077#1085#1086
  end
  object Label4: TLabel
    Left = 520
    Top = 16
    Width = 60
    Height = 25
    Caption = #1042#1088#1077#1084#1103
  end
  object lbQueue: TLabel
    Left = 16
    Top = 56
    Width = 105
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbCashman: TLabel
    Left = 136
    Top = 56
    Width = 73
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbServiced: TLabel
    Left = 280
    Top = 56
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbSimTime: TLabel
    Left = 528
    Top = 56
    Width = 11
    Height = 25
    Caption = '0'
  end
  object Label5: TLabel
    Left = 16
    Top = 136
    Width = 154
    Height = 25
    Caption = #1063#1080#1089#1083#1086' '#1082#1083#1080#1077#1085#1090#1086#1074
  end
  object lbCashBack: TLabel
    Left = 96
    Top = 96
    Width = 200
    Height = 25
    AutoSize = False
    Color = clNavy
    ParentColor = False
  end
  object lbCashBusy: TLabel
    Left = 96
    Top = 96
    Width = 97
    Height = 25
    AutoSize = False
    Color = clRed
    ParentColor = False
  end
  object Label6: TLabel
    Left = 360
    Top = 16
    Width = 136
    Height = 25
    Caption = #1041#1077#1079' '#1086#1078#1080#1076#1072#1085#1080#1103
  end
  object lbNoWait: TLabel
    Left = 400
    Top = 56
    Width = 11
    Height = 25
    Caption = '0'
  end
  object btStart: TButton
    Left = 320
    Top = 136
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 0
    OnClick = btStartClick
  end
  object edClientCount: TEdit
    Left = 184
    Top = 136
    Width = 121
    Height = 33
    TabOrder = 1
    Text = '100'
  end
  object pgStats: TPageControl
    Left = 16
    Top = 184
    Width = 833
    Height = 449
    ActivePage = shStats
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    object shStats: TTabSheet
      Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072
      object sgTimeStat: TStringGrid
        Left = 8
        Top = 8
        Width = 809
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 0
        ColWidths = (
          64
          64
          64
          64
          64
          64)
      end
      object sgCashStat: TStringGrid
        Left = 8
        Top = 80
        Width = 809
        Height = 73
        ColCount = 9
        RowCount = 2
        TabOrder = 1
      end
      object sgQueueStat: TStringGrid
        Left = 8
        Top = 168
        Width = 809
        Height = 81
        ColCount = 6
        RowCount = 3
        TabOrder = 2
      end
    end
    object shHistogram: TTabSheet
      Caption = #1043#1080#1089#1090#1086#1075#1088#1072#1084#1084#1072
      ImageIndex = 1
      object dgHistogram: TDrawGrid
        Left = 0
        Top = 0
        Width = 825
        Height = 417
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHistogramDrawCell
        OnTopLeftChanged = dgHistogramTopLeftChanged
      end
    end
  end
  object tmBank: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmBankTimer
    Left = 320
    Top = 64
  end
end
