<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Chat.aspx.vb" Inherits="Chat" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="x-ua-compatible" content="IE=8">
    <title>Chat</title>
    <link href="css/Messenger.css" rel="stylesheet" type="text/css" />
    <link href="css/gemoticons.css" rel="stylesheet" type="text/css"  />
    <script src="js/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="js/jquery.gemoticons.js" type="text/javascript"></script>
    <script src="js/Chat.js" type="text/javascript"></script>    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="Messages"></div>
        <div class="fixed-right">
             <div id="Users"></div>
        </div>
        <div class='fixed-bottom'>
            <div style="height:20%">
                <img src="/img/westernchat.jpg" class="WesternChat" />
            </div>
            <div style="text-align:center; height:auto;">
                <div class="emoticon-icons">
                    <div id="emoteSmile" onclick="AddEmoticon('smile')">:)</div>
                    <div id="emoteGrin" onclick="AddEmoticon('grin')">:D</div>
                    <div id="emoteWink" onclick="AddEmoticon('wink')">;)</div>
                    <div id="emoteCry" onclick="AddEmoticon('cry')">:'(</div>
                    <div id="emoteShocked" onclick="AddEmoticon('shocked')">:-o</div>
                    <div id="emoteSlant" onclick="AddEmoticon('slant')">:-/</div>
                    <div id="emoteAngry" onclick="AddEmoticon('angry')">x-(</div>
                    <div id="emoteFrown" onclick="AddEmoticon('frown')">:(</div>
                    <div id="emoteCool" onclick="AddEmoticon('cool')">B-)</div>
                    <div id="emoteTongue" onclick="AddEmoticon('tongue')">:P</div>
                    <div id="emoteStraightFace" onclick="AddEmoticon('straight-face')">:-|</div>
                </div>            
                <div class="open-video">
                    <%--<a href="http://genesis.wst.pwgsc.gc.ca:5080/screenshare/screenshare.jnlp" onclick="ShareDesktop()"><asp:Image runat="server" ID="desktopbtn" ImageUrl="~/img/desktop.png" ToolTip="Share your desktop" /></a>&nbsp;&nbsp;--%>
                    <asp:ImageButton ID="camerabtn" runat="server" ImageUrl="~/img/camera.png" OnClientClick="SendVideo()" ToolTip="Show your webcam" />
                </div>
            </div>
            <div style="height:60%; text-align:center;">
                <textarea type="text" aria-multiline="true" id="MessageBox" maxlength="999" runat="server" />
            </div>
        </div>        
    </div>
    <div style="display:none;">
        <div id="FromEmail"><asp:Literal runat="server" ID="ltlFrom"></asp:Literal></div>
        <div id="ToID"><asp:Literal runat="server" ID="ltlTo"></asp:Literal></div>
        <div id="UserName"><asp:Literal runat="server" ID="ltlUserName"></asp:Literal></div>
    </div>
    <script type="text/javascript">
        window.onload = LoadChatPage;
        var charLimit = 999;

        $('#MessageBox').on("paste", function () {
            if ($('#MessageBox').val().length > charLimit) { $('#MessageBox').val($('#MessageBox').val().substring(0, charLimit)) }
        });
    
        $(document).keypress(function (e) {
                      
            if ($('#MessageBox').val().length > charLimit) { $('#MessageBox').val($('#MessageBox').val().substring(0, charLimit)) }

            if (e.keyCode == "13") {
                if (e.shiftKey == true) {
                    $('#MessageBox').val = $('#MessageBox').val + "\n";
                }
                else {
                    e.preventDefault();
                    if ($('#MessageBox').val() != '') {
                        AddMessage();
                        $('#MessageBox').val("");
                    }
                }
            }
        });
    </script>
    </form>
</body>
</html>
