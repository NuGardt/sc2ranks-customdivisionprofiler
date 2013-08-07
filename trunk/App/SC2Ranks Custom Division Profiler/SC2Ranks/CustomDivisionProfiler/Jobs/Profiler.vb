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
Imports System.Runtime.InteropServices
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports NuGardt.SC2Ranks.Helper
Imports NuGardt.SC2Ranks.API.Result.Element
Imports NuGardt.SC2Ranks.API
Imports NuGardt.Core
Imports NuGardt.Core.Process
Imports NuGardt.SC2Ranks.API.Result
Imports System.Security.Principal

Namespace SC2Ranks.CustomDivisionProfiler.Jobs
  Friend Class Profiler(Of TKey)
    Inherits Job(Of TKey)

    Private ReadOnly m_ReportProgress As procReportProgress
    Private ReadOnly m_OnComplete As procOnComplete

    Private Const CellSpacing As Int32 = 2
    Private Const CellPadding As Int32 = 0

    Private Const HeaderRank As String = "#"
    Private Const HeaderLastUpdate As String = "Last Update"
    Private Const HeaderExpansion As String = "Expan."
    Private Const HeaderPoints As String = "Points"
    Private Const HeaderPlayer As String = "Player"
    Private Const HeaderWins As String = "Wins"
    Private Const HeaderLosses As String = "Losses"
    Private Const HeaderWinLossRatio As String = "W/L"
    Private Const Header1V1 As String = "1v1"
    Private Const Header2V2 As String = "2v2"
    Private Const Header3V3 As String = "3v3"
    Private Const Header4V4 As String = "4v4"
    Private Const HeaderLeague As String = "League"
    Private Const HeaderDivisionRank As String = "Rank"
    Private Const HeaderRegion As String = "Re."
    Private Const HeaderRegionRank As String = "Region"
    Private Const HeaderWorldRank As String = "World"

#Region "CSS Classes"
    Public Const CssHeader As String = "header"
    Public Const CssAlternateRow0 As String = "alternate-0"
    Public Const CssAlternateRow1 As String = "alternate-1"

    Public Const CssPlayerName As String = "player-name"
    Public Const CssPlayerSelfName As String = "player-name-"
    Public Const CssClanTag As String = "clan-tag"
    Public Const CssClanSelfTag As String = "clan-tag-"
    Public Const CssClanTagEmpty As String = "clan-tag_empty"

    Public Const CssExpansionHeader As String = "expansion-header"
    Public Const CssExpansionCell As String = "expansion-cell"
    Public Const CssRankHeader As String = "rank-header"
    Public Const CssRankCell As String = "rank-cell"
    Public Const CssLastUpdateHeader As String = "lastupdate-header"
    Public Const CssLastUpdateCell As String = "lastupdate-cell"
    Public Const CssRegionHeader As String = "region-header"
    Public Const CssRegionCell As String = "region-cell"
    Public Const CssRegionRankHeader As String = "regionrank-header"
    Public Const CssRegionRankCell As String = "regionrank-cell"
    Public Const CssWorldRankHeader As String = "worldrank-header"
    Public Const CssWorldRankCell As String = "worldrank-cell"
    Public Const CssLeagueHeader As String = "league-header"
    Public Const CssLeagueCell As String = "league-cell"
    Public Const CssDivisionRankHeader As String = "division-rank-header"
    Public Const CssDivisionRankCell As String = "division-rank-cell"
    Public Const CssPointsHeader As String = "points-header"
    Public Const CssPointsCell As String = "points-cell"
    Public Const CssPlayerHeader As String = "player-header"
    Public Const CssPlayerCell As String = "player-cell"
    Public Const CssWinsHeader As String = "wins-header"
    Public Const CssWinsCell As String = "wins-cell"
    Public Const CssLossesHeader As String = "losses-header"
    Public Const CssLossesCell As String = "losses-cell"
    Public Const CssWinLossRatioHeader As String = "winlossratio-header"
    Public Const CssWinLossRatioCell As String = "winlossratio-cell"

    Public Const CssEmptyCell As String = "empty-cell"
