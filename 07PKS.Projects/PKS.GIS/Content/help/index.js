var bb=null;
$(function() {
	changeLayout();
	//$("#layerTree").height( $("#tree").height()-$("#locationDIV").height());
	var reg=new RegExp("(^| )name=([^;]*)(;|$)");
	var str=document.cookie;
	var userName=str.match(reg);
	// if(userName==null){
		// window.location.href="index.html";
	// }
	createTree();
		if (document.documentElement.style["border-radius"] == undefined) {
		var fileref = document.createElement('script');
		fileref.setAttribute("type", "text/javascript");
		fileref.setAttribute("src", "help/PIE.js");
		if (typeof fileref != "undefined")
		document.getElementsByTagName("head")[0].appendChild(fileref);
		$(".yj").css("border-color","#385156 #ccf3fc #ccf3fc #385156");
		$('.yj').each(function() {
			PIE.attach(this);
		});
	}
	$('#ff').combobox({
        valueField: 'value',
        textField: 'label',
        data: aa,
        onChange:function(newValue,oldValue){
        	//alert("d");
    		bb=new Array();
        	var nweText = $('#ff').combobox('getText');
        	var n=0;
        	if(nweText==null||nweText==""){
            	//alert("空");
        		 $('#ff').combobox('loadData',aa);
            	}else{
        	for(var i=0;i<aa.length;i++){
            	//alert(nweText+"输入");
            	if((aa[i].label).indexOf(nweText)>=0){
                	//alert("d");
                	bb[n]=aa[i];
                	n++;
              //	alert(bb[0].label);
                	}
            	}
        	$('#ff').combobox('loadData',bb);
        	setTimeout("update()", 0);
        	// $('#ff').combobox('showPanel',true);
            	}
            },
            onSelect:function(){
            	//alert($('#ff').combobox('select'));
            	//alert("dd");
            	setTimeout("update1()", 0);
            }
        });
$("#JSMethod0101").hide();
$("#JSPackage0101").hide();

var dad = document.getElementById("cc");
$("a[name='subMenu']").click(function(item){
			$("a[name='subMenu']").each(function(){
				//$(this).css("background","url(image/menu1.png)");
			});
			//$(this).css("background","url(image/menu2.png)");
		});
});


/**
 * 模糊查询后，更新下拉面板的内容
 */
function update(){
	$('#ff').combobox('loadData',bb);

	}
/**
 * 选中查询后，恢复下拉面板的内容
 */
