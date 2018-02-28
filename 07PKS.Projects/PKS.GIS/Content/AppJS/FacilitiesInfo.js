/**
*
*   加载生产设施设备
*   2014年10月30日
*
*/
//生产产量类型
var YWCJTYPE = 'XT'; //区分弹出窗口内容
var outputState = 0;
//天外天平台ID
var twtC = '3000012100';
var twtA = '3000011000';
var twtCEP = '3000041000';
var CX = '3000012300';
var BJT = '3000020200';
var LiShuiZhongDuanId = "3000031001";
var LongWanZhongDuan = '3000031002';
var DSZhongDuan = '3000030003';
var NHZhongDuan = '3000030002';
//是否显示提示框
var isShowFacilitiesPopup = false;
//设施
var FacilitiesList = new Array();  //平台marker
var FacilitiesDescList = new Array();  //平台名称marker
//防止弹出2次页面
var count = 0;
var flag = null;
function done() {
    if (count == 0) {
        clearInterval(flag);
    }
    else {
        count = count - 1;
    }
}

$(function () {
    try {
    
        var mapClusterMarkersFacilities = L.layerGroup()
            .addTo(map);
    } catch (e) {

    }

    //loadFacilities();

    

});
//加载设施
var EmployeeList = [];
var WellsList = [];
function loadFacilities() {
    //添加点位
    $.ajax({
        url: "AppJS/allfaclities.js",
        data: {},
        type: "get",
        dataType: "json",
        success: function (result) {
            $.each(result, function (index, item) {
                DrawFacility(item);
            });
           
            //判断是否隐藏提示框
            //if (isShowFacilitiesPopup)
            //    $.each(FacilitiesList, function (index, item) {
            //        item.unbindPopup();
            //    });
        },
        error: function(e) { 
        //alert(e); 
    } 
    });
    //地图放大缩小显示不同的设施设备
    map.on('zoomend', onMapLevel);
    function onMapLevel() {
        // alert(map._zoom);
        if (map._zoom < 8) {
            //隐藏平台名称
            $.each(FacilitiesDescList, function (index, item) {

                item.setOpacity(0);  //设置标记的透明度 0为全透明               
            });
        }
        else {
            $.each(FacilitiesDescList, function (index, item) {
                item.setOpacity(1);
            })
        }
        if (map._zoom < 7) { //缩小
            
            //修改平台图标为div圆
            $.each(FacilitiesList, function (index, item) {
                var key = $.trim(item.markerID);
              
                var iconMarker = L.divIcon({ html: '<div></div>', className: 'marker-pt  marker-pt-small', iconSize: new L.Point(10, 10) });//10*10像素大小的div
                item.setIcon(iconMarker); //更改图标
               // item.setOpacity(0.5);
            });
           
            
            $.each(FacilitiesList, function (index, item) {

                var key = $.trim(item.markerID);
                //春晓CEP
                if (key == '32800000000000111') {
                    item.setOpacity(0);  //设置标记的透明度 0为全透明
                }
            });
        } else {
            ////修改回原图标
            //$.each(FacilitiesDescList, function (index, item) {
            //    item.setOpacity(1);
            //})
            //根据zoom级别调整标注大小
            $.each(FacilitiesList, function (index, item) {
                if (item.markerURL) {  //item.IMG 
                    var iconMarker = null;
                    if (map._zoom < 8) {
                        iconMarker = L.icon({
                            iconUrl: item.markerURL,
                            iconSize: [item.markerWidth, item.markerHeight]
                        });
                    }
                    else if (map._zoom < 10) {
                        iconMarker = L.icon({
                            iconUrl: item.markerURL,
                            iconSize: [item.markerWidth * 1.1, item.markerHeight * 1.1]
                        });
                    }
                    else {
                        iconMarker = L.icon({
                            iconUrl: item.markerURL,
                            //iconSize: [item._icon.clientWidth, item._icon.clientHeight]
                            iconSize: [item.markerWidth * 1.2, item.markerHeight * 1.2]
                        });
                    }
                    //修改为原图标
                    item.setIcon(iconMarker);
                }
                var key = $.trim(item.markerID);
            });
        }
    }
}


