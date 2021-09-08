object frShopMultiVis: TfrShopMultiVis
  Left = 192
  Top = 114
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1052#1072#1075#1072#1079#1080#1085' '#1089#1072#1084#1086#1086#1073#1089#1083#1091#1078#1080#1074#1072#1085#1080#1103
  ClientHeight = 655
  ClientWidth = 842
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
    Width = 161
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1080#1084#1080#1090#1072#1094#1080#1080
  end
  object Label2: TLabel
    Left = 16
    Top = 72
    Width = 154
    Height = 25
    Caption = #1063#1080#1089#1083#1086' '#1087#1088#1086#1075#1086#1085#1086#1074
  end
  object Label3: TLabel
    Left = 368
    Top = 24
    Width = 205
    Height = 25
    Caption = #1042#1099#1087#1086#1083#1085#1077#1085#1086' '#1087#1088#1086#1075#1086#1085#1086#1074
  end
  object lbRuns: TLabel
    Left = 592
    Top = 24
    Width = 11
    Height = 25
    Caption = '0'
  end
  object edSimulationTime: TEdit
    Left = 192
    Top = 16
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '480'
  end
  object edRunsCount: TEdit
    Left = 192
    Top = 72
    Width = 121
    Height = 33
    TabOrder = 1
    Text = '400'
  end
  object btStart: TButton
    Left = 328
    Top = 72
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 2
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 120
    Width = 809
    Height = 521
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
      object sgStat: TStringGrid
        Left = 8
        Top = 8
        Width = 785
        Height = 201
        ColCount = 6
        RowCount = 7
        TabOrder = 0
      end
    end
    object tsHist: TTabSheet
      Caption = #1043#1080#1089#1090#1086#1075#1088#1072#1084#1084#1072
      ImageIndex = 1
      object dgHistogram: TDrawGrid
        Left = 8
        Top = 8
        Width = 769
        Height = 473
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHistogramDrawCell
        OnTopLeftChanged = dgHistogramTopLeftChanged
      end
    end
  end
  object tmShop: TTimer
    Enabled = False
    Interval = 1
    OnTimer = tmShopTimer
    Left = 488
    Top = 72
  end
end