function update1(){
	$('#ff').combobox('loadData',aa);
}
/**
 * aa为初始化combobox下拉菜单对应的对象
 * label：为combobox的text。
 * value：为combobox的value。注：value有三部分组成：大模块id_手风琴id_功能连接id
 */
   var aa=[{
		label: 'UI功能',
		value: 'uiIJas_0_0'
	},{
		label: '窗口',
		value: 'uiIJas_popMenu05_0'
	},{
		label: '无标题栏弹窗',
		value: 'uiIJas_popMenu05_pottitle05'
	},{
		label: '有标题栏弹窗',
		value: 'uiIJas_popMenu05_popWind05'
	},{
		label: '固定中心弹窗',
		value: 'uiIJas_popMenu05_popstonN05'
	},{
		label: '关闭窗口',
		value: 'uiIJas_popMenu05_closeWind05'
	},{
		label: '链式菜单',
		value: 'uiIJas_listM05_0'
	},{
		label: '创建链式菜单',
		value: 'uiIJas_listM05_createList05'
	},{
		label: '创建链式菜单监听',
		value: 'uiIJas_listM05_RegLB05'
	},{
		label: '移除链式菜单监听',
		value: 'uiIJas_listM05_unReg05'
	},{
		label: '关闭链式菜单',
		value: 'uiIJas_listM05_deleteM05'
	},{
		label: '隐藏链式菜单',
		value: 'uiIJas_listM05_hideMD05'
	},{
		label: '显示链式菜单',
		value: 'uiIJas_listM05_showM05'
	},{
		label: '设置位置链式菜单',
		value: 'uiIJas_listM05_seMPoi05'
	},{
		label: '设置属性链式菜单',
		value: 'uiIJas_listM05_setMP05'
	},{
		label: '获取状态链式菜单',
		value: 'uiIJas_listM05_getM05'
	},{
		label: '设置链式菜单按钮',
		value: 'uiIJas_listM05_setM05'
	},{
		label: '列表式菜单',
		value: 'uiIJas_boxLeab05_0'
	},{
		label: '创建列表式菜单',
		value: 'uiIJas_boxLeab05_createBoxL05'
	},{
		label: '创建列表式菜单',
		value: 'uiIJas_tableMenu05_0'
	},{
		label: '创建表格式菜单',
		value: 'uiIJas_tableMenu05_createtableMenu05'
	},{
		label: '工具栏引导按钮',
		value: 'uiIJas_menu05_0'
	},{
		label: '创建工具栏的引导按钮',
		value: 'uiIJas_menu05_createDocke05'
	},{
		label: '隐藏工具栏的引导按钮',
		value: 'uiIJas_menu05_hideDock05'
	},{
		label: '显示工具栏的引导按钮',
		value: 'uiIJas_menu05_shouDock05'
	},{
		label: '右键菜单',
		value: 'uiIJas_mxap_ee2'
	},{
		label: '获取右键菜单响应信息',
		value: 'uiIJas_mxap_returnMenu05'
	},{
		label: '辅助功能',
		value: 'aidIJas_0_0'
	},{
		label: '坐标转换',
		value: 'aidIJas_xy405_0'
	},{
		label: '坐标转换(西安80坐标和WGS84坐标)',
		value: 'aidIJas_xy405_xy4'
	},{
		label: '播放音乐',
		value: 'aidIJas_musicid05_musicid'
	},{
		label: '抓图',
		value: 'aidIJas_hh05_hh2'
	},{
		label: '截图',
		value: 'aidIJas_hh05_hh4'
	},{
		label: '选择本地文件',
		value: 'aidIJas_hh305_hh3'
	},{
		label: '采集器',
		value: 'aidIJas_collecton05_0'
	},{
		label: '开启采集器',
		value: 'aidIJas_collecton05_openColl05'
	},{
		label: '查询采集器',
		value: 'aidIJas_collecton05_collect05'
	},{
		label: '关闭采集器',
		value: 'aidIJas_collecton05_closeCollect05'
	},{
		label: '计算分析',
		value: 'calculateIJas_0_0'
	},{
		label: '命中测试',
		value: 'calculateIJas_calculate05_xy'
	},{
		label: '世界与厂区坐标转换(世界到厂区)',
		value: 'calculateIJas_calculate05_idxyz'
	},{
		label: '世界与厂区坐标转换(厂区到世界)',
		value: 'calculateIJas_calculate05_idxyz1'
	},{
		label: '获取屏幕坐标对应经纬高',
		value: 'calculateIJas_calculate05_xy1'
	},{
		label: '获取指定点到长距离管线的投影坐标',
		value: 'calculateIJas_calculate05_xy2'
	},{
		label: '计算高度',
		value: 'calculateIJas_calculate05_xy3'
	},{
		label: '漫游管理',
		value: 'scriptIJas_0_0'
	},{
		label: '定位',
		value: 'scriptIJas_popobject05_0'
	},{
		label: '对象定位',
		value: 'scriptIJas_popobject05_objectTest'
	},{
		label: '企业定位',
		value: 'scriptIJas_popobject05_dialogBusiness'
	},{
		label: '设置相机位置姿态',
		value: 'scriptIJas_popobject05_mcamera'
	},{
		label: '获取相机位置姿态',
		value: 'scriptIJas_popobject05_getCamer05'
	},{
		label: '根据二维地图比例设置三维相机的位置姿态',
		value: 'scriptIJas_popobject05_dialogCam'
	},{
		label: '返回当前相机视野内的地图比例',
		value: 'scriptIJas_popobject05_getMapS05'
	},{
		label: '路径漫游',
		value: 'scriptIJas_flyy05_0'
	},{
		label: '飞行',
		value: 'scriptIJas_flyy05_fly'
	},{
		label: '操作器管理',
		value: 'scriptIJas_guanli05_0'
	},{
		label: '漫游器开关',
		value: 'scriptIJas_guanli05_conoff'
	},{
		label: '操作器切换',
		value: 'scriptIJas_guanli05_zonoff01'
	},{
		label: '专题功能',
		value: 'specialTopicIJas_zhuantitu05_0'
	},{
		label: '桌面推演',
		value: 'specialTopicIJas_zhuantitu05_tuiyanid'
	},{
		label: '事件处理器管理开启',
		value: 'specialTopicIJas_zhuantitu05_qq1'
	},{
		label: '事件处理器管理关闭',
		value: 'specialTopicIJas_zhuantitu05_qq2'
	},{
		label: '获取脚本列表',
		value: 'specialTopicIJas_zhuantitu05_getScrip05'
	},{
		label: '播放指定脚本',
		value: 'specialTopicIJas_zhuantitu05_dialogScript'
	},{
		label: '系统管理',
		value: 'systemIJas_0_0'
	},{
		label: '运行管理',
		value: 'systemIJas_ruan05_0'
	},{
		label: '启动3D',
		value: 'systemIJas_ruan05_ruan3D05'
	},{
		label: 'About3D',
		value: 'systemIJas_ruan05_about3d05'
	},{
		label: '停止3D',
		value: 'systemIJas_ruan05_stop05'
	},{
		label: '暂停、启动三维渲染线程',
		value: 'systemIJas_ruan05_onoff'
	},{
		label: '配置管理',
		value: 'systemIJas_setMM05_0'
	},{
		label: '坐标系切换',
		value: 'systemIJas_setMM05_dmp'
	},{
		label: '光照位置',
		value: 'systemIJas_setMM05_zuobiao'
	},{
		label: '光照设置与查询',
		value: 'systemIJas_setMM05_setSun'
	},{
		label: '天气管理',
		value: 'systemIJas_weatherSet05_0'
	},{
		label: '天气控制',
		value: 'systemIJas_weatherSet05_weather'
	},{
		label: '天气状态',
		value: 'systemIJas_weatherSet05_weather05'
	},{
		label: '挂件管理',
		value: 'systemIJas_widget05_0'
	},{
		label: '挂件控制',
		value: 'systemIJas_widget05_widget'
	},{
		label: '挂件状态',
		value: 'systemIJas_widget05_weather05'
	},{
		label: '国际化',
		value: 'systemIJas_internalt05_0'
	},{
		label: '国际化地球',
		value: 'systemIJas_internalt05_dialogLanguage'
	},{
		label: '场景管理',
		value: 'sceneIJas_0_0'
	},{
		label: '信息查询',
		value: 'sceneIJas_selectFuntion05_0'
	},{
		label: '获取企业信息列表',
		value: 'sceneIJas_selectFuntion05_dialogObject1'
	},{
		label: '返回图层列表',
		value: 'sceneIJas_selectFuntion05_tuchegn05'
	},{
		label: '返回矢量图层信息',
		value: 'sceneIJas_selectFuntion05_vector1'
	},{
		label: '返回图层元素列表',
		value: 'sceneIJas_selectFuntion05_dd1'
	},{
		label: '返回选中对象ID',
		value: 'sceneIJas_selectFuntion05_selObj05'
	},{
		label: '返回选中对象信息列表',
		value: 'sceneIJas_selectFuntion05_selObjList05'
	},{
		label: '获取对象信息列表',
		value: 'sceneIJas_selectFuntion05_objectTest3'
	},{
		label: '获取已有LOD配置的对象列表',
		value: 'sceneIJas_selectFuntion05_getLod05'
	},{
		label: '开启查询接口事件监听',
		value: 'sceneIJas_selectFuntion05_selInter05'
	},{
		label: '接口查询',
		value: 'sceneIJas_selectFuntion05_selectBall'
	},{
		label: '根据模型名称查询模型ID',
		value: 'sceneIJas_selectFuntion05_modelName'
	},{
		label: '获取对象的经纬高',
		value: 'sceneIJas_selectFuntion05_dialogObject2'
	},{
		label: '获取对象定位参数',
		value: 'sceneIJas_selectFuntion05_dialogObject3'
	},{
		label: '对象创建',
		value: 'sceneIJas_creatObj05_0'
	},{
		label: '创建点可视对象',
		value: 'sceneIJas_creatObj05_drawPointMsl111'
	},{
		label: '创建线可视对象',
		value: 'sceneIJas_creatObj05_drawLineMsl111'
	},{
		label: '创建面可视对象',
		value: 'sceneIJas_creatObj05_drawSurfaceMsl111'
	},{
		label: '创建标牌可视对象',
		value: 'sceneIJas_creatObj05_seePanel121'
	},{
		label: 'Tip标牌',
		value: 'sceneIJas_creatObj05_tipPanl'
	},{
		label: 'Text标牌',
		value: 'sceneIJas_creatObj05_textPanl'
	},{
		label: 'HTML标牌',
		value: 'sceneIJas_creatObj05_htmlPanl'
	},{
		label: '添加模型',
		value: 'sceneIJas_creatObj05_objectTest10'
	},{
		label: '批量添加屏幕标注',
		value: 'sceneIJas_creatObj05_objectTest11PL'
	},{
		label: '屏幕标注',
		value: 'sceneIJas_creatObj05_objectTest11'
	},{
		label: '空间文字',
		value: 'sceneIJas_creatObj05_objectTest12'
	},{
		label: '空间图片',
		value: 'sceneIJas_creatObj05_objectTest13'
	},{
		label: '屏幕窗口',
		value: 'sceneIJas_creatObj05_objectTest14'
	},{
		label: '屏幕文字',
		value: 'sceneIJas_creatObj05_objectTest15'
	},{
		label: '屏幕图片',
		value: 'sceneIJas_creatObj05_objectTest16'
	},{
		label: '球',
		value: 'sceneIJas_creatObj05_objectTest17'
	},{
		label: '创建标牌',
		value: 'sceneIJas_creatObj05_runPanel'
	},{
		label: '画点线面',
		value: 'sceneIJas_creatObj05_hua111'
	},{
		label: '添加管道',
		value: 'sceneIJas_creatObj05_ee1'
	},{
		label: '地形剖切',
		value: 'sceneIJas_creatObj05_qq3'
	},{
		label: '设备剖切',
		value: 'sceneIJas_creatObj05_modelpao'
	},{
		label: '设备拆分',
		value: 'sceneIJas_creatObj05_modelchai'
	},{
		label: '开启标绘监听',
		value: 'sceneIJas_creatObj05_drawCall05'
	},{
		label: '点标绘开启',
		value: 'sceneIJas_creatObj05_pointOpen'
	},{
		label: '线标绘开启',
		value: 'sceneIJas_creatObj05_lineOpean05'
	},{
		label: '线标绘取消',
		value: 'sceneIJas_creatObj05_lineCance05'
	},{
		label: '线标绘关闭',
		value: 'sceneIJas_creatObj05_lineClo05'
	},{
		label: '线标绘完成',
		value: 'sceneIJas_creatObj05_lineCom05'
	},{
		label: '面标绘开启',
		value: 'sceneIJas_creatObj05_surfaceOpen05'
	},{
		label: '面标绘取消',
		value: 'sceneIJas_creatObj05_surfaCancel05'
	},{
		label: '面标绘关闭',
		value: 'sceneIJas_creatObj05_surfaceCl05'
	},{
		label: '面标绘完成',
		value: 'sceneIJas_creatObj05_surfCom05'
	},{
		label: '对象删除',
		value: 'sceneIJas_deletObj05_0'
	},{
		label: '对象自动释放',
		value: 'sceneIJas_deletObj05_setPanel'
	},{
		label: '对象删除',
		value: 'sceneIJas_deletObj05_dialogObject4'
	},{
		label: '对象属性编辑',
		value: 'sceneIJas_updateObj05_0'
	},{
		label: '更新标牌内容',
		value: 'sceneIJas_updateObj05_updatePanel'
	},{
		label: '添加Lod信息',
		value: 'sceneIJas_updateObj05_objectTest7'
	},{
		label: '修改屏幕标注',
		value: 'sceneIJas_updateObj05_hh1'
	},{
		label: '状态管理',
		value: 'sceneIJas_popOjb05_0'
	},{
		label: '对象动作',
		value: 'sceneIJas_popOjb05_objectTest2'
	},{
		label: '高亮对象',
		value: 'sceneIJas_popOjb05_highlight'
	},{
		label: '显示对象部件动作',
		value: 'sceneIJas_popOjb05_objectTest8'
	},{
		label: '设置企业显示',
		value: 'sceneIJas_popOjb05_enterTest'
	},{
		label: '查询企业显示',
		value: 'sceneIJas_popOjb05_dialogBusiness'
	},{
		label: '设置企业显隐',
		value: 'sceneIJas_popOjb05_enterTest2'
	},{
		label: '查询企业显隐',
		value: 'sceneIJas_popOjb05_dialogBusiness1'
	},{
		label: '图层控制',
		value: 'sceneIJas_popOjb05_dd2'
	},{
		label: '矢量图层控制',
		value: 'sceneIJas_popOjb05_vectorCon'
	},{
		label: '地形透明（开启）',
		value: 'sceneIJas_popOjb05_qq4'
	},{
		label: '地形透明（关闭）',
		value: 'sceneIJas_popOjb05_diaphan05'
	}];
