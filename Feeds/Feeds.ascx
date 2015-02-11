<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Feeds.ascx.cs" Inherits="DesktopModules_WET_Feeds_Feeds" %>

<asp:LinkButton runat="server" ID="btnSettings" resourcekey="btnSettings.Text" />

<asp:Panel runat="server" ID="pnlAdmin" style='display:none;'>
    <div class="container-fluid">
        <div class="mrgn-tp-md">
            <div class="form-group">
                <p><asp:Label runat="server" resourcekey="lblFeedTypes" AssociatedControlID="txtFeed"></asp:Label><br />
                <asp:TextBox runat="server" ID="txtFeed" CssClass="form-control full-width"></asp:TextBox><br />
                <asp:Label ID="Label1" runat="server" resourcekey="lblCount"  AssociatedControlID="txtCount"></asp:Label><br />
                <asp:TextBox runat="server" ID="txtCount" Width="60px" CssClass="form-control"></asp:TextBox></p>
                <div class="text-right">
                    <asp:LinkButton runat="server" ID="btnSave" resourceKey="btnSave.Text" CssClass="btn btn-primary" OnClick="btnSave_Click" /> 
                </div>
            </div>
        </div>
    </div>
</asp:Panel>


<asp:Literal runat="server" ID="LogonText"></asp:Literal>
<asp:Panel runat="server" ID="pnlWebFeed">
    <asp:Literal runat="server" ID="ltlTop"></asp:Literal>
        <ul class="feeds-cont feed-items-<%= ModuleId %>">
            <li><asp:Literal runat="server" ID="ltlRSS"></asp:Literal></li>
        </ul>         
    </section>
    <div class="weatherimage" id="weatherimage"></div>  
</asp:Panel>



<script type="text/javascript" language="javascript">

    $(document).ready(function () {

        //Add the custom edit options to the menu
        $('.DnnModule-<%= ModuleId %> .h2-module-title').append($('#<%= btnSettings.ClientID%>'));

        setTimeout(doesRssHaveItems, 500);

        $('#<%= pnlAdmin.ClientID %>').dialog({
            autoOpen: false,
            width: 400,
            dialogClass: "dnnFormPopup",
            modal: true,
            open: function (type, data) {
                $(this).parent().appendTo("form");
            }
        });

        $('#<%= btnSettings.ClientID %>').click(function (e) {
            e.preventDefault();
            $('#<%= pnlAdmin.ClientID %>').dialog("open");
        });

        //Check if a feeds href exists, if so format it
        function doesRssHaveItems() {
            var result = "false";
            $('.feed-items-<%= ModuleId%> a').each(function () {
                if ($(this).text() != "Feed") {
                    result = "true";
                }
            });
            
            if (result == "false") {
                setTimeout(doesRssHaveItems, 500);
            }
            else {
                //Format the dates
                $('.DnnModule-<%= ModuleId %> .feeds-date').each(function () {
                    var dateFormat = $(this).text().replace('[', '').replace(']', '');
                    $(this).html(dateFormat);
                    $(this).addClass("text-muted");
                    $(this).wrap("<div></div>");
                });

                //Weather icon display
                var weathercount = 0;
                $('.feed-items-<%= ModuleId%> a[href*="weather.gc.ca"]').each(function () {
                    var condition = $(this).text().toLowerCase();
                    weathercount = weathercount + 1;

                    if (condition.indexOf('increasing cloudiness') != -1 || condition.indexOf('partly cloudy') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/Mostly_Cloudy.png' />");
                    }                    
                    else if (condition.indexOf('cloudy') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/cloudy.png' />");
                    }
                    else if (condition.indexOf('flurries') != -1 || condition.indexOf('snow') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/flurries.png' />");
                    }
                    else if (condition.indexOf('rain') != -1 || condition.indexOf('showers') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/rain.png' />");
                    }
                    else if (condition.indexOf('storm') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/storm.png' />");
                    }
                    else if (condition.indexOf('fog') != -1 || condition.indexOf('mist') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/fog.png' />");
                    }
                    else if (condition.indexOf('clear') != -1 && condition.indexOf('night') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/clear.png' />");
                    }
                    else if (condition.indexOf('sunny') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/sunny.png' />");
                    }
                    else if (condition.indexOf('clear') != -1 || condition.indexOf('sun and cloud') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/Mostly_Sunny.png' />");
                    }
                    else if (condition.indexOf('watch') != -1) {
                        $(this).parent().prepend("<img src='/DesktopModules/WET/Feeds/images/Note_Extra.png' />");
                    }
                });

                if (weathercount == 1) {
                    $('.feed-items-<%= ModuleId%>').addClass("single-weather");
                    $('.feed-items-<%= ModuleId%> img').wrap("<div></div>");
                }
            }
        }

    });
    
</script>
