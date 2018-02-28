/**
 * 输入div的显隐控制 中盈高科
 */

var id01 = null;
/*******************************************************************************
 * 根据输入div id显示输入框的实现方法 id：输入div的id titelname:输入框的题目
 */
function test(id, titlename) {
	if (id01 != null) {
		// alert(id01);
		$(id01).hide();
	}
	$('#' + id).show();
	document.getElementById(id + "01").innerHTML = titlename;
	id01 = "#" + id;
}
/*******************************************************************************
 * 没有用了
 */
function arrTojson(arr) {
	var i, jsonstr;
	var arrt = "name";
	jsonstr = "[{";
	for (i = 0; i < arr.length; i++) {
		jsonstr += "\"" + arrt + "\"" + ":" + "\"" + arr[i] + "\"},{";
	}
	jsonstr = jsonstr.substring(0, jsonstr.lastIndexOf(','));
	jsonstr += "]";
	return jsonstr;
}
/*******************************************************************************
 * 根据连接的按钮id显示对应的输入框 注：1. 简写太长的标题 2.
 * 设计输入div的id为接口名连接id后加01。通过接口名超连接的id找到输入框div的id。输入框id=连接id+01.
 */
function selectDialog(obj) {
	var textname = obj.getAttribute("id");

	var textname01 = $('#' + textname).text();
	if (textname01 == "坐标转换(西安80坐标和WGS84坐标)") {
		textname01 = "坐标转换";
	}
	if (textname01 == "世界与厂区坐标转换(世界到厂区)") {
		textname01 = "世界到厂区";
	}
	if (textname01 == "世界与厂区坐标转换(厂区到世界)") {
		textname01 = "厂区到世界";
	}
	if (textname01 == "获取屏幕坐标对应经纬高") {
		textname01 = "屏幕坐标-经纬高";
	}
	if (textname01 == "获取指定点到长距离管线的投影坐标") {
		textname01 = "点到管线的投影坐标";
	}
	if (textname01 == "暂停、启动三维渲染线程") {
		textname01 = "暂停/启动渲染线程";
	}
	if (textname01 == "根据二维地图比例设置三维相机的位置姿态") {
		textname01 = "设置相机的位姿";
	}
	if (textname01 == "根据模型名称查询模型ID") {
		textname01 = "模型名称-模型ID";
	}
	test(textname + "01", textname01);
}

/*******************************************************************************
 * 光照位置
 */
function d3(obj) {
	var paramer = "js参数说明：\r\n" + "对象ID\r\n"
			+ "type 光照对象，对象分为地球 和厂区，说明是配置地球的光照位置还是厂区的光照位置\r\n"
			+ "locationX 经度，设置光源的位置，经度(双精度浮点型)\r\n"
			+ "locationY 纬度，设置光源的位置，纬度(双精度浮点型)\r\n"
			+ "locationZ 高度，设置光源的位置，高度(双精度浮点型)";
	parent.getMethodInfo('systemIJas.js',
			'lightLocation(ID,type,locationX,locationY,locationZ)', '',
			'通过选择光照对象和位置（经纬高），来显示光照位置', paramer);
	selectDialog(obj);

}

/*******************************************************************************
 * 暂停、启动三维渲染线程
 */
function xiancheng(obj) {
	var paramer = "js参数说明：\r\n"
			+ "Flag （类型  ON-开启三维渲染  OFF-暂停三维渲染），开启三维渲染 /暂停三维渲染";
	parent.getMethodInfo('systemIJas.js', 'drawControl(Flag)', '',
			'启动/暂停三维渲染线程', paramer);
	selectDialog(obj);
}

/*******************************************************************************
 * 坐标系切换
 */
function h4(obj) {
	var coord111 = "js参数说明：\r\n" + "type 动作类型：WGS84/XIAN80 目标坐标系的类型";
	parent.getMethodInfo('systemIJas.js', 'coordSwitch(type)', '', '坐标系切换',
			coord111);
	selectDialog(obj);
}

/**
 * 光照的设置与查询
 */
function LightSettingAndSearch(obj) {
	var paramer = "js参数说明：\r\n" + "T 命令类型，区别是光照查询还是关照设置 \r\n"
			+ "LT 光照类型，区别是地球光照还是厂区光照\r\n" + "LP 光照属性，设置关照属性，例如：光照属性=“漫反射”\r\n"
			+ "lightColor 光照颜色，光照颜色为16进制颜色。例如：#FFCC00\r\n"
			+ "lightColorA 透明度，透明度范围为0到100";
	parent.getMethodInfo('systemIJas.js',
			'lightSet(T,LT,LP,lightColor,lightColorA)', '', '对光照进行设置和查询',
			paramer);
	selectDialog(obj);
}

/**
 * 天气控制
 */
function WEATHER(obj) {
	var paramer = "js参数说明：\r\n" + "X 气候：雨、雪\r\n" + "Y 状态:无、小、中、大";
	parent.getMethodInfo('systemIJas.js', 'weather(X,Y)', '', '控制三维场景中天气状态',
			paramer);
	selectDialog(obj);
}

/**
 * 挂件显隐
 */
function WIDGET(obj) {
	var paramer = "js参数说明：\r\n"
			+ "X 类型：HINTBAR（状态栏）、NAVIGATIONMAP（导航图）、CONTROLPANEL（控制面板）、FOCUSCROSS（焦点十字）\r\n"
			+ "Y 显隐的状态，设置（ON/OFF）";
	parent.getMethodInfo('systemIJas.js', 'widget(X,Y)', '', '用于查询某一个挂件的是否开启',
			paramer);
	selectDialog(obj);
}

/**
 * 查询挂件状态
 */
function WIDGETSTATUS(obj) {
	var paramer = "js参数说明：\r\n"
			+ "n 类型：HINTBAR（状态栏）、NAVIGATIONMAP（导航图）、CONTROLPANEL（控制面板）、FOCUSCROSS（焦点十字）";
	parent.getMethodInfo('systemIJas.js', 'widgetStatus(n)', '',
			'用于查询某一个挂件的是否开启', paramer);
	selectDialog(obj);
}

/**
 * 国际化
 */
function intontroalControl(obj) {
	var paramer = "js参数说明：\r\n" + "language 所要设置的语言类型，英文|中文";
	parent.getMethodInfo('systemIJas.js', 'languageControl(language)', '',
			'地球上文字国际化', paramer);
	selectDialog(obj);
}

/**
 * 启动地球前设置语言
 */

function setLanguage(obj) {
	var paramer = "js参数说明：\r\n" + "language 所要设置的语言类型，英文|中文";
	parent.getMethodInfo('systemIJas.js', 'setLanguageType(language)', '',
			'启动地球前设置语言', paramer);
	selectDialog(obj);
}

/**
 * 获取企业信息列表
 */
function popBusinessList(obj) {
	var paramer = "js参数说明：\r\n"
			+ "ID 企业ID，获取由该ID指定的对象所属的企业的信息；如果“对象ID”为０，则返回所有企业的信息列表";
	parent.getMethodInfo('sceneIJas.js', 'getEnterpriseMessage(ID)', '',
			'查询场站中企业信息列表', paramer);
	selectDialog(obj);
}
/**
 * 返回企业分组列表
 * 
 */
function popBusinessGroup(obj){
selectDialog(obj);
}

/**
 * 获取矢量图层元素信息
 */
function vector1(obj) {
	var paramer = "js参数说明：\r\n" + "content 内容，分为层名和个数\r\n"
			+ "location 位置，分为全部和位置";
	parent.getMethodInfo('sceneIJas.js',
			'returnVectorPicElementList(content,location)', '', '获取矢量图层信息',
			paramer);
	selectDialog(obj);
}

/**
 * 对内容进行筛选
 */
