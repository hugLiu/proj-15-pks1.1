/**
*
*   加载管线
*   2014年10月30日
*
*/

//天然气管线颜色-黄色
var TRQ_Color = '#fafc0d';
//混输管线颜色-红色
var HS_Color = 'red';
//原油管线颜色-黑色
var YY_Color = '#000000';

//是否显示滚动
var isShowLineMarquee = true;
//是否显示提示框
var isShowLinePopup = false;

//海管标注
var arrPipelineLabels = [];

$(function () {

    //加载海管管线
    //loadLine();
    //添加管线图例
    Legend(); 
    //if (isShowLineMarquee)
    //    LoadLineMarquee();
    try {
        //map.on('zoomend', function (e) {
        //    //alert(JSON.stringify(e));
        //    var objE = e;

        //    RefreshPipelineLabels();
        //});
    } catch (e) {

    }
});
//加载管线
var pipelines = [];
var pipelineLatLongs = [];
//取数据库数据
function loadLine() {
    //获取所有海管
    $.ajax({
        //获取海管基本信息
        url: "AppJS/DataPipelineInfo.js",
        data: {},
        type: "get",
        dataType: "json",
        success: function (result) {
            //success:请求之后调用,传入返回后的数据，以及包含成功代码的字符串
            //var sth = JSON.stringify(result);
            //alert(sth);
            pipelines = result;
            //获取海管坐标
            $.ajax({
                url: "AppJS/DataPipelineInfolatlon.js",
                data: {},
                type: "get",
                dataType: "json",
                success: function (result2) {
                    pipelineLatLongs = result2;
                    DrawPipline();
                }
            });
        }
    });
}

