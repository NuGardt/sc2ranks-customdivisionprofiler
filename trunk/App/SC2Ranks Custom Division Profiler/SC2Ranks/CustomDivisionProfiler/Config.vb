' NuGardt SC2Ranks Custom Division Profiler
' Copyright (C) 2011-2013 NuGardt Software
' http://www.nugardt.com
'
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with this program.  If not, see <http://www.gnu.org/licenses/>.
'
Imports System.Runtime.Serialization
Imports System.ComponentModel
Imports System.IO
Imports NuGardt.Core
Imports NuGardt.SC2Ranks.API
Imports System.Xml
Imports System.Xml.Serialization

Namespace SC2Ranks.CustomDivisionProfiler
  <DataContract()>
  Friend Class Config
    Implements IConfig

#Region "Defaults"
    Private Const DefaultAutoDownload As Boolean = False
    Public Shared ReadOnly DefaultConfigPath As String = My.Application.Info.ProductName + ".xml"

    'Control
    Private Const DefaultAutoClose As Boolean = False
    Private Const DefaultDisableAutoSave As Boolean = False
    Public Const DefaultRetryCount As Int32 = 3
    Public Const DefaultRetryCountMin As Int32 = 0
    Public Const DefaultRetryCountMax As Int32 = 6
    Public Shared ReadOnly DefaultRetryWaitTime As TimeSpan = TimeSpan.FromSeconds(15)
    Public Const DefaultRetryWaitTimeString As String = "00:00:15"
    Public Shared ReadOnly DefaultRetryWaitTimeMin As TimeSpan = TimeSpan.FromSeconds(1)
    Public Const DefaultRetryWaitTimeMinString As String = "00:00:01"
    Public Shared ReadOnly DefaultRetryWaitTimeMax As TimeSpan = TimeSpan.FromSeconds(180)
    Public Const DefaultRetryWaitTimeMaxString As String = "00:03:00"
    Public Const DefaultDisableAutoOpen As Boolean = False
    Public Shared ReadOnly DefaultRequestIdleTime As TimeSpan = TimeSpan.FromSeconds(5)
    Public Const DefaultRequestIdleTimeString As String = "00:00:05"
    Public Shared ReadOnly DefaultRequestIdleTimeMin As TimeSpan = TimeSpan.FromSeconds(1)
    Public Const DefaultRequestIdleTimeMinString As String = "00:00:01"
    Public Shared ReadOnly DefaultRequestIdleTimeMax As TimeSpan = TimeSpan.FromSeconds(120)
    Public Const DefaultRequestIdleTimeMaxString As String = "00:02:00"

    'Settings
    Public Const DefaultCustomDescription As String = "My Custom Division"
    Public Const DefaultCustomDivisionID As String = "51e882c969d5386f5900cac4"

    'Files & Paths
    Public Const DefaultOutputFilename As String = "StarCraft 2 Rankings.htm"
    Public Const DefaultOutputFolder As String = "."
    Public Const DefaultTemplatePath As String = ""

    'Columns
    Private Const DefaultShowRegion As Boolean = True
    Private Const DefaultShowRank As Boolean = True
    Private Const DefaultShowRegionRank As Boolean = True
    Private Const DefaultShowWorldRank As Boolean = True
    Private Const DefaultShowLastUpdate As Boolean = True
    Private Const DefaultShowLosses As Boolean = True
    Private Const DefaultShowExpansion As Boolean = False
    Private Const DefaultShowWinLossRatio As Boolean = True
    Public Const DefaultAlwaysLinkPlayers As Boolean = True
    Public Const DefaultShowClanTag As Boolean = True
    Public Const DefaultShowFavouriteRace As Boolean = True

    'Expansion
    Public Const DefaultExpansion As eSc2RanksExpansion = eSc2RanksExpansion.HotS

    'Ignore Cache
    Public Const DefaultIgnoreCacheGetCustomDivision As Boolean = True
    Public Const DefaultIgnoreCacheGetBaseTeam As Boolean = False
    Public Const DefaultIgnoreCacheGetTeam As Boolean = False
