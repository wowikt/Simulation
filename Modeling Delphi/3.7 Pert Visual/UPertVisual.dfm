object frPert: TfrPert
  Left = 192
  Top = 114
  BorderIcons = [biSystemMenu, biMinimize]
  BorderStyle = bsSingle
  Caption = #1057#1077#1090#1100' PERT'
  ClientHeight = 606
  ClientWidth = 867
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
    Width = 215
    Height = 25
    Caption = #1050#1086#1083#1080#1095#1077#1089#1090#1074#1086' '#1087#1088#1086#1075#1086#1085#1086#1074':'
  end
  object edRunCount: TEdit
    Left = 248
    Top = 16
    Width = 121
    Height = 33
    TabOrder = 0
    Text = '400'
  end
  object btStart: TButton
    Left = 384
    Top = 16
    Width = 75
    Height = 33
    Caption = #1055#1091#1089#1082
    TabOrder = 1
    OnClick = btStartClick
  end
  object pgStat: TPageControl
    Left = 16
    Top = 64
    Width = 833
    Height = 529
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
      object sgStat: TStringGrid
        Left = 8
        Top = 16
        Width = 801
        Height = 161
        ColCount = 6
        RowCount = 6
        TabOrder = 0
      end
    end
    object tsHist1: TTabSheet
      Caption = #1059#1079#1077#1083' 1'
      ImageIndex = 1
      object dgHist1: TDrawGrid
        Tag = 1
        Left = 0
        Top = 0
        Width = 825
        Height = 489
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHist1DrawCell
        OnTopLeftChanged = dgHist1TopLeftChanged
      end
    end
    object tsHist2: TTabSheet
      Caption = #1059#1079#1077#1083' 2'
      ImageIndex = 2
      object dgHist2: TDrawGrid
        Tag = 2
        Left = 0
        Top = 0
        Width = 825
        Height = 495
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHist1DrawCell
        OnTopLeftChanged = dgHist1TopLeftChanged
      end
    end
    object tsHist3: TTabSheet
      Caption = #1059#1079#1077#1083' 3'
      ImageIndex = 3
      object dgHist3: TDrawGrid
        Tag = 3
        Left = 0
        Top = 0
        Width = 825
        Height = 497
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHist1DrawCell
        OnTopLeftChanged = dgHist1TopLeftChanged
      end
    end
    object tsHist4: TTabSheet
      Caption = #1059#1079#1077#1083' 4'
      ImageIndex = 4
      object dgHist4: TDrawGrid
        Tag = 4
        Left = 0
        Top = 0
        Width = 825
        Height = 495
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHist1DrawCell
        OnTopLeftChanged = dgHist1TopLeftChanged
      end
    end
    object tsHist5: TTabSheet
      Caption = #1047#1072#1074#1077#1088#1096#1077#1085#1080#1077' '#1088#1072#1073#1086#1090#1099
      ImageIndex = 5
      object dgHist5: TDrawGrid
        Tag = 5
        Left = 0
        Top = 0
        Width = 825
        Height = 495
        ColCount = 6
        FixedCols = 2
        TabOrder = 0
        OnDrawCell = dgHist1DrawCell
        OnTopLeftChanged = dgHist1TopLeftChanged
      end
    end
  end
end
