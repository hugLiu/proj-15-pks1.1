(function (q) {

    var source = cnoocGis.h.workAreas;

    var slyMap = {
        默认:{
            "stroke": true,//边缘是否可见
            "color": '#1CEF1F',//边缘颜色
            "opacity": 1,//边缘透明度
            "weight": 2,//边缘粗细
            "fillColor": '#000000',//填充颜色
            "fillOpacity": 0.6,//填充透明度
        }
    };

    var getStyleOfFeature = function (feature, styleMap) {
        return styleMap["默认"];
    };

    $.ajaxSettings.async = false;

    var ctxConfig = null;

    $.getJSON("../Content/Gis/contextMenu4WorkArea.json", function (result) {
        ctxConfig = result;
    });

    layerCon.setOpts(source, getStyleOfFeature, slyMap, "fillColor", ctxConfig, function (feature, layer) {
        layer.bindPopup(layerCon.buildPopup(feature));
    });

    cnoocGis.getWorkAreaData= function () {
        var queryConfig = {
            "query": {
                "bot": "地震工区"
            },

            "sort": { "boid": 1 },
            "skip": 0,
            "limit": 2005
        };
        var query = layerCon.getBos(queryConfig);
        var ps = query.done(function (data, status, xhr) {
            source.addData(bizPolygonsToGeoJson(data));
        }).fail(function (data, status, xhr) {
            console && console.log && console.log(data);
        });
        return ps;
    }
})(window);