function autoSelect() {
	var selectedStr = $('#picContent option:selected').text();
	if (selectedStr == '个数') {
		var picNum = document.getElementById("picNum");
		picNum.disabled = true;
		picNum.value = "";
	} else if (selectedStr == '层名') {
		var picNum = document.getElementById("picNum");
		picNum.disabled = false;
		var list = parent.returnVectorPicElementList("个数", "");
		// var list = 8;
		for ( var i = 1; i <= list; i++) {
			// alert(i);
			$("#picNum").append(" <option>" + i + " </option>");

		}
	}
}

/**
 * 矢量图层控制
 */
function vectorCon(obj) {
	var paramer = "js参数说明：\r\n" + "vectorName 矢量图层名称\r\n"
			+ "vectorHideOrShow 类型，显示或隐藏";
	parent.getMethodInfo('sceneIJas.js',
			'ectorPicControl(vectorName,vectorHideOrShow)', '', '矢量图层控制',
			paramer);
	selectDialog(obj);
	var list = parent.returnVectorPicElementList("层名", "全部");
	// alert(list);
	// var list = "生活;学习; ";
	list = list.split(";");
	for ( var i = 0; i < list.length - 1; i++) {
		// alert(list[i]);
		$("#vectorName").append(" <option>" + list[i] + " </option>");
	}

}

/**
 * 返回图层元素列表
 */
function d1(obj) {
	var paramer = "js参数说明：\r\n"
			+ "ID 图层ID，若指定此参数，则返回此ID下的所有图层元素，否则返回所有图层下的图层元素。例如：ID=”22”";
	parent.getMethodInfo('sceneIJas.js', 'returnPicElementList(ID)', '',
			'查找出所有所属图层ＩＤ为输入ID的设备', paramer);
	selectDialog(obj);
}

/**
 * 获取对象信息列表 弹出框
 */
function popObjectSix(obj) {
	var paramer = "js参数说明：\r\n"
			+ "businessID 企业ID，查询设备所在的企业的ID，如果未指定对象ID，则返回符合企业ID、类型两个参数值的对象列表\r\n"
			+ "objectID 对象ID\r\n"
			+ "type 类型，企业ID、类型两个参数可组合使用，也可单独使用；如果三个参数都未指定，则返回所有对象\r\n";
	parent.getMethodInfo('sceneIJas.js',
			'getObjectInfoList(objectID,businessID,type)', '',
			'根据企业ID，或对象ＩＤ，或类型，列出所相关的所有对象信息', paramer);
	selectDialog(obj);
}

/**
 * 球范围查询:
 */
function selectBallDate(obj) {
	var paramer = "js参数说明：\r\n"
			+ "type 查询方式，当前支持的查询方式，为下面的任意一种类型。可用参数:圆形、圆球、圆柱、多边形、矩形、立方体\r\n"
			+ "lon01 经度\r\n"
			+ "lat01 纬度\r\n"
			+ "height01 高度\r\n"
			+ "rde01 半径,球形相关的查询半径\r\n"
			+ "type01 查询类型，相交查询方式；包含查询方式。可用参数：相交、包含\r\n"
			+ "userDate01 用户数据，用户用来确定本次查询的数据，查询过程异步进行，需要标识设定每次查询的标识，用户来确定结果和操作的对应关系\r\n"
			+ "cameraID01 企业ID\r\n"
			+ "objectType01:显示对象类型，此参数可以是平台支持的任意显示对象类型，在对象管理器中可以找到名字，如果不给此参数数或是“全部”，则查询所有的显示对象\r\n"
			+ "length01 长度\r\n" + "width01 宽度\r\n" + "height02 高度\r\n"
			+ "lonlat 经纬度点，点的坐标(多边形各个顶点)";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'selectBall(type,lon01,lat01,height01,rde01,type01,userDate01,cameraID01,objectType01,length01,width01,height02,lonlat)',
					'', '根据企业ID，或对象ＩＤ，或类型，列出所相关的所有对象信息', paramer);
	selectDialog(obj);
}

/**
 * 模型 ： 根据模型名，查询模型ID
 */
function SelectModelID(obj) {
	var paramer = "js参数说明：\r\n" + "name 模型名称";
	parent.getMethodInfo('sceneIJas.js', 'listModelName(name)', '',
			'根据模型名称查询模型ID', paramer);
	selectDialog(obj);
}

/**
 * 获取对象的经纬高 弹出框
 */
function popObjectFour(obj) {
	var paramer = "js参数说明：\r\n" + "ID 所要获取对象的ID";
	parent.getMethodInfo('sceneIJas.js', 'getObjectPDH(ID)', '',
			'根据对象ID，返回对象的在地球上的经纬度以及海拔', paramer);
	selectDialog(obj);
}

/**
 * 获取对象定位参数 弹出框
 */
function popObjectFive(obj) {
	var paramer = "js参数说明：\r\n" + "ID 所要返回定位参数的对象ID";
	parent.getMethodInfo('sceneIJas.js', 'getObjectPosition(ID)', '',
			'获取对象定位参数', paramer);
	selectDialog(obj);
}

/**
 * 画点
 */
function drawPointMsl11(obj) {
	var paramer = "js参数说明：\r\n" + "FID 创建对象所属的企业ID，注：该企业ID只能为0。只能添加到地心企业\r\n"
			+ "ID 创建对象的ID，对应ID必须以负号开头。例如：-123132" + "name 创建对象的名称\r\n"
			+ "X 点所在的经度\r\n" + "Y 点所在的纬度\r\n" + "Z 点所在的高度\r\n"
			+ "colorR 点的颜色，点颜色为16进制颜色。例如：#FFFF33\r\n"
			+ "colorA 透明度，范围为0到100\r\n" + "N 可视对象点的大小\r\n";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'drawPoint(FID,ID,name,X,Y,Z,colorR,colorA,N)',
					'',
					'通过接口在三维地球中创建点类型的可视化对象。主要用于根据数据信息在地球上创建可视化点，并通过观察点的密度，直观的看出数据信息对应的事物的分布情况',
					paramer);
	selectDialog(obj);
}

/**
 * 画线
 */
function drawLineMsl111(obj) {
	var paramer = "js参数说明：\r\n" + "FID 企业ID，注：该企业ID只能为0。只能添加到地心企业\r\n"
			+ "ID 对象ID，创建对象的ID，对应ID必须以负号开头\r\n" + "name 创建对象的名称\r\n"
			+ "(AX,AY,BX,BY) 两点的经纬度\r\n"
			+ "pColor 点的颜色，点颜色为16进制颜色。例如：#FFFF33\r\n"
			+ "pColorA 点的透明度，范围为0到100\r\n" + "s 可视对象点的大小\r\n"
			+ "lColor 线的颜色，线颜色为16进制颜色。例如：#FFFF33\r\n"
			+ "lColorA 线的透明度，范围为0到100\r\n" + "N 线宽度，可视对象线的大小";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'drawLine(FID,ID,name,AX,AY,AZ,BX,BY,BZ,pColor,pColorA,s,lColor,lColorA,N)',
					'', '通过接口在三维地球中创建线类型的可视化对象', paramer);
	selectDialog(obj);
}

/**
 * 画面
 */
function drawSurfaceMsl111(obj) {
	var paramer = "js参数说明：\r\n" + "FID 企业ID，企业ID，注：该企业ID只能为0。只能添加到地心企业\r\n"
			+ "OID 对象ID，创建对象的ID，对应ID必须以负号开头" + "NAME 名称，创建对象的名称\r\n"
			+ "pColor 点的颜色，点颜色为16进制颜色。例如：#FFFF33\r\n"
			+ "PA 点的透明度，，范围为0到100\r\n" + "PS 可视对象点的大小\r\n"
			+ "lColor 线的颜色，线颜色为16进制颜色。例如：#FFFF33\r\n" + "LA 线的透明度，范围为0到100\r\n"
			+ "LS 线宽度，可视对象线的大小\r\n" + "sColor 面颜色为16进制颜色。例如：#FF0033\r\n"
			+ "SA 面透明度，范围为0到100\r\n"
			+ "objecArry 点对象数组。构造方法：suface(x,y,z)，x,y,z对应为点位置（经纬度高）";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'drawSurface(FID,OID,NAME,pColor,PA,PS,lColor,LA,LS,sColor,SA,objecArry)',
					'', '通过接口在三维地球中创建面类型的可视化对象', paramer);
	selectDialog(obj);
}

