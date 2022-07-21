Public Class FormMDI

    Private Sub NuevosCursosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NuevosCursosToolStripMenuItem.Click
        FormIngCursos.MdiParent = Me
        FormIngCursos.Show()

    End Sub
End Class