#End Region

    Friend ReadOnly Config As IConfig

    Private BuilderAchievements As HtmlStringBuilder
    Private Builder1V1 As HtmlStringBuilder
    Private Builder2V2 As HtmlStringBuilder
    Private Builder3V3 As HtmlStringBuilder
    Private Builder4V4 As HtmlStringBuilder
    Private GeneratedInSeconds As Double

    Public CreditsUsed As Int32

    Public Output As String

    Public Sub New(ByVal ProcessTag As TKey,
                   ByVal Config As IConfig,
                   ByVal ReportProgress As procReportProgress,
                   ByVal OnComplete As procOnComplete)
      Call MyBase.New(ProcessTag)
      Me.Config = Config
      Me.m_ReportProgress = ReportProgress
      Me.m_OnComplete = OnComplete

      Me.BuilderAchievements = New HtmlStringBuilder
      Me.Builder1V1 = New HtmlStringBuilder
      Me.Builder2V2 = New HtmlStringBuilder
      Me.Builder3V3 = New HtmlStringBuilder
      Me.Builder4V4 = New HtmlStringBuilder
      Me.GeneratedInSeconds = 0
      Me.CreditsUsed = 0

      Me.Output = Nothing
    End Sub

    Public Overrides Sub Dispose2()
      Me.BuilderAchievements = Nothing
      Me.Builder1V1 = Nothing
      Me.Builder2V2 = Nothing
      Me.Builder3V3 = Nothing
      Me.Builder4V4 = Nothing
      Me.GeneratedInSeconds = Nothing
    End Sub

    Public Overrides Sub Run()
      Dim Ex As Exception = Nothing

      If Me.GenerateProfile(BuilderAchievements, Builder1V1, Builder2V2, Builder3V3, Builder4V4, GeneratedInSeconds, Me.m_ReportProgress, CreditsUsed, Ex) Then
        Me.Output = Me.Config.Template.Replace("$achievement$", BuilderAchievements.ToString).Replace("$1v1$", Builder1V1.ToString).Replace("$2v2$", Builder2V2.ToString).Replace("$3v3$", Builder3V3.ToString).Replace("$4v4$", Builder4V4.ToString).Replace("$version$", String.Format("v{0}", My.Application.Info.Version)).Replace("$customdescription$", Me.Config.CustomDescription).Replace("$generatedby$", WindowsIdentity.GetCurrent.Name).Replace("$generatedin$", GeneratedInSeconds.ToString("N3")).Replace("$timestamp$", Date.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff") + " UTC")
      End If

      If Ex IsNot Nothing Then Me.SetError = Ex

      Call Me.m_OnComplete.Invoke(Me)
    End Sub

    Private Function GenerateProfile(ByRef SbAchievements As HtmlStringBuilder,
                                     ByRef Sb1V1 As HtmlStringBuilder,
                                     ByRef Sb2V2 As HtmlStringBuilder,
                                     ByRef Sb3V3 As HtmlStringBuilder,
                                     ByRef Sb4V4 As HtmlStringBuilder,
                                     ByRef GeneratedInSeconds As Double,
                                     ByVal ReportProgress As procReportProgress,
                                     <Out> CreditsUsed As Int32,
                                     <Out> ByRef Ex As Exception) As Boolean
      CreditsUsed = 0
      Ex = Nothing

      If SbAchievements Is Nothing Then
        Ex = New ArgumentNullException("SbAchievements")
      ElseIf Sb1V1 Is Nothing Then
        Ex = New ArgumentNullException("Sb1V1")
      ElseIf Sb2V2 Is Nothing Then
        Ex = New ArgumentNullException("Sb2V2")
      ElseIf Sb3V3 Is Nothing Then
        Ex = New ArgumentNullException("Sb3V3")
      ElseIf Sb4V4 Is Nothing Then
        Ex = New ArgumentNullException("Sb4V4")
      ElseIf Me.TestNotDisposed(Ex) Then
        Dim StarTime As Date
        Dim EndTime As Date
        Dim CssClass As String = Nothing
        Dim RankService As Sc2RanksService = Nothing
        Dim CacheStream As Stream

        StarTime = Date.UtcNow

        Try
          'Try to open file log
          CacheStream = New FileStream("cache.blob", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read)
        Catch
          CacheStream = Nothing
        End Try

        Try
          Dim CustomDivisionPlayers As New SortedDictionary(Of PlayerKey, Player)
          Dim listPlayers As New List(Of Player)
          Dim dictLeague1v1 As New SortedList(Of String, Team)
          Dim listLeague1v1 As New List(Of Team)
          Dim dictLeague2v2 As New SortedList(Of String, Team)
          Dim listLeague2v2 As New List(Of Team)
          Dim dictLeague3v3 As New SortedList(Of String, Team)
          Dim listLeague3v3 As New List(Of Team)
          Dim dictLeague4v4 As New SortedList(Of String, Team)
          Dim listLeague4v4 As New List(Of Team)

          Dim Player As Player = Nothing
          Dim dMax As Int32
          Dim iMax As Int32
          Dim [Try] As Int32 = 0
          Dim IsAlternateRow As Boolean
          Dim AlternateText As String = Nothing

          If ReportProgress IsNot Nothing Then Call ReportProgress.Invoke(Me, 0, eProgressBarState.Normal)

          Dim Brackets As New List(Of eSc2RanksBracket)

          If Me.Config.Load1V1 Then Call Brackets.Add(eSc2RanksBracket._1V1)
          If Me.Config.Load2V2T Then Call Brackets.Add(eSc2RanksBracket._2V2T)
          If Me.Config.Load2V2R Then Call Brackets.Add(eSc2RanksBracket._2V2R)
          If Me.Config.Load3V3T Then Call Brackets.Add(eSc2RanksBracket._3V3T)
          If Me.Config.Load3V3R Then Call Brackets.Add(eSc2RanksBracket._3V3R)
          If Me.Config.Load4V4T Then Call Brackets.Add(eSc2RanksBracket._4V4T)
          If Me.Config.Load4V4R Then Call Brackets.Add(eSc2RanksBracket._4V4R)

          If (Brackets.Count = 0) Then
            Ex = New Exception("No to load. No brackets selected.")
            Exit Try
          End If

          Ex = Sc2RanksService.CreateInstance(Config.ApiKey, RankService, CacheStream, Me.Config)

          If (Ex IsNot Nothing) Then Exit Try

          'Get Custom Division and enumerate all members
          Call Trace.WriteLine("Downloading custom division data...")

          Dim GCDRList As IList(Of Sc2RanksGetCustomDivisionCharacterListResult) = Nothing
          Ex = ExecutePagedCall(Of GetCustomDivisionPagedCall, Sc2RanksGetCustomDivisionCharacterListResult)(Me.ProcessTag, 0, New GetCustomDivisionPagedCall(RankService, Config.CustomDivisionID, Me.Config.IgnoreCacheGetCustomDivision), 50, [Try], Me.Config.RetryCount, Me.Config.RetryWaitTime, ReportProgress, OnCancelPending, GCDRList)
          If (Ex IsNot Nothing) Then Exit Try

          With GCDRList.GetEnumerator()
            Call .Reset()

            Do While .MoveNext()
              Dim Character As Sc2RanksCharacterExtended

              CreditsUsed += .Current.CreditsUsed

              dMax = .Current.Characters.Length - 1
              For d As Int32 = 0 To dMax
                Character = .Current.Characters(d)

                'Only list a player once incase of duplicate entries.
                Player = New Player(Character)
                If (Not CustomDivisionPlayers.ContainsKey(Player)) Then
                  Call CustomDivisionPlayers.Add(Player, Player)
                  Call listPlayers.Add(Player)
                End If
              Next d
            Loop

            Call .Dispose()
          End With

          Dim TotalCalls As Int32 = Brackets.Count
          Dim CallCount As Int32 = 0

          For Each Bracket In Brackets
            Call Trace.WriteLine(String.Format("Downloading team(s) of custom division {0}.", Enums.BracketNotationBuffer.GetValue(Bracket)))

            Dim CDTList As IList(Of Sc2RanksGetCustomDivisionTeamListResult) = Nothing
            Ex = ExecutePagedCall(Of GetCustomDivisionTeamsPagedCall, Sc2RanksGetCustomDivisionTeamListResult)(Me.ProcessTag, (CallCount / TotalCalls) * 100, New GetCustomDivisionTeamsPagedCall(RankService, Me.Config.CustomDivisionID, Me.Config.Expansion, Bracket, Me.Config.IgnoreCacheGetCustomDivisionTeamList), 10, [Try], Me.Config.RetryCount, Me.Config.RetryWaitTime, ReportProgress, OnCancelPending, CDTList)
            If (Ex IsNot Nothing) Then Exit Try

            CallCount += 1
            If ReportProgress IsNot Nothing Then Call ReportProgress.Invoke(Me, (CallCount / TotalCalls) * 100, eProgressBarState.Normal)

            Dim Team As Team
            Dim PlayerKey As PlayerKey

            With CDTList.GetEnumerator()
              Call .Reset()

              Do While .MoveNext()
                CreditsUsed += .Current.CreditsUsed

                dMax = .Current.Teams.Length - 1
                For d As Int32 = 0 To dMax
                  Team = New Team(.Current.Teams(d))

                  If (Team.Team.Division IsNot Nothing) Then
                    iMax = Team.Team.Characters.Count() - 1
                    For i As Int32 = 0 To iMax
                      With Team.Team.Characters(i)
                        PlayerKey = New PlayerKey(.BattleNetID, .Region)
                      End With

                      If CustomDivisionPlayers.TryGetValue(PlayerKey, Player) Then
                        If (Team.Team.Characters.Length > 0) AndAlso (Player.RaceFromBracket >= Team.Team.Bracket) Then
                          Player.RaceFromBracket = Team.Team.Bracket
                          Player.FavouriteRace = Team.Team.Characters(0).Race
                        End If

                        Select Case Team.Team.Bracket
                          Case eSc2RanksBracket._1V1
                            If Team.Team.League = Player.Highest1V1League Then
                              If Team.DivisionRank < Player.Highest1V1Rank Then Player.Highest1V1Rank = Team.DivisionRank
                            ElseIf Team.Team.League > Player.Highest1V1League Then
                              Player.Highest1V1League = Team.Team.League
                              Player.Highest1V1Rank = Team.DivisionRank
                            End If

                            If (Not dictLeague1v1.ContainsKey(Team.Team.Division.ID)) Then
                              Call dictLeague1v1.Add(Team.Team.Division.ID, Team)
                              Call listLeague1v1.Add(Team)
                            End If
                          Case eSc2RanksBracket._2V2T, eSc2RanksBracket._2V2R
                            If Team.Team.League = Player.Highest2V2League Then
                              If Team.DivisionRank < Player.Highest2V2Rank Then Player.Highest2V2Rank = Team.DivisionRank
                            ElseIf Team.Team.League > Player.Highest2V2League Then
                              Player.Highest2V2League = Team.Team.League
                              Player.Highest2V2Rank = Team.DivisionRank
                            End If

                            If (Not dictLeague2v2.ContainsKey(Team.Team.Division.ID)) Then
                              Call dictLeague2v2.Add(Team.Team.Division.ID, Team)
                              Call listLeague2v2.Add(Team)
                            End If
                          Case eSc2RanksBracket._3V3T, eSc2RanksBracket._3V3R
                            If Team.Team.League = Player.Highest3V3League Then
                              If Team.DivisionRank < Player.Highest3V3Rank Then Player.Highest3V3Rank = Team.DivisionRank
                            ElseIf Team.Team.League > Player.Highest3V3League Then
                              Player.Highest3V3League = Team.Team.League
                              Player.Highest3V3Rank = Team.DivisionRank
                            End If

                            If (Not dictLeague3v3.ContainsKey(Team.Team.Division.ID)) Then
                              Call dictLeague3v3.Add(Team.Team.Division.ID, Team)
                              Call listLeague3v3.Add(Team)
                            End If
                          Case eSc2RanksBracket._4V4T, eSc2RanksBracket._4V4R
                            If Team.Team.League = Player.Highest4V4League Then
                              If Team.DivisionRank < Player.Highest4V4Rank Then Player.Highest4V4Rank = Team.DivisionRank
                            ElseIf Team.Team.League > Player.Highest4V4League Then
                              Player.Highest4V4League = Team.Team.League
                              Player.Highest4V4Rank = Team.DivisionRank
                            End If

                            If (Not dictLeague4v4.ContainsKey(Team.Team.Division.ID)) Then
                              Call dictLeague4v4.Add(Team.Team.Division.ID, Team)
                              Call listLeague4v4.Add(Team)
                            End If
                        End Select
                      End If
                    Next i
                  End If
                Next d
              Loop

              Call .Dispose()
            End With
          Next Bracket

          Call RankService.Dispose()

          Call Trace.WriteLine("Sorting downloaded data...")

          Call listPlayers.Sort(AddressOf Player.CompareByAchievementPointsInverted)
          Call listLeague1v1.Sort(AddressOf Team.CompareByLeagueRegionRankInverted)
          Call listLeague2v2.Sort(AddressOf Team.CompareByLeagueRegionRankInverted)
          Call listLeague3v3.Sort(AddressOf Team.CompareByLeagueRegionRankInverted)
          Call listLeague4v4.Sort(AddressOf Team.CompareByLeagueRegionRankInverted)

          Call Trace.WriteLine("Constructing HTML output...")

          Call SbAchievements.OpenTable(CellSpacing := CellSpacing, CellPadding := CellPadding)

          Call SbAchievements.OpenRow()

          If Config.ShowRank Then
            Call SbAchievements.OpenCell(CssHeader, CssRankHeader)
            Call SbAchievements.Append("<b>" + HeaderRank + "</b>")
            Call SbAchievements.CloseCell()
          End If

          Call SbAchievements.OpenCell(CssHeader, CssLastUpdateHeader)
          Call SbAchievements.Append("<b>" + HeaderLastUpdate + "</b>")
          Call SbAchievements.CloseCell()

          If Config.ShowRegion Then
            Call SbAchievements.OpenCell(CssHeader, CssRegionHeader)
            Call SbAchievements.Append("<b>" + HeaderRegion + "</b>")
            Call SbAchievements.CloseCell()
          End If

          Call SbAchievements.OpenCell(CssHeader, CssPointsHeader)
          Call SbAchievements.Append("<b>" + HeaderPoints + "</b>")
          Call SbAchievements.CloseCell()

          Call SbAchievements.OpenCell(CssHeader, CssPlayerHeader)
          Call SbAchievements.Append("<b>" + HeaderPlayer + "</b>")
          Call SbAchievements.CloseCell()

          Call SbAchievements.OpenCell(CssHeader, CssLeagueHeader)
          Call SbAchievements.Append("<b>" + Header1V1 + "</b>")
          Call SbAchievements.CloseCell()

          Call SbAchievements.OpenCell(CssHeader, CssLeagueHeader)
          Call SbAchievements.Append("<b>" + Header2V2 + "</b>")
          Call SbAchievements.CloseCell()

          Call SbAchievements.OpenCell(CssHeader, CssLeagueHeader)
          Call SbAchievements.Append("<b>" + Header3V3 + "</b>")
          Call SbAchievements.CloseCell()

          Call SbAchievements.OpenCell(CssHeader, CssLeagueHeader)
          Call SbAchievements.Append("<b>" + Header4V4 + "</b>")
          Call SbAchievements.CloseCell()

          Call SbAchievements.CloseRow()

          IsAlternateRow = False

          Dim pMax As Int32 = listPlayers.Count - 1
          For p As Int32 = 0 To pMax
            With listPlayers.Item(p)
              'todo

              If (Not Me.Config.AchievementRankingOnlyWhenRanked) OrElse .IsRankedSomewhere Then
                Call SbAchievements.OpenRow()

                If Config.ShowRank Then
                  If IsAlternateRow Then
                    Call SbAchievements.OpenCell(CssAlternateRow1, CssLastUpdateCell)
                  Else
                    Call SbAchievements.OpenCell(CssAlternateRow0, CssLastUpdateCell)
                  End If
                  Call SbAchievements.Append((p + 1).ToString("N0"))
                  Call SbAchievements.CloseCell()
                End If

                If IsAlternateRow Then
                  Call SbAchievements.OpenCell(CssAlternateRow1, CssLastUpdateCell)
                Else
                  Call SbAchievements.OpenCell(CssAlternateRow0, CssLastUpdateCell)
                End If
                Call SbAchievements.Append(.Character.UpdatedAt.ToString("yyyy-MM-dd"))
                Call SbAchievements.CloseCell()

                If Config.ShowRegion Then
                  If IsAlternateRow Then
                    Call SbAchievements.OpenCell(CssAlternateRow1, CssRegionCell)
                  Else
                    Call SbAchievements.OpenCell(CssAlternateRow0, CssRegionCell)
                  End If
                  Call SbAchievements.Append(.Character.Region.ToString)
                  Call SbAchievements.CloseCell()
                End If

                If IsAlternateRow Then
                  Call SbAchievements.OpenCell(CssAlternateRow1, CssPointsCell)
                Else
                  Call SbAchievements.OpenCell(CssAlternateRow0, CssPointsCell)
                End If
                Call SbAchievements.Append(.Character.AchievementPoints.ToString("N0"))
                Call SbAchievements.CloseCell()

                Dim Tag As String

                If .Character.Clan IsNot Nothing Then
                  Tag = .Character.Clan.Tag
                Else
                  Tag = Nothing
                End If

                Call CreatePlayerCell(.Character.Name, Tag, .FavouriteRace, .Character.Url, SbAchievements, Me.Config, IsAlternateRow)

                Call GetLeaugeImage(.Highest1V1League, .Highest1V1Rank, AlternateText, CssClass)

                If IsAlternateRow Then
                  Call SbAchievements.OpenCell(CssAlternateRow1, CssLeagueCell)
                Else
                  Call SbAchievements.OpenCell(CssAlternateRow0, CssLeagueCell)
                End If

                Call SbAchievements.AppendSpan(AlternateText, CssClass)

                Call SbAchievements.CloseCell()

                Call GetLeaugeImage(.Highest2V2League, .Highest2V2Rank, AlternateText, CssClass)

                If IsAlternateRow Then
                  Call SbAchievements.OpenCell(CssAlternateRow1, CssLeagueCell)
                Else
                  Call SbAchievements.OpenCell(CssAlternateRow0, CssLeagueCell)
                End If

                Call SbAchievements.AppendSpan(AlternateText, CssClass)

                Call SbAchievements.CloseCell()

                Call GetLeaugeImage(.Highest3V3League, .Highest3V3Rank, AlternateText, CssClass)

                If IsAlternateRow Then
                  Call SbAchievements.OpenCell(CssAlternateRow1, CssLeagueCell)
                Else
                  Call SbAchievements.OpenCell(CssAlternateRow0, CssLeagueCell)
                End If

                Call SbAchievements.AppendSpan(AlternateText, CssClass)

                Call SbAchievements.CloseCell()

                Call GetLeaugeImage(.Highest4V4League, .Highest4V4Rank, AlternateText, CssClass)

                If IsAlternateRow Then
                  Call SbAchievements.OpenCell(CssAlternateRow1, CssLeagueCell)
                Else
                  Call SbAchievements.OpenCell(CssAlternateRow0, CssLeagueCell)
                End If

                Call SbAchievements.AppendSpan(AlternateText, CssClass)

                Call SbAchievements.CloseCell()

                Call SbAchievements.CloseRow()

                If IsAlternateRow Then
                  IsAlternateRow = False
                Else
                  IsAlternateRow = True
                End If
              End If
            End With
          Next p
          Call SbAchievements.CloseTable()

          Ex = BuildRankTable(Sb1V1, Me.Config, listLeague1v1, 1)
          If (Ex IsNot Nothing) Then Exit Try

          Ex = BuildRankTable(Sb2V2, Me.Config, listLeague2v2, 2)
          If (Ex IsNot Nothing) Then Exit Try

          Ex = BuildRankTable(Sb3V3, Me.Config, listLeague3v3, 3)
          If (Ex IsNot Nothing) Then Exit Try

          Ex = BuildRankTable(Sb4V4, Me.Config, listLeague4v4, 4)
          If (Ex IsNot Nothing) Then Exit Try

          If ReportProgress IsNot Nothing Then Call ReportProgress.Invoke(Me, 100, eProgressBarState.Normal)
        Catch iEx As Exception
          If ReportProgress IsNot Nothing Then Call ReportProgress.Invoke(Me, 100, eProgressBarState.Error)
          Ex = iEx
        End Try

        Try
          If (RankService IsNot Nothing) Then Call RankService.Dispose()

          If (CacheStream IsNot Nothing) Then
            With CacheStream
              Call .Flush()
              Call .Close()
              Call .Dispose()
            End With
          End If
        Catch
          '-
        End Try

        EndTime = Date.UtcNow

        GeneratedInSeconds = ((EndTime - StarTime).TotalMilliseconds / 1000)
      End If

      Return (Ex Is Nothing)
    End Function

#Region "Paged Calling"

#Region "Class PagedCall"

    Private MustInherit Class PagedCall(Of TResponse As ISc2RanksBaseResult)

      Protected MustOverride Function iExecute(ByVal Limit As Int32,
                                               ByVal Page As Int32,
                                               ByRef Response As TResponse) As Exception

      Protected MustOverride Function iGetItemCount() As Int32

      Protected MustOverride Function iGetItemsReceivedCount() As Int32

      Public Function Execute(ByVal Limit As Int32,
                              ByVal Page As Int32,
                              ByRef Response As TResponse) As Exception
        Return Me.iExecute(Limit, Page, Response)
      End Function

      Public Function GetItemCount() As Int32
        Return Me.iGetItemCount()
      End Function

      Public Function GetItemsReceivedCount() As Int32
        Return Me.iGetItemsReceivedCount()
      End Function
    End Class

#End Region

#Region "Class GetCustomDivisionPagedCall"

    Private NotInheritable Class GetCustomDivisionPagedCall
      Inherits PagedCall(Of Sc2RanksGetCustomDivisionCharacterListResult)

      Private ReadOnly RankService As Sc2RanksService
      Private ReadOnly CustomDivisionID As String
      Private ReadOnly IgnoreCache As Boolean
      Private m_Response As Sc2RanksGetCustomDivisionCharacterListResult

      Public Sub New(ByVal RankService As Sc2RanksService,
                     ByVal CustomDivisionID As String,
                     ByVal IgnoreCache As Boolean)
        Call MyBase.New()

        Me.m_Response = Nothing

        Me.RankService = RankService
        Me.CustomDivisionID = CustomDivisionID
        Me.IgnoreCache = IgnoreCache
      End Sub

      Protected Overrides Function iExecute(ByVal Limit As Int32,
                                            ByVal Page As Int32,
                                            ByRef Response As Sc2RanksGetCustomDivisionCharacterListResult) As Exception
        Response = Nothing
        Dim Ex As Exception
        Ex = Me.RankService.GetCustomDivisionCharacterList(CustomDivisionID, eSc2RanksRegion.Global, Response, Limit, Page, Me.IgnoreCache)

        If (Ex Is Nothing) Then Me.m_Response = Response

        Return Ex
      End Function

      Protected Overrides Function iGetItemCount() As Int32
        If (Me.m_Response IsNot Nothing) Then
          Return Me.m_Response.MemberCount
        Else
          Return 1
        End If
      End Function

      Protected Overrides Function iGetItemsReceivedCount() As Int32
        If (Me.m_Response IsNot Nothing) AndAlso (Me.m_Response.Characters IsNot Nothing) Then
          Return Me.m_Response.Characters.Length
        Else
          Return 0
        End If
      End Function
    End Class

#End Region

#Region "Class GetCustomDivisionTeamsPagedCall"

    Private NotInheritable Class GetCustomDivisionTeamsPagedCall
      Inherits PagedCall(Of Sc2RanksGetCustomDivisionTeamListResult)

      Private ReadOnly RankService As Sc2RanksService
      Private ReadOnly CustomDivisionID As String
      Private ReadOnly Expansion As eSc2RanksExpansion
      Private ReadOnly Bracket As eSc2RanksBracket
      Private ReadOnly IgnoreCache As Boolean
      Private m_Response As Sc2RanksGetCustomDivisionTeamListResult

      Public Sub New(ByVal RankService As Sc2RanksService,
                     ByVal CustomDivisionID As String,
                     ByVal Expansion As eSc2RanksExpansion,
                     ByVal Bracket As eSc2RanksBracket,
                     ByVal IgnoreCache As Boolean)
        Call MyBase.New()

        Me.m_Response = Nothing

        Me.RankService = RankService
        Me.CustomDivisionID = CustomDivisionID
        Me.Expansion = Expansion
        Me.Bracket = Bracket
        Me.IgnoreCache = IgnoreCache
      End Sub

      Protected Overrides Function iExecute(ByVal Limit As Int32,
                                            ByVal Page As Int32,
                                            ByRef Response As Sc2RanksGetCustomDivisionTeamListResult) As Exception
        Response = Nothing
        Dim Ex As Exception
        Ex = Me.RankService.GetCustomDivisionTeamList(CustomDivisionID, eSc2RanksRankRegion.Global, Me.Expansion, Me.Bracket, eSc2RanksLeague.All, Response, Nothing, Limit, Page, Me.IgnoreCache)

        If (Ex Is Nothing) Then Me.m_Response = Response

        Return Ex
      End Function

      Protected Overrides Function iGetItemCount() As Int32
        If (Me.m_Response IsNot Nothing) Then
          Return Me.m_Response.MemberCount
        Else
          Return 1
        End If
      End Function

      Protected Overrides Function iGetItemsReceivedCount() As Int32
        If (Me.m_Response IsNot Nothing) AndAlso (Me.m_Response.Teams IsNot Nothing) Then
          Return Me.m_Response.Teams.Length
        Else
          Return 0
        End If
      End Function
    End Class

#End Region

    Private Shared Function ExecutePagedCall(Of TPagedCall As PagedCall(Of TResponse), TResponse As ISc2RanksBaseResult)(ByVal Key As TKey,
                                                                                                                         ByVal LastProgress As Double,
                                                                                                                         ByVal [Call] As TPagedCall,
                                                                                                                         ByVal Limit As Int32,
                                                                                                                         ByRef [Try] As Int32,
                                                                                                                         ByVal MaxTries As Int32,
                                                                                                                         ByVal RetryWaitTime As TimeSpan,
                                                                                                                         ByVal ReportProgress As procReportProgress,
                                                                                                                         ByVal OnCancelPending As procOnCancelPending(Of TKey),
                                                                                                                         ByRef ResponseList As IList(Of TResponse)) As Exception
      Dim Ex As Exception = Nothing
      Dim iResponses As New List(Of TResponse)
      Dim iResponse As TResponse = Nothing
      Dim Success As Boolean = False
      Dim Page As Int32 = 1
      Dim ItemsReceived As Int32
      Dim ItemsReceivedLastCall As Int32

      If ([Try] < 0) Then [Try] = 0
      If (MaxTries < 0) Then MaxTries = 1
      If (Limit > Sc2RanksService.MaxRequestLimit) Then Limit = Sc2RanksService.MaxRequestLimit

      [Try] += 1

      Do Until Success OrElse ([Try] > MaxTries)
        Ex = [Call].Execute(Limit, Page, iResponse)

        If (Ex Is Nothing) Then
          ItemsReceivedLastCall = [Call].GetItemsReceivedCount
          ItemsReceived += ItemsReceivedLastCall
          If (ItemsReceivedLastCall < Limit) OrElse ([Call].GetItemCount() <= ItemsReceived) Then Success = True

          Page += 1
          Call iResponses.Add(iResponse)
          If ReportProgress IsNot Nothing Then Call ReportProgress.Invoke(Nothing, LastProgress, eProgressBarState.Normal)
        Else
          Success = False
          [Try] += 1

          Call Trace.WriteLine(String.Format("Waiting for next try in {0} seconds.", RetryWaitTime.ToString))
          If (Ex IsNot Nothing) AndAlso (ReportProgress IsNot Nothing) Then Call ReportProgress.Invoke(Nothing, LastProgress, eProgressBarState.Paused)

          Call Sleep(Of TKey)(RetryWaitTime, Key, OnCancelPending)
        End If
      Loop

      [Try] = 0
      ResponseList = iResponses

      If ([Try] >= MaxTries) Then Ex = New Exception("Call failed, number of retries exceeded.")

      Return Ex
    End Function

#End Region

    Public Shared Sub GetRaceImage(ByVal Race As eSc2RanksRace,
                                   <Out()> ByRef CssClass As String)

      Select Case Race
        Case eSc2RanksRace.Protoss
          CssClass = "race-protoss"
        Case eSc2RanksRace.Terran
          CssClass = "race-terran"
        Case eSc2RanksRace.Zerg
          CssClass = "race-zerg"
        Case Else
          CssClass = "race-random"
      End Select
    End Sub

    Public Shared Sub GetLeaugeImage(ByVal Leauge As eSc2RanksLeague,
                                     ByVal Rank As Int32,
                                     <Out()> ByRef AlternateText As String,
                                     <Out()> ByRef CssClass As String)
      Select Case Leauge
        Case eSc2RanksLeague.Bronze
          AlternateText = "Bronze"

          Select Case Rank
            Case Is <= 8
              CssClass = "league-bronze league-bronze-4"
            Case Is <= 25
              CssClass = "league-bronze league-bronze-3"
            Case Is <= 50
              CssClass = "league-bronze league-bronze-2"
            Case Else
              CssClass = "league-bronze league-bronze-1"
          End Select
        Case eSc2RanksLeague.Silver
          AlternateText = "Silver"

          Select Case Rank
            Case Is <= 8
              CssClass = "league-silver league-silver-4"
            Case Is <= 25
              CssClass = "league-silver league-silver-3"
            Case Is <= 50
              CssClass = "league-silver league-silver-2"
            Case Else
              CssClass = "league-silver league-silver-1"
          End Select
        Case eSc2RanksLeague.Gold
          AlternateText = "Gold"

          Select Case Rank
            Case Is <= 8
              CssClass = "league-gold league-gold-4"
            Case Is <= 25
              CssClass = "league-gold league-gold-3"
            Case Is <= 50
              CssClass = "league-gold league-gold-2"
            Case Else
              CssClass = "league-gold league-gold-1"
          End Select
        Case eSc2RanksLeague.Platinum
          AlternateText = "Platinum"

          Select Case Rank
            Case Is <= 8
              CssClass = "league-platinum league-platinum-4"
            Case Is <= 25
              CssClass = "league-platinum league-platinum-3"
            Case Is <= 50
              CssClass = "league-platinum league-platinum-2"
            Case Else
              CssClass = "league-platinum league-platinum-1"
          End Select
        Case eSc2RanksLeague.Diamond
          AlternateText = "Diamond"

          Select Case Rank
            Case Is <= 8
              CssClass = "league-diamond league-diamond-4"
            Case Is <= 25
              CssClass = "league-diamond league-diamond-3"
            Case Is <= 50
              CssClass = "league-diamond league-diamond-2"
            Case Else
              CssClass = "league-diamond league-diamond-1"
          End Select
        Case eSc2RanksLeague.Master
          AlternateText = "Master"

          Select Case Rank
            Case Is <= 8
              CssClass = "league-master league-master-4"
            Case Is <= 25
              CssClass = "league-master league-master-3"
            Case Is <= 50
              CssClass = "league-master league-master-2"
            Case Else
              CssClass = "league-master league-master-1"
          End Select
        Case eSc2RanksLeague.GrandMaster
          AlternateText = "Grandmaster"

          Select Case Rank
            Case Is <= 16
              CssClass = "league-grandmaster league-grandmaster-4"
            Case Is <= 25
              CssClass = "league-grandmaster league-grandmaster-3"
            Case Is <= 50
              CssClass = "league-grandmaster league-grandmaster-2"
            Case Else
              CssClass = "league-grandmaster league-grandmaster-1"
          End Select
        Case Else
          AlternateText = "Unknown"
          CssClass = "league-unknown"
      End Select
    End Sub

    Private Shared Function CssClassSafeString(ByVal [Class] As String) As String
      If (Not String.IsNullOrEmpty([Class])) Then
        Return [Class].Replace("/[!\""#$%&'\(\)\*\+,\.\/:;<=>\?\@\[\\\]\^`\{\|\}~]/g", String.Empty).ToLower()
      Else
        Return [Class]
      End If
    End Function

    Private Shared Function BuildRankTable(ByRef SB As HtmlStringBuilder,
                                           ByVal Config As IConfig,
                                           ByVal Players As IList(Of Team),
                                           ByVal PlayersInBracket As Int32) As Exception
      Dim Ex As Exception = Nothing
      Dim CssClass As String = Nothing
      Dim AlternateText As String = Nothing
      Dim IsAlternateRow As Boolean = False
      Dim ClanTag As String

      If (SB Is Nothing) Then
        Ex = New ArgumentNullException("SB")
      ElseIf (Config Is Nothing) Then
        Ex = New ArgumentNullException("Config")
      ElseIf (Players Is Nothing) Then
        Ex = New ArgumentNullException("Players")
      Else
        Call SB.OpenTable(CellSpacing := CellSpacing, CellPadding := CellPadding)
        Call SB.OpenRow()

        If Config.ShowRank Then
          Call SB.OpenCell(CssHeader, CssRankHeader)
          Call SB.Append("<b>" + HeaderRank + "</b>")
          Call SB.CloseCell()
        End If

        If Config.ShowLastUpdate Then
          Call SB.OpenCell(CssHeader, CssLastUpdateHeader)
          Call SB.Append("<b>" + HeaderLastUpdate + "</b>")
          Call SB.CloseCell()
        End If

        If Config.ShowExpansion Then
          Call SB.OpenCell(CssHeader, CssExpansionHeader)
          Call SB.Append("<b>" + HeaderExpansion + "</b>")
          Call SB.CloseCell()
        End If

        If Config.ShowRegion Then
          Call SB.OpenCell(CssHeader, CssRegionHeader)
          Call SB.Append("<b>" + HeaderRegion + "</b>")
          Call SB.CloseCell()
        End If

        If Config.ShowRegionRank Then
          Call SB.OpenCell(CssHeader, CssRegionRankHeader)
          Call SB.Append("<b>" + HeaderRegionRank + "</b>")
          Call SB.CloseCell()
        End If

        If Config.ShowWorldRank Then
          Call SB.OpenCell(CssHeader, CssWorldRankHeader)
          Call SB.Append("<b>" + HeaderWorldRank + "</b>")
          Call SB.CloseCell()
        End If

        Call SB.OpenCell(CssHeader, CssLeagueHeader)
        Call SB.Append("<b>" + HeaderLeague + "</b>")
        Call SB.CloseCell()

        Call SB.OpenCell(CssHeader, CssDivisionRankHeader)
        Call SB.Append("<b>" + HeaderDivisionRank + "</b>")
        Call SB.CloseCell()

        Call SB.OpenCell(CssHeader, CssPointsHeader)
        Call SB.Append("<b>" + HeaderPoints + "</b>")
        Call SB.CloseCell()

        Call SB.OpenCell(CssHeader, CssWinsHeader)
        Call SB.Append("<b>" + HeaderWins + "</b>")
        Call SB.CloseCell()

        If Config.ShowLosses Then
          Call SB.OpenCell(CssHeader, CssLossesHeader)
          Call SB.Append("<b>" + HeaderLosses + "</b>")
          Call SB.CloseCell()
        End If

        If Config.ShowWinLossRatio Then
          Call SB.OpenCell(CssHeader, CssWinLossRatioHeader)
          Call SB.Append("<b>" + HeaderWinLossRatio + "</b>")
          Call SB.CloseCell()
        End If

        For i As Int32 = 1 To PlayersInBracket
          Call SB.OpenCell(CssHeader, CssPlayerHeader)
          Call SB.Append("<b>" + HeaderPlayer + "</b>")
          Call SB.CloseCell()
        Next i

        Call SB.CloseRow()

        Dim iMax As Int32 = Players.Count - 1
        For i = 0 To iMax
          With Players.Item(i)
            Call SB.OpenRow()

            If Config.ShowRank Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssRankCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssRankCell)
              End If
              Call SB.Append((i + 1).ToString("N0"))
              Call SB.CloseCell()
            End If

            If Config.ShowLastUpdate Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssLastUpdateCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssLastUpdateCell)
              End If
              Call SB.Append(.Team.LastGameAt.ToString("yyyy-MM-dd"))
              Call SB.CloseCell()
            End If

            If Config.ShowExpansion Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssExpansionCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssExpansionCell)
              End If
              Call SB.Append(.Team.Expansion.ToString)
              Call SB.CloseCell()
            End If

            If Config.ShowRegion Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssRegionCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssRegionCell)
              End If
              Call SB.Append(.Team.RankRegion.ToString())
              Call SB.CloseCell()
            End If

            If Config.ShowRegionRank Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssRegionRankCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssRegionRankCell)
              End If
              If .DivisionRank = Integer.MaxValue Then
                Call SB.Append("-")
              Else
                Call SB.Append(.RegionRank.ToString("N0"))
              End If
              Call SB.CloseCell()
            End If

            If Config.ShowWorldRank Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssWorldRankCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssWorldRankCell)
              End If
              If .WorldRank = Integer.MaxValue Then
                Call SB.Append("-")
              Else
                Call SB.Append(.WorldRank.ToString("N0"))
              End If
              Call SB.CloseCell()
            End If

            Call GetLeaugeImage(.Team.League, .DivisionRank, AlternateText, CssClass)

            If IsAlternateRow Then
              Call SB.OpenCell(CssAlternateRow1, CssLeagueCell)
            Else
              Call SB.OpenCell(CssAlternateRow0, CssLeagueCell)
            End If

            Call SB.OpenSpan(AlternateText, CssClass)

            Call SB.CloseCell()

            If IsAlternateRow Then
              Call SB.OpenCell(CssAlternateRow1, CssDivisionRankCell)
            Else
              Call SB.OpenCell(CssAlternateRow0, CssDivisionRankCell)
            End If
            Call SB.OpenUrl(.Team.Url.ToString, HtmlStringBuilder.eTargetFrame.NewWindow)
            If .DivisionRank = Integer.MaxValue Then
              Call SB.Append("-")
            Else
              Call SB.Append(.DivisionRank.ToString("N0"))
            End If
            Call SB.CloseUrl()
            Call SB.CloseCell()

            If IsAlternateRow Then
              Call SB.OpenCell(CssAlternateRow1, CssPointsCell)
            Else
              Call SB.OpenCell(CssAlternateRow0, CssPointsCell)
            End If
            Call SB.Append(.Team.Points.ToString("N0"))
            Call SB.CloseCell()

            If IsAlternateRow Then
              Call SB.OpenCell(CssAlternateRow1, CssWinsCell)
            Else
              Call SB.OpenCell(CssAlternateRow0, CssWinsCell)
            End If
            Call SB.Append(.Team.Wins.ToString("N0"))
            Call SB.CloseCell()

            If Config.ShowLosses Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssLossesCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssLossesCell)
              End If
              Call SB.Append(.Team.Losses.ToString("N0"))
              Call SB.CloseCell()
            End If

            If Config.ShowWinLossRatio Then
              If IsAlternateRow Then
                Call SB.OpenCell(CssAlternateRow1, CssWinLossRatioCell)
              Else
                Call SB.OpenCell(CssAlternateRow0, CssWinLossRatioCell)
              End If
              Call SB.Append(.Ratio().ToString())
              Call SB.CloseCell()
            End If

            Dim dMax As Int32 = PlayersInBracket - 1
            For d As Int32 = 0 To dMax
              If (.Team.Characters IsNot Nothing) AndAlso d <= .Team.Characters.Length - 1 Then

                With .Team.Characters(d)
                  If (.Clan IsNot Nothing) Then
                    ClanTag = .Clan.Tag
                  Else
                    ClanTag = Nothing
                  End If

                  Call CreatePlayerCell(.Name, ClanTag, .Race, .Url, SB, Config, IsAlternateRow)
                End With
              Else
                If IsAlternateRow Then
                  Call SB.OpenCell(CssAlternateRow1, CssEmptyCell)
                Else
                  Call SB.OpenCell(CssAlternateRow0, CssEmptyCell)
                End If
                Call SB.CloseCell()
              End If
            Next d

            Call SB.CloseRow()

            IsAlternateRow = (Not IsAlternateRow)
          End With
        Next i

        Call SB.CloseTable()
      End If

      Return Ex
    End Function

    Private Shared Sub CreatePlayerCell(ByVal CharacterName As String,
                                        ByVal ClanTag As String,
                                        ByVal FavouriteRace As Nullable(Of eSc2RanksRace),
                                        ByVal Url As String,
                                        ByRef SB As HtmlStringBuilder,
                                        ByVal Config As IConfig,
                                        ByVal IsAlternateRow As Boolean)
      Dim CssClass As String = Nothing
      If IsAlternateRow Then
        Call SB.OpenCell(CssAlternateRow1, CssPlayerCell)
      Else
        Call SB.OpenCell(CssAlternateRow0, CssPlayerCell)
      End If

      If FavouriteRace.HasValue AndAlso Config.ShowFavouriteRace Then Call GetRaceImage(FavouriteRace.Value, CssClass)

      If Config.AlwaysLinkPlayers Then Call SB.OpenUrl(Url, HtmlStringBuilder.eTargetFrame.NewWindow, CssClass)
      If (Not String.IsNullOrEmpty(ClanTag)) Then
        Call SB.OpenSpan(ClanTag, CssClanTag, CssClanSelfTag + CssClassSafeString(ClanTag))

        If Config.ShowClanTag Then
          Call SB.AppendFormat("[{0}]", ClanTag)
          Call SB.Append(" ")
        End If
      Else
        Call SB.OpenSpan(Nothing, CssClanTag, CssClanTagEmpty)
      End If
      Call SB.OpenSpan(CharacterName, CssPlayerName, CssPlayerSelfName + CssClassSafeString(CharacterName))
      Call SB.Append(CharacterName)
      Call SB.CloseSpan()
      If Config.AlwaysLinkPlayers Then Call SB.CloseUrl()

      Call SB.CloseCell()
    End Sub
  End Class
End Namespace