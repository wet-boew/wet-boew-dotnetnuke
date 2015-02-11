using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins;

public partial class DesktopModules_WET_SQL_SQL : DotNetNuke.Entities.Modules.PortalModuleBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnAdmin.Visible = UserInfo.IsSuperUser;
            if (UserInfo.IsSuperUser)
                btnAdmin.Visible = IsEditable;
            btnSave.Visible = UserInfo.IsSuperUser;
            pnlAdmin.Visible = UserInfo.IsSuperUser;

            if (Page.IsPostBack == false)
            {
                pnlAdmin.ToolTip = Localization.GetString("titleAdmin.Text", this.LocalResourceFile);
                txtTemplate.Text = Localization.GetString("txtTemplate.Text", this.LocalResourceFile);
                txtRowTemplate.Text = Localization.GetString("txtRowTemplate.Text", this.LocalResourceFile);
                txtItemTemplate.Text = Localization.GetString("txtItemTemplate.Text", this.LocalResourceFile);
                ltlSQL.Text = Localization.GetString("msgDefault.Text", this.LocalResourceFile);

                //Get the SQL Query
                LoadSettings();
            }

            LoadSQL();
        }
        catch (Exception ex)
        {
            Exceptions.LogException(ex);
            if (UserInfo.IsSuperUser)
            {
                Skin.AddModuleMessage(this, ex.Message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
            }
            else
            {
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("msgError.Text", this.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
            }
        }
    }

    protected void LoadSettings()
    {
        ModuleController objModules = new ModuleController();

        //OBSOLETE
        //objModules.GetModuleSettings(ModuleId);
        //Hashtable hash = objModules.GetModuleSettings(ModuleId);

        //NEW
        //ModuleConfiguration.ModuleSettings["SQLTemplate"].ToString();

        if (ModuleConfiguration.ModuleSettings["SQLTemplate"] != null)
        {
            txtTemplate.Text = ModuleConfiguration.ModuleSettings["SQLTemplate"].ToString();
        }

        if (ModuleConfiguration.ModuleSettings["SQLItemTemplate"] != null)
        {
            txtItemTemplate.Text = ModuleConfiguration.ModuleSettings["SQLItemTemplate"].ToString();
        }

        if (ModuleConfiguration.ModuleSettings["SQLRowTemplate"] != null)
        {
            txtRowTemplate.Text = ModuleConfiguration.ModuleSettings["SQLRowTemplate"].ToString();
        }

        if (ModuleConfiguration.ModuleSettings["SQLQuery"] != null)
        {
            txtSqlQuery.Text = ModuleConfiguration.ModuleSettings["SQLQuery"].ToString();
        }
    }

    protected void LoadSQL()
    {

        //OBSOLETE
        //DotNetNuke.Entities.Modules.ModuleController objModules = new DotNetNuke.Entities.Modules.ModuleController();
        //objModules.GetModuleSettings(ModuleId);
        //Hashtable hash = objModules.GetModuleSettings(ModuleId);

        //NEW
        //ModuleConfiguration.ModuleSettings["SQLTemplate"].ToString();

        if (ModuleConfiguration.ModuleSettings["SQLQuery"] != null)
        {
            SqlConnection ConnString = new SqlConnection(ConfigurationManager.AppSettings["SiteSqlServer"].ToString());
            ConnString.Open();

            SqlCommand WETCommand = new SqlCommand();
            WETCommand.CommandText = ModuleConfiguration.ModuleSettings["SQLQuery"].ToString();
            if (Request.QueryString["p1"] != null) {
                WETCommand.Parameters.AddWithValue("@SQLParam1", Request.QueryString["p1"].ToString());
            }
            else
            {
                WETCommand.Parameters.AddWithValue("@SQLParam1", "");
            }
            if (Request.QueryString["p2"] != null) {
                WETCommand.Parameters.AddWithValue("@SQLParam2", Request.QueryString["p2"].ToString());
            }
            else
            {
                WETCommand.Parameters.AddWithValue("@SQLParam2", "");
            }
            if (Request.QueryString["p3"] != null) {
                WETCommand.Parameters.AddWithValue("@SQLParam3", Request.QueryString["p3"].ToString());
            }
            else
            {
                WETCommand.Parameters.AddWithValue("@SQLParam3", "");
            }
            WETCommand.Connection = ConnString;
            try
            {
                SqlDataReader reader = default(SqlDataReader);
                reader = WETCommand.ExecuteReader();

                string TotalQuery = "";

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        string ItemList = "";

                        int ItemCount = 0;
                        while (ItemCount < reader.FieldCount)
                        {
                            //VB - reader.Item(ItemCount).ToString
                            //C# - 
                            ItemList = ItemList + ModuleConfiguration.ModuleSettings["SQLItemTemplate"].ToString().Replace("[QUERY]", reader[ItemCount].ToString());
                            ItemCount = ItemCount + 1;
                        }

                        TotalQuery = TotalQuery + ModuleConfiguration.ModuleSettings["SQLRowTemplate"].ToString().Replace("[QUERY]", ItemList);

                    }

                    TotalQuery = ModuleConfiguration.ModuleSettings["SQLTemplate"].ToString().Replace("[QUERY]", TotalQuery);
                }
                else
                {
                    TotalQuery = Localization.GetString("msgNoResults.Text", this.LocalResourceFile);
                }

                ltlSQL.Text = System.Web.HttpUtility.HtmlDecode(TotalQuery);
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
                if (UserInfo.IsSuperUser)
                {
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, ex.Message, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
                }
                else
                {
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, Localization.GetString("msgError.Text", this.LocalResourceFile), DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
                }
            }
            finally
            {
                ConnString.Close();
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DotNetNuke.Entities.Modules.ModuleController objModules = new DotNetNuke.Entities.Modules.ModuleController();
        objModules.UpdateModuleSetting(ModuleId, "SQLQuery", txtSqlQuery.Text);
        objModules.UpdateModuleSetting(ModuleId, "SQLTemplate", txtTemplate.Text);
        objModules.UpdateModuleSetting(ModuleId, "SQLItemTemplate", txtItemTemplate.Text);
        objModules.UpdateModuleSetting(ModuleId, "SQLRowTemplate", txtRowTemplate.Text);
        Response.Redirect(PortalSettings.ActiveTab.FullUrl.ToLower().Replace(System.Globalization.CultureInfo.CurrentUICulture.Name.ToLower() + "/", ""));
    }
}