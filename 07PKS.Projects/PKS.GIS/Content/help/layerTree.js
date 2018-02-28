//var str='<document><Node Name="测试"><Node NO="14" Type="Distriction" Name="行政区划线" Rect="0" Batch="1"><Node ID="1610300" Name="省界" ThreeDType="1" /><Node ID="1640100" Name="中国国界与省界" ThreeDType="" /><Node ID="1610500" Name="县界" ThreeDType="" /><Node ID="1640200" Name="县级行政界线" ThreeDType="" /><Node ID="1610400" Name="地区、自治州界" ThreeDType="" /><Node ID="1640300" Name="地级行政界线" ThreeDType="" /></Node></Node><Node Name="测试1"><Node NO="14" Type="Distriction" Name="行政区划线1" Rect="0" Batch="1"><Node ID="1610300" Name="省界1" ThreeDType="1" /><Node ID="1640100" Name="中国国界与省界1" ThreeDType="" /><Node ID="1610500" Name="县界1" ThreeDType="" /><Node ID="1640200" Name="县级行政界线1" ThreeDType="" /><Node ID="1610400" Name="地区、自治州界1" ThreeDType="" /><Node ID="1640300" Name="地级行政界线1" ThreeDType="" /></Node></Node></document>';
//var str='<document><Node Name="测试"><Node NO="14" Type="Distriction" Name="行政区划线" Rect="0" Batch="1"></Node></Node></document>';
var typhoonDataStr = "<document> " +
						"<Point index='0' time='7月3日9时' lon='145' lat='8.8' press='1000' windSpeed='18' windLevel='8' level='热带风暴' speed='15' direction='西北西' radius7='300' radius10='' />" +
						"<Point index='1' time='7月3日14时' lon='144.8' lat='9.2' press='1000' windSpeed='18' windLevel='8' level='热带风暴' speed='15' direction='西北西' radius7='300' radius10='' />" +
						"<Point index='2' time='7月3日20时' lon='144.3' lat='9.9' press='995' windSpeed='20' windLevel='8' level='热带风暴' speed='20' direction='北西' radius7='260' radius10='' />" +
						"<Point index='3' time='7月4日2时' lon='143.5' lat='10.6' press='995' windSpeed='20' windLevel='8' level='热带风暴' speed='30' direction='北西' radius7='260' radius10='' />" +
						"<Point index='4' time='7月4日8时' lon='142.2' lat='11.8' press='995' windSpeed='20' windLevel='8' level='热带风暴' speed='28' direction='北西' radius7='300' radius10='' />" +
						"<Point index='5' time='7月4日14时' lon='141.3' lat='12.8' press='995' windSpeed='20' windLevel='8' level='热带风暴' speed='30' direction='北西' radius7='300' radius10='' />" +
						"<Point index='6' time='7月4日20时' lon='139.5' lat='13.5' press='990' windSpeed='23' windLevel='9' level='热带风暴' speed='20' direction='北西' radius7='300' radius10='' />" +
						"<Point index='7' time='7月5日2时' lon='138.4' lat='14.5' press='982' windSpeed='28' windLevel='10' level='强热带风暴' speed='28' direction='西北西' radius7='300' radius10='80' />" +
						"<Point index='8' time='7月5日8时' lon='136.5' lat='15.2' press='975' windSpeed='33' windLevel='12' level='台风' speed='30' direction='北西' radius7='300' radius10='80' />" +
						"<Point index='9' time='7月5日14时' lon='135.1' lat='16.2' press='960' windSpeed='40' windLevel='13' level='台风' speed='30' direction='北西' radius7='350' radius10='110' />" +
						"<Point index='10' time='7月5日20时' lon='133.6' lat='17.1' press='935' windSpeed='53' windLevel='16' level='超强台风' speed='30' direction='北西' radius7='350' radius10='110' />" +
						"<Point index='11' time='7月6日2时' lon='132' lat='17.9' press='920' windSpeed='60' windLevel='17' level='超强台风' speed='30' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='12' time='7月6日5时' lon='131' lat='18.3' press='910' windSpeed='65' windLevel='18' level='超强台风' speed='30' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='13' time='7月6日8时' lon='130.2' lat='18.7' press='910' windSpeed='65' windLevel='18' level='超强台风' speed='30' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='14' time='7月6日11时' lon='129.3' lat='19' press='905' windSpeed='68' windLevel='18' level='超强台风' speed='30' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='15' time='7月6日14时' lon='128.6' lat='19.4' press='905' windSpeed='68' windLevel='18' level='超强台风' speed='30' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='16' time='7月6日17时' lon='127.7' lat='19.8' press='905' windSpeed='68' windLevel='18' level='超强台风' speed='30' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='17' time='7月6日20时' lon='127' lat='20.1' press='905' windSpeed='68' windLevel='18' level='超强台风' speed='27' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='18' time='7月6日23时' lon='126.3' lat='20.4' press='905' windSpeed='68' windLevel='18' level='超强台风' speed='26' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='19' time='7月7日2时' lon='125.6' lat='20.7' press='905' windSpeed='68' windLevel='18' level='超强台风' speed='20' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='20' time='7月7日5时' lon='125' lat='21.1' press='915' windSpeed='60' windLevel='17' level='超强台风' speed='20' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='21' time='7月7日6时' lon='124.8' lat='21.1' press='915' windSpeed='60' windLevel='17' level='超强台风' speed='20' direction='西北西' radius7='350' radius10='150' />" +
						"<Point index='22' time='7月7日7时' lon='124.6' lat='21.3' press='915' windSpeed='60' windLevel='17' level='超强台风' speed='20' direction='西北西' radius7='350' radius10='150' />" +
					"</document>";

var treeNodeData=null;
var checkNodeId="";
var isCheck=null;
var commandList = new Array();
var selectNode=null;

var menuIsShow = false;	//1级菜单是否正在显示
var menuName = "";	//1级菜单第一项的名字
var mouseMoveX = 0;	//鼠标滑动位置
var mouseMoveY = 0;
var curHighLayerID = 0;	//当前高亮的图层名字
var curFeatureID = 0;		//当前高亮的图元名字
var menu1Length = 108; 	//1级菜单的长度
var menu2Length = 108;	//2级菜单的长度
var baseUrlPatch;	//基本信息的Url补充
var moveLight = false; //移动高亮鼠标开关
var moveLightSwitch = true;	//移动高亮程序开关（在执行某些功能时先将滑动高亮关闭）
var firstFrame = false;	//是否第一帧已经执行
var typhoonData = new Array();
var labelCreate = false;
var currentMenuNum = 0;
var decorationXML;
var toolbarXML;
var defaultXML;
var oceanVisible;
var oceanWindXML;
var vecProgressVisible;
var urlLoc;			//html文件所在路径