#End Region

    'API
    Private m_ApiKey As String

    Private m_AutoDownload As Boolean
    Private m_Template As String
    Private m_ConfigPath As String

    'Control
    Private m_AutoClose As Boolean
    Private m_DisableAutoSave As Boolean
    Private m_RetryCount As Int32
    Private m_RetryWaitTime As TimeSpan
    Private m_DisableAutoOpen As Boolean
    Private m_RequestIdleTime As TimeSpan

    'Settings
    Private m_CustomDescription As String
    Private m_CustomDivisionID As String

    'Files & Paths
    Private m_OutputFileName As String
    Private m_OutputFolder As String
    Private m_TemplatePath As String

    'Columns
    Private m_ShowRegion As Boolean
    Private m_ShowRank As Boolean
    Private m_ShowRegionRank As Boolean
    Private m_ShowWorldRank As Boolean
    Private m_ShowLastUpdate As Boolean
    Private m_ShowLosses As Boolean
    Private m_ShowExpansion As Boolean
    Private m_ShowWinLossRatio As Boolean
    Private m_AlwaysLinkPlayers As Boolean
    Private m_ShowFavouriteRace As Boolean
    Private m_ShowClanTag As Boolean

    'Ranking Options
    Private m_Expansion As eSc2RanksExpansion
    Private m_Load1V1 As Boolean
    Private m_Load2V2T As Boolean
    Private m_Load2V2R As Boolean
    Private m_Load3V3T As Boolean
    Private m_Load3V3R As Boolean
    Private m_Load4V4T As Boolean
    Private m_Load4V4R As Boolean
    Private m_AchievementRankingOnlyWhenRanked As Boolean

    'Ignore Cache
    Private m_IgnoreCacheGetCustomDivision As Boolean
    Private m_IgnoreCacheGetBaseTeam As Boolean
    Private m_IgnoreCacheGetTeam As Boolean

    'Cache Durations
    Private m_GetCustomDivisionTeamListCacheDuration As TimeSpan
    Private m_GetCustomDivisionCacheDuration As TimeSpan

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
      Call Me.Reset()
    End Sub

    Public Sub Reset() Implements IConfig.Reset
      Me.m_ApiKey = Nothing

      Me.m_AutoDownload = DefaultAutoDownload
      Me.m_Template = My.Resources.Template
      Me.m_ConfigPath = Nothing

      'Control
      Me.m_AutoClose = DefaultAutoClose
      Me.m_DisableAutoSave = DefaultDisableAutoSave
      Me.m_RetryCount = DefaultRetryCount
      Me.m_RetryWaitTime = DefaultRetryWaitTime
      Me.m_DisableAutoOpen = DefaultDisableAutoOpen
      Me.m_RequestIdleTime = DefaultRequestIdleTime

      'Settings
      Me.m_CustomDescription = DefaultCustomDescription
      Me.m_CustomDivisionID = DefaultCustomDivisionID

      'Files & Path
      Me.m_OutputFileName = DefaultOutputFilename
      Me.m_OutputFolder = DefaultOutputFolder
      Me.m_TemplatePath = DefaultTemplatePath

      'Columns
      Me.m_ShowRegionRank = DefaultShowRegion
      Me.m_ShowRank = DefaultShowRank
      Me.m_ShowRegionRank = DefaultShowRegionRank
      Me.m_ShowWorldRank = DefaultShowWorldRank
      Me.m_ShowLastUpdate = DefaultShowLastUpdate
      Me.m_ShowExpansion = DefaultShowExpansion
      Me.m_ShowLosses = DefaultShowLosses
      Me.m_ShowWinLossRatio = DefaultShowWinLossRatio
      Me.m_AlwaysLinkPlayers = DefaultAlwaysLinkPlayers
      Me.m_ShowFavouriteRace = DefaultShowFavouriteRace
      Me.m_ShowClanTag = DefaultShowClanTag

      'Expansion
      Me.m_Expansion = DefaultExpansion
      Me.m_Load1V1 = True
      Me.m_Load2V2T = True
      Me.m_Load2V2R = True
      Me.m_Load3V3T = True
      Me.m_Load3V3R = True
      Me.m_Load4V4T = True
      Me.m_Load4V4R = True
      Me.m_AchievementRankingOnlyWhenRanked = True

      'Ignore Cache
      Me.m_IgnoreCacheGetCustomDivision = DefaultIgnoreCacheGetCustomDivision
      Me.m_IgnoreCacheGetBaseTeam = DefaultIgnoreCacheGetBaseTeam
      Me.m_IgnoreCacheGetTeam = DefaultIgnoreCacheGetTeam

      'Cache Duration
      Me.m_GetCustomDivisionTeamListCacheDuration = CacheConfig.DefaultGetCustomDivisionTeamListCacheDuration
      Me.m_GetCustomDivisionCacheDuration = CacheConfig.DefaultGetCustomDivisionCacheDuration
    End Sub

