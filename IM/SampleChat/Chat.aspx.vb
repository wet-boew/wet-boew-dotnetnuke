
Partial Class Chat
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        LoadFromAndTo()
    End Sub

    Protected Sub LoadFromAndTo()
        If Request.QueryString("ID") <> Nothing Then
            ltlTo.Text = Request.QueryString("ID").ToString
            If Request.QueryString("ID").Contains(",") Then
                camerabtn.Visible = False
                'desktopbtn.Visible = False
            End If
        End If
        ltlFrom.Text = HttpUtility.UrlDecode(Request.Cookies("Email").Value.ToString.Trim)
        ltlUserName.Text = HttpUtility.UrlDecode(Request.Cookies("UserName").Value.ToString.Trim)
        MessageBox.Focus()
    End Sub
End Class