//角色范围
var Leftx = 0;
var Topy = 0;
var Rightx = 0;
var Bottomy = 0;
var RangePoints = "";

function strByteLength( str )
{
	var realLength = 0;
	for ( var i = 0; i < str.length; i++ )
	{
		if ( str.charCodeAt(i) > 255 )
		{
			realLength += 2;
		}
		else
		{
			realLength++;
		}
	}

	return realLength;
}

function mouseMoveLayerTree()
{
	mouseMoveX = -100000;
	mouseMoveY = -100000;
}

function createTree(){
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var userID = getParamter( "UserID" );
	
	var decodeDll = new ActiveXObject( "DesDCom.DesObj.1" );
	var decodeTM = decodeDll.Decode( userID );
	var pos = decodeTM.indexOf( "$" );
	var userName = decodeTM.substring( 0, pos );
	
			
	var result = earthAX.LogOn( userName, userID );
	if ( result != "登录成功" )
	{
		alert( "登录失败" );
		return;
	}

	//var userName=getParamter("userName");

	

	var date1 = new Date();

	//只展示一个系统
	//str = earthAX.PBGISNInfo();
	var str = "<document><Node Name='总院GIS系统' /></document>";

	var date2 = new Date();
	var date3 = date2.getTime() - date1.getTime();
	//alert( date3 );

	urlLoc = "/";

	date2 = new Date();
	//获取用户范围
	var userRange = earthAX.UserRange( userName );
	if ( userRange != "" )
	{
		var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
		xmlDoc.async = false;
		xmlDoc.loadXML( userRange );

		if ( xmlDoc )
		{
			var root = xmlDoc.getElementsByTagName( "UserRange" )[0];
			Leftx = root.getAttribute( "Leftx" );
			Topy = root.getAttribute( "Topy" );
			Rightx = root.getAttribute( "Rightx" );
			Bottomy = root.getAttribute( "Bottomy" );

			if ( root.childNodes.length > 0 )
			{
				for ( var i = 0; i < root.childNodes.length; i++ )
				{
					RangePoints += "<Point lon='" + root.childNodes[i].getAttribute( "lon" ) + "' lat='" + root.childNodes[i].getAttribute( "lat" ) + "' />\n";
				}
			}
		}

	}

	decorationXML = earthAX.GetRoleDecoration( userName );
	toolbarXML = earthAX.GetRoleToolbar( userName );

	var pluginXML = earthAX.GetRolePlugin( userName );
	LoadPlugin( pluginXML );

	defaultXML = earthAX.GetRoleDefault( userName );

	oceanVisible = earthAX.GetRoleOcean( userName );
	oceanWindXML = earthAX.GetRoleOceanWind( userName );

	var modelXML = earthAX.GetRoleModel( userName );

	vecProgressVisible = earthAX.GetRoleVecProgress( userName );

	BatchLayer( userName );


	var data=xmlToJson(str).document.Node;
	treeNodeData=data;
	var date4 = new Date;
	var date5 = date4.getTime() - date2.getTime();
	//alert( date5 );

	date4 = new Date;
	var treeJson = createTreeJson(data);
	console.log(treeJson);
	$("#layerTree").tree({
		data:treeJson,
		checkbox:true,
		lines:true,
		onClick:function(node){
			var check=node.checked;
			if(!check){
				$(this).tree('check',node.target);
			}else{
				$(this).tree('uncheck',node.target);
			}
		},
		onCheck:function(node,onCheck){
			//获取孩子节点
//			if(onCheck && selectNode==null){
//				$(this).tree("select",node.target);
//				selectNode=node;
//			}else if(selectNode!=null){
//				var parentnNode=$(this).tree("getParent",node.target);
//				if(null!=parentnNode && parentnNode.id!=selectNode.id){
//					selectNode=node;
//					$(this).tree("select",node.target);
//					if($("#"+node.id).hasClass("tree-node-last") && $(this).tree("getChildren",node.target)==null){
//						selectNode=null;
//					}
//				}
//			}
			var childNodes = $("#layerTree").tree("getChildren",node.target);
			if(node.id==checkNodeId && isCheck==onCheck){
				return;
			}
			var isLeaf=$(this).tree("isLeaf",node.target);
			var childrenNodes=null;
			if(!isLeaf){
				childrenNodes=$(this).tree("getChildren",node.target);
			}
//			//获取孩子节点
//			var childNodes = $("#layerTree").tree("getChildren",node.target);
			isCheck=onCheck;
			if(onCheck){
				if(childrenNodes==null){
					checkNodeId=node.id;
					//node.iconCls= 'icon-tree-classify-node-leaf-open';
					//$(this).tree('update', node);
					$("#layerTree").tree('update', {
						target:node.target,
						iconCls:'icon-tree-classify-node-leaf-open'
					});
					var command = "<命令 命令名='设置图层显隐' 图层ID='" + node.id + "' 显隐='1' />";
					var earthAX = document.getElementById( "superEarthOBJ" );
					earthAX.Connector( command );

				}else{
					changeNodeIconCls($(this),childrenNodes,onCheck);

					for(var i=0;i<childNodes.length;i++){
						var childN = childNodes[i];
						var command = "<命令 命令名='设置图层显隐' 图层ID='" + childN.id + "' 显隐='1' />";
						var earthAX = document.getElementById( "superEarthOBJ" );
						earthAX.Connector( command );
						//alert( command );
						//alert( childN.id + ";" + childN.text );
					}
				}
			}else{
				if(childrenNodes==null){
					checkNodeId=node.id;
					//node.iconCls= 'icon-tree-classify-node-leaf';
					//$(this).tree('update', node);
					$("#layerTree").tree('update', {
						target:node.target,
						iconCls:'icon-tree-classify-node-leaf'
					});

					var command = "<命令 命令名='设置图层显隐' 图层ID='" + node.id + "' 显隐='0' />";
					var earthAX = document.getElementById( "superEarthOBJ" );
					earthAX.Connector( command );


				}else{
					changeNodeIconCls($(this),childrenNodes,onCheck);


					for(var i=0;i<childNodes.length;i++){
						var childN = childNodes[i];
						var command = "<命令 命令名='设置图层显隐' 图层ID='" + childN.id + "' 显隐='0' />";
						var earthAX = document.getElementById( "superEarthOBJ" );
						earthAX.Connector( command );
						//alert( command );
						//alert( childN.id + ";" + childN.text );
					}

				}
			}
		},onLoadSuccess:function(node,data){
			var rootNode=$(this).tree("getRoots");

			//只展示一个系统
			$(this).tree("expand",rootNode[0].target);
			$(this).tree("hideNode",rootNode[0].target);
//			selectNode=rootNode[0];
		},onBeforeExpand:function(node){
			//alert(JSON.stringify(node.children));

			if(node.children!=null){
				return true;
			}

			var earthAX = document.getElementById( "superEarthOBJ" );

			var text=node.text;
			var childrenNodeStr = earthAX.GetChildLayer( userName, text );//getChildrenNode(text);


			var childDataJson = xmlToJson(childrenNodeStr).document.Node;

			var nodeDate = getChildrenData( childDataJson );

			$(this).tree("append",{
				parent: node.target,
				data:nodeDate
			});
		},onBeforeCheck:function(node, check){
			if ( check ) //选中，
			{
				$(this).tree("expand",node.target);
			}
		}
	});

	var date6 = new Date;
	var date7 = date6.getTime() - date4.getTime();
	//alert( date7 );




	RegFirstFrame();

	function rightMenu( backvalue )
	{
		if ( backvalue.indexOf( "按钮='4'" ) )
		{
			//首先关闭已打开的菜单
			closeRightMenu();


			//解析右键位置
			var xmlDoc;
			xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
			xmlDoc.async = false;
			xmlDoc.loadXML( backvalue );

			if ( !xmlDoc )
			{
				return;
			}

			var root = xmlDoc.getElementsByTagName( "事件" )[0];
			var x = root.getAttribute( "参数1" );
			var y = root.getAttribute( "参数2" );


			//获取点击图层信息
			var command = "<命令 命令名='获取图层信息' X='" + x + "' Y='" + y + "' />";

			var result = earthAX.Connector( command );
			if ( result == "" )
			{
				return;
			}

			xmlDoc.loadXML( result );
			if ( !xmlDoc )
			{
				return;
			}

			root = xmlDoc.getElementsByTagName( "Layer" )[0];
			var layerID = root.getAttribute( "layerID" );
			var layerName = root.getAttribute( "layerName" );
			var tabName = root.getAttribute( "tabName" );
			var featureID = root.getAttribute( "featureID" );
			var featureName = root.getAttribute( "featureName" );
			var color = root.getAttribute( "color" );
			menuName = layerName + ":" + featureName;
			baseUrlPatch = "?ID=" + featureID + "&TAB=" + tabName.toUpperCase();

			//获取图层服务
			result = earthAX.UserSoap( userName, layerID );
			//alert( result );
			//earthAX.popuHTMLWindow(, utl, );


			//解析服务
			xmlDoc.loadXML( result );

			if ( !xmlDoc )
			{
				return;
			}

			var root = xmlDoc.getElementsByTagName( "document" )[0];

			var topNodes = root.childNodes;

			var maxLength = 0;
			var soapStr = "";
			for ( var i = 0; i < topNodes.length; i++ )
			{
				//Top Ele
				var serviceTitle = topNodes[i].getAttribute( "ServiceTitle" );
				var serviceUrl = topNodes[i].getAttribute( "ServiceURL" );
				var serviceAction = topNodes[i].getAttribute( "ServiceAction" );
				var serviceXML = topNodes[i].getAttribute( "ServiceXML" );

				var urlPatch = earthAX.ServicePatch( serviceAction, serviceXML, featureID, featureName, "", tabName );

				soapStr += "<soap name='" + serviceTitle + "' url='" + serviceUrl+urlPatch + "' />\n";

				var lengthTest = strByteLength( serviceTitle );
				if ( lengthTest > maxLength )
					maxLength = lengthTest;
			}

			highLightFeature( layerID, featureID, color );	//高亮
			curHighLayerID = layerID;


			menu1Length = 108;	//8 * 6 + 60
			var menu1LengthTest = strByteLength(menuName) * 6 + 60;
			if ( menu1LengthTest )
			{
				menu1Length = menu1LengthTest;
			}


			var converty = earthAX.clientHeight - y;
			maxLength = maxLength * 6 + 60;
			var menuStr = "<menu name='" + menuName + "' baseurl='" + baseUrlPatch + "' length1='" + menu1Length + "' length2='" + maxLength + "' x='" + x + "' y='" + converty + "' >\n";
			menuStr += soapStr;
			menuStr += "</menu>";
			menuStr = menuStr.replace( /\&/g, '$' );

			var tempCMD = "<命令 命令名='临时存取' 状态='存' 内容=\"" + menuStr + "\" />";
			earthAX.Connector( tempCMD );




			var url = getURL() + urlLoc + "html/menu.html";

			var soapHigh = 48;
			if ( soapStr == "" )	//若无成果关联则只显示基本信息菜单
			{
				soapHigh = 24;
			}


			currentMenuNum = (currentMenuNum + 1) % 9;	//获取一个空闲的ID
			earthAX.PopupUIHtmlWindow( "menu1"+currentMenuNum, parseInt(x)+2, converty, menu1Length, soapHigh, url, 100, false );

			tempCMD = "<命令 命令名='临时存取' 状态='存' 内容=\"" + "menu1"+currentMenuNum + "\" 序号='1' />";
			earthAX.Connector( tempCMD );

			menuIsShow = true;

		}


	}
	RegMouseEvent( rightMenu );

	//RegDockButton();
	RegInteractive();

	RegBufferAnalysis();

	RegRightMenuCallback();

	var dataUrl = earthAX.GetRoleUrl( userName );
	var dataCache = earthAX.GetRoleCache( userName );
	var earthUrl = earthAX.GetRoleEarth( userName );
	earthAX.Run3DByUrlCacheEarth( dataUrl, dataCache, earthUrl );
	//earthAX.Run3D();


	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
//	xmlDoc.load( "help/Typhoon.xml" );
	xmlDoc.loadXML( typhoonDataStr );
	if ( !xmlDoc )
	{
		return;
	}

	var tyroot = xmlDoc.getElementsByTagName( "document" )[0];
	if ( tyroot == null )
	{
		return;
	}

	for ( var i = 0; i < tyroot.childNodes.length; i++ )
	{
		var childEle = tyroot.childNodes[ i ];
		var typhoonPoint = new Object();
		typhoonPoint.index = childEle.getAttribute( "index" );
		typhoonPoint.time = childEle.getAttribute( "time" );
		typhoonPoint.lon = childEle.getAttribute( "lon" );
		typhoonPoint.lat = childEle.getAttribute( "lat" );
		typhoonPoint.press = childEle.getAttribute( "press" );
		typhoonPoint.windSpeed = childEle.getAttribute( "windSpeed" );
		typhoonPoint.windLevel = childEle.getAttribute( "windLevel" );
		typhoonPoint.level = childEle.getAttribute( "level" );
		typhoonPoint.speed = childEle.getAttribute( "speed" );
		typhoonPoint.direction = childEle.getAttribute( "direction" );
		typhoonPoint.radius7 = childEle.getAttribute( "radius7" );
		typhoonPoint.radius10 = childEle.getAttribute( "radius10" );

		typhoonData.push( typhoonPoint );
	}
}