/**
 * 创建可视对象标牌:
 */
function makeGeneral121(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 企业ID\r\n"
			+ "objectID 创建的标牌的ID\r\n" + "lon 经度\r\n" + "lat 纬度\r\n"
			+ "alt 高度\r\n" + "modelID 所属设备ID\r\n"
			+ "arrayTitle 标牌列名，例如：['设备名称','运行编号','生产厂家']\r\n"
			+ "arrayContent 标牌内容，例如：[ '接地刀闸', '1027535','陕西']\r\n"
			+ "offsetX 标牌与坐标系角度\r\n" + "offsetY 标牌与坐标系角度\r\n"
			+ "styleName 标牌样式";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'generalPanel(businessID,objectID,lon,lat,alt,modelID,arrayTitle,arrayContent,offsetX,offsetY,styleName)',
					'', '创建可视化对象标牌', paramer);
	selectDialog(obj);
}

/**
 * 画点线面
 */
function hua(obj) {
	var paramer = "js参数说明：\r\n"
			+ "fID 企业ID，用户绘制时生成的对象所属企业，一般设为0，即归到地心企业下即可。例如：bussinessID=”8”\r\n"
			+ "oID 对象ID，用户绘制时生成的对象的ID，必须为负值\r\n" + "Type 操作类型，点 | 线 | 面\r\n"
			+ "Width 线宽度";
	parent.getMethodInfo('sceneIJas.js', 'draw(fID,oID,Type,Width)', '',
			'启动画点线面的功能，并会根据用户的键盘鼠标操作，画出点线面', paramer);
	selectDialog(obj);
}

/**
 * 模型 弹出框
 */
function popObject10(obj) {
	var paramer = "js参数说明：\r\n"
			+ "businessID 企业ID，所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"
			+ "resourcePath 资源路径(X:\\X.osg)，相对路径为地球缓存下的\Res\model文件夹。也支持绝对路径。\r\n"
			+ "(positionX，positionY，positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"
			+ "(gestureX,gestureY,gestureZ) 姿态\r\n" + "(zoomX,zoomY,zoomZ) 缩放";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popModel(businessID,objectID,resourcePath,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,zoomX,zoomY,zoomZ)',
					'', '添加新模型', paramer);
	selectDialog(obj);
}
/**
 * 批量添加屏幕标注 弹出框
 * 
 * @param obj
 */
function popObject11PL(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n";
	parent.getMethodInfo('sceneIJas.js', 'popScreenOverlaysPL(bussinsessID)',
			'', '批量添加屏幕标注', paramer);
	selectDialog(obj);
}

/**
 * 屏幕标注 弹出框
 */
function popObject11(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n" + "content 内容\r\n"
			+ "font 字体，例如：SIMSUN.TTC\r\n" + "fontSize 字号\r\n"
			+ "(positionX，positionY，positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"
			+ "color 颜色，线颜色为16进制颜色。例如：#000033\r\n" + "colorA 透明度，范围为0到100\r\n"
			+ "resourcePath 资源路径，资源路径为屏幕标注的绑定图片资源的路径\r\n" + "wide 宽\r\n"
			+ "height 高\r\n" + "alignment 对齐方式，如居中，等等";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popScreenOverlays(businessID,objectID,content,font,fontSize,positionX,positionY,positionZ,colorR,colorA,resourcePath,wide,height,alignment)',
					'', '添加屏幕标注', paramer);
	selectDialog(obj);
}

/**
 * 空间文字 弹出框
 */
function popObject12(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n" + "content 内容\r\n"
			+ "font 字体，例如：SIMSUN.TTC\r\n" + "fontSize 字号\r\n"
			+ "(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"
			+ "(gestureX,gestureY,gestureY) 姿态\r\n"
			+ "colorR 颜色， 颜色为16进制颜色。例如：#000033\r\n" + "colorA 透明度，范围为0到100";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popSpaceText(businessID,objectID,content,font,fontSize,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,colorR,colorA)',
					'', '添加空间文字', paramer);
	selectDialog(obj);
}

/**
 * 空间图片 弹出框
 */
function popObject13(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"
			+ "resourcePath 资源路径(X:\\X.osg)，图片的路径\r\n"
			+ "(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）"
			+ "(gestureX,gestureY,gestureY) 姿态 \r\n" + "wide 宽\r\n"
			+ "height 高";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popSpacePictures(businessID,objectID,resourcePath,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,wide,height)',
					'', '添加空间图片', paramer);
	selectDialog(obj);
}

/**
 * 屏幕窗口 弹出框
 */
function popObject14(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"
			+ "(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"
			+ "wide 宽\r\n" + "height 高\r\n"
			+ "colorR 颜色 颜色为16进制颜色。例如：#000033\r\n" + "colorA 透明度，范围为0到100";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popScreenWallpaper(businessID,objectID,positionX,positionY,positionZ,wide,height,colorR,colorA)',
					'', '添加屏幕窗口', paramer);
	selectDialog(obj);
}

/**
 * 屏幕文字 弹出框
 */
function popObject15(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n" + "windowID 窗口ID\r\n"
			+ "font 字体，例如：SIMSUN.TTC\r\n" + "fontSize 字号\r\n"
			+ "(positionX,positionY) X,Y对应为所属企业的坐标（以企业左下角为原点）\r\n"
			+ "colorR 颜色 颜色为16进制颜色。例如：#000033\r\n" + "colorA 透明度，范围为0到100";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popScreenText(businessID,objectID,windowID,font,fontSize,positionX,positionY,colorR,colorA)',
					'', '添加屏幕文字', paramer);
	selectDialog(obj);
}

/**
 * 屏幕图片 弹出框
 */
function popObject16(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n" + "windowID 窗口ID\r\n"
			+ "resourcePath 资源路径(X:\\X.osg)，图片所在的路径\r\n"
			+ "(positionX,positionY) X,Y对应为所属企业的坐标（以企业左下角为原点）\r\n"
			+ "wide 宽\r\n" + "height 高";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popScreenPictures(businessID,objectID,windowID,resourcePath,positionX,positionY,wide,height)',
					'', '添加屏幕图片', paramer);
	selectDialog(obj);
}

/**
 * 球 弹出框
 */
function popObject17(obj) {
	var paramer = "js参数说明：\r\n" + "businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"
			+ "objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"
			+ "earthColorR 球体色，线颜色为16进制颜色。例如：#000033\r\n"
			+ "earthColorA 球体透明度，范围为0到100\r\n"
			+ "detail 细化度，细化度”并设置参数0～100\r\n"
			+ "gridColorR 格网色，线颜色为16进制颜色。例如：#000033";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'popEarth(businessID,objectID,earthColorR,earthColorA,detail,gridColorR)',
					'', '添加球', paramer);
	selectDialog(obj);
}

/**
 * 创建动态标牌:
 */
function createRun(obj) {
	var paramer = "js参数说明：\r\n" + "objectId 对象ID\r\n" + "modelId 绑定的模型ID\r\n"
			+ "arrayContent 内容对象数组\r\n" + "x 经度\r\n" + "y 纬度\r\n" + "z 高度";
	parent.getMethodInfo('sceneIJas.js',
			'createRunPanel(objectId,modelId,arrayContent,x,y,z)', '',
			'创建一个动态标牌', paramer);
	selectDialog(obj);
}

/**
 * 更新动态标牌:
 */
