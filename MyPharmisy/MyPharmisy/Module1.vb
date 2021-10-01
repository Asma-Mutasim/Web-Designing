Module Module1

    Public conStr As String = "Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\User\Documents\mydb.mdf;Integrated Security=True;Connect Timeout=30"
    Public con As New SqlClient.SqlConnection(conStr)
End Module