function moveHighLight()
{
	if ( !moveLightSwitch )
	{
		return;
	}

	if ( !moveLight )
	{
		return;
	}

	//获取点击图层信息
	var command = "<命令 命令名='获取图层信息' X='" + mouseMoveX + "' Y='" + mouseMoveY + "' 容差='50.0' />";
	var earthAX = document.getElementById( "superEarthOBJ" );
	var result = earthAX.Connector( command );
	if ( result == "" )
	{
		if ( curHighLayerID != 0 )
		{
			unHighLightFeature( curHighLayerID );
			curHighLayerID = 0;
			curFeatureID = 0;
		}

		return;
	}

	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.loadXML( result );
	if ( !xmlDoc )
	{
		return;
	}

	var root = xmlDoc.getElementsByTagName( "Layer" )[0];
	if ( root == null )
	{
		return;
	}
	var layerID = root.getAttribute( "layerID" );
	var featureID = root.getAttribute( "featureID" );
	var color = root.getAttribute( "color" );

	if ( curHighLayerID != layerID || curFeatureID != featureID )
	{
		unHighLightFeature( curHighLayerID );
		highLightFeature( layerID, featureID, color );

		curHighLayerID = layerID;
		curFeatureID = featureID;
	}


}

function BatchLayer( username )
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	var batchLayerXML = earthAX.GetBatchLayer( username );

	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.async = false;
	xmlDoc.loadXML( batchLayerXML );

	if ( xmlDoc )
	{
		var root = xmlDoc.getElementsByTagName( "document" )[0];

		if ( root.childNodes.length > 0 )
		{

			for ( var i = 0; i < root.childNodes.length; i++ )
			{
				var layerEle = root.childNodes[i];

				var tabFLDName = earthAX.TabFLDName( layerEle.getAttribute( "SpaceTable" ) );

				var command = "<命令 命令名='设置加载图层' 图层ID='" + layerEle.getAttribute( "ID" ) + "' 图层名称='" + layerEle.getAttribute( "Name" ) + "' 数据库表名='" + layerEle.getAttribute( "SpaceTable" ) + "' 几何类型='" + layerEle.getAttribute( "ThreeDType" ) + "' 范围权限='" + layerEle.getAttribute( "Rect" )+
					"' RELTOSPACE='" + layerEle.getAttribute( "RELTOSPACE" ) + "' MINSHOWDISTANCE='" + layerEle.getAttribute( "ThreeDRangeL" ) + "' MAXSHOWDISTANCE='" + layerEle.getAttribute( "ThreeDRangeH" ) + "' 显隐='" + layerEle.getAttribute( "Batch" ) + "' Color='" + layerEle.getAttribute( "ThreeDColor" ) +
					"' BorderWidth='" + layerEle.getAttribute( "ThreeDBorderWidth" ) + "' TextSize='" + layerEle.getAttribute( "ThreeDTextSize" ) + "' TextColor='" + layerEle.getAttribute( "ThreeDTextColor" ) + "' TextureFileName='" + layerEle.getAttribute( "TextureFileName" ) +
					"' PointSize='" + layerEle.getAttribute( "ThreeDPointSize" ) + "' IsTextLabel='" + layerEle.getAttribute( "IsTextLabel" ) + "' LabelRange='" + layerEle.getAttribute( "ThreeDTextRange" ) + "' Transparent='" + layerEle.getAttribute( "Transparent" ) +
					"' Deviation='" + layerEle.getAttribute( "ThreeDDeviation" ) + "' Pickable='" + layerEle.getAttribute( "ThreeDISPickable" ) + "' ThreeDPipe='" + layerEle.getAttribute( "ThreeDIS3DPipe" ) + "' ThreeDPipeRadius='" + layerEle.getAttribute( "PipeRadius" ) +
					"' Leftx='" + Leftx + "' Topy='" + Topy + "' Rightx='" + Rightx + "' Bottomy='" + Bottomy +
					"' ForeColor='" + layerEle.getAttribute( "ForeColor" ) + "' MaxLayer='" + layerEle.getAttribute( "ThreeDMaxLayer" ) + "' HighLightH='" + layerEle.getAttribute( "ThreeDHighLightH" ) + "' >\n" +
					"<Fields>\n" + tabFLDName + "</Fields>\n" +
					"<Points>\n" + RangePoints + "</Points>\n" +
					"</命令>";


					commandList.push( command );

			}
		}
	}
}

