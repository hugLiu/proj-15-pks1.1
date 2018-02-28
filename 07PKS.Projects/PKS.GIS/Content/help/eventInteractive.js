
var positionDataXml = "<document><Node lon='1' lat='2' alt='3' /><Node lon='4' lat='5' alt='6' /><Node lon='7' lat='8' alt='9' /></document>";

function RegInteractive(recallFunc)
{

	var earthAX = getEarth();
	earthAX.RegCallback( parseInt( "0x00000800", 16 ), callbackScriptJAS, parseInt( "0xffffffff", 16 ) );
	
	var LonLat = false;
	var Clouds = false;
	var Compass = false;
	var BufferAnalysis = false;
	var FlyLine = false;
	var TerrainCut = false;
	var Wake = false;
	var Rotor = false;
	var Typhoon = false;
	var Navigation = false;
	var Alarm = false;
	var OceanVisible = false;
	var Switch2D = false;
	
	var intervalAddAlarmCircle;
	var intervalDeleteAlarmCircle;

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
				
				if ( Clouds ) {
					var url = getURL() + urlLoc + "html/clouds.html";
					earthAX.PopupDragUIHtmlWindow( "clouds", 200, 200, 400, 120, url, 100, false );
					setHtmlWindowDragRect( "clouds", 0, 0, 400, 30 );
				} else {
					earthAX.CloseWindow( "clouds" );
				}

			}
			else if ( itemidentify == "Compass" )
			{
				var showFlag = Compass ? "OFF" : "ON";
				//earthAX.Connector( "<命令 命令名='导航面板配置' ><大小比例 scale='0.8'/><位置 topMargin='32' rightMargin='32'/><显隐 state='" + showFlag + "' /></命令>" );
				earthAX.Connector( "<命令 命令名='挂件显隐' 类型='CONTROLPANEL' 显隐='" + showFlag + "' />" )
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
										"<点颜色 R='0' G='255' B='0' A='0' />" +
										"<预选点颜色 R='255' G='255' B='255' A='255' />" +
										"<线宽度 size='2' />" +
										"<线颜色 R='0' G='255' B='0' A='255' />" +
										"<预选线颜色 R='255' G='0' B='0' A='255' />" +
										"<面颜色 R='0' G='255' B='0' A='75' />" +
										"<预选面颜色 R='0' G='255' B='0' A='75' />" +
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
					//停止飞行
					var command = "<命令 命令名='FlyAlongLine' 播放类型='停止' />";
					earthAX.Connector( command );
					
					command = "	<命令 命令名='贴地线标绘' 操作='关闭' />";
					earthAX.Connector( command );
				}
				else
				{
					var openCommand = "<命令 命令名='贴地线标绘' 操作='开启'> \
									  <显示配置> \
									  <是否贴地 贴地='NO' /> \
									  <点大小 size='3' /> \
									  <点颜色 R='0' G='255' B='0' A='255' /> \
									  <预选点颜色 R='255' G='255' B='255' A=255' /> \
									  <线宽度 size='3' /> \
									  <线颜色 R='255' G='0' B='0' A='255' /> \
									  <预选线颜色 R='255' G='0' B='0' A='255' /> \
									  </显示配置> \
									  <功能配置 预选点消息='是' 中间点消息='是' /> \
									  </命令>";
					earthAX.Connector( openCommand );
				}
				
				FlyLine = !FlyLine;
				
			}
			else if ( itemidentify == "Typhoon" )
			{
				moveLightSwitch = Typhoon;
				if (Typhoon)
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
										 <全局属性 colorR='0.0' colorG='75' colorB='150' colorA='1.0' /> \
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
						
						var pointCMD = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='" + id + "' 名称='塔里' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='"
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
							var typhoonCMD = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-50' 名称='塔里' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='"
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
				
				Typhoon = !Typhoon;
				
			}
			else if ( itemidentify == "Ocean" )
			{
				var url = getURL() + urlLoc + "html/ocean.html";
				earthAX.PopupDragUIHtmlWindow( "ocean", 200, 200, 380, 300, url, 100, false );
				setHtmlWindowDragRect( "ocean", 0, 0, 380, 30 );
			}
			else if ( itemidentify == "OceanVisible" )
			{
				var showFlag = OceanVisible ? "OFF" : "ON";
				earthAX.Connector( "<命令 命令名='海洋显隐' 状态='" + showFlag +　"' />" );
				OceanVisible = !OceanVisible;
			}
			else if ( itemidentify == "Wake" )
			{
				if ( Wake )
				{
					//停止
					var command = "	<命令 命令名='贴地线标绘' 操作='关闭' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='显示对象运动' 播放类型='停止' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='海洋轨迹' 状态='删除' ID='-5' sprayEffects='true' length='10.0' beamWidth='10.0' bowWave='false' bowSprayOffset='10.0' bowWaveOffset='10.0' />"
					earthAX.Connector( command );
					
					command = "<命令 命令名='海洋摇晃' 状态='删除' 偏移='5.0' ID='-5' />";
					earthAX.Connector( command );
				
					command = "<命令 命令名='对象管理'  操作类型='移除' 对象ID='-5' />"
					earthAX.Connector( command );
					
				}
				else
				{
					var command = "<命令 命令名='贴地线标绘' 操作='开启'> \
									  <显示配置> \
									  <是否贴地 贴地='NO' /> \
									  <点大小 size='3' /> \
									  <点颜色 R='0' G='255' B='0' A='255' /> \
									  <预选点颜色 R='255' G='255' B='255' A='255' /> \
									  <线宽度 size='3' /> \
									  <线颜色 R='0' G='255' B='0' A='255' /> \
									  <预选线颜色 R='0' G='255' B='0' A='255' /> \
									  </显示配置> \
									  <功能配置 预选点消息='是' 中间点消息='是' /> \
									  </命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-5' 类型ID='1' 对象名称='boat' 转换标志='false'> \
									<模型 文件名='ship.ive' 加载方式='0' 半径='177.6980' 加载时长='3.000000' 加载条件='1' 半径比例='100.000000' > \
									  <Pt3d x='120.61288341967' y='39.79925826971' z='5'>位置</Pt3d> \
									  <Pt3f x='180.000000' y='0.000000' z='0'>角度姿态</Pt3f> \
									  <Pt3f x='0.3' y='0.3' z='0.3'>缩放</Pt3f> \
									</模型> \
								</命令>"
					earthAX.Connector( command );
					
					command = "<命令 命令名='海洋摇晃' 状态='添加' 偏移='0.0' ID='-5' />";
					earthAX.Connector( command );
				}
				
				Wake = !Wake;
			}
			else if ( itemidentify == "Rotor" )
			{
				if ( Rotor )
				{
					//停止
					var command = "	<命令 命令名='贴地线标绘' 操作='关闭' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='显示对象运动' 播放类型='停止' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='海洋涡轮' 状态='删除' ID='1094359' />"
					earthAX.Connector( command );
					
				}
				else
				{
					var command = "<命令 命令名='贴地线标绘' 操作='开启'> \
									  <显示配置> \
									  <是否贴地 贴地='NO' /> \
									  <点大小 size='3' /> \
									  <点颜色 R='0' G='255' B='0' A='255' /> \
									  <预选点颜色 R='255' G='255' B='255' A='255' /> \
									  <线宽度 size='3' /> \
									  <线颜色 R='255' G='255' B='0' A='255' /> \
									  <预选线颜色 R='255' G='255' B='0' A='255' /> \
									  </显示配置> \
									  <功能配置 预选点消息='是' 中间点消息='是' /> \
									  </命令>";
					earthAX.Connector( command );
					
				}
				
				Rotor = !Rotor;
			}
			else if ( itemidentify == "TerrainCut" )
			{
				var terrainCutCmd = "";
				if ( TerrainCut )
				{
					terrainCutCmd += "<命令 命令名='剖切地形' 深度='' 贴图重复率='10' 显示底框='是' 启用='否' 返回结果='否' > \
											<剖切点 X='117' Y='39' Z='0' /> \
										</命令>";
				}
				else
				{
					terrainCutCmd += "<命令 命令名='事件处理器管理' 操作='启用处理器' 处理器名称='LandCutHandler'></命令>";
				}
				earthAX.Connector( terrainCutCmd );
				TerrainCut = !TerrainCut;
			}
			else if ( itemidentify == "Animation" )
			{
				var showAnimationCmd = "<命令 命令名='动画' 类型='播放控制' 脚本名=''> \
											<播放器信息 X='0' Y='20' 显示='true' /> \
										</命令>";
				earthAX.Connector( showAnimationCmd );
			}
			else if ( itemidentify == "Weather" )
			{
				var url = getURL() + urlLoc + "html/weather.html";
				earthAX.PopupDragUIHtmlWindow( "weather", 200, 200, 380, 350, url, 100, false );
				setHtmlWindowDragRect( "weather", 0, 0, 380, 30 );
			}
			else if ( itemidentify == "Cmd" )
			{
				var url = getURL() + urlLoc + "html/cmd.html";
				//getDlg(url, "namelocationDlg", "名称定位", 270, 330, true);
				earthAX.PopupDragUIHtmlWindow( "usercmd", 200, 200, 260, 280, url, 100, false );
				setHtmlWindowDragRect( "usercmd", 0, 0, 260, 30 );
			}
			else if ( itemidentify == "TerrainAlpha" )
			{
				var url = getURL() + urlLoc + "html/terrainAlpha.html";
				earthAX.PopupDragUIHtmlWindow( "terrainAlpha", 200, 200, 400, 120, url, 100, false );
				setHtmlWindowDragRect( "terrainAlpha", 0, 0, 400, 30 );
			}
			else if ( itemidentify == "Help" )
			{
				
			}
			else if ( itemidentify == "About" )
			{
				var url = getURL() + urlLoc + "html/about.html";
				earthAX.PopupDragUIHtmlWindow( "about", 200, 200, 260, 80, url, 100, false );
				setHtmlWindowDragRect( "about", 0, 0, 260, 30 );
			}
			else if ( itemidentify == "Navigation" )
			{
				var showFlag = Navigation ? "OFF" : "ON";
				earthAX.Connector( "<命令 命令名='挂件显隐' 类型='NAVIGATIONMAP' 显隐='" + showFlag + "' />" );
				Navigation = !Navigation;
			}
			else if ( itemidentify == "Alarm" )
			{
				if ( Alarm )//停止
				{
					clearInterval( intervalAddAlarmCircle );
					
					var command = "	<命令 命令名='贴地线标绘' 操作='关闭' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='显示对象运动' 播放类型='停止' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-6'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-7'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-8'></命令>";
					earthAX.Connector( command );
					
					
					//return;
					//clearInterval( intervalAddAlarmCircle );
					//clearInterval( intervalDeleteAlarmCircle );
					//setTimeout( deleteAlarmCircle, 500 );
					
					//command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-6'></命令>";
					//earthAX.Connector( command );
					
				}
				else		//启动
				{
					var command = "<命令 命令名='贴地线标绘' 操作='开启'> \
									  <显示配置> \
									  <是否贴地 贴地='NO' /> \
									  <点大小 size='3' /> \
									  <点颜色 R='0' G='255' B='0' A='255' /> \
									  <预选点颜色 R='255' G='255' B='255' A='255' /> \
									  <线宽度 size='3' /> \
									  <线颜色 R='0' G='255' B='0' A='255' /> \
									  <预选线颜色 R='0' G='255' B='0' A='255' /> \
									  </显示配置> \
									  <功能配置 预选点消息='是' 中间点消息='是' /> \
									  </命令>";
					earthAX.Connector( command );
					
					
					command = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-6' 类型ID='1' 对象名称='ship' 转换标志='false'> \
								<模型 文件名='ship.ive' 加载方式='0' 半径='177.6980' 加载时长='3.000000' 加载条件='1' 半径比例='100.000000' > \
									<Pt3d x='0' y='0' z='0'>位置</Pt3d> \
									<Pt3f x='180.000000' y='0.000000' z='0'>角度姿态</Pt3f> \
									<Pt3f x='1' y='1' z='1'>缩放</Pt3f> \
								</模型> \
							</命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-7' 类型ID='1' 对象名称='ship' 转换标志='false'> \
								<模型 文件名='jingshi.ive' 播放动画='1' 加载方式='0' 半径='177.6980' 加载时长='3.000000' 加载条件='0' 半径比例='10000.000000' > \
									<Pt3d x='0' y='0' z='0'>位置</Pt3d> \
									<Pt3f x='180.000000' y='0.000000' z='0'>角度姿态</Pt3f> \
									<Pt3f x='30' y='30' z='1'>缩放</Pt3f> \
								</模型> \
							</命令>";
					earthAX.Connector( command );
					
					
					setTimeout( function(){
						command = "<命令 命令名='显示对象基本动作'  ID='-6,-7' 动作类型='显隐'  参数='0' />";
						earthAX.Connector( command );
						
					}, 1000 );
					
				}
				Alarm = !Alarm;
			}
			else if ( itemidentify == "Location" )
			{
				var url = getURL() + urlLoc + "html/location.html";
				earthAX.PopupDragUIHtmlWindow( "location", 200, 200, 500, 350, url, 100, false );
				setHtmlWindowDragRect( "location", 0, 0, 500, 30 );
			}
			else if ( itemidentify == "Switch2D" )
			{
				if ( Switch2D ) //删除
				{
					var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-90'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-91'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-92'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-93'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-94'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-95'></命令>";
					earthAX.Connector( command );
					
					command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-96'></命令>";
					earthAX.Connector( command );
				}
				else	//添加标注
				{
					var switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-90' 名称='187' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='120,38.7,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
					
					switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-91' 名称='192' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='123,38,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
					
					switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-92' 名称='188' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='125,29,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
					
					switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-93' 名称='191' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='123,35,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
				
					switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-94' 名称='193' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='119.5,23.5,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
				
					switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-95' 名称='189' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='115.5,21,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
				
					switch2DMark = "<命令 命令名='对象管理' 操作类型='添加' 元素类型='屏幕标注' 企业ID='0' 对象ID='-96' 名称='184' 内容='' 字体='SIMSUN.TTF' 字号='40' 位置='108.5,20,0.0' 颜色='255,255,255,255' 资源路径='switch.png' 宽='24' 高='32' 对齐方式='CENTER' ></命令>";
					earthAX.Connector( switch2DMark );
				}
				
				Switch2D = !Switch2D;
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
				
				//var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-20'></命令>";
				//earthAX.Connector( command );
				
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
										
				var url = getURL() + urlLoc + "html/buffer.html?lon=" + centerPointLon + "&lat=" + centerPointLat + "&alt=" + centerPointAlt + "&radius=" + radius;
				earthAX.PopupDragUIHtmlWindow( "buffer", 200, 200, 300, 350, url, 100, false );
				setHtmlWindowDragRect( "buffer", 0, 0, 300, 30 );
				
				
				EarthCommand = "<命令  命令名='创建可视对象' 企业ID='0' 对象ID='-20' 类型ID='48' 对象名称='未命名' 转换标志=' false'> \
														<贴地圆形标绘 是否贴地='false'> \
														<控制点 useActualAlt='false'>";
				EarthCommand += "<Pt3dLLAR lon='" + centerPointLon + "' lat='" + centerPointLat + "' alt='" + centerPointAlt + "' radius='" + radius + "' />";
				EarthCommand += "</控制点>" +
										"<属性>" +
											"<点颜色 R='255' G='0' B='50' A='0' />" +
											"<点大小 size='0' />" +
											"<线颜色 R='0.0' G='255' B='0.0' A='255' />" +
											"<线宽度 size='3' />" +
											"<面颜色 R='225' G='150' B='50' A='0.0' />" +
										"</属性>" +
									"</贴地圆形标绘>" +
								"</命令>";
				earthAX.Connector( EarthCommand );
				
				//缓冲区分析
				//EarthCommand = "<命令 命令名='缓冲区分析' 状态='开启' 中心点='" + [centerPointLon,centerPointLat,centerPointAlt].join( ' ' ) + "' 半径='" + radius + "' />";
				//alert( EarthCommand );
				//earthAX.Connector( EarthCommand );
				
			}
			else if ( backValue.indexOf( "贴地线标绘" ) != -1 )
			{
				//飞行路径
				//alert( backValue );
				
				if ( controlPoints.childNodes.length < 2 )
				{
					return;
				}
				
//				var command = "<命令 命令名='FlyAlongLine' 俯仰角='-30' 高度='100' 速度='150' 开启平滑='是'>";
//				for ( var i = 0; i < controlPoints.childNodes.length; i++ )
//				{
//					var pointEle = controlPoints.childNodes[i];
//					command += "<关键点 位置='" + [ pointEle.getAttribute( "lon" ), pointEle.getAttribute( "lat" ), pointEle.getAttribute( "alt" ) ].join( ',' ) + "' />";
//				}
//				
//				command += "</命令>";
//				
//				earthAX.Connector( command );
			
				
				//window.open( "html/flightlocation.html", "flightlocation", "width=300,height=450,resizable=no,scrollbars=yes" );
				
				if ( FlyLine )
				{
					var controlPointsXML = "<document>\n"
					for ( var i = 0; i < controlPoints.childNodes.length; i++ )
					{
						var pointEle = controlPoints.childNodes[ i ];
						controlPointsXML += "<Node lon=\"" + pointEle.getAttribute( "lon" ) + "\" lat=\"" + pointEle.getAttribute( "lat" ) + "\" alt=\"" + pointEle.getAttribute( "alt" ) + "\" />\n";
					}
					controlPointsXML += "</document>";
					var tempCMD = "<命令 命令名='临时存取' 状态='存' 内容='" + controlPointsXML + "' />";
					earthAX.Connector( tempCMD );
					
					
					
					var url = getURL() + urlLoc + "html/flightlocation.html";
					earthAX.PopupDragUIHtmlWindow( "flightlocation", 200, 200, 370, 350, url, 100, false );
					setHtmlWindowDragRect( "flightlocation", 0, 0, 370, 30 );
				}
				else if ( Wake )
				{
					command = "<命令 命令名='海洋轨迹' 状态='添加' ID='-5' sprayEffects='true' length='10.0' beamWidth='10.0' bowWave='false' bowSprayOffset='10.0' bowWaveOffset='10.0' />"
					earthAX.Connector( command );
					
					var command = "<命令 命令名='显示对象运动' > \
									<路径平滑 启用='是' /> \
									<显示对象 对象ID='-5' 速度='5' 起始点='0' >";

					for ( var i = 0; i < controlPoints.childNodes.length; i++ )
					{
						var pointEle = controlPoints.childNodes[i];
						command += "<位置信息 经度='" + pointEle.getAttribute( "lon" ) + "' 纬度='" + pointEle.getAttribute( "lat" ) + "' 高度='5' 时间='0' />";
					}
					
					command += "</显示对象></命令>";
					
					earthAX.Connector( command );
				}
				else if ( Rotor )
				{
					command = "<命令 命令名='海洋涡轮' 状态='添加' ID='1094359' 直径='30.0' />"
					earthAX.Connector( command );
					
					var command = "<命令 命令名='显示对象运动' > \
									<路径平滑 启用='是' /> \
									<显示对象 对象ID='1094359' 速度='5' 起始点='0' >";

					for ( var i = 0; i < controlPoints.childNodes.length; i++ )
					{
						var pointEle = controlPoints.childNodes[i];
						command += "<位置信息 经度='" + pointEle.getAttribute( "lon" ) + "' 纬度='" + pointEle.getAttribute( "lat" ) + "' 高度='10' 时间='0' />";
					}
					
					command += "</显示对象></命令>";
					
					earthAX.Connector( command );
				}
				else if ( Alarm )
				{
					var shipLine = "<命令 命令名='创建可视对象' 企业ID='0' 对象ID='-8' 类型ID='18'> \
										<线> \
										 <全局属性 colorR='0.0' colorG='255' colorB='0.0' colorA='255' /> \
										 <距离线宽对> \
										  <DisLineWidth distance='0' lineWidth='2'/> \
										  <DisLineWidth distance='10000' lineWidth= '2' /> \
										  <DisLineWidth distance='100000000' lineWidth= '2' /> \
										 </距离线宽对> \
										 <点集> ";
					for ( var i = 0; i < controlPoints.childNodes.length; i++ )
					{
						var pointEle = controlPoints.childNodes[i];
						shipLine += "<PT3D X='" + pointEle.getAttribute( "lon" ) + "' Y='" + pointEle.getAttribute( "lat" ) + "' Z='5' />\n";
					}
					
					shipLine += " </点集> \
										</线> \
										</命令>";
					earthAX.Connector( shipLine );
					
					
					var command = "<命令 命令名='显示对象基本动作'  ID='-6' 动作类型='显隐'  参数='1' />";
					earthAX.Connector( command );
					
					command = "<命令 命令名='显示对象运动' > \
									<路径平滑 启用='是' /> \
									<显示对象 对象ID='-6' 速度='100' 起始点='0' >";
									
					var command1 = "<命令 命令名='显示对象运动' > \
									<路径平滑 启用='是' /> \
									<显示对象 对象ID='-7' 速度='100' 起始点='0' >";

					for ( var i = 0; i < controlPoints.childNodes.length; i++ )
					{
						var pointEle = controlPoints.childNodes[i];
						command += "<位置信息 经度='" + pointEle.getAttribute( "lon" ) + "' 纬度='" + pointEle.getAttribute( "lat" ) + "' 高度='5' 时间='0' />";
						command1 += "<位置信息 经度='" + pointEle.getAttribute( "lon" ) + "' 纬度='" + pointEle.getAttribute( "lat" ) + "' 高度='5' 时间='0' />";
					}
					
					command += "</显示对象></命令>";
					command1 += "</显示对象></命令>";
					
					earthAX.Connector( command );
					earthAX.Connector( command1 );
					
					
					intervalAddAlarmCircle = setInterval( updateAlarmCircle, 100 );
					
					
				}
			}
		}
		else if ( backValue != null && backValue.indexOf( "消息值='4'" ) != -1 )	//标牌关闭
		{
			var command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-30'></命令>";
			earthAX.Connector( command );
		}
		
		
	}
	
	function updateAlarmCircle()
	{
		var getPosCmd = "<命令 命令名='获取对象经纬高' ID='-6'></命令>";
		var result = earthAX.Connector( getPosCmd );
		
		var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
		xmlDoc.async = false;
		xmlDoc.loadXML( result );
		
		if ( !xmlDoc )
		{
			return;
		}
		
		var root = xmlDoc.getElementsByTagName( "返回信息" )[0];
		var lon = root.getAttribute( "经度" );
		var lat = root.getAttribute( "纬度" );
		var hei = root.getAttribute( "高程" );
		


		
		var longPipeCmd = "<命令 命令名='点到管线投影' 经度='" + lon + "' 纬度='" + lat + "' 计算方式='点距离' ></命令>";
		result = earthAX.Connector( longPipeCmd );
		xmlDoc.loadXML( result );
		
		if ( !xmlDoc )
		{
			return;
		}
		root = xmlDoc.getElementsByTagName( "显示对象" )[0];
		if ( !root )
		{
			return;
		}
		
		var longDis = root.getAttribute( "距离" );

		if ( longDis > 500 ) {
			var command = "<命令 命令名='显示对象基本动作'  ID='-7' 动作类型='显隐'  参数='0' />";
			earthAX.Connector( command );
		}
		else {
			var command = "<命令 命令名='显示对象基本动作'  ID='-7' 动作类型='显隐'  参数='1' />";
			earthAX.Connector( command );
		}
	}
		
	function addAlarmCircle()
	{
		var getPosCmd = "<命令 命令名='获取对象经纬高' ID='-5'></命令>";
		var result = earthAX.Connector( getPosCmd );
		
		var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
		xmlDoc.async = false;
		xmlDoc.loadXML( result );
		
		if ( !xmlDoc )
		{
			return;
		}
		
		var root = xmlDoc.getElementsByTagName( "返回信息" )[0];
		var lon = root.getAttribute( "经度" );
		var lat = root.getAttribute( "纬度" );
		var hei = root.getAttribute( "高程" );
		
		var updateShipMark = "<命令 命令名='更新属性' ID='-6'> \
								<屏幕标注> \
									<空间位置 X='" + lon + "' Y='" + lat + "' Z='" + hei + "' /> \
									<图片大小 Type='自定义' Width='64' Height='32' /> \
								</屏幕标注> \
							</命令>";
		earthAX.Connector( updateShipMark );

		
		var longPipeCmd = "<命令 命令名='点到管线投影' 经度='" + lon + "' 纬度='" + lat + "' 计算方式='点距离' ></命令>";
		result = earthAX.Connector( longPipeCmd );
		xmlDoc.loadXML( result );
		
		if ( !xmlDoc )
		{
			return;
		}
		root = xmlDoc.getElementsByTagName( "显示对象" )[0];
		
		var longDis = root.getAttribute( "距离" );
		if ( longDis > 500 )
		{
			return;
		}
		
		
		var alarmCircle = "<命令  命令名='创建可视对象' 企业ID='0' 对象ID='-53' 类型ID='48' 对象名称='未命名' 转换标志='false'> \
														<贴地圆形标绘 是否贴地='false'> \
														<控制点 useActualAlt='false'>";
		alarmCircle += "<Pt3dLLAR lon='" + lon + "' lat='" + lat + "' alt='" + hei + "' radius='500' />";
		alarmCircle += "</控制点>" +
								"<属性>" +
									"<点颜色 R='1' G='0' B='0.2' A='0' />" +
									"<点大小 size='0' />" +
									"<线颜色 R='1.0' G='0.0' B='0.0' A='0.4' />" +
									"<线宽度 size='1' />" +
									"<面颜色 R='1.0' G='0.0' B='0.0' A='0.4' />" +
								"</属性>" +
							"</贴地圆形标绘>" +
						"</命令>";
		earthAX.Connector( alarmCircle );
	}

	function deleteAlarmCircle()
	{
		command = "<命令 命令名='对象管理' 操作类型='移除' 对象ID='-53'></命令>";
		earthAX.Connector( command );
	}

}