//绘制平台、终端
function DrawFacility(item) {
    //平台、终端 弹出框内容
    //var cssStrPlat = BuildFacilityPopup(item);
    ////增加故障二级信息
    //cssStrPlat += BuildGZXXPopup(item);

    var cssStrPlat = '<div class="homeScreen" ><div class="mask" ><div class="allScreens" id="allScreens-' + $.trim(item.ProductionUnitID) + '"></div></div>	<ul class="indicators"  id="indicators-' + $.trim(item.ProductionUnitID) + '"></ul></div>';

    //var cssStrPlat = '<div class="homeScreen" style=" width: 300px;    height:100%;    padding: 1px;    margin: 0 auto 1px;" ><div class="mask"  style="width: 300px;    height: 100%;    position: relative;    overflow: hidden;    margin: 1px auto 0;"><div class="allScreens" id="allScreens-' + $.trim(item.ProductionUnitID) + '"></div></div>	<ul class="indicators"  id="indicators-' + $.trim(item.ProductionUnitID) + '"></ul></div>';

    cssStrPlat = '<div id="div_1_' + $.trim(item.ProductionUnitID) + '">' + cssStrPlat + '</div>';
    //cssStrPlat += '<div id="div_2_' + $.trim(item.ProductionUnitID) + '" style="display: none;">'
    //    //返回
    //    //+ '<div id="div_return_' + $.trim(item.ProductionUnitID) 
    //    //+ '" onclick="GZXXReturn('+$.trim(item.ProductionUnitID+')"><a>返回</a></div>'
    //    + '<div style="float:right" id="div_return_' + $.trim(item.ProductionUnitID) + '" onclick="GZXXReturn(' + $.trim(item.ProductionUnitID) + ')"><image src="../images/icon/close.png"  width="20px" height="20px"></image></div>'
    //+ '<div class="homeScreen"><div class="mask"><div class="allScreens" id="allScreens-' + $.trim(item.ProductionUnitID) + '_2"></div></div>	<ul class="indicators"  id="indicators-' + $.trim(item.ProductionUnitID) + '_2"></ul></div>' + '</div>';
    ////+ '<div class="homeScreen" ><div class="mask" ><div class="allScreens" id="allScreens-' + $.trim(item.ProductionUnitID) + '_2"></div></div>	<ul class="indicators"  id="indicators-' + $.trim(item.ProductionUnitID) + '_2"></ul></div>' + '</div>';
    //查询人数
    var member = 0;
    var strEmployees = '';
   
    //查询井口数量
    var wells = 0;
   
    //marker高度宽度
    var markerWidth = 44,
        markerHeight = 44;
    var strWells = ''

    if (item.Type == '平台') {
        markerWidth = 40;
        markerHeight = 40;

        if ($.trim(item.ProductionUnitID) == twtCEP) {
            markerWidth = 59.6; //119 * 0.5;
            markerHeight = 43; //86 * 0.5;
        }
        else if ($.trim(item.ProductionUnitID) == twtA) {
            markerWidth = 24; // 48 * 0.5;
            markerHeight = 37; //74 * 0.5;
        }

        strWells = '井口数量:<a class="fontStyle" onclick=javascript:ShowNewWindow(\"../Wells/WellsList.html?key=' + $.trim(item.ProductionUnitID) + '\")>' + wells + '</a>';
    }
    else if (item.Type == '陆上终端') {
        markerWidth = 65;
        markerHeight = 45;

        if ($.trim(item.ProductionUnitID) == LongWanZhongDuan || $.trim(item.ProductionUnitID) == LiShuiZhongDuanId) {
            markerWidth = 65 * 0.8;
            markerHeight = 45 * 0.8;
        }

    }
        //2015-7-7 增加码头类型
    else if (item.Type == '码头' || item.Type == '机场') {
        markerWidth = 24;
        markerHeight = 24;
    }
    //2015-7-7
    //var title = item.ProductionUnitFullName + '</br>投产年份:2005年</br>设计寿命:30年';
    var title = item.ProductionUnitFullName; //名称
    //画图元的参数
    var paraMarker = {
        id: item.ProductionUnitID,
        imgURL: item.IMG,
        width: markerWidth, height: markerHeight,
        lat: item.LAT, lng: item.LNG, key: item.ProductionUnitFullName,
        title: title, contentWidth: 332, contentHeight: 130,
        isShowPopup: true,
        //content: '<div style="width:100%;font-weight:600;background:#E1E5EE;height:25px;">' + strEmployees + '&nbsp;&nbsp;&nbsp;&nbsp;' + strWells + '</br></div>' + cssStrPlat
        //content: '<div style="width:100%;font-weight:600;height:25px;">' + strEmployees + '&nbsp;&nbsp;&nbsp;&nbsp;' + strWells + '</br></div>' + cssStrPlat
        content: cssStrPlat
    };
    //龙湾终端禁止弹出信息框
    if (item.ProductionUnitID == LongWanZhongDuan || item.ProductionUnitID == DSZhongDuan || item.ProductionUnitID == NHZhongDuan) {
        paraMarker.isShowPopup = false;
    }
    //2015-7-7 增加码头类型
    //if (item.Type == '码头' || item.Type == '机场') {
    //    paraMarker.isShowPopup = false;
    //}
    if (item.Type == '机场') {
        paraMarker.isShowPopup = false;
    }

    var marker = AddFacilitiesMarker(paraMarker);

    marker.markerID = item.ProductionUnitID;
    marker.markerURL = item.IMG;
    marker.markerTip = item.ProductionUnitFullName;
    marker.markerWidth = markerWidth;
    marker.markerHeight = markerHeight;
    marker.ProductionUnitType = item.Type;
    marker.on('popupopen', function (e) {  //弹出窗口打开
        var markerEvent = e.target;
        try {
            //消息推送
            //PushMessage("PINGTAICHANGE", "");
            messageSignal.sendMessage("PINGTAICHANGE", markerEvent.markerID);
        } catch (e) {

        }
        //2015-04-23 
        //构造popup内容
        BuildMarkerPopup(markerEvent.markerID);

        //BuildMarkerPopup_GZ(markerEvent.markerID);

        //地图中心坐标移动
        if (map) {
            map.panTo([item.LAT, item.LNG]);
        }
    });
    FacilitiesList.push(marker);

    //添加平台名称
    var labelMarker = L.divIcon({
        iconAnchor: [-(markerWidth / 2 + 6), 0], //右下
        className: 'icon-marker-location-label',
        //html: '<lable style=\'color:#ffffff;width:200px;\'>' + item.ProductionUnitFullName + '</lable>'
        html: '<p  class=\'map-label\'>' + item.ProductionUnitFullName + '</p>' //平台名称
    });
    //调整平台名称显示位置
    if ($.trim(marker.markerTip) == "JZ21-1WHPA") {
        labelMarker = L.divIcon({
            iconAnchor: [(markerWidth / 2 + 80), 35], //左上
            className: 'icon-marker-location-label',
            //html: '<lable style=\'color:#ffffff;width:200px;\'>' + item.ProductionUnitFullName + '</lable>'
            html: '<p   class=\'map-label\'>' + item.ProductionUnitFullName + '</p>'
        });
    }
    if ($.trim(marker.markerTip).indexOf("CEP") != -1)   //平台名称中含有CEP
    {
        labelMarker = L.divIcon({
            iconAnchor: [(markerWidth / 2 + 70), 35], //左上
            className: 'icon-marker-location-label',
            //html: '<lable style=\'color:#ffffff;width:200px;\'>' + item.ProductionUnitFullName + '</lable>'
            html: '<p   class=\'map-label\'>' + item.ProductionUnitFullName + '</p>'
        });
    }



    //平台终端名称
    var markerDesc = L.marker([item.LAT, item.LNG], { icon: labelMarker })
         .addTo(map);
    //点击平台名称时弹出窗口
    markerDesc.markerID = item.ProductionUnitID;//平台id
    markerDesc.markerFacility = marker;//关联的平台图元
    markerDesc.on('click', function (e) {
        var markerEvent = e.target;
        try {
            if (markerEvent) {
                if (markerEvent.markerFacility) {
                    markerEvent.markerFacility.openPopup();
                }
            }
        } catch (e) {

        }


    });
    FacilitiesDescList.push(markerDesc);
}

