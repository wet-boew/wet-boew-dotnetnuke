
function IsElementContains(p,c)
{
	for(c=c.parentNode;c;c=c.parentNode)if(c==p)return true;
}
function FindHoverClassElement(e)
{
	while(e)
	{
		if(e.getAttribute("hoverclass"))
			return e;
		if(e.getAttribute("onhoverenter"))
			return e;
		if(e.getAttribute("onhoverleave"))
			return e;
		e=e.parentNode;
		if(e.nodeType!=1)
			return null;
	}
}

document.onmouseover=function(event)
{
	event=window.event||event;
	if(event.srcElement)
	{
		var e=FindHoverClassElement(event.srcElement);
		if(e==null||e.contains(event.fromElement))return;
		if(e.getAttribute("hoverclass"))
		{
			if(!e.getAttribute("savedclass"))
				e.setAttribute("savedclass",e.className||"noclass");
			e.className=e.getAttribute("hoverclass")+" "+e.getAttribute("savedclass")
		}
		if(e.getAttribute("onhoverenter"))
		{
			var func=new Function("event",e.getAttribute("onhoverenter"));
			func.apply(e,[event]);
		}
	}
	if(event.target)
	{
		var e=FindHoverClassElement(event.target);
		if(e==null||(event.relativeTarget!=null&&IsElementContains(e,event.relativeTarget)))return;
		if(e.getAttribute("hoverclass"))
		{
			if(!e.getAttribute("savedclass"))
				e.setAttribute("savedclass",e.className||"noclass");
			e.className=e.getAttribute("hoverclass")+" "+e.getAttribute("savedclass")
		}
		if(e.getAttribute("onhoverenter"))
		{
			var func=new Function("event",e.getAttribute("onhoverenter"));
			func.apply(e,[event]);
		}
	}
}
document.onmouseout=function(event)
{
	event=window.event||event;
	if(event.srcElement)
	{
		var e=FindHoverClassElement(event.srcElement);
		if(e==null||e.contains(event.toElement))return;
		if(e.getAttribute("savedclass"))
			e.className=e.getAttribute("savedclass")
		if(e.getAttribute("onhoverleave"))
		{
			var func=new Function("event",e.getAttribute("onhoverleave"));
			func.apply(e,[event]);
		}
	}
	if(event.target)
	{
		var e=FindHoverClassElement(event.target);
		if(e==null||(event.relativeTarget!=null&&IsElementContains(e,event.relativeTarget)))return;
		if(e.getAttribute("savedclass"))
			e.className=e.getAttribute("savedclass")
		if(e.getAttribute("onhoverleave"))
		{
			var func=new Function("event",e.getAttribute("onhoverleave"));
			func.apply(e,[event]);
		}
	}
}

if(window.opera)
{
	document.onmousedown=function(event)
	{
		if(!event.shiftKey)return;
		for(var c=event.target;c!=null;c=c.parentNode)
		{
			if(c.oncontextmenu)
			{
				c.oncontextmenu(event);
				if(event.cancelBubble)break;
			}
			else if(c.getAttribute("oncontextmenu"))
			{
				c.oncontextmenu=new Function("event",c.getAttribute("oncontextmenu"));
				c.oncontextmenu(event);
				if(event.cancelBubble)break;
			}
		}
		event.preventDefault();
		event.returnValue=false;
	}
}


function CodeEncode(str)
{
	if(str==null)return "";
	var res='';
	for(var i=0;i<str.length;i++)
	{
		var c=str.charCodeAt(i).toString(16);
		while(c.length<4)c='0'+c;
		res+='\\u'+c;
	}
	return res;
}

function PropEncode(str)
{
	if(!str)return "";
	str=str.split("^").join("^0");
	str=str.split(";").join("^1");
	str=str.split(":").join("^2");
	str=str.split("|").join("^3");
	return str;
}
function PropDecode(str)
{
	if(!str)return "";
	str=str.split("^3").join("|");
	str=str.split("^2").join(":");
	str=str.split("^1").join(";");
	str=str.split("^0").join("^");
	return str;
}
function PropObjToStr(obj)
{
	var sb=[]
	for(var key in obj)
	{
		if(!obj.hasOwnProperty(key))continue;
		var val=obj[key];
		if(val==null)continue;
		val=String(val);
		
		key=PropEncode(key);
		val=PropEncode(val);
		sb.push(key+":"+val);
	}
	if(sb.length==0)return null;
	return sb.join("|");
}
function PropStrToObj(str)
{
	if(str==null)return null;
	if(str.length==0)return null;
	var obj={};
	var arr=str.split('|');
	for(var i=0;i<arr.length;i++)
	{
		var pairs=arr[i].split(':');
		var key=PropDecode(pairs[0]);
		var val=PropDecode(pairs[1]);
		obj[key]=val;
	}
	return obj;
}


