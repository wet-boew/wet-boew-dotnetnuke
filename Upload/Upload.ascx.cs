using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Exceptions;

public partial class DesktopModules_WET_Upload_Upload : DotNetNuke.Entities.Modules.PortalModuleBase
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (UserInfo.IsSuperUser)
        {
            AddMessage(Localization.GetString("lblAdmin.Text", this.LocalResourceFile).Replace("[CODE]", HttpUtility.HtmlEncode(Localization.GetString("msgCode.Text", this.LocalResourceFile))), "Warning");
        }
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