//
function DrawPipline() {
    //如果海管基本信息或海管坐标为空则返回
    if (!pipelines || !pipelineLatLongs) {
        return;
    }
    if (pipelines.length < 1) {
        return;
    }
    try {
        arrPipelineLabels = [];
        $.each(pipelines, function (index, item) {

            //手动添加 长期航告编号
            var hangGaoBianHao = "";
            if (item.SASSETID == "31400720081118141957170362" || item.SASSETID == "31400720091125174516590226") {
                hangGaoBianHao = "沪监航（1998）228号";
            }
            else if (item.SASSETID == "310000000000008896" || item.SASSETID == "3100000000000088666214" || item.SASSETID == "31400720081118143806013849" || item.SASSETID == "31400000000000119" || item.SASSETID == "31400720081118142307060007") {
                hangGaoBianHao = "浙海航（2006）04号";
            }
            else if (item.SASSETID == "3100000000000088144" || item.SASSETID == "3100000000000088666167") {
                hangGaoBianHao = "浙海航（2015）1号";
            }
            else if (item.SASSETID == "3100000000000088143") {
                hangGaoBianHao = "浙海航（2015）9号";
            }

            try {
                // 弹出服务页面
                var cssStr = '<div class="row map-facility-card-row">' +
              '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
                  '<a href=javascript:showPipLiner(\"' + $.trim(item.SASSETID) + '\"); class="thumbnail">' +
                      '<img src="../Images/Icon/JCXX.png" data-src="holder.js/100%x180" alt="...">' +
                         '<div class="fontStyle">基本信息</div>' +
                  '</a>' +

              '</div>' +
              '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
                   '<a href=javascript:ShowNewWindow(\"../HG/HGRouteView.html?key=' + $.trim(item.SASSETID) + '\",800,600); class="thumbnail">' +
                      '<img src="../Images/Icon/HGLY.png" data-src="holder.js/100%x180" alt="...">' +
                        '<div class="fontStyle">海管路由</div>' +
                   '</a>' +

              '</div>' +
              //'<div class="col-xs-8 col-md-3 map-facility-card-item">' +
              //    '<a href=javascript:showLine(\"../HG/BASE/HGHYGLView.html\",800,600); class="thumbnail">' +
              //        '<img src="../Images/Icon/HYGL.png" data-src="holder.js/100%x180" alt="...">' +
              //         '<div class="fontStyle">管线海域</div>' +
              //    '</a>' +

              //'</div>' +

          //'</div>' +
          //'<div class="row">' +
          '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
              //'<a href=javascript:showLine(\"../HG/PipelineMaintainAnalysis.html?key=' + $.trim(item.JSON_PipelineId) + '\",800,600); class="thumbnail">' +
              '<a href=javascript:ShowNewWindow(\"../HG/PipePigList.html?key=' + $.trim(item.SASSETID) + '\",800,600); class="thumbnail">' +
                  '<img src="../Images/Icon/HGTQ.png" data-src="holder.js/100%x180" alt="...">' +
                   '<div class="fontStyle">通球记录</div>' +
              '</a>' +

          '</div>' +

          '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
              //'<a href=javascript:showLine(\"../HG/PipelineMaintainAnalysis.html?key=' + $.trim(item.JSON_PipelineId) + '\",800,600); class="thumbnail">' +
              '<a href=javascript:ShowNewWindow(\"../HG/ToBeC.html?key=' + $.trim(item.SASSETID) + '\",800,600); class="thumbnail">' +
                  '<img src="../Images/Icon/HGTQ.png" data-src="holder.js/100%x180" alt="...">' +
                   '<div class="fontStyle">内检测记录</div>' +
              '</a>' +

          '</div>' +


    '</div>' +
    '<div class="row map-facility-card-row">' +

          '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
              '<a href=javascript:ShowNewWindow(\"../HG/PipelineFaultAnalysis.html?key=' + $.trim(item.SASSETID) + '\",800,600); class="thumbnail">' +
                  '<img src="../Images/Icon/GZTJ.png" data-src="holder.js/100%x180" alt="...">' +
                   '<div class="fontStyle">故障统计</div>' +
              '</a>' +

          '</div>' +

          '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
              '<a href=javascript:ShowNewWindow(\"../HG/PipelineRisk.html?key=' + $.trim(item.SASSETID) + '\",800,600); class="thumbnail">' +
                  '<img src="../Images/Icon/FXPG.png" data-src="holder.js/100%x180" alt="...">' +
                   '<div class="fontStyle">风险评估</div>' +
              '</a>' +

          '</div>' +

                    '<div class="col-xs-8 col-md-3 map-facility-card-item">' +
                        '<a href=javascript:ShowNewWindow(\"../HG/HGRealTimeView.html?key=' + $.trim(item.SASSETID) + '\",800,600); class="thumbnail">' +
                            '<img src="../Images/Icon/HGRT.png" data-src="holder.js/100%x180" alt="...">' +
                            '<div class="fontStyle">运行趋势</div>' +
                        '</a>' +

                    '</div>' +


                 '</div>';
                //存储管道经纬度坐标
                var latlngs = [];
                try {
                    $.each(pipelineLatLongs, function (indexLatLong, itemLatLong) {
                        if (itemLatLong) {
                            //管道位置信息表和管道属性信息表以唯一ID关联管道
                            if ($.trim(itemLatLong.PipelineId) == $.trim(item.SASSETID)) {
                                //数组末尾添加
                                latlngs.push([Number(this.Latitude), Number(this.Longitude)]);//经纬度转为数字
                            }
                        }
                    });
                } catch (e) {

                }
                //管线颜色
                var color = '#000000';
                //zhangll add at 2017312 test whether colour can be decided by the JS
                if (item.COLOUR != null)
                {
                    color = item.COLOUR;
                }
                else if (item.STRANSMEDIA == '天然气') {
                    color = TRQ_Color;//黄
                } else if (item.STRANSMEDIA == '混输') {
                    color = HS_Color;//红
                } else if (item.STRANSMEDIA == '原油') {
                    color = YY_Color;//黑
                } else if (item.STRANSMEDIA == '注水') {
                    color = '#006400';//绿
                }
                var polyline = L.polyline(latlngs, { color: color, weight: 4 });  //线宽
                /*.bindPopup(getLineHtmlContent({
                    key: item.SASSETNAME,
                    content: '<div style="width:100%;font-weight:600;background:#E1E5EE;height:45px;vertical-align:middle;">管线长度:<a class="fontStyle">'
                        + Number(item.DLENGTH).toFixed(2) + '千米</a>&nbsp;&nbsp;&nbsp;&nbsp;内管直径:<a class="fontStyle">' + item.DOD + '英寸</a></br>投产年份:<a class="fontStyle">'
                        + item.SOPERATIONSTART + '年</a>&nbsp;&nbsp;&nbsp;&nbsp;设计寿命:<a class="fontStyle">' + item.IPLANOPERYEAR + '年</a>&nbsp;&nbsp;&nbsp;&nbsp;输送介质:<a class="fontStyle">' + item.STRANSMEDIA + '</a></p></div>' + cssStr,
                    height: 140,
                    width: 300
                }))*/

                //弹出窗口
                polyline.bindPopup(item.SASSETNAME + '</br>管线长度：' + Number(item.DLENGTH).toFixed(2) + '千米</br>内管直径：' + item.DOD + '英寸</br>投产年份：' + item.SOPERATIONSTART + '年</br>设计寿命：' + item.IPLANOPERYEAR + '年</br>输送介质：' + item.STRANSMEDIA + '</br>长期航告编号：' + hangGaoBianHao).addTo(map);
                //名称提示
                polyline.bindTooltip(item.SASSETNAME, {
                    direction: 'center',
                    sticky: true,
                    //permanent:true
                }).addTo(map);

               /* //设置管道标注
                var multiCoordsLine = [];
                multiCoordsLine.push(latlngs);
                //alert(multiCoordsLine);
                //无法确定标注位置2017.3.6

                var symbolPiplineNameTile = new L.Symbol.Marker( {
                    markerOptions: {
                        clickable: false,
                        icon: L.divIcon({
                            iconSize: [12, 12],
                            iconAnchor: [24, 0],
                            className: 'my-div-icon',
                            html: '<span style="color:#FF8C00;line-height:100%;word-spacing:0px;font-size:11px;">'
                                + item.STRANSMEDIA + '管道</span>'
                        })
                    },
                    rotate: true
                }).addTo(map);
               
                var symbolPiplineLengthTile = new L.Symbol.Marker({
                    markerOptions: {
                        clickable: false,
                        icon: L.divIcon({
                            iconSize: [12, 12],
                            iconAnchor: [-24, 0],
                            className: 'my-div-icon',
                            html: '<span style="color:#FF8C00;line-height:100%;word-spacing:0px;font-size:11px;">长度'
                                + item.DLENGTH + '</span>'
                        })
                    },
                    rotate: true
                });*/
            } catch (e) {

            }


        });

    } catch (e) {

    }
    //alert(1);
   // var p = L.marker([28.66434, 123.98621]).addTo(map);


   //p.bindTooltip("111111", {
   //     direction: 'right',
     
   //}).openTooltip();

    //alert(2);
}