//弹出窗口分类
function BuildMarkerPopup(ProductionUnitID) {
    var allIcons, allScreens, dock, dockIcons, icon, stage, _i, _len, _results;

    var iconCCTV = new Icon('CCTV' + ProductionUnitID, '视频动态', '../images/icon/CCTV_i4.png', '');
    var iconGYLC = new Icon('GYLC' + ProductionUnitID, '工艺流程', '../images/icon/GYLC_i1.png', '');
    var iconDCS = new Icon('DCS' + ProductionUnitID, '远程DCS', '../images/icon/DCS_i3.png', '');
    var iconGTJL = new Icon('GTJL' + ProductionUnitID, '关停记录', '../images/icon/GTJL_i.png', '');
    var iconSCRB = new Icon('SCRB' + ProductionUnitID, '工作日报', '../images/icon/SCRB.png', '');
    var iconCLXX = new Icon('CLXX' + ProductionUnitID, '产量信息', '../images/icon/CLXX_i1.png', '');
    var iconCLXXYC = new Icon('CLXXYC' + ProductionUnitID, '产量信息', '../images/icon/CLXX_i1.png', '');
    var iconSCJXX = new Icon('SCJXX' + ProductionUnitID, '生产井信息', '../images/icon/ZJXX.png', '');
    var iconSCJW = new Icon('SCJW' + ProductionUnitID, '生产井状态', '../images/icon/ZJXX.png', '');
    var iconWXRB = new Icon('WXRB' + ProductionUnitID, '维修日报', '../images/icon/WXRB_i.png', '');
    var iconWXGD = new Icon('WXGD' + ProductionUnitID, '维修工单', '../images/icon/WXGD_i.png', '');
    var iconSBXX = new Icon('SBXX' + ProductionUnitID, '设备信息', '../images/icon/SBXX_i.png', '');
    var iconGJSB = new Icon('GJSB' + ProductionUnitID, '关键设备', '../images/icon/GJSB_i1.png', '');
    var iconBJXX = new Icon('BJXX' + ProductionUnitID, '物资信息', '../images/icon/SBBJ_i.png', '');
    var iconPTXX = new Icon('PTXX' + ProductionUnitID, '平台/终端信息', '../images/icon/PTXX_i2.png', '');
    var iconEDIS = new Icon('EDIS' + ProductionUnitID, 'EDIS', '../images/icon/EDIS.png', '');
    var iconRYXX = new Icon('RYXX' + ProductionUnitID, '人员信息', '../images/icon/gongzuoribao.png', '');
    var iconYJGL = new Icon('YJGL' + ProductionUnitID, '应急管理', '../images/icon/SBBJ_i.png', '');
    var iconYSKC = new Icon('YSKC' + ProductionUnitID, '油水库存', '../images/icon/WXGD_i.png', '');
    var iconFIELDXX = new Icon('FIELDXX' + ProductionUnitID, '油气田基本信息', '../images/icon/PTXX_i2.png', '');
    var iconZLLQ = new Icon('ZLLQ' + ProductionUnitID, '动态监测', '../images/icon/GYLC_i1.png', '');
    if (YWCJTYPE == null) {
        allIcons = [iconCCTV, iconGYLC, iconDCS, iconGTJL, iconSCRB, iconCLXX, iconSCJXX
            , iconWXRB, iconWXGD, iconSBXX, iconGJSB, iconBJXX, iconPTXX, iconEDIS];
    }
    else if (YWCJTYPE == 'SC') {
        allIcons = [iconCCTV, iconGYLC, iconDCS,  iconCLXX, iconSCJXX
            , iconPTXX];
    }
    else if (YWCJTYPE == 'XT') { //默认
        allIcons = [iconCCTV, iconSCRB, iconYJGL, iconRYXX, iconYSKC, iconPTXX];
    }
    else if (YWCJTYPE == 'YC') {
        allIcons = [iconFIELDXX, iconSCJW, iconCLXXYC, iconSCRB, iconZLLQ, iconGTJL];
    }
    else if (YWCJTYPE == 'HG') {
        allIcons = [iconGTJL, iconSCRB, iconCLXX
           , iconWXRB, iconWXGD, iconSBXX, iconBJXX, iconPTXX];
    }
    

    allScreens = $('#allScreens-' + ProductionUnitID);
    allScreens.Touchable();
    stage = new Stage(allIcons);
    stage.addScreensTo(allScreens);
    stage.addIndicatorsTo('#indicators-' + ProductionUnitID);
    allScreens.bind('touchablemove', function (e, touch) {
        if (touch.currentDelta.x < -5) {
            stage.next();
        }
        if (touch.currentDelta.x > 5) {
            return stage.previous();
        }
    });

 
}


