using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Diagnostics;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class DesktopModules_WET_Feeds_Feeds : DotNetNuke.Entities.Modules.PortalModuleBase
{

    ModuleController objModules = new ModuleController();
    string CurrentLocale = System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0, 2);

	protected void Page_Load(object sender, EventArgs e)
    {

        if (Page.IsPostBack == false)
        {
            btnSettings.Visible = IsEditable;
            pnlAdmin.ToolTip = Localization.GetString("titleSettings.Text", this.LocalResourceFile);

            LoadFeed();
        }
	}

    protected void LoadFeed()
    {
        pnlWebFeed.Visible = false;
        //This crazy long list of weather locations is taken from the drop down lists on the weather site
        string WeatherLocations = "ON|Provincial Summary/ON-122|Alexandria/ON-1|Algonquin Park (Brent)/ON-29|Algonquin Park (Lake of Two Rivers)/ON-114|Alliston/ON-30|Apsley/ON-111|Armstrong/ON-148|Atikokan/ON-164|Attawapiskat/ON-102|Bancroft/ON-151|Barrie/ON-133|Barry's Bay/ON-3|Belleville/ON-124|Big Trout Lake/ON-21|Blind River/ON-9|Bracebridge/ON-4|Brampton/ON-86|Brantford/ON-8|Brockville/ON-31|Burk's Falls/ON-95|Burlington/ON-32|Caledon/ON-81|Cambridge/ON-91|Chapleau/ON-11|Chatham-Kent/ON-75|Cobourg/ON-33|Cochrane/ON-150|Collingwood/ON-152|Cornwall/ON-83|Deep River/ON-34|Dorion/ON-72|Dryden/ON-35|Dunchurch/ON-36|Dundalk/ON-167|Ear Falls/ON-136|Earlton/ON-170|Elliot Lake/ON-173|Fort Albany/ON-12|Fort Erie/ON-159|Fort Frances/ON-171|Fort Severn/ON-90|Gananoque/ON-160|Goderich/ON-37|Gogama/ON-144|Gore Bay/ON-38|Gravenhurst/ON-39|Greater Napanee/ON-40|Greater Sudbury/ON-153|Greenstone (Beardmore)/ON-70|Greenstone (Geraldton)/ON-97|Greenstone (Nakina)/ON-5|Guelph/ON-41|Gull Bay/ON-42|Haldimand County/ON-165|Haliburton/ON-68|Halton Hills/ON-77|Hamilton/ON-71|Hawkesbury/ON-73|Hearst/ON-78|Hornepayne/ON-88|Huntsville/ON-156|Ignace/ON-145|Kakabeka Falls/ON-43|Kaladar/ON-142|Kapuskasing/ON-44|Kawartha Lakes (Fenelon Falls)/ON-168|Kawartha Lakes (Lindsay)/ON-74|Kemptville/ON-96|Kenora/ON-146|Killarney/ON-28|Kincardine/ON-69|Kingston/ON-76|Kirkland Lake/ON-82|Kitchener-Waterloo/ON-45|Lake Superior (Provincial Park)/ON-46|Lambton Shores/ON-87|Lansdowne House/ON-23|Leamington/ON-47|Lincoln/ON-137|London/ON-108|Marathon/ON-85|Markham/ON-169|Midland/ON-48|Mine Centre/ON-24|Mississauga/ON-163|Montreal River Harbour/ON-113|Moosonee/ON-92|Morrisburg/ON-89|Mount Forest/ON-93|Muskoka/ON-49|New Tecumseth/ON-25|Newmarket/ON-125|Niagara Falls/ON-26|Nipigon/ON-50|Norfolk/ON-139|North Bay/ON-51|North Perth/ON-79|Oakville/ON-101|Ogoki/ON-140|Orangeville/ON-13|Orillia/ON-117|Oshawa/ON-118|Ottawa (Kanata - Orléans)/ON-52|Ottawa (Richmond - Metcalfe)/ON-7|Owen Sound/ON-53|Oxtongue Lake/ON-103|Parry Sound/ON-99|Peawanuck/ON-131|Pembroke/ON-112|Petawawa/ON-121|Peterborough/ON-54|Pickering/ON-120|Pickle Lake/ON-55|Pikangikum/ON-56|Port Carling/ON-80|Port Colborne/ON-19|Port Elgin/ON-149|Port Perry/ON-27|Prince Edward (Picton)/ON-57|Quinte West/ON-104|Red Lake/ON-58|Renfrew/ON-59|Richmond Hill/ON-172|Rodney/ON-141|Rondeau (Provincial Park)/ON-105|Sachigo Lake/ON-129|Sandy Lake/ON-147|Sarnia/ON-60|Saugeen Shores/ON-162|Sault Ste. Marie/ON-134|Savant Lake/ON-61|Sharbot Lake/ON-84|Shelburne/ON-161|Simcoe/ON-135|Sioux Lookout/ON-166|Sioux Narrows/ON-106|Smiths Falls/ON-62|South Bruce Peninsula/ON-107|St. Catharines/ON-98|St. Thomas/ON-115|Stirling/ON-116|Stratford/ON-18|Strathroy/ON-174|Sudbury (Greater)/ON-63|Sydenham/ON-22|Temiskaming Shores/ON-123|Terrace Bay/ON-100|Thunder Bay/ON-17|Tillsonburg/ON-127|Timmins/ON-157|Tobermory/ON-143|Toronto/ON-128|Toronto Island/ON-126|Trenton/ON-154|Upsala/ON-64|Vaughan/ON-109|Vineland/ON-16|Walkerton/ON-138|Wawa/ON-132|Webequie/ON-14|Welland/ON-65|West Nipissing/ON-66|Westport/ON-119|Whitby/ON-67|White River/ON-130|Wiarton/ON-155|Winchester/ON-94|Windsor/ON-110|Wingham/ON-15|Woodstock/ON-158|Wunnummin Lake/AGCN|Agricultural Summary/AB|Provincial Summary/AB-12|Airdrie/AB-10|Athabasca/AB-49|Banff/AB-2|Barrhead/AB-57|Beaverlodge/AB-43|Bow Island/AB-42|Bow Valley (Provincial Park)/AB-36|Breton/AB-58|Brooks/AB-52|Calgary/AB-61|Calgary (Olympic Park)/AB-18|Camrose/AB-3|Canmore/AB-64|Cardston/AB-60|Claresholm/AB-1|Cochrane/AB-23|Cold Lake/AB-59|Coronation/AB-17|Crowsnest/AB-48|Drayton Valley/AB-62|Drumheller/AB-50|Edmonton/AB-71|Edmonton (Int'l Aprt)/AB-72|Edson/AB-63|Elk Island (National Park)/AB-67|Esther/AB-28|Fort Chipewyan/AB-20|Fort McMurray/AB-65|Garden Creek/AB-45|Grande Cache/AB-31|Grande Prairie/AB-37|Hendrickson Creek/AB-24|High Level/AB-4|High River/AB-68|Highvale/AB-14|Hinton/AB-70|Jasper/AB-34|Kananaskis (Nakiska Ridgetop)/AB-32|Lac La Biche/AB-39|Lacombe/AB-30|Lethbridge/AB-15|Lloydminster/AB-51|Medicine Hat/AB-33|Mildred Lake/AB-19|Milk River/AB-38|Nordegg/AB-11|Okotoks/AB-35|Onefour/AB-25|Peace River/AB-46|Pincher Creek/AB-21|Rainbow Lake/AB-29|Red Deer/AB-40|Red Earth Creek/AB-16|Rocky Mountain House/AB-54|Slave Lake/AB-44|Stavely/AB-5|Stettler/AB-22|Stony Plain/AB-6|Strathmore/AB-47|Suffield/AB-53|Sundre/AB-7|Taber/AB-69|Three Hills/AB-27|Vauxhall/AB-26|Vegreville/AB-55|Wainwright/AB-66|Waterton Park/AB-8|Westlock/AB-9|Wetaskiwin/AB-56|Whitecourt/AB-41|Willow Creek (Provincial Park)/BC|Provincial Summary/BC-7|100 Mile House/BC-81|Abbotsford/BC-70|Agassiz/BC-67|Atlin/BC-94|Bella Bella/BC-18|Bella Coola/BC-22|Blue River/BC-43|Burns Lake/BC-55|Cache Creek/BC-19|Campbell River/BC-6|Cassiar/BC-21|Castlegar/BC-23|Chetwynd/BC-24|Chilliwack/BC-12|Clearwater/BC-91|Clinton/BC-61|Comox/BC-92|Courtenay/BC-77|Cranbrook/BC-26|Creston/BC-95|Cummins Lakes (Provincial Park)/BC-25|Dawson Creek/BC-14|Dease Lake/BC-8|Dome Creek/BC-40|Esquimalt/BC-15|Estevan Point/BC-83|Fort Nelson/BC-78|Fort St. John/BC-1|Gibsons/BC-34|Golden/BC-32|Gonzales Point/BC-9|Good Hope Lake/BC-39|Grand Forks/BC-93|Gulf Islands (Southern)/BC-36|Hope/BC-31|Hope Slide/BC-72|Invermere/BC-45|Kamloops/BC-48|Kelowna/BC-30|Kitimat/BC-11|Kootenay (National Park)/BC-87|Liard River/BC-28|Lillooet/BC-33|Lytton/BC-90|Mackenzie/BC-29|Malahat/BC-56|Masset/BC-47|McBride/BC-49|Merritt/BC-63|Muncho Lake/BC-38|Nakusp/BC-20|Nanaimo/BC-37|Nelson/BC-69|Osoyoos/BC-16|Pemberton/BC-84|Penticton/BC-35|Pitt Meadows/BC-46|Port Alberni/BC-89|Port Hardy/BC-58|Powell River/BC-79|Prince George/BC-57|Prince Rupert/BC-41|Princeton/BC-42|Puntzi Mountain/BC-64|Quesnel/BC-65|Revelstoke/BC-10|Rock Creek/BC-51|Salmon Arm/BC-88|Sandspit/BC-3|Sechelt/BC-82|Smithers/BC-52|Sparwood/BC-50|Squamish/BC-73|Stewart/BC-54|Summerland/BC-60|Tatlayoko Lake/BC-80|Terrace/BC-53|Tetsa River (Provincial Park)/BC-17|Tofino/BC-71|Trail/BC-5|Ucluelet/BC-13|Valemount/BC-74|Vancouver/BC-44|Vanderhoof/BC-27|Vernon/BC-85|Victoria/BC-59|Victoria (Hartland)/BC-66|Victoria (University of)/BC-75|Victoria Harbour/BC-86|Whistler/BC-62|White Rock/BC-76|Williams Lake/BC-68|Yoho (National Park)/MB|Provincial Summary/MB-3|Altona/MB-21|Arnes/MB-43|Bachelors Island/MB-54|Berens River/MB-33|Bissett/MB-37|Bloodvein/MB-52|Brandon/MB-53|Brochet/MB-35|Carberry/MB-65|Carman/MB-42|Churchill/MB-58|Dauphin/MB-49|Deerwood/MB-40|Delta/MB-2|Dominion City/MB-47|Emerson/MB-24|Fisher Branch/MB-60|Flin Flon/MB-64|Gillam/MB-62|Gimli/MB-63|Gods Lake/MB-9|Grand Beach/MB-57|Grand Rapids/MB-48|Gretna/MB-50|Hunters Point/MB-59|Island Lake/MB-22|Leaf Rapids/MB-18|Little Grand Rapids/MB-41|Lynn Lake/MB-15|McCreary/MB-45|Melita/MB-10|Minnedosa/MB-17|Morden/MB-11|Morris/MB-25|Norway House/MB-14|Oak Point/MB-27|Oxford House/MB-19|Pilot Mound/MB-44|Pinawa/MB-4|Pine Falls/MB-5|Poplar River/MB-29|Portage la Prairie/MB-67|Pukatawagan/MB-6|Richer/MB-32|Roblin/MB-39|Shamattawa/MB-61|Shilo/MB-28|Shoal Lake/MB-12|Snow Lake/MB-16|Souris/MB-23|Sprague/MB-13|Steinbach/MB-46|Swan River/MB-51|Tadoule Lake/MB-30|The Pas/MB-34|Thompson/MB-66|Turtle Mountain (Provincial Park)/MB-55|Victoria Beach/MB-20|Virden/MB-7|Vita/MB-31|Wasagaming/MB-8|Whiteshell/MB-26|Winkler/MB-38|Winnipeg/MB-36|Winnipeg (The Forks)/MB-56|York Factory/NB|Provincial Summary/NB-22|Bas-Caraquet/NB-28|Bathurst/NB-1|Bouctouche/NB-2|Campbellton/NB-30|Charlo/NB-3|Chipman/NB-26|Dalhousie/NB-4|Doaktown/NB-32|Edmundston/NB-29|Fredericton/NB-5|Fundy (National Park)/NB-6|Grand Falls/NB-27|Grand Manan/NB-8|Hopewell/NB-9|Kouchibouguac/NB-25|Miramichi/NB-31|Miscou Island/NB-36|Moncton/NB-10|Mount Carleton (Provincial Park)/NB-11|Oromocto/NB-34|Point Escuminac/NB-33|Point Lepreau/NB-12|Quispamsis/NB-13|Richibucto/NB-14|Rogersville/NB-15|Sackville/NB-18|Saint Andrews/NB-23|Saint John/NB-16|Saint-Quentin/NB-17|Shediac/NB-24|St. Leonard/NB-35|St. Stephen/NB-19|Sussex/NB-20|Tracadie-Sheila/NB-21|Woodstock/NL|Provincial Summary/NL-34|Badger/NL-43|Bay Roberts/NL-14|Bonavista/NL-4|Buchans/NL-31|Burgeo/NL-2|Campbellton/NL-28|Cape Race/NL-32|Cartwright/NL-47|Cartwright Junction (Trans-Labrador Hwy)/NL-17|Channel - Port aux Basques/NL-21|Churchill Falls/NL-1|Clarenville/NL-41|Corner Brook/NL-33|Daniel's Harbour/NL-39|Deer Lake/NL-44|Englee/NL-16|Gander/NL-5|Grand Bank/NL-6|Grand Falls-Windsor/NL-7|Gros Morne/NL-48|Gull Island Rapids (Trans-Labrador Hwy)/NL-23|Happy Valley-Goose Bay/NL-38|Hopedale/NL-45|L'Anse-au-Loup/NL-22|La Scie/NL-20|Labrador City/NL-9|Lewisporte/NL-25|Makkovik/NL-26|Marble Mountain/NL-29|Mary's Harbour/NL-3|Marystown/NL-10|Musgrave Harbour/NL-40|Nain/NL-42|New-Wes-Valley/NL-30|Placentia/NL-11|Port au Choix/NL-46|Rigolet/NL-13|Rocky Harbour/NL-8|St Alban's/NL-37|St. Anthony/NL-24|St. John's/NL-36|St. Lawrence/NL-27|Stephenville/NL-15|Terra Nova (National Park)/NL-35|Twillingate/NL-12|Wabush Lake/NL-19|Winterland/NL-18|Wreckhouse/NT|Provincial Summary/NT-13|Aklavik/NT-26|Colville Lake/NT-22|Deline/NT-1|Detah/NT-14|Ekati (Lac de Gras)/NT-2|Enterprise/NT-5|Fort Good Hope/NT-29|Fort Liard/NT-10|Fort McPherson/NT-27|Fort Providence/NT-3|Fort Resolution/NT-4|Fort Simpson/NT-17|Fort Smith/NT-18|Gameti/NT-8|Hay River/NT-28|Indin River/NT-30|Inuvik/NT-31|Lutselke/NT-12|Nahanni Butte/NT-21|Norman Wells/NT-16|Paulatuk/NT-19|Sachs Harbour/NT-15|Trout Lake/NT-20|Tuktoyaktuk/NT-11|Tulita/NT-7|Ulukhaktok/NT-9|Wekweeti/NT-6|Whati/NT-23|Wrigley/NT-24|Yellowknife/NS|Provincial Summary/NS-33|Amherst/NS-2|Annapolis Royal/NS-3|Antigonish/NS-37|Baccaro Point/NS-4|Baddeck/NS-36|Beaver Island/NS-5|Bridgetown/NS-6|Bridgewater/NS-27|Brier Island/NS-41|Cape George/NS-34|Caribou/NS-38|Chéticamp/NS-20|Digby/NS-7|Economy/NS-24|Fourchu Head/NS-32|Grand Étang/NS-35|Greenwood/NS-8|Guysborough/NS-19|Halifax/NS-40|Halifax (Shearwater)/NS-23|Hart Island/NS-16|Ingonish/NS-42|Kejimkujik (National Park)/NS-17|Kentville/NS-39|Liverpool/NS-21|Lunenburg/NS-22|Malay Falls/NS-1|New Glasgow/NS-18|North East Margaree/NS-43|North Mountain (Cape Breton)/NS-13|Parrsboro/NS-26|Port Hawkesbury/NS-10|Sheet Harbour/NS-11|Shelburne/NS-12|St. Peter's/NS-31|Sydney/NS-14|Tatamagouche/NS-28|Tracadie/NS-25|Truro/NS-30|Western Head/NS-15|Windsor/NS-29|Yarmouth/NU|Provincial Summary/NU-22|Alert/NU-10|Arctic Bay/NU-20|Arviat/NU-14|Baker Lake/NU-15|Cambridge Bay/NU-2|Cape Dorset/NU-17|Chesterfield/NU-18|Clyde River/NU-9|Coral Harbour/NU-19|Ennadai/NU-11|Eureka/NU-24|Gjoa Haven/NU-12|Grise Fiord/NU-4|Hall Beach/NU-23|Igloolik/NU-21|Iqaluit/NU-26|Kimmirut/NU-13|Kugaaruk/NU-16|Kugluktuk/NU-1|Nanisivik/NU-7|Pangnirtung/NU-25|Pond Inlet/NU-5|Qikiqtarjuaq/NU-28|Rankin Inlet/NU-3|Repulse Bay/NU-27|Resolute/NU-29|Sanikiluaq/NU-8|Taloyoak/NU-6|Whale Cove/PE|Provincial Summary/PE-5|Charlottetown/PE-6|East Point/PE-2|Maple Plains/PE-1|North Cape/PE-4|St. Peters Bay/PE-3|Summerside/QC|Provincial Summary/QC-36|Akulivik/QC-144|Alma/QC-168|Amos/QC-146|Amqui/QC-38|Asbestos/QC-117|Aupaluk/QC-161|Bagotville/QC-160|Baie-Comeau/QC-69|Baie-James/QC-165|Baie-Saint-Paul/QC-48|Beauceville/QC-49|Beauharnois/QC-50|Bécancour/QC-51|Bernières/QC-25|Berthierville/QC-52|Blainville/QC-163|Blanc-Sablon/QC-53|Boisbriand/QC-54|Bonaventure/QC-55|Candiac/QC-120|Cap Chat/QC-56|Carignan/QC-57|Carleton-sur-Mer/QC-58|Chambly/QC-D4|Chandler/QC-119|Charlevoix/QC-59|Chelsea/QC-152|Chevery/QC-121|Chibougamau/QC-60|Coaticook/QC-46|Contrecoeur/QC-61|Cowansville/QC-62|Delson/QC-63|Deux-Montagnes/QC-64|Dolbeau-Mistassini/QC-65|Donnacona/QC-2|Drummondville/QC-26|Escoumins/QC-140|Farnham/QC-29|Fermont/QC-27|Forestville/QC-67|Forillon/QC-95|Forillon (National Park)/QC-101|Gaspé/QC-94|Gaspésie (Parc national)/QC-126|Gatineau/QC-A1|Gouin (Réservoir)/QC-5|Granby/QC-169|Grande-Vallée/QC-104|Havre St-Pierre/QC-68|Huntingdon/QC-103|Îles-de-la-Madeleine/QC-131|Inukjuak/QC-112|Ivujivik/QC-137|Joliette/QC-70|Kamouraska/QC-118|Kangiqsualujjuaq/QC-155|Kangiqsujuaq/QC-159|Kangirsuk/QC-150|Kuujjuaq/QC-105|Kuujjuarapik/QC-156|L'Assomption/QC-77|L'Islet/QC-158|La Grande Rivière/QC-100|La Grande-Quatre/QC-11|La Malbaie/QC-71|La Prairie/QC-45|La Sarre/QC-154|La Tuque/QC-30|La Vérendrye (Réserve faunique)/QC-99|Lac Raglan/QC-73|Lac-Saint-Jean/QC-72|Lac-Mégantic/QC-14|Lachute/QC-74|Lanaudière/QC-113|Laurentides (Réserve faunique)/QC-76|Laval/QC-D3|Le Gardeur/QC-78|Lévis/QC-109|Longueuil/QC-81|Lorraine/QC-D2|Louiseville/QC-83|Magog/QC-153|Manicouagan/QC-102|Maniwaki/QC-84|Marieville/QC-85|Mascouche/QC-129|Matagami/QC-15|Matane/QC-151|Matapedia/QC-89|Mauricie/QC-90|Mingan/QC-123|Mirabel/QC-92|Mont Saint-Hilaire/QC-127|Mont-Joli/QC-47|Mont-Laurier/QC-167|Mont-Tremblant/QC-124|Montmagny/QC-147|Montréal/QC-16|Murdochville/QC-125|Natashquan/QC-111|New Carlisle/QC-110|Nicolet/QC-93|Otterburn Park/QC-17|Papineau/QC-114|Parent/QC-18|Percé/QC-96|Pincourt/QC-20|Pointe-à-la-Croix/QC-19|Pontiac/QC-97|Port-Cartier/QC-142|Port-Menier/QC-98|Prévost/QC-40|Puvirnituq/QC-106|Quaqtaq/QC-133|Québec/QC-A0|Repentigny/QC-21|Richelieu/QC-A2|Rigaud/QC-138|Rimouski/QC-108|Rivière-du-Loup/QC-134|Roberval/QC-A3|Rosemère/QC-148|Rouyn-Noranda/QC-166|Saguenay/QC-A5|Saint-Amable/QC-A6|Saint-Basile Le Grand/QC-A7|Saint-Constant/QC-B4|Saint-Eustache/QC-B5|Saint-Félicien/QC-B6|Saint-Georges/QC-22|Saint-Hyacinthe/QC-28|Saint-Jean-sur-Richelieu/QC-13|Saint-Jérôme/QC-B7|Saint-Lazare/QC-B8|Saint-Lin-Laurentides/QC-B9|Saint-Luc/QC-C0|Saint-Michel-des-Saints/QC-C1|Saint-Nicéphore/QC-C2|Saint-Rémi/QC-C3|Saint-Sauveur/QC-C4|Saint-Timothée/QC-33|Sainte-Agathe/QC-A8|Sainte-Anne-Des-Monts/QC-A9|Sainte-Anne-Des-Plaines/QC-B0|Sainte-Catherine/QC-B1|Sainte-Julie/QC-B2|Sainte-Sophie/QC-B3|Sainte-Thérèse/QC-143|Salaberry-de-Valleyfield/QC-128|Salluit/QC-115|Schefferville/QC-141|Sept-Îles/QC-132|Shawinigan/QC-136|Sherbrooke/QC-C6|Sorel-Tracy/QC-135|Sutton/QC-107|Tadoussac/QC-145|Tasiujaq/QC-139|Témiscamingue/QC-35|Temiscouata/QC-C7|Terrebonne/QC-C8|Thetford Mines/QC-C9|Tracy/QC-24|Trois-Pistoles/QC-130|Trois-Rivières/QC-122|Umiujaq/QC-149|Val-d'Or/QC-42|Val-des-Monts/QC-D0|Vallée de la Matapédia/QC-41|Vanier/QC-162|Varennes/QC-66|Vaudreuil-Dorion/QC-157|Victoriaville/QC-116|Waskaganish/SK|Provincial Summary/SK-51|Assiniboia/SK-2|Biggar/SK-48|Broadview/SK-39|Buffalo Narrows/SK-3|Canora/SK-4|Carlyle/SK-30|Collins Bay/SK-42|Coronach/SK-29|Cypress Hills (Provincial Park)/SK-45|Eastend/SK-18|Elbow/SK-53|Estevan/SK-5|Fort Qu'Appelle/SK-17|Hudson Bay/SK-6|Humboldt/SK-43|Indian Head/SK-7|Kamsack/SK-20|Key Lake/SK-21|Kindersley/SK-13|La Loche/SK-38|La Ronge/SK-35|Last Mountain Lake (Sanctuary)/SK-52|Leader/SK-56|Lloydminster/SK-14|Lucky Lake/SK-16|Maple Creek/SK-22|Meadow Lake/SK-46|Melfort/SK-8|Melville/SK-24|Moose Jaw/SK-9|Moosomin/SK-47|Nipawin/SK-34|North Battleford/SK-37|Outlook/SK-10|Oxbow/SK-55|Pelican Narrows/SK-27|Prince Albert/SK-32|Regina/SK-54|Rockglen/SK-23|Rosetown/SK-1|Rosthern/SK-40|Saskatoon/SK-26|Scott/SK-11|Shaunavon/SK-50|Southend Reindeer/SK-25|Spiritwood/SK-36|Stony Rapids/SK-41|Swift Current/SK-12|Tisdale/SK-44|Uranium City/SK-28|Val Marie/SK-15|Waskesiu Lake/SK-49|Watrous/SK-31|Weyburn/SK-19|Wynyard/SK-33|Yorkton/YT|Provincial Summary/YT-15|Beaver Creek/YT-7|Burwash Landing/YT-2|Carcross/YT-17|Carmacks/YT-6|Dawson/YT-4|Dempster (Highway)/YT-12|Faro/YT-5|Haines Junction/YT-1|Kluane Lake/YT-10|Mayo/YT-11|Old Crow/YT-3|Rancheria/YT-9|Rock River/YT-8|Ross River/YT-14|Teslin/YT-13|Watson Lake/YT-16|Whitehorse";
        
        //Get the module configurations being used
        string maxCount = "5";
        string webFeed = "No RSS";
        if (ModuleConfiguration.ModuleSettings["MaxCount" + CurrentLocale] == null)
        {
            if (ModuleConfiguration.ModuleSettings["MaxCount"] != null)
            {
                maxCount = ModuleConfiguration.ModuleSettings["MaxCount"].ToString();
                webFeed = ModuleConfiguration.ModuleSettings["WebFeed"].ToString();
            }
        }
        else
        {
            maxCount = ModuleConfiguration.ModuleSettings["MaxCount" + CurrentLocale].ToString();
            webFeed = ModuleConfiguration.ModuleSettings["WebFeed" + CurrentLocale].ToString();
        }
        webFeed = webFeed.ToLower();
        txtCount.Text = maxCount;
        txtFeed.Text = webFeed;

        //Get max count for RSS items
        string extraClass = "";
        if (webFeed.Contains("weather.gc.ca"))
        {
            extraClass = " weatheroffice";
        }
        ltlTop.Text += "<section class='wb-feeds limit-" + maxCount + extraClass + "'>";

        //Determine the Web Feed URL setup
        if (webFeed != null)
        {
            ltlRSS.Text = "<a href='" + webFeed + "'>Feed</a>";

            //Check for user location if a weatheroffice link
            if (webFeed.Contains("weather.gc.ca"))
            {
                string WeatherLink = webFeed;
                //if (string.IsNullOrEmpty(UserInfo.Profile.City) == false)
                //{
                //    if (WeatherLocations.Contains(UserInfo.Profile.City))
                //    {
                //        //Attempt to find this users location
                //        string[] WeatherArray = WeatherLocations.Split('/');
                //        foreach (string location in WeatherArray)
                //        {
                //            if (string.IsNullOrEmpty(location) == false)
                //            {
                //                if (location.EndsWith(UserInfo.Profile.City))
                //                {
                //                    ltlTitle.Text = "<h3>" + location.Substring(location.IndexOf("|") + 1) + "</h3>";
                //                    WeatherLink = "http://weather.gc.ca/rss/city/";
                //                    WeatherLink = WeatherLink + location.Substring(0, location.IndexOf("|")).ToLower();
                //                    WeatherLink = WeatherLink + "_" + CurrentLocale.Substring(0,1) + ".xml";
                //                    break; // TODO: might not be correct. Was : Exit For
                //                }
                //            }
                //        }

                //    }

                //}

                ltlRSS.Text = "<a href='" + WeatherLink + "' rel='external'>Feed</a>";

                //Hide the date if weather link
                ltlTop.Text = "<style type='text/css'>.DnnModule-" + ModuleId + " .feeds-date {display:none;}</style>" + ltlTop.Text;
            }
            pnlWebFeed.Visible = true;
        }

    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        if (string.IsNullOrEmpty(txtFeed.Text))
            txtFeed.Text = "No RSS";
        if (string.IsNullOrEmpty(txtCount.Text))
            txtCount.Text = "5";

        if (ModuleConfiguration.ModuleSettings["MaxCount"] == null)
        {
            objModules.UpdateModuleSetting(ModuleId, "MaxCount", txtCount.Text);
            objModules.UpdateModuleSetting(ModuleId, "WebFeed", txtFeed.Text.ToLower());
        }

        objModules.UpdateModuleSetting(ModuleId, "MaxCount" + CurrentLocale, txtCount.Text);
        objModules.UpdateModuleSetting(ModuleId, "WebFeed" + CurrentLocale, txtFeed.Text.ToLower());
        
        Response.Redirect(PortalSettings.ActiveTab.FullUrl.Replace(System.Globalization.CultureInfo.CurrentUICulture.Name.ToLower() + "/", ""));
    }
}



