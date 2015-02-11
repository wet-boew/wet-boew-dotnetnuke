using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;

public partial class DesktopModules_WET_Slideshow_Slideshow : DotNetNuke.Entities.Modules.PortalModuleBase
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
            int count = 0;
            string fadeType = "in fade";            
            while (reader.Read())
            {
                //Loads the list for Admin to see
                if (IsEditable)
                {
                    //This adds to the page when no links are added for the current language
                    if (((CurrentLocale.Substring(0, 2) == "en" && reader["TotalEnglish"].ToString() == "0") || (CurrentLocale.Substring(0, 2) == "fr" && reader["TotalFrench"].ToString() == "0")) && thisLanguage)
                    {
                        AddMessage(Localization.GetString("msgLang.Text", LocalResourceFile), "Information");
                        thisLanguage = false;
                    }
                    //Generate the list item
                    count = count + 1;
                    string title = "";
                    string link = "";
                    string description = "";
                    string image = "";
                    string listitem = "";
                    string listpanel = "";
                    Boolean addthis = false;

                    if (((CurrentLocale.Substring(0, 2) == "en" || reader["TotalFrench"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "fr") && reader["Locale"].ToString() == "en")
                    || ((CurrentLocale.Substring(0, 2) == "fr" || reader["TotalEnglish"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "en") && reader["Locale"].ToString() == "fr"))
                    {
                        title = reader["LinkName"].ToString();
                        description = reader["LinkDescription"].ToString().Replace("\"", "");
                        link = reader["LinkURL"].ToString();

                        image = "<img src='" + link + "' alt='" + title + "' desc=\"" + description + "\" order='" + reader["ListOrder"].ToString() + "' />";
                        if (reader["ListOrder"].ToString() == "0")
                        {
                            image = image + "<span style='position: absolute; top: 15px; left: 50px;' class='glyphicon glyphicon-eye-close text-warning' title='" + Localization.GetString("ADMINLimited.Text", LocalResourceFile) + "'></span>";
                        }
                        //display the picture only if the actual file exists
                        if (System.IO.File.Exists(Server.MapPath(link)))
                        {
                            addthis = true;
                        }
                    }

                    //Linkify the description
                    title = title.Replace("\n", " ");
                    if (!title.Contains("http://www"))
                    {
                    title = title.Replace("www.", " http://www.");
                    }
                    //title = Regex.Replace(title, @"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'.,<>?«»“”‘’]))", "<a target='_blank' href=\"$1\">$1</a>");

                    //Add the link if it exists                
                    if (addthis)
                    {
                        string editTag = "";

                        if (IsEditable)
                        {
                            editTag = "<div class='wet-list-edit" + ModuleId + "' refid='textitem" + reader["ID"].ToString() + "'><span class='glyphicon glyphicon-pencil'></span></div>";
                        }

                        if (title != "")
                        {
                            listpanel = "<div role=\"tabpanel\" id=\"panel" + count + "\" class=\"" + fadeType + "\">" +
                                            "<figure>" +
                                               editTag + "<span id='textitem" + reader["ID"].ToString() + "'>" + image + "</span>" +
                                                "<figcaption>" +
                                                    "<p><a target='blank' href='" + title + "'>" + description + "</a></p>" +
                                                "</figcaption>" +
                                            "</figure>" +
                                        "</div>";
                            listitem = "<a href=\"#panel" + count + "\">" + image + "</a>";

                            if (title != "")
                            {
                                listitem = "<div class='label label-default'>" + title + "</div>" + listitem;
                            }
                        }
                        else
                        {

                            listpanel = "<div role=\"tabpanel\" id=\"panel" + count + "\" class=\"" + fadeType + "\">" +
                                            "<figure>" +
                                               editTag + "<span id='textitem" + reader["ID"].ToString() + "'>" + image + "</span>" +
                                                "<figcaption>" +
                                                    "<p>" + description + "</p>" +
                                                "</figcaption>" +
                                            "</figure>" +
                                        "</div>";
                            listitem = "<a href=\"#panel" + count + "\">" + image + "</a>";

                            if (title != "")
                            {
                                listitem = "<div class='label label-default'>" + title + "</div>" + listitem;
                            }
                        }

                        if (ltlTabList.Text == "")
                        {
                            ltlTabList.Text += "<li class='active'>" + listitem + "</li>";
                        }
                        else
                        {
                            ltlTabList.Text += "<li>" + listitem + "</li>";
                        }
                        ltlTabPanels.Text += listpanel;

                        if (fadeType == "in fade") { fadeType = "out fade"; }
                    }
                }
                //Loads the list for users to see
                else
                {
                    if (reader["ListOrder"].ToString() != "0" )
                    {
                    //Generate the list item
                    count = count + 1;
                    string title = "";
                    string link = "";
                    string description = "";
                    string image = "";
                    string listitem = "";
                    string listpanel = "";
                    Boolean addthis = false;

                    if (((CurrentLocale.Substring(0, 2) == "en" || reader["TotalFrench"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "fr") && reader["Locale"].ToString() == "en")
                    || ((CurrentLocale.Substring(0, 2) == "fr" || reader["TotalEnglish"].ToString() == "0" && CurrentLocale.Substring(0, 2) == "en") && reader["Locale"].ToString() == "fr"))
                    {
                        title = reader["LinkName"].ToString();
                        description = reader["LinkDescription"].ToString().Replace("\"", "");
                        link = reader["LinkURL"].ToString();

                        image = "<img src='" + link + "' alt='" + title + "' desc=\"" + description + "\" order='" + reader["ListOrder"].ToString() + "' />";

                        //display the picture only if the actual file exists
                        if (System.IO.File.Exists(Server.MapPath(link)))
                        {
                            addthis = true;
                        }
                    }

                    //Linkify the link
                    title = reader["LinkName"].ToString().Replace("\"", "");
                    title = title.Replace("\n", " ");
                    if (!title.Contains("http://www"))
                    {
                        title = title.Replace("www.", " http://www.");
                    }                   
                    //title = Regex.Replace(title, @"(?i)\b((?:[a-z][\w-]+:(?:/{1,3}|[a-z0-9%])|www\d{0,3}[.]|[a-z0-9.\-]+[.][a-z]{2,4}/)(?:[^\s()<>]+|\(([^\s()<>]+|(\([^\s()<>]+\)))*\))+(?:\(([^\s()<>]+|(\([^\s()<>]+\)))*\)|[^\s`!()\[\]{};:'.,<>?«»“”‘’]))", "<a target='_blank' href=\"$1\">$1</a>");

                    //Add the link if it exists                
                    if (addthis)
                    {
                        string editTag = "";

                        if (IsEditable)
                        {
                            editTag = "<div class='wet-list-edit" + ModuleId + "' refid='textitem" + reader["ID"].ToString() + "'><span class='glyphicon glyphicon-pencil'></span></div>";
                        }
                        if (title != "")                           
                        {
                            
                            listpanel = "<div role=\"tabpanel\" id=\"panel" + count + "\" class=\"" + fadeType + "\">" +
                                            "<figure>" +
                                               editTag + "<span id='textitem" + reader["ID"].ToString() + "'>" + image + "</span>" +
                                                "<figcaption>" +
                                                    "<p><a target='blank' href='" + title + "'>" + description + "</a></p>" +
                                                "</figcaption>" +
                                            "</figure>" +
                                        "</div>";
                            listitem = "<a href=\"#panel" + count + "\">" + image + "</a>";

                            if (title != "")
                            {
                                listitem = "<div class='label label-default'>" + title + "</div>" + listitem;
                            }
                        }

                        if (ltlTabList.Text == "")
                        {
                            ltlTabList.Text += "<li class='active'>" + listitem + "</li>";
                        }
                        else
                        {
                            ltlTabList.Text += "<li>" + listitem + "</li>";
                        }
                        ltlTabPanels.Text += listpanel;

                        if (fadeType == "in fade") { fadeType = "out fade"; }
                    }
                    }
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