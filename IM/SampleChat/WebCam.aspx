<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WebCam.aspx.vb" Inherits="WebCam" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
<head>
    <title>Video Chat</title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <script type="text/javascript" src="js/swfobject.js"></script>
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/Chat.js"></script>
    <script type="text/javascript">
        var type = geturl('type');
        var flashvars = { VideoName: geturl('touser') };
        if (type == 1) {
            swfobject.embedSWF("vc_publish.swf", "myContent", "100%", "100%", "8.0.0", "expressInstall.swf", flashvars);
        }
        else {
            swfobject.embedSWF("vc_recieve.swf", "myContent", "100%", "100%", "8.0.0", "expressInstall.swf", flashvars);
        }

        function geturl(name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regexS = "[\\?&]" + name + "=([^&#]*)";
            var regex = new RegExp(regexS);
            var results = regex.exec(window.location.href);
            if (results == null) return "";
            else return results[1];
        }

        window.onbeforeunload = function (event) {
            if (type == 1) {
                CloseVideo();            
                return null;
            }
        };
		</script>
    <style media="screen" type="text/css">
        html, body, #myContent {
            height: 100%;
        }
        body {
            margin: 0;
            padding: 0;
            overflow: hidden;
        }
        /* this is for ie*/
        object {
            position: absolute;
            outline: none;
        }
    </style>
</head>
<body>
    <div style="display:none;">
        <div id="FromEmail"><asp:Literal runat="server" ID="ltlFrom"></asp:Literal></div>
        <div id="ToID"><asp:Literal runat="server" ID="ltlTo"></asp:Literal></div>
        <textarea type="text" id="MessageBox"  runat="server">has stopped sharing their webcam</textarea>
    </div>
    <div id="myContent">
        <h1>You need the Adobe Flash Player for this demo, download it by clicking the image below.</h1>
        <p>
            <a href="http://www.adobe.com/go/getflashplayer">
                <img src="http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif" alt="Get Adobe Flash player" /></a>
        </p>
    </div>
</body>
</html>
