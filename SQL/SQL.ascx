<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SQL.ascx.cs" Inherits="DesktopModules_WET_SQL_SQL" %>

<asp:LinkButton runat="server" ID="btnAdmin" resourcekey="btnAdmin.Text" />

<asp:Literal runat="server" ID="ltlSQL"></asp:Literal>

<asp:Panel runat="server" ID="pnlAdmin" style='display:none;'>
    <p>
        <asp:Label ID="Label1" runat="server" AssociatedControlID="txtSqlQuery" resourceKey="lblQuery.Text" CssClass="sql-lbl" /><br />

	    <asp:TextBox runat="server" ID="txtSqlQuery" TextMode="MultiLine" Rows="5" Columns="50" CssClass="form-control sql-tbx" /> <br />
        
        <asp:Label ID="Label2" runat="server" AssociatedControlID="txtTemplate" resourceKey="lblTemplate.Text" CssClass="sql-lbl"/><br />
        
        <asp:TextBox runat="server" ID="txtTemplate" TextMode="MultiLine" Rows="5" Columns="50" CssClass="form-control sql-tbx" /> <br />
        
        <asp:Label ID="Label3" runat="server" AssociatedControlID="txtRowTemplate" resourceKey="lblRowTemplate.Text" CssClass="sql-lbl"/><br />
        
        <asp:TextBox runat="server" ID="txtRowTemplate" TextMode="MultiLine" Rows="5" Columns="50" CssClass="form-control sql-tbx" /> <br />
        
        <asp:Label ID="Label4" runat="server" AssociatedControlID="txtItemTemplate" resourceKey="lblItemTemplate.Text" CssClass="sql-lbl" /><br />
       
        <asp:TextBox runat="server" ID="txtItemTemplate" TextMode="MultiLine" Rows="5" Columns="50" CssClass="form-control sql-tbx" /> <br />
        
        
        <div class="text-right">
          <asp:LinkButton runat="server" ID="btnSave" resourceKey="btnSave.Text" CssClass="btn btn-primary" OnClick="btnSave_Click" /> 
        </div>

    </p>
</asp:Panel>

<script type="text/javascript" language="javascript">
    function CallDeletePost() {
        return confirm('<%= Localization.GetString("msgDelete.Text", this.LocalResourceFile) %>');
    }

    $(document).ready(function () {

        $('#<%= pnlAdmin.ClientID %>').dialog({
            autoOpen: false,
            width: 600,
            dialogClass: "dnnFormPopup",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });

        $('#<%= btnAdmin.ClientID %>').click(function () {
            $('#<%= pnlAdmin.ClientID %>').dialog("open");
            return false;
        });

        //Add the custom edit options to the menu
        $('.DnnModule-<%= ModuleConfiguration.ModuleID%> .h2-module-title').append($('#<%= btnAdmin.ClientID%>'));


    });


</script>