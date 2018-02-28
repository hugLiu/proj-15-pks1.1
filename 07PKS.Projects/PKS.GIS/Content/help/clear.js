/*
*	输入框清空方法		中盈高科
*/

/*
*	定位到指定企业
*/
function popBusiness02(){
		$('#businessID').val('');
}

/*
*	获取企业信息列表
*/
function popBusinessList02(){
			 $('#objectID').val('');			
}
/**
 * 返回企业分组列表
 * 
 */
function popBusinessGroup02(){
	var busnessID1 = $("#busnessID1").val('');
	var busnessGroup001 = $("#busnessGroup001").val('');	
	
}
/**
 * 
 * 经纬线
 */
function LonAndLatLine02(){
	var yanR = $('#yanR').val('1');
	var yanG = $('#yanG').val('0.0');
	var yanB = $('#yanB').val('0.0');
	var yanA = $('#yanA').val('1');
	var ziR = $('#ziR').val('1');
	var ziG = $('#ziG').val('0.0');
	var ziB = $('#ziB').val('0.0');
	var ziA = $('#ziA').val('1');
	var fonte = $('#fonte').val('16');
}

/*
*	查询企业显示
*/
function popBusinessTwo02(){	
		$('#businessID').val('');
		
}

/*
*	查询企业显隐
*/
function popBusinessThree02(){
				 $('#businessID5').val('');
				
}
/*
*	设置企业显示 
*/
function popBusinessFour02(){
		         $('#businessID1').val('');
			
}

/*
*	设置企业显隐  
*/
function popBusinessFive02(){
				$('#businessID2').val('');
				
}

/***** 对象测试    ********/

/*
*	对象删除  
*/
function popObjectOne02(){
				 $('#objectID23').val('');
			
}

/*
*	对象定位  
*/
function popObjectTwo02(){
				$('#objectID1').val('');				
}

/*
*	对象动作  
*/
function popObjectThree02(){
				 $('#objectID2').val('');
				 $('#parameter').val('');

}

/*
*	获取对象的经纬高  
*/
function popObjectFour02(){
				 $('#objectID1').val('');
}

/*
*	获取对象定位参数  
*/
function popObjectFive02(){
				 $('#objectID22').val('');
}

/*
*  获取对象信息列表  
*/
function popObjectSix02(){
				$('#objectID3').val('');
				$('#businessID3').val('');
				$('#types').val('');
}


/*
*  添加Lod信息  
*/
function popObject702(){
				$('#objectID7').val('');
			    $('#x7').val('');
				$('#y7').val('');
			    $('#z7').val('');
				$('#minDistance7').val('');
				$('#maxDistance7').val('');
}

/*
*  显示对象部件动作  
*/
function popObject802(){
				$('#objectID8').val('');
				$('#partsName8').val('');
				$('#parameter8').val('');
}

/*
*  创建可视对象  
*/
function popObject902(){
				$('#businessID9').val('');
				$('#objectID9').val('');
				$('#typeID9').val('');
				$('#name9').val('');

}

/*
*  模型  
*/
function popObject1002(){

				var id = $('#objectID10').val();
				deleteWithClear(id);
				$('#businessID10').val('');
				$('#objectID10').val('');
				$('#resourcePath10').val('');
				$('#position10x').val('');
				$('#position10y').val('');
				$('#position10z').val('');
				$('#gesture10x').val('');
				$('#gesture10y').val('');
				$('#gesture10z').val('');
				$('#zoom10x').val('');
				$('#zoom10y').val('');
				$('#zoom10z').val('');
}
/*
*  批量增加屏幕标注  
*/
function popObject11PL02(){
				$('#businessIDPL11').val('');
				deleteWithClear("-11218,-11219");
}
/*
*  屏幕标注  
*/
function popObject1102(){
	var id = $('#objectID11').val();
	deleteWithClear(id);
				$('#businessID11').val('');
				$('#objectID11').val('');
				$('#content11').val('');
				$('#font11').val('');
				$('#fontSize11').val('');
				$('#position11x').val('');
				$('#position11y').val('');
				$('#position11z').val('');
				$('#color11r').val('');
				$('#color11a').val('');
				$('#resourcePath11').val('');
				$('#wide11').val('');
				$('#height11').val('');
				$('#alignment11').val('');
}