function updateRun(obj) {
	var paramer = "js参数说明：\r\n" + "objectID  需要修改的标牌标号\r\n" + "row 行数\r\n"
			+ "col 列数\r\n" + "content 内容";
	parent.getMethodInfo('sceneIJas.js',
			'updateRunPanel(objectID,row,col,content)', '', '修改标牌的内容', paramer);
	selectDialog(obj);
}

/**
 * 更新Tip标牌
 */
function updateTip(obj) {
	var paramer = "js参数说明：\r\n" + "objectID  需要修改的标牌标号\r\n"
			+ "content 修改的内容\r\n" + "wordr 字体颜色";
	parent.getMethodInfo('sceneIJas.js', 'updateTipPanel(ID,content,wordr)',
			'', '更改Tip标牌', paramer);
	selectDialog(obj);
}

/**
 * 更新Text标牌
 */
function updateText(obj) {
	var paramer = "js参数说明：\r\n" + "objectID  需要修改的标牌标号\r\n" + "row 行数\r\n"
			+ "col 列数\r\n" + "content 内容";
	parent.getMethodInfo('sceneIJas.js', 'updateTextPanel(ID,content,wordr)',
			'无', '修改Text标牌', paramer);
	selectDialog(obj);
}

/**
 * 更新Html标牌
 */
function updateHtml(obj) {
	var paramer = "js参数说明：\r\n" + "objectID  需要修改的Html标牌ID\r\n" + "content 内容";
	parent.getMethodInfo('sceneIJas.js',
			'updateRunPanel(objectID,row,col,content)', '', '修改标牌的内容', paramer);
	selectDialog(obj);
}

/**
 * 对象自动释放:
 */
function setRun(obj) {
	var paramer = "js参数说明：\r\n" + "objectID03 所要释放的标牌id\r\n" + "time 设置多长时间后释放";
	parent.getMethodInfo('sceneIJas.js', 'setRunPanel(objectID03,time)', '',
			'标牌对象自动释放', paramer);
	selectDialog(obj);
}

/**
 * Tip标牌:
 */
function creatTip(obj) {
	var paramer = "js参数说明：\r\n"
			+ "FID 企业ID，对象所属企业ID\r\n"
			+ "ID 对象ID，对应ID必须为负值，并且不能重复\r\n"
			+ "name 名称\r\n"
			+ "point 绑定对象ID\r\n"
			+ "wordsize 字体大小\r\n"
			+ "wordtype 字体样式，添加的文字的样式。例如：SIMYOU.TTF\r\n"
			+ "worda 文字透明度，范围为0到100\r\n"
			+ "wordr 文字的颜色，颜色为16进制颜色。例如：#FF0033\r\n"
			+ "bordera 边框透明度，范围为0到100\r\n"
			+ "bordera 边框的颜色，颜色为16进制颜色。例如：#00FFFF\r\n"
			+ "wordname 文字内容\r\n"
			+ "(locationx,locationy,locationz) 空间位置。如果绑定对象id为空或者不存在的情况下，该属性产生效果";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'createTipPanel(FID,ID,name,pointer,wordsize,wordtype,worda,wordr,bordera,borderr,wordname,locationx,locationy,locationz)',
					'', '创建一个Tip标牌', paramer);
	selectDialog(obj);
}
/**
 * Text标牌:
 */
function creatText(obj) {
	var paramer = "js参数说明：\r\n" + "busessID 企业ID，对象所属企业ID\r\n"
			+ "ID 对象ID，对应ID必须为负值，并且不能重复\r\n" + "name 标牌名称\r\n" + "titel 标题\r\n"
			+ "titelFont 标题字体，例如：wqy-zenhei_0.ttc\r\n"
			+ "titelFontHeight 标题行高度\r\n" + "titelBkpic 标牌背景图片\r\n"
			+ "titelColor 标题颜色，颜色为16进制颜色，如：#000033\r\n"
			+ "titelColorA 标题透明度，范围为0到100\r\n" + "bkpic 背景图片\r\n"
			+ "minscale 最小缩放倍数\r\n" + "maxscale 最大缩放倍数\r\n"
			+ "posX01 绑定对象ID\r\n"
			+ "posX,posY,posZ:绑定位置。如果绑定对象id为空或者不存在的情况下，该属性产生效果\r\n"
			+ "bkColor 背景颜色，颜色为16进制颜色。例如：#000033\r\n"
			+ "bkColorA 背景透明度，范围为0到100\r\n"
			+ "alignType 对齐方式，有三种：center， left， right\r\n"
			+ "usegrid 是否有网格，True或false\r\n" + "gridLineWidth 网格宽度\r\n"
			+ "gridColor 网格颜色，颜色为16进制颜色。例如：#000033\r\n"
			+ "gridColorA 网格透明度，范围为0到100\r\n" + "itemObject 标牌内容对象";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'createTextPanel(busessID,ID,name,titel,titelFont,titelFontHeight,titelBkpic,titelColor,titelColorA,bkpic,minscale,maxscale,posX01,posX,posY,posZ,bkColor,bkColorA,alignType,usegrid,gridLineWidth,gridColor,gridColorA,itemObject)',
					'', '创建一个Text标牌', paramer);
	selectDialog(obj);
}
/**
 * html标牌:
 */
function creathtml(obj) {
	var paramer = "js参数说明：\r\n"
			+ "FID 企业ID，对象所属企业ID\r\n"
			+ "ID 对象ID，对应ID必须为负值，并且不能重复\r\n"
			+ "name 标牌名称\r\n"
			+ "htmlUrl html内容的地址\r\n"
			+ "(bindPosX,bindPosY,bindPosZ) 标牌的位置。如果绑定对象id不存在或者为空，那没绑定位置起作用\r\n"
			+ "weight 宽\r\n"
			+ "height 高\r\n"
			+ "titelBkpic 标牌的背景颜色，颜色为16进制颜色，如：#000033\r\n"
			+ "titelBkpicA 标牌背景透明度，范围为0到100\r\n"
			+ "visiable 绑定对象ID\r\n"
			+ "titelBkpic1：关闭按钮颜色\r\n"
			+ "titelBkpicA1：关闭按钮透明度\r\n"
			+ "drag：html标牌是否可以拖动，true或false\r\n"
			+ "spaceLift,spaceUp,spaceRight,spaceDown：四个数值的含义分别为：左边距 上边距 右边距 下边距如果这里面的边距为0，则不显示边框。\r\n"
			+ "foundSize：此参数表示标牌角上的圆角大小，如果是0，则是矩形框。";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'createHtmlPanel(FID,ID,name,htmlUrl,bindPosX,bindPosY,bindPosZ,weight,height,titelBkpic,titelBkpicA,visiable)',
					'', '创建一个HTML标牌', paramer);
	selectDialog(obj);
}

/**
 * 添加管道
 */
function e1(obj) {
	var paramer = "js参数说明：\r\n" + "lon1,lat1,h1 起点经纬度高\r\n"
			+ "lon2,lat2,h2 终点经纬度高\r\n" + "r 管子的半径\r\n"
			+ "ee1r 管子的颜色，颜色为16进制颜色。例如：#000033";
	parent.getMethodInfo('sceneIJas.js',
			'pipeAdd(lon1,lat1,h1,lon2,lat2,h2,r,ee1r)', '',
			'通过选择类型（创建，清除），及两点的经纬高，颜色等参数，来添加或清除管段', paramer);
	selectDialog(obj);
}

/**
 * 地形剖切
 */
function q3(obj) {
	var paramer = "js参数说明：\r\n" + "arrayPoint 点对象数组\r\n" + "deep 深度";
	parent.getMethodInfo('sceneIJas.js', 'geographyCut(arrayPoint,deep)', '',
			'通过深度，贴图重复率，显示底框是否启用来刨切地面图形', paramer);
	selectDialog(obj);
}

/**
 * 模型刨切:
 */
