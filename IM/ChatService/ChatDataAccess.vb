Imports System.Data
Imports System.Configuration
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Linq
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Net.Mail

Namespace Data
    Public Class ChatDataAccess
        'MESSAGES
        Public Sub AddMessage(FromEmail As String, ToID As String, Message As String)
            If IsNumeric(ToID.Replace(",", "").Trim) Then
                Dim reader As SqlDataReader
                Dim command As New SqlCommand()
                command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
                command.CommandType = CommandType.StoredProcedure

                If ToID.Contains(",") Then
                    command.CommandText = "AddMessageForGroup"
                Else
                    command.CommandText = "AddMessage"
                End If

                command.Parameters.Add(New SqlParameter("@Message", Message))
                command.Parameters.Add(New SqlParameter("@FromEmail", FromEmail))
                command.Parameters.Add(New SqlParameter("@ToID", ToID))

                Try
                    command.Connection.Open()
                    reader = command.ExecuteReader()
                    While reader.Read()
                        If reader.Item("TimeDiff") > 15 Then
                            'create the mail message
                            Dim mail As New MailMessage()
                            'set the addresses
                            mail.From = New MailAddress("webmessenger@pwgsc.gc.ca")
                            mail.[To].Add(reader.Item("Email"))
                            'set the content
                            mail.Subject = "Western Chat - Unread Instant Message"
                            mail.IsBodyHtml = True
                            mail.Body = "A message was sent to you from " & reader.Item("UserName") & " via Western Chat (instant messenger) while you were off-line.<br /><br /><span style='color:#003366'>" & Replace(Replace(reader.Item("EmailMessage"), Chr(10), "<br />"), Chr(13), "<br />") & "</span><br /><br />To view and reply to this message <a href='thewire'>click here</a> to launch the WiRE and the Western Chat application."
                            mail.Body = HttpUtility.HtmlDecode(HttpUtility.UrlDecode(mail.Body))
                            'set the server
                            Dim smtp As New SmtpClient("10.2.7.169")
                            'send the message
                            Try
                                smtp.Send(mail)
                                'Response.Write("Your Email has been sent sucessfully - Thank You")
                            Catch exc As Exception
                                'Response.Write("Send failure: " & exc.ToString())
                            End Try
                        End If
                    End While
                Finally
                    command.Connection.Close()
                    command.Connection.Dispose()
                End Try
            End If
        End Sub

        Public Function CheckMessages(FromEmail As String, ToID As String, Typing As Boolean) As String
            Dim Messages As String = ""
            Dim IsTyping As String = "False"
            If IsNumeric(ToID.Replace(",", "").Replace(" ", "")) Then
                Dim reader As SqlDataReader
                Dim command As New SqlCommand()                
                Dim LastMessageDate As DateTime = "January 1, 2000"
                Dim ShowDate As DateTime = "January 1, 2000"
                Dim Style As String = ""

                command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
                command.CommandType = CommandType.StoredProcedure
                If ToID.Contains(",") Then
                    command.CommandText = "CheckMessagesForGroup"
                    command.Parameters.Add(New SqlParameter("@FirstUser", ToID.Substring(0, ToID.IndexOf(","))))
                Else
                    command.CommandText = "CheckMessages"
                End If

                command.Parameters.Add(New SqlParameter("@FromEmail", FromEmail))
                command.Parameters.Add(New SqlParameter("@ToID", ToID))
                command.Parameters.Add(New SqlParameter("@Typing", Typing))

                Try
                    command.Connection.Open()
                    reader = command.ExecuteReader()
                    While reader.Read()       

                        If FromEmail = reader("Email").ToString() Then
                            Style = "style='color:#551155;'"
                        Else
                            Style = "style='color:#226655;'"
                        End If

                        If reader.Item("ReturnTyping").ToString = "True" Or reader.Item("ReturnTyping").ToString = "1" Then
                            IsTyping = "True"
                        End If
                        If reader.Item("Email").ToString <> FromEmail Then
                            If IsDate(reader.Item("MessageDate").ToString) Then
                                If DateTime.Parse(reader.Item("MessageDate").ToString) > LastMessageDate Then
                                    LastMessageDate = DateTime.Parse(reader.Item("MessageDate").ToString)
                                End If
                            End If
                        End If
                        If reader("MessageBody") = "a9a3721aac40b9828285077f3cea00a0" And reader.Item("ToUserId").ToString <> ToID Then
                            If (ShowDate = DateTime.Parse(reader.Item("MessageDate").Date.ToString())) Then
                                Messages += "<li><div class='timestamp'>(" & DateTime.Parse(reader("MessageDate").ToString()).ToShortTimeString & ")</div><span " & Style & " class='displayname'>" & reader("UserName").ToString() & " </span><a href='#' onclick='RecieveVideo(" + reader.Item("ToUserId").ToString + ");'>would like to share their webcam click here</a></li>"
                            Else
                                Messages += "<li><div class='timestamp'><span> " & DateTime.Parse(reader.Item("MessageDate").Date.ToString()).ToLongDateString & " </span>(" & DateTime.Parse(reader("MessageDate").ToString()).ToShortTimeString & ")</div><span " & Style & " class='displayname'>" & reader("UserName").ToString() & " </span><a href='#' onclick='RecieveVideo(" + reader.Item("ToUserId").ToString + ");'>would like to share their webcam click here</a></li>"
                            End If
                        ElseIf reader("MessageBody") = "a9a3721aac40b9828285077f3cea00a1" And reader.Item("ToUserId").ToString <> ToID Then
                            If (ShowDate = DateTime.Parse(reader.Item("MessageDate").Date.ToString())) Then
                                Messages += "<li><div class='timestamp'>(" & DateTime.Parse(reader("MessageDate").ToString()).ToShortTimeString & ")</div><span " & Style & " class='displayname'>" & reader("UserName").ToString() & " </span><a href='#' onclick='RecieveDesktop(" + reader.Item("ToUserId").ToString + ");'>would like to share their desktop click here</a></li>"
                            Else
                                Messages += "<li><div class='timestamp'><span> " & DateTime.Parse(reader.Item("MessageDate").Date.ToString()).ToLongDateString & " </span>(" & DateTime.Parse(reader("MessageDate").ToString()).ToShortTimeString & ")</div><span " & Style & " class='displayname'>" & reader("UserName").ToString() & " </span><a href='#' onclick='RecieveDesktop(" + reader.Item("ToUserId").ToString + ");'>would like to share their desktop click here</a></li>"
                            End If
                        ElseIf reader("MessageBody") = "a9a3721aac40b9828285077f3cea00a0" Then
                        ElseIf reader("MessageBody") = "a9a3721aac40b9828285077f3cea00a1" Then
                        Else
                            If (ShowDate = DateTime.Parse(reader.Item("MessageDate").Date.ToString())) Then
                                Messages += "<li><div class='timestamp'>(" & DateTime.Parse(reader("MessageDate").ToString()).ToShortTimeString & ")</div><span " & Style & " class='displayname'>" & reader("UserName").ToString() & "</span>: " & reader("MessageBody").ToString() & "</li>"
                            Else
                                Messages += "<li><div class='timestamp'><span> " & DateTime.Parse(reader.Item("MessageDate").Date.ToString()).ToLongDateString & " </span>(" & DateTime.Parse(reader("MessageDate").ToString()).ToShortTimeString & ")</div><span " & Style & " class='displayname'>" & reader("UserName").ToString() & "</span>: " & reader("MessageBody").ToString() & "</li>"
                            End If
                        End If
                        ShowDate = DateTime.Parse(reader.Item("MessageDate").Date.ToString())

                    End While
                Finally
                    command.Connection.Close()
                    command.Connection.Dispose()
                End Try
                Dim Month, Day, Hour, Minute, Second As String
                Month = LastMessageDate.Month
                Day = LastMessageDate.Day
                Hour = LastMessageDate.Hour
                Minute = LastMessageDate.Minute
                Second = LastMessageDate.Second
                If LastMessageDate.Month < 10 Then Month = "0" & Month
                If LastMessageDate.Day < 10 Then Day = "0" & Day
                If LastMessageDate.Hour < 10 Then Hour = "0" & Hour
                If LastMessageDate.Minute < 10 Then Minute = "0" & Minute
                If LastMessageDate.Second < 10 Then Second = "0" & Second
                Messages = "({ ""lastmessage"": """ & LastMessageDate.Year & Month & Day & Hour & Minute & Second & """,""response"": """ & "<ul>" & Messages & "</ul>" & """,""returntyping"": """ & IsTyping & """ })"

            End If
            Return Messages
        End Function

        Public Function LoadSession(Email As String) As String
            Dim ReturnValue As String = ""
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "GetStartupInfo"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            Try
                command.Connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader
                While reader.Read
                    ReturnValue = "({ ""userstatus"": """ & reader.Item("Status") & """,""userstatusmessage"": """ & reader.Item("StatusMessage") & """ })"
                End While
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return ReturnValue
        End Function

        Public Function UpdateStatus(Email As String, Status As String) As String
            Dim ReturnValue As String = ""
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "UpdateStatus"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            command.Parameters.Add(New SqlParameter("@Status", Status))
            Try
                command.Connection.Open()
                command.ExecuteNonQuery()
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return ReturnValue
        End Function

        Public Function CheckRecentMessages(ToEmail As String) As String
            Dim NewMessages As String = ""
            Dim reader As SqlDataReader
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "CheckRecentMessages"

            command.Parameters.Add(New SqlParameter("@ToEmail", ToEmail))

            Try
                command.Connection.Open()
                reader = command.ExecuteReader()
                While reader.Read()
                    NewMessages += reader.Item("UserResponses").ToString & ";"
                End While
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return NewMessages
        End Function

        Public Function GetMessageHistory(Email As String) As String
            Dim ReturnValue As String = "<table class='HistoryTable'><tr><th>Time</th><th>From</th><th>To</th><th>Message</th></tr>"
            Dim reader As SqlDataReader
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "GetMessageHistory"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            Try
                command.Connection.Open()
                reader = command.ExecuteReader()
                While reader.Read()
                    Dim msg As String = reader("MessageBody").ToString()
                    If msg = "a9a3721aac40b9828285077f3cea00a0" Then
                        msg = "would like to show their webcam "
                    End If
                    ReturnValue += "<tr><td>" & reader("MessageDate").ToString() & "</td><td>" & reader("FromUser").ToString() & "</td><td>" & reader("ToUser").ToString() & "</td><td><div style='max-width:400px;'>" & msg & "</div></td></tr>"
                End While
                ReturnValue += "</table>"
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return ReturnValue
        End Function

        Public Function DeleteHistory(Email As String)
            Dim ReturnValue As String = ""
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "DeleteHistory"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            Try
                command.Connection.Open()
                command.ExecuteNonQuery()
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return ReturnValue
        End Function

        'USERS
        Public Sub AddUser(UserName As String, ImageLink As String, LastActive As DateTime, Email As String, Status As String, varGUID As String)
            Dim command As New SqlCommand()

            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "AddUser"
            command.Parameters.Add(New SqlParameter("@UserName", UserName))
            command.Parameters.Add(New SqlParameter("@ImageLink", ImageLink))
            command.Parameters.Add(New SqlParameter("@Email", Email))
            command.Parameters.Add(New SqlParameter("@Status", Status))
            command.Parameters.Add(New SqlParameter("@LastActive", LastActive))
            command.Parameters.Add(New SqlParameter("@GUID", varGUID))

            Try
                command.Connection.Open()
                command.ExecuteNonQuery()
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
        End Sub

        Public Sub UpdateUser(UserName As String, ImageLink As String, LastActive As DateTime, Email As String)
            Dim command As New SqlCommand()

            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "UpdateUser"
            command.Parameters.Add(New SqlParameter("@UserName", UserName))
            command.Parameters.Add(New SqlParameter("@ImageLink", ImageLink))
            command.Parameters.Add(New SqlParameter("@Email", Email))
            command.Parameters.Add(New SqlParameter("@LastActive", LastActive))
            Try
                command.Connection.Open()
                command.ExecuteNonQuery()
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
        End Sub

        Public Function CheckUsers(Email As String, Filter As String) As String
            Dim UserList As String = ""
            Dim reader As SqlDataReader
            Dim ds As New DataSet()
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "CheckUsers"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            If Filter = "User Search" Then
                command.Parameters.Add(New SqlParameter("@Filter", ""))
            Else
                command.Parameters.Add(New SqlParameter("@Filter", Filter))
            End If
            Try
                command.Connection.Open()
                reader = command.ExecuteReader()
                While reader.Read()
                    UserList += "<tr><td class='tdimage'><img src='" & reader("ImageLink").ToString() & "' class='userimage' /></td>"
                    UserList += "<td class='tdstatus'><img src='img/status_" & reader("Status") & ".png' /></td>"
                    UserList += "<td class='tduser'><a href='#' ondblclick='OpenChatWithFocus(" & reader("ID").ToString() & ");'><span class='userdisplayname'>" & reader("UserName").ToString() & "</span>"
                    UserList += "<div class='userstatus'>" & reader("StatusMessage").ToString() & "</div></a></td></tr>"
                End While
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return "<table id='UserList'>" & UserList & "</table>"
        End Function

        Public Function GetUser(Email As String, ID As Integer, varGUID As String) As Integer
            Dim UserId As Integer = -1
            Dim ds As New DataSet()
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "ChatGetUser"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            command.Parameters.Add(New SqlParameter("@ID", ID))
            command.Parameters.Add(New SqlParameter("@GUID", varGUID))
            Try
                command.Connection.Open()
                UserId = Integer.Parse(command.ExecuteScalar().ToString())
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return UserId
        End Function

        Public Sub ChangeStatusMessage(Message As String, Email As String)
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "ChangeStatusMessage"
            command.Parameters.Add(New SqlParameter("@Message", Message))
            command.Parameters.Add(New SqlParameter("@Email", Email))
            Try
                command.Connection.Open()
                command.ExecuteNonQuery()
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
        End Sub

        Public Function GetGroupChatContacts(Email As String, Filter As String) As String
            Dim ReturnValue As String = ""
            Dim reader As SqlDataReader
            Dim command As New SqlCommand()
            command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = "GetGroupChatContacts"
            command.Parameters.Add(New SqlParameter("@Email", Email))
            command.Parameters.Add(New SqlParameter("@Filter", Filter))
            Try
                command.Connection.Open()
                reader = command.ExecuteReader()
                While reader.Read()
                    ReturnValue += "<li><input type='button' onclick='AddToGroup(" & reader("ID").ToString() & ")' value='Select' id='" & reader("ID").ToString() & "' />&nbsp;<img src='" & reader("ImageLink").ToString() & "' class='userimage' /><span class='userdisplayname'>" & reader("UserName").ToString() & "</span></li>"
                End While
            Finally
                command.Connection.Close()
                command.Connection.Dispose()
            End Try
            Return "<ul>" & ReturnValue & "</ul>"
        End Function

        Public Function GetConversationUsers(FromEmail As String, ToID As String) As String
            Dim Users As String = ""
            If IsNumeric(ToID.Replace(",", "").Trim) Then
                Dim reader As SqlDataReader
                Dim command As New SqlCommand()
                command.Connection = New SqlConnection(ConfigurationManager.ConnectionStrings("ChatConnectionString").ConnectionString)
                command.CommandType = CommandType.StoredProcedure
                command.CommandText = "GetConversationUsers"

                command.Parameters.AddWithValue("@Email", FromEmail)
                command.Parameters.AddWithValue("@ToID", ToID)

                Try
                    command.Connection.Open()
                    reader = command.ExecuteReader()
                    While reader.Read()
                        Users += "<p><img src='" & reader("ImageLink").ToString() & "' /><br />" & reader("UserName").ToString() & "</p>"
                    End While
                Finally
                    command.Connection.Close()
                    command.Connection.Dispose()
                End Try
            End If
            Return Users
        End Function
    End Class
End Namespace

