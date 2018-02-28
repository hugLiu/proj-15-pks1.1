/**
 * 无参数接口的提交方法
 * 注：无参数接口的所有方法都应该用Demo结尾。方便辨识。
 */

/**
 * 隐藏输入框
 */
function hadediagl(){
	if (id01 != null) {
		$(id01).hide();
	}
}
/**
 * 启动地球
 */
function run3DDemo() {
	var t=parent.getVector();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('systemIJas.js', 'run3D()', '无', '启动三维地球', paramer);
	hadediagl();
}

/**
 * 停止地球
 */
function stop3DDemo() {
	parent.stop3D();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('systemIJas.js', 'stop3D()', '无', '停止三维地球', paramer);
	hadediagl();
}

/**
 * 关于地球
 */
function aboutBoxDemo() {
	parent.aboutBox();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('systemIJas.js', 'aboutBox()', '无', '关于三维地球', paramer);
	hadediagl();
}

/**
 * 获取版本
 */
function versionDemo(){
	var t = parent.getVersion();
	parent.getMethodInfo('systemIJas.js', 'getVersion()', t, '获取版本', '无');
}
/**
 * 获取天气状态
 */
function weatherStatusDemo() {
	var t = parent.weatherStatus();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('systemIJas.js', 'weatherStatus()', t, '查询当前场景中的天气状态',
			paramer);
	hadediagl();
}

/**
 * 返回图层列表
 */
function returnPicListDemo() {
	var t = parent.returnPicList();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('systemIJas.js', 'returnPicList()', t, '返回图层列表',
			paramer);
	hadediagl();
}

/**
 * 返回选中对象ID
 */
function selectedObjectIDDemo() {
	var rid = parent.selectedObjectID();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'selectedObjectID()', rid,
			'返回三维场景中选中的设备ＩＤ', paramer);
	hadediagl();
}

/***
 * 返回选中对象信息列表
 */
function selectedObjectInfoListDemo() {
	var infoList = parent.selectedObjectInfoList();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'selectedObjectInfoList()', infoList,
			'返回在三维场景中选中的设备信息', paramer);
	hadediagl();
}

/**
 * 返回所有LOD对象信息列表
 */
function getLodObjectListDemo() {
	var objList = parent.getLodObjectList();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'getLodObjectList()', objList,
			'列出所有LOD对象信息列表', paramer);
	hadediagl();
}

/**
 * 开启接口查询事件监听
 */
function selectInterfaceDemo() {
	parent.selectInterface(resultt);
	var paramer = "js参数说明：\r\n" + "resultt 回调函数";
	getMethodInfo('sceneIJas.js', 'selectInterface()', '无', '开启接口查询事件监听',
			paramer);
	hadediagl();
}

/**
 * 
 */
function resultt(aa) {
	alert(aa);
}

/**
 * 控件标注
 */
function dimSpaceDemo() {
	parent.dimSpace();
	hadediagl();
}

/**
 * 点标绘开启
 */
function pointOpenDemo() {
	parent.pointOpen("7", "#ff0000", "0.7", "#ffff00", "0.7");
	var paramer = "js参数说明：\r\n" + "size 点所显示的大小\r\n"
			+ "pColor 点颜色，颜色为16进制颜色。例如：#ff0000\r\n" + "PA 点透明度，范围为0到100\r\n"
			+ "color 预选点颜色，颜色为16进制颜色。例如：#ffff00\r\n" + "A 预选点透明度，范围为0到100";
	parent.getMethodInfo('sceneIJas.js', 'pointOpen(size,pColor,PA,color,A)',
			'无', '按住键盘shift和点击鼠标左键会在屏幕上标绘出一些点', paramer);
	hadediagl();
}

/**
 * 线标绘开启
 */
function lineOpenDemo() {
	parent.lineOpen("7", "#ff0000", "0.7", "#ffff00", "0.7", "5", "#00ff00",
			"0.7", "#00ff00", "0.7");
	var paramer = "js参数说明：\r\n" + "pSize 点所显示的大小\r\n"
			+ "pColor 点颜色，颜色为16进制颜色。例如：#ff0000\r\n" + "PA 点透明度，范围为0到100\r\n"
			+ "psColor 预选点颜色，颜色为16进制颜色。例如：#ffff00\r\n"
			+ "PSA 预选点透明度，范围为0到100\r\n" + "lSize 线宽度\r\n"
			+ "lColor 线颜色，预选点颜色，颜色为16进制颜色。例如：#00ff00\r\n"
			+ "LA 线透明度，范围为0到100\r\n"
			+ "lsColor 预选线颜色，预选点颜色，颜色为16进制颜色。例如：#00ff00\r\n"
			+ "LSA 预选线透明度，范围为0到100";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'lineOpen(pSize,pColor,PA,psColor,PSA,lSize,lColor,LA,lsColor,LSA)',
					'无', '按住键盘shift和点击鼠标左键拖动会在屏幕上标绘出一些线', paramer);
	hadediagl();
}

