//Variables
var WindowOpen = [];
var LastMessage = 0;

//Check the Users =======================================================================================================================
function CheckUsers() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'CheckUsers',
        UserName: encodeURIComponent($("#varUserName").html()),
        Email: encodeURIComponent($("#varEmail").html()),
        Picture: encodeURIComponent($("#varImageLink").html()),
        GUID: encodeURIComponent($("#varGUID").html()),
        Filter: encodeURIComponent($("#txtFilter").val())
    },
    function (myData) {
        if ($("#varEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#varEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#varEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }        
        $("#Users").html(myData.response.replace(/&amp;/gi, '&'));
    });
}

//Load the user information======================================================================================================================
function LoadChatPage() {
    SoundManager_Init();
    $(".emoticon-icons").gemoticon();
    CheckMessages();
    GetConversationUsers();
}

function GetConversationUsers() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'GetConversationUsers',
        FromEmail: $("#FromEmail").html(),
        ToID: $("#ToID").html()
    },
    function (myData) {
        if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        $("#Users").html(myData.response.replace(/&amp;/gi, '&'));
    });
}


//Update the user information on load =======================================================================================================================
function UpdateUser() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'UpdateUser',
        UserName: encodeURIComponent($("#varUserName").html()),
        Email: encodeURIComponent($("#varEmail").html()),
        Picture: encodeURIComponent($("#varImageLink").html())
    },
    function (myData) {
        if ($("#varEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#varEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#varEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
    });
}

//Load the startup info =======================================================================================================================
function LoadSession() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'LoadSession',
        Email: $("#varEmail").html()
    },
    function (myData) {
        if ($("#varEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#varEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#varEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        $("#UserName").html($("#varUserName").html() + " (" + "<img src='img/status_" + myData.userstatus + ".png' style='vertical-align:middle;' />" + myData.userstatus + ")" + "<img src='img/arrowdown.png' style='padding-left: 3px' />");
        $("#StatusMessage").html(decodeURIComponent(myData.userstatusmessage));        
    });

    if (document.cookie.indexOf("PWGSCIMSound") >= 0) {
        var v = GetCookie("PWGSCIMSound");
        if (v == "true")
            document.getElementById("soundimg").src = "img/sound_on.png";
        if (v == "false")
            document.getElementById("soundimg").src = "img/sound_off.png";
    }
    else {
        SetCookie("PWGSCIMSound", "true", 3600 * 24 * 365);
    }
}

//Change Users Status =======================================================================================================================
function UpdateStatus(m) {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'UpdateStatus',
        Email: $("#varEmail").html(),
        Status: m
    },
    function (myData) {
        if ($("#varEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#varEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#varEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
    });    
    $("#UserName").html($("#varUserName").html() + " (" + "<img src='img/status_" + m + ".png' style='vertical-align:middle;' />" + m + ")" + "<img src='img/arrowdown.png' style='padding-left: 3px' />");
}


function ReCheckUsers() {
    CheckUsers();
    CheckRecentMessages();
    setTimeout(ReCheckUsers, 10000);
}

//Check the Messages =======================================================================================================================
function ScrollBottom() {
    $("#Messages").animate({ scrollTop: $(document).height() + 1000 }, 500);
}

function CheckMessages() {
    //Check if the user is typing right now
    var IsTyping = 'False';
    if ($('#MessageBox').val() != '' && $('#MessageBox').is(':focus')) { IsTyping = 'True' }

    //Check the messages, and update if they are typing
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'CheckMessages',
        FromEmail: $("#FromEmail").html(),
        ToID: $("#ToID").html(),
        Typing: IsTyping
    },
    function (myData) {
        if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        if (LastMessage < myData.lastmessage || LastMessage == 1) {            
            if ($('#MessageBox').is(':focus') == false && LastMessage != 1) {
                window.focus();
                if ($("#Messages").html() != '') {
                    var v = GetCookie("PWGSCIMSound");
                    if (v == "true") {
                        //Change the .mp3 alert
                        SoundManager_Play("bubbles");
                    }
                }
            }
            $("#Messages").html(checkforurl(decodeURIComponent(myData.response.replace(/&amp;/gi, '&'))));
            $("#Messages").gemoticon();
            ScrollBottom();
            LastMessage = myData.lastmessage;
        }

        $(".typing").remove();
        if (myData.returntyping == 'True') {
            $("#Messages").append('<p class="typing">Typing...</p>');
            ScrollBottom();
        } 
        ReCheckMessages()
    });
}

function checkforurl(text) {
    var exp = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
    return text.replace(exp,"<a href='$1' target='blank'>$1</a>"); 
}

function ReCheckMessages() {
    setTimeout(CheckMessages, 2000);
}

