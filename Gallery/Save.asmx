<%@ WebService Language="C#" Class="Gallery_Save" %>

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

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// <System.Web.Script.Services.ScriptService()> _
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[ScriptService()]
public class Gallery_Save : System.Web.Services.WebService
{

	DotNetNuke.Entities.Modules.ModuleController MC = new DotNetNuke.Entities.Modules.ModuleController();
	DotNetNuke.Entities.Modules.ModuleInfo TempModuleInfo = new DotNetNuke.Entities.Modules.ModuleInfo();
	DotNetNuke.Entities.Modules.ModuleController objModules = new DotNetNuke.Entities.Modules.ModuleController();

    SqlConnection ConnString = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["SiteSqlServer"].ToString());

	[WebMethod()]
	public string AddNew(string Locale, int ModuleID, string Title, string Link)
	{
        string returnval = "";
		ConnString.Open();
		SqlCommand WETCommand = new SqlCommand();
     	WETCommand.CommandText = "INSERT INTO WET_List(LinkName, LinkDescription, LinkURL, Locale, ListOrder, ModuleID) VALUES(@Name, '', @Link, @Locale, ISNULL((SELECT Top 1 ListOrder FROM WET_List WHERE ModuleID = @ModuleID Order By ListOrder Desc),0) + 1, @ModuleID) SELECT @@IDENTITY";

		WETCommand.Parameters.AddWithValue("@ModuleID", ModuleID);
		WETCommand.Parameters.AddWithValue("@Name", Title);
        WETCommand.Parameters.AddWithValue("@Locale", Locale);
        WETCommand.Parameters.AddWithValue("@Link", Link);        
		WETCommand.Connection = ConnString;

		try {
			returnval = WETCommand.ExecuteScalar().ToString();
		} catch (Exception ex) {
            returnval = ex.Message;
		} finally {
			ConnString.Close();
			ConnString.Dispose();
		}

        return returnval;
	}

    [WebMethod()]
    public string Update(int itemid, string Title, string Description, string Order)
    {
        string returnval = "";
        ConnString.Open();
        SqlCommand WETCommand = new SqlCommand();

        if (Order == "-1")
        {
            WETCommand.CommandText = "UPDATE WET_List SET LinkName = @Title, LinkDescription = @Description WHERE ID = @ItemID";
        }
        else
        {
            WETCommand.CommandText = "UPDATE WET_List SET LinkName = @Title, LinkDescription = @Description, ListOrder = @Order WHERE ID = @ItemID";
            WETCommand.Parameters.AddWithValue("@Order", Order);            
        }        
        
        WETCommand.Parameters.AddWithValue("@Title", Title);
        WETCommand.Parameters.AddWithValue("@Description", Description);
        WETCommand.Parameters.AddWithValue("@ItemID", itemid);
        
        WETCommand.Connection = ConnString;

        try
        {
            WETCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            returnval = ex.Message;
        }
        finally
        {
            ConnString.Close();
            ConnString.Dispose();
        }

        return returnval;
    }

    [WebMethod()]
    public string Delete(int itemid)
    {
        string returnval = "";
        ConnString.Open();
        SqlCommand WETCommand = new SqlCommand();
        
        WETCommand.CommandText = @"DECLARE @ReturnVal VARCHAR(500) 
                                   SET @ReturnVal = (SELECT LinkURL FROM WET_List WHERE ID = @ItemID)
                                   DELETE FROM WET_List WHERE ID = @ItemID
                                   SELECT @ReturnVal";

        WETCommand.Parameters.AddWithValue("@ItemID", itemid);
        WETCommand.Connection = ConnString;  

        try
        {
            string filePath = WETCommand.ExecuteScalar().ToString();                       
            System.IO.File.Delete(Server.MapPath(filePath)); 
            
            //delete thumb
            filePath = filePath.Substring(0, filePath.LastIndexOf("/") + 1) + "thumb_" + filePath.Substring(filePath.LastIndexOf("/") + 1);
            System.IO.File.Delete(Server.MapPath(filePath));           
        }
        catch (Exception ex)
        {
            returnval = ex.Message;
        }
        finally
        {
            ConnString.Close();
            ConnString.Dispose();
        }

        return returnval;
    }

}