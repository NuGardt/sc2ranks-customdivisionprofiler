Namespace SC2Ranks.CustomDivisionProfiler
  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Partial Class Setting

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
      Me.pgSettings = New System.Windows.Forms.PropertyGrid()
      Me.panMain = New System.Windows.Forms.TableLayoutPanel()
      Me.cmdClose = New System.Windows.Forms.Button()
      Me.panMain.SuspendLayout()
      Me.SuspendLayout()
      '
      'pgSettings
      '
      Me.panMain.SetColumnSpan(Me.pgSettings, 2)
      Me.pgSettings.Dock = System.Windows.Forms.DockStyle.Fill
      Me.pgSettings.Location = New System.Drawing.Point(3, 3)
      Me.pgSettings.Name = "pgSettings"
      Me.pgSettings.Size = New System.Drawing.Size(308, 380)
      Me.pgSettings.TabIndex = 0
      '
      'panMain
      '
      Me.panMain.ColumnCount = 2
      Me.panMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
      Me.panMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106.0!))
      Me.panMain.Controls.Add(Me.pgSettings, 0, 0)
      Me.panMain.Controls.Add(Me.cmdClose, 1, 1)
      Me.panMain.Dock = System.Windows.Forms.DockStyle.Fill
      Me.panMain.Location = New System.Drawing.Point(0, 0)
      Me.panMain.Name = "panMain"
      Me.panMain.RowCount = 2
      Me.panMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
      Me.panMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
      Me.panMain.Size = New System.Drawing.Size(314, 412)
      Me.panMain.TabIndex = 1
      '
      'cmdClose
      '
      Me.cmdClose.Dock = System.Windows.Forms.DockStyle.Fill
      Me.cmdClose.Location = New System.Drawing.Point(211, 389)
      Me.cmdClose.Name = "cmdClose"
      Me.cmdClose.Size = New System.Drawing.Size(100, 20)
      Me.cmdClose.TabIndex = 1
      Me.cmdClose.Text = "Close"
      Me.cmdClose.UseVisualStyleBackColor = True
      '
      'Setting
      '
      Me.AcceptButton = Me.cmdClose
      Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
      Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
      Me.ClientSize = New System.Drawing.Size(314, 412)
      Me.Controls.Add(Me.panMain)
      Me.Name = "Setting"
      Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
      Me.Text = "NuGardt SC2Ranks Custom Division Profiler - Settings"
      Me.panMain.ResumeLayout(False)
      Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pgSettings As System.Windows.Forms.PropertyGrid
    Friend WithEvents panMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents cmdClose As System.Windows.Forms.Button
  End Class
End Namespace