#Region "TryParse"

    Public Shared Function TryParse(ByVal cmd As CommandLine,
                                    ByRef Config As IConfig,
                                    Optional ByRef Ex As Exception = Nothing) As Boolean
      Ex = Nothing

      If cmd Is Nothing Then
        Ex = New ArgumentNullException("cmd")
      Else
        Dim sTemp As String = Nothing

        If cmd.IsSet("reset") Then
          Config = New Config
          Call Config.Reset()
          Return True
        End If

        If cmd.GetValues("config", sTemp) AndAlso Read(sTemp.Trim, Config) Then
          Config.ConfigPath = sTemp.Trim
        Else
          If (Not Read(DefaultConfigPath, Config)) Then Config = New Config
          Config.ConfigPath = DefaultConfigPath
        End If

        With Config
          If (cmd.GetValues("customdivisionid", sTemp) OrElse cmd.GetValues("cid", sTemp)) Then .CustomDivisionID = sTemp

          If cmd.IsSet("autodownload") OrElse cmd.IsSet("ad") Then
            .AutoDownload = True

            If cmd.IsSet("autoclose") OrElse cmd.IsSet("ac") Then .AutoClose = True
          End If

          If cmd.IsSet("disableautosave") OrElse cmd.IsSet("das") Then .DisableAutosave = True

          If cmd.GetValues("filename", sTemp) Then
            .OutputFileName = sTemp.Trim
          ElseIf String.IsNullOrEmpty(.OutputFileName) Then
            .OutputFileName = DefaultOutputFilename
          End If

          If cmd.GetValues("savepath", sTemp) And Directory.Exists(sTemp) Then
            .OutputFolder = sTemp.Trim
          ElseIf String.IsNullOrEmpty(.OutputFolder) Then
            .OutputFolder = "."
          End If

          If cmd.GetValues("templatepath", sTemp) Then
            Try
              Dim Stream As StreamReader

              Stream = New StreamReader(sTemp.Trim)

              .Template = Stream.ReadToEnd
              .TemplatePath = sTemp.Trim

              With Stream
                Call .Close()
                Call .Dispose()
              End With
            Catch iEx As Exception
              .Template = My.Resources.Template
              Ex = iEx
            End Try
          ElseIf String.IsNullOrEmpty(.Template) Then
            .Template = My.Resources.Template
            .TemplatePath = Nothing
          End If

          If cmd.GetValues("retrycount", sTemp) Then
            Dim iRetryCount As Int32 = Nothing

            If Integer.TryParse(sTemp, iRetryCount) AndAlso iRetryCount >= DefaultRetryCountMin AndAlso iRetryCount <= DefaultRetryCountMax Then
              .RetryCount = iRetryCount
            Else
              .RetryCount = DefaultRetryCount
            End If
          ElseIf .RetryCount < 0 AndAlso .RetryCount >= 6 Then
            .RetryCount = DefaultRetryCount
          End If

          If cmd.GetValues("retrywaittime", sTemp) Then
            Dim iRetryWaitTime As TimeSpan = Nothing

            If TimeSpan.TryParse(sTemp, iRetryWaitTime) AndAlso iRetryWaitTime >= DefaultRetryWaitTimeMin AndAlso iRetryWaitTime <= DefaultRetryWaitTimeMax Then
              .RetryWaitTime = iRetryWaitTime
            Else
              .RetryWaitTime = DefaultRetryWaitTime
            End If
          ElseIf .RetryWaitTime < DefaultRetryWaitTimeMin AndAlso .RetryWaitTime > DefaultRetryWaitTimeMax Then
            .RetryWaitTime = DefaultRetryWaitTime
          End If
        End With
      End If

      Return (Ex Is Nothing)
    End Function

    Private Shared Function Read(ByVal Path As String,
                                 ByRef Config As IConfig,
                                 Optional ByRef Ex As Exception = Nothing) As Boolean
      Ex = Nothing
      Config = Nothing

      If String.IsNullOrEmpty(Path) Then
        Ex = New ArgumentNullException("Path")
      ElseIf Not File.Exists(Path) Then
        Ex = New FileNotFoundException(Path)
      Else
        Try
          Dim Stream As Stream
          Dim XMLReader As XmlReader
          Dim iConfig As Config = Nothing

          Stream = New FileStream(Path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read)

          XMLReader = XMLReader.Create(Stream)

          If FromXml(XMLReader, iConfig, Ex) Then Config = iConfig

          With Stream
            Call .Close()
            Call .Dispose()
          End With
        Catch iEx As Exception
          Ex = iEx
        End Try
      End If

      Return (Ex Is Nothing)
    End Function

    Public Function ToXml(ByVal Stream As XmlWriter,
                          Optional ByRef Ex As Exception = Nothing) As Boolean Implements IConfig.ToXml
      Ex = Nothing

      If Stream Is Nothing Then
        Ex = New ArgumentNullException("Stream")
      Else
        Try
          Dim Serilizer As DataContractSerializer

          Serilizer = New DataContractSerializer(Me.GetType)
          Call Serilizer.WriteObject(Stream, Me)

        Catch iEx As Exception
          Ex = iEx
        End Try
      End If

      Return (Ex Is Nothing)
    End Function

    Public Shared Function FromXml(ByVal Stream As XmlReader,
                                   ByRef Config As Config,
                                   Optional ByRef Ex As Exception = Nothing) As Boolean
      Ex = Nothing
      Config = Nothing

      If Stream Is Nothing Then
        Ex = New ArgumentNullException("Stream")
      Else
        Try
          Dim Serializer As DataContractSerializer

          Serializer = New DataContractSerializer(GetType(Config))
          Config = DirectCast(Serializer.ReadObject(Stream), Config)

        Catch iEx As Exception
          Ex = iEx
        End Try
      End If

      Return (Ex Is Nothing)
    End Function

    Public Function iFromXML(ByVal Stream As XmlReader,
                             ByRef Config As Config,
                             Optional ByRef Ex As Exception = Nothing) As Boolean Implements IConfig.FromXml
      Return FromXml(Stream, Config, Ex)
    End Function