/*
*  空间文字  
*/
function popObject1202(){
	var id = $('#objectID12').val();
	deleteWithClear(id);
			    $('#businessID12').val('');
				$('#objectID12').val('');
				$('#content').val('');
				$('#font12').val('');
				$('#fontSize12').val('');
				$('#position12x').val('');
				$('#position12y').val('');
				$('#position12z').val('');
				$('#color12r').val('');
				$('#color12a').val('');
				$('#gesture12x').val('');
				$('#gesture12y').val('');
				$('#gesture12z').val('');
}

/*
*  空间图片  
*/
function popObject1302(){
	var id = $('#objectID13').val();
	deleteWithClear(id);
				$('#businessID13').val('');
				$('#objectID13').val('');
				$('#resourcePath13').val('');
				$('#wide13').val('');
				$('#position13x').val('');
				$('#position13y').val('');
				$('#position13z').val('');
				$('#height13').val('');
				$('#gesture13x').val('');
				$('#gesture13y').val('');
				$('#gesture13z').val('');
}

/*
*  屏幕窗口  
*/
function popObject1402(){
	var id = $('#objectID14').val();
	deleteWithClear(id);
				$('#businessID14').val('');
				$('#objectID14').val('');
				$('#position14x').val('');
				$('#position14y').val('');
				$('#position14z').val('');
				$('#wide14').val('');
				$('#height14').val('');
				$('#color14r').val('');
				$('#color14a').val('');
}

/*
* 屏幕文字  
*/
function popObject1502(){
	var id = $('#objectID15').val();
	deleteWithClear(id);
				$('#businessID15').val('');
				$('#objectID15').val('');
				$('#font15').val('');
				$('#neirong').val('');
				$('#fontSize15').val('');
				$('#position15x').val('');
				$('#position15y').val('');
				$('#color15r').val('');
				$('#color15a').val('');
				$('#windowID15').val('');
}

/*
*  屏幕图片 
*/
function popObject1602(){
	var id = $('objectID16').val();
	deleteWithClear(id);
				$('#businessID16').val('');
				$('#objectID16').val('');
				$('#windowID16').val('');
				$('#resourcePath16').val('');
				$('#wide16').val('');
				$('#position16x').val('');
				$('#position16y').val('');
				$('#height16').val('');
}

/*
*  球  
*/
function popObject1702(){
	var id = $('#objectID17').val();
	deleteWithClear(id);
				$('#businessID17').val('');
				$('#objectID17').val('');
				$('#rearth').val('');
				$('#earthColor17r').val('');
				$('#earthColor17a').val('');
				$('#detail17').val('');
				$('#gridColor17r').val('');
}

/*
*  标牌  
*/
function popObject1802(){
			
}
/**********分隔*********/
/*
*	相机：设置相机位置，姿态
*/
function setCamera02(){
				$('#longitude').val('');
				$('#latitude').val('');
				$('#elevation').val('');
				$('#yawA').val('');
				$('#pitchingA').val('');
				$('#rollA').val('');
}

/*
*	相机：根据二维地图比例设置三维相机的位置姿态
*/
function popCamera02(){
				$('#value').val('');
}

/*
*	命中测试
*/
function hit02(){
				$('#xyX1').val('');
				$('#xyY1').val('');
}

/*
*	世界与厂区坐标转换(世界到厂区)
*/
function change02(){
				$('#idxyzID').val('');
				$('#idxyzX').val('');
				$('#idxyzY').val('');
				$('#idxyzZ').val('');
}

/*
*	世界与厂区坐标转换(厂区到世界)
*/

function change1102(){
				$('#idxyzID1').val('');
				$('#idxyzX1').val('');
				$('#idxyzY1').val('');
				$('#idxyzZ1').val('');
}

/*
*	获取屏幕坐标点的经纬高
*/
function get102(){
				$('#xyX2').val('');
				$('#xyY2').val('');
}

/*
*	获取指定点到长距离管线的投影坐标
*/
function get202(){
				$('#xyX3').val('');
				$('#xyY3').val('');
}

/*
*	坐标转换(由西安80坐标到WGS84坐标)
*/
function change102(){
				$('#xyX').val('');
				$('#xyY').val('');
}

