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
Imports System.IO
Imports NuGardt.Core
Imports NuGardt.SC2Ranks.CustomDivisionProfiler.Jobs
Imports NuGardt.Core.Process
Imports System.Xml
Imports System.Text

Namespace SC2Ranks.CustomDivisionProfiler
  Public Class Form
    Private Settings As Setting
    Private m_Data As String
    Private m_Config As IConfig
    Private PM As Manager(Of Guid)

    Public Sub New()
      Call MyBase.New()

      ' Dieser Aufruf ist für den Designer erforderlich.
      Call Me.InitializeComponent()

      ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
      Me.Icon = My.Resources.ICO_HwC_Logo
      Me.lblVersion.Text = "v" + My.Application.Info.Version.ToString

      Me.m_Data = Nothing
      Me.Settings = Nothing

      Me.PM = New Manager(Of Guid)(1)

      Dim cmd As New CommandLine
      If Not Config.TryParse(cmd, Me.m_Config) Then Me.m_Config = New Config
    End Sub

    Private Sub FormFormClosing(ByVal Sender As Object,
                                ByVal e As FormClosingEventArgs) Handles Me.FormClosing
      Try
        Dim Stream As Stream
        Dim XmlWriter As XmlWriter
        Dim XmlSettings As XmlWriterSettings
        Dim Path As String

        If String.IsNullOrEmpty(Me.m_Config.ConfigPath) Then
          Path = Config.DefaultConfigPath
        Else
          Path = Me.m_Config.ConfigPath
        End If

        Stream = New FileStream(Path, FileMode.Create)
        XmlSettings = New XmlWriterSettings

        XmlSettings.Indent = True

        XmlWriter = XmlWriter.Create(Stream, XmlSettings)

        Call Me.m_Config.ToXML(XmlWriter)

        Call XmlWriter.Flush()

        With Stream
          Call .Flush()
          Call .Close()
          Call .Dispose()
        End With

        If Me.PM IsNot Nothing Then Call Me.PM.Dispose()
        Me.PM = Nothing
      Catch iEx As Exception
        If Verbose Then
          Call Trace.WriteLine(iEx.ToString, (New StackFrame).GetMethod.Name)
        Else
          Call Trace.WriteLine(iEx.Message)
        End If
      End Try
    End Sub

    Private Sub FormGotFocus(ByVal Sender As Object,
                             ByVal e As EventArgs) Handles Me.Activated
      Dim Data As String = My.Computer.Clipboard.GetText
      If Not Me.PM.IsBusy AndAlso Me.txtURL.Text.Length = 0 AndAlso ParseUrl(Data, Nothing) Then Me.txtURL.Text = Data
    End Sub

    Private Sub FormShown(ByVal Sender As Object,
                          ByVal e As EventArgs) Handles Me.Shown
      Call Me.UpdateCurrentCustomDivisionIDLabel()
      If Me.m_Config.AutoDownload Then Call Me.DownloadCustomDivision()
    End Sub

    Private Sub GetDataClick(ByVal Sender As Object,
                             ByVal e As EventArgs) Handles cmdGetData.Click
      Call Me.DownloadCustomDivision()
    End Sub

    Private Sub SaveClick(ByVal Sender As Object,
                          ByVal e As EventArgs) Handles cmdSave.Click
      Dim Stream As Stream = Nothing
      Dim Dialog As SaveFileDialog

      Try
        Dialog = New SaveFileDialog

        With Dialog
          .FileName = Me.m_Config.OutputFileName
          .Filter = "HTML|*.htm"

          If Not String.IsNullOrEmpty(Me.m_Config.OutputFileName) Then .InitialDirectory = Directory.GetCurrentDirectory

          If .ShowDialog = DialogResult.OK Then
            Dim FI As FileInfo = Nothing
            Dim DI As DirectoryInfo
            Try
              FI = New FileInfo(.FileName)
              Me.m_Config.OutputFolder = FI.DirectoryName
              Me.m_Config.OutputFileName = FI.Name
            Catch
              '-
            End Try

            Stream = New FileStream(.FileName, FileMode.Create, FileAccess.Write, FileShare.Read)
            Dim Buffer() As Byte

            Buffer = Encoding.UTF8.GetBytes(Me.m_Data)

            Call Stream.Write(Buffer, 0, Buffer.Length)

            'If the images folder does not exist, copy it =D
            Dim ImagePath As String = Path.Combine(FI.DirectoryName, "Images")
            If Not Directory.Exists(ImagePath) Then
              Call Directory.CreateDirectory(ImagePath)

              DI = New DirectoryInfo(Path.Combine(My.Application.Info.DirectoryPath, "Images"))
              For Each F In DI.GetFiles("*.png")
                Call F.CopyTo(Path.Combine(ImagePath, F.Name))
              Next F
            End If
          End If

          Call .Dispose()

          If Stream IsNot Nothing Then
            With Stream
              Call .Flush()
              Call .Close()
              Call .Dispose()
            End With
          End If

          Call MsgBox("File saved successfully.", MsgBoxStyle.Information)
        End With
      Catch iEx As Exception
        Dim tMessage As String

        If Verbose Then
          tMessage = iEx.ToString
        Else
          tMessage = iEx.Message
        End If

        If Verbose Then
          Call Trace.WriteLine(tMessage, (New StackFrame).GetMethod.Name)
        Else
          Call Trace.WriteLine(tMessage)
        End If
        Call MsgBox(tMessage, MsgBoxStyle.Critical)
      End Try
    End Sub

    Private Sub DownloadCustomDivision()
      If Not Me.PM.IsBusy Then
        Dim Profile As Profiler(Of Guid)

        Me.pbProgress.Value = 0
        Me.pbProgress.Maximum = (100 * 1000)
        Me.m_Data = Nothing

        Me.cmdSettings.Enabled = False
        Me.cmdGetData.Enabled = False
        Me.cmdSave.Enabled = False
        Me.cmdPaste.Enabled = False
        Me.txtURL.Enabled = False

        Profile = New Profiler(Of Guid)(Guid.NewGuid, Me.m_Config, AddressOf Me.ProgressChanged, AddressOf Me.iCompleteProfiler)
        Call Me.PM.Add(Profile)
      End If
    End Sub

    Private Delegate Sub procOnCompleteProfiler(ByVal Job As Profiler(Of Guid))

    Private ReadOnly OnCompleteProfiler As procOnCompleteProfiler = AddressOf Me.iOnCompleteProfiler

    Private Sub iCompleteProfiler(ByVal Job As Job(Of Guid))
      Call Me.BeginInvoke(Me.OnCompleteProfiler, New Object() {DirectCast(Job, Profiler(Of Guid))})
    End Sub

    Private Sub iOnCompleteProfiler(ByVal Job As Profiler(Of Guid))
      Dim tMessage As String

      If (Job Is Nothing) Then
        tMessage = "No data received."
        If Verbose Then
          Call Trace.WriteLine(tMessage, (New StackFrame).GetMethod.Name)
        Else
          Call Trace.WriteLine(tMessage)
        End If
        Call MsgBox(tMessage, MsgBoxStyle.Information)
      ElseIf (Job.Error IsNot Nothing) AndAlso (TypeOf Job.Error Is Exception) Then
        If Verbose Then
          tMessage = "Error: " + Job.Error.ToString
        Else
          tMessage = "Error: " + Job.Error.Message
        End If

        If Verbose Then
          Call Trace.WriteLine(tMessage, (New StackFrame).GetMethod.Name)
        Else
          Call Trace.WriteLine(tMessage)
        End If
        Call MsgBox(tMessage, MsgBoxStyle.Critical)
      ElseIf (Not String.IsNullOrEmpty(Job.Output)) Then
        Dim Path As String = IO.Path.Combine(Me.m_Config.OutputFolder, Me.m_Config.OutputFileName)
        Dim Stream As Stream

        Me.m_Data = Job.Output

        Try
          If Not Me.m_Config.DisableAutosave Then
            Stream = New FileStream(Path, FileMode.Create, FileAccess.Write, FileShare.Read)
            Dim Buffer() As Byte

            Buffer = Encoding.UTF8.GetBytes(Me.m_Data)

            Call Stream.Write(Buffer, 0, Buffer.Length)

            With Stream
              Call .Flush()
              Call .Close()
              Call .Dispose()
            End With

            If (Not Me.m_Config.AutoClose) AndAlso (Not Me.m_Config.DisableAutoOpen) Then Call Diagnostics.Process.Start(Path)
          End If

          If Not Me.m_Config.AutoClose Then Call MsgBox(String.Format("Data received successfully!{0}Credits used: {1}", vbCrLf, Job.CreditsUsed.ToString()), MsgBoxStyle.Information)
        Catch iEx As Exception
          If Verbose Then
            tMessage = "Error saving data: " + iEx.ToString
          Else
            tMessage = "Error saving data: " + iEx.Message
          End If

          If Verbose Then
            Call Trace.WriteLine(tMessage, (New StackFrame).GetMethod.Name)
          Else
            Call Trace.WriteLine(tMessage)
          End If
          If Not Me.m_Config.AutoClose Then Call MsgBox(tMessage, MsgBoxStyle.Critical)
        End Try
        Me.cmdSave.Enabled = True
      Else
        tMessage = "Unknown data received."
        If Verbose Then
          Call Trace.WriteLine(tMessage, (New StackFrame).GetMethod.Name)
        Else
          Call Trace.WriteLine(tMessage)
        End If
        If Not Me.m_Config.AutoClose Then Call MsgBox(tMessage, MsgBoxStyle.Information)
      End If

      Me.pbProgress.Value = 0
      Me.cmdSettings.Enabled = True
      Me.cmdGetData.Enabled = True
      Me.cmdPaste.Enabled = True
      Me.txtURL.Enabled = True

      If Me.m_Config.AutoClose AndAlso Me.m_Config.AutoDownload Then Call Me.Close()
    End Sub

    Private Sub ProgressChanged(ByVal Sender As Object,
                                ByVal ProgressPercentage As Double,
                                ByVal State As eProgressBarState)
      Call Me.BeginInvoke(New procProgressChanged(AddressOf Me.iProgressChanged), New Object() {ProgressPercentage, State})
    End Sub

    Private Delegate Sub procProgressChanged(ByVal ProgressPercentage As Double,
                                             ByVal State As eProgressBarState)

    Private Sub iProgressChanged(ByVal ProgressPercentage As Double,
                                 ByVal State As eProgressBarState)
      Call ProgressBarState.SetState(Me.pbProgress, State)

      If (Not Double.IsInfinity(ProgressPercentage)) AndAlso (Not Double.IsNaN(ProgressPercentage)) Then Me.pbProgress.Value = Convert.ToInt32(ProgressPercentage * 1000)

      Call Me.pbProgress.Refresh()