#End Region

#Region "Properties"

    <Browsable(False)>
    Public Property AutoDownload As Boolean Implements IConfig.AutoDownload
      Get
        Return Me.m_AutoDownload
      End Get
      Set(ByVal Value As Boolean)
        Me.m_AutoDownload = Value
      End Set
    End Property

    <Browsable(False)>
    Public Property Template As String Implements IConfig.Template
      Get
        Return Me.m_Template
      End Get
      Set(ByVal Value As String)
        Me.m_Template = Value
      End Set
    End Property

    <Browsable(False)>
    Public Property ConfigPath As String Implements IConfig.ConfigPath
      Get
        Return Me.m_ConfigPath
      End Get
      Set(ByVal Value As String)
        Me.m_ConfigPath = Value
      End Set
    End Property

#Region "API"

    <DataMember(Name := "ApiKey")>
    <DisplayName("API Key")>
    <Description("Your private SC2Ranks API Key.")>
    <Category("API")>
    Public Property ApiKey As String Implements IConfig.ApiKey
      Get
        Return Me.m_ApiKey
      End Get
      Set(ByVal Value As String)
        Me.m_ApiKey = Value
      End Set
    End Property

#End Region

#Region "Control"

    <DataMember(Name := "AutoClose")>
    <DefaultValue(DefaultAutoClose)>
    <DisplayName("Auto Close")>
    <Description("Automatically close the application when the download is complete. Only available when AutoDownload is enabled.")>
    <Category("Control")>
    Public Property AutoClose As Boolean Implements IConfig.AutoClose
      Get
        Return Me.m_AutoClose
      End Get
      Set(ByVal Value As Boolean)
        Me.m_AutoClose = Value
      End Set
    End Property

    <DataMember(Name := "DisableAutoSave")>
    <DefaultValue(DefaultDisableAutoSave)>
    <DisplayName("Disable autosaving")>
    <Description("Diables the automatic saving when a download completes.")>
    <Category("Control")>
    Public Property DisableAutosave As Boolean Implements IConfig.DisableAutosave
      Get
        Return Me.m_DisableAutoSave
      End Get
      Set(ByVal Value As Boolean)
        Me.m_DisableAutoSave = Value
      End Set
    End Property

    <DataMember(Name := "RetryCount")>
    <DefaultValue(DefaultRetryCount)>
    <DisplayName("Retry Count")>
    <Description("The number of times a command is retried. Valid Range: 0-6")>
    <Category("Control")>
    Public Property RetryCount As Int32 Implements IConfig.RetryCount
      Get
        Return Me.m_RetryCount
      End Get
      Set(ByVal Value As Int32)
        Me.m_RetryCount = Value

        If Value >= DefaultRetryCountMin AndAlso Value <= DefaultRetryCountMax Then
          Me.m_RetryCount = Value
        Else
          Me.m_RetryCount = DefaultRetryCount
        End If
      End Set
    End Property

    <DataMember(Name := "RetryWaitTime")>
    <DefaultValue(GetType(TimeSpan), DefaultRetryWaitTimeString)>
    <DisplayName("Retry wait time")>
    <Description("The retry wait time in seconds. Valid Range: 0-180")>
    <Category("Control")>
    Public Property RetryWaitTime As TimeSpan Implements IConfig.RetryWaitTime
      Get
        Return Me.m_RetryWaitTime
      End Get
      Set(ByVal Value As TimeSpan)
        If Value >= DefaultRetryWaitTimeMin AndAlso Value <= DefaultRetryWaitTimeMax Then
          Me.m_RetryWaitTime = Value
        Else
          Me.m_RetryWaitTime = DefaultRetryWaitTime
        End If
      End Set
    End Property

    <DataMember(Name := "DisableAutoOpen")>
    <DefaultValue(DefaultDisableAutoOpen)>
    <DisplayName("Disable auto open")>
    <Description("Disable the automatic opening after downloading.")>
    <Category("Control")>
    Public Property DisableAutOpen As Boolean Implements IConfig.DisableAutoOpen
      Get
        Return Me.m_DisableAutoOpen
      End Get
      Set(ByVal Value As Boolean)
        Me.m_DisableAutoOpen = Value
      End Set
    End Property

    <DataMember(Name := "RequestIdleTime")>
    <DefaultValue(GetType(TimeSpan), DefaultRequestIdleTimeString)>
    <DisplayName("Request Idle Time")>
    <Description("The time before sending another command to the SC2Ranks.com server.")>
    <Category("Control")>
    Public Property RequestIdleTime As TimeSpan Implements IConfig.RequestIdleTime
      Get
        Return Me.m_RequestIdleTime
      End Get
      Set(Value As TimeSpan)
        If Value >= DefaultRequestIdleTimeMin AndAlso Value <= DefaultRequestIdleTimeMax Then
          Me.m_RequestIdleTime = Value
        Else
          Me.m_RequestIdleTime = DefaultRequestIdleTime
        End If
      End Set
    End Property

