<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Gallery.ascx.cs" Inherits="DesktopModules_WET_Gallery_Gallery" %>

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
                    endpoint: '/DesktopModules/WET/Upload/Upload.aspx?FilePath=/Portals/<%= PortalId %>/Photos/Uploaded/<%= ModuleId %>/<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>/'
                },
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
                            var thislink = '/Portals/<%= PortalId %>/Photos/Uploaded/<%= ModuleId %>/<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>/thumb_' + fileName.replace(/\..+$/, '') + '_' + responseJSON.msAddOn + '.' + fileName.substr((fileName.lastIndexOf('.') + 1));
                            AddWetImage('<%= ModuleId %>', '<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>', fileName.substring(0, fileName.lastIndexOf(".")), thislink, getLastOrder('<%= ModuleConfiguration.ModuleID%>'), '<%= PortalAlias.HTTPAlias %>', "Gallery");
                        }
                    }
                }
            });

            //handles deleting items
            $('#<%= btnDeleteLink.ClientID %>').click(function (e) {
                e.preventDefault();
                var r = confirm('<%= Localization.GetString("msgDelete.Text", "/DesktopModules/WET/Gallery/App_LocalResources/Gallery")%>');
                if (r == true) {
                    var thisid = $('#<%= txtHiddenID.ClientID %>').text();
                    $.ajax({
                        url: "/DesktopModules/WET/Gallery/Save.asmx/Delete",
                        data: "{ 'itemid': '" + thisid +
                           "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8"
                    }).done(function (result) {
                        $('#<%= pnlEdit.ClientID %>').dialog("close");
                        $('#listitem' + thisid).remove();
                    });
                }
            });

            //handles editing items
            $(document).on('click', '.wet-list-edit<%= ModuleId %>', function (e) {
                e.preventDefault();
                var thisid = $(this).attr("refid");
                $('#<%= txtName.ClientID %>').val($('#' + thisid + ' img').attr("alt"));
                $('#<%= txtDescription.ClientID %>').val($('#' + thisid).parent().attr("title"));
                $('#<%= txtOrder.ClientID %>').val($('#' + thisid).parent().attr("order"));
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
                $.ajax({
                    url: "/DesktopModules/WET/Gallery/Save.asmx/Update",
                    data: "{ 'itemid': '" + $('#<%= txtHiddenID.ClientID %>').text() +
                        "','Title': '" + $('#<%= txtName.ClientID %>').val().replaceAll("'", "&#39;") +
                        "','Description': '" + $('#<%= txtDescription.ClientID %>').val().replaceAll("'", "&#39;") +
                        "','Order': '" + order +
                        "' }",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8"
                }).done(function (result) {
                    $('#textitem' + $('#<%= txtHiddenID.ClientID %>').text() + ' img').attr("alt", $('#<%= txtName.ClientID %>').val());
                    $('#textitem' + $('#<%= txtHiddenID.ClientID %>').text()).parent().attr("title", $('#<%= txtDescription.ClientID %>').val());
                    $('#textitem' + $('#<%= txtHiddenID.ClientID %>').text()).parent().attr("order", order);
                    $('#listitem' + $('#<%= txtHiddenID.ClientID %>').text() + ' .label').html($('#<%= txtName.ClientID %>').val());
                    $('.DnnModule-<%= ModuleId %> .wet-list').prepend($("#listitem" + $('#<%= txtHiddenID.ClientID %>').text()));
                    $('.DnnModule-<%= ModuleId %> .wet-list li a').each(function (index) {
                        if ($(this).attr("order") != undefined && $(this).parent().attr("id").replace("listitem", "") != $('#<%= txtHiddenID.ClientID %>').text()) {

                            if (parseInt(order) >= parseInt($(this).attr("order"))) {
                                $(this).parent().after($("#listitem" + $('#<%= txtHiddenID.ClientID %>').text()));
                            }
                        }
                    });
                });
                $('#<%= pnlEdit.ClientID %>').dialog("close");
            });

        });

        //Load the dialog for editing an item
        function LoadEdit<%= ModuleConfiguration.ModuleID %>() {
            var obj = $('#<%= pnlEdit.ClientID %>').dialog("open");
        }

    </script>
</asp:Panel>

<section class="wb-lbx lbx-gal">
    <ul class="list-inline wet-list text-center">
        <asp:Literal runat="server" ID="ltlLinks"></asp:Literal>
    </ul>
</section>

<asp:Panel runat="server" ID="pnlEdit" style="display:none">
    <div class="container-fluid">
        <div class="row center-block form-horizontal mrgn-tp-md" >
            <div class="form-group">
                <asp:label ID="Label1" runat="server" resourcekey="lblName.Text" CssClass="control-label col-md-4" AssociatedControlID="txtName"></asp:label>
                <asp:textbox runat="server" ID="txtName" CssClass="form-control col-md-8"></asp:textbox>
            </div>
            <div class="form-group mrgn-tp-md">
                <asp:label ID="Label4" runat="server" resourcekey="lblDescription.Text" CssClass="control-label col-md-4" AssociatedControlID="txtDescription"></asp:label>
                <asp:textbox runat="server" ID="txtDescription" CssClass="form-control col-md-8"></asp:textbox>
            </div>
            <div class="form-group">
                <asp:label ID="Label3" runat="server" resourcekey="lblOrder.Text" CssClass="control-label col-md-4" AssociatedControlID="txtOrder"></asp:label>
                <asp:textbox runat="server" ID="txtOrder" CssClass="form-control col-md-8"></asp:textbox>
            </div>
            <div class="form-group col-md-12 text-right">
                <asp:LinkButton runat="server" ID="btnDeleteLink" Cssclass="btn btn-link" resourcekey="btnDelete.Text" />        
                <asp:LinkButton ID="btnSaveLink" runat="server" resourcekey="btnSave.Text" CssClass="btn btn-primary"></asp:LinkButton>
            </div>
            <asp:Label runat="server" ID="txtHiddenID" style="display:none"></asp:Label>
        </div>
    </div>
</asp:Panel>




