var SearchResultServiceExtend = SearchResultService.extend({
    init: function () {
        this._super();
    },
  
    getTitle: function (metadataItem) {
        //console.log("重写基类方法:getTitle");
    }
});
di.register("SearchResult", SearchResultServiceExtend);