/**
 * 线标绘取消
 */
function lineCancelDemo() {
	parent.lineCancel();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'lineCancel()', '无', '取消线标绘', paramer);
	hadediagl();
}

/**
 * 线标绘关闭
 */
function lineCloseDemo() {
	parent.lineClose();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'lineClose()', '无', '关闭线标绘', paramer);
	hadediagl();
}

/**
 * 线标绘完成
 */
function lineCompleteDemo() {
	parent.lineComplete();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'lineComplete()', '无', '完成线标绘',
			paramer);
	hadediagl();
}

/**
 * 面标绘开启
 */
function surfaceOpenDemo() {
	parent.surfaceOpen("7", "#ff0000", "0.7", "#ffff00", "0.7", "5", "#00ff00",
			"0.7", "#00ff00", "0.7", "#0000ff", "0.7", "#ff00ff", "0.7");
	var paramer = "js参数说明：\r\n" + "pSize 点所显示的大小\r\n"
			+ "pColor 点颜色，颜色为16进制颜色。例如：#ff0000\r\n" + "PA 点透明度，范围为0到100\r\n"
			+ "psColor 预选点颜色，颜色为16进制颜色。例如：#ffff00\r\n"
			+ "PSA 预选点透明度，范围为0到100\r\n" + "lSize 线宽度\r\n"
			+ "lColor 线颜色，颜色为16进制颜色。例如：#ff0000\r\n" + "LA 线透明度，范围为0到100\r\n"
			+ "lsColor 预选线颜色，颜色为16进制颜色。例如：#ff0000\r\n"
			+ "LSA 预选线透明度，范围为0到100\r\n" + "sColor 面颜色，颜色为16进制颜色。例如：#0000ff\r\n"
			+ "SA 面透明度，范围为0到100\r\n" + "ssColor 预选面颜色，颜色为16进制颜色。例如：#ff00ff\r\n"
			+ "SSA 预选面透明度，范围为0到100";
	parent
			.getMethodInfo(
					'sceneIJas.js',
					'surfaceOpen(pSize,pColor,PA,psColor,PSA,lSize,lColor,LA,lsColor,LSA,sColor,SA,ssColor,SSA)',
					'无', '按住键盘shift和点击鼠标左键拖动会在屏幕上标绘出一些面', paramer);
	hadediagl();
}

/**
 * 面标绘取消
 */
function surfaceCancelDemo() {
	parent.surfaceCancel();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'surfaceCancel()', '无', '取消面标绘',
			paramer);
	hadediagl();
}

/**
 * 面标绘关闭
 */
function surfaceCloseDemo() {
	parent.surfaceClose();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'surfaceClose()', '无', '关闭线标绘',
			paramer);
	hadediagl();
}

/**
 * 面标绘完成
 */
function surfaceCompleteDemo() {
	parent.surfaceComplete();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'surfaceComplete()', '无', '完成线标绘',
			paramer);
	hadediagl();
}

/**
 * 地表透明度设置
 */
function diaphaneityOffDemo() {
	parent.diaphaneityOff();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'diaphaneityOff()', '无', '关于地表的透明度设置',
			paramer);
	hadediagl();
}

/**
 * 获取相机位置
 */
function getCameraPositionDemo() {
	var t = parent.getCameraPosition();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('scriptIJas.js', 'getCameraPosition()', t, '获取相机位置',
			paramer);
	hadediagl();
}

/**
 * 返回当前相机视野的地图比例
 */
function getMapScaleDemo() {
	var t = parent.getMapScale();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('scriptIJas.js', 'getMapScale()', t, '返回当前相机视野内的地图比例',
			paramer);
	hadediagl();
}

/**
 * 飞行
 */
function flyDemo() {
	alert("待确定");
}

/**
 * 获取脚本列表
 */
function getScriptListDemo() {
	var t = parent.getScriptList();
	var paramer = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('specialTopicIJas.js', 'getScriptList()', t, '获取脚本列表',
			paramer);
	hadediagl();
}

