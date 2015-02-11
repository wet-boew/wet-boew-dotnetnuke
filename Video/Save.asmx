<%@ WebService Language="C#" Class="Video_Save" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using DotNetNuke;
using System.Web.Script.Services;
using System.Data.SqlClient;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// <System.Web.Script.Services.ScriptService()> _
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService()]
public class Video_Save : System.Web.Services.WebService
{

	ModuleController MC = new ModuleController();
    LocaleController LC = new LocaleController();

	[WebMethod()]
	public void AddNew(string ThisLocale, int ModuleID, int PortalID, string Link, string type)
	{
        string DefaultLocale = LC.GetDefaultLocale(PortalID).Code.Substring(0, 2);
        string locale = "";
        
        if (DefaultLocale != ThisLocale) {
            locale = ThisLocale;
        }

        if (type == "poster")
        {
            MC.UpdateModuleSetting(ModuleID, "Poster" + locale.ToUpper(), Link);
        }
        else
        {
            MC.UpdateModuleSetting(ModuleID, "Video" + locale.ToUpper(), Link);
        }     
	}

}