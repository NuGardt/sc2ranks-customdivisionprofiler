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
Imports NuGardt.SC2Ranks.API
Imports NuGardt.SC2Ranks.API.Result.Element

Namespace SC2Ranks.CustomDivisionProfiler
  Friend Class Player
    Inherits PlayerKey

    Public Character As Sc2RanksCharacterExtended

    Public Highest1V1League As eSc2RanksLeague
    Public Highest1V1Rank As Int32

    Public Highest2V2League As eSc2RanksLeague
    Public Highest2V2Rank As Int32

    Public Highest3V3League As eSc2RanksLeague
    Public Highest3V3Rank As Int32

    Public Highest4V4League As eSc2RanksLeague
    Public Highest4V4Rank As Int32

    Public FavouriteRace As eSc2RanksRace
    Public RaceFromBracket As eSc2RanksBracket

    Public Sub New(ByVal Character As Sc2RanksCharacterExtended)
      Call MyBase.New(Character.BattleNetID, Character.Region, Character.AchievementPoints)

      Me.Character = Character

      Me.Highest1V1League = eSc2RanksLeague.All
      Me.Highest1V1Rank = Nothing

      Me.Highest2V2League = eSc2RanksLeague.All
      Me.Highest2V2Rank = Nothing

      Me.Highest3V3League = eSc2RanksLeague.All
      Me.Highest3V3Rank = Nothing

      Me.Highest4V4League = eSc2RanksLeague.All
      Me.Highest4V4Rank = Nothing

      Me.FavouriteRace = eSc2RanksRace.Random
      Me.RaceFromBracket = eSc2RanksBracket._4V4T
    End Sub

    Public ReadOnly Property IsRankedSomewhere As Boolean
      Get
        Return (Me.Highest1V1League <> eSc2RanksLeague.All) OrElse (Me.Highest2V2League <> eSc2RanksLeague.All) OrElse (Me.Highest3V3League <> eSc2RanksLeague.All) OrElse (Me.Highest4V4League <> eSc2RanksLeague.All)
      End Get
    End Property
  End Class
End Namespace