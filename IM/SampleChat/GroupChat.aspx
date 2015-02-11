<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GroupChat.aspx.vb" Inherits="GroupChat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="css/Messenger.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/Chat.js" type="text/javascript"></script>
    <script src="js/protoutil.js" type="text/javascript"></script>
</head>
<body class="groupchatbody">
    <form id="form1" runat="server">
        <div style="display: none;">
            <div id="Email"><asp:Literal runat="server" ID="ltlEMail"></asp:Literal></div>
            <div id="varGUID"><asp:Literal runat="server" ID="ltlGUID"></asp:Literal></div>
            <div id="GroupList"></div>
        </div>
        <div id="Added" style="display:none;">
            <span class="userdisplayname">The following users will be added to your group chat:</span>
            <ul id="ListUL" style="padding:0 0 0 15px;">

            </ul>
            <input id="ok" type="button" value="Begin Group Chat" onclick="OpenGroupChat()" />
            <hr />
        </div>
        <div>
            <div style="padding-bottom:10px;"><span class="userdisplayname">Type a users name to narrow down the list</span></div>
            <span  title="Filter Users">
                <input type="text" id="txtFilter" onkeyup="GetGroupChatContacts()" />
            </span>
        </div>
        <div id="Selected">
        </div>
        <div id="ContactsList">
        </div>
        <img src="/img/westernchat.jpg" class="WesternChatMainPage" />
        <script type="text/javascript">
            window.onload = GetGroupChatContacts;
        </script>
    </form>
</body>
</html>