function makeGeneral(obj) {
	var paramer = "js参数说明：\r\n" + "modelpath02  所要剖切的标牌id";
	parent.getMethodInfo('sceneIJas.js', 'modelSliced(modelpath02)', '',
			'通过用断面的形式剖切设备，一查看内部结构', paramer);
	selectDialog(obj);
}

/**
 * 模型拆分:
 */
function modelchai(obj) {
	var paramer = "js参数说明：\r\n" + "modelpath01  所要拆分的标牌id";
	parent.getMethodInfo('sceneIJas.js', 'modelSplit(modelpath01)', '',
			'按原件拆分模型', paramer);
	selectDialog(obj);
}

/**
 * 对象删除 弹出框
 */
function popObjectOne(obj) {
	var paramer = "js参数说明：\r\n" + "objectID 所要删除的对象ID";
	parent.getMethodInfo('sceneIJas.js', 'objectDelete(objectID)', '',
			'删除指定ID的对象', paramer);
	selectDialog(obj);
}

/**
 * 添加Lod信息 弹出框
 */
function popObject7(obj) {
	var paramer = "js参数说明：\r\n"
			+ "mode （自身中心点|设定中心点）模式\r\n"
			+ "ID 对象ID，添加的Lod信息的ID\r\n"
			+ "(x,y,z)经纬度高，当模式是自身中心点时,不需要设置XYZ,只设置距离.当模式是设定中心点时, 需要设置距离，同时需要设置XYZ的值,XYZ值即设定的中心点\r\n"
			+ "minDistance 最小距离，如果海拔高度小于这个值，LOD信息就不显示\r\n"
			+ "maxDistance 最大距离，如果海拔高度大于这个值，LOD信息就不显示";
	parent.getMethodInfo('sceneIJas.js',
			'addLodInfo(mode,ID,x,y,z,minDistance,maxDistance)', '',
			'设置一定高度范围内该对象是可见的，在最大最小范围之外是不可见的', paramer);
	selectDialog(obj);
}

/**
 * 修改屏幕标注
 */
function h1(obj) {
	var paramer = "js参数说明：\r\n" + "oID 对象ID\r\n" + "fID 企业ID\r\n"
			+ "content 修改内容\r\n" + "value 值";
	parent.getMethodInfo('sceneIJas.js',
			'changeScreenLabel(oID,fID,content,value)', '',
			'对屏幕标注进行修改，包括对屏幕标注文本和图片', paramer);
	selectDialog(obj);
}

/**
 * 对象动作 弹出框
 */
function popObjectThree(obj) {
	var paramer = "js参数说明：\r\n"
			+ "ID 对象ID\r\n"
			+ "actionType （显隐/选中/透明/闪烁/清除所有效果）\r\n"
			+ "parameter 参数，显隐：0-隐藏 1-显示；选中：不需要参数；透明：0~100整数（数值大小与透明度成正比，0为不透，100为全透）；闪烁：整数（闪烁次数）；清除所有效果：不需要参数";
	parent.getMethodInfo('sceneIJas.js',
			'objectActions(ID,actionType,parameter)', '',
			'可以设置设备的状态：如显隐性，闪烁，透明', paramer);
	selectDialog(obj);
}

/**
 * 高亮模型
 */
function HIGHLIGHT(obj) {
	var paramer = "js参数说明：\r\n" + "ID 处理的对象的ID\r\n"
			+ "color 所需要高亮成的颜色，颜色为16进制颜色。例如：#FF0033";
	parent.getMethodInfo('sceneIJas.js', 'highLight(ID,color)', '', '高亮设备',
			paramer);
	selectDialog(obj);
}

/**
 * 显示对象部件动作 弹出框
 */
function popObject8(obj) {
	var paramer = "js参数说明：\r\n" + "ID 对象ID\r\n"
			+ "actionType (隐藏部件 / 显示部件 / 隐藏全部部件 /显示全部部件) 动作类型\r\n"
			+ "partsName 部件名称\r\n" + "parameter 参数";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'displayPartsAction(ID,actionType,maxDistance,parameter)',
					'',
					'为模型提供的接口，若模型的各个部件遵循一定的命名规则：名称中包含BuJian，则可以通过部件的名称对这些部件进行一定操作，如显示隐藏等',
					paramer);
	selectDialog(obj);
}

/**
 * 设置企业显示 弹出框
 */
function popBusinessFour(obj) {
	var paramer = "js参数说明：\r\n" + "ID 企业ID\r\n" + "model 细分模型（启用/禁用）";
	parent.getMethodInfo('sceneIJas.js', 'setEnterpriseDisplay(ID,model)', '',
			'设置企业是否启用细分模型', paramer);
	selectDialog(obj);
}

/**
 * 查询企业显示弹出框
 */
function popBusinessTwo(obj) {
	var paramer = "js参数说明：\r\n" + "ID 企业ID";
	parent.getMethodInfo('sceneIJas.js', 'queryEnterpriseShow(ID)', '',
			'查询指定企业的是否启用细分模型的功能', paramer);
	selectDialog(obj);
}

/**
 * 设置企业显隐 弹出框
 */
function popBusinessFive(obj) {
	var paramer = "js参数说明：\r\n" + "ID 企业ID\r\n" + "actionType 动作类型，显示|隐藏";
	parent.getMethodInfo('sceneIJas.js',
			'setEnterpriseShowOrHide(ID,actionType)', '', '显示或者隐藏对应ID的企业',
			paramer);
	selectDialog(obj);
}

/**
 * 查询企业显隐弹出框
 */
function popBusinessThree(obj) {
	var paramer = "js参数说明：\r\n" + "ID 企业ID";
	parent.getMethodInfo('sceneIJas.js', 'queryEnterpriseShowOrHide(ID)', '',
			'查询对应企业ID的企业是显示态，还是隐藏', paramer);
	selectDialog(obj);
}

/**
 * 图层控制
 */
function d2(obj) {
	var paramer = "js参数说明：\r\n" + "ID 图层的ID\r\n" + "s 参数，隐藏 / 显示";
	parent.getMethodInfo('sceneIJas.js', 'picControl(ID,s)', '', '控制图层的显隐',
			paramer);
	selectDialog(obj);
}

/**
 * 开启透明
 */
function q4(obj) {
	var paramer = "js参数说明：\r\n" + "n 0-100间的浮点值。0为全透明，100为不透明";
	parent.getMethodInfo('sceneIJas.js', 'diaphaneityOn(n)', '', '关于地表的透明度设置',
			paramer);
	selectDialog(obj);
}

/**
 * 对象定位 弹出框
 */
function popObjectTwo(obj) {
	var scrip1 = "视角定位到对应ID设备";
	var script01 = "js参数说明：\r\n" + "ID 待定位的对象的ID\r\n" + "locatType 定位方式（普通/飞行）";
	parent.getMethodInfo('scriptIJas.js', 'objectLocation(ID,locatType)', '',
			scrip1, script01);
	selectDialog(obj);
}

/**
 * 定位到指定企业弹出框
 */
function popBusiness(obj) {
	var scrip2 = "根据ID值，定位到该id所对企业";
	var script02 = "js参数说明：\r\n" + "ID 待定位的企业ID\r\n" + "type 定位方式（普通/飞行）";
	parent.getMethodInfo('scriptIJas.js', 'enterprisePosition(ID)', '', scrip2,
			script02);
	selectDialog(obj);
}

/**
 * 相机：设置相机位置，姿态
 */
function setCamera(obj) {
	var scrip3 = "通过此命令可以将视角定位到地球上的唯一点";
	var script03 = "js参数说明：\r\n" + "longitude 经度\r\n" + "latitude 纬度\r\n"
			+ "elevation 海拔\r\n" + "yawA 偏航角，相机头部与正北方的夹角\r\n"
			+ "pitchingA 俯仰角，相机指向与水平面的夹角\r\n" + "rollA 滚转角，沿相机指向视线为轴旋转\r\n"
			+ "locateM 定位方式，普通/飞行";
	parent
			.getMethodInfo(
					'scriptIJas.js',
					'setCameraPosition(longitude,latitude,elevation,yawA,pitchingA,rollA,locateM)',
					'', scrip3, script03);
	selectDialog(obj);
}

