<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Messenger.aspx.vb" Inherits="Messenger" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/Messenger.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/protoutil.js" type="text/javascript"></script>
    <script src="js/jquery.contextMenu.js" type="text/javascript"></script>
    <script src="js/jquery.watermark-2.0.js" type="text/javascript"></script>
    <script src="js/Chat.js" type="text/javascript"></script>    
</head>    
<body>
    <form id="form1" runat="server">
        <div>
            <table border='0' cellspacing='0' cellpadding='0' class="MessengerBars">
                <tr>
                    <td style="width:50px">
                        <a runat="server" id="profileUser" target="_blank" title="Your Profile"><img class="MessengerAvatar" width="100" height="100" runat="server" id="imgUser" /></a>
                    </td>
                    <td style="vertical-align: top;">
                        <div id="UserName" class="UserName" hoverclass="MessengerLineButtonHover">
                        </div>
                        <div style="clear:both"></div>
                        <div id="StatusMessage" contenteditable="true" class="StatusMessage" hoverclass="MessengerLineButtonHover" onclick="EditStatusMessage()">
                        </div>
                    </td>
                </tr>
                <tr style="background: url('/img/cn-psnb2.gif');height: 32px;">
                    <td colspan="2">
                        <span class="MessangerToolbarItem" title="View message history" hoverclass="MessangerToolbarItemHover" onclick="OpenMessageHistory()">
                            <img src="img/history.png" />
                        </span>
                        <span class="MessangerToolbarItem" title="Create group chat" hoverclass="MessangerToolbarItemHover" onclick="GroupChat()">
                            <img src="img/invite.png" />
                        </span>
                        <span class="MessangerToolbarItem" title="Toggle sound" hoverclass="MessangerToolbarItemHover" onclick="ToggleSound()">
                            <img src="img/sound_on.png" id="soundimg" />
                        </span>
                        <span class="MessangerToolbarItem" title="View messenger help" hoverclass="MessangerToolbarItemHover" onclick="javascript:window.open('http://thewire.wst.pwgsc.gc.ca/Communities/Committees/WRIntranetCommittee/YournewIntranet/WesternChat.aspx')">
                            <img src="img/help.png" />
                        </span>
                        <span class="MessangerToolbarItem FilterBox" title="Filter Users" >
                            <input type="text" id="txtFilter" onkeyup="CheckUsers()" />
                        </span>
                    </td>
                </tr>
            </table>

            <div id="Users"  style="top:105px; position:absolute; overflow-y:auto; width:100%; height:73%"></div>
            <div><img src="/img/westernchat.jpg" class="WesternChatMainPage" /></div>
            <div style="display: none">
                <asp:Literal runat="server" ID="ltlEmail"></asp:Literal>
                <asp:Literal runat="server" ID="ltlUserName"></asp:Literal>
                <asp:Literal runat="server" ID="ltlPicture"></asp:Literal>
                <asp:Literal runat="server" ID="ltlGUID"></asp:Literal>
            </div>

            <script type="text/javascript">
                $(document).ready(function () {
                    ReCheckUsers();
                    LoadSession();
                    UpdateUser();
                    SoundManager_Init();
                });
                //Status Options =======================================================================================================================
                $(function () {
                    $.contextMenu({
                        selector: '.UserName',
                        trigger: 'left',
                        callback: function (key, options) {
                            var m = key;
                            window.console && console.log(m) || UpdateStatus(m);
                        },
                        items: {
                            "Online": { name: "Online", icon: "online" },
                            "Away": { name: "Away", icon: "away" },
                            "Busy": { name: "Busy", icon: "busy" },
                            "Phone": { name: "Phone", icon: "phone" }
                        }
                        
                    });
                    $('#txtFilter').watermark('User Search');
                });
            </script>
        </div>
    </form>
</body>        
</html>
