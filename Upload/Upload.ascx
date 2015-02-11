<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Upload.ascx.cs" Inherits="DesktopModules_WET_Upload_Upload" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>

<link href="/DesktopModules/WET/Upload/FineUpload/fineuploader-3.3.0.css" rel="stylesheet" type="text/css" />
<script src="/DesktopModules/WET/Upload/FineUpload/jquery.fineuploader-3.3.0.js" type="text/javascript"></script>

<dnn:DnnFolderDropDownList runat="server" ID="dnnFolderList" AutoPostBack="true" ></dnn:DnnFolderDropDownList>

<% if (dnnFolderList.SelectedFolder != null) { %>
<asp:Panel runat="server" ID="pnlUpload"></asp:Panel>

<script type="text/javascript" lang="javascript">

    $(document).ready(function () {

        var uploader = new qq.FineUploader({
            element: document.getElementById('<%= pnlUpload.ClientID %>'),
            request: {
                endpoint: '/DesktopModules/WET/Upload/Upload.aspx?Folder=' + getFolderGUID()
            },
            text: {
                uploadButton: '<%= Localization.GetString("msgUpload.Text", this.LocalResourceFile) %>',
                dragZone: '<%= Localization.GetString("msgDrag.Text", this.LocalResourceFile) %>'
            },
            <%= Localization.GetString("msgTemplate.Text", this.LocalResourceFile) %>
            validation: {
                sizeLimit: 2147483647
            }
        });

    });

    //Get the ID from a dnnpagelist dropdown
    function getFolderID(value) {
        if (value.indexOf("key") <= 0) {
            value = "-1";
        }
        else {
            value = value.substr(value.indexOf("key") + 6);
            value = value.substr(0, value.indexOf('"'));
        }
        return value;
    }

    function getFolderGUID() {
        var returnVal = '';
        <% 
            if (dnnFolderList.SelectedFolder != null) {
        %>
                returnVal = '<%= dnnFolderList.SelectedFolder.UniqueId.ToString() %>';
        <%               
           }
        %>
        return returnVal;
    }

    

</script>

<% } %>
