
Partial Class WebCam
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString("touser") <> Nothing Then
            ltlTo.Text = Request.QueryString("touser").ToString
        End If
        ltlFrom.Text = HttpUtility.UrlDecode(Request.Cookies("Email").Value.ToString.Trim)
    End Sub
End Class