/**
 * 创建工具栏的引导按钮
 */
function createDockleaderDemo() {
	parent.createDockleader('msl_dockleader','20','40','10','50','50','20','20','20','20','/texture/leader_back.gif','0.3','2','/texture/show.png','/texture/hide.png');
	var mtp = "js参数说明：\r\n" + "identify 按钮的标识\r\n"+"initposx 初始位置X\r\n"+"initposy 初始位置Y\r\n"+"itemdistance 按钮的间距\r\n"+"iconwidth iconheight 按钮的大小\r\n"+"marginleft marginright margintop marginbottom 距离上下左右的边距\r\n"+"pic 背景图片\r\n"+"alpha 透明度\r\n"+"bindoffsetx 绑定时位置的屏幕坐标x偏移\r\n"+"showPic 工具栏正在显示时，显示的按钮图片名称\r\n"+"hidePic 工具栏正在隐藏时，显示的按钮图片名称";
	parent.getMethodInfo('uiIJas.js', 'createDockleader(identify,initposx,initposy,itemdistance,iconwidth,iconheight,marginleft,marginright,margintop,marginbottom,pic,alpha,bindoffsetx,showPic,hidePic)', '无', '创建工具栏的引导按钮',
			mtp);
	hadediagl();
}

/**
 * 隐藏工具栏的引导按钮
 */
function hideDockleaderDemo() {
	parent.hideDockleader();
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('uiIJas.js', 'hideDockleader()', '无', '隐藏工具栏的引导按钮',
			mtp);
	hadediagl();
}

/**
 * 显示工具栏的引导按钮
 */
function showDockleaderDemo() {
	parent.showDockleader();
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('uiIJas.js', 'showDockleader()', '无', '显示工具栏的引导按钮',
			mtp);
	hadediagl();
}

/**
 * 关闭工具栏的引导按钮
 */
function deleteDockleaderDemo(){
	parent.deleteMenu('dockleader', 'msl_dockleader');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'deleteMenu(type,targetidentify)', '无',
			'删除工具栏的引导按钮', mtp);
	hadediagl();
}

/**
 * 得到右键菜单当前选中项的索引值
 */
function returnMenuDemo() {
	var t = parent.returnMenu();
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('uiIJas.js', 'returnMenu()', t, '得到右键菜单当前选中项的索引值。',
			mtp);
	hadediagl();
}

/**
 * 开启采集器
 */
function openCollectorDemo() {
	parent.openCollector();
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('aidIJas.js', 'openCollector()', '无', '开启采集器。', mtp);
	hadediagl();
}

/**
 * 返回采集器的位置及姿态信息
 */
function collectorInfoDemo() {
	var t=parent.collectorInfo();
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('aidIJas.js', 'collectorInfo()', t,
			'返回采集器的位置及姿态信息。', mtp);
	hadediagl();
}

/**
 * 关闭采集器
 */
function closeCollectorDemo() {
	parent.closeCollector();
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('aidIJas.js', 'closeCollector()', '采集器关闭', '关闭采集器。',
			mtp);
	hadediagl();
}
/**
 * 查询对象监听
 */ 
function selectInterfaceDemo() {
	parent.selectInterface(resultt);
	var mtp = "js参数说明：\r\n" + "无";
	parent.getMethodInfo('sceneIJas.js', 'selectInterface()', '无', '查询对象监听。',
			mtp);
	hadediagl();
}

/**
 * 无标题窗口
 */
function popnotitleDemo() {
	parent
			.popnotitle('xyzq1', '400', '50', '200', '300', 'http://www.baidu.com',
					'128');
	var mtp = "js参数说明：\r\n" + "name 窗口名（窗口的唯一标示）\r\n"
			+ "x,y 以地图左上角为原点，对应X，Y轴的位置\r\n" + "width 窗口的宽度\r\n"
			+ "height 窗口的高度\r\n" + "url 窗口中内容的地址\r\n" + "n 窗口透明度（0到100）";
	parent.getMethodInfo('uiIJas.js',
			'popnotitle(name,x,y,width,height,url,n)', '无', '在三维场景中创建一个无标题的窗口',
			mtp);
	hadediagl();
}

function closePopnotitleDemo(){
	parent.closewindowbyname('xyzq1');
}

/**
 * 有标题窗口
 */
