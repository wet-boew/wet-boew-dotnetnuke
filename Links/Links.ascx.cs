using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;


public partial class DesktopModules_WET_Links_Links : DotNetNuke.Entities.Modules.PortalModuleBase
{
    SqlConnection ConnString = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SiteSqlServer"].ToString());
    string CurrentLocale = System.Globalization.CultureInfo.CurrentUICulture.Name;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pnlAdmin.Visible = IsEditable;
            pnlEdit.ToolTip = Localization.GetString("pnlEdit.Text", this.LocalResourceFile);
            if (Page.IsPostBack == false)
            {
                LoadList();
                if (IsEditable) { AddLastListItem(); }
            }
        }
        catch (Exception ex)
        {
            AddMessage("Error", ex.Message, ex);
        }
    }

    protected void LoadList()
    {
		ConnString.Open();
		SqlCommand WETCommand = new SqlCommand();

        WETCommand.CommandText = "SELECT *, (SELECT COUNT(*) FROM WET_List WHERE ModuleID = @ModuleID AND Locale = 'en') AS TotalEnglish, (SELECT COUNT(*) FROM WET_List WHERE ModuleID = @ModuleID AND Locale = 'fr') AS TotalFrench FROM WET_List WHERE ModuleID = @ModuleID ORDER BY ListOrder";
        WETCommand.Parameters.AddWithValue("@ModuleID", ModuleId);
		WETCommand.Connection = ConnString;

        try
        {
            SqlDataReader reader = WETCommand.ExecuteReader();
            Boolean thisLanguage = true;

            while (reader.Read())
            {
                //This adds to the page when no links are added for the current language
                if (IsEditable)
                {
                    if (((CurrentLocale.Substring(0, 2) == "en" && reader["TotalEnglish"].ToString() == "0") || (CurrentLocale.Substring(0, 2) == "fr" && reader["TotalFrench"].ToString() == "0")) && thisLanguage)
                    {
                        AddMessage(Localization.GetString("msgLang.Text", LocalResourceFile), "Information");
                        thisLanguage = false;
                    }
                }

                //Generate the list item
                string title = "";
                string link = "";
                Boolean addthis = false;

                if (((CurrentLocale.Substring(0, 2) == "en" || reader["TotalFrench"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "fr") && reader["Locale"].ToString() == "en")
                || ((CurrentLocale.Substring(0, 2) == "fr" || reader["TotalEnglish"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "en") && reader["Locale"].ToString() == "fr"))
                {
                    title = reader["LinkName"].ToString();
                    link = reader["LinkURL"].ToString();
                    addthis = true;
                }

                //Add the link if it exists
                if (addthis)
                {
                    string editTag = "";
                    if (IsEditable) {
                        editTag = "<span class='wet-list-edit" + ModuleId + "' refid='textitem" + reader["ID"].ToString() + "'><span class='glyphicon glyphicon-pencil mrgn-rght-sm'></span></span>";
                    }
                    if (link != "")
                    {
                        title = "<a order='" + reader["ListOrder"].ToString() + "' href='" + link + "'>" + editTag + "<span id='textitem" + reader["ID"].ToString() + "'>" + title + "</span></a>";
                    }
                    else
                    {
                        title = "<a order='" + reader["ListOrder"].ToString() + "'>" + editTag + "<span id='textitem" + reader["ID"].ToString() + "'>" + title + "</span></a>";
                    }

                    ltlLinks.Text += "<li id='listitem" + reader["ID"].ToString() + "'>" + title + "</li>";
                }
            }	
	
		} catch (Exception ex) {
            AddMessage("Error",ex.Message, ex);
		} finally {
			ConnString.Close();
			ConnString.Dispose();
		}
    }

    protected void AddLastListItem()
    {
        ltlLinks.Text += "<li class='active wet-list-" + ModuleConfiguration.ModuleID + "'><a href='#' " +
            "onclick=\"AddWetDocument(" + ModuleId + ", '" + CurrentLocale.Substring(0, 2) + "', '" + Localization.GetString("msgEnterText.Text", LocalResourceFile) + "','',getLastOrder('" + ModuleId + "'),'" + PortalAlias.HTTPAlias + "','" + Localization.GetString("msgEnterText.Text", "/DesktopModules/WET/Links/App_LocalResources/Links") + "');\">" + 
            Localization.GetString("msgNew.Text", LocalResourceFile) + "</a></li>";
    }

    protected void AddMessage(string Message, string Type, Exception ex = null)
    {
        if (Type == "Error")
        {
            Exceptions.LogException(ex);
            if (UserInfo.IsSuperUser)
            {
                Skin.AddModuleMessage(this, Message, ModuleMessage.ModuleMessageType.RedError);
            }
            else
            {                
                Skin.AddModuleMessage(this, Localization.GetString("msgError.Text", this.LocalResourceFile), ModuleMessage.ModuleMessageType.RedError);
            }
        }
        else if (Type == "Success")
        {
            Skin.AddModuleMessage(this, Message, ModuleMessage.ModuleMessageType.GreenSuccess);
        }
        else if (Type == "Warning")
        {
            Skin.AddModuleMessage(this, Message, ModuleMessage.ModuleMessageType.YellowWarning);
        }
        else if (Type == "Information")
        {
            Skin.AddModuleMessage(this, Message, ModuleMessage.ModuleMessageType.BlueInfo);
        }
    }
}