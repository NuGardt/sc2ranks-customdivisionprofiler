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
Imports NuGardt.SC2Ranks.CustomDivisionProfiler.Jobs
Imports NuGardt.Core
Imports System.Threading
Imports NuGardt.Core.Process
Imports System.Text

Namespace SC2Ranks.CustomDivisionProfiler
  Module StartUp
    Public Verbose As Boolean
    Public ConsoleMode As Boolean
    Public Config As IConfig

    Public Trace As TraceListener

    Private Sub TraceRelay(ByVal Message As String)
      Call System.Console.Write(Message)
    End Sub

    Public Sub Main()
      Dim TraceListener As TraceListener
      Dim LogStream As Stream = Nothing

      Dim cmdl As New CommandLine
      Dim tString As String = Nothing

      If cmdl.IsSet("log") Then
        Try
          If cmdl.GetValues("logfilename", tString) Then
            LogStream = New FileStream(tString + ".log", FileMode.Append, FileAccess.Write, FileShare.Read)
          Else
            LogStream = New FileStream(My.Application.Info.AssemblyName + ".log", FileMode.Append, FileAccess.Write, FileShare.Read)
          End If
        Catch
          '-
        End Try

        TraceListener = New TraceListener(LogStream, AddressOf TraceRelay)
      Else
        TraceListener = New TraceListener(AddressOf TraceRelay)
      End If

      Trace = TraceListener
      Call Diagnostics.Trace.Listeners.Add(TraceListener)
      Diagnostics.Trace.AutoFlush = True

      If cmdl.IsSet("verbose") OrElse cmdl.IsSet("v") Then
        Verbose = True
      Else
        Verbose = False
      End If

      Call Trace.WriteLine(String.Format("{0} - v{1}", My.Application.Info.Title, My.Application.Info.Version.ToString))
      Call Trace.WriteLine(My.Application.Info.Copyright)
      If Verbose Then Call Trace.WriteSystemInformation()

      Dim RunningMono As Boolean = Mono.IsRunningOnMono()

      If RunningMono Then Call Trace.WriteLine("Mono runtime detected: " + Mono.GetMonoRuntimeVersion())

      If Not CustomDivisionProfiler.Config.TryParse(cmdl, Config) Then Config = New Config

      If cmdl.IsSet("commandline") OrElse cmdl.IsSet("cl") Then
        'Command Line
        ConsoleMode = True
        Dim Profile As Job(Of EventWaitHandle)
        Dim EventHandle As New EventWaitHandle(False, EventResetMode.AutoReset)
        Dim PM = New Manager(Of EventWaitHandle)(1)

        Call Trace.WriteLine("Profiling...")
        Profile = New Profiler(Of EventWaitHandle)(EventHandle, Config, Nothing, AddressOf OnCompleteProfiler)

        Call PM.Add(Profile)

        Call EventHandle.WaitOne()
        Call PM.Dispose()
      Else
        'GUI
        ConsoleMode = False

        Try
          Call Application.EnableVisualStyles()
          Call Application.Run(New Form)
        Catch iEx As Exception
          If RunningMono Then Call Trace.WriteLine("An x-server is required when running this app in GUI (Forms) mode. Try using '/commandline'.")
          Call Trace.WriteLine(iEx)
        End Try
      End If

      Call Diagnostics.Trace.Listeners.Remove(TraceListener)
      Call TraceListener.Dispose()

      If LogStream IsNot Nothing Then
        With LogStream
          Call .Flush()
          Call .Close()
          Call .Dispose()
        End With
      End If

      Environment.ExitCode = 0
    End Sub

    Private ReadOnly OneHundredMilliseconds As TimeSpan = TimeSpan.FromMilliseconds(100)

    Friend Sub Sleep(Of TKey)(ByVal SleepTime As TimeSpan,
                              ByVal Key As TKey,
                              ByVal OnCancelPending As procOnCancelPending(Of TKey))
      If SleepTime < OneHundredMilliseconds Then
        Call Thread.Sleep(SleepTime)
      Else
        Dim CurrentSleepTime As Int64 = 0
        Dim TargetSleepTime As Int64 = Convert.ToInt64(SleepTime.TotalMilliseconds)

        Do Until CurrentSleepTime >= TargetSleepTime
          Call Thread.Sleep(100)
          If OnCancelPending IsNot Nothing AndAlso OnCancelPending.Invoke(Nothing, Key) Then Exit Sub
          CurrentSleepTime += 100
        Loop
      End If
    End Sub

    Private Sub OnCompleteProfiler(ByVal Job As Job(Of EventWaitHandle))
      Dim tJob As Profiler(Of EventWaitHandle) = TryCast(Job, Profiler(Of EventWaitHandle))

      If (tJob IsNot Nothing) Then
        If (Not String.IsNullOrEmpty(tJob.Output)) Then
          Dim Stream As Stream
          Stream = New FileStream(Path.Combine(tJob.Config.OutputFolder, tJob.Config.OutputFileName), FileMode.Create, FileAccess.Write, FileShare.Read)
          Dim Buffer() As Byte

          Buffer = Encoding.UTF8.GetBytes(tJob.Output)

          Call tJob.Dispose()

          Call Stream.Write(Buffer, 0, Buffer.Length)

          With Stream
            Call .Flush()
            Call .Close()
            Call .Dispose()
          End With

          Call Trace.WriteLine("Profiling completed.")
          Call Trace.WriteLine("Credits used: " + tJob.CreditsUsed.ToString())
        Else
          Call Trace.WriteLine("Profiling failed with no result output.")
          If (tJob.Error IsNot Nothing) Then Call Trace.WriteLine(tJob.Error)
        End If
      End If

      Call Job.ProcessTag.Set()
    End Sub
  End Module
End Namespace