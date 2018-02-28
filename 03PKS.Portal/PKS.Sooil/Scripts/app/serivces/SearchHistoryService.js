var SearchHistoryService = BaseService.extend({
    init: function() {
        this._super();
    },
    /**
   * 获取热词
   * @param {successCallback} 调用成功回调方法
   * @param {topCount} 靠前多少个
   * @returns {} 
   */
    getTopHotWords: function(successCallback, topCount) {
        if (!topCount)
            topCount = 5;
        $.ajax({
            url: "/SearchHistory/GetHotSearchWord",
            type: "post",
            dataType: "json",
            data: { "topCount": topCount },
            success: function(data) {
                successCallback(data);
            },
            error: function() {

            }
        });
    },
    /**
     * 保存用户行为记录公用方法
     * @param {} historyModel 
     * @returns {} 
     */
    saveSearchHistoryModel: function(historyModel) {
        $.ajax({
            async:false,
            url: "/SearchHistory/SaveLogResult",
            data: {
                model: JSON.stringify(historyModel)
            },
            type: "post",
            datatype: "json",
            success: function(result) {
                
            },
            error: function() {
                
            }
        });
    },

    /**
     * 历史数据
     * @param {} pageId 
     * @param {} historyData 
     * @returns {} 
     */
    saveSearchHistoryData: function(pageId, historyData) {
        $.ajax({
            async:false,
            url: "/SearchHistory/SavaEnvData",
            data: {
                pageId: pageId,
                data: JSON.stringify(historyData)
            },
            type: "post",
            datatype: "json",
            success: function(result) {
            }
        });
    }
});