function showPipLiner(id) {
    if (id.indexOf("x") < 0) {
        var url = '../HG/HGDetail.html?id=' + id;
        //showLine(url);
        ShowNewWindow(url);
    } else {
        alert("管道基本信息为空。");
    }
}

function showLine(url) {
    window.open(url, "_blank", "width=1254, resizable=yes, scrollbars=yes, titlebar=yes")
}
//添加管线图例
function Legend() {
    var conBox = L.control({ position: 'bottomleft' });
    conBox.onAdd = function (map) {
        this._div = L.DomUtil.create('div', ''); // create a div with a class "info"
        this.update();
        return this._div;
    };
    conBox.update = function (props) {
        //this._div.innerHTML = '<div style="padding:10px;opacity:0.8;background:#BAC2D8;border-radius:5px;color:#12385F;font-size:14px;font-weight:bolder;"><span style="color:' + TRQ_Color + ';">————天然气</span></br><span  style="color:' + HS_Color + ';">————混输</span></br><span  style="color:' + YY_Color + ';">————原油</span></div>';
        this._div.innerHTML = '<div style="padding:10px;background:rgba(255, 255, 255, 0.2) !important;filter: alpha(opacity=20);  background:#BAC2D8;border-radius:5px;color:#12385F;font-size:14px;font-weight:bold;"><span style="color:' + TRQ_Color + ';">————天然气</span></br><span  style="color:' + HS_Color + ';">————混输</span></br><span  style="color:' + YY_Color + ';">————原油</span></br><span  style="color:#0000EE;">————注水</span></div>';
    };
    conBox.addTo(map);
    return conBox
}
function getLineHtmlContent(props) {
    var htmlContent = "";
    htmlContent += '<div style="width:100%;text-align:center;min-height:' + props.height + 'px;min-width:' + props.width + 'px;"><h5 style="color:#12385F;">';
    htmlContent += props.key;
    htmlContent += '</h5>';
    //htmlContent+='<div><img src="Images/CheckPoints/KP340.958.png" style="height:120px;width:160px;" /></div>';

    htmlContent += '<div>' + props.content + '</div>';

    htmlContent += "</div>";
    return htmlContent;
}


function RefreshPipelineLabels() {
    try {
        var zoomLevel = map.getZoom();
        if (arrPipelineLabels) {
            if (arrPipelineLabels.length > 0) {
                $.each(arrPipelineLabels, function (index, item) {
                    if (item) {
                        if (zoomLevel < 9) {
                            item.onRemove(map);
                        }
                        else {
                            item.onAdd(map);
                        }
                    }
                });
            }

        }
    } catch (e) {

    }

}