function poptitlewindowDemo() {
	parent.poptitlewindow('xyzq2', '50', '50', '200', '300', '有标题栏弹窗',
			'http://www.baidu.com', '128');
	var mtp = "js参数说明：\r\n" + "name 窗口名（窗口的唯一标示）\r\n"
			+ "x,y 以地图左上角为原点，对应X，Y轴的位置\r\n" + "width 窗口的宽度\r\n"
			+ "height 窗口的高度\r\n" + "title 窗口标题名\r\n" + "url 窗口中内容的地址\r\n"
			+ "n 窗口透明度（0到100）";
	parent.getMethodInfo('uiIJas.js',
			'poptitlewindow(name,x,y,width,height,title,url,n)', '无',
			'在场景中创建一个带有标题的窗口', mtp);
	hadediagl();
}

function closePoptitlewindowDemo(){
	parent.closewindowbyname('xyzq2');
}

/**
 * 固定中心弹窗
 */
function popstonewindowDemo() {
	parent.popstonewindow('xyzq3', '200', '300', '固定中心弹窗', 'http://www.baidu.com',
			'128');
	var mtp = "js参数说明：\r\n" + "name 窗口名（窗口的唯一标示）\r\n"
			+ "x,y 以地图左上角为原点，对应X，Y轴的位置\r\n" + "width 窗口的宽度\r\n"
			+ "height 窗口的高度\r\n" + "title 窗口标题名\r\n" + "url 窗口中内容的地址\r\n"
			+ "n 窗口透明度（0到100）";
	parent.getMethodInfo('uiIJas.js',
			'popstonewindow(name,width,height,title,url,n)', '无',
			'在三维场景中创建一个固定中心的窗口', mtp);
	hadediagl();
}

function closePopstonewindowDemo(){
	parent.closewindowbyname('xyzq3');
}

/**
 * 创建一个无标题可拖动的窗口
 */
function popupDragUIHtmlWindowDemo(){
	parent.popupDragUIHtmlWindow('xyzq4', '50', '50', '200', '300',
			'http://www.baidu.com', '50');
	var mtp = "js参数说明：\r\n" + "name 窗口name（窗口的唯一标示）\r\n"
			+ "x,y 以地图左上角为原点，对应X，Y轴的位置\r\n" + "width 窗口的宽度\r\n"
			+ "height 窗口的高度\r\n"+ "url 窗口中内容的地址\r\n"
			+ "n 窗口透明度（0到100）";
	parent.getMethodInfo('uiIJas.js',
			'popupDragUIHtmlWindowDemo(name,x,y,width,height,url,n)', '无',
			'在场景中创建一个无标题可拖动的窗口', mtp);
	hadediagl();
}

function closePopupDragUIHtmlWindowDemo(){
	parent.closewindowbyname('xyzq4');
}

/**
 * 有标题栏弹窗(新)
 */
function popupDBHtmlWindowDemo(){
	parent.popupDragBuinessHtmlWindow('xyzq5', '50', '50', '200', '300', '有标题栏弹窗',
			'http://www.baidu.com', '50');
	var mtp = "js参数说明：\r\n" + "name 窗口name（窗口的唯一标示）\r\n"
			+ "x,y 以地图左上角为原点，对应X，Y轴的位置\r\n" + "width 窗口的宽度\r\n"
			+ "height 窗口的高度\r\n" + "title 窗口标题名\r\n" + "url 窗口中内容的地址\r\n"
			+ "n 窗口透明度（0到100）";
	parent.getMethodInfo('uiIJas.js',
			'poptitlewindow(name,x,y,width,height,title,url,n)', '无',
			'在场景中创建一个带有标题的窗口', mtp);
	hadediagl();
}

function closePopupDBHtmlWindowDemo(){
	parent.closewindowbyname('xyzq5');
}

/**
 * 固定中心弹窗（新）
 */
function popupDBHtmlWindowCenteredDemo(){
	parent.popupDragBuinessHtmlWindowCentered('xyzq6','200', '300', '固定中心弹窗', 'http://www.baidu.com',
	'50');
var mtp = "js参数说明：\r\n" + "name 窗口name（窗口的唯一标示）\r\n"
	+ "width 窗口的宽度\r\n"
	+ "height 窗口的高度\r\n" + "title 窗口标题名\r\n" + "url 窗口中内容的地址\r\n"
	+ "n 窗口透明度（0到100）";
parent.getMethodInfo('uiIJas.js',
	'popstonewindow(name,width,height,title,url,n)', '无',
	'在三维场景中创建一个固定中心的窗口', mtp);
hadediagl();
}

