(function(q) {

    var source = cnoocGis.h.secondSus;

    var slyMap = {
        默认: {
            "stroke": false,//边缘是否可见
            "color": 'blue',//边缘颜色
            "opacity": 0.3,//边缘透明度
            "weight": 1,//边缘粗细
            "fillColor": '#DFFDFB',//填充颜色
            "fillOpacity": 0,//填充透明度
        },
        凸起: {
            "stroke": true,//边缘是否可见
            "color": 'blue',//边缘颜色
            "opacity": 0.3,//边缘透明度
            "weight": 1,//边缘粗细
            "fillColor": '#FFEDA0',//填充颜色
            "fillOpacity": 1,//填充透明度
        },
        凹陷: {
            "stroke": true,//边缘是否可见
            "color": 'blue',//边缘颜色
            "opacity": 0.3,//边缘透明度
            "weight": 1,//边缘粗细
            "fillColor": '#BFDDFF',//填充颜色
            "fillOpacity": 1,//填充透明度
        }
    };

    var getFeatureStyle = function (feature, styleConfig) {
        var bo = feature.bo;
        if (/凸起/g.test(bo))
            return styleConfig["凸起"];
        if (/凹陷/g.test(bo))
            return styleConfig["凹陷"];
        return styleConfig["默认"];
    };

    $.ajaxSettings.async = false;

    var ctxConfig = null;

    $.getJSON("../Content/Gis/contextMenu4Su.json", function (result) {
        ctxConfig = result;
    });

    layerCon.setOpts(source, getFeatureStyle, slyMap, "fillColor", ctxConfig, function (feature, layer) {
        layer.bindPopup(layerCon.buildPopup(feature));
    });

    cnoocGis.getSecondSuData = function () {
        var queryConfig = {
            "query": {
                "bot": "二级构造单元"
            },
            "sort": { "boid": 1 },
            "skip": 0,
            "limit": 3005
        };
        var query = layerCon.getBos(queryConfig);
        var ps = query.done(function (data, status, xhr) {
            source.addData(bizPolygonsToGeoJson(data));
        }).fail(function (data, status, xhr) {
            console && console.log && console.log(data);
        });
        return ps;
    };

})(window);