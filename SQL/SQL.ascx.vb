
Partial Class DesktopModules_WET_SQLResults_SQL
    Inherits DotNetNuke.Entities.Modules.PortalModuleBase

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Try
            btnAdmin.Visible = UserInfo.IsSuperUser
            If UserInfo.IsSuperUser Then btnAdmin.Visible = IsEditable
            btnSave.Visible = UserInfo.IsSuperUser
            pnlAdmin.Visible = UserInfo.IsSuperUser

            If Page.IsPostBack = False Then
                pnlAdmin.ToolTip = Localization.GetString("titleAdmin.Text", Me.LocalResourceFile)
                txtTemplate.Text = Localization.GetString("txtTemplate.Text", Me.LocalResourceFile)
                txtRowTemplate.Text = Localization.GetString("txtRowTemplate.Text", Me.LocalResourceFile)
                txtItemTemplate.Text = Localization.GetString("txtItemTemplate.Text", Me.LocalResourceFile)
                ltlSQL.Text = Localization.GetString("msgDefault.Text", Me.LocalResourceFile)

                'Get the SQL Query
                LoadSettings()
            End If

            LoadSQL()
        Catch ex As Exception
            DotNetNuke.Services.Exceptions.LogException(ex)
            If UserInfo.IsSuperUser Then
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, ex.Message, Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            Else
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("msgError.Text", Me.LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
            End If
        End Try
    End Sub

    Protected Sub LoadSettings()
        Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
        objModules.GetModuleSettings(ModuleId)
        Dim hash As Hashtable = objModules.GetModuleSettings(ModuleId)

        If String.IsNullOrEmpty(hash("SQLTemplate")) = False Then
            txtTemplate.Text = hash("SQLTemplate").ToString
        End If

        If String.IsNullOrEmpty(hash("SQLItemTemplate")) = False Then
            txtItemTemplate.Text = hash("SQLItemTemplate").ToString
        End If

        If String.IsNullOrEmpty(hash("SQLRowTemplate")) = False Then
            txtRowTemplate.Text = hash("SQLRowTemplate").ToString
        End If

        If String.IsNullOrEmpty(hash("SQLQuery")) = False Then
            txtSqlQuery.Text = hash("SQLQuery").ToString
        End If
    End Sub

    Protected Sub LoadSQL()
        Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
        objModules.GetModuleSettings(ModuleId)
        Dim hash As Hashtable = objModules.GetModuleSettings(ModuleId)

        If String.IsNullOrEmpty(hash("SQLQuery")) = False Then
            Dim ConnString As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("SiteSqlServer").ToString)
            ConnString.Open()

            Dim WETCommand As New SqlCommand
            WETCommand.CommandText = hash("SQLQuery").ToString
            WETCommand.Connection = ConnString
            Try
                Dim reader As SqlDataReader
                reader = WETCommand.ExecuteReader

                Dim TotalQuery As String = ""

                If reader.HasRows Then
                    While reader.Read

                        Dim ItemList As String = ""

                        Dim ItemCount As Integer = 0
                        While ItemCount < reader.FieldCount
                            ItemList = ItemList & hash("SQLItemTemplate").ToString.Replace("[QUERY]", reader.Item(ItemCount).ToString)
                            ItemCount = ItemCount + 1
                        End While

                        TotalQuery = TotalQuery & hash("SQLRowTemplate").ToString.Replace("[QUERY]", ItemList)

                    End While

                    TotalQuery = hash("SQLTemplate").ToString.Replace("[QUERY]", TotalQuery)
                Else
                    TotalQuery = Localization.GetString("msgNoResults.Text", Me.LocalResourceFile)
                End If

                ltlSQL.Text = HttpUtility.HtmlDecode(TotalQuery)
            Catch ex As Exception
                DotNetNuke.Services.Exceptions.LogException(ex)
                If UserInfo.IsSuperUser Then
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, ex.Message, Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                Else
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(Me, Localization.GetString("msgError.Text", Me.LocalResourceFile), Skins.Controls.ModuleMessage.ModuleMessageType.RedError)
                End If
            Finally
                ConnString.Close()
            End Try
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
        objModules.UpdateModuleSetting(ModuleId, "SQLQuery", txtSqlQuery.Text)
        objModules.UpdateModuleSetting(ModuleId, "SQLTemplate", txtTemplate.Text)
        objModules.UpdateModuleSetting(ModuleId, "SQLItemTemplate", txtItemTemplate.Text)
        objModules.UpdateModuleSetting(ModuleId, "SQLRowTemplate", txtRowTemplate.Text)
        Response.Redirect(PortalSettings.ActiveTab.FullUrl.ToLower.Replace(System.Globalization.CultureInfo.CurrentUICulture.Name.ToLower + "/", ""))
    End Sub
End Class
