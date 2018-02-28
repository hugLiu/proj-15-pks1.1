var searchBarSerivce = new SearchBarService();
function getSearchHistory(sourceWay) {
    var searchHistoryModel = {};
    //if (sourceWay) {
    //    var newId = new GUID();
    //    searchHistoryModel.SourceId = thisPageId;
    //    searchHistoryModel.SourcePageNameEnum = PageName.Index;
    //    searchHistoryModel.TargetId = newId.newGUID();
    //    searchHistoryModel.TargetPageNameEnum = "";
    //}
    //else {
    //    searchHistoryModel.SourceId = "";
    //    searchHistoryModel.SourcePageNameEnum = "";
    //    searchHistoryModel.TargetId = thisPageId;
    //    searchHistoryModel.TargetPageNameEnum = PageName.Index;
    //}

    searchHistoryModel.PageResultsEnum = "success";
    searchHistoryModel.SourceWayEnum = "Search";
    searchHistoryModel.InputWord = $("#searchContentId").val();
    searchHistoryModel.BO = "";
    searchHistoryModel.BOT = "";
    searchHistoryModel.PT = "";
    searchHistoryModel.BP = "";
    searchHistoryModel.Title = "";
    searchHistoryModel.Iiid = "";
    return searchHistoryModel;
}
/**
 * 点搜索表单提交前的检查
 * @returns {} 
 */
function check() {
    var valueI = document.getElementById('searchContentId').value;
    if (valueI.replace(/(^\s+)|(\s+$)/g, '') == "") {
        return false;
    }

    //用户行为------收集数据库model  有历史行为操作时调用和页面加载
    var searchHistorySerivce = new SearchHistoryService();
    searchHistorySerivce.saveSearchHistoryModel(getSearchHistory());
    //return false;
}
/**
 * 搜索框输入时的下拉推荐
 * @returns {} 
 */
function handle() {
    var txtValue = $("#searchContentId").val().replace("'", "");
    searchBarSerivce.getRecommandWords(function (words) {
        $("#enlist").html("");//刷新下拉列表
        if (words.length > 0) {
            //entryList = data;
            for (var i = 0; i < words.length; i++) {
                var result = "<li style='height:35px;'><a style='line-height:35px;' href='/search/list?w=" + words[i].text + "' title='" + words[i].text + "'><p style='line-height:35px'>" + subString(words[i].text, 40, true) + "</p></a></li>";
                $("#enlist").append(result);
            }
            $(".yxj_tc li").mouseover(function (e) {
                $(this).addClass("on").siblings().removeClass("on");
            });
        }
        $(".yxj_tc").show();

    }, txtValue);

    //放大镜图标隐藏
    //        if(txtValue==''){
    //            $('#icon-search').show();
    //        }else{
    //            $('#icon-search').hide();
    //        }
}

$(document).click(function (e) {
    $("#enlist").html("");
});
function openAdvanced() {
    window.CrashBoard.show({
        id: "advanced",
        url: "/search/advancesearch",
        maskable: true, //是否绘制遮布
        width: '800px',//宽度
        height: '420px',//高度
        callback: function (data) {
        },
        title: '高级搜索'
    });
}
//点击搜索框时隐藏放大镜图标
$(function () {
    $('#searchContentId').focus(function () {
        $('#icon-search').hide();
    })
    $('#searchContentId').blur(function () {
        $('#icon-search').show();
    })
});

var data = {
    searchText: '',
    selectedSourceName: '全部',
    sourceNameList: [],
    hotWords: []
};

var searchText = $.util.pageParams["w"];
if (searchText) {
    searchText = decodeURIComponent(searchText).replace(/\+/ig, ' ');
    if (searchText.indexOf('%') > -1) searchText = decodeURIComponent(searchText);
}
data.searchText = searchText;

var selectedSourceNameParam = $.util.getPageParam("sourcename");
if (selectedSourceNameParam && selectedSourceNameParam !== "0")
    data.selectedSourceName = selectedSourceNameParam;


//数据来源
data.sourceNameList = searchBarSerivce.getSourcenames();
var searchBarConfig = new Vue({
    el: "#searchbar",
    data: data
});
//热词 【异步】
searchBarSerivce.getTopHotWords(function (data) {
    searchBarConfig.hotWords = data;
});