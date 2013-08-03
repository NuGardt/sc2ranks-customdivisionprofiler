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
Imports NuGardt.SC2Ranks.API.Result

Namespace SC2Ranks.CustomDivisionProfiler
  Friend Class Player
    Inherits PlayerKey

    Public Character As Sc2RanksCharacterResult

    Public Highest1V1League As eSc2RanksLeague
    Public Highest1V1Rank As Integer

    Public Highest2V2League As eSc2RanksLeague
    Public Highest2V2Rank As Integer

    Public Highest3V3League As eSc2RanksLeague
    Public Highest3V3Rank As Integer

    Public Highest4V4League As eSc2RanksLeague
    Public Highest4V4Rank As Integer

    Public Sub New(ByVal Character As Sc2RanksCharacterResult)
      Call MyBase.New(Character.BattleNetID, Character.Region, Character.AchievementPoint)

      Me.Character = Character
    End Sub
  End Class
End Namespace