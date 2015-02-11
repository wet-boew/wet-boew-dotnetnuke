using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.Skins;
using DotNetNuke.UI.Skins.Controls;
using System.Collections;

public partial class DesktopModules_WET_Menu_Menu : DotNetNuke.Entities.Modules.PortalModuleBase
{
    ModuleController MC = new ModuleController();
    TabController TC = new TabController();
    string ThisLocale = System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2);
    LocaleController LC = new LocaleController();

    protected void Page_Load(object sender, System.EventArgs e)
    {
        try
        {
            if (Page.IsPostBack == false)
            {
                string DefaultLocale = LC.GetDefaultLocale(PortalSettings.PortalId).Code.Substring(0,2);
                ltlMenu.Text = "";
                int ParentTab = -1;
                Boolean SubPages = false;
                Boolean ShowHidden = false;

                if (ModuleConfiguration.ModuleSettings["Recursive"] != null)
                {
                    SubPages = Boolean.Parse(ModuleConfiguration.ModuleSettings["Recursive"].ToString());
                }

                if (ModuleConfiguration.ModuleSettings["IncludeInvisible"] != null)
                {
                    ShowHidden = Boolean.Parse(ModuleConfiguration.ModuleSettings["IncludeInvisible"].ToString());
                }

                if (ModuleConfiguration.ModuleSettings["ParentTab"] == null)
                {
                    ParentTab = TabId;
                }
                else
                {
                    ParentTab = int.Parse(ModuleConfiguration.ModuleSettings["ParentTab"].ToString());
                }

                dnnAddParent.SelectedPage = TC.GetTab(ParentTab, PortalSettings.PortalId, true);

                LoadMenu(ParentTab, SubPages, ShowHidden);
   
                if (IsEditable)
                {
                    cbxMenu.Checked = SubPages;
                    cbxShowHidden.Checked = ShowHidden;
                    pnlAdmin.ToolTip = Localization.GetString("titleAdmin", this.LocalResourceFile);
                }
            }
            btnAdmin.Visible = IsEditable;
        }
        catch (Exception ex)
        {
            AddMessage(ex.Message, "Error", ex);
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

    protected void LoadMenu(int ParentID, bool SubPages, bool ShowHidden)
    {
        try
        {
            string DefaultLocale = LC.GetDefaultLocale(PortalSettings.PortalId).Code.Substring(0, 2);

            //Menu options
            string currentitems = "";
            int tabLevel = TC.GetTab(ParentID,PortalSettings.PortalId).Level + 1;         

            //Get list of pages

            foreach (TabInfo T in TabController.GetPortalTabs(PortalSettings.PortalId, -1, false, "", true, false, true, true, true))
            {
                Boolean usePage = false;
                if (ParentID == T.ParentId)
                {
                    usePage = true;
                }
                if (SubPages && (tabLevel + 1 == T.Level))
                {
                    if (TC.GetTab(TC.GetTab(T.TabID, PortalSettings.PortalId).ParentId, PortalSettings.PortalId).ParentId == ParentID)
                    {
                        usePage = true;
                    }
                }
                if (usePage)
                {
                    if (T.IsVisible || ShowHidden)
                    {
                        string link = T.FullUrl;
                        string title = T.TabName;

                        if (DefaultLocale != ThisLocale)
                        {
                            Hashtable hash = TC.GetTabSettings(T.TabID);
                            if (hash["PageTitle" + ThisLocale] != null)
                            {
                                title = hash["PageTitle" + ThisLocale].ToString();
                            }
                        }

                        if (T.TabID == PortalSettings.ActiveTab.TabID)
                        {
                            currentitems += "<li class='active'><a href=\"" + link + "\">" + title + "</a></li>";                            
                        }
                        else
                        {
                            if (CheckSubPages(T.TabID) && T.Level == tabLevel)
                            {
                                currentitems += "<li class='bg-info'><a href=\"" + link + "\">" + title + "</a></li>";
                            }
                            else
                            {
                                currentitems += "<li><a href=\"" + link + "\">" + title + "</a></li>";
                            }
                        }
                    }
                }                
            }

            ltlMenu.Text = currentitems;

        }
        catch (Exception ex)
        {
            if (PortalSettings.UserInfo.IsSuperUser)
                ltlMenu.Text = ex.Message;
            Exceptions.LogException(ex);
        }
    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        MC.UpdateModuleSetting(ModuleId, "Recursive", cbxMenu.Checked.ToString());
        MC.UpdateModuleSetting(ModuleId, "ParentTab", dnnAddParent.SelectedItem.Value);
        MC.UpdateModuleSetting(ModuleId, "IncludeInvisible", cbxShowHidden.Checked.ToString());

        Response.Redirect(PortalSettings.ActiveTab.FullUrl.ToLower().Replace(System.Globalization.CultureInfo.CurrentUICulture.Name.ToLower() + "/", ""));
    }

    protected Boolean CheckSubPages(int ThisTabID)
    {
        Boolean HasSubPages = false;
        foreach (TabInfo T in TabController.GetPortalTabs(PortalSettings.PortalId, -1, false, "", true, false, true, true, true))
        {
            if (ThisTabID == T.ParentId)
            {
                HasSubPages = true;
            }
        }
        return HasSubPages;
    }
}