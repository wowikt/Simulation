object frQuarryVis: TfrQuarryVis
  Left = 164
  Top = 128
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1050#1072#1088#1100#1077#1088
  ClientHeight = 563
  ClientWidth = 722
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
    Left = 136
    Top = 16
    Width = 124
    Height = 25
    Caption = #1069#1082#1089#1082#1072#1074#1072#1090#1086#1088#1099
  end
  object Label2: TLabel
    Left = 16
    Top = 16
    Width = 107
    Height = 25
    Caption = #1057#1072#1084#1086#1089#1074#1072#1083#1099
  end
  object Label3: TLabel
    Left = 552
    Top = 16
    Width = 138
    Height = 25
    Caption = #1048#1079#1084#1077#1083#1100#1095#1080#1090#1077#1083#1100
  end
  object Label5: TLabel
    Left = 64
    Top = 184
    Width = 60
    Height = 25
    Caption = '<==='
  end
  object Label6: TLabel
    Left = 400
    Top = 184
    Width = 60
    Height = 25
    Caption = '<==='
  end
  object lbExcavatorQueue0: TLabel
    Left = 40
    Top = 56
    Width = 73
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbExcavatorQueue1: TLabel
    Left = 40
    Top = 96
    Width = 73
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbExcavatorQueue2: TLabel
    Left = 40
    Top = 136
    Width = 73
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbExcavator0: TLabel
    Left = 160
    Top = 56
    Width = 73
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbExcavator1: TLabel
    Left = 160
    Top = 96
    Width = 73
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbExcavator2: TLabel
    Left = 160
    Top = 136
    Width = 73
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbMillQueue: TLabel
    Left = 456
    Top = 96
    Width = 97
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbMill: TLabel
    Left = 584
    Top = 96
    Width = 73
    Height = 25
    Alignment = taCenter
    AutoSize = False
  end
  object lbBackTrip: TLabel
    Left = 240
    Top = 184
    Width = 97
    Height = 25
    AutoSize = False
  end
  object Label7: TLabel
    Left = 16
    Top = 232
    Width = 218
    Height = 25
    Caption = #1042#1088#1077#1084#1103' '#1084#1086#1076#1077#1083#1080#1088#1086#1074#1072#1085#1080#1103
  end
  object Label8: TLabel
    Left = 344
    Top = 16
    Width = 67
    Height = 25
    Caption = #1042#1088#1077#1084#1103':'
  end
  object lbSimTime: TLabel
    Left = 424
    Top = 16
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbExcavator0Completed: TLabel
    Left = 248
    Top = 56
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbExcavator2Completed: TLabel
    Left = 248
    Top = 136
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbExcavator1Completed: TLabel
    Left = 248
    Top = 96
    Width = 11
    Height = 25
    Caption = '0'
  end
  object shExcavator0Back: TShape
    Left = 280
    Top = 56
    Width = 100
    Height = 25
    Brush.Color = clNavy
  end
  object shExcavator0: TShape
    Left = 280
    Top = 56
    Width = 100
    Height = 25
    Brush.Color = clRed
  end
  object shExcavator1Back: TShape
    Left = 280
    Top = 96
    Width = 100
    Height = 25
    Brush.Color = clNavy
  end
  object shExcavator1: TShape
    Left = 280
    Top = 96
    Width = 100
    Height = 25
    Brush.Color = clRed
  end
  object shExcavator2Back: TShape
    Left = 280
    Top = 136
    Width = 100
    Height = 25
    Brush.Color = clNavy
  end
  object shExcavator2: TShape
    Left = 280
    Top = 136
    Width = 100
    Height = 25
    Brush.Color = clRed
  end
  object lbMillCompleted: TLabel
    Left = 672
    Top = 96
    Width = 11
    Height = 25
    Caption = '0'
  end
  object shMillBack: TShape
    Left = 576
    Top = 56
    Width = 100
    Height = 25
    Brush.Color = clNavy
  end
  object shMill: TShape
    Left = 576
    Top = 56
    Width = 100
    Height = 25
    Brush.Color = clRed
  end
  object lbForwardTrip0: TLabel
    Left = 392
    Top = 56
    Width = 41
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbForwardTrip1: TLabel
    Left = 392
    Top = 96
    Width = 41
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object lbForwardTrip2: TLabel
    Left = 392
    Top = 136
    Width = 41
    Height = 25
    Alignment = taRightJustify
    AutoSize = False
  end
  object Label9: TLabel
    Left = 512
    Top = 136
    Width = 86
    Height = 25
    Caption = #1058#1103#1078#1077#1083#1099#1093
  end
  object Label10: TLabel
    Left = 512
    Top = 176
    Width = 66
    Height = 25
    Caption = #1051#1077#1075#1082#1080#1093
  end
  object lbHeavyCount: TLabel
    Left = 616
    Top = 136
    Width = 11
    Height = 25
    Caption = '0'
  end
  object lbLightCount: TLabel
    Left = 616
    Top = 176
    Width = 11
    Height = 25
    Caption = '0'
  end
  object Label11: TLabel
    Left = 512
    Top = 216
    Width = 113
    Height = 25
    Caption = #1044#1086#1089#1090#1072#1074#1083#1077#1085#1086
  end
  object lbDelivered: TLabel
    Left = 640
    Top = 216
    Width = 11
    Height = 25
    Caption = '0'
  end
  object edSimulationTime: TEdit
    Left = 248
    Top = 224
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '480'
  end
  object btStart: TButton
    Left = 384
    Top = 224
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 1
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 272
    Width = 689
    Height = 273
    ActivePage = tsActions
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 2
    object tsActions: TTabSheet
      Caption = #1044#1077#1081#1089#1090#1074#1080#1103
      object sgService: TStringGrid
        Left = 8
        Top = 16
        Width = 665
        Height = 145
        ColCount = 9
        TabOrder = 0
      end
      object sgAction: TStringGrid
        Left = 8
        Top = 176
        Width = 665
        Height = 57
        ColCount = 6
        RowCount = 2
        TabOrder = 1
        ColWidths = (
          64
          64
          64
          64
          64
          64)
      end
    end
    object tsQueues: TTabSheet
      Caption = #1054#1095#1077#1088#1077#1076#1080
      ImageIndex = 1
      object sgQueue: TStringGrid
        Left = 8
        Top = 8
        Width = 665
        Height = 161
        ColCount = 6
        RowCount = 6
        TabOrder = 0
        RowHeights = (
          24
          24
          24
          24
          24
          24)
      end
    end
  end
  object tmQuarry: TTimer
    Enabled = False
    Interval = 100
    OnTimer = tmQuarryTimer
    Left = 360
    Top = 176
  end
end
