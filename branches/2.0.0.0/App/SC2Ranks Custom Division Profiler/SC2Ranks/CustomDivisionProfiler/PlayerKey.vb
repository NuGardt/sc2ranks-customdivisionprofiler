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

Namespace SC2Ranks.CustomDivisionProfiler
  Friend Class PlayerKey
    Implements IComparable(Of PlayerKey)
    Implements IEquatable(Of PlayerKey)

    Public ReadOnly BattleNetID As Int64
    Public ReadOnly Region As eSc2RanksRegion
    Public ReadOnly AchievementPoints As Int32

#Region "Compare & Equals"

    Private Function CompareTo(ByVal Other As PlayerKey) As Int32 Implements IComparable(Of PlayerKey).CompareTo
      Return Me.Compare(Me, Other)
    End Function

    Private Function Compare(ByVal X As PlayerKey,
                             ByVal Y As PlayerKey) As Int32
      Dim Erg As Int32

      If X IsNot Nothing Then
        If Y IsNot Nothing Then
          Erg = X.BattleNetID.CompareTo(Y.BattleNetID)
          If Erg = 0 Then Erg = X.Region.CompareTo(Y.Region)
        Else
          Erg = 1
        End If
      ElseIf Y IsNot Nothing Then
        Erg = - 1
      Else
        Erg = 0
      End If

      Return Erg
    End Function

    Public Shared Function CompareByAchievementPoints(ByVal X As PlayerKey,
                                                      ByVal Y As PlayerKey) As Int32
      Dim Erg As Int32

      If X IsNot Nothing Then
        If Y IsNot Nothing Then
          Erg = X.AchievementPoints.CompareTo(Y.AchievementPoints)
        Else
          Erg = 1
        End If
      Else
        Erg = 0
      End If

      Return Erg
    End Function

    Public Shared Function CompareByAchievementPointsInverted(ByVal X As PlayerKey,
                                                              ByVal Y As PlayerKey) As Int32
      Dim Erg As Int32

      If X IsNot Nothing Then
        If Y IsNot Nothing Then
          Erg = X.AchievementPoints.CompareTo(Y.AchievementPoints)
        Else
          Erg = 1
        End If
      Else
        Erg = 0
      End If

      Erg = Erg * - 1

      Return Erg
    End Function

    Public Function Equals1(Other As PlayerKey) As Boolean Implements IEquatable(Of PlayerKey).Equals
      Return (Equals(Me.BattleNetID, Other.BattleNetID))
    End Function

#End Region

    Public Sub New(ByVal BattleNetID As Int64,
                   ByVal Region As eSc2RanksRegion,
                   ByVal AchievementPoints As Int32)
      Me.BattleNetID = BattleNetID
      Me.Region = Region
      Me.AchievementPoints = AchievementPoints
    End Sub

    Public Sub New(ByVal BattleNetID As Int64,
                   ByVal Region As eSc2RanksRegion)
      Me.BattleNetID = BattleNetID
      Me.Region = Region
    End Sub
  End Class
End Namespace