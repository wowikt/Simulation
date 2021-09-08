object frLoadingVis: TfrLoadingVis
  Left = 186
  Top = 106
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1043#1088#1091#1079#1086#1074#1099#1077' '#1087#1077#1088#1077#1074#1086#1079#1082#1080
  ClientHeight = 538
  ClientWidth = 666
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
    Top = 56
    Width = 46
    Height = 25
    Caption = #1050#1091#1095#1080
  end
  object Label2: TLabel
    Left = 72
    Top = 16
    Width = 111
    Height = 25
    Caption = #1055#1086#1075#1088#1091#1079#1095#1080#1082#1080
  end
  object Label3: TLabel
    Left = 72
    Top = 96
    Width = 107
    Height = 25
    Caption = #1057#1072#1084#1086#1089#1074#1072#1083#1099
  end
  object lbHeapsQueue: TLabel
    Left = 88
    Top = 56
    Width = 153
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbLoadersQueue: TLabel
    Left = 200
    Top = 16
    Width = 41
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbTrucksQueue: TLabel
    Left = 192
    Top = 96
    Width = 49
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lb: TLabel
    Left = 16
    Top = 144
    Width = 60
    Height = 25
    Caption = #1042#1088#1077#1084#1103
  end
  object lbSimTime: TLabel
    Left = 88
    Top = 144
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbLoader0: TLabel
    Left = 280
    Top = 32
    Width = 65
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbLoader1: TLabel
    Left = 280
    Top = 72
    Width = 65
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object Label4: TLabel
    Left = 16
    Top = 184
    Width = 218
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1084#1086#1076#1077#1083#1080#1088#1086#1074#1072#1085#1080#1103
  end
  object lbForward: TLabel
    Left = 432
    Top = 48
    Width = 49
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbUnload: TLabel
    Left = 560
    Top = 88
    Width = 81
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbBack: TLabel
    Left = 432
    Top = 128
    Width = 49
    Height = 25
    AutoSize = False
  end
  object Label8: TLabel
    Left = 552
    Top = 48
    Width = 95
    Height = 25
    Caption = #1056#1072#1079#1075#1088#1091#1079#1082#1072
  end
  object Label9: TLabel
    Left = 376
    Top = 48
    Width = 45
    Height = 25
    Caption = '==>'
  end
  object shLoader0Back: TShape
    Left = 264
    Top = 8
    Width = 100
    Height = 25
    Brush.Color = clNavy
  end
  object shLoader0Usage: TShape
    Left = 264
    Top = 8
    Width = 100
    Height = 25
    Brush.Color = clRed
  end
  object shLoader1Back: TShape
    Left = 264
    Top = 104
    Width = 100
    Height = 25
    Brush.Color = clNavy
  end
  object shLoader1Usage: TShape
    Left = 264
    Top = 104
    Width = 100
    Height = 25
    Brush.Color = clRed
  end
  object Label5: TLabel
    Left = 256
    Top = 40
    Width = 8
    Height = 25
    Caption = '/'
  end
  object Label6: TLabel
    Left = 256
    Top = 72
    Width = 8
    Height = 25
    Caption = '\'
  end
  object Label7: TLabel
    Left = 496
    Top = 48
    Width = 45
    Height = 25
    Caption = '==>'
  end
  object Label11: TLabel
    Left = 496
    Top = 128
    Width = 45
    Height = 25
    Caption = '<=='
  end
  object Label12: TLabel
    Left = 224
    Top = 128
    Width = 195
    Height = 25
    Caption = '^============'
  end
  object lbLoads0: TLabel
    Left = 376
    Top = 8
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbLoads1: TLabel
    Left = 376
    Top = 104
    Width = 11
    Height = 25
    Caption = '0'
  end
  object edModelingTime: TEdit
    Left = 248
    Top = 176
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '480'
  end
  object btStart: TButton
    Left = 384
    Top = 176
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 1
    OnClick = btStartClick
  end
  object pgStats: TPageControl
    Left = 16
    Top = 224
    Width = 633
    Height = 297
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
      object sgService: TStringGrid
        Left = 8
        Top = 8
        Width = 609
        Height = 105
        ColCount = 9
        RowCount = 3
        TabOrder = 0
      end
      object sgQueue: TStringGrid
        Left = 8
        Top = 120
        Width = 609
        Height = 129
        ColCount = 6
        TabOrder = 1
      end
    end
  end
  object tmLoading: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmLoadingTimer
    Left = 128
    Top = 136
  end
end
