/**
*
*   加载海域基线
*   2014年10月30日
*
*/

$(function () {
    loadBaseLine();
});
function loadBaseLine() {
    $.ajax({
        url: ServiceURL + "IServices/BaseLine.js",
        data: {},
        type: "get",
        dataType: "json",
        success: function (result) {
            $.each(result, function () {
                var latlngs = [];
                var points=[];
                $.each(this.JSON_Data, function (index, item) {
                    latlngs.push([ Number(item.Lat),Number(item.Lng)]);
                });
                //绘制线
                var polyline = L.polyline(latlngs, { color: '#000000', weight: 1, opacity: 0.9, dashArray: [10, 10] }).bindLabel(this.JSON_Name).addTo(map);
        });
}
});
}