//*****************控制选择功能模块按钮的状态**********
    function select4(){
	$("#JSMethod0101").hide();
    $("#canShu0101").hide();
	$("#JSPackage0101").hide();
	}
	function updateimage01(){
	document.getElementById("JSMethod01image").src="image/fangfa.png";
	document.getElementById("canShu01image").src="image/canshu.png";
	document.getElementById("JSPackage01image").src="image/jsbao.png";
	}
	function updateimage02(){
	document.getElementById("returnValue01image").src="image/resout.png";
	document.getElementById("functionHelp01image").src="image/xiangxi.png";
	}
    function select2(obj1){
	var idname02=obj1.getAttribute("id");
	select4();
	$("#"+idname02+"01").show();
	var image01="image/"+idname02+"image.png";
	updateimage01();
	document.getElementById(idname02+"image").src=image01;
	}
	function select5(){
	$("#returnValue0101").hide();
    $("#functionHelp0101").hide();
	}
    function select3(obj2){
	var idname03=obj2.getAttribute("id");
	select5();
	$("#"+idname03+"01").show();
	var image02="image/"+idname03+"image.png";
	updateimage02();
	document.getElementById(idname03+"image").src=image02;
	}
    /**
     * 控制iframe对应的资源路径（控制显示对应的html）
     */
	function select(obj){
    var idname=obj.getAttribute("id");
	var iframe3 = document.getElementById("iframe2");
	if(idname=="systemIJas"){
    iframe3.src="html/systemIJas.html";
	}else if(idname=="aidIJas"){
	iframe3.src="html/aidIJas.html";
	}else if(idname=="calculateIJas"){
	iframe3.src="html/calculateIJas.html";
	}else if(idname=="sceneIJas"){
	iframe3.src="html/sceneIJas.html";
	}else if(idname=="scriptIJas"){
	iframe3.src="html/scriptIJas.html";
	}else if(idname=="uiIJas"){
	iframe3.src="html/uiIJas.html";
	}else if(idname=="specialTopicIJas"){
	iframe3.src="html/specialTopicIJas.html";
	}else if(idname=="inputXml"){
		parent.getMethodInfo('aidIJas.js','runXmlMessage(message)','','运行XML命令','message: XML命令内容');
	     iframe3.src="html/inputXml.html";
	}
	else if(idname=="vectorIJas"){
	    iframe3.src="html/vectorIJas.html";
	}
	else if(idname=="vectoreditIJas"){
		 iframe3.src="html/vectoreditIJas.html";
	}
	else{
	iframe3.src="html/hello.html";
	}
	}
