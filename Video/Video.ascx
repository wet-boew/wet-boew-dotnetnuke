<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Video.ascx.cs" Inherits="DesktopModules_WET_Video_Video" %>

<link href="/DesktopModules/WET/Upload/FineUpload/fineuploader-3.3.0.css" rel="stylesheet" type="text/css" />
<script src="/DesktopModules/WET/Upload/FineUpload/jquery.fineuploader-3.3.0.js" type="text/javascript"></script>

<asp:Panel runat="server" ID="pnlAdmin">
    <asp:Panel runat="server" ID="fineuploader"></asp:Panel>
    <script type="text/javascript">

        $(document).ready(function () {
            var uploader = new qq.FineUploader({
                element: document.getElementById('<%= fineuploader.ClientID %>'),
                request: {
                    endpoint: '/DesktopModules/WET/Upload/Upload.aspx?FilePath=/Portals/<%= PortalId %>/Videos/Uploaded/<%= ModuleId %>/<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>/'
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
                        var link = '/Portals/<%= PortalId %>/Videos/Uploaded/<%= ModuleId %>/<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>/' + fileName.replace(/\..+$/, '') + '_' + responseJSON.msAddOn + '.' + fileName.substr((fileName.lastIndexOf('.') + 1));
                        if (fileName.indexOf('.mp4') <= 0) {
                            $('.video-module-<%= ModuleId %>').attr("poster", link);
                            AddWetVideo('<%= ModuleId %>', '<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>', link, '<%= PortalAlias.HTTPAlias %>', '<%= PortalSettings.PortalId %>', "poster");
                        }
                        else {
                            AddWetVideo('<%= ModuleId %>', '<%= System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0,2) %>', link, '<%= PortalAlias.HTTPAlias %>', '<%= PortalSettings.PortalId %>', "video");
                        }
                    }
                }
            }
        });
    });

    </script>
</asp:Panel>

<asp:Label runat="server" ID="lblVideos"></asp:Label>

<style type="text/css">
    .glyphicon-subtitles
    {
        display: none;
    }
</style>