function RegFirstFrame(recallFunc)
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	earthAX.RegCallback(parseInt("0x00000001", 16), callbackScriptJAS , parseInt('0xffffffff', 16));

	//返回事件，处理后调用返回函数
	function callbackScriptJAS(backValue)
	{

		if(null != backValue && backValue.indexOf("消息值='128'") != -1 )
		{
			firstFrame = true;


			//
			//earthAX.Connector( "<命令 命令名='状态栏元素配置'><相机高度显隐 state='OFF'/></命令>" );

			setInterval( moveHighLight, 100 );


			//设置挂件显隐
			DecorationSetting();

			//设置浮动工具栏项
			ToolbarSetting();

			//设置默认视点
			DefaultSetting();

			//设置海洋显隐
			OceanVisibleSetting();

			//设置海洋风参数
			OceanWindSetting();

			//设置矢量加载进度条显隐
			VecProgressVisibleSetting();


			for ( var i = 0; i < commandList.length; i++ )
			{
				//alert( commandList[i] );
				//var earthAX = document.getElementById( "superEarthOBJ" );
				earthAX.Connector( commandList[i] );
			}
		}

	}

}

/* 判断是否是拖动弹起的 */
var dragMouse = false;

function RegMouseEvent(recallFunc){
	var earthAX = getEarth();
	earthAX.RegCallback(parseInt("0x00000002", 16), callbackScriptJAS , parseInt('0xffffffff', 16));

	//返回事件，处理后调用返回函数
	function callbackScriptJAS(backValue){
		moveLight = false;

		if ( null == backValue )
		{
			return;
		}
		/* 按下、双击或滚动的时候，如果菜单显示，则将菜单关闭 */
		if ( (backValue.indexOf("消息值='1'") != -1 || backValue.indexOf("消息值='4'") != -1 || backValue.indexOf("消息值='512'") != -1) && menuIsShow )
		{
			closeRightMenu();
			menuIsShow = false;
		}

		/* 记录鼠标拖动 */
		else if ( backValue.indexOf("消息值='8'") != -1 )
		{
			dragMouse = true;
		}

		/* 鼠标弹起，弹出右键菜单 */
		else if( backValue.indexOf("消息值='2'") != -1 )
		{
			if ( dragMouse )
			{
				dragMouse = false;
				return;
			}
			if (backValue.indexOf("按钮='4'") != -1)
				recallFunc(backValue);

		}

		/* 记录鼠标滑动时的坐标 */
		else if ( backValue.indexOf( "消息值='16'" ) != -1 )
		{
			var xmlDoc;
			xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
			xmlDoc.async = false;
			xmlDoc.loadXML( backValue );

			if ( !xmlDoc )
			{
				return;
			}

			var root = xmlDoc.getElementsByTagName( "事件" )[0];
			var x = root.getAttribute( "参数1" );
			var y = root.getAttribute( "参数2" );

			mouseMoveX = x;
			mouseMoveY = y;

			moveLight = true;

		}

		/* 响应鼠标双击事件 */
		else if ( backValue.indexOf( "消息值='4'" ) != -1 )
		{
			if ( backValue.indexOf( "按钮='1'" ) != -1 )
			{
				//弹出标牌
				var selectCmd = "<命令 命令名='命中测试' X='" + mouseMoveX + "' Y='" + mouseMoveY + "' ></命令>";
				var selectResult = earthAX.Connector( selectCmd );

				var xmlDoc;
				xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
				xmlDoc.async = false;
				xmlDoc.loadXML( selectResult );

				if ( !xmlDoc )
				{
					return;
				}

				var root = xmlDoc.getElementsByTagName( "对象" )[0];
				if ( root == null )
				{
					return;
				}
				var objID = parseInt( root.getAttribute( "ID" ) );
				var objName = root.getAttribute( "名称" );

				if ( objID > -(61 + typhoonData.length) && objID < -60 )	//弹出台风标牌
				{
					var index = -(objID + 61);
					var typhoonPoint = typhoonData[ index ];
					if ( labelCreate )
					{
						var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-30'></命令>";
						earthAX.Connector( command );
					}


					var labelCmd = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-30' 类型ID='24' 名字='中盈高科' 转换标志='false'>" +
										"<TEXTLABEL TITEL='" + typhoonPoint.time + "' TITELFONT='simhei.ttf' TITELFONTHEIGHT='6' TITELBKPIC='' " +
											"TITLEBKCOLOR='0.0 0.63 0.9 1.0' TITELCOLOR='1 1 1 1' TITLEALIGN='center' TITLEUNDERLINE='false' BKPIC='' " +
											"MINSCALE='0.15' MAXSCALE='0.15' BINDPOS='" + typhoonPoint.lon + " " + typhoonPoint.lat + " " + "0.0' " +
											"BKCOLOR='0.8 0.8 0.8 0.8' USEGRID='true' GRIDCOLOR='0.2852 0.5594 0.6241 0.5' " +
											"GRIDLINEWIDTH='1' BINDID='0' MINSHOWDISTANCE='100' MAXSHOWDISTANCE='100000000' MINSHOWANGLE='0' MAXSHOWANGLE='90' BTNIMAGE='' " +
											"HORIZONTALOFFSET='-25' VERTICALOFFSET='70' ARROWCOLOR='0.2852 0.5594 0.6241 0.4' USETRIANGLE='true' ARROWLINEWIDTH='2' TRIANGLEFIXED='false'> " +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>当前位置</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.lon + "/" + typhoonPoint.lat + "度</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>中心气压</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.press + "百帕</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>最大风速</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.windSpeed + "米/秒</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>风力</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.windLevel + "级</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>等级</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.level + "</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>移动速度</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.speed + "公里/小时</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>移动方向</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.direction + "</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>七级半径</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.radius7 + "公里</ITEM>" +
												"</ROW>" +
												"<ROW>" +
													"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>十级半径</ITEM>" +
													"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>" + typhoonPoint.radius10 + "公里</ITEM>" +
												"</ROW>" +
										"</TEXTLABEL>" +
									"</命令>";

					earthAX.Connector( labelCmd );
					labelCreate = true;


					var showLabelCmd = "<命令 命令名='显示对象基本动作'  ID='-30' 动作类型='显隐'  参数='1' ></命令>";
					earthAX.Connector( showLabelCmd );
				}
				else if ( objID == "1094359" || objID == "1042640" )	//弹出飞机或厂区建筑物标牌
				{
					var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-31'></命令>";
					earthAX.Connector( command );

					var labelCmd = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-31' 类型ID='24' 名字='' 转换标志='false'>" +
							"<TEXTLABEL TITEL='直升机参数' TITELFONT='simhei.ttf' TITELFONTHEIGHT='6' TITELBKPIC='' " +
								"TITLEBKCOLOR='0.0 0.63 0.9 1.0' TITELCOLOR='1 1 1 1' TITLEALIGN='center' TITLEUNDERLINE='false' BKPIC='' " +
								"MINSCALE='0.15' MAXSCALE='0.15' BINDID='" + objID +
								"' BKCOLOR='0.8 0.8 0.8 0.8' USEGRID='true' GRIDCOLOR='0.2852 0.5594 0.6241 0.5' " +
								"GRIDLINEWIDTH='1' MINSHOWDISTANCE='10' MAXSHOWDISTANCE='100000000' MINSHOWANGLE='0' MAXSHOWANGLE='90' BTNIMAGE='' " +
								"HORIZONTALOFFSET='-25' VERTICALOFFSET='70' ARROWCOLOR='0.2852 0.5594 0.6241 0.4' USETRIANGLE='true' ARROWLINEWIDTH='2' TRIANGLEFIXED='false'> " +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>旋翼直径</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>32.00m</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>尾桨直径</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>7.61m</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>机长</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>40.03m</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>机高</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>8.15m</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>起飞重量</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>49600kg</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>平飞速度</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>295km/h</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>巡航速度</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>255km/h</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>悬停高度</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>1000m</ITEM>" +
									"</ROW>" +
									"<ROW>" +
										"<ITEM COLOR='0.1 0.1 0.1 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>航程</ITEM>" +
										"<ITEM COLOR='0 0 0 1' FONTNAME='msyh.ttf' FONTHEIGHT='4' USERDATA='' ALIGNTYPE='left'>500km</ITEM>" +
									"</ROW>" +
							"</TEXTLABEL>" +
						"</命令>";


						earthAX.Connector( labelCmd );

					var showLabelCmd = "<命令 命令名='显示对象基本动作'  ID='-31' 动作类型='显隐'  参数='1' ></命令>";
					earthAX.Connector( showLabelCmd );
				}
				else if ( objID == "1090879" || objID == "1086101" || objID == "1089998" || objID == "1089446" || objID == "1090901" )	//弹出双向定位界面
				{
					var windowExist = earthAX.JudgmentWindow( "location" );
					if ( windowExist )
					{
						earthAX.InvokeUICallback( "location", objID );
					}
					else
					{
						var url = getURL() + urlLoc + "html/location.html?objID=" + objID;
						earthAX.PopupDragUIHtmlWindow( "location", 200, 200, 500, 350, url, 100, false );
						setHtmlWindowDragRect( "location", 0, 0, 500, 30 );
					}

				}
				else if ( -100 < parseInt( objID ) && parseInt( objID ) <= -90 )	//切换二三维
				{
					earthAX.Switch2D( objName );
				}

			}

		}

	}
}


