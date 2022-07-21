<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormIngCursos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FormIngCursos))
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.DatCProgramas = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.NUEVO = New System.Windows.Forms.Button()
        Me.GUARDAR = New System.Windows.Forms.Button()
        Me.ELIMINA = New System.Windows.Forms.Button()
        Me.CERRAR = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.CODIGO = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.NOMBRE = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.NCREDITOS = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.mshCursos = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.mshCursos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.DatCProgramas)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(351, 78)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        '
        'DatCProgramas
        '
        Me.DatCProgramas.FormattingEnabled = True
        Me.DatCProgramas.Location = New System.Drawing.Point(13, 39)
        Me.DatCProgramas.Name = "DatCProgramas"
        Me.DatCProgramas.Size = New System.Drawing.Size(301, 25)
        Me.DatCProgramas.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "PROGRAMA"
        '
        'NUEVO
        '
        Me.NUEVO.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.NUEVO.Image = Global.SistemaPagos.My.Resources.Resources.Add2
        Me.NUEVO.Location = New System.Drawing.Point(18, 21)
        Me.NUEVO.Name = "NUEVO"
        Me.NUEVO.Size = New System.Drawing.Size(132, 51)
        Me.NUEVO.TabIndex = 3
        Me.NUEVO.Text = "NUEVO"
        Me.NUEVO.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.NUEVO.UseVisualStyleBackColor = True
        '
        'GUARDAR
        '
        Me.GUARDAR.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GUARDAR.Image = Global.SistemaPagos.My.Resources.Resources.Save
        Me.GUARDAR.Location = New System.Drawing.Point(156, 21)
        Me.GUARDAR.Name = "GUARDAR"
        Me.GUARDAR.Size = New System.Drawing.Size(132, 51)
        Me.GUARDAR.TabIndex = 4
        Me.GUARDAR.Text = "GRABAR"
        Me.GUARDAR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.GUARDAR.UseVisualStyleBackColor = True
        '
        'ELIMINA
        '
        Me.ELIMINA.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ELIMINA.Image = Global.SistemaPagos.My.Resources.Resources.Delete
        Me.ELIMINA.Location = New System.Drawing.Point(623, 295)
        Me.ELIMINA.Name = "ELIMINA"
        Me.ELIMINA.Size = New System.Drawing.Size(132, 51)
        Me.ELIMINA.TabIndex = 5
        Me.ELIMINA.Text = "ELIMINAR"
        Me.ELIMINA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ELIMINA.UseVisualStyleBackColor = True
        '
        'CERRAR
        '
        Me.CERRAR.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CERRAR.Image = Global.SistemaPagos.My.Resources.Resources.Close
        Me.CERRAR.Location = New System.Drawing.Point(294, 21)
        Me.CERRAR.Name = "CERRAR"
        Me.CERRAR.Size = New System.Drawing.Size(132, 51)
        Me.CERRAR.TabIndex = 7
        Me.CERRAR.Text = "SALIR"
        Me.CERRAR.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.CERRAR.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.mshCursos)
        Me.GroupBox3.Location = New System.Drawing.Point(369, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(386, 277)
        Me.GroupBox3.TabIndex = 1
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "LISTADO DE CURSOS"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.CERRAR)
        Me.GroupBox4.Controls.Add(Me.NUEVO)
        Me.GroupBox4.Controls.Add(Me.GUARDAR)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 295)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(439, 86)
        Me.GroupBox4.TabIndex = 1
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "OPCIONES"
        '
        'CODIGO
        '
        Me.CODIGO.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.CODIGO.Location = New System.Drawing.Point(13, 43)
        Me.CODIGO.Name = "CODIGO"
        Me.CODIGO.Size = New System.Drawing.Size(135, 22)
        Me.CODIGO.TabIndex = 1
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
        'NOMBRE
        '
        Me.NOMBRE.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.NOMBRE.Location = New System.Drawing.Point(13, 91)
        Me.NOMBRE.Name = "NOMBRE"
        Me.NOMBRE.Size = New System.Drawing.Size(301, 22)
        Me.NOMBRE.TabIndex = 1
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
        'NCREDITOS
        '
        Me.NCREDITOS.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.NCREDITOS.Location = New System.Drawing.Point(13, 141)
        Me.NCREDITOS.Name = "NCREDITOS"
        Me.NCREDITOS.Size = New System.Drawing.Size(135, 22)
        Me.NCREDITOS.TabIndex = 5
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
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.NCREDITOS)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.NOMBRE)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.CODIGO)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 97)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(357, 192)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "DATOS DEL CURSO"
        '
        'mshCursos
        '
        Me.mshCursos.Location = New System.Drawing.Point(7, 22)
        Me.mshCursos.Name = "mshCursos"
        Me.mshCursos.OcxState = CType(resources.GetObject("mshCursos.OcxState"), System.Windows.Forms.AxHost.State)
        Me.mshCursos.Size = New System.Drawing.Size(362, 226)
        Me.mshCursos.TabIndex = 0
        '
        'FormIngCursos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 407)
        Me.Controls.Add(Me.ELIMINA)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Font = New System.Drawing.Font("Century Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "FormIngCursos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "INGRESO DE CURSOS"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.mshCursos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents DatCProgramas As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents NUEVO As System.Windows.Forms.Button
    Friend WithEvents GUARDAR As System.Windows.Forms.Button
    Friend WithEvents ELIMINA As System.Windows.Forms.Button
    Friend WithEvents CERRAR As System.Windows.Forms.Button
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents CODIGO As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents NOMBRE As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents NCREDITOS As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents mshCursos As AxMSFlexGridLib.AxMSFlexGrid
End Class
