<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormModificarCursos
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
        Me.NUEVO = New System.Windows.Forms.Button()
        Me.EDITAR = New System.Windows.Forms.Button()
        Me.SALIR = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.CREDITOS = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.NOMCURSO = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CODCURSO = New System.Windows.Forms.TextBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'NUEVO
        '
        Me.NUEVO.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NUEVO.Image = Global.SistemaPagos.My.Resources.Resources.Add2
        Me.NUEVO.Location = New System.Drawing.Point(15, 14)
        Me.NUEVO.Name = "NUEVO"
        Me.NUEVO.Size = New System.Drawing.Size(132, 51)
        Me.NUEVO.TabIndex = 3
        Me.NUEVO.Text = "NUEVO"
        Me.NUEVO.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.NUEVO.UseVisualStyleBackColor = True
        '
        'EDITAR
        '
        Me.EDITAR.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EDITAR.Image = Global.SistemaPagos.My.Resources.Resources.Save
        Me.EDITAR.Location = New System.Drawing.Point(15, 71)
        Me.EDITAR.Name = "EDITAR"
        Me.EDITAR.Size = New System.Drawing.Size(132, 51)
        Me.EDITAR.TabIndex = 4
        Me.EDITAR.Text = "GRABAR"
        Me.EDITAR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.EDITAR.UseVisualStyleBackColor = True
        '
        'SALIR
        '
        Me.SALIR.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SALIR.Image = Global.SistemaPagos.My.Resources.Resources.Close
        Me.SALIR.Location = New System.Drawing.Point(15, 128)
        Me.SALIR.Name = "SALIR"
        Me.SALIR.Size = New System.Drawing.Size(132, 51)
        Me.SALIR.TabIndex = 7
        Me.SALIR.Text = "SALIR"
        Me.SALIR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.SALIR.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.CREDITOS)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.NOMCURSO)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.CODCURSO)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(357, 192)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "DATOS DEL CURSO"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(13, 122)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(98, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "N° DE CREDITOS"
        '
        'CREDITOS
        '
        Me.CREDITOS.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.CREDITOS.Location = New System.Drawing.Point(13, 141)
        Me.CREDITOS.Name = "CREDITOS"
        Me.CREDITOS.Size = New System.Drawing.Size(135, 22)
        Me.CREDITOS.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "NOMBRE DEL CURSO"
        '
        'NOMCURSO
        '
        Me.NOMCURSO.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.NOMCURSO.Location = New System.Drawing.Point(13, 91)
        Me.NOMCURSO.Name = "NOMCURSO"
        Me.NOMCURSO.Size = New System.Drawing.Size(301, 22)
        Me.NOMCURSO.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(59, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "CODIGO"
        '
        'CODCURSO
        '
        Me.CODCURSO.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.CODCURSO.Location = New System.Drawing.Point(13, 43)
        Me.CODCURSO.Name = "CODCURSO"
        Me.CODCURSO.Size = New System.Drawing.Size(135, 22)
        Me.CODCURSO.TabIndex = 1
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.SALIR)
        Me.GroupBox4.Controls.Add(Me.NUEVO)
        Me.GroupBox4.Controls.Add(Me.EDITAR)
        Me.GroupBox4.Location = New System.Drawing.Point(375, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(163, 192)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = " "
        '
        'FormModificarCursos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(563, 224)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FormModificarCursos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MODIFICAR CURSOS"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents NUEVO As System.Windows.Forms.Button
    Friend WithEvents EDITAR As System.Windows.Forms.Button
    Friend WithEvents SALIR As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CREDITOS As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents NOMCURSO As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CODCURSO As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
End Class