function JoinToMsg(msgid,nvc,args)
{
	var props="";
	if(nvc!=null)props=PropObjToStr(nvc);
	if(props==null)props="";
	
	if(args==null||args.length==0)
	{
		if(props==null)
			return msgid;
		return msgid+";"+props;
	}
	
	var sb=[];
	sb.push(msgid);
	sb.push(props);
	for(var i=0;i<args.length;i++)
	{
		var arg=args[i];
		if(arg==null)
		{
			sb.push("");
			continue;
		}
		arg=String(arg);
		arg=arg.split("^").join("^0");
		arg=arg.split(";").join("^1");
		sb.push(arg);
	}
	return sb.join(";");
}
function SplitMsg(msg)
{
	var res={};
	res.nvc={};
	res.args=[];
	
	var pos=msg.indexOf(';');
	if(pos==-1)
	{
		res.msg=msg;
		return res;
	}
	
	res.msg=msg.substring(0,pos);
	
	msg=msg.substring(pos+1);
	pos=msg.indexOf(';');
	if(pos==-1)
	{
		res.nvc=PropStrToObj(msg);
		return res;
	}
	
	res.nvc=PropStrToObj(msg.substring(0,pos));
	
	var parts=msg.substring(pos+1).split(';');
	for(var i=0;i<parts.length;i++)
	{
		var part=parts[i];
		part=part.split("^1").join(";");
		part=part.split("^0").join("^");
		parts[i]=part;
	}
	res.args=parts;
	return res;
}
		



/****************************************************************\
	Cookie Functions
\****************************************************************/


function SetCookie(name,value,seconds)
{
	var cookie=name+"="+escape(value)+"; path=/;";
	if(seconds)
	{
		var d=new Date();
		d.setSeconds(d.getSeconds()+seconds);
		cookie+=" expires="+d.toUTCString()+";";
	}
	document.cookie=cookie;
}
function GetCookie(name)
{
	var cookies=document.cookie.split(';');
	for(var i=0;i<cookies.length;i++)
	{
		var parts=cookies[i].split('=');
		if(name==parts[0].replace(/\s/g,''))
			return unescape(parts[1])
	}
	//return undefined..
}
function GetCookieDictionary()
{
	var dict={};
	var cookies=document.cookie.split(';');
	for(var i=0;i<cookies.length;i++)
	{
		var parts=cookies[i].split('=');
		dict[parts[0].replace(/\s/g,'')]=unescape(parts[1]);
	}
	return dict;
}
function GetCookieArray()
{
	var arr=[];
	var cookies=document.cookie.split(';');
	for(var i=0;i<cookies.length;i++)
	{
		var parts=cookies[i].split('=');
		var cookie={name:parts[0].replace(/\s/g,''),value:unescape(parts[1])};
		arr[arr.length]=cookie;
	}
	return arr;
}



/****************************************************************\
	Position Functions
\****************************************************************/
//get the position of a element ( by the scroll offset )
function GetScrollPostion(e)
{
	var b=window.document.body;
	var p=b;
	if(window.document.compatMode=="CSS1Compat")
	{
		p=window.document.documentElement;
	}
	
	if(e==b)return {left:0,top:0};

	//if(e.getBoundingClientRect)
	//{
	//	var b=e.getBoundingClientRect();
	//	return {left:p.scrollLeft+b.left,top:p.scrollTop+b.top};
	//}

	var l=0;
	var t=0;
	var box;
	var offset;

	l = e.offsetLeft;
	t = e.offsetTop;
	offset = e.offsetParent;
	if (offset != e) {
		while (offset) {
			l += offset.offsetLeft;
			t += offset.offsetTop;
			offset = offset.offsetParent;
		}
	}
	if (window.opera) {
		offset = e.offsetParent;
		while (offset && offset.tagName.toUpperCase() != "BODY" && offset.tagName.toUpperCase() != "HTML") {
			l -= offset.scrollLeft;
			t -= offset.scrollTop;
			offset = offset.offsetParent;
		}
	}
	else {
		offset = e.parentNode;
		while (offset && offset.tagName.toUpperCase() != "BODY" && offset.tagName.toUpperCase() != "HTML") {
			l -= offset.scrollLeft;
			t -= offset.scrollTop;
			offset = offset.parentNode;
		}
	}
	return {left:l,top:t}
}
//get the position of a element ( by the client offset )
function GetClientPosition(e)
{
	var b=window.document.body;
	var p=b;
	if(window.document.compatMode=="CSS1Compat")
	{
		p=window.document.documentElement;
	}
	
	if(e==b)return {left:-p.scrollLeft,top:-p.scrollTop};
	
	if(e.getBoundingClientRect)
	{
		var b=e.getBoundingClientRect();
		return {left:b.left-p.clientLeft,top:b.top-p.clientTop};
	}
	
	var l=0;
	var t=0;
	for(var e1=e;e1!=null&&e1!=b;e1=e1.offsetParent)
	{
		l+=e1.offsetLeft;
		t+=e1.offsetTop;
	}
	return {left:l-p.scrollLeft,top:t-p.scrollTop}
}
//get absolute or relative parent
function GetStandParent(e)
{
	if(e.currentStyle)
	{
		for(var pe=e.parentElement;pe!=null;pe=pe.parentElement)
		{
			var sp=pe.currentStyle.position;
			if(sp=="absolute"||sp=="relative")
				return pe;
		}
	}
	else
	{
		var view=e.ownerDocument.defaultView;
		for(var pe=e.parentNode;pe!=null&&pe.nodeType==1;pe=pe.parentNode)
		{
			var sp=view.getComputedStyle(pe, "").getPropertyValue("position")
			if(sp=="absolute"||sp=="relative")
				return pe;
		}
	}
	return (e.ownerDocument||e.document).body;
}
//calc the position of floate that relative to e
function CalcPosition(floate,e)
{
	var epos=GetScrollPostion(e);
	var spos=GetScrollPostion(GetStandParent(floate));
	var s=GetStandParent(floate);
	var pos={left:epos.left-spos.left-(s.clientLeft||0),top:epos.top-spos.top-(s.clientTop||0)};
	return pos
}

