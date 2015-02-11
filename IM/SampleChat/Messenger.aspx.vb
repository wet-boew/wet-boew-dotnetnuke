Imports System.Data.SqlClient
Imports System.Data

Partial Class Messenger
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            If Request.QueryString("Email") <> Nothing Then
                LoadURL()
            Else
                LoadSession()
            End If
        Catch ex As Exception
            Response.Write("<h4>Either your session has expired or there was a problem loading your messenger.  Please visit <a href='http://thewire.wst.pwgsc.gc.ca/Default.aspx?LoadMessenger=True'>http://thewire.wst.pwgsc.gc.ca/</a> again and try to load it again from there.</h4>")
        End Try
    End Sub

    Protected Sub LoadURL()

        '-----------------this is so we can link to our profile on the wire from profile pic.  Not used by other regions.
        If Request.QueryString("UserID") <> Nothing Then
            Dim UserIDCookie As HttpCookie = New HttpCookie("UserID")
            UserIDCookie.Value = Request.QueryString("UserID").ToString
            UserIDCookie.Expires = Today.AddDays(30)
            Response.Cookies.Add(UserIDCookie)
        End If
        '--------------------------------------------------------------------------------------------------

        If Request.QueryString("Email") <> Nothing Then
            Dim EmailCookie As HttpCookie = New HttpCookie("Email")
            EmailCookie.Value = HttpUtility.UrlEncode(HttpUtility.HtmlDecode(Request.QueryString("Email").ToString))
            EmailCookie.Expires = Today.AddDays(30)
            Response.Cookies.Add(EmailCookie)
        End If

        If Request.QueryString("UserName") <> Nothing Then
            Dim UserNameCookie As HttpCookie = New HttpCookie("UserName")
            UserNameCookie.Value = HttpUtility.UrlEncode(HttpUtility.HtmlDecode(Request.QueryString("UserName").ToString))
            UserNameCookie.Expires = Today.AddDays(30)
            Response.Cookies.Add(UserNameCookie)
        End If

        If Request.QueryString("ImageLink") <> Nothing Then
            Dim ImageLinkCookie As HttpCookie = New HttpCookie("ImageLink")
            ImageLinkCookie.Value = HttpUtility.UrlEncode(HttpUtility.HtmlDecode(Request.QueryString("ImageLink").ToString))
            ImageLinkCookie.Expires = Today.AddDays(30)
            Response.Cookies.Add(ImageLinkCookie)
        End If

        If Request.QueryString("GUID") <> Nothing Then
            Dim GUIDCookie As HttpCookie = New HttpCookie("GUID")
            GUIDCookie.Value = HttpUtility.UrlEncode(Request.QueryString("GUID").ToString)
            GUIDCookie.Expires = Today.AddDays(30)
            Response.Cookies.Add(GUIDCookie)
        End If

        Response.Redirect("/Messenger.aspx")
    End Sub

    Protected Sub LoadSession()
        profileUser.href = "http://thewire.wst.pwgsc.gc.ca/UserProfile/tabid/1133/userId/" & Request.Cookies("UserID").Value.ToString.Trim & "/Default.aspx"
        ltlEmail.Text = "<div id='varEmail'>" & HttpUtility.UrlDecode(Request.Cookies("Email").Value.ToString.Trim) & "</div>"
        ltlUserName.Text = "<div id='varUserName'>" & HttpUtility.UrlDecode(Request.Cookies("UserName").Value.ToString.Trim) & "</div>"
        ltlPicture.Text = "<div id='varImageLink'>" & HttpUtility.UrlDecode(Request.Cookies("ImageLink").Value.ToString.Trim) & "</div>"
        ltlGUID.Text = "<div id='varGUID'>" & HttpUtility.UrlDecode(Request.Cookies("GUID").Value.ToString.Trim) & "</div>"
        imgUser.Src = HttpUtility.UrlDecode(Request.Cookies("ImageLink").Value.ToString.Trim)
    End Sub

End Class