//添加点位信息
//para：画图元的参数：imgURL 图标地址 width 图标宽 height 图标高 lat 纬度 lng 经度 key 主键 title 标题 content 内容
function AddFacilitiesMarker(para) {
    //配置Icon
    var iconCheckMarker = L.icon({
        iconUrl: para.imgURL,
        iconSize: [para.width, para.height]
     
    });
    //配置Marker
    var marker = L.marker([para.lat, para.lng], { icon: iconCheckMarker, riseOnHover: true }); //是否凸显
  

    if (YWCJTYPE == "HG") {
    
    }
    else {
        if (para.isShowPopup == true) {
            marker.bindPopup(
                 getFacilitiesMarkerHtmlContent({  //return props.content;
                     key: para.key,
                     content: para.content,  //content: cssStrPlat
                     width: para.contentWidth,
                     height: para.contentHeight
                 })
                 );
        }
  
        /*marker.bindLabel(para.title)   //ProductionUnitFullName   Leaflet.label
        
             .on('click', function (e) {
                 var markerEvent = e.target;  //触发该事件的DOM元素
         

             })
             .openPopup() //打开弹出窗口
        .addTo(map);*/
    }
   
    marker.addTo(map);

    //label
    return marker;
   
}
//返回Popup显示内容
function getFacilitiesMarkerHtmlContent(props) {
   

    return props.content;
}




