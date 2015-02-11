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

public partial class DesktopModules_WET_Gallery_Gallery : DotNetNuke.Entities.Modules.PortalModuleBase {
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
                string description = "";
                string image = "";
                string listitem = "";
                Boolean addthis = false;

                if (((CurrentLocale.Substring(0, 2) == "en" || reader["TotalFrench"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "fr") && reader["Locale"].ToString() == "en")
                || ((CurrentLocale.Substring(0, 2) == "fr" || reader["TotalEnglish"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "en") && reader["Locale"].ToString() == "fr"))
                {
                    title = reader["LinkName"].ToString();
                    description = reader["LinkDescription"].ToString();
                    link = reader["LinkURL"].ToString();

                    //display the thumb file if it exists
                    string thumb = link.Substring(0, link.LastIndexOf("/") + 1) + "thumb_" + link.Substring(link.LastIndexOf("/") + 1);
                    if (System.IO.File.Exists(Server.MapPath(thumb)))
                    {
                        image = "<img src='" + thumb + "' alt='" + title + "' />";
                    }
                    else
                    {
                        image = "<img src='" + link + "' alt='" + title + "' />";
                    }                    

                    //display the picture only if the actual file exists
                    if (System.IO.File.Exists(Server.MapPath(link)))
                    {
                        addthis = true;
                    }
                }

                //Add the link if it exists                
                if (addthis)
                {
                    string editTag = "";

                    if (IsEditable)
                    {
                        editTag = "<div class='wet-gallery-edit wet-list-edit" + ModuleId + "' refid='textitem" + reader["ID"].ToString() + "'><span class='glyphicon glyphicon-pencil'></span></div>";
                    }

                    if (link != "")
                    {
                        listitem = "<a order='" + reader["ListOrder"].ToString() + "' href='" + link + "' title='" + description + "'>" + editTag + "<span id='textitem" + reader["ID"].ToString() + "'>" + image + "</span></a>";

                        if (title != "")
                        {
                            listitem = "<div class='label label-default'>" + title + "</div>" + listitem;
                        }
                    }

                    ltlLinks.Text += "<li id='listitem" + reader["ID"].ToString() + "'>" + listitem + "</li>";
                }
            }

        }
        catch (Exception ex)
        {
            AddMessage("Error", ex.Message, ex);
        }
        finally
        {
            ConnString.Close();
            ConnString.Dispose();
        }
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