#If DEBUG Then
      Call Trace.WriteLine(String.Format("Percentage: {0}%", ProgressPercentage.ToString("N2")))
#End If
    End Sub

    Private Sub Sc2RanksClick(ByVal Sender As Object,
                              ByVal e As EventArgs) Handles lblSC2Ranks.Click
      Try
        Call Diagnostics.Process.Start("http://www.sc2ranks.com")
      Catch
        '-
      End Try
    End Sub

    Private Sub UrlTextChanged(ByVal Sender As Object,
                               ByVal e As EventArgs) Handles txtURL.TextChanged
      Dim CustomDivisionID As String = Nothing
      If ParseUrl(Me.txtURL.Text, CustomDivisionID) Then
        Me.m_Config.CustomDivisionID = CustomDivisionID
        Call Me.errProvider.SetError(Me.txtURL, Nothing)
      Else
        Call Me.errProvider.SetError(Me.txtURL, "Invalid ID or URL" + vbCrLf + "ID Example: 51e882c969d5386f5900cac4" + vbCrLf + "URL Example: http://www.sc2ranks.com/cdiv/51e882c969d5386f5900cac4/handle-with-care")
      End If

      Call Me.UpdateCurrentCustomDivisionIDLabel()
    End Sub

    Private Shared Function ParseUrl(ByVal Value As String,
                                     ByRef CustomDivisionID As String) As Boolean
      CustomDivisionID = Nothing
      Dim Erg As Boolean = False

      If Not String.IsNullOrEmpty(Value) Then
        'Sample: http://www.sc2ranks.com/cdiv/51e882c969d5386f5900cac4/handle-with-care

        Const Prefix As String = "/cdiv/"
        Const Suffix As String = "/"

        If Value.Length = 24 Then
          CustomDivisionID = Value
          Erg = True
        Else
          Try
            Dim URL As New Uri(Value)

            If URL.Host.Contains("sc2ranks.com") Then
              Dim Path As String = URL.AbsolutePath
              Dim StartIndex As Int32
              Dim EndIndex As Int32

              StartIndex = Path.IndexOf(Prefix, StringComparison.InvariantCultureIgnoreCase)

              If StartIndex <> - 1 Then
                EndIndex = Path.Substring(StartIndex + Prefix.Length).IndexOf(Suffix, StringComparison.InvariantCultureIgnoreCase)

                If (EndIndex <> - 1) AndAlso (Path.Substring(StartIndex + Prefix.Length, EndIndex).Length = 24) Then
                  CustomDivisionID = Path.Substring(StartIndex + Prefix.Length, EndIndex)
                  Erg = True
                End If
              End If
            End If
          Catch
            '-
          End Try
        End If
      End If

      Return Erg
    End Function

    Private Sub UpdateCurrentCustomDivisionIDLabel()
      Me.lblCurrentDivisionInfo.Text = "Current Custom Division ID: " + Me.m_Config.CustomDivisionID.ToString
    End Sub

    Private Sub PasteClick(ByVal Sender As Object,
                           ByVal e As EventArgs) Handles cmdPaste.Click
      Me.txtURL.Text = My.Computer.Clipboard.GetText
    End Sub

    Private Sub FormKeyDown(ByVal Sender As Object,
                            ByVal e As KeyEventArgs) Handles Me.KeyDown
      Dim Ex As Exception = Nothing
      If e.KeyCode = Keys.F1 Then Call System.Diagnostics.Process.Start("http://www.nugardt.com/open-source/sc2ranks-custom-division-profiler/")
    End Sub

    Private Sub SettingsClick(ByVal Sender As Object,
                              ByVal e As EventArgs) Handles cmdSettings.Click
      If Me.Settings Is Nothing Then
        Me.Settings = New Setting(Me.m_Config, AddressOf Me.OnSettingsClose)
        Call Me.Settings.ShowDialog(Me)
      Else
        Call Me.Settings.Show()
      End If
    End Sub

    Private Sub OnSettingsClose()
      Call Me.Settings.Dispose()
      Me.Settings = Nothing
      Call Me.UpdateCurrentCustomDivisionIDLabel()
    End Sub

    Private Sub LogoClick(ByVal Sender As Object,
                          ByVal e As EventArgs) Handles lblLogo.Click
      Try
        Call Diagnostics.Process.Start("http://www.nugardt.com/software/sc2-cdp/")
      Catch
        '-
      End Try
    End Sub
  End Class
End Namespace