function closePopupDBHtmlWindowCenteredDemo(){
	parent.closewindowbyname('xyzq6');
}
/**
 * 给HTML弹出窗口增加设定拖动范围
 */
function setwindowDemo(){
	parent.setHtmlWindowDragRect('xyzq4', '0', '0', '100', '100');
    var mtp = "js参数说明：\r\n" + "name 窗口name（窗口的唯一标示）\r\n"
	+ "x：可拖动范围的左边的距离\r\n"+"y ：可拖动范围的上边的边距\r\n" + "width 可推动范围的宽度\r\n"
	+ "height 可拖动范围的高度\r\n"+"设置的范围为：弹出窗口左上角100*100的范围\r\n";
parent.getMethodInfo('uiIJas.js',
	'setHtmlWindowDragRect(name, x, y, width,height)', '无',
	'给HTML弹出窗口增加设定拖动范围，即是鼠标事件能响应的范围（在弹出窗上）', mtp);
hadediagl();
}
/**
 * 给HTML弹出窗口重新设定位置大小
 */
function setHtmlWindowRect(){
	parent.setHtmlWindowRect('xyzq6', '0', '0', '50', '50');
	var mtp = "js参数说明：\r\n" + "name 窗口name（窗口的唯一标示）\r\n"
	+ "x：可拖动范围的左边的距离\r\n"+"y ：可拖动范围的上边的边距\r\n" + "width 宽度\r\n"
	+ "height 高度\r\n"+"";
	alert('test');
	parent.getMethodInfo('uiIJas.js',
			'setHtmlWindowRect(name, x, y, width,height)', '无',
			'给HTML弹出窗口重新设定位置大小', mtp);
	hadediagl();
}

/**
 * 将创建的窗口进行关闭
 */
function closewindowbynameDemo() {
	parent.closewindowbyname('xyzq');
	var mtp = "js参数说明：\r\n" + "name 窗口名（窗口的唯一标示）";
	parent.getMethodInfo('uiIJas.js', 'closewindowbyname(name)', '无',
			'将创建的窗口进行关闭', mtp);
	hadediagl();
}

/**
 * 创建列表菜单
 */
function createListmenuDemo() {
	var menu1 = new Menu('item1', '/texture/b_3.png', '数据展示');
	var menu2 = new Menu('item2', '/texture/b_2.png', '培训演练');
	var menu3 = new Menu('item3', '/texture/b_2.png', '弹出窗口1');
	var menu4 = new Menu('item4', '/texture/b_2.png', '弹出窗口2');
	var menu5 = new Menu('item5', '/texture/b_2.png', '弹出窗新的调用方法');
	var arrayMenu = [ menu1, menu2,menu3,menu4,menu5 ];
	parent.createListmenu('boxmenu', arrayMenu, 'box_1', '20', '100', '10',
			'20', '20', '0', '0', '', '0.3', '#00ff00', '0.7', '5', '0.4',
			'/texture/b_0.png', '0.8', '50', '35', '0', '0', '0', '0', 'hello',
			'#000000', '0.8', '13', 'msyh.ttf', '170', '35', '10', '10', '5',
			'5', '/texture/button_back.gif', '0.3', '#00ff00', '0.5',
			'#ffffff', '0.8', '21', '#ffffff', '0.8', '24', '#ffffff', '0.8',
			'21');
	var mtp = "js参数说明：\r\n"
			+ "type 表示待创建UI的类型，控件会根据接收到的这个类型，解析具体的UI配置，生成相应的UI\r\n"
			+ "arrayMenu 链式菜单内容对象，构造方法function Menu(id,picture,title)，例如：id='item1'，pinture='/texture/b_3.png'，title='数据展示'\r\n"
			+ "id 链式菜单的标识，例如：id='box_1'\r\n"
			+ "(initposx,initposy) 菜单的初始位置\r\n"
			+ "itemdistance 菜单中元素的间距\r\n"
			+ "(marginleft,marginright,margintop,marginbottom) 菜单中元素距离上下左右的边距\r\n"
			+ "picB 菜单项的背景图片\r\n" + "alphaB 菜单项的背景透明度\r\n"
			+ "color 菜单项的背景颜色\r\n" + "coloralphaA 菜单项的背景颜色透明度\r\n"
			+ "shownum 只显示部分按钮时，显示按钮的个数，只能为整数\r\n"
			+ "slidetime 只显示部分按钮，通过滚动显示其他按钮时，滚动时间(s)\r\n" + "picI 菜单项的图标\r\n"
			+ "alphaB 菜单项的图标透明度\r\n" + "widthI 图标的宽度\r\n" + "heightI 图标的高度\r\n"
			+ "(padleftI,padrightI,padtopI,padbottomI) 图标距离上下左右的边距\r\n"
			+ "label 文本\r\n" + "colorA 文本颜色\r\n" + "coloralphaB 文本颜色透明度\r\n"
			+ "fontsize 字体大小\r\n" + "fontname 字体样式\r\n" + "width 文本的宽度\r\n"
			+ "height 文本的高度\r\n"
			+ "(padleft,padright,padtop,padbottom) 文本距离上下左右的边距\r\n"
			+ "picT 背景图片\r\n" + "alphaI 透明度\r\n" + "colorI 颜色\r\n"
			+ "coloralphaI 透明度\r\n"
			+ "(colorN,coloralphaN,fontsizeN) 鼠标未对菜单做任何操作时文本字体的颜色、透明度和大小\r\n"
			+ "(colorF,coloralphaF,fontsizeF) 鼠标移入菜单时文本字体的颜色、透明度和大小\r\n"
			+ "(colorC,coloralphaC,fontsizeC) 鼠标点击菜单时文本字体的颜色、透明度和大小";
	parent
			.getMethodInfo(
					'uiIJas.js',
					'createListmenu(type,tableMenu,id,initposx,initposy,rowspace,colspace,marginleft,marginright,margintop,marginbottom,picB,alphaB,colorredB,coloralphaB,picM,alphaM,colorredM,coloralphaM,picC,alphaC,widhtC,heightC,padleftC,padrightC,padtopC,padbottomC,label,colorredC,coloralphaC,fontsizeC,fontnameC,width,height,padleft,padright,padtop,padbottom,colorredN,coloralphaN,fontsizeN,colorredF,coloralphaF,fontsizeF,colorredI,coloralphaI,fontsizeC)',
					'无', '创建一个列表菜单', mtp);
	hadediagl();
}

