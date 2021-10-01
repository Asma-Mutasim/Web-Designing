


Imports System.Data.SqlClient

Public Class Form1
    Dim dt As Date = Date.Now() ' تعريف متغير التاريخ
    Dim SQLstr As String
    Dim cmd As SqlCommand = New SqlCommand
    Dim DataSetC As New DataSet
    Dim DataSet1 As New DataSet
    Dim DataSetC1 As New DataSet
    Dim DataSetC2 As New DataSet
    Dim SQLC As String = "SELECT * FROM itemtble"
    Dim SQLC1 As String = "SELECT * FROM medrble"
    Dim SQLC2 As String = "SELECT * FROM supply"
    Dim DataAdapter2 As SqlDataAdapter
    Dim dataset2 As New DataSet
    Dim dataset3 As New DataSet
    Dim DataAdapter3 As SqlDataAdapter
    Dim dataset4 As New DataSet
    Dim sqlstr2 As String
    Dim sqlstr3 As String
    Dim sqlstr4 As String
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadGrd()
        loadGrd2()
        loadGrd1()
        On Error Resume Next
        Me.CenterToScreen()
        DateTimePicker1.Value = dt 'متغير التاريخ
        SQLstr = "SELECT * FROM medrble"
        con.Open()
        Dim DataAdapter1 As New SqlDataAdapter(SQLstr, con)
        DataAdapter1.Fill(DataSet1, "medrble") 'التعامل مع جدول الاصناف
        con.Close()
        DataGridView2.DataSource = DataSet1
        DataGridView2.DataMember = "medrble"
        DataGridView2.Refresh()
        ''''''''''''''
        sqlstr2 = "select * from buy" ' التعامل مع جدول الفواتير
        con.Open()
        Dim DataAdapter2 As New SqlDataAdapter(sqlstr2, con)
        DataAdapter2.Fill(dataset2, "buy")
        con.Close()
        'DataGridView1.DataSource = dataset2
        ' DataGridView1.DataMember = "buy"
        'DataGridView1.Refresh()
        TextBox5.DataBindings.Add("Text", dataset2, "buy.Id")
        '''''' 
        sqlstr3 = "select * from bill "
        con.Open()
        Dim DataAdapter3 As New SqlDataAdapter(sqlstr3, con)
        DataAdapter3.Fill(dataset3, "bill")
        con.Close()
        Call add_number() 'لوضع رقم اخر فاتورة غي شاشة المبيعات   
        '''''''' عرض بيانات العملاء في كمبو بوكس
        sqlstr4 = "select * from supply"
        con.Open()
        Dim dataadaper As New SqlDataAdapter(sqlstr4, con)
        dataadaper.Fill(dataset4, "supply")
        con.Close()
        For i As Integer = 0 To Me.BindingContext(dataset4, "supply").Count - 1
            Dim AddStr As String = dataset4.Tables("supply").Rows(i).Item(1).ToString
            If AddStr <> "" Then ComboBox2.Items.Add(AddStr)
        Next
        '''''''
        ComboBox1.Items.Add(" مبيعات")
        ComboBox1.Items.Add("مشتريات")
        '''''''''''''''''''''''''
        txtItem.DataBindings.Add("Text", DataSet1, "medrble.item_name")
        'txtprice.DataBindings.Add("text", DataSet1, "item.sail2")
        txtcost.DataBindings.Add("text", DataSet1, "medrble.cost") 'لعرض تكلفة مبيعات الصنف
        ' كود لجعل فاتورة المشتريات التكلفة تساوي سعر الشراء
        If ComboBox1.Text = "مشتريات" Then 'للتحويل من فاتورة مبيعات الي فاتورة مشتريات
            txtprice.DataBindings.Add("text", DataSet1, "medrble.cost") 'لعرض تكلفة مبيعات الصنف
            'Else
            '    txtprice.DataBindings.Add("text", DataSet1, "medtble.sail2")
        End If
        Exit Sub
        Call dgv2() ' تنسيق الداتا جريد الخاصة بالاصناف
        Call search() ';كود عرض الفاتورة بالرقم
        Call dgv1() ' تصم الداتا جريد فيو الخاصة بالفاتورة البيع
        Call tot() '  البيع حساب اجمالي الفاتورة
        Call SearchBill() 'عرض اسم العميل الخاص بالفاتورة
    End Sub
    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        gb1.Visible = True
        gb2.Visible = False
        gb3.Visible = False
        gbsell.Visible = False
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        gb2.Visible = True
        gb1.Visible = False
        gb3.Visible = False
        gbsell.Visible = False
    End Sub
    Sub clear()
        txtname.Text = ""
        txtdate.Text = ""
        idtxt.Text = ""
    End Sub
    Sub loadGrd()
        DataSetC.Clear()
        If con.State() Then con.Close()
        con.Open()

        Dim DataAdapterS As New SqlDataAdapter(SQLC, con)
        DataAdapterS.Fill(DataSetC, "itemtble")
        con.Close()

        GV.DataSource = DataSetC
        GV.DataMember = "itemtble"
        GV.Refresh()

        GV.Columns(0).HeaderText = "الرقم"
        GV.Columns(1).HeaderText = "اسم الصنف"
        GV.Columns(2).HeaderText = "التاريخ"

    End Sub
    Private Sub bt_Click(sender As Object, e As EventArgs) Handles bt.Click



        If txtname.Text = "" Then
            MsgBox("خطأ هناك احد الحقول الأساسية فارغة")
            Exit Sub
        End If
        Try
            If con.State() Then con.Close()
            con.Open()
            Dim cmd As SqlCommand = New SqlCommand("INSERT INTO itemtble (Id,cat_name,cat_date) VALUES (@id,@cat_name,@cat_date)", con)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("id", idtxt.Text)
            cmd.Parameters.AddWithValue("cat_name", txtname.Text)
            cmd.Parameters.AddWithValue("cat_date", txtdate.Text)

            cmd.ExecuteNonQuery()
            MsgBox("تم حفظ البيانات بنجاح")
            clear()
            loadGrd()

        Catch ex As Exception
            MsgBox("مشكلة غير معروفة")

        End Try





    End Sub




    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        idtxt.Enabled = False
        Dim search As String = InputBox("ادخل رقم الصنف")
        If search <> "" Then
            Try

                If con.State() Then con.Close()
                con.Open()

                cmd.CommandText = "SELECT Id, cat_name, cat_date FROM itemtble  WHERE Id=" & CLng(search)
                cmd.Connection = con

                Dim dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read = True Then
                    'Do While drOLEDB.Read TextBox9.Text=search
                    search &= dr.Item(0).ToString & "" & dr.Item(1).ToString & "" & dr.Item(2).ToString
                    idtxt.Text = dr.Item(0).ToString
                    txtdate.Text = dr.Item(2).ToString
                    txtname.Text = dr.Item(1).ToString


                    dr.Close()
                    con.Close()
                    Exit Sub
                    con.Close()

                End If
            Catch ex As Exception
                MsgBox("حاول مرة اخرى")
            End Try
        Else
            MsgBox("ادخل الرقم")
            Exit Sub
        End If
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If txtname.Text = "" Then
            MsgBox("يجب البحث عن السجل أولاً")
            Exit Sub
        End If

        If MsgBox("هل تريد الحذف حقاً؟", MsgBoxStyle.OkCancel, "تأكيد التأكيد") = MsgBoxResult.Cancel Then
            Exit Sub
        End If

        Try
            con.Open()

            Dim cmd As New SqlCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "DELETE FROM itemtble WHERE Id = " & idtxt.Text & " "
            cmd.ExecuteScalar()
            con.Close()
            MsgBox("تم الحذف بنجاح")

            LoadGrd()
            clear()


        Catch ex As Exception
            MsgBox("خطأ غير معروف")
            Exit Sub
        End Try
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If txtname.Text = "" Then
            MsgBox("يجب البحث عن السجل أولاً")
            Exit Sub
        End If


        con.Open()
        cmd.Connection = con
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "update itemtble Set cat_name = @cat_name , cat_date = @cat_date where Id = " & idtxt.Text & " "
        cmd.Parameters.AddWithValue("cat_name", txtname.Text)
        cmd.Parameters.AddWithValue("cat_date", txtdate.Text)
        cmd.ExecuteNonQuery()

        con.Close()
        MsgBox("تم تعديل البيانات بنجاح")

        clear()
        loadGrd()
    End Sub














    ' end of gb1   ..................................................................................................


    '                        ......................................................................................


    '                                  ...................................................................









    Sub clear1()
        TextBox1.Text = ""
        txtno.Text = ""
        txtamount.Text = ""
        txtcompany.Text = ""
        txtcost.Text = ""
        txtuse.Text = ""

    End Sub
    Sub loadGrd1()
        DataSetC1.Clear()
        If con.State() Then con.Close()
        con.Open()

        Dim DataAdapterS1 As New SqlDataAdapter(SQLC1, con)
        DataAdapterS1.Fill(DataSetC1, "medrble")
        con.Close()

        DataGridView1.DataSource = DataSetC1
        DataGridView1.DataMember = "medrble"
        DataGridView1.Refresh()

        DataGridView1.Columns(0).HeaderText = "الرقم"
        DataGridView1.Columns(1).HeaderText = "اسم الصنف"
        DataGridView1.Columns(2).HeaderText = "الكمية"
        DataGridView1.Columns(3).HeaderText = "الشركة"
        DataGridView1.Columns(4).HeaderText = "السعر"
        DataGridView1.Columns(5).HeaderText = "الاستعمال"
        DataGridView1.Columns(6).HeaderText = "تاريخ الانتاج"
        DataGridView1.Columns(7).HeaderText = "تاريخ الانتهاء"
        DataGridView1.Columns(5).Width = 300



    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If txtamount.Text = "" Or txtcompany.Text = "" Or txtcost.Text = "" Or txtuse.Text = "" Or TextBox1.Text = "" Then
            MsgBox("خطأ هناك احد الحقول الأساسية فارغة")
            Exit Sub
        End If
        Try
            If con.State() Then con.Close()
            con.Open()
            Dim cmd As SqlCommand = New SqlCommand("insert into medrble(Id,item_name,amount,med_comp,cost,med_use,med_start,med_end) values(@id,@itemName,@amount,@med_comp,@cost,@med_use,@med_start,@med_end)", con)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("id", txtno.Text)
            cmd.Parameters.AddWithValue("itemName", TextBox1.Text)
            cmd.Parameters.AddWithValue("amount", txtamount.Text)
            cmd.Parameters.AddWithValue("med_comp", txtcompany.Text)
            cmd.Parameters.AddWithValue("cost", txtcost.Text)
            cmd.Parameters.AddWithValue("med_use", txtuse.Text)
            cmd.Parameters.AddWithValue("med_start", txtdate1.Text)
            cmd.Parameters.AddWithValue("med_end", txt2date.Text)
            cmd.ExecuteNonQuery()
            MsgBox("تم حفظ البيانات بنجاح")
            clear1()
            loadGrd1()
        Catch ex As Exception
            MsgBox("مشكلة غير معروفة")

        End Try
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Dim search As String = InputBox("ادخل رقم الصنف")
        If search <> "" Then
            Try

                If con.State() Then con.Close()
                con.Open()

                cmd.CommandText = "Select Id,item_name,amount,med_comp,cost,med_use,med_start,med_end from medrble where Id=" & CLng(search)
                cmd.Connection = con

                Dim dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read = True Then
                    'Do While drOLEDB.Read TextBox9.Text=search
                    search &= dr.Item(0).ToString & "" & dr.Item(1).ToString & "" & dr.Item(2).ToString & "" & dr.Item(3).ToString & "" & dr.Item(4).ToString & "" & dr.Item(5).ToString & "" & dr.Item(6).ToString & "" & dr.Item(7).ToString
                    txtno.Text = dr.Item(0).ToString
                    TextBox1.Text = dr.Item(1).ToString
                    txtamount.Text = dr.Item(2).ToString
                    txtcompany.Text = dr.Item(3).ToString
                    txtcost.Text = dr.Item(4).ToString
                    txtuse.Text = dr.Item(5).ToString
                    txtdate1.Text = dr.Item(6).ToString
                    txt2date.Text = dr.Item(7).ToString
                    dr.Close()
                    con.Close()
                    Exit Sub
                    con.Close()
                End If
            Catch ex As Exception
                MsgBox("حاول مرة اخرى")
            End Try
        Else
            MsgBox("ادخل الرقم")
            Exit Sub
        End If
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        If txtno.Text = "" Then
            MsgBox("يجب البحث عن السجل أولاً")
            Exit Sub
        End If

        If MsgBox("هل تريد الحذف حقاً؟", MsgBoxStyle.OkCancel, "تأكيد التأكيد") = MsgBoxResult.Cancel Then
            Exit Sub
        End If

        Try


            con.Open()

            Dim cmd As New SqlCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "DELETE FROM medrble  WHERE Id = " & txtno.Text & " "
            cmd.ExecuteScalar()
            con.Close()
            MsgBox("تم الحذف بنجاح")

            loadGrd1()
            clear1()


        Catch ex As Exception
            MsgBox("خطأ غير معروف")
            Exit Sub
        End Try
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        If txtno.Text = "" Then
            MsgBox("يجب البحث عن السجل أولاً")
            Exit Sub
        End If


        con.Open()
        cmd.Connection = con
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "update medrble Set item_name = @itemName,amount=@amount,med_comp=@med_comp,cost=@cost,med_use=@med_use,med_start=med_start,med_end=@med_end  where Id = " & txtno.Text & " "
        cmd.Parameters.AddWithValue("itemName", TextBox1.Text)
        cmd.Parameters.AddWithValue("amount", txtamount.Text)
        cmd.Parameters.AddWithValue("med_comp", txtcompany.Text)
        cmd.Parameters.AddWithValue("cost", txtcost.Text)
        cmd.Parameters.AddWithValue("med_use", txtuse.Text)
        cmd.Parameters.AddWithValue("med_start", txtdate1.Text)
        cmd.Parameters.AddWithValue("med_end", txt2date.Text)
        cmd.ExecuteNonQuery()

        con.Close()
        MsgBox("تم تعديل البيانات بنجاح")

        clear1()
        loadGrd1()
    End Sub




    ' end of gb2  .....................................................................................................

    '...............................................................................................................

    '.............................................................................................................








    Sub clear2()
        TextBox2.Text = ""
        TextBox3.Text = ""
        txtaddress.Text = ""

    End Sub
    Sub loadGrd2()
        DataSetC2.Clear()
        If con.State() Then con.Close()
        con.Open()

        Dim DataAdapterS As New SqlDataAdapter(SQLC2, con)
        DataAdapterS.Fill(DataSetC2, "supply")
        con.Close()

        DataGridView2.DataSource = DataSetC2
        DataGridView2.DataMember = "supply"
        DataGridView2.Refresh()

        DataGridView2.Columns(0).HeaderText = "الرقم"
        DataGridView2.Columns(1).HeaderText = "اسم المورد"
        DataGridView2.Columns(2).HeaderText = "العنوان"

        DataGridView2.Columns(1).Width = 300
        DataGridView2.Columns(2).Width = 500



    End Sub


    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        If TextBox2.Text = "" Or txtaddress.Text = "" Then
            MsgBox("خطأ هناك احد الحقول الأساسية فارغة")
            Exit Sub
        End If
        Try
            If con.State() Then con.Close()
            con.Open()
            Dim cmd As SqlCommand = New SqlCommand("insert into supply(Id,amelname,address) values(@id1,@ameluName,@address1)", con)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.AddWithValue("id1", TextBox3.Text)
            cmd.Parameters.AddWithValue("ameluName", TextBox2.Text)
            cmd.Parameters.AddWithValue("address1", txtaddress.Text)

            cmd.ExecuteNonQuery()
            MsgBox("تم حفظ البيانات بنجاح")
            clear2()
            loadGrd2()
        Catch ex As Exception
            MsgBox("مشكلة غير معروفة")

        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        gb1.Visible = False
        gb2.Visible = False
        gb3.Visible = True
        gbsell.Visible = False
    End Sub



    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click

        If TextBox3.Text = "" Then
            MsgBox("يجب البحث عن السجل أولاً")
            Exit Sub
        End If

        If MsgBox("هل تريد الحذف حقاً؟", MsgBoxStyle.OkCancel, "تأكيد التأكيد") = MsgBoxResult.Cancel Then
            Exit Sub
        End If

        Try


            con.Open()

            Dim cmd As New SqlCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "DELETE FROM supply  WHERE Id = " & TextBox3.Text & " "
            cmd.ExecuteScalar()
            con.Close()
            MsgBox("تم الحذف بنجاح")

            loadGrd2()
            clear2()


        Catch ex As Exception
            MsgBox("خطأ غير معروف")
            Exit Sub
        End Try

    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        If TextBox3.Text = "" Then
            MsgBox("يجب البحث عن السجل أولاً")
            Exit Sub
        End If


        con.Open()
        cmd.Connection = con
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "update supply Set amelname = @amel1Name,address=@address  where Id = " & TextBox3.Text & " "
        cmd.Parameters.AddWithValue("amel1Name", TextBox2.Text)
        cmd.Parameters.AddWithValue("address", txtaddress.Text)

        cmd.ExecuteNonQuery()

        con.Close()
        MsgBox("تم تعديل البيانات بنجاح")

        clear2()
        loadGrd2()
    End Sub



    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        Dim search As String = InputBox("ادخل رقم الصنف")
        If search <> "" Then
            Try

                If con.State() Then con.Close()
                con.Open()

                cmd.CommandText = "Select Id,amelname,address from supply  where Id=" & CLng(search)
                cmd.Connection = con

                Dim dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read = True Then
                    'Do While drOLEDB.Read TextBox9.Text=search
                    search &= dr.Item(0).ToString & "" & dr.Item(1).ToString & "" & dr.Item(2).ToString
                    TextBox3.Text = dr.Item(0).ToString
                    TextBox2.Text = dr.Item(1).ToString
                    txtaddress.Text = dr.Item(2).ToString

                    dr.Close()
                    con.Close()
                    Exit Sub
                    con.Close()
                End If
            Catch ex As Exception
                MsgBox("حاول مرة اخرى")
            End Try
        Else
            MsgBox("ادخل الرقم")
            Exit Sub
        End If
    End Sub











    'end of gb3 .......................................................................................
    '.....................................................................................................................
    ',.......................................................................................









    Private Sub search() 'كود البحث برقم الفاتورة

        DataAdapter3 = New SqlDataAdapter("Select * From buy Where Id like'" & Trim$(txtBillNumber.Text) & "'", con)
        dataset2.Clear()
        DataAdapter3.Fill(dataset2, "buy")
        DataGridView4.DataSource = dataset2
        DataGridView4.DataMember = "buy"
        DataGridView4.Refresh()
        DataGridView4.RefreshEdit()
    End Sub
    Private Sub dgv1()
        ' On Error Resume Next
        ''''
        If DataGridView4 IsNot Nothing Then

            Dim count As Integer = 0 'كود الترقيم التلقائي للداتا جريد فيو

            While (count <= (DataGridView4.Rows.Count - 2))

                DataGridView4.Rows(count).HeaderCell.Value = String.Format((count + 1).ToString(), "0")

                count += 1

            End While

        End If

        ''''''''''''''''''''''
        DataGridView4.Columns(0).Visible = False
        DataGridView4.Columns(1).Visible = False
        DataGridView4.Columns(2).Visible = True
        DataGridView4.Columns(3).Visible = False
        DataGridView4.Columns(4).Visible = False
        DataGridView4.Columns(5).Visible = False
        DataGridView4.Columns(6).Visible = False
        DataGridView4.Columns(7).Visible = True
        DataGridView4.Columns(8).Visible = True
        DataGridView4.Columns(9).Visible = True
        DataGridView4.Columns(10).Visible = False

        '''''''''''''''''''''

        DataGridView4.Columns(2).Width = 300
        DataGridView4.Columns(7).Width = 120
        DataGridView4.Columns(8).Width = 120
        DataGridView4.Columns(9).Width = 120

        ''''''''''''''''''''''''''''''''''''''''''''''''

        DataGridView4.Columns(2).HeaderText = "أسم الصنف"
        DataGridView4.Columns(7).HeaderText = " الكمية"
        DataGridView4.Columns(8).HeaderText = "السعر "
        DataGridView4.Columns(9).HeaderText = " الاجمالي"
        '''''''''''''''''
        DataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView4.MultiSelect = False
        With Me.DataGridView4
            .RowsDefaultCellStyle.BackColor = Color.Aqua
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Azure
        End With
    End Sub
    Private Sub butEnter_Click(sender As Object, e As EventArgs) Handles butEnter.Click
        'لحفظ الاصناف بالفواتير
        If Trim(txtBillNumber.Text) <> "" And Trim(txtItem.Text) <> "" Or Trim(txtq.Text) <> "" Then
        Else
            MsgBox("يرجى إدخال البيانات", MsgBoxStyle.Critical, "خطأ في ادخال البيانات")
            Exit Sub
        End If
        If butEnter.Text = "أدخال" Then
            '  Dim sqcomand As New OleDb.SqlCommand
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Insert Into buy([Id],[itemna],[date],[qout],[sale],[totalqout],[win]) values ('" & txtBillNumber.Text & "','" & txtItem2.Text & "','" & DateTimePicker1.Text & "','" & txtq.Text & "','" & txtprice.Text & "','" & txtTotal.Text & "','" & txtWin.Text & "')"
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()

            ' If Trim(txtBillNumber.Text) <> "" Then Exit Sub' لعرض ناتج الفاتورة
            Call search()
            Call dgv1()
            Call tot()
            txtq.Text = ""
            txtprice.Text = ""
            txtItem2.Focus()
        Else
            cmd.Connection = con
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "Insert Into buy([billno],[itemna],[date],[qin],[cost],[totalqin],[win]) values ('" & txtBillNumber.Text & "','" & txtItem2.Text & "','" & DateTimePicker1.Text & "','" & txtq.Text & "','" & txtprice.Text & "','" & txtTotal.Text & "','" & txtWin.Text & "')"
            con.Open()
            cmd.ExecuteNonQuery()
            con.Close()


            Call search()
            Call dgv1billBuy() 'عرض فواتير الشراء
            Call totBuy() ' لعرض أجمالي فاتورة الشراء
            txtq.Text = ""
            txtprice.Text = ""
            txtItem2.Focus()
        End If
    End Sub
    Private Sub tot()
        Dim Total1 As Double ' حساب أجمالي الفاتورة
        Dim Row As DataGridViewRow
        For Each Row In DataGridView4.Rows
            Dim celV1 As DataGridViewTextBoxCell = Row.Cells(9)
            If IsNumeric(celV1.Value) = True Then
                Total1 += celV1.Value
            End If
        Next
        txtbilltotal.Text = Total1
    End Sub

    Private Sub dgv2() ' تنسيق الداتا حريد فيو الخاصة بالبحث عن الاصناف
        '''''''''''''''''
        DataGridView3.Columns(0).Visible = False
        DataGridView3.Columns(2).Visible = False
        DataGridView3.Columns(3).Visible = False
        DataGridView3.Columns(4).Visible = False
        DataGridView3.Columns(5).Visible = False
        DataGridView3.Columns(6).Visible = False

        DataGridView3.Columns(1).Visible = True

        DataGridView3.Columns(1).Width = 300
        ''''''''''''''''''''''''''''''''''''''''''''''''
        DataGridView3.Columns(1).HeaderText = "أسم الصنف"
        ''''
        DataGridView3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView3.MultiSelect = False
        With Me.DataGridView3
            .RowsDefaultCellStyle.BackColor = Color.Beige
            .AlternatingRowsDefaultCellStyle.BackColor = Color.White
        End With
        ''''''''''''''''''''


    End Sub
    Private Sub add_number()
        Dim cmd2 As New SqlCommand ' كود الترقيم لوضع رقم اخر فاتورة في شاشة البيعات
        cmd2.CommandType = CommandType.Text
        cmd2.Connection = con
        cmd2.CommandText = "select max (billno) from [bill] " ' كود الترقيم التلقائي يعود لا علي قيم في الجدول
        con.Open()
        Dim a As Integer = cmd2.ExecuteScalar
        con.Close()
        txtBillNumber.Text = a


    End Sub
    Private Sub SaveBill()
        'لحفظ بيانات الفاتورة
        CMD.Connection = con
        CMD.CommandType = CommandType.Text
        cmd.CommandText = "Insert Into bill([billno],[kind],[kind1],[date]) values ('" & txtBillNumber.Text & "','" & ComboBox1.Text & "','" & ComboBox2.Text & "','" & DateTimePicker1.Text & "')"
        con.Open()
        CMD.ExecuteNonQuery()
        con.Close()
        MsgBox("تم حفظ بيانات الفاتورة بنجاح يمكنك الان ادخال الاصناف في الفاتورة  ")
        butEnter.Enabled = True
        txtItem2.Focus()
        txtItem2.BackColor = Color.Yellow
    End Sub
    Private Sub SearchBill() ';,] كود البحث لعرض اسم العميل مقترن بالفاتورة الخاصة بة
        On Error Resume Next
        DataAdapter3 = New SqlDataAdapter("Select * From bill Where billno like'" & Trim$(txtBillNumber.Text) & "'", con)
        dataset3.Clear()
        DataAdapter3.Fill(dataset3, "bill")
        ComboBox2.DataBindings.Add("Text", dataset3, "bill.kind1")

    End Sub
    Private Sub dgv1billBuy()
        ' On Error Resume Next 'لعرض النتيجة في فاتورة الشراء
        ''''
        If DataGridView4 IsNot Nothing Then

            Dim count As Integer = 0 'كود الترقيم التلقائي للداتا جريد فيو

            While (count <= (DataGridView4.Rows.Count - 2))

                DataGridView4.Rows(count).HeaderCell.Value = String.Format((count + 1).ToString(), "0")

                count += 1

            End While

        End If

        ''''''''''''''''''''''
        DataGridView4.Columns(0).Visible = False
        DataGridView4.Columns(1).Visible = False
        DataGridView4.Columns(2).Visible = True
        DataGridView4.Columns(3).Visible = True
        DataGridView4.Columns(4).Visible = True
        DataGridView4.Columns(5).Visible = True
        DataGridView4.Columns(6).Visible = False
        DataGridView4.Columns(7).Visible = False
        DataGridView4.Columns(8).Visible = False
        DataGridView4.Columns(9).Visible = False
        DataGridView4.Columns(10).Visible = False

        '''''''''''''''''''''

        DataGridView4.Columns(2).Width = 300
        DataGridView4.Columns(3).Width = 120
        DataGridView4.Columns(4).Width = 120
        DataGridView4.Columns(5).Width = 120

        ''''''''''''''''''''''''''''''''''''''''''''''''

        DataGridView4.Columns(2).HeaderText = "أسم الصنف"
        DataGridView4.Columns(3).HeaderText = " الكمية"
        DataGridView4.Columns(4).HeaderText = "التكلفة "
        DataGridView4.Columns(5).HeaderText = " الاجمالي"
        '''''''''''''''''
        DataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView4.MultiSelect = False
        With Me.DataGridView4
            .RowsDefaultCellStyle.BackColor = Color.Aqua
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Azure
        End With
    End Sub
    Private Sub totBuy() 'لعرض أجمالي فاتورة الشراء
        Dim Total1 As Double ' حساب أجمالي الفاتورة
        Dim Row As DataGridViewRow
        For Each Row In DataGridView4.Rows
            Dim celV1 As DataGridViewTextBoxCell = Row.Cells(5)
            If IsNumeric(celV1.Value) = True Then
                Total1 += celV1.Value
            End If
        Next
        txtbilltotal.Text = Total1
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        gb1.Visible = False
        gb2.Visible = False
        gb3.Visible = False
        gbsell.Visible = True
    End Sub

    Private Sub butDelet_Click(sender As Object, e As EventArgs) Handles butDelet.Click
        If MsgBox(" هل تريد حذف الصنف الحالي  ", MsgBoxStyle.Question + MsgBoxStyle.OkCancel, "حذف سجل") = MsgBoxResult.Cancel Then Exit Sub

        ' حذف صنف من الفاتورة

        Dim SavInto As New SqlCommand
        Dim DataAdapter2 As New SqlDataAdapter(sqlstr2, con)
        SavInto.Connection = con
        SavInto.CommandType = CommandType.Text
        SavInto.CommandText = "DELETE  FROM buy WHERE Id like " & TextBox5.Text & ""

        con.Open()
        SavInto.ExecuteNonQuery()
        dataset2.Clear()

        con.Close()
        ''''
        Call search() 'أستدعاء كود البحث
        If CheckBox1.Checked = True Then
            Call dgv1billBuy() 'فاتورة الشراء
            Call totBuy() 'اجمالي فاتورة الشراء
        Else
            Call dgv1() ' تصم الداتا جريد فيو الخاصة بالفاتورة البيع
            Call tot() '  البيع حساب اجمالي الفاتورة

        End If
    End Sub


    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        On Error Resume Next ' كود عرض البحث عن رقم الفاتورة
        If Trim(txtBillNumber.Text) <> "" Then
            Call SearchBill() 'لعرض اسم العميل الخاص بالفاتورة 
            Call search() 'كود البحث برقم الفاتورة

        End If

        If CheckBox1.Checked = True Then
            Call dgv1billBuy() 'فاتورة الشراء
            Call totBuy() 'اجمالي فاتورة الشراء
        Else
            Call dgv1() ' تصم الداتا جريد فيو الخاصة بالفاتورة البيع
            Call tot() '  البيع حساب اجمالي الفاتورة

        End If


        butEnter.Enabled = True
    End Sub
    'Public Class rptform

    'Private Sub rptform_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    If con.State() Then con.Close()
    '    con.Open()
    '    Dim str As String = "SELECT        bill.billnu, bill.kind, bill.kind2, bill.date, buy.ItemNa, buy.Qout, buy.sail, buy.totalQout FROM bill INNER JOIN buy ON bill.billnu = buy.BILLnu WHERE bill.billnu ='" + Label1.Text + "' "
    '    Dim ds As DataSet1 = New DataSet1()
    '    Dim da As SqlDataAdapter = New SqlDataAdapter(str, con)

    '    da.Fill(ds.Tables("bill"))
    '    Dim ord As CrystalReport1 = New CrystalReport1()
    '    ord.SetDataSource(ds.Tables("bill"))
    '    CrystalReportViewer1.ReportSource = ord
    '    CrystalReportViewer1.Refresh()
    'End Sub
    Private Sub Button24_Click(sender As Object, e As EventArgs) Handles Button24.Click
        'rptform.Label1.Text = txtBillNumber.Text
        'Me.Close()
        'rptform.Show()
    End Sub

    Private Sub butAdd_Click(sender As Object, e As EventArgs) Handles butAdd.Click

        If MsgBox("   هل تريد تسجيل فاتورة جديدة   ", MsgBoxStyle.Information + MsgBoxStyle.OkCancel, "تسجيل فاتورة جديدة") = MsgBoxResult.Cancel Then Exit Sub



        Dim cmd2 As New SqlCommand  ' كود الترقيم التلقائي
        cmd2.CommandType = CommandType.Text
        cmd2.Connection = con
        cmd2.CommandText = "select max (billno) from [bill] " ' كود الترقيم التلقائي يعود لا علي قيم في الجدول
        con.Open()
        Dim a As Integer = cmd2.ExecuteScalar
        con.Close()
        txtBillNumber.Text = a + 1
        txtBlance.Text = "0"

        Call search() 'كود البحث عن رقم الفاتورة
        Call dgv1() 'أستدعاء تنسيق الداتا جريد
        Call tot() 'مجموع الفاتورة
        Call SaveBill() 'حغظ بيانات الفاتورة

    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        'كود لعرض ارقام الغواتير السابقة
        txtBillNumber.Text = txtBillNumber.Text - 1

        Call search()

        If CheckBox1.Checked = True Then
            Call dgv1billBuy() 'فاتورة الشراء
            Call totBuy() 'اجمالي فاتورة الشراء
        Else
            Call dgv1() ' تصم الداتا جريد فيو الخاصة بالفاتورة البيع
            Call tot() '  البيع حساب اجمالي الفاتورة

        End If
        Call SearchBill() 'عرض اسم العميل الخاص بالفاتورة
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

    End Sub
End Class