/**
 * 相机：根据二维地图比例设置三维相机的位置姿态
 */
function popCamera(obj) {
	var scrip5 = "设置当前视角下，地图上的纬度跨度值";
	var script05 = "js参数说明：\r\n"
			+ "mapScale 二维地图比例，表示当前视角下，地图上的纬度跨度。单位为度。如，设置比例为10.0表示设置当前相机的位置姿态，使之南北方向(即纬度方向)恰好能看到10.0度的范围。东西方向(即经度方向)能看到的范围由视角的比例间接确定。例如：mapScale=”0.0037284”";
	parent.getMethodInfo('scriptIJas.js', 'setCameraPositionByMap(mapScale)',
			'', scrip5, script05);
	selectDialog(obj);
}

/**
 * 操作器切换
 */
function change2(obj) {
	var scrip8 = "通过切换地球操作器";
	var script08 = "js参数说明：\r\n" + "Type 操作器类型(0,1)，跟踪球操作器|地球操作器";
	parent.getMethodInfo('scriptIJas.js', 'controlSwitch(Type)', '', scrip8,
			script08);
	selectDialog(obj);
}
/**
 * 漫游器开关
 */
function change3(obj) {
	var scrip7 = "漫游器开关";
	var script07 = "js参数说明：\r\n" + "Type 漫游开关(ON/OFF)";
	parent.getMethodInfo('scriptIJas.js', 'wanderChanger(Type)', '', scrip7,
			script07);
	selectDialog(obj);
}

/**
 * 推演
 */
function tuiyan11(obj) {
	var speci01 = "js参数说明：\r\n" + "type 新建/移除\r\n" + "name 推演名称";
	parent.getMethodInfo('specialTopicIJas.js', 'tuiYan(type,name)', '',
			'推演功能', speci01);
	selectDialog(obj);
}

/**
 * 事件处理器管理开启
 */
function q1(obj) {
	var spTol2 = "事件处理器管理(开启)";
	var speci02 = "js参数说明：\r\n" + "name 开启事务的处理器名称";
	parent.getMethodInfo('specialTopicIJas.js', 'thingManageOn(name)', '',
			spTol2, speci02);
	selectDialog(obj);
}
/**
 * 事件处理器管理关闭
 */
function q2(obj) {
	var spTol3 = "事件处理器管理(关闭)";
	var speci03 = "js参数说明：\r\n" + "name 关闭事务的处理器名称";
	parent.getMethodInfo('specialTopicIJas.js', 'thingManageOff(name)', '',
			spTol3, speci03);
	selectDialog(obj);
}

/**
 * 动画 ： 播放脚本
 */
function playCortoon(obj) {
	var spTol4 = "在三维地球中播放指定名称的脚本";
	var speci044 = "js参数说明：\r\n" + "scriptName 播放脚本的名字";
	parent.getMethodInfo('specialTopicIJas.js', 'playScript(scriptName)', '',
			spTol4, speci044);
	selectDialog(obj);
	var list = parent.getScriptList();
	var xmlStrDoc = parseXML(list);
	// 解析 ，绑定
	$(xmlStrDoc).find("名称").each(function() {
		var objectList = $(this).text();
		$("#scriptName").append(" <option>" + objectList + " </option>");
	});
}

/**
 * 设置播放器的位置
 */
/*
 * 
 * function setPlayer(obj){ var spTol4 = "设置播放器的位置"; var speci044 =
 * "js参数说明：\r\n" + "X 位置X\r\n"+"Y 位置Y";
 * parent.getMethodInfo('specialTopicIJas.js', 'setPlayerPosition(x,y)',
 * '',spTol4, speci044); selectDialog(obj); }
 */

/**
 * 解析XML
 */
function parseXML(str) {
	if (window.DOMParser) { // Mozilla Explorer
		parser = new DOMParser();
		xmlStrDoc = parser.parseFromString(str, "text/xml");
	} else { // Internet Explorer
		xmlStrDoc = new ActiveXObject("Microsoft.XMLDOM");
		xmlStrDoc.async = "false";
		xmlStrDoc.loadXML(str);
	}
	;
	return xmlStrDoc;
}

/**
 * 命中测试
 */
function hit(obj) {
	var etn = "根据指定的屏幕点坐标，由该点垂直屏幕向屏幕内引一条射线，获取该射线与场景中相交的第一个对象的信息";
	var calcu01 = "js参数说明：\r\n"
			+ "X, Y 坐标值是以三维视口左下角为原点，向右为X正向，向上为Y正向的屏幕像素坐标，坐标值是以三维视口左下角为原点";
	parent.getMethodInfo('calculateIJas.js', 'hitTest(X,Y)', '', etn, calcu01);
	selectDialog(obj);
}

/**
 * 世界与厂区坐标转换(世界到厂区)
 */
function change(obj) {
	var etn1 = "通过把具体世界坐标，转换成具体企业自己坐标格式对应的坐标";
	var calcu02 = "js参数说明：\r\n" + "ID 需要转换成对应企业坐标格式的企业ID\r\n" + "X，Y，Z 经纬度海拔";
	parent.getMethodInfo('calculateIJas.js', 'worldFactorySwitch(ID,X,Y,Z)',
			'', etn1, calcu02);
	selectDialog(obj);
}

/**
 * 世界与厂区坐标转换(厂区到世界)
 */
function change11(obj) {
	var etn2 = "通过把具体企业坐标，转换成具体世界坐标";
	var calcu03 = "js参数说明：\r\n" + "ID 厂区坐标所在的企业\r\n" + "X，Y，Z 厂区坐标X，Y轴，及高";
	parent.getMethodInfo('calculateIJas.js', 'factoryWorldSwitch(ID,X,Y,Z)',
			'', etn2, calcu03);
	selectDialog(obj);
}

/**
 * 获取屏幕坐标点的经纬高
 */
function get1(obj) {
	var etn3 = "获取屏幕坐标对应经纬高";
	var calcu04 = "js参数说明：\r\n"
			+ "X，Y 向右为X正向的屏幕像素坐标，向上为Y正向的屏幕像素坐标，坐标值是以三维视口左下角为原点";
	parent.getMethodInfo('calculateIJas.js', 'getXYZ(X,Y)', '', etn3, calcu04);
	selectDialog(obj);
}

/**
 * 获取指定点到长距离管线的投影坐标
 */
function get2(obj) {
	var etn4 = "获取指定点到长距离管线的投影坐标";
	var calcu05 = "js参数说明：\r\n" + "X，Y 指定点的经纬度";
	parent.getMethodInfo('calculateIJas.js', 'getShadowXY(X,Y)', '', etn4,
			calcu05);
	selectDialog(obj);
}

/**
 * 计算高度
 */
function count(obj) {
	var etn5 = "计算高度(返回当前加载的地球在指定的经纬点的海拔高度，如果计算失败,高度的值为0.)";
	var calcu06 = "js参数说明：\r\n" + "X，Y 指定点经纬度";
	parent.getMethodInfo('calculateIJas.js', 'countHighly(X,Y)', '', etn5,
			calcu06);
	selectDialog(obj);
}

/**
 * 创建右键菜单
 */
function e2(obj) {
	var mtp = "js参数说明：\r\n" + "content 菜单显示内容\r\n" + "ID 右键弹出菜单对象ID \r\n"
			+ "X,Y 以屏幕左上角为原点，X向右为正,Y向下为正";
	parent.getMethodInfo('uiIJas.js', 'rightMenu(ID,content,X,Y)', '',
			'创建一个满足用户需求的右键菜单，该菜单在双击模型之后在指定的位置X,Y生成。', mtp);
	selectDialog(obj);
}