/*
*	计算高度
*/
function count02(){
				$('#xyX4').val('');
				$('#xyY4').val('');
}
/*
*	暂停、启动三维渲染线程
*/
function xiancheng02(){
				
}

/*
*	操作器切换
*/
function change202(){
				
}
/*
*	漫游器开关
*/
function change302(){

}
/*
*	推演
*/
function tuiyan1102(){
				$('#name11').val('');
}

/*
*	播放音乐
*/
function music02(){
			    $('#path11').val('');
}
/*
*	画点线面
*/
function hua02(){

				var id = $('#oid1111').val();
				deleteWithClear(id);
				$('#fid1111').val('');
				$('#oid1111').val('');
				$('#w1111').val('');
}

/*
*	修改屏幕标注
*/
function h102(){
				$('#hh1oid').val('');
				$('#hh1fid').val('');
				$('#hh1val').val('');
}
/*
*	抓图
*/
function h202(){
				$('#hh2path').val('');
				$('#hh2sosx').val('');
				$('#hh2sosy').val('');
				$('#hh2soex').val('');
				$('#hh2soey').val('');
				$('#hh2high').val('');
				$('#hh2time').val('');
}
/*
*	选择本地文件
*/
function h302(){
				$('#hh3ob').val('');
			
}
/*
*	坐标系切换
*/
function h402(){
			
}
/*
*	事件处理器管理开启
*/
function q102(){
				$('#qq1NAME').val('');
}
/*
*	事件处理器管理关闭
*/
function q202(){
				$('#qq2NAME').val('');
}
/*
*	地形剖切
*/
function q302(){
				$('#qq3X1').val('');
				$('#qq3Y1').val('');
				$('#qq3Z1').val('');
				$('#qq3X2').val('');
				$('#qq3Y2').val('');
				$('#qq3Z2').val('');
				$('#qq3X3').val('');
				$('#qq3Y3').val('');
				$('#qq3Z3').val('');
				$('#qq3D1').val('');
}
/*
*	开启透明
*/
function q402(){
				$('#qq4val').val('');
}
/*
*	添加管道
*/
function e102(){
				$('#ee1l11').val('');
				$('#ee1l12').val('');
				$('#ee1h1').val('');
				$('#ee1l21').val('');
				$('#ee1l22').val('');
				$('#ee1h2').val('');
				$('#ee1bj').val('');
			    $('#ee1r').val('');
}
/*
*	创建右键菜单
*/
function e202(){
				$('#contentMenu1').val('');
				$('#ee2ID').val('');
				$('#ee2X').val('');
				$('#ee2Y').val('');
}
/*
*	返回图层元素列表
*/
function d102(){
				$('#dd1ID').val('');
}
/*
*	图层控制
*/
function d202(){
				$('#dd2id').val('');
		        $('#dd2can').val('');
}
/*
*	光照位置
*/
function d302(){
				$('#dd3ID').val('');
				$('#dd3lox').val('');
				$('#dd3loy').val('');
				$('#dd3loz').val('');
}
/*
*	 播放脚本
*/
function playCortoon02(){
				$('#scriptName').val('');
				$('#dataX').val('');
				 $('#dataY').val('');
}

/*
*	鹰眼相关操作
*/
function y102(){
}

/*
*	高亮模型
*/
function HIGHLIGHT02(){
				$('#highlightid').val('');
				$('#highlightR').val('');

}
/*
*	天气控制
*/
function WEATHER02(){
				$('#qihou').val('');
				$('#zhuangtai').val('');
}
/*
*	挂件显隐
*/
function WIDGET02(){
			
}

/*
*	查询挂件状态
*/
function WIDGETSTATUS02(){

}

/*
*	光照的设置与查询
*/
function LightSettingAndSearch02(){
				$('#lightSetType4').val('');
				$('#lightSetType7').val('');
}

/*
*	画点
*/
function drawPointMsl1102(){
				var id = $('#drawPointMsl2').val();
				deleteWithClear(id);
				$('#drawPointMsl1').val('');
				$('#drawPointMsl2').val('');
				$('#drawPointMsl3').val('');
				$('#drawPointMsl4').val('');
				$('#drawPointMsl5').val('');
				$('#drawPointMsl5Z').val('');
				$('#drawPointMsl6').val('');
				$('#drawPointMsl9').val('');
				$('#drawPointMsl10').val('');
			
}