function RegBufferAnalysis(recallFunc)
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	earthAX.RegCallback(parseInt("0x00060000", 16), callbackScriptJAS , parseInt('0xffffffff', 16));

	//返回事件，处理后调用返回函数
	function callbackScriptJAS(backValue)
	{

		if(null != backValue && backValue.indexOf("消息值='1'") != -1 )
		{
			moveLightSwitch = true;
		}

	}

}

function RegRightMenuCallback() {
	var earthAX = document.getElementById( "superEarthOBJ" );
	earthAX.RegUICallback( "rightmenu", callbackScriptJAS );

	function callbackScriptJAS( url ){
		if ( null != url ) {
			
			url = url.replace( /\$/g, '&' );
			window.open( url );
			//var menuNum = earthAX.Connector( "<命令 命令名='临时存取' 状态='取' 序号='1' />" );
			//earthAX.HideWindow( menuNum );

			//menuNum = earthAX.Connector( "<命令 命令名='临时存取' 状态='取' 序号='2' />" );
			//earthAX.HideWindow( menuNum );
		}
	}
}

function highLightFeature( layerID, featureID, color )
{
	if ( layerID == 0 )
	{
		return;
	}

	var earthAX = getEarth();
	var command = "<命令 命令名='高亮矢量要素' 高亮='ON' LayerID=" + layerID + " FeatureID='" + featureID + "' Color='" + color + "' />";
	//alert( command );
	earthAX.Connector( command );
}