/**
 * 坐标转换(由西安80坐标到WGS84坐标)
 */
function change1(obj) {
	var mtp = " js参数说明：\r\n" + "type 切换类型：广东管网_西安80_WGS84/广东管网_WGS84_西安80\r\n"
			+ "x,y 平面坐标系X，Y";
	parent.getMethodInfo('aidIJas.js', 'coordTransform(X,Y)', '',
			'由西安80平面坐标转换到WGS84经纬坐标，或由WGS84经纬坐标转换到西安80平面坐标。', mtp);
	selectDialog(obj);
}

/**
 * 抓图
 */
function h2(obj) {
	var mtp = "js参数说明：\r\n" + "type 动作类型（开始、停止）\r\n" + "path 保存图片的路径\r\n"
			+ "startx 抓图区域的范围中起始经度\r\n" + "starty 抓图区域的范围中起始纬度\r\n"
			+ "endx 抓图区域的范围中终点经度\r\n" + "endyx 抓图区域的范围中终点纬度\r\n"
			+ "hight 抓图高度\r\n" + "time 抓图单张图片间的时间间隔";
	parent
			.getMethodInfo(
					'aidIJas.js',
					'printScreen(type,path,startx,starty,endx,endy,high,time)',
					'',
					'类型参数为“停止”时，其他的参数无效；类型参数为“开始”时，系统按照参数，开始在指定的区域连续抓图，保存单张图片到指定路径；抓图完成时，会将所有抓取到的图片合并为一张图片',
					mtp);
	selectDialog(obj);
}

/**
 * 截图
 */
function pich4(obj) {
	var paramer = "js参数说明：\r\n" + "pictureUrl 输出截图所放的位置\r\n" + "name 截图的名称";
	parent.getMethodInfo('aidIJas.js', 'savePicture(pictureUrl,name)', '',
			'输出截图', paramer);
	selectDialog(obj);
}

/**
 * 播放音乐
 */
function music(obj) {
	var mtp = "js参数说明：\r\n" + "Type 类型：开始 | 结束 | 暂停 | 继续\r\n" + "Path 音乐文件路径";
	parent.getMethodInfo('aidIJas.js', 'playMusic(Path,Type)', '', '播放音乐', mtp);
	selectDialog(obj);
}

/**
 * 选择本地文件
 */
function h3(obj) {
	var mtp = "js参数说明：\r\n"
			+ "s 'shp文件(*.shp)|*.shp|所有文件(*.*)|*.*|' 设定选择文件时,文件的后缀.上面例子中是显示shp文件或是所有文件如果想只显示IVE文件,可以这样ive文件(*.ive)|*.ive|\r\n"
			+ "type 是否多选（true、false）";
	parent.getMethodInfo('aidIJas.js', 'choiceFile(s,type)', '',
			'提供一个可以选择文件的对话框', mtp);
	selectDialog(obj);
}

/**
 * 飞行
 */
function flyScript(obj) {
	var paramer = "js参数说明：\r\n"
			+ "modelId 对象ID\r\n"
			+ "compId 企业ID\r\n"
			+ "oprateType 绘制飞行路径，绘制,开始,暂停、继续、结束\r\n"
			+ "width 宽度\r\n"
			+ "distence 距离\r\n"
			+ "colorR 颜色，颜色为16进制颜色。例如：#00FFFF\r\n"
			+ "colorA 透明度，范围为0到100\r\n"
			+ "flyPoint 飞行点对象数组。构造方法FlyPoint(X,Y,Z,yawA,rollA,picthA,timeF)，X,Y,Z：为飞行点的位置yawA,rollA,picthA：飞行姿态（偏航角，俯仰角，滚转角）timeF：单段飞行时间（秒）\r\n"
			+ "music 音乐对象构造方法Music(file,begin,timeA)，file：音乐文件路径begin：开始播放时间timeA：总时长";
	parent
			.getMethodInfo(
					'scriptIJas.js',
					'flyControl(modelId,compId,oprateType,width,distence,colorR,colorA)',
					'', '飞行', paramer);
	selectDialog(obj);
}

/**
 * 设置状态栏
 */
function setHintbarAttr(obj) {
	selectDialog(obj);
}

/**
 * 设置工具栏
 */
function setToolAttr(obj) {
	selectDialog(obj);
}

/**
 * 设置控制面板
 */
function setControlpanelAttr(obj) {
	selectDialog(obj);
}

/**
 * 设置焦点十字
 */
function setFocuscrossAttr(obj) {
	selectDialog(obj);
}

/**
 * 模型移动
 */
function move(obj) {
	selectDialog(obj);
}

/**
 * 设置局部经纬线
 */
function setLonAndLatLine(obj) {
	selectDialog(obj);
}

/**
 * 
 * 经纬线
 */
 function LonAndLatLine(obj){
	 var paramer = "js参数说明：\r\n" + "设置经纬网:\r\n颜色R、G、B、A:线的颜色及透明度\r\n文字R、G、B、A:文字的颜色及透明度\r\nfontSize:文字大小\r\n state:显示开关";
		parent.getMethodInfo('systemIJas.js', 'settingLonAndLatLine(yanR,yanG,yanB,yanA,ziR,ziG,ziB,ziA,fonte,landjingweiType)', '无',
				'显示全球的经纬网络', paramer);
		selectDialog(obj);
 }

/**
 * 
 * 地球变灰
 */
function EarthGray(obj){ 
	var paramer = "js参数说明：\r\n" + "参数：启用|禁用";
	parent.getMethodInfo('systemIJas.js', 'settingEarthGray(grayOprate)', '无',
			'显示黑白地球，二三维一体化。', paramer);
	selectDialog(obj);
	
}


/**
 * （新剖切设备）
 */
function newModelpao(obj) {
	selectDialog(obj);
}
/**
 * 
 * 判断矢量图层是否存在
 */
function isExistDemo(obj) {
	var etn = "根据图层名称，查询图层是否存在";
	var calcu01 = "js参数说明：\r\n" + "name为图层名称";
	parent.getMethodInfo('vectorIJas.js', 'isExist(name)', '', etn, calcu01);
	selectDialog(obj);

}
/**
 * 获取图层的显示状态
 * 
 * @param obj
 */
function getStateDemo(obj) {
	var etn = "根据图层名称，查看图层的显示状态";
	var calcu01 = "js参数说明：\r\n" + "name为图层名称";
	parent.getMethodInfo('vectorIJas.js', 'getState(name)', '', etn, calcu01);
	selectDialog(obj);

}
/**
 * 设置图层的显示状态
 * 
 * @param obj
 */
function setStateDemo(obj) {
	var etn = "根据图层名称和状态，设置图层的显示状态";
	var calcu01 = "js参数说明：\r\n" + "图层名称name,显示状态为1表示显示，为0时不显示";
	parent.getMethodInfo('vectorIJas.js', 'setState(name)', '', etn, calcu01);
	selectDialog(obj);

}
/**
 * 图查文
 * 
 * @param obj
 */
function searchDemo1(obj) {
	var etn = "根据图层名称、查询类型、查询属性、坐标串来进行图查文";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n" + "type为查询类型\r\n"
			+ "property为查询属性\r\n" + "x,y为查询的坐标串，表示经纬度,radius为半径";
	parent.getMethodInfo('vectorIJas.js',
			'search1(name,type,property,x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,radius)',
			'', etn, calcu01);
	selectDialog(obj);

}
/**
 * 文查图
 * 
 * @param obj
 */
function searchDemo2(obj) {
	var etn = "根据图层名称、查询语句、查询属性进行文查图";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n" + "statement为查询语句\r\n"
			+ "property为查询属性\r\n";
	parent.getMethodInfo('vectorIJas.js', 'search2(name,property,statement)',
			'', etn, calcu01);
	selectDialog(obj);

}
/**
 * 高亮要素颜色
 * 
 * @param obj
 */
