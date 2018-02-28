(function (q) {

    var source = cnoocGis.h.wells;

    var slyMap = {
        默认: {
            radius: 4,
            color: "#000",
            weight: 1,
            opacity: 1,
            fillColor: "#ff7800",
            fillOpacity: 0.8,
        }
    };

    var getStyleOfFeature = function (feature, styleMap) {
        return styleMap["默认"];
    };

    $.ajaxSettings.async = false;

    var ctxConfig = null;

    $.getJSON("../Content/Gis/contextMenu4Well.json", function (result) {
        ctxConfig = result;
    });

    layerCon.setOptsOfPt(source, getStyleOfFeature, slyMap, ["fillColor","color"], ctxConfig, function (feature, latlng, marker) {
        marker.bindPopup(layerCon.buildPopup(feature));
    });

    cnoocGis.getWellData = function () {
        var queryConfig = {
            "query": {
                "bot": "井"

            },
            "sort": { "boid": 1 },
            "skip": 0,
            "limit": 1000
        };
        var query = layerCon.getBos(queryConfig);
        var ps = query.done(function (data, status, xhr) {
            source.addData(bizPointsToGeoJson(data));
        }).fail(function (data, status, xhr) {
            console && console.log && console.log(data);
        });
        return ps;
    };
})(window);


