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
Imports System.Xml

Namespace SC2Ranks.CustomDivisionProfiler
  Friend Interface IConfig
    Inherits ICacheConfig

    Property AutoDownload As Boolean

    Property Template As String

    Property ConfigPath As String

    'API
    Property ApiKey As String

    'Control
    Property AutoClose As Boolean

    Property DisableAutosave As Boolean

    Property RetryCount As Int32

    Property RetryWaitTime As TimeSpan

    Property DisableAutoOpen As Boolean

    Property RequestIdleTime As TimeSpan

    'Settings
    Property CustomDivisionID As String

    Property CustomDescription As String

    'Files & Paths
    Property OutputFileName As String

    Property OutputFolder As String

    Property TemplatePath As String

    'Columns
    Property ShowRegion As Boolean

    Property ShowRank As Boolean

    Property ShowRegionRank As Boolean

    Property ShowWorldRank As Boolean

    Property ShowLastUpdate As Boolean

    Property ShowLosses As Boolean

    Property ShowExpansion As Boolean

    Property ShowWinLossRatio As Boolean

    Property AlwaysLinkPlayers As Boolean

    Property ShowFavouriteRace As Boolean

    Property ShowClanTag As Boolean

    'Ranking Options
    Property Expansion As eSc2RanksExpansion

    Property Load1V1 As Boolean

    Property Load2V2T As Boolean

    Property Load2V2R As Boolean

    Property Load3V3T As Boolean

    Property Load3V3R As Boolean

    Property Load4V4T As Boolean

    Property Load4V4R As Boolean

    Property AchievementRankingOnlyWhenRanked As Boolean

    'Ignore Cache
    Property IgnoreCacheGetCustomDivision As Boolean

    Property IgnoreCacheGetBaseTeam As Boolean

    Property IgnoreCacheGetTeam As Boolean

    Sub Reset()

    Function ToXml(ByVal Stream As XmlWriter,
                   Optional ByRef Ex As Exception = Nothing) As Boolean

    Function FromXml(ByVal Stream As XmlReader,
                     ByRef Config As Config,
                     Optional ByRef Ex As Exception = Nothing) As Boolean
  End Interface
End Namespace