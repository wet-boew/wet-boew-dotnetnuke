using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

public partial class DesktopModules_WET_MLHTML_Edit : DotNetNuke.Entities.Modules.PortalModuleBase
{
    string ConnString = System.Configuration.ConfigurationManager.AppSettings["SiteSqlServer"].ToString();

    protected void Page_Load(object sender, System.EventArgs e)
    {
        //Populate the HTML
        try
        {
            litPreview.Text = Localization.GetString("NoPreview.Text", LocalResourceFile);
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand();
                //Grab a different version if a different language
                LocaleController LangController = new LocaleController();
                string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;

                //Get the last HTML Text             
                command.CommandText = "SELECT [ItemID],[Content],[Summary] FROM [HtmlText] WHERE ModuleID = @ModuleID ORDER BY LastModifiedOnDate ";
                command.Parameters.AddWithValue("@ModuleID", ModuleId);
                command.Connection = connection;
                try
                {
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    bool LocaleNotFound = true;
                    if (reader.HasRows)
                    {
                        //Populate the field
                        while (reader.Read())
                        {
                            if (reader["Summary"].ToString().Trim() == ThisLocale.Trim())
                            {
                                LocaleNotFound = false;
                                txtAdminView.Text = HttpUtility.HtmlDecode(reader["Content"].ToString());
                            }
                        }
                        if (LocaleNotFound)
                        {
                           FirstEntry.Text = "True";
                        }
                    }
                    else
                    {
                        FirstEntry.Text = "True";
                    }
                    if (!Page.IsPostBack)
                    {
                        DisplayVersions();
                    }
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            if (PortalSettings.UserInfo.IsSuperUser)
                txtAdminView.Text = ex.Message;
        }
    }

    protected void BtnSaveContent_Click(object sender, System.EventArgs e)
    {
        try
        {
            if (FirstEntry.Text == "True")
            {
                //Populate the HTML if first entry
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnString))
                    {
                        SqlCommand command = new SqlCommand();
                        //Grab a different version if a different language
                        LocaleController LangController = new LocaleController();
                        string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;                          
                        command.CommandText = "insert into dbo.HtmlText ( ModuleID, Content, Summary, Version, StateID, IsPublished, CreatedByUserID, CreatedOnDate, LastModifiedByUserID, LastModifiedOnDate ) values ( " + ModuleId + ", '" + txtAdminView.Text + "', '" + ThisLocale + "', 1, 1, 1, " + UserId + ", getdate(), " + UserId + ", getdate() )";
                        command.Connection = connection;
                        try
                        {
                            command.Connection.Open();
                            command.ExecuteNonQuery();
                            txtAdminView.Text = "";
                        }
                        finally
                        {
                            command.Connection.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (PortalSettings.UserInfo.IsSuperUser)
                        txtAdminView.Text = ex.Message;
                }
            }
            else
            {
                int VersionNumber = 0;
                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    SqlCommand command = new SqlCommand();
                    //Grab a different version if a different language
                    LocaleController LangController = new LocaleController();
                    string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;
                    //Get the last Version         
                    command.CommandText = "select max(Version) from dbo.HtmlText where ModuleID = " + ModuleId + " and CONVERT(NVARCHAR(MAX), Summary) = N'" + ThisLocale + "'";
                    command.Connection = connection;        
                    try
                    {
                        command.Connection.Open();
                        VersionNumber = Convert.ToInt32(command.ExecuteScalar());
                        if (VersionNumber >= 1)
                        {
                            VersionNumber = VersionNumber + 1;
                        }
                        else
                        {
                            VersionNumber = 1;
                        }
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }

                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    SqlCommand command = new SqlCommand();
                    //Grab a different version if a different language
                    LocaleController LangController = new LocaleController();
                    string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;
                    //Get the last HTML Text                             
                    command.CommandText = "insert into dbo.HtmlText ( ModuleID, Content, Summary, Version, StateID, IsPublished, CreatedByUserID, CreatedOnDate, LastModifiedByUserID, LastModifiedOnDate ) values ( " + ModuleId + ", '" + txtAdminView.Text + "', '" + ThisLocale + "', " + VersionNumber + ", 1, 1, " + UserId + ", getdate(), " + UserId + ", getdate() )";
                    command.Connection = connection;
                    try
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                        txtAdminView.Text = "";
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }

                using (SqlConnection connection = new SqlConnection(ConnString))
                {
                    string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;
                    SqlCommand command = new SqlCommand();
                    command.CommandText = "delete HtmlText from HtmlText T1 inner join ( select ROW_NUMBER() OVER (order by [Version] desc) RowNum, [Version] from HtmlText where ModuleID = " + ModuleId + " AND CONVERT(NVARCHAR(MAX), Summary) = N'" + ThisLocale + "') T2 on T1.[Version] = T2.[Version] where T2.RowNum > 5 AND ModuleID =" + ModuleId;
                    command.Connection = connection;
                    try
                    {
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                    finally
                    {
                        command.Connection.Close();
                    }
                }
            }
            Response.Redirect(PortalSettings.ActiveTab.FullUrl, true);
        }
        catch (Exception ex)
        {
            if (PortalSettings.UserInfo.IsSuperUser)
                txtAdminView.Text = ex.Message;
            Exceptions.LogException(ex);
        }
    }

