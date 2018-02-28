(function (q) {

    var source = cnoocGis.h.firstSus;

    var slyMap = {
        默认: {
            "stroke": true,//边缘是否可见
            "color": 'black',//边缘颜色
            "opacity": 0.3,//边缘透明度
            "weight": 1,//边缘粗细
            "fillColor": '#FED2D1',//填充颜色
            "fillOpacity": 0,//填充透明度
        },
        隆起: {
            "stroke": true,//边缘是否可见
            "color": 'black',//边缘颜色
            "opacity": 0.3,//边缘透明度
            "weight": 1,//边缘粗细
            "fillColor": '#FFDBAA',//填充颜色
            "fillOpacity": 1,//填充透明度
        },
        低隆起: {
            "stroke": true,//边缘是否可见
            "color": 'black',//边缘颜色
            "opacity": 0.3,//边缘透明度
            "weight": 1,//边缘粗细
            "fillColor": '#FFE2BC',//填充颜色
            "fillOpacity": 1,//填充透明度
        }
    };

    var getFeatureStyle = function (feature, styleConfig) {
        var bo = feature.bo;
        if (/低隆起/g.test(bo))
            return styleConfig["低隆起"];
        if (/隆起/g.test(bo))
            return styleConfig["隆起"];
        return styleConfig["默认"];
    };

    layerCon.setOpts(source, getFeatureStyle, slyMap,"fillColor");

    cnoocGis.getFirstSuData = function () {
        var queryConfig = {
            "query": {
                "bot": "一级构造单元"
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