/**
 * 删除列表菜单
 */
function deleteMenuDemo() {
	parent.deleteMenu('boxmenu', 'box_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'deleteMenu(type,targetidentify)', '无',
			'删除列表菜单', mtp);
	hadediagl();
}

/**
 * 隐藏列表菜单
 */
function hideMenuDemo() {
	parent.hideMenu('boxmenu', 'box_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'hideMenu(type,targetidentify)', '无',
			'对创建的列表菜单进行隐藏控制', mtp);
	hadediagl();
}

/**
 * 显示列表菜单
 */
function showMenuDemo() {
	parent.showMenu('boxmenu', 'box_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'showMenu(type,targetidentify)', '无',
			'显示创建的列表菜单', mtp);
	hadediagl();
}

/**
 * 设置列表菜单的位置
 */
function setMenuPositionDemo() {
	parent.setMenuPosition('boxmenu', 'box_1', '20', '20');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示\r\n"
			+ "x,y 链式菜单的左上角点的位置（以地图窗口左上角为原点）";
	parent.getMethodInfo('uiIJas.js',
			'setMenuPosition(type,targetidentify,x,y)', '无', '设置列表菜单的位置', mtp);
	hadediagl();
}

/**
 * 设置列表菜单的属性
 */
function setMenuProDemo() {
	parent.setMenuPro('boxmenu', 'box_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'setMenuPro(type,targetidentify)', '无',
			'设置列表菜单的属性', mtp);
	hadediagl();
}

/**
 * 获取列表菜单的状态
 */