function unHighLightFeature( layerID )
{
	if ( layerID == 0 )
	{
		return;
	}

	var earthAX = getEarth();
	var command = "<命令 命令名='高亮矢量要素' 高亮='OFF' LayerID='" + layerID + "' />";
	earthAX.Connector( command );
}

/**
 * 获取相对URL路径公共方法
 */
function getURL() {
    var curWwwPath = location.href;
    var pathName = location.pathname;
    var pos = curWwwPath.indexOf(pathName);
    //获取主机地址，如： http://localhost:8080
    var localhostPaht = curWwwPath.substring(0, pos);
    //获取带"/"的项目名，如：/uimcardprj
    var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
    return(localhostPaht + projectName);
}



function changeNodeIconCls(treeObj,childrenNodes,onCheck){
	if(onCheck){
		for(var i=0;i<childrenNodes.length;i++){
			var childrenNode=childrenNodes[i];
			var isLeaf=treeObj.tree("isLeaf",childrenNode.target);
			if(isLeaf){
				checkNodeId=childrenNode.id;
				//childrenNode.iconCls= 'icon-tree-classify-node-leaf-open';
				//treeObj.tree('update', childrenNode);
				treeObj.tree('update', {
					target:childrenNode.target,
					iconCls:'icon-tree-classify-node-leaf-open'
				});
			}else{
				var childNodes=treeObj.tree("getChildren",childrenNode.target);
				changeNodeIconCls(treeObj,childNodes,onCheck);
			}
		}
	}else{
		for(var i=0;i<childrenNodes.length;i++){
			var childrenNode=childrenNodes[i];
			var isLeaf=treeObj.tree("isLeaf",childrenNode.target);
			if(isLeaf){
				checkNodeId=childrenNode.id;
				//childrenNode.iconCls= 'icon-tree-classify-node-leaf';
				//treeObj.tree('update', childrenNode);
				treeObj.tree('update', {
					target:childrenNode.target,
					iconCls:'icon-tree-classify-node-leaf'
				});
			}else{
				var childNodes=treeObj.tree("getChildren",childrenNode.target);
				changeNodeIconCls(treeObj,childNodes,onCheck);
			}
		}
	}
}