#End Region

#Region "Settings"

    <DataMember(Name := "CustomDescription")>
    <DefaultValue(DefaultCustomDescription)>
    <DisplayName("Custom Description")>
    <Description("A custom description for the template.")>
    <Category("Settings")>
    Public Property CustomDescription As String Implements IConfig.CustomDescription
      Get
        Return Me.m_CustomDescription
      End Get
      Set(ByVal Value As String)
        Me.m_CustomDescription = Value
      End Set
    End Property

    <DataMember(Name := "CustomDivision")>
    <DefaultValue(DefaultCustomDivisionID)>
    <DisplayName("Custom Division ID")>
    <Description("The SC2Ranks.com Custom Division Identifier.")>
    <Category("Settings")>
    Public Property CustomDivisionID As String Implements IConfig.CustomDivisionID
      Get
        Return Me.m_CustomDivisionID
      End Get
      Set(ByVal Value As String)
        Me.m_CustomDivisionID = Value
      End Set
    End Property

#End Region

#Region "Files & Paths"

    <DataMember(Name := "OutputFileName")>
    <DefaultValue(DefaultOutputFilename)>
    <DisplayName("Output file name")>
    <Description("The output file name.")>
    <Category("Files & Paths")>
    Public Property OutputFileName As String Implements IConfig.OutputFileName
      Get
        Return Me.m_OutputFileName
      End Get
      Set(ByVal Value As String)

        If String.IsNullOrEmpty(Value) Then
          Me.m_OutputFileName = DefaultOutputFilename
        Else
          Me.m_OutputFileName = Value
        End If
      End Set
    End Property

    <DataMember(Name := "OutputFolder")>
    <DefaultValue(DefaultOutputFolder)>
    <DisplayName("Output Folder")>
    <Description("The output path. Set to '.' for executing directory.")>
    <Category("Files & Paths")>
    Public Property OutputFolder As String Implements IConfig.OutputFolder
      Get
        Return Me.m_OutputFolder
      End Get
      Set(ByVal Value As String)
        If String.IsNullOrEmpty(Value) Then
          Me.m_OutputFolder = "."
        Else
          Me.m_OutputFolder = Value
        End If
      End Set
    End Property

    <DataMember(Name := "TemplatePath")>
    <DefaultValue(DefaultTemplatePath)>
    <DisplayName("Template Path")>
    <Description("The path to a template file. Leave bank to use the internal template.")>
    <Category("Files & Paths")>
    Public Property TemplatePath As String Implements IConfig.TemplatePath
      Get
        Return Me.m_TemplatePath
      End Get
      Set(ByVal Value As String)

        If String.IsNullOrEmpty(Value) Then
          Me.m_Template = My.Resources.Template
          Me.m_TemplatePath = Nothing
        Else
          Try
            Dim Stream As StreamReader

            Stream = New StreamReader(Value)

            Me.m_Template = Stream.ReadToEnd
            Me.m_TemplatePath = Value

            With Stream
              Call .Close()
              Call .Dispose()
            End With
          Catch iEx As Exception
            Me.m_Template = My.Resources.Template
            Me.m_TemplatePath = Nothing
          End Try
        End If
      End Set
    End Property

#End Region

