<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Edit.ascx.cs" Inherits="DesktopModules_WET_MLHTML_Edit" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link href="../../../boew-wet/css/wet-boew.min.css" rel="stylesheet" type="text/css" />

<%@ Register TagPrefix="dnnweb" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnncl" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnncl:DnnCssInclude ID="customJS" runat="server" FilePath="DesktopModules/HTML/edit.css" AddTag="false" />

<dnn:TextEditor runat="server" ID="txtAdminView" Width="100%" Height="430px" />
<div align="right">
    <asp:Button runat="server" ID="BtnSaveContent" resourcekey="Save.Text" CssClass="dnnPrimaryAction" OnClick="BtnSaveContent_Click" />
</div>

<asp:Label runat="server" ID="FirstEntry" Visible="false"></asp:Label>

<h2 class="SectionHeader" runat="server"><%=LocalizeString("dshVersions")%></h2>
<asp:DataGrid ID="dgVersions" runat="server" CssClass="Grid" OnItemDataBound="VersionsGridItemDataBound" OnItemCommand="VersionsGridItemCommand" DataKeyField="ItemID" AutoGenerateColumns="false" CellPadding="10">
    <HeaderStyle CssClass="GridHeader" />
    <Columns>
        <asp:BoundColumn HeaderText="Version" DataField="Version" />
        <asp:BoundColumn HeaderText="Date" DataField="LastModifiedOnDate" />
        <asp:BoundColumn HeaderText="User" DataField="DisplayName" />
        <asp:TemplateColumn>
            <HeaderTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:Image ID="imgDelete" runat="server" iconkey="ActionDelete" resourcekey="VersionsRemove" />
                        </td>
                        <td>
                            <asp:Image ID="imgPreview" runat="server" iconkey="View" resourcekey="VersionsPreview" />
                        </td>
                        <td>
                            <asp:Image ID="imgRollback" runat="server" iconkey="Restore" resourcekey="VersionsRollback" />
                        </td>
                    </tr>
                </table>
            </HeaderTemplate>
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:ImageButton ID="btnRemove" runat="server" CommandName="Remove" iconkey="ActionDelete" text="Delete" resourcekey="VersionsRemove" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnPreview" runat="server" CommandName="Preview" iconkey="View" text="Preview" resourcekey="VersionsPreview" />
                        </td>
                        <td>
                            <asp:ImageButton ID="btnRollback" runat="server" CommandName="RollBack" iconkey="Restore" text="Rollback" resourcekey="VersionsRollback" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateColumn>
    </Columns>
</asp:DataGrid>
<asp:Label runat="server" ID="NoVersions" resourcekey="NoVersions" Visible="false" />

<h2 class="SectionHeader"><%=LocalizeString("dshPreview")%></h2>
<asp:Literal ID="litPreview" runat="server" />

