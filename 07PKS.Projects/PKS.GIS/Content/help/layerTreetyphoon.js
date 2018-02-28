//var str='<document><Node Name="测试"><Node NO="14" Type="Distriction" Name="行政区划线" Rect="0" Batch="1"><Node ID="1610300" Name="省界" ThreeDType="1" /><Node ID="1640100" Name="中国国界与省界" ThreeDType="" /><Node ID="1610500" Name="县界" ThreeDType="" /><Node ID="1640200" Name="县级行政界线" ThreeDType="" /><Node ID="1610400" Name="地区、自治州界" ThreeDType="" /><Node ID="1640300" Name="地级行政界线" ThreeDType="" /></Node></Node><Node Name="测试1"><Node NO="14" Type="Distriction" Name="行政区划线1" Rect="0" Batch="1"><Node ID="1610300" Name="省界1" ThreeDType="1" /><Node ID="1640100" Name="中国国界与省界1" ThreeDType="" /><Node ID="1610500" Name="县界1" ThreeDType="" /><Node ID="1640200" Name="县级行政界线1" ThreeDType="" /><Node ID="1610400" Name="地区、自治州界1" ThreeDType="" /><Node ID="1640300" Name="地级行政界线1" ThreeDType="" /></Node></Node></document>';
//var str='<document><Node Name="测试"><Node NO="14" Type="Distriction" Name="行政区划线" Rect="0" Batch="1"></Node></Node></document>';
var treeNodeData=null;
var checkNodeId="";
var isCheck=null;
var commandList = new Array();
var selectNode=null;

var nameList = new Array();	//服务名
var urlList=new Array();	//服务地址
var menuIsShow = false;	//1级菜单是否正在显示
var menuLevel = 1;	//将要弹出的菜单等级
var level2type = 0;	//正在显示的2级菜单类型。0表示不显示，1表示基本信息，2表示成果关联
var menuName = "";	//1级菜单第一项的名字
var mouseX = 0;		//鼠标位置，便于计算2级菜单的弹出位置
var mouseY = 0;
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

