<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MessageHistory.aspx.vb" Inherits="MessageHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="css/Messenger.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/Chat.js" type="text/javascript"></script>
    <script src="js/protoutil.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="display:none;">
        <div id="Email"><asp:Literal runat="server" ID="ltlEMail"></asp:Literal></div>
    </div>
    <div class="HistoryHeader" hoverclass="MessengerLineButtonHover" onclick="DeleteHistory()"><img align="absmiddle" src="../img/delete.png" /> Delete All History</div>
    <div id="MessageHistory">
    
    </div>
            <script type="text/javascript">
               window.onload = GetMessageHistory;             
            </script> 
    </form>
</body>
</html>
