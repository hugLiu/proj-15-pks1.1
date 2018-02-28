$(function () {

    $.ajaxSettings.async = false;

    cnoocGis.initialMap();

    //cnoocGis.renderLayers();

    //cnoocGis.getBasinData();
    //cnoocGis.getFirstSuData();
    //cnoocGis.getSecondSuData();
    //cnoocGis.getWorkAreaData();
    //cnoocGis.getTrapData();
    //cnoocGis.getWellData();
    //cnoocGis.Legend();
    cnoocGis.showtime();
    //cnoocGis.locationPoint();


    $(document).on("changed.bs.select", ".selectpicker", function (e) {
        var target = e.currentTarget;
        var bot = $(target).closest(".filter-sidebar").attr("id");
        var filterScript = layerCon.getBotFilterScript(target);
        var source = layerCon.getSourceById(bot);
        if (source)
            source.setFilter(function (f) {
                var ret = eval(filterScript);
                return ret;
            });
        filterScript = null;
    });

});