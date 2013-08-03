Namespace SC2Ranks.CustomDivisionProfiler
  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class Form
    Inherits System.Windows.Forms.Form

    'Das Formular überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
      Try
        If disposing AndAlso components IsNot Nothing Then
          components.Dispose()
        End If
      Finally
        MyBase.Dispose(disposing)
      End Try
    End Sub

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
      Me.components = New System.ComponentModel.Container()
      Me.pbProgress = New System.Windows.Forms.ProgressBar()
      Me.cmdSave = New System.Windows.Forms.Button()
      Me.cmdGetData = New System.Windows.Forms.Button()
      Me.tabMain = New System.Windows.Forms.TableLayoutPanel()
      Me.lblLogo = New System.Windows.Forms.Label()
      Me.lblInfo = New System.Windows.Forms.Label()
      Me.lblVersion = New System.Windows.Forms.Label()
      Me.lblSC2Ranks = New System.Windows.Forms.Label()
      Me.txtURL = New System.Windows.Forms.TextBox()
      Me.lblURL = New System.Windows.Forms.Label()
      Me.lblCurrentDivisionInfo = New System.Windows.Forms.Label()
      Me.cmdPaste = New System.Windows.Forms.Button()
      Me.cmdSettings = New System.Windows.Forms.Button()
      Me.errProvider = New System.Windows.Forms.ErrorProvider(Me.components)
      Me.tabMain.SuspendLayout()
      CType(Me.errProvider, System.ComponentModel.ISupportInitialize).BeginInit()
      Me.SuspendLayout()
      '
      'pbProgress
      '
      Me.tabMain.SetColumnSpan(Me.pbProgress, 3)
      Me.pbProgress.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pbProgress.Location = New System.Drawing.Point(3, 81)
      Me.pbProgress.Name = "pbProgress"
      Me.pbProgress.Size = New System.Drawing.Size(378, 20)
      Me.pbProgress.TabIndex = 3
      '
      'cmdSave
      '
      Me.cmdSave.Dock = System.Windows.Forms.DockStyle.Fill
      Me.cmdSave.Enabled = False
      Me.cmdSave.Location = New System.Drawing.Point(131, 55)
      Me.cmdSave.Name = "cmdSave"
      Me.cmdSave.Size = New System.Drawing.Size(122, 20)
      Me.cmdSave.TabIndex = 2
      Me.cmdSave.Text = "Save to File"
      Me.cmdSave.UseVisualStyleBackColor = True
      '
      'cmdGetData
      '
      Me.cmdGetData.Dock = System.Windows.Forms.DockStyle.Fill
      Me.cmdGetData.Location = New System.Drawing.Point(3, 55)
      Me.cmdGetData.Name = "cmdGetData"
      Me.cmdGetData.Size = New System.Drawing.Size(122, 20)
      Me.cmdGetData.TabIndex = 0
      Me.cmdGetData.Text = "Generate Stats"
      Me.cmdGetData.UseVisualStyleBackColor = True
      '
      'tabMain
      '
      Me.tabMain.ColumnCount = 4
      Me.tabMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
      Me.tabMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
      Me.tabMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
      Me.tabMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
      Me.tabMain.Controls.Add(Me.cmdGetData, 0, 2)
      Me.tabMain.Controls.Add(Me.cmdSave, 1, 2)
      Me.tabMain.Controls.Add(Me.pbProgress, 0, 3)
      Me.tabMain.Controls.Add(Me.lblLogo, 0, 4)
      Me.tabMain.Controls.Add(Me.lblInfo, 1, 4)
      Me.tabMain.Controls.Add(Me.lblVersion, 0, 5)
      Me.tabMain.Controls.Add(Me.lblSC2Ranks, 2, 4)
      Me.tabMain.Controls.Add(Me.txtURL, 1, 0)
      Me.tabMain.Controls.Add(Me.lblURL, 0, 0)
      Me.tabMain.Controls.Add(Me.lblCurrentDivisionInfo, 0, 1)
      Me.tabMain.Controls.Add(Me.cmdPaste, 2, 1)
      Me.tabMain.Controls.Add(Me.cmdSettings, 2, 2)
      Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
      Me.tabMain.Location = New System.Drawing.Point(0, 0)
      Me.tabMain.Margin = New System.Windows.Forms.Padding(0)
      Me.tabMain.Name = "tabMain"
      Me.tabMain.RowCount = 7
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 128.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
      Me.tabMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
      Me.tabMain.Size = New System.Drawing.Size(385, 254)
      Me.tabMain.TabIndex = 2
      '
      'lblLogo
      '
      Me.lblLogo.Cursor = System.Windows.Forms.Cursors.Hand
      Me.lblLogo.Dock = System.Windows.Forms.DockStyle.Fill
      Me.lblLogo.Image = Global.NuGardt.My.Resources.Resources.PNG_HwC_Logo
      Me.lblLogo.Location = New System.Drawing.Point(3, 104)
      Me.lblLogo.Name = "lblLogo"
      Me.lblLogo.Size = New System.Drawing.Size(122, 128)
      Me.lblLogo.TabIndex = 6
      '
      'lblInfo
      '
      Me.lblInfo.AutoSize = True
      Me.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill
      Me.lblInfo.Location = New System.Drawing.Point(131, 104)
      Me.lblInfo.Name = "lblInfo"
      Me.lblInfo.Size = New System.Drawing.Size(122, 128)
      Me.lblInfo.TabIndex = 7
      Me.lblInfo.Text = "1. Click on 'Get Data' to obtain all relevant data from SC2Ranks.com" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2. Click 'S" & _
      "ave to File' to write the data formatted into a HTML file." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
      '
      'lblVersion
      '
      Me.lblVersion.AutoSize = True
      Me.tabMain.SetColumnSpan(Me.lblVersion, 3)
      Me.lblVersion.Dock = System.Windows.Forms.DockStyle.Fill
      Me.lblVersion.Location = New System.Drawing.Point(3, 232)
      Me.lblVersion.Name = "lblVersion"
      Me.lblVersion.Size = New System.Drawing.Size(378, 20)
      Me.lblVersion.TabIndex = 8
      Me.lblVersion.TextAlign = System.Drawing.ContentAlignment.BottomLeft
      '
      'lblSC2Ranks
      '
      Me.lblSC2Ranks.Cursor = System.Windows.Forms.Cursors.Hand
      Me.lblSC2Ranks.Dock = System.Windows.Forms.DockStyle.Fill
      Me.lblSC2Ranks.Image = Global.NuGardt.My.Resources.Resources.PNG_SC2Ranks
      Me.lblSC2Ranks.Location = New System.Drawing.Point(259, 104)
      Me.lblSC2Ranks.Name = "lblSC2Ranks"
      Me.lblSC2Ranks.Size = New System.Drawing.Size(122, 128)
      Me.lblSC2Ranks.TabIndex = 9
      '
      'txtURL
      '
      Me.tabMain.SetColumnSpan(Me.txtURL, 2)
      Me.txtURL.Dock = System.Windows.Forms.DockStyle.Fill
      Me.txtURL.Location = New System.Drawing.Point(131, 3)
      Me.txtURL.MaxLength = 255
      Me.txtURL.Name = "txtURL"
      Me.txtURL.Size = New System.Drawing.Size(250, 20)
      Me.txtURL.TabIndex = 11
      '
      'lblURL
      '
      Me.lblURL.AutoSize = True
      Me.lblURL.Dock = System.Windows.Forms.DockStyle.Fill
      Me.lblURL.Location = New System.Drawing.Point(3, 0)
      Me.lblURL.Name = "lblURL"
      Me.lblURL.Size = New System.Drawing.Size(122, 26)
      Me.lblURL.TabIndex = 12
      Me.lblURL.Text = "Division ID or URL:"
      Me.lblURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      '
      'lblCurrentDivisionInfo
      '
      Me.lblCurrentDivisionInfo.AutoSize = True
      Me.tabMain.SetColumnSpan(Me.lblCurrentDivisionInfo, 2)
      Me.lblCurrentDivisionInfo.Dock = System.Windows.Forms.DockStyle.Fill
      Me.lblCurrentDivisionInfo.Location = New System.Drawing.Point(3, 26)
      Me.lblCurrentDivisionInfo.Name = "lblCurrentDivisionInfo"
      Me.lblCurrentDivisionInfo.Size = New System.Drawing.Size(250, 26)
      Me.lblCurrentDivisionInfo.TabIndex = 10
      Me.lblCurrentDivisionInfo.Text = "Current Division ID: 7085"
      Me.lblCurrentDivisionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
      '
      'cmdPaste
      '
      Me.cmdPaste.Dock = System.Windows.Forms.DockStyle.Fill
      Me.cmdPaste.Location = New System.Drawing.Point(259, 29)
      Me.cmdPaste.Name = "cmdPaste"
      Me.cmdPaste.Size = New System.Drawing.Size(122, 20)
      Me.cmdPaste.TabIndex = 13
      Me.cmdPaste.Text = "Paste from clipboard"
      Me.cmdPaste.UseVisualStyleBackColor = True
      '
      'cmdSettings
      '
      Me.cmdSettings.Dock = System.Windows.Forms.DockStyle.Fill
      Me.cmdSettings.Location = New System.Drawing.Point(259, 55)
      Me.cmdSettings.Name = "cmdSettings"
      Me.cmdSettings.Size = New System.Drawing.Size(122, 20)
      Me.cmdSettings.TabIndex = 22
      Me.cmdSettings.Text = "Settings"
      Me.cmdSettings.UseVisualStyleBackColor = True
      '
      'errProvider
      '
      Me.errProvider.ContainerControl = Me
      Me.errProvider.RightToLeft = True
      '
      'Form
      '
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(385, 254)
      Me.Controls.Add(Me.tabMain)
      Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
      Me.KeyPreview = True
      Me.MaximizeBox = False
      Me.MinimizeBox = False
      Me.Name = "Form"
      Me.Text = "NuGardt SC2Ranks Custom Division Profiler"
      Me.tabMain.ResumeLayout(False)
      Me.tabMain.PerformLayout()
      CType(Me.errProvider, System.ComponentModel.ISupportInitialize).EndInit()
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbProgress As System.Windows.Forms.ProgressBar
    Friend WithEvents tabMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cmdGetData As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents lblLogo As System.Windows.Forms.Label
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents lblSC2Ranks As System.Windows.Forms.Label
    Friend WithEvents txtURL As System.Windows.Forms.TextBox
    Friend WithEvents lblURL As System.Windows.Forms.Label
    Friend WithEvents lblCurrentDivisionInfo As System.Windows.Forms.Label
    Friend WithEvents cmdPaste As System.Windows.Forms.Button
    Friend WithEvents errProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents cmdSettings As System.Windows.Forms.Button

  End Class
End Namespace