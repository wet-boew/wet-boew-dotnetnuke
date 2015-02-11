using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

public partial class DesktopModules_WET_MLHTML_MLHTML : DotNetNuke.Entities.Modules.PortalModuleBase, IActionable
{
    string ConnString = System.Configuration.ConfigurationManager.AppSettings["SiteSqlServer"].ToString();

    protected void Page_Load(object sender, System.EventArgs e)
    {
        //Populate the HTML
        try
        {
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
                    string defaultLocale = "";
                    bool otherLocal = true;
                    lblUserView.Text = Localization.GetString("txtDefault.Text", LocalResourceFile);

                    //User View
                    while (reader.Read())
                    {
                        if (reader["Summary"].ToString().Trim() == ThisLocale.Trim())
                        {
                            lblUserView.Text = HttpUtility.HtmlDecode(reader["Content"].ToString());
                        }

                        //Store the default locale incase no locale set for the current one
                        if (LangController.GetDefaultLocale(PortalId).Code.Trim() == reader["Summary"].ToString().Trim())
                        {
                            defaultLocale = reader["Content"].ToString();
                        }

                        if (otherLocal & IsEditable)
                        {
                            otherLocal = false;
                            CheckOtherLanguages();                         
                        }
                    }

                    //Load the default locale if it has data but the other doesn't
                    if ((string.IsNullOrEmpty(lblUserView.Text) | lblUserView.Text == Localization.GetString("txtDefault.Text", LocalResourceFile)) & string.IsNullOrEmpty(defaultLocale) == false)
                    {
                        lblUserView.Text = HttpUtility.HtmlDecode(defaultLocale);
                    }

                    if (ModuleConfiguration.PaneName == "RightPane")
                    {
                        lblUserView.Text = "<p>" + lblUserView.Text + "</p>";
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
                lblUserView.Text = ex.Message;
        }
    }

    protected void CheckOtherLanguages()
    {
        //Check to see if other languages exist and if they are populated or not
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand();
                string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;          
                command.CommandText = "SELECT CultureCode, CultureName FROM Languages WHERE CultureCode <> 'en-US' and CultureCode <> '" + ThisLocale + "'";
                command.Connection = connection;
                try
                {
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        DisplayLanguageMessage(reader["CultureCode"].ToString(), reader["CultureName"].ToString());
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
                lblUserView.Text = ex.Message;
        }
    }

    protected void DisplayLanguageMessage(string CultureCode, string CultureName)
    {
        //Check to see if other languages exist and if they are populated or not
        try
        {
            using (SqlConnection connection = new SqlConnection(ConnString))
            {
                SqlCommand command = new SqlCommand();               
                string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;
                command.CommandText = "SELECT DISTINCT [ModuleID] FROM [HtmlText] WHERE ModuleID = " + ModuleId + " AND CONVERT(NVARCHAR(MAX), Summary) = N'" + CultureCode + "'";
                command.Connection = connection;
                try
                {
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        AlertSection.Visible = true;
                        lblOtherLanguages.Text = lblOtherLanguages.Text + "You have not entered content for <b>''" + CultureName + "''</b> yet.<br>";
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
                lblUserView.Text = ex.Message;
        }
    }

    public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
    {
        get
        {
            DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
            if (IsEditable)
            {
                Actions.Add(GetNextActionID(), Localization.GetString("Edit.Text", LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.EditContent, "", "", EditUrl("Edit"), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
            }
            return Actions;
        }
    }
}