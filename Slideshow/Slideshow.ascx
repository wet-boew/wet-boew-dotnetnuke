<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Slideshow.ascx.cs" Inherits="DesktopModules_WET_Slideshow_Slideshow" %>

<link href="/DesktopModules/WET/Upload/FineUpload/fineuploader-3.3.0.css" rel="stylesheet" type="text/css" />
<script src="/DesktopModules/WET/Upload/FineUpload/jquery.fineuploader-3.3.0.js" type="text/javascript"></script>

<asp:Panel runat="server" ID="pnlAdmin">
    <asp:Panel runat="server" ID="fineuploader"></asp:Panel>

    <script type="text/javascript">

        //Uploader
        $(document).ready(function () {
            var uploader = new qq.FineUploader({
                element: document.getElementById('<%= fineuploader.ClientID %>'),
                request: {
                    endpoint: '/DesktopModules/WET/Upload/Upload.aspx?FilePath=/Portals/<%= PortalId %>/Slides/Uploaded/<%= ModuleId %>/<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>/'
                },
                multiple: false,
                text: {
                    uploadButton: '<%= Localization.GetString("msgUpload.Text", "/DesktopModules/WET/Upload/App_LocalResources/Upload") %>',
                    dragZone: '<%= Localization.GetString("msgDrag.Text", "/DesktopModules/WET/Upload/App_LocalResources/Upload") %>',
                },
                <%= Localization.GetString("msgTemplate.Text", "/DesktopModules/WET/Upload/App_LocalResources/Upload") %>
                validation: {
                    sizeLimit: 121028608
                },
                callbacks: {
                    onComplete: function (id, fileName, responseJSON) {
                        if (responseJSON.success) {
                            var thislink = '/Portals/<%= PortalId %>/Slides/Uploaded/<%= ModuleId %>/<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>/thumb_' + fileName.replace(/\..+$/, '') + '_' + responseJSON.msAddOn + '.' + fileName.substr((fileName.lastIndexOf('.') + 1));
                            AddWetImage('<%= ModuleId %>', '<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>', fileName.substring(0, fileName.lastIndexOf(".")), thislink, getLastOrder('<%= ModuleConfiguration.ModuleID%>'), '<%= PortalAlias.HTTPAlias %>', "Slideshow");
                        }
                    }
                }
            });

            //handles hiding items
            $('#<%= cbxHide.ClientID %>').click(function (e) {                
                    var thisid = $('#<%= cbxHide.ClientID %>').checked();                
            });

            //handles deleting items
            $('#<%= btnDeleteLink.ClientID %>').click(function (e) {
                e.preventDefault();
                var r = confirm('<%= Localization.GetString("msgDelete.Text", "/DesktopModules/WET/Slideshow/App_LocalResources/Slideshow")%>');
                if (r == true) {
                    var thisid = $('#<%= txtHiddenID.ClientID %>').text();
                    $.ajax({
                        url: "/DesktopModules/WET/Slideshow/Save.asmx/Delete",
                        data: "{ 'itemid': '" + thisid +
                           "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8"
                    }).done(function (result) {
                        location.reload();
                    });
                }
            });

            //handles editing items
            $(document).on('click', '.wet-list-edit<%= ModuleId %>', function (e) {
                e.preventDefault();
                var thisid = $(this).attr("refid");
                $('#<%= txtLink.ClientID %>').val($('#' + thisid + ' img').attr("alt"));
                $('#<%= txtDescription.ClientID %>').val($('#' + thisid + ' img').attr("desc"));
                $('#<%= txtOrder.ClientID %>').val($('#' + thisid + ' img').attr("order"));
               
                var ClientID = $('#<%= txtOrder.ClientID %>').val()
                if (ClientID == "0") 
                {
                $('#<%= cbxHide.ClientID %>').prop('checked', true);
                }
                else  
                {
                $('#<%= cbxHide.ClientID %>').prop('checked', false);
                }

                $('#<%= txtHiddenID.ClientID %>').html(thisid.replace("textitem", ""));
                LoadEdit<%= ModuleConfiguration.ModuleID %>();
            });

            //Dialog for editing item
            $('#<%= pnlEdit.ClientID %>').dialog({
                autoOpen: false,
                modal: true,
                dialogClass: "dnnFormPopup",
                width: 450
            });

            //handles saving the edited item
            $('#<%= btnSaveLink.ClientID %>').click(function (e) {
                e.preventDefault();
                var order = $('#<%= txtOrder.ClientID %>').val();
                var hide = $('#<%= cbxHide.ClientID %>').prop('checked');
                $.ajax({
                    url: "/DesktopModules/WET/Slideshow/Save.asmx/Update",
                    data: "{ 'itemid': '" + $('#<%= txtHiddenID.ClientID %>').text() +
                        "','Title': '" + $('#<%= txtLink.ClientID %>').val().replaceAll("'", "&#39;") +
                        "','Description': '" + $('#<%= txtDescription.ClientID %>').val().replaceAll("'", "&#39;") +
                        "','Order': '" + order +
                        "','Hide': '" + hide +
                        "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8"
                }).done(function (result) {
                    location.reload();
                });
            });

        });

        //Load the dialog for editing an item
        function LoadEdit<%= ModuleConfiguration.ModuleID %>() {
            var obj = $('#<%= pnlEdit.ClientID %>').dialog("open");
        }

    </script>
