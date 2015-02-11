using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;

public partial class DesktopModules_WET_Video_Video : DotNetNuke.Entities.Modules.PortalModuleBase
{
    ModuleController MC = new ModuleController();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pnlAdmin.Visible = IsEditable;
            pnlAdmin.ToolTip = Localization.GetString("titleSettings.Text", this.LocalResourceFile);

            string videoPath = "";
            string posterPath = "";
            string locale = System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2).ToUpper();

            //Default values
            if (ModuleConfiguration.ModuleSettings["Video"] != null)
            {
                videoPath = ModuleConfiguration.ModuleSettings["Video"].ToString();
            }
            if (ModuleConfiguration.ModuleSettings["Poster"] != null)
            {
                posterPath = ModuleConfiguration.ModuleSettings["Poster"].ToString();
            }

            //For this locale
            if (ModuleConfiguration.ModuleSettings["Video" + locale] != null)
            {
                videoPath = ModuleConfiguration.ModuleSettings["Video" + locale].ToString();
            }
            if (ModuleConfiguration.ModuleSettings["Poster" + locale] != null)
            {
                posterPath = ModuleConfiguration.ModuleSettings["Poster" + locale].ToString();
            }

            if (videoPath == "")
            {
                if (IsEditable) { AddMessage(Localization.GetString("msgNoVideo.Text", this.LocalResourceFile), "Information"); }
            }
            else
            {
                if (IsEditable) {
                    if (posterPath == "" || videoPath == "")
                    {
                        AddMessage(Localization.GetString("msgStart.Text", this.LocalResourceFile), "Information");
                    }
                }
                ShowOneVideo(videoPath, posterPath);
            }

            if (UserInfo.IsSuperUser)
            {
                if (IsEditable) { AddMessage(Localization.GetString("msgSuper.Text", this.LocalResourceFile), "Warning"); }
            }

        }
        catch (Exception ex)
        {
            AddMessage(ex.Message, "Error", ex);
        }
    }

    protected void ShowOneVideo(string videoPath, string posterPath)
    {

        string title = videoPath.Substring(videoPath.LastIndexOf("/") + 1);

        string poster = "";
        if (posterPath != "") { poster = "poster='" + posterPath + "'"; }

        string mp4Video = "<source type='video/mp4' src='" + videoPath + "'></source>";

        lblVideos.Text = "<figure class='wb-mltmd' >" +
                            @"<video " + poster + @" title='" + title + @"' class='video-module-" + ModuleId + @"'>
		                        " + mp4Video + @"
	                        </video>                          
                        </figure>";
    }

    private void AddMessage(string message, string type, Exception ex = null)
    {
        switch (type)
        {
            case "Error":
                Exceptions.LogException(ex);
                if (UserInfo.IsSuperUser)
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
                else
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("msgError.Text", this.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
                break;

            case "Success":
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.GreenSuccess);
                break;

            case "Warning":
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.YellowWarning);
                break;

            case "Information":
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.BlueInfo);
                break;
        }
    }
}