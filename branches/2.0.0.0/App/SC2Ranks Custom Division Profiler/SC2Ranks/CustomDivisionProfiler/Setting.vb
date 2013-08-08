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
Imports NuGardt.Core

Namespace SC2Ranks.CustomDivisionProfiler
  Friend Class Setting
    Inherits System.Windows.Forms.Form

    Private ReadOnly Config As IConfig
    Private ReadOnly OnClose As procRelay

    Public Sub New(ByRef Config As IConfig,
                   ByVal OnClose As procRelay)
      Call MyBase.New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      Call Me.InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
      Me.Icon = My.Resources.ICO_HwC_Logo

      Me.Config = Config
      Me.OnClose = OnClose
    End Sub

    Private Sub SettingFormClosed(ByVal Sender As Object,
                                  ByVal e As FormClosedEventArgs) Handles Me.FormClosed
      Call Me.OnClose.Invoke()
    End Sub

    Private Sub SettingLoad(ByVal Sender As Object,
                            ByVal e As EventArgs) Handles Me.Load
      Me.pgSettings.SelectedObject = Me.Config
    End Sub

    Private Sub CloseClick(ByVal Sender As Object,
                           ByVal e As EventArgs) Handles cmdClose.Click
      Call Me.Close()
    End Sub
  End Class
End Namespace