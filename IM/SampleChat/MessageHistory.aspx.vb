
Partial Class MessageHistory
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        ltlEMail.Text = HttpUtility.UrlDecode(Request.Cookies("Email").Value.ToString.Trim)
    End Sub

End Class