function getMenuStateDemo() {
	var list = parent.getMenuState('boxmenu', 'box_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示\r\n"
			+ "x,y 链式菜单的左上角点的位置（以地图窗口左上角为原点）";
	parent.getMethodInfo('menuIjas.js', 'getMenuState(type,targetidentify)',
			list, '获取列表菜单的状态', mtp);
	hadediagl();
}

/**
 * 设置菜单中的单个按钮元素的显示状态
 */
function setMenuButtonStateDemo() {
	parent.setMenuButtonState('boxmenu', 'box_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示\r\n"
			+ "x,y 链式菜单的左上角点的位置（以地图窗口左上角为原点）";
	parent.getMethodInfo('uiIJas.js',
			'setMenuButtonState(type,targetidentify)', '无',
			'设置菜单中的单个按钮元素的显示状态', mtp);
	hadediagl();
}

/**
 * 创建列表菜单的引导按钮
 */
function createBoxLeaderDemo() {
	parent.createBoxLeader('boxleader','boxleader_1','15','480','10','30','30','/texture/leader_back.gif','0.7','box_1','2','/texture/show.png','/texture/hide.png','/texture/up.png','/texture/down.png');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n"+"identify 按钮的标识\r\n"+"initposx initposy 初始位置\r\n"+"itemdistance 按钮的间距\r\n"+"iconwidth iconheight 按钮的大小\r\n"+"pic 背景图片\r\n"
		+"alpha 透明度\r\n"
		+"bindbox 与之绑定的菜单的标识符\r\n"
	    +"bindoffsetx 绑定时位置的屏幕坐标x偏移\r\n"
		+"showPic 菜单正在显示时，显示的按钮图片名称\r\n"
		+"hidePic 菜单正在隐藏时，显示的按钮图片名称\r\n"
		+"upPic 向上滚动按钮显示的图片名称\r\n"
		+"downPic 向下滚动按钮显示的图片名称";
	parent.getMethodInfo('uiIJas.js', 'createBoxLeader(type,identify,initposx,initposy,itemdistance,iconwidth,iconheight,pic,alpha,bindbox,bindoffsetx,showPic,hidePic,upPic,downPic)', '无',
			'创建列表菜单的引导按钮', mtp);
	hadediagl();
}

/**
 * 关闭列表式菜单
 */
function deleteBoxLeaderDemo(){
	parent.deleteMenu('boxleader', 'boxleader_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'deleteMenu(type,targetidentify)', '无',
			'删除列表式菜单', mtp);
	hadediagl();
}

/**
 * 创建表格样式的菜单
 */
function createTableMenuDemo() {
	var tableText1 = new TableText('item1', '中盈', '/texture/b_1.png', '1', '1');
	var tableText2 = new TableText('item2', '高科', '/texture/b_2.png', '2', '1');
	var tableMenu = [ tableText1, tableText2 ];
	parent.createTableMenu('tablemenu', tableMenu, 'table_1', '100', '50',
			'30', '30', '5', '5', '10', '40', '/texture/button_back.gif',
			'0.3', '#346712', '0.5', '/texture/normal.png', '0.7', '#00ff00',
			'0.5', '/texture/b_0.png', '0.8', '50', '20', '0', '0', '0', '0',
			'hello', '#000000', '0.8', '13', 'msyh.ttf', '70', '20', '10',
			'10', '10', '10', '#ff0f0f', '0.8', '13', '#0000ff', '0.8', '19',
			'#ff0000', '0.8', '17');
	var mtp = "js参数说明：\r\n"
			+ "type 表示待创建UI的类型，控件会根据接收到的这个类型，解析具体的UI配置，生成相应的UI\r\n"
			+ "tableMenu 表格菜单内容对象，构造方法TableText(id,value,pic,row,column)，例如：id='item1',value='中盈',pic='/texture/b_1.png',row='1',column='1'\r\n"
			+ "id 表格菜单的标识，例如：id='table_1'\r\n"
			+ "(initposx,initposy) 菜单的初始位置\r\n"
			+ "(rowspace,colspace) 行间距与列间距\r\n"
			+ "(marginleft,marginright,margintop,marginbottom) 边距\r\n"
			+ "picB 文本的背景图片\r\n" + "alphaB 文本的背景透明度\r\n"
			+ "colorredB 文本的背景颜色\r\n" + "coloralphaB 文本的背景颜色透明度\r\n"
			+ "picM 背景图片\r\n" + "alphaM 透明度\r\n" + "colorredM 颜色\r\n"
			+ "coloralphaM 颜色的透明度\r\n" + "picC 图标的背景图片\r\n" + "alphaC 透明度\r\n"
			+ "widthC 图标的宽度\r\n" + "heightC 图标的高度\r\n"
			+ "(padleftC,padrightC,padtopC,padbottomC) 图标距离上下左右的边距\r\n"
			+ "label 文本\r\n" + "colorredC 文本颜色\r\n"
			+ "coloralphaC 文本颜色的透明度\r\n" + "fontsizeC 字体大小\r\n"
			+ "fontnameC 字体样式\r\n" + "width 文本的宽度\r\n" + "height 文本的高度\r\n"
			+ "(padleft,padright,padtop,padbottom) 文本距离上下左右的边距\r\n"
			+ "(colorN,coloralphaN,fontsizeN) 鼠标未对菜单做任何操作时文本字体的颜色、透明度和大小\r\n"
			+ "(colorF,coloralphaF,fontsizeF) 鼠标移入菜单时文本字体的颜色、透明度和大小\r\n"
			+ "(colorI,coloralphaI,fontsizeI) 鼠标点击菜单时文本字体的颜色、透明度和大小";
	parent
			.getMethodInfo(
					'uiIJas.js',
					'createTableMenu(type,tableMenu,id,initposx,initposy,rowspace,colspace,marginleft,marginright,margintop,marginbottom,picB,alphaB,colorredB,coloralphaB,picM,alphaM,colorredM,coloralphaM,picC,alphaC,widhtC,heightC,padleftC,padrightC,padtopC,padbottomC,label,colorredC,coloralphaC,fontsizeC,fontnameC,width,height,padleft,padright,padtop,padbottom,colorredN,coloralphaN,fontsizeN,colorredF,coloralphaF,fontsizeF,colorredI,coloralphaI,fontsizeI)',
					'无', '创建表格样式的菜单', mtp);
	hadediagl();
}

/**
 * 关闭列表式菜单
 */
function deleteTableMenuDemo(){
	parent.deleteMenu('tablemenu', 'table_1');
	var mtp = "js参数说明：\r\n" + "type 菜单类型\r\n" + "targetidentify 窗口的唯一标示";
	parent.getMethodInfo('uiIJas.js', 'deleteMenu(type,targetidentify)', '无',
			'删除表格式菜单', mtp);
	hadediagl();
}

/**
 * 开启标绘监听
 */
function drawCallBackDemo() {
	parent.drawCallBack(methodInfo);
	var mtp = "js参数说明：\r\n" + "methodInfo 回调函数";
	parent.getMethodInfo('sceneIJas.js', 'drawCallBack(methodInfo)', '无',
			'开启标绘监听', mtp);
	hadediagl();

}

/**
 * 创建链式菜单监听
 */
function RegListmenuBackDemo(recallFunc) {
	parent.RegListmenuBack(recallFunc);
	var mtp = "js参数说明：\r\n" + "recallFunc 回调函数";
	parent.getMethodInfo('uiIJas.js', 'RegListmenuBack(recallFunc)', '无',
			'创建监听', mtp);
	hadediagl();
}

/**
 * 将菜单响应事件监听关闭
 */
function UnRegDemo(name) {
	parent.UnReg(name);
	var mtp = "js参数说明：\r\n" + "UnReg_id 传入值为要结束监听的ID";
	parent.getMethodInfo('uiIJas.js', 'UnReg(UnReg_id)', '无', '将菜单响应事件监听关闭',
			mtp);
	hadediagl();
}

/**
 * 方法信息
 */
function methodInfo(s) {
	$('#jsPackage').val('无 ');
	$('#jsMethod').val('无');
	$('#returnValue').val(s);
	alert(s);
}



function GPStest(){
	
	var t = parent.TGPS();
	alert(t);
}

function openGps(){
	parent.setCameraPosition("116.5180050558","39.3597251118","432388.4224708965","0.288036","-90.000000","0.000000","普通");
	bolmt="1";
	//alert(1);
	//parent.selectInterface(resultt);
	//parent.regGPSBack(a);
	parent.RegGPSBack(gpsInfor);
	setTimeout("openGps()",3000);
	
}
var bolmt="-1";
var num = 0;
function gpsInfor(result){
	//alert(e);
	if(bolmt=="1"){
		var xmlDoc = $.parseXML(result);
		var domObject = $(xmlDoc).find('串口信息');
		var content = $(domObject).attr("内容");
		if(content.indexOf("$GPGLL")!=-1){
			bolmt="-1";
			content = content.split(",");
			//alert(content[7]);
			var lat = content[1];
			latdd = lat.slice(0,2);
			latmm = lat.slice(2,lat.length);
			lat = parseFloat(latdd) + parseFloat(latmm/60);
			var lon = content[3];
			londd = lon.slice(0,3);
			lonmm = lon.slice(3,lon.length);
			lon = parseFloat(londd) + parseFloat(lonmm/60);
			var a = lon+" "+lat;
			parent.getMethodInfo('sysytemIJas.js', 'RegGPSBack(recallFunc)', a,'得到串口信息',"回调函数");
			//alert(num);
			parent.drawPoint('0',num,'123',lon,lat,'','#ff0000','100','20');
			num--;
			//alert(a);
		}
	}
	//setTimeout("gpsInfor('"+result+"')",3000);
	
}

function GPSclose(){
	var t = parent.CGPS();
	alert(t);
}
