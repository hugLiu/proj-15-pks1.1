(function (q) {

    var source = cnoocGis.h.basins;

    var slyMap = {
        默认: {
            "stroke": true,//边缘是否可见
            "color": '#',//边缘颜色
            "opacity": 1,//边缘透明度
            "weight": 0.6,//边缘粗细
            "fillColor": '#FFFFFF',//填充颜色
            "fillOpacity": 0.3,//填充透明度
        }
    };

    var getStyleOfFeature = function (feature, styleConfig){
        return styleConfig["默认"];
    };

    $.ajaxSettings.async = false;

    var ctxConfig = null;

    $.getJSON("../Content/Gis/contextMenu4Basin.json", function (result) {
        ctxConfig = result;
    });

    layerCon.setOpts(source, getStyleOfFeature, slyMap, "fillColor", ctxConfig);

    cnoocGis.getBasinData = function () {
        var queryConfig = {
            "query": {
                "bot": "盆地"
            },

            "sort": { "boid": 1 },
            "skip": 0,
            "limit": 2000
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