#Region "Columns"

    <DataMember(Name := "ShowRegion")>
    <DefaultValue(DefaultShowRegion)>
    <DisplayName("Show region")>
    <Description("Display the region code (EU, US, SEA or KR).")>
    <Category("Columns")>
    Public Property ShowRegion As Boolean Implements IConfig.ShowRegion
      Get
        Return Me.m_ShowRegion
      End Get
      Set(ByVal Value As Boolean)
        Me.m_ShowRegion = Value
      End Set
    End Property

    <DataMember(Name := "ShowRank")>
    <DefaultValue(DefaultShowRank)>
    <DisplayName("Show custom division rank")>
    <Description("Display the custom division rank column.")>
    <Category("Columns")>
    Public Property ShowRank As Boolean Implements IConfig.ShowRank
      Get
        Return Me.m_ShowRank
      End Get
      Set(ByVal Value As Boolean)
        Me.m_ShowRank = Value
      End Set
    End Property

    <DataMember(Name := "ShowRegionRank")>
    <DefaultValue(DefaultShowRegionRank)>
    <DisplayName("Show region rank")>
    <Description("Display the region rank column.")>
    <Category("Columns")>
    Public Property ShowRegionRank As Boolean Implements IConfig.ShowRegionRank
      Get
        Return Me.m_ShowRegionRank
      End Get
      Set(ByVal Value As Boolean)
        Me.m_ShowRegionRank = Value
      End Set
    End Property

    <DataMember(Name := "ShowWorldRank")>
    <DefaultValue(DefaultShowWorldRank)>
    <DisplayName("Show world rank")>
    <Description("Display the world rank column.")>
    <Category("Columns")>
    Public Property ShowWorldRank As Boolean Implements IConfig.ShowWorldRank
      Get
        Return Me.m_ShowWorldRank
      End Get
      Set(ByVal Value As Boolean)
        Me.m_ShowWorldRank = Value
      End Set
    End Property

    <DataMember(Name := "ShowLastUpdate")>
    <DefaultValue(DefaultShowLastUpdate)>
    <DisplayName("Show last update")>
    <Description("-")>
    <Category("Columns")>
    Public Property ShowLastUpdate As Boolean Implements IConfig.ShowLastUpdate
      Get
        Return Me.m_ShowLastUpdate
      End Get
      Set(Value As Boolean)
        Me.m_ShowLastUpdate = Value
      End Set
    End Property

    <DataMember(Name := "ShowExpansion")>
    <DefaultValue(DefaultShowExpansion)>
    <DisplayName("Show expansion")>
    <Description("Show the name of the expansion for that entry.")>
    <Category("Columns")>
    Public Property ShowExpansion As Boolean Implements IConfig.ShowExpansion
      Get
        Return Me.m_ShowExpansion
      End Get
      Set(Value As Boolean)
        Me.m_ShowExpansion = Value
      End Set
    End Property

    <DataMember(Name := "ShowLosses")>
    <DefaultValue(DefaultShowLosses)>
    <DisplayName("Show losses")>
    <Description("-")>
    <Category("Columns")>
    Public Property ShowLosses As Boolean Implements IConfig.ShowLosses
      Get
        Return Me.m_ShowLosses
      End Get
      Set(Value As Boolean)
        Me.m_ShowLosses = Value
      End Set
    End Property

    <DataMember(Name := "ShowWinLossRatio")>
    <DefaultValue(DefaultShowWinLossRatio)>
    <DisplayName("Show win/loss ratio")>
    <Description("-")>
    <Category("Columns")>
    Public Property ShowWinLossRatio As Boolean Implements IConfig.ShowWinLossRatio
      Get
        Return Me.m_ShowWinLossRatio
      End Get
      Set(Value As Boolean)
        Me.m_ShowWinLossRatio = Value
      End Set
    End Property

    <DataMember(Name := "AlwaysLinkPlayers")>
    <DefaultValue(DefaultAlwaysLinkPlayers)>
    <DisplayName("Always link players")>
    <Description("Always link players to SC2Ranks.com in the rankings.")>
    <Category("Columns")>
    Public Property AlwaysLinkPlayers As Boolean Implements IConfig.AlwaysLinkPlayers
      Get
        Return Me.m_AlwaysLinkPlayers
      End Get
      Set(Value As Boolean)
        Me.m_AlwaysLinkPlayers = Value
      End Set
    End Property

    <DataMember(Name := "ShowFavouriteRace")>
    <DefaultValue(DefaultShowFavouriteRace)>
    <DisplayName("Show favourite race")>
    <Description("Show the race icon to the left of the player name.")>
    <Category("Columns")>
    Public Property ShowFavouriteRace As Boolean Implements IConfig.ShowFavouriteRace
      Get
        Return Me.m_ShowFavouriteRace
      End Get
      Set(ByVal Value As Boolean)
        Me.m_ShowFavouriteRace = Value
      End Set
    End Property

    <DataMember(Name := "ShowClanTag")>
    <DefaultValue(DefaultShowClanTag)>
    <DisplayName("Show clan tag")>
    <Description("Show the clan tag to the left of the player name.")>
    <Category("Columns")>
    Public Property ShowClanTag As Boolean Implements IConfig.ShowClanTag
      Get
        Return Me.m_ShowClanTag
      End Get
      Set(ByVal Value As Boolean)
        Me.m_ShowClanTag = Value
      End Set
    End Property

#End Region

