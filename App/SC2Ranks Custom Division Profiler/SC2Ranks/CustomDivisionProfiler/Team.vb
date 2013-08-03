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
Imports NuGardt.SC2Ranks.API.Result.Element

Namespace SC2Ranks.CustomDivisionProfiler
  Friend Class Team
    Public ReadOnly Team As Sc2RanksCharacterTeamElement

    Public Sub New(ByVal Team As Sc2RanksCharacterTeamElement)
      Me.Team = Team
    End Sub

    Public Function Ratio() As String
      Try
        Return ((Me.Team.Wins / (Me.Team.Wins + Me.Team.Losses)) * 100).ToString("N2") + "%"
      Catch
        Return "-"
      End Try
    End Function

    Public Function Losses() As String
      Return Me.Team.Losses.ToString("N0")
    End Function

    Public Function DivisionRank() As Int32
      If (Me.Team.Division Is Nothing) OrElse (Not Me.Team.Division.Rank.HasValue) Then
        Return Int32.MaxValue
      Else
        Return Me.Team.Division.Rank.Value
      End If
    End Function

    Public Function WorldRank() As Integer
      If (Me.Team.Rankings Is Nothing) OrElse Me.Team.Rankings.World = 0 Then
        Return Integer.MaxValue
      Else
        Return Me.Team.Rankings.World
      End If
    End Function

    Public Function RegionRank() As Integer
      If (Me.Team.Rankings Is Nothing) OrElse Me.Team.Rankings.Region = 0 Then
        Return Integer.MaxValue
      Else
        Return Me.Team.Rankings.Region
      End If
    End Function

    Public Shared Function CompareByLeagueRegionRankInverted(ByVal X As Team,
                                                             ByVal Y As Team) As Integer
      Dim Erg As Integer

      If X IsNot Nothing Then
        If Y IsNot Nothing Then
          Erg = X.Team.League.CompareTo(Y.Team.League)
          If Erg = 0 Then
            Erg = X.RegionRank.CompareTo(Y.RegionRank) * - 1
            If Erg = 0 Then
              Erg = X.Team.Points.CompareTo(Y.Team.Points)
              If Erg = 0 Then
                Erg = X.DivisionRank.CompareTo(Y.DivisionRank) * - 1
                If Erg = 0 Then Erg = X.Team.Wins.CompareTo(Y.Team.Wins) * - 1
              End If
            End If
          End If
        Else
          Erg = 1
        End If
      Else
        Erg = 0
      End If

      Erg = Erg * - 1

      Return Erg
    End Function
  End Class
End Namespace