    protected void DisplayVersions()
    {
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand();
                //Grab a different version if a different language
                LocaleController LangController = new LocaleController();
                string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;
                connection.Open();
                command.CommandText = "SELECT H.[ItemID],H.[Version],H.[LastModifiedOnDate],H.[LastModifiedByUserID],U.[DisplayName] FROM [HtmlText] H inner join [Users] U on H.LastModifiedByUserID = U.UserID WHERE ModuleID = " + ModuleId + " and CONVERT(NVARCHAR(MAX), Summary) = N'" + ThisLocale + "' ORDER BY Version desc";
                command.Connection = connection;
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sda.Fill(ds);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dgVersions.DataSource = ds;
                        dgVersions.DataBind();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        dgVersions.Visible = false;
                        NoVersions.Visible = true;
                    }
                }
                else
                {
                    dgVersions.Visible = false;
                    NoVersions.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            if (PortalSettings.UserInfo.IsSuperUser)
                txtAdminView.Text = ex.Message;
        }
    }

    protected void VersionsGridItemCommand(Object sender, DataGridCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName.ToLower())
            {
                case "remove":
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(ConnString))
                        {
                            SqlCommand command = new SqlCommand();
                            command.CommandText = "delete from dbo.HtmlText where ModuleID = " + ModuleId + " and ItemID = " + Convert.ToInt32(dgVersions.DataKeys[e.Item.ItemIndex]);
                            command.Connection = connection;
                            try
                            {
                                command.Connection.Open();
                                command.ExecuteNonQuery();
                            }
                            finally
                            {
                                command.Connection.Close();
                                DisplayVersions();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (PortalSettings.UserInfo.IsSuperUser)
                            txtAdminView.Text = ex.Message;
                    }
                    break;
                case "rollback":
                    using (SqlConnection connection = new SqlConnection(ConnString))
                    {
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "SELECT [Content] FROM [HtmlText] WHERE ItemID = " + Convert.ToInt32(dgVersions.DataKeys[e.Item.ItemIndex]);
                        command.Connection = connection;
                        try
                        {
                            command.Connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                txtAdminView.Text = HttpUtility.HtmlDecode(reader["Content"].ToString());
                            }
                        }
                        finally
                        {
                            command.Connection.Close();
                        }
                    }
                    break;
                case "preview":
                    using (SqlConnection connection = new SqlConnection(ConnString))
                    {
                        SqlCommand command = new SqlCommand();
                        command.CommandText = "SELECT [Content] FROM [HtmlText] WHERE ItemID = " + Convert.ToInt32(dgVersions.DataKeys[e.Item.ItemIndex]);
                        command.Connection = connection;
                        try
                        {
                            command.Connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                litPreview.Text = HttpUtility.HtmlDecode(reader["Content"].ToString());
                            }
                        }
                        finally
                        {
                            command.Connection.Close();
                        }
                    }
                    break;
            }
        }
        catch (Exception exc)
        {
            Exceptions.ProcessModuleLoadException(this, exc);
        }
    }

    protected void VersionsGridItemDataBound(Object sender, DataGridItemEventArgs e)
    {
        var item = e.Item as DataGridItem;
        foreach (TableCell cell in item.Cells)
        {
            foreach (Control cellControl in cell.Controls)
            {
                if (cellControl is ImageButton)
                {
                    var imageButton = cellControl as ImageButton;
                    switch (imageButton.CommandName.ToLower())
                    {
                        case "rollback":
                            //hide rollback for the first item
                            if (dgVersions.CurrentPageIndex == 0)
                            {
                                if ((item.ItemIndex == 0))
                                {
                                    imageButton.Visible = false;
                                    break;
                                }
                            }
                            imageButton.Visible = true;
                            break;

                        case "remove":
                            var msg = Localization.GetString("DeleteVersion.Confirm", LocalResourceFile);
                            imageButton.OnClientClick = "return confirm(\"" + msg + "\");";
                            break;
                    }
                }

            }
        }
    }
}