//********************结束***********************************************************************************
	/**
	 * 执行输入框内的xml命令
	 */
	function runXML(){
		var xmlMessage=$("#xmlMessage").val();
		var messageXML=parent.runXmlMessage(xmlMessage);
		if(messageXML==null){
			messageXML="";
		}
		parent.getMethodInfo('aidIJas.js','runXmlMessage(message)',messageXML,'运行XML命令','message: XML命令内容');
	}
	/**
	 * 清空xml命令输入框的内容
	 */
	function clearXML(){
		$("#xmlMessage").val('');
	}
	var mqu;
	var mqu01;
	/**
	 * 根据模糊查詢combobox選中的內容查詢接口
	 */
	function findIt() {
		var type01 = $('#ff').combobox('getValue');
		if(type01){
		var skuai01 = type01.split("_");
		$("a[name='subMenu']").each(function(){
			$(this).css("background","url(image/menu1.png)");
		});

			if(skuai01[0]!=0){
				var idname=skuai01[0];
				var iframe3 = document.getElementById("iframe2");
				var urlArry = iframe3.src.split("/");
				//alert(urlArry.length);
				var urlA=urlArry[urlArry.length-1];
				var urlB=urlA.split(".");
				var urlC=urlB[0];
				$('#'+idname).css("background","url(image/menu2.png)");
				if(urlC==idname){

				}else{
				if(idname=="systemIJas"){
			    iframe3.src="html/systemIJas.html";
				}else if(idname=="aidIJas"){
				iframe3.src="html/aidIJas.html";
				}else if(idname=="calculateIJas"){
				iframe3.src="html/calculateIJas.html";
				}else if(idname=="sceneIJas"){
				iframe3.src="html/sceneIJas.html";
				}else if(idname=="scriptIJas"){
				iframe3.src="html/scriptIJas.html";
				}else if(idname=="uiIJas"){
				iframe3.src="html/uiIJas.html";
				}else if(idname=="specialTopicIJas"){
				iframe3.src="html/specialTopicIJas.html";
				}else if(idname=="inputXml"){
					parent.getMethodInfo('aidIJas.js','runXmlMessage(message)','','运行XML命令','message: XML命令内容');
				iframe3.src="html/inputXml.html";
				}else{
				iframe3.src="html/hello.html";
				}
				}
			}

			if(skuai01[1]!=0){
				var iframe3 = document.getElementById("iframe2");
				mqu=skuai01[1];
				//alert(iframe3.src);
				//iframe2.window.mtpp();
				//console.log(iframe3);
				//console.log(window.frames["iframe2"]);
				setTimeout("ha()", 0);
				//window.frames["iframe2"].mtpp();
				//iframe3.mtpp();
				}
			mqu01=skuai01[2];
		}
			else{
				alert("请输入要搜索的接口名！");
			}
	}
	/**
	 * 調用子頁面方法
	 */
	function ha(){
		iframe2.window.mtpp(mqu,mqu01);
	}
	/*
	 * 关闭监听
	 */
	function stopEarth(){
		UnReg("0x00000002");
		UnReg("0x00000010");
		UnReg("0x00000800");
		stop3D();
		}

	var locationType = 0;
	function changeLocationType(){
		if($("#nameLocation").css("display")=="none"){
			$("#nameLocation").css("display","");
			$("#geoLocation").css("display","none");
			$("#dmm").css("visibility","hidden");
			$("#locationTypeBtn").val("转为经纬度");
			locationType = 1;
		}else{
			$("#nameLocation").css("display","none");
			$("#geoLocation").css("display","");
			$("#dmm").css("visibility","visible");
			$("#locationTypeBtn").val("转为名称");
			locationType = 0;
		}
	}

	function locationPostion(){
		//alert("定位");
		var earthAX = getEarth();
		if ( !earthAX )
		{
			return;
		}


		if ( locationType == 0 )
		{
			var lon = document.getElementById( "longitude" ).value;
			var lat = document.getElementById( "latitude" ).value;
			var height = document.getElementById( "height" ).value;


			var zipcode = new RegExp( "^\\-?\\d+\\.?\\d*$" );
			if ( !zipcode.test( lon ) || parseFloat( lon ) < -180.0 || parseFloat( lon ) > 180.0 )
			{
				alert( "请输入合法的经度！" );
				return;
			}

			if ( !zipcode.test( lat ) || parseFloat( lat ) < -90.0 || parseFloat( lat ) > 90.0 )
			{
				alert( "请输入合法的纬度！" );
				return;
			}

			if ( !zipcode.test( height ) )
			{
				alert( "请输入合法的高度！" );
				return;
			}

			earthAX.Connector( "<命令 命令名='设置相机位置'  经度='" + lon + "' 纬度='" + lat + "' 海拔='" + height + "' 偏航角='0.0' 俯仰角='-90.0' 滚转角='0.0' 定位方式='普通' />" );
		}
		else
		{
			var featureName = document.getElementById( "featureName" ).value;
			if ( featureName == "" )
			{
				alert( "请输入名称！" );
				return;
			}

			var featureInfoResult = earthAX.Connector( "<命令 命令名='获取要素信息' 要素名称=" + featureName + " />" );
			var xmlDoc = new ActiveXObject( "Microsoft.XMLDOM" );
			xmlDoc.async = false;
			xmlDoc.loadXML( featureInfoResult );
			if ( !xmlDoc )
			{
				return;
			}

			root = xmlDoc.getElementsByTagName( "document" )[0];
			if ( !root )
			{
				return;
			}

			var topNodes = root.childNodes;
			if ( topNodes.length == 0 )
			{
				alert( "无此名称！" );
				return;
			}

			if ( topNodes.length == 1 )
			{
				//直接定位
				var layerID = topNodes[0].getAttribute( "layerID" );
				var featureID = topNodes[0].getAttribute( "featureID" );

				earthAX.Connector( "<命令 命令名='定位要素' LayerID='" + layerID + "' FeatureID='" + featureID + "' />" );
			}
			else
			{
				//弹框选择
				//for ( var i = 0; i < topNodes.length; i++ )
				//{
				//
				//}

				//var layerID = topNodes[0].getAttribute( "layerID" );
				//var featureID = topNodes[0].getAttribute( "featureID" );

				//earthAX.Connector( "<命令 命令名='定位要素' LayerID='" + layerID + "' FeatureID='" + featureID + "' />" );
				top.nameListDataXml = featureInfoResult;

				var url = getURL() + "/html/namelocation.html?FeatureName=" + featureName;
				//getDlg(url, "namelocationDlg", "名称定位", 270, 330, true);
				earthAX.PopupDragUIHtmlWindow( "nameLocation", 200, 200, 260, 350, url, 100, false );
				setHtmlWindowDragRect( "nameLocation", 0, 0, 260, 30 );

			}


		}

	}

	function changeLayout()
	{
		$("#tree").layout("resize",{"height":$("#layOutDiv").height()});
		$("#layerTreeDIV").height( $("#tree").height() - $("#locationDIV").height() - 28 );
		if ( $("#layerTreeDIV").height() < 200 )
		{
			$("#layerTreeDIV").height(200);
		}
	}
