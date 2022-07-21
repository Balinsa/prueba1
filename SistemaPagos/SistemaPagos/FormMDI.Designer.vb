<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormMDI
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SistemasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CursosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PlanDeEstudiosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ProfesoresToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AulasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.NuevosCursosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditarCursosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SistemasToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(623, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SistemasToolStripMenuItem
        '
        Me.SistemasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CursosToolStripMenuItem, Me.ToolStripSeparator1, Me.PlanDeEstudiosToolStripMenuItem, Me.ToolStripSeparator2, Me.ProfesoresToolStripMenuItem, Me.ToolStripSeparator3, Me.AulasToolStripMenuItem, Me.ToolStripSeparator4, Me.SalirToolStripMenuItem})
        Me.SistemasToolStripMenuItem.Name = "SistemasToolStripMenuItem"
        Me.SistemasToolStripMenuItem.Size = New System.Drawing.Size(65, 20)
        Me.SistemasToolStripMenuItem.Text = "&Sistemas"
        '
        'CursosToolStripMenuItem
        '
        Me.CursosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NuevosCursosToolStripMenuItem, Me.EditarCursosToolStripMenuItem})
        Me.CursosToolStripMenuItem.Name = "CursosToolStripMenuItem"
        Me.CursosToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.CursosToolStripMenuItem.Text = "&Cursos"
        '
        'PlanDeEstudiosToolStripMenuItem
        '
        Me.PlanDeEstudiosToolStripMenuItem.Name = "PlanDeEstudiosToolStripMenuItem"
        Me.PlanDeEstudiosToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.PlanDeEstudiosToolStripMenuItem.Text = "&Plan de Estudios"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(157, 6)
        '
        'ProfesoresToolStripMenuItem
        '
        Me.ProfesoresToolStripMenuItem.Name = "ProfesoresToolStripMenuItem"
        Me.ProfesoresToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.ProfesoresToolStripMenuItem.Text = "P&rofesores"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(157, 6)
        '
        'AulasToolStripMenuItem
        '
        Me.AulasToolStripMenuItem.Name = "AulasToolStripMenuItem"
        Me.AulasToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.AulasToolStripMenuItem.Text = "&Aulas"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(157, 6)
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.SalirToolStripMenuItem.Text = "&Salir"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(157, 6)
        '
        'NuevosCursosToolStripMenuItem
        '
        Me.NuevosCursosToolStripMenuItem.Name = "NuevosCursosToolStripMenuItem"
        Me.NuevosCursosToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.NuevosCursosToolStripMenuItem.Text = "&Nuevos Cursos"
        '
        'EditarCursosToolStripMenuItem
        '
        Me.EditarCursosToolStripMenuItem.Name = "EditarCursosToolStripMenuItem"
        Me.EditarCursosToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.EditarCursosToolStripMenuItem.Text = "&Editar Cursos"
        '
        'FormMDI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(623, 341)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FormMDI"
        Me.Text = "FormMDI"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SistemasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CursosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents PlanDeEstudiosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ProfesoresToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents AulasToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SalirToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NuevosCursosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditarCursosToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