//Check Recent Messages =======================================================================================================================
function CheckRecentMessages() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'CheckRecentMessages',
        ToEmail: $("#varEmail").html()
    },
    function (myData) {
        if ($("#varEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#varEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#varEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        var newUserIDs = myData.response.split(";");
        for (var i = 0; i < newUserIDs.length; i++) {
            if (newUserIDs[i] != "") {
                OpenChat(newUserIDs[i]);
            }
        }


    });
}

//Add a Message =======================================================================================================================
function AddMessage() {
    //Temporarily add it as a post
    var NewMessage = '<li><div class="timestamp">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</div><span style="color:#551155;" class="displayname">' + $("#UserName").html() + ':</span> ' + $("#MessageBox").val().replace(/\n/g, "<br />").replace(/script/gi, '') + '</li></ul>';
    $(".typing").remove();
    $("#Messages").html($("#Messages").html().replace("</ul>", "").replace("</UL>", "") + NewMessage);
    $("#Messages").gemoticon();
    ScrollBottom();
    LastMessage = 1;

    //Save the Message
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'AddMessage',
        Message: encodeURIComponent($("#MessageBox").val().replace(/%/gi, '&#37;')).replace(/'/gi, '&#39;'),
        FromEmail: $("#FromEmail").html(),
        ToID: $("#ToID").html()
    },
    function (myData) {
        if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        CheckMessagesAfterAdd();
    });
}

function CheckMessagesAfterAdd() {
    //Check if the user is typing right now
    var IsTyping = 'False';

    //Check the messages, and update if they are typing
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'CheckMessages',
        FromEmail: $("#FromEmail").html(),
        ToID: $("#ToID").html(),
        Typing: IsTyping
    },
    function (myData) {
        if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        $("#Messages").html(checkforurl(decodeURIComponent(myData.response.replace(/&amp;/gi, '&'))));
        $("#Messages").gemoticon();
        ScrollBottom();
        LastMessage = myData.lastmessage;
    });
}

//Open a new chat window =======================================================================================================================
function OpenChat(ToID) {
    var titleid;
    titleid = ToID;
    if ($.isNumeric(ToID)) { }
    else { titleid = titleid.replace(/,/gi, 'x') }

    var randNum = Math.ceil(Math.random() * 200) + 200;
    var makeSound = 'False';

    if (WindowOpen[ToID] == undefined) {
        WindowOpen[ToID] = window.open('/Chat.aspx?New=T&ID=' + ToID, 'Chat' + titleid, 'resizable=1,width=550,status=1,height=460,left=' + randNum);
        makeSound = 'True';
    } else if (WindowOpen[ToID].closed) {
        WindowOpen[ToID] = window.open('/Chat.aspx?New=T&ID=' + ToID, 'Chat' + titleid, 'resizable=1,width=550,status=1,height=460,left=' + randNum);
        makeSound = 'True';
    }

    if (makeSound == 'True') {
        var v = GetCookie("PWGSCIMSound");
        if (v == "true") {
            //Change the .mp3 alert
            SoundManager_Play("bubbles");
        }
    }
}

//Open a new chat window =======================================================================================================================
function OpenChatWithFocus(ToID) {
    var titleid;
    titleid = ToID;
    if ($.isNumeric(ToID)) { }
    else { titleid = titleid.replace(/,/gi, 'x') }

    var randNum = Math.ceil(Math.random() * 200) + 200;

    if (WindowOpen[ToID] == undefined) {
        WindowOpen[ToID] = window.open('/Chat.aspx?ID=' + ToID, 'Chat' + titleid, 'resizable=1,width=550,status=1,height=460,left=' + randNum);
    } else if (WindowOpen[ToID].closed) {
        WindowOpen[ToID] = window.open('/Chat.aspx?ID=' + ToID, 'Chat' + titleid, 'resizable=1,width=550,status=1,height=460,left=' + randNum);
    } else {
        WindowOpen[ToID].focus();
    }
}
//Web Cam functions =======================================================================================================================
function SendVideo() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
   {
       Method: 'AddMessage',
       Message: encodeURIComponent('a9a3721aac40b9828285077f3cea00a0'),
       FromEmail: $("#FromEmail").html(),
       ToID: $("#ToID").html()
   },
   function (myData) {
       if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
           if (myData.error != undefined) { alert(myData.error); }
       }
   });
    var win = window.open('http://thewire.wst.pwgsc.gc.ca:7060/WebCam.aspx?type=1&touser=' + $("#ToID").html(), '_blank', 'resizable=1,status=1,width=380,height=290,left=' + 500)
    return false;
}

