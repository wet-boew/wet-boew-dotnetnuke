<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="DesktopModules_WET_Menu_Menu" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<asp:Panel runat="server" ID="pnlAdmin" style="display:none;overflow:hidden">
     <div class="container-fluid">
        <div class="row center-block form-horizontal mrgn-tp-md" >
            <div class="form-group">
                <asp:Label runat="server" ID="lblMenu" resourcekey="lblMenu.Text" AssociatedControlID="dnnAddParent" CssClass="control-label col-md-4"></asp:Label>
                <dnn:DnnPageDropDownList ID="dnnAddParent" runat="server" IncludeActiveTab="true" IncludeDisabledTabs="true" CssClass="col-md-8"   />
            </div>
            <div class="form-group">
                <div class="col-md-4"></div>
                <asp:CheckBox runat="server" ID="cbxMenu" resourcekey="cbxMenu.Text" CssClass="checkbox col-md-8" />
            </div>
             <div class="form-group">
                <div class="col-md-4"></div>
                <p><asp:CheckBox runat="server" ID="cbxShowHidden" resourcekey="cbxHidden.Text" CssClass="checkbox col-md-8" /></p>
            </div>
             <div class="form-group">
                <div class="col-md-12 text-right ">
                    <asp:LinkButton runat="server" ID="btnSave" resourcekey="btnSave.Text" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                </div>
             </div>
        </div>
    </div>
</asp:Panel>

<div style="display:none">
    <asp:LinkButton runat="server" ID="btnAdmin" resourcekey="btnAdmin.Text"  />
</div>

<div class="nav nav-pills nav-stacked">
    <asp:Literal runat="server" ID="ltlMenu"></asp:Literal>
</div>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {

        $('#<%= pnlAdmin.ClientID %>').dialog({
            autoOpen: false,
            width: 450,
            height: 465,
            dialogClass: "dnnFormPopup",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });

        $('#<%= btnAdmin.ClientID %>').click(function (e) {
            e.preventDefault();
            $('#<%= pnlAdmin.ClientID %>').dialog("open");

        });

        //Add the custom edit options to the menu
        $('.DnnModule-<%= ModuleConfiguration.ModuleID%> .h2-module-title').append($('#<%= btnAdmin.ClientID %>'));

    });

</script>