/*
*	画线
*/
function drawLineMsl11102(){
				var id = $('#drawLineMsl2').val();
				deleteWithClear(id);
				$('#drawLineMsl1').val('');
				$('#drawLineMsl2').val('');
				$('#drawLineMsl3').val('');
				$('#drawLineMsl4').val('');
				$('#drawLineMsl5').val('');
				$('#drawLineMsl5z').val('');
				$('#drawLineMsl6').val('');
				$('#drawLineMsl7').val('');
				$('#drawLineMsl7z').val('');
				$('#drawLineMsl8').val('');
				$('#drawLineMsl11').val('');
				$('#drawLineMsl12').val('');
				$('#drawLineMsl13').val('');
				$('#drawLineMsl16').val('');
				$('#drawLineMsl17').val('');
}

/*
*	画面
*/
function drawSurfaceMsl11102(){

				var id = $('#drawSurfaceMsl2').val();
				deleteWithClear(id);
				$('#drawSurfaceMsl1').val('');
				$('#drawSurfaceMsl2').val('');
				$('#drawSurfaceMsl3').val('');
				$('#drawSurfaceMsl4').val('');
				$('#drawSurfaceMsl5').val('');
				$('#drawSurfaceMsl5z').val('');
				$('#drawSurfaceMsl6').val('');
				$('#drawSurfaceMsl7').val('');
				$('#drawSurfaceMsl7z').val('');
				$('#drawSurfaceMsl8').val('');
				$('#drawSurfaceMsl9').val('');
				$('#drawSurfaceMsl9z').val('');
				$('#drawSurfaceMsl10').val('');
				$('#drawSurfaceMsl13').val('');
				$('#drawSurfaceMsl14').val('');
				$('#drawSurfaceMsl15').val('');
				$('#drawSurfaceMsl18').val('');
				$('#drawSurfaceMsl19').val('');
				$('#drawSurfaceMsl20').val('');
				$('#drawSurfaceMsl23').val('');
}
/*
*	 播放含有事件的脚本动画
*/
function playControlCortoon02(){
				$('#scriptName').val('');
}
/*
*	 根据模型名，查询模型ID
*/
function SelectModelID02(){
				$('#objectName').val('');
}
/*
*	国际化
*/
function intontroalControl02(){
	
}

/*
*	启动地球前设置语言
*/
function setLanguage01(){
				$('#languageType').combobox('clear');
}

/*
*	截图
*/
function pictureControl02(){
				$('#pictureName').val('');
				$('#pirctureUrl').val('');

}
/*
*	Tip标牌:
*/
function creatTip02(){

				var id = $('#drawPointMsl201').val();
				deleteWithClear(id);
				$('#drawPointMsl101').val('');
				$('#drawPointMsl201').val('');
				$('#drawPointMsl301').val('');
				$('#point').val('');
				$('#wordsize').val('');
				$('#wordtype').val('');
				$('#worda').val('');
				$('#wordr').val('');
				$('#bordera').val('');
				$('#borderr').val('');
				$('#wordname').val('');
				$('#locationx').val('');
				$('#locationy').val('');
				$('#locationz').val('');
}

/*
 * 修改Tip标牌
 */
function updateTip02(){
	$('#objectTipID').val('');
}

/*
 * 修改Text标牌
 */
function updateText02(){
	$('#objectTextID').val('');
}

