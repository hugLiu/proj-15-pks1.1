
function DecorationSetting()
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.async = false;
	xmlDoc.loadXML( decorationXML );
	
	if ( xmlDoc )
	{
		var root = xmlDoc.getElementsByTagName( "document" )[0];
		
		if ( root.childNodes.length > 0 )
		{
			for ( var i = 0; i < root.childNodes.length; i++ )
			{
				//在有海水的情况下，由海水插件来控制某些挂件的显隐
				var decorationEle = root.childNodes[i];
				var decorationName = decorationEle.getAttribute( "name" );
				var decorationStatus = decorationEle.getAttribute( "status" );
				var decorationCmd = "";
				if ( decorationName == "StatusBar" )
				{
					decorationCmd = "<命令 命令名='挂件显隐' 类型='HINTBAR' 显隐='" + (decorationStatus == "1" ? "ON" : "OFF") + "' />";
				}
				else if( decorationName == "SkyDome" )
				{
					decorationCmd = "<命令 命令名='地球装饰' 类型='SKYDOME' 显隐='" + (decorationStatus == "1" ? "ON" : "OFF") + "' />";
				}
				else if( decorationName == "Atmosphere" )
				{
					decorationCmd = "<命令 命令名='地球装饰' 类型='AREOSPHERE' 显隐='" + (decorationStatus == "1" ? "ON" : "OFF") + "' />";
				}
				else if( decorationName == "SunShine" )
				{
					decorationCmd = "<命令 命令名='地球装饰' 类型='SUNSHINE' 显隐='" + (decorationStatus == "1" ? "ON" : "OFF") + "' />";
				}
				else if( decorationName == "Cross" )
				{
					decorationCmd = "<命令 命令名='挂件显隐' 类型='FOCUSCROSS' 显隐='" + (decorationStatus == "1" ? "ON" : "OFF") + "' />";
				}
				
				
				earthAX.Connector( decorationCmd );
				
			}
		}
	}
}

function ToolbarSetting()
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.async = false;
	xmlDoc.loadXML( toolbarXML );
	
	if ( xmlDoc )
	{
		var root = xmlDoc.getElementsByTagName( "document" )[0];
		
		if ( root.childNodes.length > 0 )
		{
			var toolbarCmd = "<命令 命令名='添加浮动工具栏项'>" +
				"<浮动工具栏>" +
				"<属性 initLBPosX= '0.f' initLBPosY= '0.f' singleWidth='48' singleHeight='48' distanceRatio='0.3' tipOffsetX='0.0' tipOffsetY='0.0' tipColorR='255.0' tipColorG='255.0' tipColorB='255.0' tipBkColorR='0.0' tipBkColorG='0.0' tipBkColorB='0.0'tipFontName='SIMHEI.TTF' tipFontSize='11.5'  tipAlwaysShow='0'>" +
				"<距离透明渐变 processDis='100.0' minTrans='0.4' maxTrans='1.0'/>" +
				"<接触变形 direction='down' endureTime='0.3' maxScale='1.3'/>" +
				"<自动调整位置 margin='10.0' hMargin='10.0' align='mid' verAlign='top'/>" +
				"</属性>";
			
			var itemStr = "<元素>";
			var multiStateItem = "<元素显示配置 type='CustomMultiStateItemShow'>";
			var singleStateItem = "<元素显示配置 type='SingleStateItemShow' >";
			var dualStateItem = "<元素显示配置 type='CustomDualStateItemMarkShow' heightoffset='-2.f' markwidth='11.f' markheight='9.f' >";
			for ( var i = 0; i < root.childNodes.length; i++ )
			{
				var toolbarEle = root.childNodes[i];
				var toolbarName = toolbarEle.getAttribute( "name" );
				var toolbarIdentify = toolbarEle.getAttribute( "identify" );
				var toolbarMark = toolbarEle.getAttribute( "mark" );
				
				itemStr += "<Item identify='" + toolbarIdentify + "' />";
				
				if ( toolbarMark == "Y" )
				{
					singleStateItem += "<Item identify='" + toolbarIdentify + "' pic='OsgDock/" + toolbarIdentify + ".png' tip='" + toolbarName + "' />";
					dualStateItem += "<Item identify='" + toolbarIdentify + "' />";
				}
				else
				{
					multiStateItem += "<Item identify='" + toolbarIdentify + "' >" +
							"<State value='0' pic='OsgDock/" + toolbarIdentify + ".png' tip='" + toolbarName + "' />" +
							"<State value='1' pic='OsgDock/" + toolbarIdentify + ".png' tip='" + toolbarName + "' />" +
						"</Item>";
				}
				
			}
			
			itemStr += "</元素>";
			multiStateItem += "</元素显示配置>";
			singleStateItem += "</元素显示配置>";
			dualStateItem += "</元素显示配置>";
			
			toolbarCmd += itemStr + multiStateItem + singleStateItem + dualStateItem;
			toolbarCmd += "</浮动工具栏>";
			toolbarCmd += "</命令>";
			
			earthAX.Connector( toolbarCmd );
		}
	}
}

function LoadPlugin( pluginXML )
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.async = false;
	xmlDoc.loadXML( pluginXML );
	
	if ( xmlDoc )
	{
		var root = xmlDoc.getElementsByTagName( "document" )[0];
		
		if ( root.childNodes.length > 0 )
		{
			for ( var i = 0; i < root.childNodes.length; i++ )
			{
				var pluginEle = root.childNodes[i];
				var pluginName = pluginEle.getAttribute( "name" );
				earthAX.LoadPlugin( pluginName );
			}
		}
		
	}
}

function DefaultSetting()
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.async = false;
	xmlDoc.loadXML( defaultXML );
	
	if( xmlDoc )
	{
		var root = xmlDoc.getElementsByTagName( "CameraPos" )[0];
		if ( root )
		{
			var defaultCMD = "<命令 命令名='设置相机位置'  经度='" + root.getAttribute( "lon" ) + 
							"' 纬度='" + root.getAttribute( "lat" ) + "' 海拔='" + root.getAttribute( "alt" ) +
							"' 偏航角='" + root.getAttribute( "yaw" ) + "' 俯仰角='" + root.getAttribute( "pitch" ) +
							"' 滚转角='" + root.getAttribute( "roll" ) + "' 定位方式='普通'></命令>";
			earthAX.Connector( defaultCMD );
		}
	}
}

function OceanVisibleSetting()
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	if ( oceanVisible == "N" )
	{
		earthAX.Connector( "<命令 命令名='海洋显隐' 状态='OFF' />" );
	}
}

function OceanWindSetting()
{
	//return;
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.async = false;
	xmlDoc.loadXML( oceanWindXML );
	
	if( xmlDoc )
	{
		var root = xmlDoc.getElementsByTagName( "OceanWind" )[0];
		if ( root )
		{
			earthAX.Connector( "<命令 命令名='海洋风速风向' 状态='清除' 风速='30.0' 风向='0.0' 风区长度='10000000.0' />" )
			
			var oceanWindCMD = "<命令 命令名='海洋风速风向' 状态='添加' 风速='" + root.getAttribute( "speed") + "' 风向='" + root.getAttribute( "dir" ) + "' 风区长度='10000000.0' />";

			earthAX.Connector( oceanWindCMD );
		}
	}
}

function VecProgressVisibleSetting()
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	if ( vecProgressVisible == "Y" )
	{
		earthAX.Connector( "<命令 命令名='矢量加载进度显隐' Show='1'  />" );
	}
}

