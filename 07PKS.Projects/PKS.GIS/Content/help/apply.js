/**
*	输入框提交方法		中盈高科
*/


/****** 企业测试    ********/
/**
*	定位到指定企业
*/
function popBusiness01(){
				var id = $('#businessID').val();
				var type = $('#locType').combobox('getValue');
				parent.enterprisePosition(id,type);
				var scrip2="根据ID值，定位到该id所对应的企业";
				var script02="js参数说明：\r\n"+"ID 待定位的企业ID\r\n"+"type 定位方式（普通/飞行）";
				parent.getMethodInfo('scriptIJas.js','enterprisePosition(ID,type)','无',scrip2,script02);
}

/**
*	获取企业信息列表
*/
function popBusinessList01(){
				var id = $('#objectID').val();
				var infoList = parent.getEnterpriseMessage(id);
				var paramer = "js参数说明：\r\n"+"ID 企业ID，获取由该ID指定的对象所属的企业的信息；如果“对象ID”为０，则返回所有企业的信息列表";
				parent.getMethodInfo('sceneIJas.js','getEnterpriseMessage(ID)',infoList,'查询场站中企业信息列表',paramer);
}

/**
*	查询企业显示
*/
function popBusinessTwo01(){	
				var id = $('#businessID').val();
				var infoList = parent.queryEnterpriseShow(id);
				var paramer = "js参数说明：\r\n"+"ID 企业ID";
				parent.getMethodInfo('sceneIJas.js','queryEnterpriseShow(ID)',infoList,'查询指定企业的是否启用细分模型的功能',paramer);
}

/**
*	查询企业显隐
*/
function popBusinessThree01(){
				var id = $('#businessID5').val();
				var infoList = parent.queryEnterpriseShowOrHide(id);
				var paramer = "js参数说明：\r\n"+"ID 企业ID";
				parent.getMethodInfo('sceneIJas.js','queryEnterpriseShowOrHide(ID)',infoList,'查询对应企业ID的企业是显示态，还是隐藏',paramer);
}

/**
*	设置企业显示 
*/
function popBusinessFour01(){
				var id = $('#businessID1').val();
				var model = $('#modelType').combobox('getValue');
				parent.setEnterpriseDisplay(id,model);
				var paramer = "js参数说明：\r\n"+"ID 企业ID\r\n"+"model 细分模型（启用/禁用）";
				parent.getMethodInfo('sceneIJas.js','setEnterpriseDisplay(ID,model)','无','设置企业是否启用细分模型',paramer);
}

/**
*	设置企业显隐  
*/
function popBusinessFive01(){
				var id = $('#businessID2').val();
				var actionType = $('#actionType').combobox('getValue');
				parent.setEnterpriseShowOrHide(id,actionType);
				var paramer = "js参数说明：\r\n"+"ID 企业ID\r\n"+"actionType 动作类型，显示|隐藏";
				getMethodInfo('sceneIJas.js','setEnterpriseShowOrHide(ID,actionType)','无','显示或者隐藏对应ID的企业',paramer);
}

/****** 对象测试    ********/

/**
*	对象删除  
*/
function popObjectOne01(){
				var id = $('#objectID23').val();
				parent.objectDelete(id);
				var paramer = "js参数说明：\r\n"+"objectID 所要删除的对象ID";
				parent.getMethodInfo('sceneIJas.js','objectDelete(objectID)','无','删除指定ID的对象',paramer);
}

/**
*	对象定位  
*/
function popObjectTwo01(){
				var id = $('#objectID1').val();
				var locatType = $('#locatType').combobox('getValue');
				parent.objectLocation(id,locatType);
				var scrip1="视角定位到对应ID设备";
				var script01="js参数说明：\r\n"+"ID 待定位的对象的ID\r\n"+"locatType 定位方式（普通/飞行）";
				parent.getMethodInfo('scriptIJas.js','objectLocation(ID,locatType)','无',scrip1,script01);
}

/**
*	对象动作  
*/
function popObjectThree01(){
				var id = $('#objectID2').val();
				var objActionType = $('#objActionType').combobox('getValue');
				var parameter = $('#parameter').val();
				parent.objectActions(id,objActionType,parameter);
				var paramer = "js参数说明：\r\n"+"ID 对象ID\r\n"+"actionType （显隐/选中/透明/闪烁/清除所有效果）\r\n"+"parameter 参数，显隐：0-隐藏 1-显示；选中：不需要参数；透明：0~100整数（数值大小与透明度成正比，0为不透，100为全透）；闪烁：整数（闪烁次数）；清除所有效果：不需要参数";
				parent.getMethodInfo('sceneIJas.js','objectActions(ID,actionType,parameter)','无','可以设置设备的状态：如显隐性，闪烁，透明',paramer);
}

/**
*	获取对象的经纬高  
*/
function popObjectFour01(){
				var id = $('#objectID1').val();
				var t = parent.getObjectPDH(id);
				var paramer = "js参数说明：\r\n"+"ID 所要获取对象的ID";
				parent.getMethodInfo('sceneIJas.js','getObjectPDH(ID)',t,'根据对象ID，返回对象的在地球上的经纬度以及海拔',paramer);
}

/**
*	获取对象定位参数  
*/
function popObjectFive01(){
				var id = $('#objectID22').val();
				var infoList = parent.getObjectPosition(id);
				var paramer = "js参数说明：\r\n"+"ID 所要返回定位参数的对象ID";
				parent.getMethodInfo('sceneIJas.js','getObjectPosition(ID)',infoList,'获取对象定位参数',paramer);
}

/**
*  获取对象信息列表  
*/
function popObjectSix01(){
				var id = $('#objectID3').val();
				var businessID = $('#businessID3').val();
				var type = $('#types').val();
				var infoList = parent.getObjectInfoList(id,businessID,type);
				var paramer = "js参数说明：\r\n"+"businessID 企业ID，查询设备所在的企业的ID，如果未指定对象ID，则返回符合企业ID、类型两个参数值的对象列表\r\n"+"objectID 对象ID\r\n"+"type 类型，企业ID、类型两个参数可组合使用，也可单独使用；如果三个参数都未指定，则返回所有对象\r\n";
				parent.getMethodInfo('sceneIJas.js','getObjectInfoList(objectID,businessID,type)',infoList,'根据企业ID，或对象ＩＤ，或类型，列出所相关的所有对象信息',paramer);
}


/**
*  添加Lod信息  
*/
function popObject701(){
				var model7 = $('#model7').combobox('getValue');
				var objectID7 = $('#objectID7').val();
				var x7 = $('#x7').val();
				var y7 = $('#y7').val();
				var z7 = $('#z7').val();
				var minDistance7 = $('#minDistance7').val();
				var maxDistance7 = $('#maxDistance7').val();
				parent.addLodInfo(model7,objectID7,x7,y7,z7,minDistance7,maxDistance7);
				var paramer = "js参数说明：\r\n"+"mode （自身中心点|设定中心点）模式\r\n"+"ID 对象ID，添加的Lod信息的ID\r\n"+"(x,y,z)经纬度高，当模式是自身中心点时,不需要设置XYZ,只设置距离.当模式是设定中心点时, 需要设置距离，同时需要设置XYZ的值,XYZ值即设定的中心点\r\n"+"minDistance 最小距离，如果海拔高度小于这个值，LOD信息就不显示\r\n"+"maxDistance 最大距离，如果海拔高度大于这个值，LOD信息就不显示";
				parent.getMethodInfo('sceneIJas.js','addLodInfo(mode,ID,x,y,z,minDistance,maxDistance)','无','设置一定高度范围内该对象是可见的，在最大最小范围之外是不可见的',paramer);
}

/**
*  显示对象部件动作  
*/
function popObject801(){
				var objectID8 = $('#objectID8').val();
				var actionType8 = $('#actionType8').combobox('getValue');
				var partsName8 = $('#partsName8').val();
				var parameter8 = $('#parameter8').val();
				parent.displayPartsAction(objectID8,actionType8,partsName8,parameter8);
				var paramer = "js参数说明：\r\n"+"ID 对象ID\r\n"+"actionType (隐藏部件 / 显示部件 / 隐藏全部部件 /显示全部部件) 动作类型\r\n"+"partsName 部件名称\r\n"+"parameter 参数";
				parent.getMethodInfo('sceneIJas.js','displayPartsAction(ID,actionType,maxDistance,parameter)','无','为模型提供的接口，若模型的各个部件遵循一定的命名规则：名称中包含BuJian，则可以通过部件的名称对这些部件进行一定操作，如显示隐藏等',paramer);
}

/**
*  创建可视对象  
*/
function popObject901(){
				var businessID9 = $('#businessID9').val();
				var objectID9 = $('#objectID9').val();
				var changeBool9 = $('#changeBool9').combobox('getValue');
				var typeID9 = $('#typeID9').val();
				var name9 = $('#name9').val();
				parent.createVisualObject(businessID9,objectID9,typeID9,name9,changeBool9);
}

