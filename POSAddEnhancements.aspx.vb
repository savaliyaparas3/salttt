Imports System.Data
Imports System.Data.SqlClient


Partial Class POSAddEnhancements
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Session("emp_id") Is Nothing Then
        '    Session.Abandon()
        '    Response.Redirect("POSLogin.aspx")
        'End If


        If Not IsPostBack Then
            Page.Title = System.Configuration.ConfigurationManager.AppSettings("postitle") & " - " & "Enhancements"
            ViewState("sortcolumn") = "createdate"
            ViewState("sortstatus") = "DESC"
            Call getEnhancements()
        End If
    End Sub

    Public Sub getEnhancements()

        Dim con As String = System.Configuration.ConfigurationManager.ConnectionStrings("POSConnectionsetup").ConnectionString
        Dim cn As New SqlConnection
        cn.ConnectionString = con
        cn.Open()

        'create command for stored procedure
        Dim objCommand As SqlCommand = New SqlCommand()
        objCommand.Connection = cn
        objCommand.CommandText = "proc_admin_get_enhancement"
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@sort", ViewState("sortcolumn"))
        objCommand.Parameters.AddWithValue("@status", ViewState("sortstatus"))

        Dim ObjDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        ObjDataAdapter.SelectCommand = objCommand

        Dim objDataset As DataSet = New DataSet()

        'Try
        ObjDataAdapter.Fill(objDataset, "enhancements")

        'If objDataset.Tables(0).Rows.Count > 0 Then
        gvEnhancement.DataSource = objDataset.Tables(0)
        gvEnhancement.DataBind()
        'Else

        'End If
        'Catch ex As Exception
        'Response.Write(ex.ToString())
        'Finally
        'objConnection.DBDisconnect()
        cn.Close()
        'End Try
    End Sub


    Protected Sub gvEnhancement_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvEnhancement.PreRender

        gvEnhancement.Columns(0).Visible = False

    End Sub

    Protected Sub gvEnhancement_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvEnhancement.RowDataBound

        gvEnhancement.Columns(0).Visible = True
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lbtnEdit"), LinkButton).PostBackUrl = "POSEditEnhancements.aspx?eid=" & e.Row.Cells(0).Text
            e.Row.Cells(3).ToolTip = e.Row.Cells(3).Text
            If (e.Row.Cells(3).Text.Length > 63) Then
                e.Row.Cells(3).ToolTip = e.Row.Cells(3).Text
                e.Row.Cells(3).Text = Mid(e.Row.Cells(3).Text, 1, 63) & "..."
            End If
            e.Row.Cells(5).Attributes.Add("onClick", "javaScript:return delete_msg();")

            Dim idenhance As New TextBox
            idenhance.Text = CType(e.Row.FindControl("lblEnhancement"), Label).Text()
            'HttpUtility.HtmlDecode(idenhance.Value.Trim())
            'CType(e.Row.FindControl("litenhancement"), Literal).Text = CType(e.Row.FindControl("lblEnhancement"), Label).Text
            CType(e.Row.FindControl("litenhancement"), Literal).Text() = HttpUtility.HtmlDecode(idenhance.Text.ToString().Trim())

            'Text='<%# RemoveHTML(Trim(Bind("enhancement"))) %>'


        Else
            If e.Row.RowType = DataControlRowType.Header Then
                For Each cell As TableCell In e.Row.Cells
                    If cell.HasControls Then
                        Dim imgCreatedate As Image
                        Dim imgExpirationdate As Image
                        Dim imgEnhancement As Image


                        imgCreatedate = CType(e.Row.FindControl("imgCreatedate"), Image)
                        imgExpirationdate = CType(e.Row.FindControl("imgExpirationdate"), Image)
                        imgEnhancement = CType(e.Row.FindControl("imgEnhancement"), Image)

                        If ViewState("sortcolumn") = "createdate" Then
                            imgCreatedate.Visible = True
                            imgExpirationdate.Visible = False
                            imgEnhancement.Visible = False


                            If ViewState("sortstatus") = "ASC" Then
                                imgCreatedate.ImageUrl = "~/Images/arrowup.gif"
                            Else
                                imgCreatedate.ImageUrl = "~/Images/arrowdown.gif"
                            End If
                        End If
                        If ViewState("sortcolumn") = "expiryDate" Then
                            imgCreatedate.Visible = False
                            imgExpirationdate.Visible = True
                            imgEnhancement.Visible = False

                            If ViewState("sortstatus") = "ASC" Then
                                imgExpirationdate.ImageUrl = "~/Images/arrowup.gif"
                            Else
                                imgExpirationdate.ImageUrl = "~/Images/arrowdown.gif"
                            End If
                        End If
                        If ViewState("sortcolumn") = "enhancement" Then
                            imgCreatedate.Visible = False
                            imgExpirationdate.Visible = False
                            imgEnhancement.Visible = True

                            If ViewState("sortstatus") = "ASC" Then
                                imgEnhancement.ImageUrl = "~/Images/arrowup.gif"
                            Else
                                imgEnhancement.ImageUrl = "~/Images/arrowdown.gif"
                            End If
                        End If
                    End If
                Next
            End If
        End If
    End Sub

    Protected Sub gvEnhancement_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvEnhancement.Sorting
        If ViewState("sortcolumn") = e.SortExpression Then
            ViewState("sortcolumn") = e.SortExpression
            If ViewState("sortstatus") = "ASC" Then
                ViewState("sortstatus") = "DESC"
            Else
                ViewState("sortstatus") = "ASC"
            End If
        Else
            ViewState("sortcolumn") = e.SortExpression
            ViewState("sortstatus") = "ASC"

        End If
        Call getEnhancements()
    End Sub

    Protected Sub gvEnhancement_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEnhancement.RowCommand

    End Sub

    Protected Sub gvEnhancement_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvEnhancement.SelectedIndexChanged
        Dim Key As String = gvEnhancement.SelectedValue
        deleteEnhancement(Key)
        Call getEnhancements()
    End Sub


    Public Sub deleteEnhancement(ByVal id As String)

        Dim con As String = System.Configuration.ConfigurationManager.ConnectionStrings("POSConnectionsetup").ConnectionString
        Dim cn As New SqlConnection
        cn.ConnectionString = con
        cn.Open()


        'create command for stored procedure
        Dim objCommand As SqlCommand = New SqlCommand()

        objCommand.Connection = cn
        objCommand.CommandText = "proc_admin_delete_enhancement"
        objCommand.CommandType = CommandType.StoredProcedure
        objCommand.Parameters.AddWithValue("@enhanceid", id.Trim())
        'Try
        objCommand.ExecuteNonQuery()

        'Catch ex As Exception
        'Response.Write(ex.ToString())
        'Finally
        cn.Close()
        ' End Try
    End Sub

    Protected Sub gvEnhancement_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvEnhancement.PageIndexChanging
        gvEnhancement.PageIndex = e.NewPageIndex

        Call getEnhancements()
    End Sub

    Public Shared Function NoHTML(ByVal Htmlstring As String) As String
        '删除脚本
        Htmlstring = Regex.Replace(Htmlstring, "<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase)
        '删除HTML
        Htmlstring = Regex.Replace(Htmlstring, "<(.[^>]*)>", "", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "([\r\n])[\s]+", "", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "-->", "", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "<!--.*", "", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(quot|#34);", """", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(amp|#38);", "&", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(lt|#60);", "<", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(gt|#62);", ">", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(nbsp|#160);", " ", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(iexcl|#161);", "¡", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(cent|#162);", "¢", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(pound|#163);", "£", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&(copy|#169);", "©", RegexOptions.IgnoreCase)
        Htmlstring = Regex.Replace(Htmlstring, "&#(\d+);", "", RegexOptions.IgnoreCase)

        Htmlstring.Replace("<", "")
        Htmlstring.Replace(">", "")
        Htmlstring.Replace(vbCr & vbLf, "")
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim()

        Return Htmlstring
    End Function
End Class