#Region "Ranking Options"

    <DataMember(Name := "Expansion")>
    <DefaultValue(DefaultExpansion)>
    <DisplayName("Expansion")>
    <Description("Choose the expansion.")>
    <Category("Ranking Options")>
    Public Property Expansion As eSc2RanksExpansion Implements IConfig.Expansion
      Get
        Return Me.m_Expansion
      End Get
      Set(Value As eSc2RanksExpansion)
        Me.m_Expansion = Value
      End Set
    End Property

    <DataMember(Name := "AchievementRankingOnlyWhenRanked")>
    <DefaultValue(True)>
    <DisplayName("Achievement Ranking Only When Ranked")>
    <Description("A character only gets ranked in achievements when ranked in at least 1 selected bracket.")>
    <Category("Ranking Options")>
    Public Property AchievementRankingOnlyWhenRanked As Boolean Implements IConfig.AchievementRankingOnlyWhenRanked
      Get
        Return Me.m_AchievementRankingOnlyWhenRanked
      End Get
      Set(ByVal Value As Boolean)
        Me.m_AchievementRankingOnlyWhenRanked = Value
      End Set
    End Property

    <DataMember(Name := "Load1v1Bracket")>
    <DefaultValue(True)>
    <DisplayName("Load 1v1 Bracket")>
    <Description("Load data for 1v1 bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load1V1 As Boolean Implements IConfig.Load1V1
      Get
        Return Me.m_Load1V1
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load1V1 = Value
      End Set
    End Property

    <DataMember(Name := "Load2v2RBracket")>
    <DefaultValue(True)>
    <DisplayName("Load 2v2R Bracket")>
    <Description("Load data for 2v2 random team bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load2V2R As Boolean Implements IConfig.Load2V2R
      Get
        Return Me.m_Load2V2R
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load2V2R = Value
      End Set
    End Property

    <DataMember(Name := "Load2v2TBracket")>
    <DefaultValue(True)>
    <DisplayName("Load 2v2T Bracket")>
    <Description("Load data for 2v2 team bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load2V2T As Boolean Implements IConfig.Load2V2T
      Get
        Return Me.m_Load2V2T
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load2V2T = Value
      End Set
    End Property

    <DataMember(Name := "Load3v3RBracket")>
    <DefaultValue(True)>
    <DisplayName("Load 3v3R Bracket")>
    <Description("Load data for 3v3 random team bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load3V3R As Boolean Implements IConfig.Load3V3R
      Get
        Return Me.m_Load3V3R
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load3V3R = Value
      End Set
    End Property

    <DataMember(Name := "Load3v3TBracket")>
    <DefaultValue(True)>
    <DisplayName("Load 3v3T Bracket")>
    <Description("Load data for 3v3 team bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load3V3T As Boolean Implements IConfig.Load3V3T
      Get
        Return Me.m_Load3V3T
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load3V3T = Value
      End Set
    End Property

    <DataMember(Name := "Load4v4RBracket")>
    <DefaultValue(True)>
    <DisplayName("Load 4v4R Bracket")>
    <Description("Load data for 4v4 random team bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load4V4R As Boolean Implements IConfig.Load4V4R
      Get
        Return Me.m_Load4V4R
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load4V4R = Value
      End Set
    End Property

    <DataMember(Name := "Load4v4TBracket")>
    <DefaultValue(True)>
    <DisplayName("Load 4v4T Bracket")>
    <Description("Load data for 4v4 team bracket ranking.")>
    <Category("Ranking Options")>
    Public Property Load4V4T As Boolean Implements IConfig.Load4V4T
      Get
        Return Me.m_Load4V4T
      End Get
      Set(ByVal Value As Boolean)
        Me.m_Load4V4T = Value
      End Set
    End Property

#End Region

#Region "Ignore Cache"

    <DataMember(Name := "IgnoreCacheGetCustomDivision")>
    <DefaultValue(DefaultIgnoreCacheGetCustomDivision)>
    <DisplayName("Get Custom Division")>
    <Description("Ignore cache information and redownload the members of the custom division. Useful when members were added or removed.")>
    <Category("Ignore Cache")>
    Public Property IgnoreCacheGetCustomDivision As Boolean Implements IConfig.IgnoreCacheGetCustomDivision
      Get
        Return Me.m_IgnoreCacheGetCustomDivision
      End Get
      Set(ByVal Value As Boolean)
        Me.m_IgnoreCacheGetCustomDivision = Value
      End Set
    End Property

    <DataMember(Name := "IgnoreCacheGetBaseTeam")>
    <DefaultValue(DefaultIgnoreCacheGetBaseTeam)>
    <DisplayName("Get Player")>
    <Description("Ignore cache information and redownload player information (achievement points, clan tag, etc.).")>
    <Category("Ignore Cache")>
    Public Property IgnoreCacheGetBaseTeam As Boolean Implements IConfig.IgnoreCacheGetBaseTeam
      Get
        Return Me.m_IgnoreCacheGetBaseTeam
      End Get
      Set(ByVal Value As Boolean)
        Me.m_IgnoreCacheGetBaseTeam = Value
      End Set
    End Property

    <DataMember(Name := "IgnoreCacheGetTeam")>
    <DefaultValue(DefaultIgnoreCacheGetTeam)>
    <DisplayName("Get Team")>
    <Description("Ignore cache information and redownload team information (1v1, 2v2, 2v2R, etc.).")>
    <Category("Ignore Cache")>
    Public Property IgnoreCacheGetTeam As Boolean Implements IConfig.IgnoreCacheGetTeam
      Get
        Return Me.m_IgnoreCacheGetTeam
      End Get
      Set(ByVal Value As Boolean)
        Me.m_IgnoreCacheGetTeam = Value
      End Set
    End Property

#End Region

#Region "Cache Durations"

    <XmlIgnore()>
    <Browsable(False)>
    Private Property CustomDivisionAddCacheDuration As TimeSpan Implements ICacheConfig.CustomDivisionAddCacheDuration
      Get
        Return CacheConfig.DefaultCustomDivisionAddCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property CustomDivisionRemoveCacheDuration As TimeSpan Implements ICacheConfig.CustomDivisionRemoveCacheDuration
      Get
        Return CacheConfig.DefaultCustomDivisionRemoveCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetBaseDataCacheDuration As TimeSpan Implements ICacheConfig.GetBaseDataCacheDuration
      Get
        Return CacheConfig.DefaultGetBaseDataCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetCharacterCacheDuration As TimeSpan Implements ICacheConfig.GetCharacterCacheDuration
      Get
        Return CacheConfig.DefaultGetCharacterCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetCharacterListCacheDuration As TimeSpan Implements ICacheConfig.GetCharacterListCacheDuration
      Get
        Return CacheConfig.DefaultGetCharacterListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetCharacterTeamListCacheDuration As TimeSpan Implements ICacheConfig.GetCharacterTeamListCacheDuration
      Get
        Return CacheConfig.DefaultGetCharacterTeamListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetCharacterTeamsListCacheDuration As TimeSpan Implements ICacheConfig.GetCharacterTeamsListCacheDuration
      Get
        Return CacheConfig.DefaultGetCharacterTeamsListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetClanCacheDuration As TimeSpan Implements ICacheConfig.GetClanCacheDuration
      Get
        Return CacheConfig.DefaultGetClanCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetClanCharacterListCacheDuration As TimeSpan Implements ICacheConfig.GetClanCharacterListCacheDuration
      Get
        Return CacheConfig.DefaultGetClanCharacterListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetClanTeamListCacheDuration As TimeSpan Implements ICacheConfig.GetClanTeamListCacheDuration
      Get
        Return CacheConfig.DefaultGetClanTeamListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <DataMember(Name := "GetCustomDivisionCacheDuration")>
    <DefaultValue(GetType(TimeSpan), CacheConfig.DefaultGetCustomDivisionCacheDurationString)>
    <DisplayName("Get Custom Division Cache Duration")>
    <Description("")>
    <Category("Cache Duration")>
    Public Property GetCustomDivisionCacheDuration As TimeSpan Implements ICacheConfig.GetCustomDivisionCacheDuration
      Get
        Return Me.m_GetCustomDivisionCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        Me.m_GetCustomDivisionCacheDuration = Value
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetCustomDivisionCharacterListCacheDuration As TimeSpan Implements ICacheConfig.GetCustomDivisionCharacterListCacheDuration
      Get
        Return CacheConfig.DefaultGetCustomDivisionCharacterListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetCustomDivisionsCacheDuration As TimeSpan Implements ICacheConfig.GetCustomDivisionsCacheDuration
      Get
        Return CacheConfig.DefaultGetCustomDivisionsCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <DataMember(Name := "GetCustomDivisionTeamListCacheDuration")>
    <DefaultValue(GetType(TimeSpan), CacheConfig.DefaultGetCustomDivisionTeamListCacheDurationString)>
    <DisplayName("Get Team (Custom Division) Duration")>
    <Description("")>
    <Category("Cache Duration")>
    Public Property GetCustomDivisionTeamListCacheDuration As TimeSpan Implements ICacheConfig.GetCustomDivisionTeamListCacheDuration
      Get
        Return Me.m_GetCustomDivisionTeamListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        Me.m_GetCustomDivisionTeamListCacheDuration = Value
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetDivisionCacheDuration As TimeSpan Implements ICacheConfig.GetDivisionCacheDuration
      Get
        Return CacheConfig.DefaultGetDivisionCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetDivisionsTopCacheDuration As TimeSpan Implements ICacheConfig.GetDivisionsTopCacheDuration
      Get
        Return CacheConfig.DefaultGetDivisionsTopCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetDivisionTeamsTopCacheDuration As TimeSpan Implements ICacheConfig.GetDivisionTeamsTopCacheDuration
      Get
        Return CacheConfig.DefaultGetDivisionTeamsTopCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property GetRankingsTopCacheDuration As TimeSpan Implements ICacheConfig.GetRankingsTopCacheDuration
      Get
        Return CacheConfig.DefaultGetRankingsTopCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

    <XmlIgnore()>
    <Browsable(False)>
    Private Property SearchCharacterTeamListCacheDuration As TimeSpan Implements ICacheConfig.SearchCharacterTeamListCacheDuration
      Get
        Return CacheConfig.DefaultSearchCharacterTeamListCacheDuration
      End Get
      Set(ByVal Value As TimeSpan)
        '-
      End Set
    End Property

#End Region

#End Region
  End Class
End Namespace