//get the best position to put the floate
function AdjustMirror(floate,e,pos)
{
	//c:Client,f:floate,e:e,p:floate"s StandParent,m:Mirror

	//get the size of window
	var cw=window.document.body.clientWidth;
	var ch=window.document.body.clientHeight;
	if(window.document.compatMode=="CSS1Compat")
	{
		cw=window.document.documentElement.clientWidth;
		ch=window.document.documentElement.clientHeight;
	}
	
	//get the size of float element
 	var fw=floate.offsetWidth;
	var fh=floate.offsetHeight;
	
	var pcpos=GetClientPosition(GetStandParent(floate));
	
	//get the center of float element
	var fmpos={left:pcpos.left+pos.left+fw/2,top:pcpos.top+pos.top+fh/2};//

	var empos={left:pcpos.left+pos.left,top:pcpos.top+pos.top};

	var isbody=false;
	if(e!=null)
	{
		if(e.nodeName=="BODY")
		{
			isbody=true;
		}
		
		var ecpos=GetClientPosition(e);
		//get the center of the relative element
		empos={left:ecpos.left+e.offsetWidth/2,top:ecpos.top+e.offsetHeight/2};//
	}
	
	var allowjump=!isbody;
	var allowmove=true;
 
	//left<-->right

	if(fmpos.left-fw/2<0)
	{
		if((empos.left*2-fmpos.left)+fw/2<=cw)
		{
			if(allowjump)fmpos.left=empos.left*2-fmpos.left;
		}
		else if(allowmove)
		{
			fmpos.left=fw/2+4;
		}
	}
	else if(fmpos.left+fw/2>cw)
	{
		if((empos.left*2-fmpos.left)-fw/2>=0)
		{
			if(allowjump)fmpos.left=empos.left*2-fmpos.left;
		}
		else if(allowmove)
		{
			fmpos.left=cw-fw/2-4;
		}
	}
	

	//top<-->bottom

	if(fmpos.top-fh/2<0)
	{
		if((empos.top*2-fmpos.top)+fh/2<=ch)
		{
			if(allowjump)fmpos.top=empos.top*2-fmpos.top;
		}
		else if(allowmove)
		{
			fmpos.top=fh/2+4;
		}
	}
	else if(fmpos.top+fh/2>ch)
	{
		if((empos.top*2-fmpos.top)-fh/2>=0)
		{
			if(allowjump)fmpos.top=empos.top*2-fmpos.top;
		}
		else if(allowmove)
		{
			fmpos.top=ch-fh/2-4;
		}
	}
 
	pos.left=fmpos.left-pcpos.left-fw/2;
	pos.top=fmpos.top-pcpos.top-fh/2;
}



/****************************************************************\
	Prototype
\****************************************************************/

function Element_GetText()
{
	var r = this.ownerDocument.createRange();
	r.selectNodeContents(this);
	return r.toString();
}

if( typeof(Element)!="undefined" && Element)
{
	if(Element.prototype&&Element.prototype.__defineGetter__)
	{
		Element.prototype.__defineGetter__("text",Element_GetText);
		Element.prototype.__defineGetter__("innerText",Element_GetText);
	}
	else
	{
	}
}

var __active_element;
function __active_element_focus()
{
	window.__isblur=false;
}
function __active_element_blur()
{
	window.__isblur=true;
}
if(document.attachEvent)
{
	document.attachEvent("onactivate",function(){
		__active_element=document.activeElement;
		if(!__active_element)return;
		__active_element.attachEvent("onfocus",__active_element_focus);
		__active_element.attachEvent("onblur",__active_element_blur);
	});
	document.attachEvent("ondeactivate",function(){
		if(!__active_element)return;
		__active_element.detachEvent("onfocus",__active_element_focus);
		__active_element.detachEvent("onblur",__active_element_blur);
		__active_element=null;
	});
}
else
{
	document.addEventListener("focus",__active_element_focus,true);
	document.addEventListener("blur",__active_element_blur,true);
}
function GetWindowIsFocus()
{
	return window.__isblur?false:true;
}
function FocusWindow()
{
	var e=__active_element;
	//TODO: FireFox doesn't work?
	window.focus();
	if(e && e.focus )
	{
		try
		{
			e.focus();
		}
		catch(x)
		{
		}
	}
}