function setlightDemo(obj) {
	var etn = "设置图层的高亮颜色";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "ID为id\r\n" + "name为图层名称\r\n"
			+ "color为颜色\r\n";
	parent.getMethodInfo('vectorIJas.js', 'setlight(ID,name,color)', '', etn,
			calcu01);
	selectDialog(obj);

}
/**
 * 高亮要素颜色
 * 
 * @param obj
 */
function recoverDemo(obj) {
	var etn = "恢复高亮显示";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n";
	parent.getMethodInfo('vectorIJas.js', 'recoverLight(name)', '', etn,
			calcu01);
	selectDialog(obj);

}
/**=======================矢量图层开始===========================**/
/**
 * 登录
 * 
 */
function loginDemo(obj) {
	var etn = "登录";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "username为用户名称\r\n" + "password为密码\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'login(username,password)', '',
			etn, calcu01);
	selectDialog(obj);
}
/**
 * 新建一个图层
 * 
 */
function newLayer(obj) {
	var etn = "新建图层";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name 图层名称，该处不包括命名空间\r\n" + "type 图层类型（Point Line Polygon）\r\n"+
				"attr 属性：long OBJECTID,string 3NAME,（即属性类型 属性名称，属性类型为long double string int，属性名称OBJECTID必须有，属性名称均为大写）\r\n"+
				"des 符号化描述：对应vec.vecfg中的字符串，如果为空，则为程序默认设置。\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'addLayer(name,type,attr,des)', '',
			etn, calcu01);
	selectDialog(obj);
}
/**
 * 设置编辑图层
 * 
 */
function setLayerGo(obj) {
	var etn = "设置编辑图层";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'setLayer(name)', '设置编辑图层', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 选择编辑图层
 * 
 */
function selectLayer(obj) {
	var etn = "选择编辑图层";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n";
	parent.getMethodInfo('vectoreditIJas.js', '1. getVector()；2. setLayer(name)', '选择编辑图层', etn,
			calcu01);
	selectDialog(obj);
	var info = parent. getVector();
	var optionList=[];
	var nameList = info.split("featureType");
	for(var i=0;i<nameList.length;i++){
		var nameInfo = nameList[i].split("name=\"")[1];
		var name = nameInfo.split("\"")[0];
		$("#selectLayerTable").append("<option value='"+name+"'>"+name+"</option>");
	}
}
/**
 * 删除图层
 * 
 */
function deleteLayer(obj) {
	var etn = "删除图层";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'delLayer(name)', '删除图层', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 设置图层显示顺序
 */
function setLayerSequence(obj){
	var etn = "设置图层显示顺序";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "name为图层名称\r\n"+
					"index为序号，序号：0 - maxLayerNumber，maxLayerNumber为当前加载的图层最大数，设置的序号不应超过该参数";
	parent.getMethodInfo('vectoreditIJas.js', 'setLayerSeq(name,index)', '设置图层显示顺序', etn,
			calcu01);
	selectDialog(obj);
}
//**==================================要素开始=========================================**//
/**
 * 选中要素
 * 
 */
function selElement(obj) {
	var etn = "选中要素";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "id为要素id\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'selectElement(id)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 新建矢量要素
 */
function newElement(obj){
	var etn = "新建矢量要素";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "coor为坐标：经度,纬度 经度,纬度id\r\n"+
				"属性：属性名 属性值,属性名 属性值(此处无需OBJECTID，该属性字段为默认的，属性名称需带命名空间)\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'addElement(coor,attr)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 编辑指定要素属性
 */
function editorElementAttr(obj){
	var etn = "编辑指定要素属性";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"Attributes为属性：属性名 属性值,属性名 属性值\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'editorElement(objectId,Attributes)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 删除元素
 */
function delElement(obj){
	var etn = " 删除要素";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"yesorno为是否从服务器中删除：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'deleteElement(objectId,yesorno)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 精确编辑要素（综合）
 */
function wholeEditElements(obj){
	var etn = " 精确编辑要素（综合）";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"yesorno为是否从服务器中删除：是/否\r\n";
	var aboutJsFun = "方法说明：\r\n" + "第一步：选中要素selectElement(id)\r\n"+
						"第二步：获取选中要素坐标elementCoordinate()\r\n"+
						"第三步：对坐标进行精确编辑elementCoordinate()\r\n"+
						"yesorno为是否从服务器中删除：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', aboutJsFun, '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 *  Delete键删除要素监听事件
 */
function layerDeleteKeyListen(obj){
	var etn = "  Delete键删除要素监听事件";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"yesorno为是否从服务器中删除：是/否\r\n";
	var aboutJsFun = "方法说明：\r\n" + "第一步：选中要素selectElement(id)\r\n"+
						"第二步：delete键监听layerDeleteKey(recall)\r\n"+
						"recall为回调函数：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', aboutJsFun, '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 编辑要素属性（综合）
 */
function wholeEditlementAttr(obj){
	var etn = " 编辑要素属性（综合）";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"yesorno为是否从服务器中删除：是/否\r\n";
	var aboutJsFun = "方法说明：\r\n" + "第一步：选中要素selectElement(id)\r\n"+
						"第二步：获取选中要素属性elementCoordinate()\r\n"+
						"第三步：编辑指定要素属性elementCoordinate()\r\n"+
						"yesorno为是否从服务器中删除：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', aboutJsFun, '', etn,
			calcu01);
	selectDialog(obj);
}
//**========================节点开始====================**//
/**
 * 精准编辑插入要素节点
 */
function insertElementNote(obj){
	var etn = "精准编辑插入要素节点";// 详细描述
	var calcu01 = "js参数说明：\r\n" + "num为所在部分编号\r\n"+"index为节点索引\r\n"+"lon为经度\r\n"
				"lat为纬度\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'insertNote(num,index,lon,lat)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 精准编辑更新节点
 */
function updateElementNote(obj){
	var etn = "精准编辑更新节点";// 详细描述
	var calcu01 = "js参数说明：\r\n" +"index为节点索引\r\n"+"lon为经度\r\n"+
				"lat为纬度\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'updateNote(index,lon,lat)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 精准编辑增加节点
 */
function addElementNote(obj){
	var etn = "精准编辑增加节点";// 详细描述
	var calcu01 = "js参数说明：\r\n" +"index为节点索引\r\n"+"lon为经度\r\n"+
				"lat为纬度\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'addNote(index,lon,lat)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 精准编辑删除节点
 */
function delElementNote(obj){
	var etn = "精准编辑删除节点";// 详细描述
	var calcu01 = "js参数说明：\r\n" +"index为节点索引\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'delNote(index)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 提交要素属性
 */
function submitElement(obj){
	var etn = "提交要素属性";// 详细描述
	var calcu01 = "js参数说明：\r\n" +"objectId为要素的OBJECTID\r\n"+
				"coordinate为属性：属性类型 属性名称 属性值,如：int ChinaMap:LENGTH 1111,string ChinaMap:NAME sss,long ChinaMap:OBJECTID 1"+
				"flag为是否提交：是/否";
	parent.getMethodInfo('vectoreditIJas.js', 'submitElementAttribute(objectId,coordinate,flag)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 提交当前编辑至服务器
 */
function sumintElementToServer(obj){
	var etn = "提交当前编辑至服务器";// 详细描述
	var calcu01 = "js参数说明：\r\n" +"flag为是否提交：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'submitServer(flag)', '', etn,
			calcu01);
	selectDialog(obj);
}
/**
 * 登出
 * 
 */
function loginOut(obj) {
	var etn = "登出";// 详细描述
	var calcu01 = "js参数说明：无参数";
	parent.getMethodInfo('vectoreditIJas.js', 'loginOut', '',
			etn, calcu01);
	selectDialog(obj);
}