/*
*	Text标牌:
*/
function creatText02(){
				var id = $('#drawPointMsl202').val();
				deleteWithClear(id);
				$('#drawPointMsl102').val('');
				$('#drawPointMsl202').val('');
				$('#drawPointMsl302').val('');
				$('#posx001').val('');
				$('#titelFont').val('');
				$('#titelFontHeight').val('');
				$('#fontHeight').val('');
				$('#titelBkpic').val('');
				$('#titelColor').val('');
				$('#titelColora').val('');
				$('#rowColor').val('');
				$('#rowColora').val('');
				$('#bkpic').val('');
				$('#bkColor').val('');
				$('#bkColora').val('');
				$('#closeType').val('');
				$('#minscale').val('');
				$('#maxscale').val('');
				$('#posx').val('');
				$('#posy').val('');
				$('#posz').val('');
				$('#rowFont').val('');
				$('#bindPosx').val('');
				$('#bindPosy').val('');
				$('#bindPosz').val('');
				$('#tiptitle').val('');
}
/*
*	html标牌
*/
function creathtml02(){

				var id = $('#drawPointMsl203').val();
				deleteWithClear(id);
				$('#drawPointMsl103').val('');
				$('#drawPointMsl203').val('');
				$('#drawPointMsl303').val('');
				$('#htmlUrl').val('');
				$('#bindPosxx').val('');
				$('#bindPosyy').val('');
				$('#bindPoszz').val('');
				$('#weight').val('');
				$('#height').val('');
				$('#titelBkcolor').val('');
				$('#titelBkpica').val('');
				$('#visiable').val('');
				$('#titelBkcolor1').val('');
				$('#titelBkpica1').val('');
				$('#drag').val('');
				$('#spaceLift').val('');
				$('#spaceUp').val('');
				$('#spaceRight').val('');
				$('#spaceDown').val('');
				$('#foundSize').val('');
				$('#dragectLeft').val('');
				$('#dragectTop').val('');
				$('#dragectWidth').val('');
				$('#dragectHeight').val('');
}
/*
*	创建动态标牌
*/
function createRun02(){

				var id = $('#objectID01').val();
				deleteWithClear(id);
				$('#objectID01').val('');
				$('#modelID01').val('');
				$('#posX01').val('');
				$('#posY01').val('');
				$('#posZ01').val(''); 
}
/*
*	更新动态标牌
*/
function updateRun02(){
				$('#objectID02').val('');
}
/*
*	对象自动释放
*/
function setRun02(){
				$('#objectID03').val('');
				$('#time').val('');
}
/*
*	创建可视对象标牌
*/
function makeGeneral12102(){

				var id = $('#objectID04').val();
				deleteWithClear(id);
				$('#objectID04').val('');
				$('#modelID04').val('');
}
/*
*	球范围查询
*/
function selectBallDate02(){
				$('#lon01').val('');
				$('#lat01').val('');
				$('#height01').val('');
				$('#rde01').val('');
				$('#userDate01').val('');
				$('#cameraID01').val('');
				$('#objectType01').val('');
                $('#length01').val('');
				$('#width01').val('');
				$('#height02').val('');
				$('#objectType01').val('');
				$('#lonlat').val('');
}
/*
*	模型拆分
*/
function modelchai02(){
				$('#modelpath0101').val('');
}
/*
*	模型刨切
*/
function makeGeneral02(){
	           $('#modelpath0202').val('');
}

function fly02(){
	$('#compId').val('');
	$('#modelId').val('');
	$('#oprateType').val('');
	$('#width').val('');
	$('#distence').val('');
	$('#colorR').val('');
	$('#colorA').val('');
}

/*
 * 清空表单时删除添加的可视对象
 */
function deleteWithClear(id){
	parent.objectDelete(id);
}

/*
 * 状态栏
 */
function settingHintbar02(){
	 $('#hintbarFontSize').val('');
	 $('#hintbarFontColor').val('');
	 $('#hintbarFontA').val('');
	 $('#hintbarBackColor').val('');
	 $('#hintbarBackA').val('');
}

/*
 * 工具栏
 */
function settingTool02(){
	$('#toolspaceDis').val('');
	$('#toolHight').val('');
	$('#toolFontSize').val('');
	$('#toolFontColor').val('');
	$('#toolFrameColor').val('');
}

/*
 * 控制面板
 */
function settingControlpanel02(){
	 $('#ControlpanelScale').val('');
	 $('#ControlpanelTopMargin').val('');
	 $('#ControlpanelRightMargin').val('');
}

/*
 * 焦点十字
 */
function settingFocuscross02(){
	 $('#FocuscrossWinth').val('');
	 $('#FocuscrossHight').val('');
	 $('#FocuscrossFontColor').val('');
}

/*
 * 模型移动
 */
function objectMove02(){
	$('#moveID').val('');
	$('#firstLon').val('');
	$('#firstLat').val('');
	$('#firstAlt').val('');
	$('#time1').val('');
	$('#secondLon').val('');
	$('#secondLat').val('');
	$('#secondAlt').val('');
	$('#time2').val('');
}