function createTree(){
	var userName=getParamter("userName");
	
	var earthAX = document.getElementById( "superEarthOBJ" );
	
	var date1 = new Date();
	str = earthAX.PBGISNInfo();
	
	var date2 = new Date();
	var date3 = date2.getTime() - date1.getTime();
	//alert( date3 );
	
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

	

	var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
	xmlDoc.load( "help/Typhoon.xml" );
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
	
	

	
	var data=xmlToJson(str).document.Node;
	treeNodeData=data;
	var date4 = new Date;
	var date5 = date4.getTime() - date2.getTime();
	//alert( date5 );
	
	date4 = new Date;
	var treeJson=createTreeJson(data);
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
					node.iconCls= 'icon-tree-classify-node-leaf-open';
					$(this).tree('update', node);
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
					node.iconCls= 'icon-tree-classify-node-leaf';
					$(this).tree('update', node);
					
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
			var earthAX = document.getElementById( "superEarthOBJ" );
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
			
			urlList = new Array();
			nameList = new Array();
			urlList.length=0;
			nameList.length = 0;
			for ( var i = 0; i < topNodes.length; i++ )
			{
				//Top Ele
				var serviceTitle = topNodes[i].getAttribute( "ServiceTitle" );
				var serviceUrl = topNodes[i].getAttribute( "ServiceURL" );
				var serviceAction = topNodes[i].getAttribute( "ServiceAction" );
				var serviceXML = topNodes[i].getAttribute( "ServiceXML" );
				
				var urlPatch = earthAX.ServicePatch( serviceAction, serviceXML, featureID, featureName, "", tabName );
				
				urlList.push( serviceUrl + urlPatch );
				nameList.push( serviceTitle );
			}
			
			
			closeDlg("rightMenu");
			
			//if ( urlList.length == 0 )
			//{
			//	return;
			//}
			highLightFeature( layerID, featureID );	//高亮
			curHighLayerID = layerID;

			//弹出右键菜单
	     	var offset=$("#superEarth").offset();
	     	var ObjHeight=$("#superEarth").height();
	     	var x1=parseInt(x)+offset.left;
	     	var y1=ObjHeight-parseInt(y)+offset.top;
			menuLevel = 1;
			mouseX = x1;
			mouseY = y1;
			
			menu1Length = 108;	//8 * 6 + 60
			var menu1LengthTest = strByteLength(menuName) * 6 + 60;
			if ( menu1LengthTest )
			{
				menu1Length = menu1LengthTest;
			}
			
	        getDlg1("html/test.html", 'rightMenu', '',menu1Length,47,parseInt(x1),parseInt(y1));
			
			menuIsShow = true;
			
		}

		
	}
	RegMouseEvent( rightMenu );
	
	RegDockButton();
	
	RegBufferAnalysis();

	earthAX.Run3D();
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

	if ( curHighLayerID != layerID || curFeatureID != featureID )
	{
		unHighLightFeature( curHighLayerID );
		highLightFeature( layerID, featureID );
		
		curHighLayerID = layerID;
		curFeatureID = featureID;
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
			for ( var i = 0; i < commandList.length; i++ )
			{
				//alert( commandList[i] );
				//var earthAX = document.getElementById( "superEarthOBJ" );
				earthAX.Connector( commandList[i] );
			}
			
			//
			//earthAX.Connector( "<命令 命令名='状态栏元素配置'><相机高度显隐 state='OFF'/></命令>" );
			
			setInterval( moveHighLight, 100 );
			
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
			closeDlg("rightMenu");
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
		
		else if ( backValue.indexOf( "消息值='4'" ) != -1 )
		{
			if ( backValue.indexOf( "按钮='0'" ) != -1 )
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
				var objID = parseInt( root.getAttribute( "ID" ) );
				
				if ( objID > -(61 + typhoonData.length) && objID < -60 )
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
			}
			
		}
			
	}
}
	
	
var LonLat = false;
var Clouds = false;
var Compass = false;
var BufferAnalysis = false;
var FlyLine = false;
function RegDockButton(recallFunc)
{

	var earthAX = getEarth();
	earthAX.RegCallback( parseInt( "0x00000800", 16 ), callbackScriptJAS, parseInt( "0xffffffff", 16 ) );
	

	function callbackScriptJAS( backValue )
	{
		if ( null != backValue && backValue.indexOf("消息值='2'") != -1 )
		{
			//处理消息值
			var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
			xmlDoc.loadXML( backValue );
			
			if ( !xmlDoc )
			{
				return;
			}
			
			var root = xmlDoc.getElementsByTagName( "事件" )[0];
			
			var uiNotifyNode = root.childNodes[0];
			
			var itemidentify = uiNotifyNode.getAttribute( "itemidentify" );
			if ( itemidentify == "Back" )
			{
				earthAX.Connector( "<命令 命令名='设置相机位置'  经度='114.25' 纬度='35.6' 海拔='15000000' 偏航角='0' 俯仰角='-90' 滚转角='0' 定位方式='普通' />" );
			}
			else if ( itemidentify == "LonLat" )
			{
				var showFlag = LonLat ? "OFF" : "ON";
				earthAX.Connector( "<命令 命令名= '经纬网'><显隐 state='" + showFlag + "' /></命令>" );
				LonLat = !LonLat;
			}
			else if ( itemidentify == "Clouds" )
			{
				var showFlag = Clouds ? "OFF" : "ON";
				earthAX.Connector( "<命令 命令名='挂件显隐' 类型='CLOUDS' 显隐='" + showFlag + "' />" );
				Clouds = !Clouds;
			}
			else if ( itemidentify == "Compass" )
			{
				var showFlag = Compass ? "OFF" : "ON";
				earthAX.Connector( "<命令 命令名='导航面板配置' ><大小比例 scale='0.8'/><位置 topMargin='32' rightMargin='32'/><显隐 state='" + showFlag + "' /></命令>" );
				Compass = !Compass;
			}
			else if ( itemidentify == "BufferAnalysis" )
			{
				if ( BufferAnalysis )
				{
					//关闭
					earthAX.Connector( "<命令 命令名='缓冲区分析' 状态='关闭' />")
					
					earthAX.Connector( "<命令 命令名='贴地圆形标绘' 操作='关闭' />")
					
					var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-20'></命令>";
					earthAX.Connector( command );
				}
				else
				{
					moveLightSwitch = false;
					//开启
					var openCommand = 	"<命令 命令名='贴地圆形标绘' 操作='开启'>" +
										"<显示配置>" +
										"<点大小 size='3' />" +
										"<点颜色 R='0' G='1' B='0' A='0' />" +
										"<预选点颜色 R='1' G='1' B='1' A='1' />" +
										"<线宽度 size='2' />" +
										"<线颜色 R='0' G='1' B='0' A='1' />" +
										"<预选线颜色 R='1' G='0' B='0' A='1' />" +
										"<面颜色 R='0' G='1' B='0' A='0.0' />" +
										"<预选面颜色 R='0' G='1' B='0' A='0.0' />" +
										"</显示配置>" +
										"<功能配置 是否贴地='否' />" +
										"</命令>";
					earthAX.Connector( openCommand );
				}
				
				
				BufferAnalysis = !BufferAnalysis;
			}
			else if ( itemidentify == "FlyLine" )
			{
				moveLightSwitch = FlyLine;
				if ( FlyLine )
				{
					var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-60'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-50'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-51'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-52'></命令>";
					earthAX.Connector( command );
					
					for ( var i = 0; i < typhoonData.length; i++ )
					{
						var id = -(61+i);
						command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='" + id + "' />";
						earthAX.Connector( command );
					}
				}
				else
				{
					var typhoonLine = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-60' 类型ID='18'> \
										<线> \
										 <全局属性 colorR='0.0' colorG='0.3' colorB='0.6' colorA='1.0' /> \
										 <距离线宽对> \
										  <DisLineWidth distance='0' lineWidth='2'/> \
										  <DisLineWidth distance='10000' lineWidth= '2' /> \
										  <DisLineWidth distance='100000000' lineWidth= '2' /> \
										 </距离线宽对> \
										 <点集> ";
					for ( var i = 0; i < typhoonData.length; i++ )
					{
						var typhoonPoint = typhoonData[ i ];
						typhoonLine += "<PT3D X='" + typhoonPoint.lon + "' Y='" + typhoonPoint.lat + "' Z='0' />\n";
						
						var id = -(61+i);
						var levelImage = "blue.png";
						if ( typhoonPoint.level == "强热带风暴" )
							levelImage = "yellow.png";
						else if ( typhoonPoint.level == "台风" )
							levelImage = "orange.png";
						else if ( typhoonPoint.level == "超强台风" )
							levelImage = "red.png";
						
						var pointCMD = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='" + id + "' 名字='塔里' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='"
							+ typhoonPoint.lon + "," + typhoonPoint.lat + "," + "0.0' 颜色='255,255,255,255' 资源路径='" + levelImage +"' 宽='12' 高='12' 对齐方式='CENTER' ></命令>";
						earthAX.Connector( pointCMD );

					}
					
					typhoonLine += " </点集> \
										</线> \
										</命令>";
					earthAX.Connector( typhoonLine );
					
					
					//if( i == typhoonData.length - 1 )
						{
							var typhoonPoint = typhoonData[ typhoonData.length - 1 ];
							var typhoonCMD = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-50' 名字='塔里' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='"
							+ typhoonPoint.lon + "," + typhoonPoint.lat + "," + "0.0' 颜色='255,255,255,255' 资源路径='typhoon.gif' 宽='40' 高='40' 对齐方式='CENTER' ></命令>";
							earthAX.Connector( typhoonCMD );
							
							var typhoonCircle1CMD = "<命令  命令名='创建可视对象' 企业ID='0' 对象ID='-51' 类型ID='48' 对象名称='未命名' 转换标志=' false'> \
														<贴地圆形标绘 是否贴地='false'> \
														<控制点 useActualAlt='false'>";
							typhoonCircle1CMD += "<Pt3dLLAR lon='" + typhoonPoint.lon + "' lat='" + typhoonPoint.lat + "' alt='0.0' radius='" + typhoonPoint.radius7 + "000' />";
							typhoonCircle1CMD += "</控制点>" +
													"<属性>" +
														"<点颜色 R='1' G='0' B='0.2' A='0' />" +
														"<点大小 size='0' />" +
														"<线颜色 R='0.9' G='0.6' B='0.2' A='1.0' />" +
														"<线宽度 size='1' />" +
														"<面颜色 R='0.9' G='0.6' B='0.2' A='0.4' />" +
													"</属性>" +
												"</贴地圆形标绘>" +
											"</命令>";
							earthAX.Connector( typhoonCircle1CMD );
							
							var typhoonCircle2CMD = "<命令  命令名='创建可视对象' 企业ID='0' 对象ID='-52' 类型ID='48' 对象名称='未命名' 转换标志=' false'> \
														<贴地圆形标绘 是否贴地='false'> \
														<控制点 useActualAlt='false'>";
							typhoonCircle2CMD += "<Pt3dLLAR lon='" + typhoonPoint.lon + "' lat='" + typhoonPoint.lat + "' alt='0.0' radius='" + typhoonPoint.radius10 + "000' />";
							typhoonCircle2CMD += "</控制点>" +
													"<属性>" +
														"<点颜色 R='1' G='0' B='0.2' A='0' />" +
														"<点大小 size='0' />" +
														"<线颜色 R='0.9' G='0.6' B='0.2' A='1.0' />" +
														"<线宽度 size='1' />" +
														"<面颜色 R='0.9' G='0.6' B='0.2' A='0.4' />" +
													"</属性>" +
												"</贴地圆形标绘>" +
											"</命令>";
							earthAX.Connector( typhoonCircle2CMD );
						}
					
					
					
				}
				
				FlyLine = !FlyLine;
				
			}
		}
		else if ( backValue != null && backValue.indexOf( "消息值='3'" ) != -1 )
		{
			//alert( backValue );
			
			//解析xml
			var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
			xmlDoc.async = false;
			xmlDoc.loadXML( backValue );
			
			if ( !xmlDoc )
			{
				return;
			}
			
			var root = xmlDoc.getElementsByTagName( "事件" )[0];
			
			var poltInfoEle = root.childNodes[0];
			if ( poltInfoEle == null )
			{
				return;
			}
			var polttype = poltInfoEle.getAttribute( "polttype" );
			var msgtype = poltInfoEle.getAttribute( "msgtype" );
			
			if ( msgtype != "标绘结束" )
			{
				return;
			}
			
			var finishtypeEle = poltInfoEle.childNodes[0];
			var finishtype = finishtypeEle.getAttribute( "type" );
			if ( finishtype != "完成" )
			{
				return;
			}
			
			var controlPoints = poltInfoEle.childNodes[1];
			
			if ( backValue.indexOf( "贴地圆形标绘" ) != -1 )
			{
				//缓冲区分析
				if ( controlPoints.childNodes.length < 2 )
				{
					return;
				}
				
				var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-20'></命令>";
				earthAX.Connector( command );
				
				var centerPointEle = controlPoints.childNodes[0];
				var circlePointEle = controlPoints.childNodes[1];
				
				var centerPointLon = centerPointEle.getAttribute( "lon" );
				var centerPointLat = centerPointEle.getAttribute( "lat" );
				var centerPointAlt = centerPointEle.getAttribute( "alt" );
				var circlePointLon = circlePointEle.getAttribute( "lon" );
				var circlePointLat = circlePointEle.getAttribute( "lat" );
				var circlePointAlt = circlePointEle.getAttribute( "alt" );
				
				//获得中心点三维坐标
				var EarthCommand = "<命令 命令名='获取厂区坐标' 企业ID='0' 经度='" + centerPointLon + "' 纬度='" + centerPointLat + "' 高度='" + centerPointAlt + "' />";
				var XYZResult = earthAX.Connector( EarthCommand );
				xmlDoc.loadXML( XYZResult );
				root = xmlDoc.getElementsByTagName( "命令结果" )[0];
				if ( root == null )
				{
					return;
				}
				var centerPointX = root.getAttribute( "x" );
				var centerPointY = root.getAttribute( "y" );
				var centerPointZ = root.getAttribute( "z" );
				
				//获得周边三维坐标
				EarthCommand = "<命令 命令名='获取厂区坐标' 企业ID='0' 经度='" + circlePointLon + "' 纬度='" + circlePointLat + "' 高度='" + circlePointAlt + "' />";
				XYZResult = earthAX.Connector( EarthCommand );
				xmlDoc.loadXML( XYZResult );
				root = xmlDoc.getElementsByTagName( "命令结果" )[0];
				if ( root == null )
				{
					return;
				}
				var circlePointX = root.getAttribute( "x" );
				var circlePointY = root.getAttribute( "y" );
				var circlePointZ = root.getAttribute( "z" );
				
				//计算半径
				var radius = Math.sqrt( Math.pow( circlePointX - centerPointX, 2 ) + 
										Math.pow( circlePointY - centerPointY, 2 ) +
										Math.pow( circlePointZ - centerPointZ, 2 ) );
				
				
				//缓冲区分析
				//EarthCommand = "<命令 命令名='缓冲区分析' 状态='开启' 中心点='" + [centerPointLon,centerPointLat,centerPointAlt].join( ' ' ) + "' 半径='" + radius + "' />";
				
				
				EarthCommand = "<命令  命令名='创建可视对象' 企业ID='0' 对象ID='-20' 类型ID='48' 对象名称='未命名' 转换标志=' false'> \
														<贴地圆形标绘 是否贴地='false'> \
														<控制点 useActualAlt='false'>";
				EarthCommand += "<Pt3dLLAR lon='" + centerPointLon + "' lat='" + centerPointLat + "' alt='" + centerPointAlt + "' radius='" + radius + "' />";
				EarthCommand += "</控制点>" +
										"<属性>" +
											"<点颜色 R='1' G='0' B='0.2' A='0' />" +
											"<点大小 size='0' />" +
											"<线颜色 R='0.0' G='1' B='0.0' A='1.0' />" +
											"<线宽度 size='3' />" +
											"<面颜色 R='0.9' G='0.6' B='0.2' A='0.0' />" +
										"</属性>" +
									"</贴地圆形标绘>" +
								"</命令>";
							
				//alert( EarthCommand );
				earthAX.Connector( EarthCommand );
				
				
				var openCommand = 	"<命令 命令名='贴地圆形标绘' 操作='开启'>" +
										"<显示配置>" +
										"<点大小 size='3' />" +
										"<点颜色 R='0' G='1' B='0' A='0' />" +
										"<预选点颜色 R='1' G='1' B='1' A='1' />" +
										"<线宽度 size='2' />" +
										"<线颜色 R='0' G='1' B='0' A='1' />" +
										"<预选线颜色 R='1' G='0' B='0' A='1' />" +
										"<面颜色 R='0' G='1' B='0' A='0.0' />" +
										"<预选面颜色 R='0' G='1' B='0' A='0.0' />" +
										"</显示配置>" +
										"<功能配置 是否贴地='否' />" +
										"</命令>";
				earthAX.Connector( openCommand );
			}
			else if ( backValue.indexOf( "贴地线标绘" ) != -1 )
			{
				//飞行路径
				//alert( backValue );
				
				if ( controlPoints.childNodes.length < 2 )
				{
					return;
				}
				
				var command = "<命令 命令名='FlyAlongLine' 俯仰角='-30' 高度='100' 速度='150' 开启平滑='是'>";
				for ( var i = 0; i < controlPoints.childNodes.length; i++ )
				{
					var pointEle = controlPoints.childNodes[i];
					command += "<关键点 位置='" + [ pointEle.getAttribute( "lon" ), pointEle.getAttribute( "lat" ), pointEle.getAttribute( "alt" ) ].join( ',' ) + "' />";
				}
				
				command += "</命令>";
			
				
				//window.open( "html/flightlocation.html", "flightlocation", "width=300,height=450,resizable=no,scrollbars=yes" );
				earthAX.Connector( command );
				//earthAX.PopupDragUIHtmlWindow( "flightlocation", 200, 200, 300, 300, "html/hello.html", 100 );
			}
		}
		else if ( backValue != null && backValue.indexOf( "消息值='4'" ) != -1 )	//标牌关闭
		{
			var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-30'></命令>";
			earthAX.Connector( command );
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

function highLightFeature( layerID, featureID )
{
	if ( layerID == 0 )
	{
		return;
	}
	
	var earthAX = getEarth();
	var command = "<命令 命令名='高亮矢量要素' 高亮='ON' LayerID=" + layerID + " FeatureID='" + featureID + "' />";
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
				childrenNode.iconCls= 'icon-tree-classify-node-leaf-open';
				treeObj.tree('update', childrenNode);
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
				childrenNode.iconCls= 'icon-tree-classify-node-leaf';
				treeObj.tree('update', childrenNode);
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
				obj={"id":id,"text":name,"children":null,"attributes":attributes,"state":"closed"};
			}else{//iconCls
				obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-close","state":"closed"};
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
			obj={"id":id,"text":name,"children":null,"attributes":attributes,"iconCls":"icon-tree-classify-node","state":"closed"};
		}else{//iconCls
			obj={"id":id,"text":name,"children":nodeData,"attributes":attributes,"iconCls":"icon-tree-classify-node-leaf","state":"closed"};
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
					"' Deviation='" + data.Deviation + "' Pickable='" + data.ThreeDISPickable + "' ThreeDPipe='" + data.ThreeDIS3DPipe + "' PipeRadius='" + data.PipeRadius + 
					"' Leftx='" + Leftx + "' Topy='" + Topy + "' Rightx='" + Rightx + "' Bottomy='" + Bottomy + 
					"' >\n" +
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
				"' Deviation='" + childrenData.Deviation + "' Pickable='" + childrenData.ThreeDISPickable + "' ThreeDPipe='" + childrenData.ThreeDIS3DPipe + "' PipeRadius='" + childrenData.PipeRadius + 
				"' Leftx='" + Leftx + "' Topy='" + Topy + "' Rightx='" + Rightx + "' Bottomy='" + Bottomy + 
				"' >\n" +
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
function getDlg2( url, id, name, w, h, x, y, urllist2, namelist2, level2 )
{
	urlList = urllist2;
	nameList = namelist2;
	menuLevel = level2;
	
	menu2Length = 108;
	if ( w > menu2Length )
	{
		menu2Length = w;
	}
	
	getDlg1( url, id, name, w, h, x, y );
}


function getDlg1(url, dialogid, title, w, h,l,t,modal, closable,maximizable,resizable) {
	if (!modal) {
		modal = false;
	}
	if (closable == null) {
		closable = false;
	}
	if (maximizable == null) {
		maximizable = false;
	}
	if (resizable == null) {
		resizable = false;
	}
	
	if (dlgNumber > 5) {
		$.messager.alert('提示', '最多只能同时存在五个弹出窗口！', 'info');
		dlgNumber == 5;
		return;
	}
	if(title!=''){
		h = h + 30;
	}else{
		h = h + 2;
	}
	var clientHeight = document.documentElement.clientHeight;
	var clientWidth = document.documentElement.clientWidth;
	if (h > clientHeight) {
		h = clientHeight;
	}
	if (w > clientWidth) {
		w = clientWidth;
	}
	// 弹出窗口div的id
	var dlgid = 'dlgDiv_' + dialogid;
	var dlgDiv = $("#" + dlgid);
	dlgDiv.height(h);
	dlgDiv.width(w);
	var dlgIframe = $("#iframe_" + dialogid);
	var window_mask = $("#" + dlgid + "-mask");
	if (dlgDiv.length == 0) {
		dlgDiv = $("<div></div>").appendTo("body");
		dlgDiv.attr("id", dlgid);
		dlgDiv.attr("name", "dlgDiv");
		dlgIframe = $("<iframe width=\"100%\" height=\"100%\" src=\"\" frameborder=\"0\"></iframe>").appendTo(dlgDiv);
		dlgIframe.attr("id", "iframe_" + dialogid);
		// 在窗口下面添加一个遮罩层，解决窗口被activie控件遮挡问题
		window_mask = $("<div id=\""
				+ dlgid
				+ "-mask\" class=\"window-maskDiv\"><iframe style=\"position: absolute; z-index: -1; width: 100%; height: 100%; top: 0;left:0;scrolling:no;\" frameborder=\"0\"></iframe></div>");
		window_mask.width(w);
		window_mask.height(h);
		window_mask.appendTo("body");
		dlgNumber++;
	}

	// 初始化窗口
	$('#' + dlgid).dialog({
		title : title,
		height : h,
		width : w,
		modal : modal,
		shadow : false,
		closable : closable,
		maximizable:maximizable,
		resizable:resizable,
		left:l,
		top:t,
		onMove : function(left, top) {
			if ($(this).panel('options').reSizing) {
				return;
			}
			var parentObj = $(this).panel('panel').parent();
			var width = $(this).panel('options').width;
			var height = $(this).panel('options').height;
			var right = left + width;
			var buttom = top + height;
			var parentWidth = parentObj.width();
			var parentHeight = parentObj.height();

			if (left < 0) {
				left = 0;
				$(this).panel('move', {
					left : 0
				});
			}
			if (top < 0) {
				top = 0;
				$(this).panel('move', {
					top : 0
				});
			}

			if (parentObj.css("overflow") == "hidden") {
				var inline = $.data(this, "window").options.inline;
				if (inline == false) {
					parentObj = $(window);
				}
				if (left > parentObj.width() - width) {
					left = parentObj.width() - width;
					$(this).panel('move', {
						"left" : parentObj.width() - width
					});
				}
				if (top > parentObj.height() - height) {
					top = parentObj.height() - height;
					$(this).panel('move', {
						"top" : parentObj.height() - height
					});
				}
			}
			window_mask.css('left', left);
			window_mask.css('top', top);
		},
		onClose : function() {
			$('#' + dlgid).dialog('destroy');
			$('#' + dlgid).attr("closeFlag", "");
			window_mask.remove();
			dlgNumber = dlgNumber - 1;
		},
		onResize:function(width, height){
			var left = $(this).panel('options').left;
			var top = $(this).panel('options').top;
			window_mask.css('left', left);
			window_mask.css('top', top);
			window_mask.css('width', width);
			window_mask.css('height', height);
		},
		onMinimize : function() {
			window_mask.css('display', 'none');
		},
		onOpen : function() {
			window_mask.css('display', 'block');
			var window_z_index = $(this).panel("panel").css('z-index');
			window_mask.css('z-index', window_z_index - 1);
		}
	});
//	$('.panel .window ').removeClass("window").addClass("window1");
	// 为属性closeFlag赋值
	$('#' + dlgid).attr("closeFlag", dialogid);
	dlgIframe[0].contentWindow.location.href = url;
	if(title==''){
		$('body').find('div.window').removeClass("window").addClass("window1");
		//$('body').find('div.window1').width(w);	//使用w会使得1级菜单显示不全
		$('body').find('div.window1').width(menu1Length>menu2Length?menu1Length:menu2Length);	//选择最大值，否则会造成1级菜单或2级菜单显示不全
		$('#'+dlgid).width(w);
		$('#'+dlgid).height(h);
		$("#"+dlgid).find('div.dialog-content').width(w);
		$("#"+dlgid).find('div.dialog-content').height(h);
		$("#"+dlgid).find('div.panel').width(w);
	}
}
function closeDlg(dialogid) {
	var dlgid = 'dlgDiv_' + dialogid;
	var dlgDiv = $("#" + dlgid);
	if (dlgDiv.length != 0) {
		dlgDiv.dialog('close');
	}
	
	if ( dialogid == "rightMenu" )	//关闭时还原
	{
		closeDlg( "baseMenu" );
		level2type = 0;
		
		unHighLightFeature( curHighLayerID );
		curHighLayerID = 0;
	}

}