</asp:Panel>

<div class="wb-tabs playing carousel-s2">
	<ul role="tablist">
        <asp:Literal runat="server" ID="ltlTabList"></asp:Literal>
	</ul>    
	<div class="tabpanels">
        <asp:Literal runat="server" ID="ltlTabPanels"></asp:Literal>
	</div>
</div>

<asp:Panel runat="server" ID="pnlEdit" style="display:none">
    <div class="container-fluid">
        <div class="row center-block form-horizontal mrgn-tp-md" >
<%--            <div class="form-group">
                <asp:label ID="Label1" runat="server" resourcekey="lblName.Text" CssClass="control-label col-xs-4" AssociatedControlID="txtName"></asp:label>
                <asp:textbox runat="server" ID="txtName" CssClass="form-control col-xs-8"></asp:textbox>
            </div>--%>
            <div class="form-group">
                <asp:label ID="Label4" runat="server" resourcekey="lblDescription.Text" CssClass="control-label col-xs-4" AssociatedControlID="txtDescription"></asp:label>
                <asp:textbox runat="server" ID="txtDescription" CssClass="form-control col-xs-8" TextMode="MultiLine" Columns="23" Rows="9"></asp:textbox>
            </div>
            <div class="form-group">
                <asp:label ID="Label1" runat="server" resourcekey="lblLink.Text" CssClass="control-label col-xs-4" AssociatedControlID="txtLink"></asp:label>
                <asp:textbox runat="server" ID="txtLink" CssClass="form-control col-xs-8"></asp:textbox>
            </div>
            <div class="form-group">
                <asp:label ID="Label3" runat="server" resourcekey="lblOrder.Text" CssClass="control-label col-xs-4" AssociatedControlID="txtOrder"></asp:label>
                <asp:textbox runat="server" ID="txtOrder" CssClass="form-control col-xs-8"></asp:textbox>
                <asp:Label ID="Label2" runat="server" resourcekey="lblHide.Text" CssClass="control-label col-xs-4" AssociatedControlID="cbxHide"></asp:Label>
                <asp:CheckBox ID="cbxHide" runat="server" CssClass="cbxBig" />
            </div>
            <div class="form-group col-xs-12 text-right">
                <asp:LinkButton runat="server" ID="btnDeleteLink" Cssclass="btn btn-link" resourcekey="btnDelete.Text" />        
                <asp:LinkButton ID="btnSaveLink" runat="server" resourcekey="btnSave.Text" CssClass="btn btn-primary"></asp:LinkButton>
            </div>
            <asp:Label runat="server" ID="txtHiddenID" style="display:none"></asp:Label>
        </div>
    </div>
</asp:Panel>
