(function (q) {

    var source = cnoocGis.h.traps;

    layerCon.setOptsOfTrap(source, null, function (feature, layer) {
        layer.bindPopup(layerCon.buildPopup(feature));
    });

    cnoocGis.getTrapData = function () {
        var queryConfig = {
            "query": {
                "bot": "圈闭"
            },
            "sort": { "boid": 1 },
            "skip": 0,
            "limit": 2000
        };
        var query = layerCon.getBos(queryConfig);
        var ps = query.done(function (data, status, xhr) {
            source.addData(bizGeometryCollectionsToGeoJson(data));
        }).fail(function (data, status, xhr) {
            console && console.log && console.log(data);
        });
        return ps;
    };

})(window);