function xmlToJson(xml) {
//	xml=(xml+"").replace("\\","");
    var index=xml.indexOf("\n<");
//    xml=xml.substring(index+1,xml.length);
    xml=$.parseXML(xml);
		tab = '';
		var X = {
			toObj : function(xml) {
				var o = {};
				if (xml.nodeType == 1) {
					if (xml.attributes.length)
						for (var i = 0; i < xml.attributes.length; i++)
							o["" + xml.attributes[i].nodeName] = (xml.attributes[i].nodeValue || "")
									.toString();
					if (xml.firstChild) {
						var textChild = 0, cdataChild = 0, hasElementChild = false;
						for (var n = xml.firstChild; n; n = n.nextSibling) {
							if (n.nodeType == 1)
								hasElementChild = true;
							else if (n.nodeType == 3
									&& n.nodeValue.match(/[^ \f\n\r\t\v]/))
								textChild++;
							else if (n.nodeType == 4)
								cdataChild++;
						}
						if (hasElementChild) {
							if (textChild < 2 && cdataChild < 2) {
								X.removeWhite(xml);
								for (var n = xml.firstChild; n; n = n.nextSibling) {
									if (n.nodeType == 3)
										o["#text"] = X.escape(n.nodeValue);
									else if (n.nodeType == 4)
										o["#cdata"] = X.escape(n.nodeValue);
									else if (o[n.nodeName]) {
										if (o[n.nodeName] instanceof Array)
											o[n.nodeName][o[n.nodeName].length] = X
													.toObj(n);
										else
											o[n.nodeName] = [
													o[n.nodeName],
													X.toObj(n) ];
									} else
										o[n.nodeName] = X.toObj(n);
								}
							} else {
								if (!xml.attributes.length)
									o = X.escape(X.innerXml(xml));
								else
									o["#text"] = X.escape(X.innerXml(xml));
							}
						} else if (textChild) {
							if (!xml.attributes.length)
								o = X.escape(X.innerXml(xml));
							else
								o["#text"] = X.escape(X.innerXml(xml));
						} else if (cdataChild) {
							if (cdataChild > 1)
								o = X.escape(X.innerXml(xml));
							else
								for (var n = xml.firstChild; n; n = n.nextSibling)
									o["#cdata"] = X.escape(n.nodeValue);
						}
					}
					if (!xml.attributes.length && !xml.firstChild)
						o = null;
				} else if (xml.nodeType == 9) {
					o = X.toObj(xml.documentElement);
				} else
					alert("unhandled node type: " + xml.nodeType);
				return o;
			},
			toJson : function(o, name, ind) {
				var json = name ? ("\"" + name + "\"") : "";
				if (o instanceof Array) {
					for (var i = 0, n = o.length; i < n; i++)
						o[i] = X.toJson(o[i], "", ind + "\t");
					json += (name ? ":[" : "[")
							+ (o.length > 1 ? ("\n" + ind + "\t"
									+ o.join(",\n" + ind + "\t") + "\n" + ind)
									: o.join("")) + "]";
				} else if (o == null)
					json += (name && ":") + "null";
				else if (typeof (o) == "object") {
					var arr = [];
					for ( var m in o)
						arr[arr.length] = X.toJson(o[m], m, ind + "\t");
					json += (name ? ":{" : "{")
							+ (arr.length > 1 ? ("\n" + ind + "\t"
									+ arr.join(",\n" + ind + "\t") + "\n" + ind)
									: arr.join("")) + "}";
				} else if (typeof (o) == "string")
					json += (name && ":") + "\"" + o.toString() + "\"";
				else
					json += (name && ":") + o.toString();
				return json;
			},
			innerXml : function(node) {
				var s = ""
				if ("innerHTML" in node)
					s = node.innerHTML;
				else {
					var asXml = function(n) {
						var s = "";
						if (n.nodeType == 1) {
							s += "<" + n.nodeName;
							for (var i = 0; i < n.attributes.length; i++)
								s += " "
										+ n.attributes[i].nodeName
										+ "=\""
										+ (n.attributes[i].nodeValue || "")
												.toString() + "\"";
							if (n.firstChild) {
								s += ">";
								for (var c = n.firstChild; c; c = c.nextSibling)
									s += asXml(c);
								s += "</" + n.nodeName + ">";
							} else
								s += "/>";
						} else if (n.nodeType == 3)
							s += n.nodeValue;
						else if (n.nodeType == 4)
							s += "<![CDATA[" + n.nodeValue + "]]>";
						return s;
					};
					for (var c = node.firstChild; c; c = c.nextSibling)
						s += asXml(c);
				}
				return s;
			},
			escape : function(txt) {
				return txt.replace(/[\\]/g, "\\\\").replace(/[\"]/g, '\\"')
						.replace(/[\n]/g, '\\n').replace(/[\r]/g, '\\r');
			},
			removeWhite : function(e) {
				e.normalize();
				for (var n = e.firstChild; n;) {
					if (n.nodeType == 3) {
						if (!n.nodeValue.match(/[^ \f\n\r\t\v]/)) {
							var nxt = n.nextSibling;
							e.removeChild(n);
							n = nxt;
						} else
							n = n.nextSibling;
					} else if (n.nodeType == 1) {
						X.removeWhite(n);
						n = n.nextSibling;
					} else
						n = n.nextSibling;
				}
				return e;
			}
		};
		if (xml.nodeType == 9)
			xml = xml.documentElement;
		var json = X.toJson(X.toObj(X.removeWhite(xml)), xml.nodeName, "\t");
		return JSON.parse("{"+ tab+ (tab ? json.replace(/\t/g, tab) : json.replace(/\t|\n/g,"")) + "}");
}
function createTreeJson(dataObj){
	var treeObj=new Array();
	if(dataObj.length>1){
		for(var i=0;i<dataObj.length;i++){
			var data=dataObj[i];
			var name=data.Name;
			var node=data.Node;
			var id1=data.ID;
			var id2=data.NO;
			var id=id1==null?id2:id1;
			var nodeData=null;
			var obj={};
			var attributes={"ThreeDType":data.ThreeDType};
			if(node!=null && node!=""){
				//nodeData=getChildrenData(node);
				obj={"id":id,"text":name,"children":null,"attributes":attributes,"iconCls":"icon-tree-classify-node-root","state":"closed"};
			}else{//iconCls
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-root","state":"closed"};
			}
			treeObj.push(obj);
		}
	}else{
		var name=dataObj.Name;
		var node=dataObj.Node;
		var id1=dataObj.ID;
		var id2=dataObj.NO;
		var id=id1==null?id2:id1;
		var nodeData=null;
		var obj={};
		var attributes={"ThreeDType":dataObj.ThreeDType};
		if(node!=null && node!=""){
			nodeData=getChildrenData(node);
			obj={"id":id,"text":name,"children":null,"attributes":attributes,"iconCls":"icon-tree-classify-node-root","state":"closed"};
		}else{//iconCls
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-root","state":"closed"};
		}
		treeObj.push(obj);
	}
	return treeObj;
}
function getChildrenData(childrenData){

	var treeChildrenObj=new Array();
	if(childrenData.length>1){
		for(var i=0;i<childrenData.length;i++){
			var data=childrenData[i];
			var name=data.Name;
			var node=data.Node;
			var id1=data.ID;
			var id2=data.NO;
			var id=id1==null?id2:id1;
			var nodeData=null;
			var obj={};
			var attributes={"ThreeDType":data.ThreeDType};
			if(node!=null && node!=""){
				nodeData=getChildrenData(node);
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node","state":"closed"};
			}else{
				//场景启动
				//<命令 命令名="设置加载图层" 图层名称="地级行政界线" 图层ID="2" 几何类型="POLYLINE" 显隐="1"/>
				var earthAX = document.getElementById( "superEarthOBJ" );
				if ( earthAX )
				{
					var tabFLDName = earthAX.TabFLDName( data.SpaceTable );

					var command = "<命令 命令名='设置加载图层' 图层ID='" + id + "' 图层名称='" + data.Name + "' 数据库表名='" + data.SpaceTable + "' 几何类型='" + data.ThreeDType + "' 范围权限='" + data.Rect +
					"' RELTOSPACE='" + data.RELTOSPACE + "' MINSHOWDISTANCE='" + data.ThreeDRangeL + "' MAXSHOWDISTANCE='" + data.ThreeDRangeH + "' 显隐='" + data.Batch + "' Color='" + data.ThreeDColor +
					"' BorderWidth='" + data.ThreeDBorderWidth + "' TextSize='" + data.ThreeDTextSize + "' TextColor='" + data.ThreeDTextColor + "' TextureFileName='" + data.TextureFileName +
					"' PointSize='" + data.ThreeDPointSize + "' IsTextLabel='" + data.IsTextLabel + "' LabelRange='" + data.ThreeDTextRange + "' Transparent='" + data.Transparent +
					"' Deviation='" + data.ThreeDDeviation + "' Pickable='" + data.ThreeDISPickable + "' ThreeDPipe='" + data.ThreeDIS3DPipe + "' PipeRadius='" + data.ThreeDPipeRadius +
					"' Leftx='" + Leftx + "' Topy='" + Topy + "' Rightx='" + Rightx + "' Bottomy='" + Bottomy +
					"' ForeColor='" + data.ForeColor + "' MaxLayer='" + data.ThreeDMaxLayer + "' HighLightH='" + data.ThreeDHighLightH + "' >\n" +
					"<Fields>\n" + tabFLDName + "</Fields>\n" +
					"<Points>\n" + RangePoints + "</Points>\n" +
					"</命令>";

					if ( firstFrame )
					{
						earthAX.Connector( command );
					}
					else
					{
						commandList.push( command );
					}

					//alert( command );
				}


				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-leaf"};
			}
			treeChildrenObj.push(obj);
		}
	}else{
		var name=childrenData.Name;
		var node=childrenData.Node;
		var id1=childrenData.ID;
		var id2=childrenData.NO;
		var id=id1==null?id2:id1;
		var nodeData=null;
		var obj={};
		var attributes={"ThreeDType":childrenData.ThreeDType};
		if(node!=null && node!=""){
			nodeData=getChildrenData(node);
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node","state":"closed"};
		}else{
			//场景启动
			//<命令 命令名="设置加载图层" 图层名称="地级行政界线" 图层ID="2" 几何类型="POLYLINE" 显隐="1"/>
			var earthAX = document.getElementById( "superEarthOBJ" );
			if ( earthAX )
			{
				var tabFLDName = earthAX.TabFLDName( childrenData.SpaceTable );

				var command = "<命令 命令名='设置加载图层' 图层ID='" + id + "' 图层名称='" + childrenData.Name + "' 数据库表名='" + childrenData.SpaceTable + "' 几何类型='" + childrenData.ThreeDType + "' 范围权限='" + childrenData.Rect +
				"' RELTOSPACE='" + childrenData.RELTOSPACE + "' MINSHOWDISTANCE='" + childrenData.ThreeDRangeL + "' MAXSHOWDISTANCE='" + childrenData.ThreeDRangeH + "' 显隐='" + childrenData.Batch + "' Color='" + childrenData.ThreeDColor +
				"' BorderWidth='" + childrenData.ThreeDBorderWidth + "' TextSize='" + childrenData.ThreeDTextSize + "' TextColor='" + childrenData.ThreeDTextColor + "' TextureFileName='" + childrenData.TextureFileName +
				"' PointSize='" + childrenData.ThreeDPointSize + "' IsTextLabel='" + childrenData.IsTextLabel + "' LabelRange='" + childrenData.ThreeDTextRange + "' Transparent='" + childrenData.Transparent +
				"' Deviation='" + childrenData.ThreeDDeviation + "' Pickable='" + childrenData.ThreeDISPickable + "' ThreeDPipe='" + childrenData.ThreeDIS3DPipe + "' PipeRadius='" + childrenData.ThreeDPipeRadius +
				"' Leftx='" + Leftx + "' Topy='" + Topy + "' Rightx='" + Rightx + "' Bottomy='" + Bottomy +
				"' ForeColor='" + childrenData.ForeColor + "' MaxLayer='" + childrenData.ThreeDMaxLayer + "' HighLightH='" + childrenData.ThreeDHighLightH + "' >\n" +
				"<Fields>\n" + tabFLDName + "</Fields>\n" +
				"<Points>\n" + RangePoints + "</Points>\n" +
				"</命令>";

				if ( firstFrame )
				{
					earthAX.Connector( command );
				}
				else
				{
					commandList.push( command );
				}
				//alert( command );
			}


			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-leaf"};
		}
		treeChildrenObj.push(obj);
	}
	return treeChildrenObj;
}
function getChildrenNode(text){
	var nodeData=null;
	if(treeNodeData.length>1){
		for(var i=0;i<treeNodeData.length;i++){
			var data=treeNodeData[i];
			var name=data.Name;
			var node=data.Node;
			if(typeof(node)=='undefined'){
				break;
			}
			if(name==text){
				nodeData=getChildrenData(node);
				break;
			}else{
				nodeData=getSonChildrenNode(text,node);
				if(nodeData!=null){
					break;
				}
			}
		}
	}else{
		var name=treeNodeData.Name;
		var node=treeNodeData.Node;
		if(typeof(node)=='undefined'){
			return;
		}
		if(name==text){
			nodeData=getChildrenData(node);
		}else{
			nodeData=getSonChildrenNode(text,node);
		}
	}
	return nodeData;
}
function getSonChildrenNode(text,nodeDataObj){
	var nodeData=null;
	if(nodeDataObj.length>1){
		for(var i=0;i<nodeDataObj.length;i++){
			var data=nodeDataObj[i];
			var name=data.Name;
			var node=data.Node;
			if(typeof(node)=='undefined'){
				break;
			}
			if(name==text){
				nodeData=getChildrenData(node);
				break;
			}else{
				nodeData=getSonChildrenNode(text,node);
				if(nodeData!=null){
					break;
				}
			}
		}
	}else{
		var name=nodeDataObj.Name;
		var node=nodeDataObj.Node;
		if(typeof(node)=='undefined'){
			return;
		}
		if(name==text){
			nodeData=getChildrenData(node);
		}else{
			nodeData=getSonChildrenNode(text,node);
		}
	}
	return nodeData;
}

var close=0;
function colseTree(){
	if(close==0){
		$("#layOutDiv").layout("collapse","west");
		close=1;
	}else if(close==1){
		$("#layOutDiv").layout("expand","west");
		close=0;
	}
	changeLayout();
}

function closeRightMenu()
{
	var earthAX = document.getElementById( "superEarthOBJ" );
	earthAX.CloseWindow( "menu1" + currentMenuNum );				//关闭一级菜单

	var menu2Num = earthAX.Connector( "<命令 命令名='临时存取' 状态='取' 序号='2' />" );
	earthAX.CloseWindow( menu2Num );								//关闭二级菜单

	unHighLightFeature( curHighLayerID );
	curHighLayerID = 0;
}