function RecieveVideo(UserId) {
    window.open('http://thewire.wst.pwgsc.gc.ca:7060/WebCam.aspx?type=0&touser=' + UserId, '_blank', 'resizable=1,status=1,width=380,height=290,left=' + 500)
    return false;
}

function CloseVideo() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'AddMessage',
        Message: encodeURIComponent($("#MessageBox").val()).replace(/'/gi, '&#39;'),
        FromEmail: $("#FromEmail").html(),
        ToID: $("#ToID").html()
    },
    function (myData) {
        if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
    });
    alert('You have stopped sharing your webcam.') 
    return false;
}
//Update status message =======================================================================================================================
function EditStatusMessage() {
    var divHtml = $("#StatusMessage").html();
    var editableText = $("<input id='statmes'/>");
    editableText.val(divHtml);
    $("#StatusMessage").replaceWith(editableText);
    editableText.focus();
    editableText.blur(editableTextBlurred);
    $("#statmes").css("width", "200px")
    $("#statmes").keypress(function (event) {
        if (event.which == 13) {
            $("#Users").focus()
        }
    });
}

function editableTextBlurred() {
    var html = $(this).val().substring(0, 50);
    var viewableText = $("<div id='StatusMessage' contenteditable='true' class='StatusMessage' hoverclass='MessengerLineButtonHover'>");
    viewableText.html(html.replace(/script/gi, ""));
    $(this).replaceWith(viewableText);
    // setup the click event for this new div
    $(viewableText).click(EditStatusMessage);
    if (html == "") {
        $("#StatusMessage").text("Enter a personal message")
        html = "Enter a personal message"
    }
    ChangeStatusMessage(html)
};

function ChangeStatusMessage(html) {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'ChangeStatusMessage',
        Email: $("#varEmail").html(),
        Message: encodeURIComponent(html)
    },
    function (myData) {
        if ($("#varEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#varEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#varEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
    });
}

//Show Chat History =======================================================================================================================
function OpenMessageHistory() {
    window.open('http://thewire.wst.pwgsc.gc.ca:7060/MessageHistory.aspx', null, 'resizable=1,status=0,menubar=0,width=840,height=540')
}

function GetMessageHistory() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'GetMessageHistory',
        Email: $("#Email").html()
    },
    function (myData) {
        if ($("#Email").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#Email").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#Email").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        $("#MessageHistory").html(checkforurl(myData.response.replace(/&amp;/gi, '&').replace(/&script;/gi, '')));
    });
}
//Delete Chat History =======================================================================================================================
function DeleteHistory() {
    var r = confirm("Are you sure you want to delete all history?")
    if (r == true) {
        $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
        {
            Method: 'DeleteHistory',
            Email: $("#Email").html()
        },
        function (myData) {
            if ($("#Email").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#Email").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#Email").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
                if (myData.error != undefined) { alert(myData.error); }
            }
        });
        setTimeout(GetMessageHistory, 500);
    }
    else {
    }
}
//Group Chat =======================================================================================================================
function GroupChat() {
    window.open('http://thewire.wst.pwgsc.gc.ca:7060/GroupChat.aspx', null, 'scrollbars=1,resizable=1,status=0,menubar=0,width=350,height=500')
}
function GetGroupChatContacts() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'GetGroupChatContacts',
        Email: $("#Email").html(),
        Filter: encodeURIComponent($("#txtFilter").val())
    },
    function (myData) {
        if ($("#Email").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#Email").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#Email").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        $("#ContactsList").html(decodeURIComponent(myData.response));
    });
}
function AddToGroup(ID) {
    $('#Added').css("display", "block");
    if ($('#ListUL').find('#' + ID).length == 0) {
        $("#GroupList").append(',' + ID )
        $('#ListUL').append("<li id='" + ID + "'><img class='userimage' src='" + $('#' + ID).next().attr('src') + "' /> <span class='userdisplayname'>" + $('#' + ID).next().next().text() + "</span></li>");
    }
}
function OpenGroupChat() {
    //Get the UserID    
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
    {
        Method: 'GetUserID',
        GUID: encodeURIComponent($("#varGUID").html()),
        Email: encodeURIComponent($("#Email").html())
    },
    function (myData) {
        if ($("#Email").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#Email").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#Email").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
            if (myData.error != undefined) { alert(myData.error); }
        }
        var ChatWindowID = myData.response + $("#GroupList").html();
        OpenChatWithFocus(ChatWindowID);
        window.close();
    });
}
// Sound Settings and Sound Playing functions
function ToggleSound() {
    var img = document.getElementById("soundimg").src;
    if (img.indexOf('sound_on.png') != -1) {
        document.getElementById("soundimg").src = "img/sound_off.png";
        SetCookie("PWGSCIMSound", "false", 3600 * 24 * 365);
    }
    else {
        document.getElementById("soundimg").src = "img/sound_on.png";
        SetCookie("PWGSCIMSound", "true", 3600 * 24 * 365);
    }
}

