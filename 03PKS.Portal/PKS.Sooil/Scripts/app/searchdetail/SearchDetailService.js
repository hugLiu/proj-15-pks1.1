var SearchDetailService = BaseService.extend({
    init: function() {
        this._super();
    },
    getDetailInfo: function () {
        var id = this.getPageParam("id");
        var data = this.esService.get(id);

    }
});
di.register("SearchDetail", SearchDetailService);