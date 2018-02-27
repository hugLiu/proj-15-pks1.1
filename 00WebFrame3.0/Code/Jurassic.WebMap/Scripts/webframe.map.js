(function ($) {
    var map;

    //创建和初始化地图函数：
    $.fn.initMap = function (mapFormData) {
        var zoom = 17;
        map = new BMap.Map(this.attr("id"));//在百度地图容器中创建一个地图
        setMapEvent();//设置地图事件
        addMapControl();//向地图添加控件
        //  addMarker();//向地图中添加marker
        var mapAddress = mapFormData.Address;
        $('#' + mapFormData.HiddenId).val(JSON.stringify(mapAddress)).change();
        var txtBox = mini ? $('#' + mapFormData.TextBoxId + ' input') : $('#' + mapFormData.TextBoxId);
        txtBox.val(mapAddress.Address);

        if (mapAddress.TargetLongitude > 0) {
        }
        else {
            mapAddress.TargetLongitude = 114.388758;
            mapAddress.TargetLatitude = 30.471632;
        }

        var geoc = new BMap.Geocoder();
        if (mapAddress.Longitude > 0) {
            //如果参数已经包含坐标，则定位到此坐标
            var point = new BMap.Point(mapAddress.Longitude, mapAddress.Latitude);
            map.centerAndZoom(point, zoom);
            map.clearOverlays();
            map.addOverlay(new BMap.Marker(point));
        }
        else {
            //否则获取当前位置
            var geolocation = new BMap.Geolocation();
            geolocation.getCurrentPosition(function (r) {
                if (this.getStatus() == BMAP_STATUS_SUCCESS) {
                    map.centerAndZoom(r.point, zoom);
                    map.panTo(r.point);
                    geoc.getLocation(r.point, afterLocate);
                    //  alert('您的位置：' + r.point.lng + ',' + r.point.lat);
                }
                else {
                    alert('failed' + this.getStatus());
                }
            }, { enableHighAccuracy: true });
        }

        function afterLocate(rs) {
            var addComp = rs.addressComponents;
            var address = addComp.province + ", " + addComp.city + ", " + addComp.district + ", " + addComp.street + ", " + addComp.streetNumber;
            txtBox.val(address);
            map.clearOverlays();
            map.addOverlay(new BMap.Marker(rs.point));
            savePoint(rs.point);
        }

        function savePoint(p) {
            if (p) {
                mapAddress.Longitude = p.lng;
                mapAddress.Latitude = p.lat;
            }
            else {
                p = new BMap.Point(mapAddress.Longitude, mapAddress.Latitude);
            }
            var targetPoint = new BMap.Point(mapAddress.TargetLongitude, mapAddress.TargetLatitude);
            mapAddress.Address = txtBox.val();
            if (targetPoint) {
                mapAddress.Distance = map.getDistance(p, targetPoint).toFixed(2);
            }
            $('#' + mapFormData.HiddenId).val(JSON.stringify(mapAddress)).change();
        }

        if (mapFormData.AllowManualPoint) {
            //用点击来确定地点
            map.addEventListener("click", function (e) {
                var pt = e.point;
                geoc.getLocation(pt, afterLocate);
            });

            var timeId;
            //在文本框中输入来确定地点
            txtBox.change(function () {
                //根据文本框的变化在地图上定位
                if (timeId) {
                    clearTimeout(timeId);
                    timeId = null;
                }
                timeId = setTimeout(function () {
                    var txt = txtBox.val();
                    // 创建地址解析器实例
                    // 将地址解析结果显示在地图上,并调整地图视野
                    geoc.getPoint(txt, function (point) {
                        if (point) {
                            map.centerAndZoom(point, zoom);
                            map.clearOverlays();
                            map.addOverlay(new BMap.Marker(point));
                            savePoint(point);
                        } else {
                            //alert("您选择地址没有解析到结果!");
                        }
                    }, "武汉市");
                }, 1000);
            });
        }

        map.setTarget = function (point) {
            mapAddress.TargetLongitude = point.lng;
            mapAddress.TargetLatitude = point.lat;
            point = new BMap.Point(point.lng, point.lat);
            savePoint();
            map.centerAndZoom(point, zoom);
            map.clearOverlays();
            map.addOverlay(new BMap.Marker(point));
        };
        return map;
    }

    //地图事件设置函数：
    function setMapEvent() {
        //map.enableDragging();//启用地图拖拽事件，默认启用(可不写)
        map.enableScrollWheelZoom();//启用地图滚轮放大缩小
        map.enableDoubleClickZoom();//启用鼠标双击放大，默认启用(可不写)
        map.enableKeyboard();//启用键盘上下左右键移动地图
    }

    //地图控件添加函数：
    function addMapControl() {
        //向地图中添加缩放控件
        var ctrl_nav = new BMap.NavigationControl({ anchor: BMAP_ANCHOR_TOP_LEFT, type: BMAP_NAVIGATION_CONTROL_LARGE });
        map.addControl(ctrl_nav);
        //向地图中添加缩略图控件
        var ctrl_ove = new BMap.OverviewMapControl({ anchor: BMAP_ANCHOR_BOTTOM_RIGHT, isOpen: 0 });
        map.addControl(ctrl_ove);
        //向地图中添加比例尺控件
        var ctrl_sca = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_BOTTOM_LEFT });
        map.addControl(ctrl_sca);
    }
})(jQuery);