var soundflash;
function SoundManager_Init() {
    var isIE = navigator.appName.toLowerCase().indexOf('internet explorer') + 1;
    var isMac = navigator.appVersion.toLowerCase().indexOf('mac') + 1;
    if (isIE && isMac) return;

    var url = "http://thewire.wst.pwgsc.gc.ca:7060/SoundPlayer.swf";
    var div = document.createElement("DIV");
    div.style.position = "absolute";
    div.style.left = "-256px";
    div.style.width = "64px";
    div.style.height = "64px";
    var flash;
    var http = "http";
    if (window.location.href.indexOf("https://") != -1) http = "https";

    if (isIE) {
        div.innerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="' + http + '://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0"><param name="movie" value="' + url + '"><param name="quality" value="high"></object>'
        flash = div.getElementsByTagName('object')[0];
    }
    else {
        div.innerHTML = '<embed src="' + url + '" width="1" height="1" quality="high" pluginspage="' + http + '://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash"></embed>'
        flash = div.getElementsByTagName('embed')[0];
    }
    document.body.insertBefore(div, document.body.firstChild);
    soundflash = flash;
}

function SoundManager_Play(soundID) {
    var url = "http://thewire.wst.pwgsc.gc.ca:7060/sound/" + soundID + ".mp3";

    try {
        soundflash.SetVariable("_root.action", "Play:" + url);
    }
    catch (x) {
    }

}
// Sharing Desktop Functions
function ShareDesktop() {
    $.getJSON('http://thewire.wst.pwgsc.gc.ca:7061/ChatService.ashx?callback=?',
   {
       Method: 'AddMessage',
       Message: encodeURIComponent('a9a3721aac40b9828285077f3cea00a1'),
       FromEmail: $("#FromEmail").html(),
       ToID: $("#ToID").html()
   },
   function (myData) {
       if ($("#FromEmail").html() == 'Lee.Soyemi@pwgsc-tpsgc.gc.ca' || $("#FromEmail").html() == 'wst.webmaster@pwgsc.gc.ca' || $("#FromEmail").html() == 'Michael.Mackey@pwgsc-tpsgc.gc.ca') {
           if (myData.error != undefined) { alert(myData.error); }
       }
   });
   alert('why does this need to be here?') 
   return false;
}
function RecieveDesktop(userID) {
    window.open('http://thewire.wst.pwgsc.gc.ca:5080/screenshare/screenviewer.html', '_blank', 'resizable=1,status=1,width=1280,height=1024')
}
//Get and Set Cookie Funcion
function SetCookie(name, value, seconds) {
    var cookie = name + "=" + escape(value) + "; path=/;";
    if (seconds) {
        var d = new Date();
        d.setSeconds(d.getSeconds() + seconds);
        cookie += " expires=" + d.toUTCString() + ";";
    }
    document.cookie = cookie;
}
function GetCookie(name) {
    var cookies = document.cookie.split(';');
    for (var i = 0; i < cookies.length; i++) {
        var parts = cookies[i].split('=');
        if (name == parts[0].replace(/\s/g, ''))
            return unescape(parts[1])
    }
}
//Emoticons
function AddEmoticon(emote) {
    if (emote == 'smile') {
        $('#MessageBox').val($('#MessageBox').val() + ':)');
    }
    else if (emote == 'grin') {
        $('#MessageBox').val($('#MessageBox').val() + ':D');
    }
    else if (emote == 'wink') {
        $('#MessageBox').val($('#MessageBox').val() + ';)');
    }
    else if (emote == 'cry') {
        $('#MessageBox').val($('#MessageBox').val() + ":'(");
    }
    else if (emote == 'shocked') {
        $('#MessageBox').val($('#MessageBox').val() + ':-o');
    }
    else if (emote == 'slant') {
        $('#MessageBox').val($('#MessageBox').val() + ':-/');
    }
    else if (emote == 'angry') {
        $('#MessageBox').val($('#MessageBox').val() + 'x-(');
    }
    else if (emote == 'frown') {
        $('#MessageBox').val($('#MessageBox').val() + ':(');
    }
    else if (emote == 'cool') {
        $('#MessageBox').val($('#MessageBox').val() + 'B-)');
    }
    else if (emote == 'tongue') {
        $('#MessageBox').val($('#MessageBox').val() + ':P');
    }
    else if (emote == 'straight-face') {
        $('#MessageBox').val($('#MessageBox').val() + ':-|');
    }
    $("#MessageBox").focus()
}