/*
 * 设置局部经纬线
 */
function setLonAndLatLine02(){
	$('#maxLat').val('');
	$('#maxLon').val('');
	$('#minLat').val('');
	$('#minLon').val('');
}

/*
 * （新）设备剖切
 */
function newModelpao02(){
	$('#paoID').val('');
	$('#paoName').val('');
	$('#paoOffSet').val('');
}
/*
 * 矢量图层是否存在
 */
function isExist02(){
	$('#name').val('');	
}
/*
 * 获取矢量图层的显示状态
 */
function getState02(){
	$('#name1').val('');	
}
/*
 * 设置图层的显示状态
 */
function setState02(){
	$('#name2').val('');
	$('#state').combobox('clear');
}
/*
 * 点类型图查文
 */
function searcho02(){
	$('#oname').val('');
	$('#oproperty').val('');
	$('#ox').val('');
	$('#oy').val('');
}
/*
 * 矩形类型图查文
 */
function searchr02(){
	$('#rname').val('');
	$('#rproperty').val('');
	$('#rx1').val('');
	$('#ry1').val('');
	$('#rx2').val('');
	$('#ry2').val('');
}
/*
 * 多边形类型图查文
 */
function searchp02(){
	$('#pname').val('');
	$('#pproperty').val('');
	$('#px1').val('');
	$('#py1').val('');
	$('#px2').val('');
	$('#py2').val('');
	$('#px3').val('');
	$('#py3').val('');
	$('#px4').val('');
	$('#py4').val('');
	$('#px5').val('');
	$('#py5').val('');
}

/*
 * 圆类型图查文
 */
function searchc02(){
	$('#cname').val('');
	$('#cproperty').val('');
	$('#cx').val('');
	$('#cy').val('');
	$('#radius').val('');
}
/*
 * 圆类型图查文
 */
function search202(){
	$('#wname').val('');
	$('#wproperty').val('');
	$('#statement').val('');
 
}
function setlight02(){
	  $('#lname').val('');
	  s$('#lid').val('');
	  $('#color').val('');
}
function recoverlight02(){
	$('#hname').val('');
}

function login02(){
	$('#username').val('');
	$('#password').val('');
}
//**====================================图层开始**=======================//
/**
 * 新增一个图层，参数清除
 */
function newLayerClear(){
	$('#layername').val('');
	$('#layerType').val('');
	$('#layerAttr').val('');
	$('#layerDes').val('');
}

//**======================================要素开始============================**//
/**
 * 选中要素
 */
function selecteElementClear(){
	$('#elementObjectId').val('');
}
/**
 * 选中要素
 */
function newElementClear(){
	$('#elementCoordinate').val('');
	$('#elementAttribute').val('');
}
/**
 *  编辑指定要素属性
 */
function editorElementAttrClear(){
	$('#editorElementId').val('');
	$('#editorElementAttr').val('');
}
/**
 * 删除元素
 */
function delElementClear(){
	$('#delElementId').val('');
	$('#delElementOrNo').val('');
}
/**
 * 提交要素属性
 */
function submitElementClear(){
	$('#submitElementId').val('');
	$('#submitElementAttr').val('');
	$('#yesOrNo').val('');
}
/**
 * 提交当前编辑至服务器
 */
function sumintElementToServerClear(){
	$('#yesOrNoToServer').val('');
}
//**========================================节点开始=======================**//
/**
 * 精准编辑插入要素节点
 */
function insertElementNoteClear(){
	$('#insertElementNum').val('');
	$('#insertElementIndex').val('');
	$('#insertElementLon').val('');
	$('#insertElementLat').val('');
}
/**
 * 精准编辑更新节点
 */
function updateElementNoteClear(){
	$('#updateElementIndex').val('');
	$('#updateElementLon').val('');
	$('#updateElementLat').val('');
}
/**
 * 精准编辑增加节点
 */
function addElementNoteClear(){
	$('#addElementIndex').val('');
	$('#addElementLon').val('');
	$('#addElementLat').val('');
}
/**
 * 精准编辑删除节点
 */
function addElementNoteClear(){
	$('#delElementIndex').val('');
}