/**
*  模型  
*/
function popObject1001(){
				var businessID10 = $('#businessID10').val();
				var objectID10 = $('#objectID10').val();
				var resourcePath10 = $('#resourcePath10').val();
				var positionX = $('#position10x').val();
				var positionY = $('#position10y').val();
				var positionZ = $('#position10z').val();
				var gestureX = $('#gesture10x').val();
				var gestureY = $('#gesture10y').val();
				var gestureZ = $('#gesture10z').val();
				var zoomX = $('#zoom10x').val();
				var zoomY = $('#zoom10y').val();
				var zoomZ = $('#zoom10z').val();
				parent.popModel(businessID10,objectID10,resourcePath10,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,zoomX,zoomY,zoomZ);
				locationWithCreate(objectID10,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 企业ID，所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"resourcePath 资源路径(X:\\X.osg)，相对路径为地球缓存下的\Res\model文件夹。也支持绝对路径。\r\n"+"(positionX，positionY，positionZ) X，Y，Z对应为所属企业的坐标（以企业左下角为原点）\r\n"+"(gestureX,gestureY,gestureZ) 姿态\r\n"+"(zoomX,zoomY,zoomZ) 缩放";
				parent.getMethodInfo('sceneIJas.js','popModel(businessID,objectID,resourcePath,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,zoomX,zoomY,zoomZ)','无','添加新模型',paramer);
}
/**
*  批量添加屏幕标注  
*/
function popObject11PL01(){
				var businessID11 = $('#businessIDPL11').val();
				parent.popScreenOverlaysPL(businessID11);
				setTimeout("locationWithCreate('-11218','普通')", 0);
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n";
				parent.getMethodInfo(
						'sceneIJas.js',
						'popScreenOverlaysPL(bussinsessID)',
						'', '批量添加屏幕标注', paramer);
				
			//	locationWithCreate('-11218','普通');
}
/**
*  屏幕标注  
*/
function popObject1101(){
				var businessID11 = $('#businessID11').val();
				var objectID11 = $('#objectID11').val();
				var content11 = $('#content11').val();
				var font11 = $('#font11').val();
				var fontSize11 = $('#fontSize11').val();
				var positionX = $('#position11x').val();
				var positionY = $('#position11y').val();
				var positionZ = $('#position11z').val();
				var colorR = $('#color11r').val();
				var colorA = $('#color11a').val();
				var resourcePath11 = $('#resourcePath11').val();
				var wide11 = $('#wide11').val();
				var height11 = $('#height11').val();
				var alignment11 = $('#alignment11').val();
				parent.popScreenOverlays(businessID11,objectID11,content11,font11,fontSize11,positionX,positionY,positionZ,colorR,colorA,resourcePath11,wide11,height11,alignment11);
				locationWithCreate(objectID11,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"content 内容\r\n"+"font 字体，例如：SIMSUN.TTC\r\n"+"fontSize 字号\r\n"+"(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"+"color 颜色，线颜色为16进制颜色。例如：#000033\r\n"+"colorA 透明度，范围为0到100\r\n"+"resourcePath 资源路径，资源路径为屏幕标注的绑定图片资源的路径\r\n"+"wide 宽\r\n"+"height 高\r\n"+"alignment 对齐方式，如居中，等等";
				parent.getMethodInfo('sceneIJas.js','popScreenOverlays(businessID,objectID,content,font,fontSize,positionX,positionY,positionZ,colorR,colorA,resourcePath,wide,height,alignment)','无','添加屏幕标注',paramer);

}

/**
*  空间文字  
*/
function popObject1201(){
				var businessID12 = $('#businessID12').val();
				var objectID12 = $('#objectID12').val();
				var content = $('#content').val();
				var font12 = $('#font12').val();
				var fontSize12 = $('#fontSize12').val();
				var positionX = $('#position12x').val();
				var positionY = $('#position12y').val();
				var positionZ = $('#position12z').val();
				var colorR = $('#color12r').val();
				var colorA = $('#color12a').val();
				var gestureX = $('#gesture12x').val();
				var gestureY = $('#gesture12y').val();
				var gestureZ = $('#gesture12z').val();
				parent.popSpaceText(businessID12,objectID12,content,font12,fontSize12,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,colorR,colorA);
				locationWithCreate(objectID12,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"content 内容,\r\n"+"font 字体，例如：SIMSUN.TTC\r\n"+"fontSize 字号\r\n"+"(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"+"(gestureX，gestureY，gestureY) 姿态\r\n"+"colorR 颜色， 颜色为16进制颜色。例如：#000033\r\n"+"colorA 透明度，范围为0到100";		
				parent.getMethodInfo('sceneIJas.js','popSpaceText(businessID,objectID,content,font,fontSize,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,colorR,colorA)','无','添加空间文字',paramer);
}

/**
*  空间图片  
*/
function popObject1301(){
				var businessID13 = $('#businessID13').val();
				var objectID13 = $('#objectID13').val();
				var resourcePath13 = $('#resourcePath13').val();
				var wide13 = $('#wide13').val();
				var positionX = $('#position13x').val();
				var positionY = $('#position13y').val();
				var positionZ = $('#position13z').val();
				var height13 = $('#height13').val();
				var gestureX = $('#gesture13x').val();
				var gestureY = $('#gesture13y').val();
				var gestureZ = $('#gesture13z').val();
				parent.popSpacePictures(businessID13,objectID13,resourcePath13,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,wide13,height13);
				locationWithCreate(objectID13,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"resourcePath 资源路径(X:\\X.osg)，图片的路径\r\n"+"(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）"+"(gestureX，gestureY，gestureY) 姿态 \r\n"+"wide 宽\r\n"+"height 高";
				parent.getMethodInfo('sceneIJas.js','popSpacePictures(businessID,objectID,resourcePath,positionX,positionY,positionZ,gestureX,gestureY,gestureZ,wide,height)','无','添加空间图片',paramer);
}

/**
*  屏幕窗口  
*/
function popObject1401(){
				var businessID14 = $('#businessID14').val();
				var objectID14 = $('#objectID14').val();
				var positionX = $('#position14x').val();
				var positionY = $('#position14y').val();
				
				var wide14 = $('#wide14').val();
				var height14 = $('#height14').val();
				var colorR = $('#color14r').val();
				var colorA = $('#color14a').val();
				parent.popScreenWallpaper(businessID14,objectID14,positionX,positionY,wide14,height14,colorR,colorA);
				locationWithCreate(objectID14,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"(positionX,positionY,positionZ) X,Y,Z对应为所属企业的坐标（以企业左下角为原点）\r\n"+"wide 宽\r\n"+"height 高\r\n"+"colorR 颜色 颜色为16进制颜色。例如：#000033\r\n"+"colorA 透明度，范围为0到100";
				parent.getMethodInfo('sceneIJas.js','popScreenWallpaper(businessID,objectID,positionX,positionY,positionZ,wide,height,colorR,colorA)','无','添加屏幕窗口',paramer);
}

/**
* 屏幕文字  
*/
function popObject1501(){
				var businessID15 = $('#businessID15').val();
				var objectID15 = $('#objectID15').val();
				var font15 = $('#font15').val();
				var fontSize15 = $('#fontSize15').val();
				var positionX = $('#position15x').val();
				var positionY = $('#position15y').val();
				var colorR = $('#color15r').val();
				var colorA = $('#color15a').val();
				var windowID15 = $('#windowID15').val();
				var neirong=$('#neirong').val();
				parent.popScreenText(businessID15,objectID15,windowID15,neirong,font15,fontSize15,positionX,positionY,colorR,colorA);
				locationWithCreate(objectID15,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"windowID 窗口ID\r\n"+"neirong 显示内容\r\n"+"font 字体，例如：SIMSUN.TTC\r\n"+"fontSize 字号\r\n"+"(positionX，positionY) X,Y对应为所属企业的坐标（以企业左下角为原点）\r\n"+"colorR 颜色 颜色为16进制颜色。例如：#000033\r\n"+"colorA 透明度，范围为0到100";
				parent.getMethodInfo('sceneIJas.js','popScreenText(businessID,objectID,windowID,neirong,font,fontSize,positionX,positionY,colorR,colorA)','无','添加屏幕文字',paramer);
}

/**
*  屏幕图片 
*/
function popObject1601(){
				var businessID16 = $('#businessID16').val();
				var objectID16 = $('#objectID16').val();
				var windowID16 = $('#windowID16').val();
				var resourcePath16 = $('#resourcePath16').val();
				var wide16 = $('#wide16').val();
				var positionX = $('#position16x').val();
				var positionY = $('#position16y').val();
				var height16 = $('#height16').val();
				parent.popScreenPictures(businessID16,objectID16,windowID16,resourcePath16,positionX,positionY,wide16,height16);
				locationWithCreate(objectID16,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"windowID 窗口ID\r\n"+"resourcePath 资源路径(X:\\X.osg)，图片所在的路径\r\n"+"(positionX，positionY) X,Y对应为所属企业的坐标（以企业左下角为原点）\r\n"+"wide 宽\r\n"+"height 高";
				parent.getMethodInfo('sceneIJas.js','popScreenPictures(businessID,objectID,windowID,resourcePath,positionX,positionY,wide,height)','无','添加屏幕图片',paramer);
}

/**
*  球  
*/
function popObject1701(){
				var businessID17 = $('#businessID17').val();
				var objectID17 = $('#objectID17').val();
				var earthColorR = $('#earthColor17r').val();
				var earthColorA = $('#earthColor17a').val();
				var detail17 = $('#detail17').val();
				var gridColorR = $('#gridColor17r').val();
				var grid=$('#grid').combobox('getValue');
				var rearth=$('#rearth').val();
				parent.popEarth(businessID17,objectID17,earthColorR,earthColorA,rearth,grid,detail17,gridColorR);
				locationWithCreate(objectID17,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 所属企业ID，必须设置一个已存在的企业ID\r\n"+"objectID 对象ID，对象ID为一个负数，且应避免重复\r\n"+"earthColorR 球体色，线颜色为16进制颜色。例如：#000033\r\n"+"earthColorA 球体透明度，范围为0到100\r\n"+"detail 细化度，细化度”并设置参数0～100\r\n"+"gridColorR 格网色，线颜色为16进制颜色。例如：#000033";
				parent.getMethodInfo('sceneIJas.js','popEarth(businessID,objectID,earthColorR,earthColorA,detail,gridColorR)','无','添加球',paramer);
}

/**
*  标牌  
*/
function popObject1801(){
				var businessID18 = '0';
				var objectID18 = '-88';
				var typeID18 = '17';
				var name18 = '我的标牌';
				var changeBool18 = 'false';
				var x18 = '0';
				var y18 = '0';
				var z18 = '0';
				var bindID18 = '';
				var rows18 = '1';
				var cols18 = '1';
				var hmargin18 = '15';
				var vmargin18 = '15';
				var rowspace18 = '2';
				var colspace18 = '2';
				var r18 = '204';
				var g18 = '204';
				var b18 = '204';
				var a18 = '128';
				var handleWidth18 = '0.5f';
				var offsetX18 = '30.f';
				var offsetY18 = '30.f';
				var imageFolder18 = '春意盎然';
				var row18 = '0';
				var col18 = '0';
				var fontName18 = 'SIMFANG.TTF';
				var fontSize18 = '20';
				var fR18 = '0';
				var fG18 = '0';
				var fB18 = '0';
				var fA18 = '255';
				var hor18 = '居中';
				var ver18 = '居中';
				var type18 = 'Button';
				var staticImageFile18 = 'button_static.png';
				var clickingImageFile18 = 'button_clicking.png';
				var dataStr18 = 'fff';
				
				parent.popBallon(businessID18,objectID18,typeID18,name18,changeBool18,x18,y18,z18,bindID18,rows18,cols18,hmargin18,
						vmargin18,rowspace18,colspace18,r18,g18,b18,a18,handleWidth18,offsetX18,offsetY18,imageFolder18,row18,col18,fontName18,fontSize18,
						fR18,fG18,fB18,fA18,hor18,ver18,type18,staticImageFile18,clickingImageFile18,dataStr18);
				locationWithCreate(objectID18,'普通');
}
/***********分隔*********/
/**
*	相机：设置相机位置，姿态
*/
function setCamera01(){
				var longitude = $('#longitude').val();
				var latitude = $('#latitude').val();
				var elevation = $('#elevation').val();
				var yawA = $('#yawA').val();
				var pitchingA = $('#pitchingA').val();
				var rollA = $('#rollA').val();
				var locateM = $('#locateM').combobox('getValue');
				parent.setCameraPosition(longitude,latitude,elevation,yawA,pitchingA,rollA,locateM);
				var scrip3="通过此命令可以将视角定位到地球上的唯一点";
				var script03="js参数说明：\r\n"+"longitude 经度\r\n"+"latitude 纬度\r\n"+"elevation 海拔\r\n"+"yawA 偏航角，相机头部与正北方的夹角\r\n"+"pitchingA 俯仰角，相机指向与水平面的夹角\r\n"+"rollA 滚转角，沿相机指向视线为轴旋转\r\n"+"locateM 定位方式，普通/飞行";
				parent.getMethodInfo('scriptIJas.js','setCameraPosition(longitude,latitude,elevation,yawA,pitchingA,rollA,locateM)','无',scrip3,script03);
}

/**
*	相机：根据二维地图比例设置三维相机的位置姿态
*/
function popCamera01(){
				var value = $('#value').val();
				parent.setCameraPositionByMap(value);
				var scrip5="设置当前视角下，地图上的纬度跨度值";
				var script05="js参数说明：\r\n"+"mapScale 二维地图比例，表示当前视角下，地图上的纬度跨度。单位为度。如，设置比例为10.0表示设置当前相机的位置姿态，使之南北方向(即纬度方向)恰好能看到10.0度的范围。东西方向(即经度方向)能看到的范围由视角的比例间接确定。例如：mapScale=”0.0037284”";
				parent.getMethodInfo('scriptIJas.js','setCameraPositionByMap(mapScale)','无',scrip5,script05);
}

/**
*	：命中测试
*/
function hit01(){
				var X = $('#xyX1').val();
				var Y = $('#xyY1').val();
				var t = parent.hitTest(X,Y);
				var etn="根据指定的屏幕点坐标，由该点垂直屏幕向屏幕内引一条射线，获取该射线与场景中相交的第一个对象的信息";
				var calcu01="js参数说明：\r\n"+"X, Y 坐标值是以三维视口左下角为原点，向右为X正向，向上为Y正向的屏幕像素坐标，坐标值是以三维视口左下角为原点";
				parent.getMethodInfo('calculateIJas.js','hitTest(X,Y)',t,etn,calcu01);
}

/**
*	：世界与厂区坐标转换(世界到厂区)
*/
function change01(){
				var ID = $('#idxyzID').val();
				var X = $('#idxyzX').val();
				var Y = $('#idxyzY').val();
				var Z = $('#idxyzZ').val();
				var t = parent.worldFactorySwitch(ID,X,Y,Z);
				var etn1="通过把具体世界坐标，转换成具体企业自己坐标格式对应的坐标";
				var calcu02="js参数说明：\r\n"+"ID 需要转换成对应企业坐标格式的企业ID\r\n"+"X，Y，Z 经纬度海拔";
				parent.getMethodInfo('calculateIJas.js','worldFactorySwitch(ID,X,Y,Z)',t,etn1,calcu02);
}

/**
*	：世界与厂区坐标转换(厂区到世界)
*/
function change1101(){
				var ID = $('#idxyzID1').val();
				var X = $('#idxyzX1').val();
				var Y = $('#idxyzY1').val();
				var Z = $('#idxyzZ1').val();
				var t = parent.factoryWorldSwitch(ID,X,Y,Z);
				var etn2="把企业内对应的坐标，转换成世界坐标";
				var calcu03="js参数说明：\r\n"+"ID 厂区坐标所在的企业\r\n"+"X，Y，Z 厂区坐标X，Y轴，及高";
				parent.getMethodInfo('calculateIJas.js','factoryWorldSwitch(ID,X,Y,Z)',t,etn2,calcu03);
}

/**
*	：获取屏幕坐标点的经纬高
*/
function get101(){
				var X = $('#xyX2').val();
				var Y = $('#xyY2').val();
				var t = parent.getXYZ(X,Y);
				var etn3="获取屏幕坐标对应经纬高";
				var calcu04="js参数说明：\r\n"+"X，Y 向右为X正向的屏幕像素坐标，向上为Y正向的屏幕像素坐标，坐标值是以三维视口左下角为原点";
				parent.getMethodInfo('calculateIJas.js','getXYZ(X,Y)',t,etn3,calcu04);
}

/**
*	：获取指定点到长距离管线的投影坐标
*/
function get201(){
				var X = $('#xyX3').val();
				var Y = $('#xyY3').val();
				//var type = $('#disType').combobox('getValue');
				var type = $('#disType').combobox('getValue');
				var t = parent.getShadowXY(X,Y,type);
				var etn4="获取指定点到长距离管线的投影坐标";
				var calcu05="js参数说明：\r\n"+"X，Y 指定点的经纬度";
				parent.getMethodInfo('calculateIJas.js','getShadowXY(X,Y)',t,etn4,calcu05);
}

/**
*	：坐标转换(西安80坐标和WGS84坐标)
*/
function change101(){
	            var qieLeix=$('#qieLeix01').combobox('getValue');
				var X = $('#X').val();
				var Y = $('#Y').val();
				var t = parent.coordTransform(qieLeix,X,Y);
				var mtp="js参数说明：\r\n"+"type 切换类型：广东管网_西安80_WGS84/广东管网_WGS84_西安80\r\n"+"x,y 平面坐标系X，Y";
				parent.getMethodInfo('aidIJas.js','coordTransform(X,Y)',t,'由西安80平面坐标转换到WGS84经纬坐标，或由WGS84经纬坐标转换到西安80平面坐标。',mtp);
}

/**
*	：计算高度
*/
function count01(){
				var X = $('#xyX4').val();
				var Y = $('#xyY4').val();
				var t = parent.countHighly(X,Y);
				var etn5="计算高度(返回当前加载的地球在指定的经纬点的海拔高度，如果计算失败,高度的值为0.)";
				var calcu06="js参数说明：\r\n"+"X，Y 指定点经纬度";
				parent.getMethodInfo('calculateIJas.js','countHighly(X,Y)',t,etn5,calcu06);
}
/**
*	：暂停、启动三维渲染线程
*/
function xiancheng01(){
				var onoroff = $('#onoroff').combobox('getValue');
				parent.drawControl(onoroff);
				var paramer = "js参数说明：\r\n"+"Flag （类型  ON-开启三维渲染  OFF-暂停三维渲染），开启三维渲染 /暂停三维渲染";
				parent.getMethodInfo('systemIJas.js','drawControl(Flag)','无','启动/暂停三维渲染线程',paramer);
}

/**
*	：操作器切换
*/
function change201(){
				var onoroff01 = $('#onoroff01').combobox('getValue');
				parent.controlSwitch(onoroff01);
				var scrip8="通过切换地球操作器";
				var script08="js参数说明：\r\n"+"Type 操作器类型(0,1)，跟踪球操作器|地球操作器";
				getMethodInfo('scriptIJas.js','controlSwitch(Type)','无',scrip8,script08);
}
/**
*	：漫游器开关
*/
function change301(){
				var onoroff = $('#onoroff').combobox('getValue');
				parent.wanderChanger(onoroff);
				var scrip7="漫游器开关";
				var script07="js参数说明：\r\n"+"Type 漫游开关(ON/OFF)";
				parent.getMethodInfo('scriptIJas.js','wanderChanger(Type)','无',scrip7,script07);
}
/**
*	：推演
*/
function tuiyan1101(){
				var tuiyantype11 = $('#tuiyantype11').combobox('getValue');
				var name11 = $('#name11').val();
				parent.tuiYan(tuiyantype11,name11);
				var spTol1=name11+"推演功能";
				var speci01="js参数说明：\r\n"+"type 新建/移除\r\n"+"name 推演名称";
				parent.getMethodInfo('specialTopicIJas.js','tuiYan(type,name)','无',spTol1,speci01);
}

/**
*	：播放音乐
*/
function music01(){
				var path11 = $('#path11').val();
				var otype = $('#otype').combobox('getValue');
				parent.playMusic(path11,otype);
				var mtp="js参数说明：\r\n"+"Type 类型：开始 | 结束 | 暂停 | 继续\r\n"+"Path 音乐文件路径";
				parent.getMethodInfo('aidIJas.js','playMusic(Path,Type)','无','播放音乐',mtp);
}
/**
*	：画点线面
*/
function hua01(){
				var fid1111 = $('#fid1111').val();
				var oid1111 = $('#oid1111').val();
				var w1111 = $('#w1111').val();
				var type1111 = $('#type1111').combobox('getValue');
				parent.draw(fid1111,oid1111,type1111,w1111);
				var paramer = "js参数说明：\r\n"+"fID 企业ID，用户绘制时生成的对象所属企业，一般设为0，即归到地心企业下即可。例如：bussinessID=”8”\r\n"+"oID 对象ID，用户绘制时生成的对象的ID，必须为负值\r\n"+"Type 操作类型，点 | 线 | 面\r\n"+"Width 线宽度";
				parent.getMethodInfo('sceneIJas.js','draw(fID,oID,Type,Width)','无','启动画点线面的功能，并会根据用户的键盘鼠标操作，画出点线面',paramer);
}

/**
*	：修改屏幕标注
*/
function h101(){
				var hh1oid = $('#hh1oid').val();
				var hh1fid = $('#hh1fid').val();
				var hh1con = $('#hh1con').combobox('getValue');
				var hh1val = $('#hh1val').val();
				var t = parent.changeScreenLabel(hh1oid,hh1fid,hh1con,hh1val);
				var paramer = "js参数说明：\r\n"+"oID 对象ID\r\n"+"fID 企业ID\r\n"+"content 修改内容\r\n"+"value 值";
				parent.getMethodInfo('sceneIJas.js','changeScreenLabel(oID,fID,content,value)',t,'对屏幕标注进行修改，包括对屏幕标注文本和图片',paramer);
}
/**
*	：抓图
*/
function h201(){
				var hh2type = $('#hh2type').combobox('getValue');
				var hh2path = $('#hh2path').val();
				var hh2sosx = $('#hh2sosx').val();
				var hh2sosy = $('#hh2sosy').val();
				var hh2soex = $('#hh2soex').val();
				var hh2soey = $('#hh2soey').val();
				var hh2high = $('#hh2high').val();
				var hh2time = $('#hh2time').val();
				parent.printScreen(hh2type,hh2path,hh2sosx,hh2sosy,hh2soex,hh2soey,hh2high,hh2time);
				var mtp="js参数说明：\r\n"+"type 动作类型（开始、停止）\r\n"+"path 保存图片的路径\r\n"+"startx 抓图区域的范围中起始经度\r\n"+"starty 抓图区域的范围中起始纬度\r\n"+"endx 抓图区域的范围中终点经度\r\n"+"endyx 抓图区域的范围中终点纬度\r\n"+"hight 抓图高度\r\n"+"time 抓图单张图片间的时间间隔";
				parent.getMethodInfo('aidIJas.js','printScreen(type,path,startx,starty,endx,endy,high,time)','无','类型参数为“停止”时，的参数无效；类型参数为“开始”时，系统按照参数，开始在指定的区域连续抓图，保存单张图片到指定路径；抓图完成时，会将所有抓取到的图片合并为一张图片',mtp);
}
/**
*	：选择本地文件
*/
function h301(){
				var hh3ob = $('#hh3ob').val();
				var hh3type = $('#hh3type').combobox('getValue');
				var t = parent.choiceFile(hh3ob,hh3type);
				var mtp="js参数说明：\r\n"+"s 'shp文件(*.shp)|*.shp|所有文件(*.*)|*.*|' 设定选择文件时,文件的后缀.上面例子中是显示shp文件或是所有文件如果想只显示IVE文件,可以这样ive文件(*.ive)|*.ive|\r\n"+"type 是否多选（true、false）";
				parent.getMethodInfo('aidIJas.js','choiceFile(s,type)',t,'提供一个可以选择文件的对话框',mtp);
}
/**
*	：坐标系切换
*/
function h401(){
				var hh4type = $('#hh4type').combobox('getValue');
				parent.coordSwitch(hh4type);
				var coord11="坐标系切换";
				var coord111="js参数说明：\r\n"+"type 动作类型：WGS84/XIAN80 目标坐标系的类型";
				parent.getMethodInfo('systemIJas.js','coordSwitch(hh4type)','无',coord11,coord111);
				
}
/**
*	：事件处理器管理开启
*/
function q101(){
				var qq1NAME = $('#qq1NAME').val();
				parent.thingManageOn(qq1NAME);
				var spTol2="事件处理器管理(开启)";
				var speci02="js参数说明：\r\n"+"name 开启事务的处理器名称";
				parent.getMethodInfo('specialTopicIJas.js','thingManageOn(name)','无',spTol2,speci02);
}
/**
*	：事件处理器管理关闭
*/
function q201(){
				var qq2NAME = $('#qq2NAME').val();
				parent.thingManageOff(qq2NAME);
				var spTol3="事件处理器管理(关闭)";
				var speci03="js参数说明：\r\n"+"name 关闭事务的处理器名称";
				parent.getMethodInfo('specialTopicIJas.js','thingManageOff(name)','无',spTol3,speci03);
}
/**
*	：地形剖切
*/
function q301(){
		var qq3X1 = $('#qq3X1').val();
		var qq3Y1 = $('#qq3Y1').val();
		var qq3Z1 = $('#qq3Z1').val();
		var qq3X2 = $('#qq3X2').val();
		var qq3Y2 = $('#qq3Y2').val();
		var qq3Z2 = $('#qq3Z2').val();
		var qq3X3 = $('#qq3X3').val();
		var qq3Y3 = $('#qq3Y3').val();
		var qq3Z3 = $('#qq3Z3').val();
		var qq3D1 = $('#qq3D1').val();
		var Point1 = new CutPoint(qq3X1,qq3Y1,qq3Z1);
		var Point2 = new CutPoint(qq3X2,qq3Y2,qq3Z2);
		var Point3 = new CutPoint(qq3X3,qq3Y3,qq3Z3);
		var arrayPoint = [Point1,Point2,Point3];
		parent.geographyCut(arrayPoint,qq3D1);
		var paramer = "js参数说明：\r\n"+"arrayPoint 点对象数组\r\n"+"deep 深度";
		parent.getMethodInfo('sceneIJas.js','geographyCut(arrayPoint,deep)','无','通过深度，贴图重复率，显示底框是否启用来刨切地面图形',paramer);
}
/**
*	：开启透明
*/
function q401(){
				var qq4val = $('#qq4val').val();
				parent.diaphaneityOn(qq4val);
				var paramer = "js参数说明：\r\n"+"n 0-100间的浮点值。0为全透明，100为不透明";
				parent.getMethodInfo('sceneIJas.js','diaphaneityOn(n)','无','关于地表的透明度设置',paramer);
}
/**
*	：添加管道
*/
function e101(){
				var ee1l11 = $('#ee1l11').val();
				var ee1l12 = $('#ee1l12').val();
				var ee1h1 = $('#ee1h1').val();
				var ee1l21 = $('#ee1l21').val();
				var ee1l22 = $('#ee1l22').val();
				var ee1h2 = $('#ee1h2').val();
				var ee1bj = $('#ee1bj').val();
				var ee1r = $('#ee1r').val();
				parent.pipeAdd(ee1l11,ee1l12,ee1h1,ee1l21,ee1l22,ee1h2,ee1bj,ee1r);
				var paramer = "js参数说明：\r\n"+"lon1,lat1,h1 起点经纬度高\r\n"+"lon2,lat2,h2 终点经纬度高\r\n"+"r 管子的半径\r\n"+"ee1r 管子的颜色，颜色为16进制颜色。例如：#000033";
				parent.getMethodInfo('sceneIJas.js','pipeAdd(lon1,lat1,h1,lon2,lat2,h2,r,ee1r)','无','通过选择类型（创建，清除），及两点的经纬高，颜色等参数，来添加或清除管段',paramer);
}
/**
*	：创建右键菜单
*/
function e201(){
				var content = $('#contentMenu1').val();
				var ee2ID = $('#ee2ID').val();
				var ee2X = $('#ee2X').val();
				var ee2Y = $('#ee2Y').val();
				parent.rightMenu(ee2ID,content,ee2X,ee2Y);
				var mtp="js参数说明：\r\n"+"content 菜单显示内容"+"ID 右键弹出菜单对象ID \r\n"+"X,Y 以屏幕左上角为原点，X向右为正,Y向下为正";
				parent.getMethodInfo('uiIJas.js','rightMenu(ID,content,X,Y)','无','创建一个满足用户需求的右键菜单，该菜单在双击模型之后在指定的位置X,Y生成。',mtp);
}
/**
*	：返回图层元素列表
*/
function d101(){
				var dd1ID = $('#dd1ID').val();
				var t = parent.returnPicElementList(dd1ID);
				var paramer = "js参数说明：\r\n"+"ID 图层ID，若指定此参数，则返回此ID下的所有图层元素，否则返回所有图层下的图层元素。例如：ID=”22”";
				parent.getMethodInfo('sceneIJas.js','returnPicElementList(ID)',t,'查找出所有所属图层ＩＤ为输入ID的设备',paramer);
}
/**
 * 返回矢量图层信息
 */
function vector101(){
	var picContent = $('#picContent option:selected').text();
	//var picContent = $('#picContent').combobox('getValue');
	//var picNum = $('#picNum').combobox('getValue');
	var picNum = $('#picNum option:selected').text();
	if(picNum=="全部"){
		var t = parent.returnVectorPicElementList(picContent,picNum);
	}else{
		var t = parent.returnVectorPicElementList(picContent,picNum-1);
	}
	var paramer = "js参数说明：\r\n"+"content 内容，分为层名和个数\r\n"+"location 位置，分为全部和位置";
	parent.getMethodInfo('sceneIJas.js','returnVectorPicElementList(content,location)',t,'获取矢量图层信息',paramer);
	
}

/**
 * 矢量图层控制
 */
function vectorCon01(){
	var vectorName = $('#vectorName option:selected').text();
	var vectorHideOrShow = $('#vectorHideOrShow option:selected').text(); 
	parent.ectorPicControl(vectorName,vectorHideOrShow);
	var paramer = "js参数说明：\r\n"+"vectorName 矢量图层名称\r\n"+"vectorHideOrShow 类型，显示或隐藏";
	parent.getMethodInfo('sceneIJas.js','ectorPicControl(vectorName,vectorHideOrShow)','无','矢量图层控制',paramer);
	
}

/**
*	：图层控制
*/
function d201(){
				var dd2id = $('#dd2id').val();
				var dd2can = $('#dd2can').combobox('getValue');
				parent.picControl(dd2id,dd2can);
				var paramer = "js参数说明：\r\n"+"ID 图层的ID\r\n"+"s 参数，隐藏 / 显示";
				parent.getMethodInfo('sceneIJas.js','picControl(ID,s)','无','控制图层的显隐',paramer);
}
/**
*	：光照位置
*/
function d301(){
				var dd3ID = $('#dd3ID').val();
				var dd3type = $('#dd3type').combobox('getValue');
				var dd3loX = $('#dd3lox').val();
				var dd3loY = $('#dd3loy').val();
				var dd3loZ = $('#dd3loz').val();
				parent.lightLocation(dd3ID,dd3type,dd3loX,dd3loY,dd3loZ);
				var paramer = "js参数说明：\r\n"+"对象ID\r\n"+"type 光照对象，对象分为地球 和厂区，说明是配置地球的光照位置还是厂区的光照位置\r\n"+"(locationX，locationY，locationZ) 经纬度高，设置光源的位置，（双精度浮点型）";
				getMethodInfo('systemIJas.js','lightLocation(ID,type,locationX,locationY,locationZ)','无','通过选择光照对象和位置（经纬高），来显示光照位置',paramer);
}
/**
*	动画 ： 播放脚本
*/
function playCortoon01(){
	parent.DonghuaReback(resultt);
				var scriptName = $('#scriptName').val();
				var x = $('#dataX').val();
				var y = $('#dataY').val();
				var type = $('#playerType').combobox('getValue');
				parent.playScript(scriptName,x,y,type);
				var spTol4="在三维地球中播放指定名称的脚本";
				var speci044="js参数说明：\r\n"+"scriptName 播放脚本的名字";
				parent.getMethodInfo('specialTopicIJas.js','playScript(scriptName)','无',spTol4,speci044);	
}
/**
*	设置播放器的位置
*/
/*function setPlayer01(){
				var x = $('#dataX').val();
				var y = $('#dataY').val();
				var type = $('#playerType').combobox('getValue');
				parent.setPlayerPosition(x,y,type);
				var spTol4 = "设置播放器的位置";
				var speci044 = "js参数说明：\r\n" + "X 位置X\r\n"+"Y 位置Y\r\n"+"type 显示|隐藏";
				parent.getMethodInfo('specialTopicIJas.js', 'setPlayerPosition(x,y,type)', '无',spTol4, speci044);
}*/


/**
 * 监听带有事件的动画
 */
function resultt(aa) {
	alert(aa);
}


/**
*	：鹰眼相关操作
*/
function y101(){
				var yy1type = $('#yy1type').combobox('getValue');
				var yy1st = $('#yy1st').combobox('getValue');
				parent.yingyan(yy1type,yy1st);
}

/**
*	高亮模型
*/
function HIGHLIGHT01(){
				var A = $('#highlightid').val();
				var color = $('#highlightR').val();
				parent.highLight(A,color);
				var paramer = "js参数说明：\r\n"+"ID 处理的对象的ID\r\n"+"color 所需要高亮成的颜色，颜色为16进制颜色。例如：#FF0033";
				parent.getMethodInfo('sceneIJas.js','highLight(ID,color)','无','高亮设备',paramer);
}
/**
*	天气控制
*/
function WEATHER01(){
				var A = $('#qihou').combobox('getValue');
				var B = $('#zhuangtai').combobox('getValue');
				parent.weather(A,B);
				var paramer = "js参数说明：\r\n"+"X 气候：雨、雪\r\n"+"Y 状态:无、小、中、大";
				parent.getMethodInfo('systemIJas.js','weather(X,Y)','无','控制三维场景中天气状态',paramer);
}
/**
*	挂件显隐
*/
function WIDGET01(){
				var A = $('#wwwwtype').combobox('getValue');
				var B = $('#wwwwstatus').combobox('getValue');
				parent.widget(A,B);
				var paramer = "js参数说明：\r\n"+"X 类型：HINTBAR（状态栏）、NAVIGATIONMAP（导航图）、CONTROLPANEL（控制面板）、FOCUSCROSS（焦点十字）\r\n"+"Y 显隐的状态，设置（ON/OFF）";
				parent.getMethodInfo('systemIJas.js','widget(X,Y)','无','控制地球窗口上功能挂架的显隐',paramer);
}

/**
*	查询挂件状态
*/
function WIDGETSTATUS01(){
				var A = $('#wwwtype').combobox('getValue');
				var t = parent.widgetStatus(A);
				var paramer = "js参数说明：\r\n"+"n 类型：HINTBAR（状态栏）、NAVIGATIONMAP（导航图）、CONTROLPANEL（控制面板）、FOCUSCROSS（焦点十字）";
				parent.getMethodInfo('systemIJas.js','widgetStatus(n)',t,'用于查询某一个挂件的是否开启',paramer);
}

/**
*	光照的设置与查询
*/
function LightSettingAndSearch01(){
				var lightSetType1 = $('#lightSetType1').combobox('getValue');
				var lightSetType2 = $('#lightSetType2').combobox('getValue');
				var lightSetType3 = $('#lightSetType3').combobox('getValue');
				var lightSetType4 = $('#lightSetType4').val();
				var lightSetType7 = $('#lightSetType7').val();
				var t = parent.lightSet(lightSetType1,lightSetType2,lightSetType3,lightSetType4,lightSetType7);
				var paramer = "js参数说明：\r\n"+"T 命令类型，区别是光照查询还是关照设置 \r\n"+"LT 光照类型，区别是地球光照还是厂区光照\r\n"+"LP 光照属性，设置关照属性，例如：光照属性=“漫反射”\r\n"+"lightColor 光照颜色，光照颜色为16进制颜色。例如：#FFCC00\r\n"+"lightColorA 透明度，透明度范围为0到100";
				parent.getMethodInfo('systemIJas.js','lightSet(T,LT,LP,lightColor,lightColorA)',t,'对光照进行设置和查询',paramer);
}	

/**
*	画点
*/
function drawPointMsl1101(){
				var drawPointMsl1 = $('#drawPointMsl1').val();
				var drawPointMsl2 = $('#drawPointMsl2').val();
				var drawPointMsl3 = $('#drawPointMsl3').val();
				var drawPointMsl4 = $('#drawPointMsl4').val();
				var drawPointMsl5 = $('#drawPointMsl5').val();
				var drawPointMsl5Z = $('#drawPointMsl5Z').val();
				var drawPointMsl6 = $('#drawPointMsl6').val();
				var drawPointMsl9 = $('#drawPointMsl9').val();
				var drawPointMsl10= $('#drawPointMsl10').val();
				parent.drawPoint(drawPointMsl1,drawPointMsl2,drawPointMsl3,drawPointMsl4,drawPointMsl5,drawPointMsl5Z,drawPointMsl6,drawPointMsl9,drawPointMsl10);
				//locationWithCreate(drawPointMsl2,'普通');
				var paramer = "js参数说明：\r\n"+"FID 创建对象所属的企业ID，注：该企业ID只能为0。只能添加到地心企业\r\n"+"ID 创建对象的ID，对应ID必须以负号开头。例如：-123132"+"name 创建对象的名称\r\n"+"X 点所在的经度\r\n"+"Y 点所在的纬度\r\n"+"Z 点所在的高度\r\n"+"colorR 点的颜色，点颜色为16进制颜色。例如：#FFFF33\r\n"+"colorA 透明度，范围为0到100\r\n"+"N 可视对象点的大小\r\n";
				parent.getMethodInfo('sceneIJas.js','drawPoint(FID,ID,name,X,Y,Z,colorR,colorA,N)','无','通过接口在三维地球中创建点类型的可视化对象。主要用于根据数据信息在地球上创建可视化点，并通过观察点的密度，直观的看出数据信息对应的事物的分布情况',paramer);
}

/**
*	画线
*/
function drawLineMsl11101(){
				var a = $('#drawLineMsl1').val();
				var b = $('#drawLineMsl2').val();
				var c = $('#drawLineMsl3').val();
				var d = $('#drawLineMsl4').val();
				var e = $('#drawLineMsl5').val();
				var eZ = $('#drawLineMsl5z').val();
				var f = $('#drawLineMsl6').val();
				var g = $('#drawLineMsl7').val();
				var gZ = $('#drawLineMsl7z').val();
				var h = $('#drawLineMsl8').val();
				var k= $('#drawLineMsl11').val();
				var l= $('#drawLineMsl12').val();
				var m= $('#drawLineMsl13').val();
				var p= $('#drawLineMsl16').val();
				var q= $('#drawLineMsl17').val();
				parent.drawLine(a,b,c,d,e,eZ,f,g,gZ,h,k,l,m,p,q);
				//locationWithCreate(b,'普通');
				var paramer = "js参数说明：\r\n"+"FID 企业ID，注：该企业ID只能为0。只能添加到地心企业\r\n"+"ID 对象ID，创建对象的ID，对应ID必须以负号开头\r\n"+"name 创建对象的名称\r\n"+"(AX,AY,BX,BY) 两点的经纬度\r\n"+"pColor 点的颜色，点颜色为16进制颜色。例如：#FFFF33\r\n"+"pColorA 点的透明度，范围为0到100\r\n"+"s 可视对象点的大小\r\n"+"lColor 线的颜色，线颜色为16进制颜色。例如：#FFFF33\r\n"+"lColorA 线的透明度，范围为0到100\r\n"+"N 线宽度，可视对象线的大小";
				parent.getMethodInfo('sceneIJas.js','drawLine(FID,ID,name,AX,AY,AZ,BX,BY,BZ,pColor,pColorA,s,lColor,lColorA,N)','无','通过接口在三维地球中创建线类型的可视化对象',paramer);
}

/**
*	画面
*/
function drawSurfaceMsl11101(){
				var a = $('#drawSurfaceMsl1').val();
				var b = $('#drawSurfaceMsl2').val();
				var c = $('#drawSurfaceMsl3').val();
				var d = $('#drawSurfaceMsl4').val();
				var e = $('#drawSurfaceMsl5').val();
				var eZ = $('#drawSurfaceMsl5z').val();
				var f = $('#drawSurfaceMsl6').val();
				var g = $('#drawSurfaceMsl7').val();
				var gZ = $('#drawSurfaceMsl7z').val();
				var h = $('#drawSurfaceMsl8').val();
				var i = $('#drawSurfaceMsl9').val();
				var iZ = $('#drawSurfaceMsl9z').val();
				var j= $('#drawSurfaceMsl10').val();
				var m= $('#drawSurfaceMsl13').val();
				var n= $('#drawSurfaceMsl14').val();
				var o= $('#drawSurfaceMsl15').val();
				var r= $('#drawSurfaceMsl18').val();
				var s= $('#drawSurfaceMsl19').val();
				var t= $('#drawSurfaceMsl20').val();
				var w= $('#drawSurfaceMsl23').val();
				var sufaceArray=new Array();
				var suface1=new suface(d,e,eZ);
				var suface2=new suface(f,g,gZ);
				var suface3=new suface(h,i,iZ);
				sufaceArray.push(suface1);
				sufaceArray.push(suface2);
				sufaceArray.push(suface3);
				parent.drawSurface(a,b,c,j,m,n,o,r,s,t,w,sufaceArray);
				//locationWithCreate(b,'普通');
				var paramer = "js参数说明：\r\n"+"FID 企业ID，企业ID，注：该企业ID只能为0。只能添加到地心企业\r\n"+"OID 对象ID，创建对象的ID，对应ID必须以负号开头"+"NAME 名称，创建对象的名称\r\n"+"pColor 点的颜色，点颜色为16进制颜色。例如：#FFFF33\r\n"+"PA 点的透明度，，范围为0到100\r\n"+"PS 可视对象点的大小\r\n"+"lColor 线的颜色，线颜色为16进制颜色。例如：#FFFF33\r\n"+"LA 线的透明度，范围为0到100\r\n"+"LS 线宽度，可视对象线的大小\r\n"+"sColor 面颜色为16进制颜色。例如：#FF0033\r\n"+"SA 面透明度，范围为0到100\r\n"+"objecArry 点对象数组。构造方法：suface(x,y,z)，x,y,z对应为点位置（经纬度高）";
				parent.getMethodInfo('sceneIJas.js','drawSurface(FID,OID,NAME,pColor,PA,PS,lColor,LA,LS,sColor,SA,objecArry)','无','通过接口在三维地球中创建面类型的可视化对象',paramer);
}
/**
*	动画1 ： 播放含有事件的脚本动画
*/
function playControlCortoon01(){
				var id = $('#scriptName').val();
				//alert(id);
			    parent.playScript(id);
				parent.DonghuaReback(aa);
				function aa(ww){
					
				}
}
/**
*	模型 ： 根据模型名，查询模型ID
*/
function SelectModelID01(){
				var id = $('#objectName').val();
				var modelID = parent.listModelName(id);
				var paramer = "js参数说明：\r\n"+"name 模型名称";
				parent.getMethodInfo('sceneIJas.js','listModelName(name)',modelID,'根据模型名称查询模型ID',paramer);
}
/**
*	国际化
*/
function intontroalControl01(){
				var id = $('#language').combobox('getValue');
				parent.languageControl(id);
				var paramer = "js参数说明：\r\n"+"language 所要设置的语言类型，英文|中文";
				parent.getMethodInfo('systemIJas.js','languageControl(language)','无','地球上文字国际化',paramer);
}

/**
*	启动地球前设置语言
*/
function setLanguage01(){
				var language = $('#languageType').combobox('getValue');
				parent.setLanguageType(language);
				var paramer = "js参数说明：\r\n"+"language 所要设置的语言类型，英文|中文";
				parent.getMethodInfo('systemIJas.js','setLanguageType(language)','无','启动地球前设置语言',paramer);
}
/**
*	截图
*/
function pictureControl01(){
				var name = $('#pictureName').val();
				var prictureUrl = $('#pirctureUrl').val();
				parent.savePicture(prictureUrl,name);
				var paramer = "js参数说明：\r\n"+"pictureUrl 输出截图所放的位置\r\n"+"name 截图的名称";
				parent.getMethodInfo('aidIJas.js','savePicture(pictureUrl,name)','无','输出截图',paramer);
}
/**
*	Tip标牌:
*/
function creatTip01(){
	            var drawPointMsl1 = $('#drawPointMsl101').val();
				var drawPointMsl2 = $('#drawPointMsl201').val();
				var drawPointMsl3 = $('#drawPointMsl301').val();
				var point = $('#point').val();
				var wordsize = $('#wordsize').val();
				var wordtype = $('#wordtype').val();
				var worda = $('#worda').val();
				var wordr = $('#wordr').val();
				var bordera = $('#bordera').val();
				var borderr= $('#borderr').val();
				var wordname= $('#wordname').val();
				var locationx= $('#locationx').val();
				var locationy= $('#locationy').val();
				var locationz= $('#locationz').val();
				if(point!=null){
					parent.objectLocation(point,"普通");
				}
				parent.createTipPanel(drawPointMsl1,drawPointMsl2,drawPointMsl3,point,wordsize,wordtype,worda,wordr,bordera,borderr,wordname,locationx,locationy,locationz);
				locationWithCreate(drawPointMsl2,'普通');
				var paramer = "js参数说明：\r\n"+"FID 企业ID，对象所属企业ID\r\n"+"ID 对象ID，对应ID必须为负值，并且不能重复\r\n"+"name 名称\r\n"+ "point 绑定对象ID\r\n"+"wordsize 字体大小\r\n"+"wordtype 字体样式，添加的文字的样式。例如：SIMYOU.TTF\r\n"+"worda 文字透明度，范围为0到100\r\n"+"wordr 文字的颜色，颜色为16进制颜色。例如：#FF0033\r\n"+"bordera 边框透明度，范围为0到100\r\n"+"bordera 边框的颜色，颜色为16进制颜色。例如：#00FFFF\r\n"+"wordname 文字内容\r\n"+"(locationx,locationy,locationz) 空间位置。如果绑定对象id为空或者不存在的情况下，该属性产生效果";
				parent.getMethodInfo('sceneIJas.js','createTipPanel(FID,ID,name,pointer,wordsize,wordtype,worda,wordr,bordera,borderr,wordname,locationx,locationy,locationz)','无','创建一个Tip标牌',paramer);
}
/**
*	Text标牌:
*/
function creatText01(){
				var drawPointMsl1 = $('#drawPointMsl102').val();
				var drawPointMsl2 = $('#drawPointMsl202').val();
				var drawPointMsl3 = $('#drawPointMsl302').val();
				var title = $('#tiptitle').val();
				var titelFont = $('#rowFont').val();
				var titelFontHeight = $('#titelFontHeight').val();
				var titelBkpic = $('#titelBkpic').val();
				var titelColor = $('#titelColor').val();
				var titelColorA = $('#titelColora').val();
				var bkpic = $('#bkpic').val();
				var minscale= $('#minscale').val();
				var maxscale= $('#maxscale').val();
				var posX01= $('#posx001').val();
				var posX= $('#posx').val();
				var posY= $('#posy').val();
				var posZ= $('#posz').val();
				var bkColor = $('#bkColor').val();
				var bkColorA = $('#bkColora').val();
				var alignType= $('#titelFont').combobox('getValue');
				var usegrid= $('#bindPosx').val();
				var gridLineWidth= $('#bindPosy').val();
				var gridColor = $('#rowColor').val();
				var gridColorA = $('#rowColora').val();
				var itemArry=new Array();
				var item1=new textPanel("#FF0000","100","simfang.ttf","12","center","中盈高科","1","1");
				var item2=new textPanel("#FF0000","100","simfang.ttf","12","center","中盈高科","2","1");
				var item3=new textPanel("#FF0000","100","simfang.ttf","12","center","中盈高科","3","1");
				var item4=new textPanel("#FF0000","100","simfang.ttf","12","center","中盈高科","1","2");
				var item5=new textPanel("#FF0000","100","simfang.ttf","12","center","中盈高科","1","3");
				var item6=new textPanel("#FF0000","100","simfang.ttf","12","center","中盈高科","3","3");
				itemArry.push(item1);
				itemArry.push(item2);
				itemArry.push(item3);
				itemArry.push(item4);
				itemArry.push(item5);
				itemArry.push(item6);
				
				if(posX01!=null){
					//alert(posX01);
					parent.objectLocation(posX01,"普通");
				}
				parent.createTextPanel(drawPointMsl1,drawPointMsl2,drawPointMsl3,title,titelFont,titelFontHeight,titelBkpic,titelColor,titelColorA,bkpic,minscale,
						maxscale,posX01,posX,posY,posZ,bkColor,bkColorA,alignType,usegrid,gridLineWidth,gridColor,gridColorA,itemArry)
				locationWithCreate(drawPointMsl2,'普通');
				var paramer = "js参数说明：\r\n"+"busessID 企业ID，对象所属企业ID\r\n"+"ID 对象ID，对应ID必须为负值，并且不能重复\r\n"+"name 标牌名称\r\n"+"titel 标题\r\n"+"titelFont 标题字体，例如：wqy-zenhei_0.ttc\r\n"+"titelFontHeight 标题行高度\r\n"+"titelBkpic 标牌背景图片\r\n"+"titelColor 标题颜色，颜色为16进制颜色，如：#000033\r\n"+"titelColorA 标题透明度，范围为0到100\r\n"+"bkpic 背景图片\r\n"+"minscale 最小缩放倍数\r\n"+"maxscale 最大缩放倍数\r\n"+"posX01 绑定对象ID\r\n"+"posX,posY,posZ:绑定位置。如果绑定对象id为空或者不存在的情况下，该属性产生效果\r\n"+"bkColor 背景颜色，颜色为16进制颜色。例如：#000033\r\n"+"bkColorA 背景透明度，范围为0到100\r\n"+"alignType 对齐方式，有三种：center， left， right\r\n"+"usegrid 是否有网格，True或false\r\n"+"gridLineWidth 网格宽度\r\n"+"gridColor 网格颜色，颜色为16进制颜色。例如：#000033\r\n"+"gridColorA 网格透明度，范围为0到100\r\n"+"itemObject 标牌内容对象";
				parent.getMethodInfo('sceneIJas.js','createTextPanel(busessID,ID,name,titel,titelFont,titelFontHeight,titelBkpic,titelColor,titelColorA,bkpic,minscale,maxscale,posX01,posX,posY,posZ,bkColor,bkColorA,alignType,usegrid,gridLineWidth,gridColor,gridColorA,itemObject)','无','创建一个Text标牌',paramer);
}
/**
*	html标牌:
*/
function creathtml01(){
				var drawPointMsl1 = $('#drawPointMsl103').val();
				var drawPointMsl2 = $('#drawPointMsl203').val();
				var drawPointMsl3 = $('#drawPointMsl303').val();
				var htmlUrl = $('#htmlUrl').val();
				var bindPosX = $('#bindPosxx').val();
				var bindPosY = $('#bindPosyy').val();
				var bindPosZ = $('#bindPoszz').val();
				var weight = $('#weight').val();
				var height = $('#height').val();
				var titelBkpic = $('#titelBkcolor').val();
				var titelBkpicA = $('#titelBkpica').val();
				var visiable = $('#visiable').val();
				var titelBkpic1 = $('#titelBkcolor1').val();
				var titelBkpicA1 = $('#titelBkpica1').val();
				
				var drag = $('#drag').val();
				var spaceLift = $('#spaceLift').val();
				var spaceUp = $('#spaceUp').val();
				var spaceRight = $('#spaceRight').val();
				var spaceDown = $('#spaceDown').val();
				var foundSize = $('#foundSize').val();
				var dragectLeft= $('#dragectLeft').val();
				var dragectTop= $('#dragectTop').val();
				var dragectWidth= $('#dragectWidth').val();
				var dragectHeight= $('#dragectHeight').val();
				/**if(visiable!=null||visiable!=""){
					//alert(visiable);
					parent.objectLocation(visiable,"普通");
				}*/
		 		parent.createHtmlPanel(drawPointMsl1,drawPointMsl2,drawPointMsl3,htmlUrl,bindPosX,bindPosY,bindPosZ,weight,height,titelBkpic,titelBkpicA,visiable,titelBkpic1,titelBkpicA1,drag,spaceLift,spaceUp,spaceRight,spaceDown,foundSize,dragectLeft,dragectTop,dragectWidth,dragectHeight);
				var paramer = "js参数说明：\r\n"+"FID 企业ID，对象所属企业ID\r\n"+"ID 对象ID，对应ID必须为负值，并且不能重复\r\n"+"name 标牌名称\r\n"+"htmlUrl html内容的地址\r\n"+"(bindPosX,bindPosY,bindPosZ) 标牌的位置。如果绑定对象id不存在或者为空，那没绑定位置起作用\r\n"+"weight 宽\r\n"+"height 高\r\n"+"titelBkpic 标牌的背景颜色，颜色为16进制颜色，如：#000033\r\n"+"titelBkpicA 标牌背景透明度，范围为0到100\r\n"+"visiable 绑定对象ID\r\n"
				+"titelBkpic1：关闭按钮颜色\r\n"+"titelBkpicA1：关闭按钮透明度\r\n"
				+"drag：html标牌是否可以拖动，true或false\r\n"+"spaceLift,spaceUp,spaceRight,spaceDown：四个数值的含义分别为：左边距 上边距 右边距 下边距如果这里面的边距为0，则不显示边框。\r\n"+"foundSize：此参数表示标牌角上的圆角大小，如果是0，则是矩形框。";
				parent.getMethodInfo('sceneIJas.js','createHtmlPanel(FID,ID,name,htmlUrl,bindPosX,bindPosY,bindPosZ,weight,height,titelBkpic,titelBkpicA,visiable)','无','创建一个HTML标牌',paramer);
}
/**
*	创建动态标牌:
*/
function createRun01(){
		var objectID01 = $('#objectID01').val();
		var modelID01 = $('#modelID01').val();
		var posX = $('#posX01').val();
		var posY = $('#posY01').val();
		var posZ = $('#posZ01').val(); 
		var ballonInfor1 = new BallonInfor('中盈',1,2);
		var ballonInfor2 = new BallonInfor('北京',2,4);
		var ballonInfor3 = new BallonInfor('科大',1,3);
		var ballonInfor4 = new BallonInfor('天工',2,2);
		var arrayContent = [ballonInfor1,ballonInfor2,ballonInfor3,ballonInfor4];
 		parent.createRunPanel(objectID01,modelID01,arrayContent,posX,posY,posZ);
		var paramer = "js参数说明：\r\n"+"objectId 对象ID\r\n"+"modelId 绑定的模型ID\r\n"+"arrayContent 内容对象数组\r\n"+"x 经度\r\n"+"y 纬度\r\n"+"z 高度";
		parent.getMethodInfo('sceneIJas.js','createRunPanel(objectId,modelId,arrayContent,x,y,z)','无','创建一个动态标牌',paramer);
}
/**
*	更新动态标牌:
*/
function updateRun01(){
				var objectID02 = $('#objectID02').val();
				//alert(objectID02);
				//setTimeout("parent.updateRunPanel(objectID02,'2','2','Web标牌')", 1000);
				var i = Math.floor(Math.random()*10);
		 		parent.updateRunPanel(objectID02,'2','2',i);
		 		//注释掉是为了保证正常的全屏
				var paramer = "js参数说明：\r\n"+"objectID  需要修改的标牌标号\r\n"+"row 行数\r\n"+"col 列数\r\n"+"content 内容";
				parent.getMethodInfo('sceneIJas.js','updateRunPanel(objectID,row,col,content)','无','修改标牌的内容',paramer);
				setTimeout("updateRun02()", 500);
}
function updateRun02(){
	var objectID02 = $('#objectID02').val();
	var i = Math.floor(Math.random()*10);
	parent.updateRunPanel(objectID02,'2','2',i);
}

/**
*	修改Tip标牌:
*/
var num=0;
function updateTip01(){
	
				var objectTipID = $('#objectTipID').val();
				//alert(objectTipID);
				//setTimeout("parent.updateRunPanel(objectID02,'2','2','Web标牌')", 1000);
				var content = ["1000","2000","3000","4000","4000","5000","6000","7000","8000","9000"];
				var color = ["#00ffff","#00ff00","#0000ff","#ffff0","#00ffff"];
				var i = Math.floor(Math.random()*10);
				var j = Math.floor(Math.random()*5);
				parent.updateTipPanel(objectTipID,content[i],color[j]);
			/*	var paramer = "js参数说明：\r\n"+"objectID  需要修改的标牌标号\r\n"+"content 修改的内容\r\n"+"wordr 字体颜色";
				parent.getMethodInfo('sceneIJas.js','updateTipPanel(ID,content,wordr)','','更改Tip标牌',paramer);*/
				if(num<5){
					setTimeout("updateTip01()", 500);
					num++;
				}
}

/**
*	修改Text标牌:
*/
function updateText01(){
				var content = ["10","20","30","40","50","60","70","80","90","00"];
				var color = ["#00ffff","#00ff00","#0000ff","#ffff0","#00ffff"];
				var i = Math.floor(Math.random()*10);
				var j = Math.floor(Math.random()*5);
				var objectTextID = $('#objectTextID').val();
				var itemArry=new Array();
				var item1=new uPtextPanel("#FF0000","100","simfang.ttf","12","center","A属性","1","1");
				var item2=new uPtextPanel("#FF0000","100","simfang.ttf","12","center","B属性","2","1");
				var item3=new uPtextPanel("#FF0000","100","simfang.ttf","12","center","C属性","3","1");
				var item4=new uPtextPanel(color[j],"100","simfang.ttf","12","center",content[i],"1","2");
				var item5=new uPtextPanel(color[j],"100","simfang.ttf","12","center",content[i],"1","3");
				var item6=new uPtextPanel(color[j],"100","simfang.ttf","12","center",content[i],"3","3");
				itemArry.push(item1);
				itemArry.push(item2);
				itemArry.push(item3);
				itemArry.push(item4);
				itemArry.push(item5);
				itemArry.push(item6);
				//alert(objectID02);
				//setTimeout("parent.updateRunPanel(objectID02,'2','2','Web标牌')", 1000);
				
		 		parent.updateTextPanel(objectTextID,itemArry);
				/*var paramer = "js参数说明：\r\n"+"objectID  需要修改的标牌标号\r\n"+"row 行数\r\n"+"col 列数\r\n"+"content 内容";
				parent.getMethodInfo('sceneIJas.js','updateTextPanel(ID,content,wordr)','无','修改Text标牌',paramer);*/
		 		if(num<5){
		 			setTimeout("updateText01()", 500);
		 			num++;
		 		}
}

/**
*	更新动态标牌:
*/
/*function updateHtml01(){
				var objectHtmlID = $('#objectHtmlID').val();
				//alert(objectID02);
				//setTimeout("parent.updateRunPanel(objectID02,'2','2','Web标牌')", 1000);
				var i = Math.floor(Math.random()*10);
		 		parent.updateRunPanel(objectID02,'2','2',i);
				var paramer = "js参数说明：\r\n"+"objectID  需要修改的标牌标号\r\n"+"row 行数\r\n"+"col 列数\r\n"+"content 内容";
				parent.getMethodInfo('sceneIJas.js','updateRunPanel(objectID,row,col,content)','无','修改标牌的内容',paramer);
				setTimeout("updateRun01()", 500);
}*/

/**
*	对象自动释放:
*/
function setRun01(){
				var objectID03 = $('#objectID03').val();
				var time = $('#time').val();
			//	alert(objectID03);
		 		parent.setRunPanel(objectID03,time);
				var paramer = "js参数说明：\r\n"+"objectID03 所要释放的标牌id\r\n"+"time 设置多长时间后释放";
				parent.getMethodInfo('sceneIJas.js','setRunPanel(objectID03,time)','无','标牌对象自动释放',paramer);
}
/**
*	创建可视对象标牌:
*/
function makeGeneral12101(){
				var objectID03 = $('#objectID04').val();
				var modelID04 = $('#modelID04').val();
				var fangshi='普通';
				var bussinesId=parent.findBussID(modelID04);
				parent.objectLocation(modelID04,fangshi);  //定位
				var arrayTitle = ['设备名称','运行编号','生产厂家'];
				var arrayContent = [ '接地刀闸', '1027535','陕西'];
		 		parent.generalPanel(bussinesId, objectID03,'113','29','1000', modelID04, arrayTitle,arrayContent,'50.f','10.f','浅浅粉色');
		 		//locationWithCreate(objectID03,'普通');
				var paramer = "js参数说明：\r\n"+"businessID 企业ID\r\n"+"objectID 创建的标牌的ID\r\n"+"lon 经度\r\n"+"lat 纬度\r\n"+"alt 高度\r\n"+"modelID 所属设备ID\r\n"+"arrayTitle 标牌列名，例如：['设备名称','运行编号','生产厂家']\r\n"+"arrayContent 标牌内容，例如：[ '接地刀闸', '1027535','陕西']\r\n"+"offsetX 标牌与坐标系角度\r\n"+"offsetY 标牌与坐标系角度\r\n"+"styleName 标牌样式";
				parent.getMethodInfo('sceneIJas.js','generalPanel(businessID,objectID,lon,lat,alt,modelID,arrayTitle,arrayContent,offsetX,offsetY,styleName)','无','创建可视化对象标牌',paramer);
}
/**
*	球范围查询:
*/
function selectBallDate01(){
				var lon01 = $('#lon01').val();
				var lat01 = $('#lat01').val();
				var height01 = $('#height01').val();
				var rde01 = $('#rde01').val();
				var userDate01 = $('#userDate01').val();
				var cameraID01 = $('#cameraID01').val();
				var objectType01 = $('#objectType01').val();

					var length01 = $('#length01').val();
				var width01 = $('#width01').val();
				var height02 = $('#height02').val();
				var objectType01 = $('#objectType01').val();
				var lonlat = $('#lonlat').val();

				var type01 = $('#type01').combobox('getValue');
				var type = $('#type').combobox('getValue');
			//	alert(objectID03);
		 		var to = parent.selectBall(type,lon01,lat01,height01,rde01,type01,userDate01,cameraID01,objectType01,length01,width01,height02,lonlat);
				var paramer = "js参数说明：\r\n"+"type 查询方式，当前支持的查询方式，为下面的任意一种类型。可用参数:圆形、圆球、圆柱、多边形、矩形、立方体\r\n"+"lon01 经度\r\n"+"lat01 纬度\r\n"+"height01 高度\r\n"+"rde01 半径,球形相关的查询半径\r\n"+"type01 查询类型，相交查询方式；包含查询方式。可用参数：相交、包含\r\n"+"userDate01 用户数据，用户用来确定本次查询的数据，查询过程异步进行，需要标识设定每次查询的标识，用户来确定结果和操作的对应关系\r\n"+"cameraID01 企业ID\r\n"+"objectType01:显示对象类型，此参数可以是平台支持的任意显示对象类型，在对象管理器中可以找到名字，如果不给此参数数或是“全部”，则查询所有的显示对象\r\n"+"length01 长度\r\n"+"width01 宽度\r\n"+"height02 高度\r\n"+"lonlat 经纬度点，点的坐标(多边形各个顶点)";
				parent.getMethodInfo('sceneIJas.js','selectBall(type,lon01,lat01,height01,rde01,type01,userDate01,cameraID01,objectType01,length01,width01,height02,lonlat)',to,'根据企业ID，或对象ＩＤ，或类型，列出所相关的所有对象信息',paramer);
}
/**
*	模型拆分:
*/
function modelchai01(){
				var modelpath0101 = $('#modelpath0101').val();
			//	alert(objectID03);
		 		parent.modelSplit(modelpath0101);
				var paramer = "js参数说明：\r\n"+"modelpath01  所要拆分的标牌id";
				parent.getMethodInfo('sceneIJas.js','modelSplit(modelpath01)','无','按原件拆分模型',paramer);
}
/**
*	模型刨切:
*/
function makeGeneral01(){
				var modelpath0202 = $('#modelpath0202').val();
			//	alert(objectID03);
		 		parent.modelSliced(modelpath0202);
				var paramer = "js参数说明：\r\n"+"modelpath02  所要剖切的标牌id";
				parent.getMethodInfo('sceneIJas.js','modelSliced(modelpath02)','无','通过用断面的形式剖切设备，一查看内部结构',paramer);
}
/**
*	飞行:
*/
function fly01(){
	var compId = $('#compId').val();
	var modelId = $('#modelId').val();
	var oprateType = $('#oprateType').val();
	var width = $('#width').val();
	var distence = $('#distence').val();
	var colorR = $('#colorR').val();
	var colorA = $('#colorA').val();
	var flyPoint1 = new FlyPoint("111.74","40.34","300","-56.34","-111.24","0.000","20");
	var flyPoint2 = new FlyPoint("112.74","41.34","300","-52.34","-131.24","0.000","20");
	var flyPoint = [flyPoint1,flyPoint2];
	var music1 = new Music("","10","20");
	var music2 = new Music("","15","20");
	var music = [music1,music2];
	parent.flyControl(modelId,compId,oprateType,width,distence,colorR,colorA,flyPoint,music);
	var paramer = "js参数说明：\r\n"+"modelId 对象ID\r\n"+"compId 企业ID\r\n"+"oprateType 绘制飞行路径，绘制,开始、暂停、继续、结束\r\n"+"width 宽度\r\n"+"distence 距离\r\n"+"colorR 颜色，颜色为16进制颜色。例如：#00FFFF\r\n"+"colorA 透明度，范围为0到100\r\n"+"flyPoint 飞行点对象数组。构造方法FlyPoint(X,Y,Z,yawA,rollA,picthA,timeF)，X,Y,Z：为飞行点的位置yawA,rollA,picthA：飞行姿态（偏航角，俯仰角，滚转角）timeF：单段飞行时间（秒）\r\n"+"music 音乐对象构造方法Music(file,begin,timeA)，file：音乐文件路径begin：开始播放时间timeA：总时长";
	parent.getMethodInfo('scriptIJas.js','flyControl(modelId,compId,oprateType,width,distence,colorR,colorA,flyPoint,music)','无','飞行',paramer);
	
}


/*
 * 创建临时对象后定位，为的是更快速明显地看到效果
 */

function locationWithCreate(id,locatType){
	parent.objectLocation(id,locatType);
}

/*
 * 状态栏
 */
function settingHintbar01(){
	var hintbarFontSize = $('#hintbarFontSize').val();
	var hintbarFontColor = $('#hintbarFontColor').val();
	var hintbarFontA = $('#hintbarFontA').val();
	var hintbarBackColor = $('#hintbarBackColor').val();
	var hintbarBackA = $('#hintbarBackA').val();
	var wwwHintbar = $('#wwwHintbar').combobox('getValue');
	parent.setHintbar(hintbarFontSize,hintbarFontColor,hintbarFontA,hintbarBackColor,hintbarBackA,wwwHintbar);
}

/*
 * 工具栏
 */
function settingTool01(){
	var show = $('#toolShow').combobox('getValue');
	var toolspaceDis = $('#toolspaceDis').val();
	var toolHight = $('#toolHight').val();
	var toolFontSize = $('#toolFontSize').val();
	var toolFontColor = $('#toolFontColor').val();
	var toolFrameColor = $('#toolFrameColor').val();
	var wwwTool = $('#wwwTool').combobox('getValue');
	parent.setTool(show,toolspaceDis,toolHight,toolFontSize,toolFontColor,toolFrameColor,wwwTool);
}

/*
 * 控制面板
 */
function settingControlpanel01(){
	var ControlpanelScale = $('#ControlpanelScale').val();
	var ControlpanelTopMargin = $('#ControlpanelTopMargin').val();
	var ControlpanelRightMargin = $('#ControlpanelRightMargin').val();
	var wwwControlpanel = $('#wwwControlpanel').combobox('getValue');
	parent.setControlpanel(ControlpanelScale,ControlpanelTopMargin,ControlpanelRightMargin,wwwControlpanel);
}

/*
 * 焦点十字
 */
function settingFocuscross01(){
	var FocuscrossWinth = $('#FocuscrossWinth').val();
	var FocuscrossHight = $('#FocuscrossHight').val();
	var FocuscrossFontColor = $('#FocuscrossFontColor').val();
	var wwwFocuscross = $('#wwwFocuscross').combobox('getValue');
	parent.setFocuscross(FocuscrossWinth,FocuscrossHight,FocuscrossFontColor,wwwFocuscross);
}

/**
 * 模型移动
 */
function objectMove01(){
	var ID = $('#moveID').val();
	var firstLon = $('#firstLon').val();
	var firstLat = $('#firstLat').val();
	var firstAlt = $('#firstAlt').val();
	var tine1 = $('#time1').val();
	var secondLon = $('#secondLon').val();
	var secondLat = $('#secondLat').val();
	var secondAlt = $('#secondAlt').val();
	var time2 = $('#time2').val();
	parent.objectMove(ID,firstLon,firstLat,firstAlt,tine1,secondLon,secondLat,secondAlt,time2);
}

/**
 * 设置局部经纬线
 */
function setLonAndLatLine01(){
	var maxLat = $('#maxLat').val();
	var maxLon = $('#maxLon').val();
	var minLat = $('#minLat').val();
	var minLon = $('#minLon').val();
	var landType = $('#landType').combobox('getValue');
	var hideType = $('#hideType').combobox('getValue');
	parent.settingLonAndLatLine(maxLat,maxLon,minLat,minLon,landType,hideType);
}
/**
 * 
 * 地球变灰
 */
function EarthGray01(){
	var grayOprate = $('#GrayType').combobox("getValue");;
	parent.settingEarthGray(grayOprate);
}
/**
 * 经纬网
 *     
 */
function LonAndLatLine01(){
	var yanR = $('#yanR').val();
	var yanG = $('#yanG').val();
	var yanB = $('#yanB').val();
	var yanA = $('#yanA').val();
	var ziR = $('#ziR').val();
	var ziG = $('#ziG').val();
	var ziB = $('#ziB').val();
	var ziA = $('#ziA').val();
	var fonte = $('#fonte').val();
	var landjingweiType = $('#landjingweiType').combobox("getValue");
	parent.settingLonAndLatLine(yanR,yanG,yanB,yanA,ziR,ziG,ziB,ziA,fonte,landjingweiType);
	
}

/**
 * （新）设备剖切
 */
function newModelpao01(){
	var paoID = $('#paoID').val();
	var paoName = $('#paoName').val();
	var paoType = $('#paoType').combobox('getValue');
	var paoSet = $('#paoSet').combobox('getValue');
	var paoXYZ = $('#paoXYZ').combobox('getValue');
	var paoOffSet = $('#paoOffSet').val();
	var paoRoll = $('#paoRoll').combobox('getValue');
	parent.objectCutting(paoID,paoName,paoType,paoSet,paoXYZ,paoOffSet,paoRoll);
}
/**
 * 获取矢量图层
 */
function getVectorDemo(){
	var t=parent.getVector();
	var para="无";
	parent.getMethodInfo('vectorIJas.js', ' getVector()', t, '获取矢量图层', para);
}
/**
 * 判断矢量图层是否存在
 */
function isExist01(){
	var  name= $('#name').val();
	var t=parent.isExist(name);
	var para="js参数说明：\r\n"
		+ "name为图层名称";
	parent.getMethodInfo('vectorIJas.js', 'isExist()', t, '判断图层是否存在', para);
}
/**
 * 获取矢量图层的显示状态
 */
function getState01(){
	var  name= $('#name1').val();
	var t=parent.isExist(name);
	var para="js参数说明：\r\n"
		+ "name为图层名称";
	parent.getMethodInfo('vectorIJas.js', 'getState()', t, '获取矢量图层的显示状态', para);
}

/**
 * 设置矢量图层的显示状态
 */
function setState01(){
	var  name= $('#name2').val();
	var state=$('#state').combobox('getValue');
	var t=parent.setState(name,state);
	var para="js参数说明：\r\n"
		+ "图层名称name,显示状态为1表示显示，为0时不显示";
	parent.getMethodInfo('vectorIJas.js', 'setState()', t, '获取矢量图层的显示状态', para);
}
/**
 *点类型图查文
 */
function searcho01(){
  var name=$('#oname').val();
  var property=$('#oproperty').val();
  var x=$('#ox').val();
  var y=$('#oy').val();
  var t=parent.search1(name,'point',property,x,y);
  var para="js参数说明：\r\n"
		+ "name为图层名称\r\n"+"type为查询类型\r\n"+
		"property为查询属性\r\n"+"x,y为查询的坐标串，表示经纬度,radius为半径";
  parent.getMethodInfo('vectorIJas.js', 'search1(name,type,property,x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,radius)', t, '点类型图查文', para);
}
/**
 * 矩形类型图查文
 */
function searchr01(){
  var name=$('#rname').val();
  var property=$('#rproperty').val();
  var x1=$('#rx1').val();
  var y1=$('#ry1').val();
  var x2=$('#rx2').val();
  var y2=$('#ry2').val();
  var t=parent.search1(name,'rectangle',property,x1,y1,x2,y2);
  var para="js参数说明：\r\n"
		+ "name为图层名称\r\n"+"type为查询类型\r\n"+
		"property为查询属性\r\n"+"x,y为查询的坐标串，表示经纬度,radius为半径";
  parent.getMethodInfo('vectorIJas.js', 'search1(name,type,property,x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,radius)', t, '矩形类型图查文', para);		
}
/**
 * 多边形类型图查文
 */
function searchp01(){
  var name=$('#pname').val();
  var property=$('#pproperty').val();
  var x1=$('#px1').val();
  var y1=$('#py1').val();
  var x2=$('#px2').val();
  var y2=$('#py2').val();
  var x3=$('#px3').val();
  var y3=$('#py3').val();
  var x4=$('#px4').val();
  var y4=$('#py4').val();
  var x5=$('#px5').val();
  var y5=$('#py5').val();
  var t=parent.search1(name,'polygon',property,x1,y1,x2,y2,x3,y3,x4,y4,x5,y5);
  var para="js参数说明：\r\n"
		+ "name为图层名称\r\n"+"type为查询类型\r\n"+
		"property为查询属性\r\n"+"x,y为查询的坐标串，表示经纬度,radius为半径";
  parent.getMethodInfo('vectorIJas.js', 'search1(name,type,property,x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,radius)', t, '多边形类型图查文', para);		
}

/**
 *圆类型图查文
 */
function searchc01(){
  var name=$('#cname').val();
  var property=$('#cproperty').val();
  var x=$('#cx').val();
  var y=$('#cy').val();
  var radius=$('#radius').val();
  var t=parent.search1(name,'circle',property,x,y,'','','','','','','','',radius);
  var para="js参数说明：\r\n"
		+ "name为图层名称\r\n"+"type为查询类型\r\n"+
		"property为查询属性\r\n"+"x,y为查询的坐标串，表示经纬度,radius为半径";
  parent.getMethodInfo('vectorIJas.js', 'search1(name,type,property,x1,y1,x2,y2,x3,y3,x4,y4,x5,y5,radius)', t, '圆类型图查文', para);
}
/**
 *文查图
 */
function search201(){
	  var name=$('#wname').val();
	  var property=$('#wproperty').val();
	  var statement=$('#statement').val();
	  var t=parent.search2(name,property,statement);
	  var para= "js参数说明：\r\n"
			+ "name为图层名称\r\n"+"statement为查询语句\r\n"+
			"property为查询属性\r\n";
	  parent.getMethodInfo('vectorIJas.js', 'search2(name,property,statement)', t, '文查图', para);
}
/**
 *高亮要素颜色
 */
function setlight01(){
	  var name=$('#lname').val();
	  var ID=$('#lid').val();
	  var color=$('#color').val();
	  var t=parent.setLight(ID,name,color);
	  var para="js参数说明：\r\n"
		   +"ID为id\r\n"
			+ "name为图层名称\r\n"+
			"color为颜色\r\n";
	  parent.getMethodInfo('vectorIJas.js', 'setLight(ID,name,color)', t, '高亮要素颜色', para);
}
/**
 *恢复高亮显示
 */
function recoverlight01(){
	  var name=$('#hname').val();
	  var t=parent.recoverLight(name);
	  var para="js参数说明：\r\n"+ "name为图层名称\r\n";
	  parent.getMethodInfo('vectorIJas.js', 'recoverLight(name)', t, '恢复高亮显示', para);
}
//**=======================图层开始==================================**/
/**
 *登陆
 */
function login01(){
	  var username=$('#username').val();
	  var password= $('#password').val();
	  var t= parent.login(username,password);  
	  var para="js参数说明：\r\n"+ "username为用户名\r\n"+"password为密码\r\n";
	  parent.getMethodInfo('vectoreditIJas.js', 'login(username,password)', t, '登录', para);
}
/**
 * 新增图层
 */
function newLayerDemo(){
	  var name=$('#layername').val();
	  var type= $('#layerType').val();
	  var attr=$('#layerAttr').val();
	  var des= $('#layerDes').val();
	  var t= parent.addLayer(name,type,attr,des);  
	  var para="js参数说明：\r\n" + "name 图层名称，该处不包括命名空间\r\n" + "type 图层类型（Point Line Polygon）\r\n"+
		"attr 属性：long OBJECTID,string 3NAME,（即属性类型 属性名称，属性类型为long double string int，属性名称OBJECTID必须有，属性名称均为大写）\r\n"+
		"des 符号化描述：对应vec.vecfg中的字符串，如果为空，则为程序默认设置。\r\n";
	  parent.getMethodInfo('vectoreditIJas.js', 'addLayer(name,type,attr,des)', t, '新增一个图层', para);
}
/**
 *设置编辑图层
 */
function setLayerDemo(){
	  var name=$('#layername2').val();	
	  var t= parent.setLayer(name);  
	  var para="js参数说明：\r\n"+ "name为图层名称\r\n";
	  parent.getMethodInfo('vectoreditIJas.js', 'setLayer(name)', t, '设置编辑图层', para)	
}
/**
 *删除辑图层
 */
function delLayerDemo(){
	  var name=$('#dleLayername').val();	
	  var t= parent.delLayer(name);  
	  var para="js参数说明：\r\n"+ "ame为图层名称，要带上命名空间\r\n";
	  parent.getMethodInfo('vectoreditIJas.js', 'delLayer(name)', t, '删除图层', para)	
}
/**
 * 设置图层显示顺序
 */
function setLayerSequenceDemo(){
	 var name=$('#sequenceName').val();	
	 var index=$('#sequenceIndex').val();	
	 var t= parent.setLayerSeq(name,index);  
	 var para="js参数说明：\r\n" + "name为图层名称\r\n"+
		"index为序号，序号：0 - maxLayerNumber，maxLayerNumber为当前加载的图层最大数，设置的序号不应超过该参数";
	 parent.getMethodInfo('vectoreditIJas.js', 'setLayerSeq(name,index)', t, '设置图层显示顺序', para)	
}
/**
 * 选择编辑图层
 */
function selectLayerDemo(){
	var name = $('#selectLayerTable').val();
	var t= parent.setLayer(name);  
	var para="js参数说明：\r\n"+ "name为图层名称\r\n";
	parent.getMethodInfo('vectoreditIJas.js', '1. getVector()；2. setLayer(name)', t, '选择编辑图层', para)	
	
}
/**
 * 图层鼠标双击监听事件
 */
function layerDoubleClickListen(){
	var t= parent.layerDoubleClick(layerDoubleClickCall);  
	var para="js参数说明：\r\n"+ "消息值为0X00000001\r\n";
	parent.getMethodInfo('rebackIJas.js', 'layerDoubleClick()', t, '图层鼠标双击监听事件', para)	
}
/**
 * 鼠标双击监听事件回调函数
 */
function layerDoubleClickCall(backValue){
	//解析数据，获取参数3
	var doclist = backValue.split(' ');
	var elementId;
	for(var i=0;i<doclist.length;i++){
		if(doclist[i].indexOf('参数1')>-1){
			elementId = doclist[i].split('\'')[1];
		}
	}
	//选择要素
	parent.selectElement(elementId);
	var layerName = $('#selectLayerTable').val();
	var url = "../html/newElementAttr.html?elementId="+elementId+"&layerName="+layerName;
	top.getDlg(url, "编辑新建要素属性", "编辑新建要素属性", 480, 300);
	
}
/**
 * 图层中Delete键删除要素监听事件
 */
function layerDeleteKeyListenDemo(){
	var id = $('#layerDeleteElementId').val();
	parent.selectElement(id);
	var t= parent.layerDeleteKey(layerDeleteKeyCall);  
	var para="js参数说明：\r\n"+ "消息值为0X00000004\r\n";
	parent.getMethodInfo('rebackIJas.js', 'layerDeleteKey()', t, '图层中Delete键删除要素监听事件', para)	
}
/**
 * Delete键删除要素监听事件回调函数
 */
function layerDeleteKeyCall(backValue){
	//解析数据，获取参数3
	var doclist = backValue.split(' ');
	var elementId;
	for(var i=0;i<doclist.length;i++){
		if(doclist[i].indexOf('参数1')>-1){
			elementId = doclist[i].split('\'')[1];
		}
	}
	//删除要素，提交服务器
	parent.deleteElement(elementId,'是');
}
//**==========================要素开始=====================**//
/**
 * 要素提交监听
 */
function listenSubmit(){
	parent.recallListenElementSubmit(elementSubmitListen);
	var para="js参数说明：\r\n"+ "elementSubmitListen:回调函数\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'recallListenElementSubmit(id)', t, '要素提交监听', para);
}
/**
 * 监听是否提交要素，回调处理函数
 */
function elementSubmitListen(backValue){
	if(backValue.indexOf("是")>-1){
		alert("存在更新的要素");
		parent.submitServer('是');
	}
	
}
/**
 *选择要素
 */
function selecteElementDemo(){
	  var id=$('#elementObjectId').val();	
	  var t= parent.selectElement(id);  
	  var para="js参数说明：\r\n"+ "ID:要素OBJECTID\r\n";
	  parent.getMethodInfo('vectoreditIJas.js', 'selectElement(id)', t, '选中要素', para);
}
/**
 * 新建矢量要素
 */
function newElementDemo(){
	var coor = $('#elementCoordinate').val();
	var attr = $('#elementAttribute').val();
	var t= parent.addElement(coor,attr);  
	var para = "js参数说明：\r\n" + "coor为坐标：经度,纬度 经度,纬度id\r\n"+
				"属性：属性名 属性值,属性名 属性值(此处无需OBJECTID，该属性字段为默认的，属性名称需带命名空间)\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'addElement(coor,attr)',t, '新建一个矢量要素', para);
}
/**
 * 新建矢量要素，编辑坐标
 */
function newElementCoordinate(){
	var url = "../html/addElement.html";
	top.getDlg(url, "编辑新建要素坐标", "编辑新建要素坐标", 480, 300);
}
/**
 * 新建矢量要素，编辑属性
 */
function newElementAttribute(){
	var url = "../html/addElementAttr.html";
	top.getDlg(url, "编辑新建要素属性", "编辑新建要素属性", 480, 300);
}
/**
 * 设置编辑状态
 */
function setElementState(){
	var t = parent.setEditState();  
	var para = "无";
	parent.getMethodInfo('vectoreditIJas.js', 'setEditState()',t, '设置编辑状态', para);
}
/**
 * 获取选中要素坐标
 */
function selcetElementNote(){
	var t = parent.elementCoordinate();
	var para = "无";
	parent.getMethodInfo('vectoreditIJas.js', 'elementCoordinate()',t, '获取选中要素坐标', para);
}
/**
 * 获取选中要素属性
 */
function selectElementAttr(){
	var t = parent.elementProperty();  
	var para = "无";
	parent.getMethodInfo('vectoreditIJas.js', 'elementProperty()',t, '获取选中要素属性', para);
}
/**
 * 编辑指定要素属性
 */
function editorElementAttrDemo(){
	var objectId = $('#editorElementId').val();
	var Attributes = $('#editorElementAttr').val();
	var t = parent.editorElement(objectId,Attributes); 
	var para = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"Attributes为属性：属性名 属性值,属性名 属性值\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'editorElement(objectId,Attributes)', t, ' 编辑指定要素属性',
			para);
}
/**
 * 删除要素
 */
function delElementDemo(){
	var objectId = $('#delElementId').val();
	var yesorno = $('#delElementOrNo').val();
	var t = parent.deleteElement(objectId,yesorno); 
	var para = "js参数说明：\r\n" + "objectId为要素OBJECTID\r\n"+
				"yesorno为是否从服务器中删除：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'deleteElement(objectId,yesorno)', t, ' 删除要素',
			para);
}
/**
 * 完成矢量编辑
 */
function completeElement(){
	var t = parent.finishEdit();  
	var para = "无";
	parent.getMethodInfo('vectoreditIJas.js', 'finishEdit()',t, '完成矢量编辑', para);
}
/**
 * 提交要素属性
 */
function submitElementDemo(){
	var objectId = $('#submitElementId').val();
	var coordinate = $('#submitElementAttr').val();
	var flag = $('#yesOrNo').val();
	var t = parent.submitElementAttribute(objectId,coordinate,flag); 
	var para = "js参数说明：\r\n" +"objectId为要素的OBJECTID\r\n"+
				"coordinate为属性：属性类型 属性名称 属性值,如：int ChinaMap:LENGTH 1111,string ChinaMap:NAME sss,long ChinaMap:OBJECTID 1"+
				"flag为是否提交：是/否";
	parent.getMethodInfo('vectoreditIJas.js', 'submitElementAttribute(objectId,coordinate,flag)', t, ' 提交要素属性',
			para);
}
/**
 * 提交当前编辑至服务器
 */
function sumintElementToServerDemo(){
	var flag = $('#yesOrNoToServer').val();
	var t = parent.submitServer(flag); 
	var para = "js参数说明：\r\n" +"flag为是否提交：是/否\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'submitServer(flag)', t, ' 提交当前编辑至服务器',
			para);
}
/**
 * 精确编辑要素（综合）
 */
function wholeEditElementsDemo(){
	var elementId = $('#wholeEditElementId').val();
	//选择要素
	parent.selectElement(elementId);
	var url = "../html/editorElement.html?elementId="+elementId;
	top.getDlg(url, "精确编辑", "精确编辑", 480, 550);
}
/**
 * 批量编辑要素属性
 */
function batchEditlementAttr(){
	var layerName = $('#selectLayerTable').val();
	var url = "../html/batchEditorElement.html?layerName="+layerName;
	top.getDlg(url, "批量编辑", "批量编辑", 480, 550);
}
/**
 * 编辑要素属性（综合）
 */
function wholeEditlementAttrDemo(){
	var elementId = $('#wholeEditlementAttrId').val();
	//选择要素
	parent.selectElement(elementId);
	var url = "../html/editorElementAttr.html?elementId="+elementId;
	top.getDlg(url, "编辑属性", "编辑属性", 400, 300);
}
/**
 *退出适量编辑
 */
function loginOut(){
	  var t= parent.logout();  
	  alert(t);
	  var para="js参数说明：无参数";
	  parent.getMethodInfo('vectoreditIJas.js', 'logout()', t, ' 退出', '');
 }
//**=======================节点开始=========================**//
/**
 * 精准编辑插入要素节点
 */
function insertElementNoteDemo(){
	var num = $('#insertElementNum').val();
	var index = $('#insertElementIndex').val();
	var lon = $('#insertElementLon').val();
	var lat = $('#insertElementLat').val();
	var t = parent.insertNote(num,index,lon,lat); 
	var para = "js参数说明：\r\n" + "num为所在部分编号\r\n"+"index为节点索引\r\n"+"lon为经度\r\n"+
				"lat为纬度\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'insertNote(num,index,lon,lat)', t, '精准编辑插入要素节点',
			para);
	
}
/**
 * 精准编辑更新节点
 */
function updateElementNoteDemo(){
	var index = $('#updateElementIndex').val();
	var lon = $('#updateElementLon').val();
	var lat = $('#updateElementLat').val();
	var t = parent.updateNote(index,lon,lat); 
	var para = "js参数说明：\r\n" +"index为节点索引\r\n"+"lon为经度\r\n"+
				"lat为纬度\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'updateNote(index,lon,lat)', t, '精准编辑更新节点',
			para);
}
/**
 * 精准编辑增加节点
 */
function addElementNoteDemo(){
	var index = $('#addElementIndex').val();
	var lon = $('#addElementLon').val();
	var lat = $('#addElementLat').val();
	var t = parent.addNote(index,lon,lat); 
	var para = "js参数说明：\r\n" +"index为节点索引\r\n"+"lon为经度\r\n"+
				"lat为纬度\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'addNote(index,lon,lat)', t, '精准编辑增加节点',
			para);
}
/**
 * 精准编辑删除节点
 */
function addElementNoteDemo(){
	var index = $('#delElementIndex').val();
	var t = parent.delNote(index); 
	var para = "js参数说明：\r\n" +"index为节点索引\r\n";
	parent.getMethodInfo('vectoreditIJas.js', 'delNote(index)', t, '精准编辑删除节点',
			para);
}
/**
 * 设置增加首节点状态
 */
function addFirstNoteState(){
	var t = parent.setAddFirstNoteState();
	var para = "无参数";
	parent.getMethodInfo('vectoreditIJas.js', 'setAddFirstNoteState()',t, '设置增加首节点状态', para);
}
/**
 * 设置增加尾节点状态
 */
function addLastNoteState(){
	var t = parent.setAddLastNoteState();
	var para = "无参数";
	parent.getMethodInfo('vectoreditIJas.js', 'setAddLastNoteState()',t, '设置增加尾节点状态', para);
}