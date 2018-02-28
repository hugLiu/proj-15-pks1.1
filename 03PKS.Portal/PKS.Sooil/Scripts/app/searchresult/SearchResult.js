/// <reference path="../vue.2.3.2.js" />
app.init(function () {

    function getPageParamObj() {
        var pageParams = {}
        var href = location.search;
        var queryParams = href.split('&');
        var advanceFilterFields = ["ischecked","field","operator", "matchtext1","matchtext2" ];
        for (var i = 0; i < queryParams.length; i++) {
            if (!queryParams[i] || queryParams[i].length === 1)
                continue;
            if (queryParams[i].indexOf("?") === 0) {
                queryParams[i] = queryParams[i].substring(1);
            }
            var paramArr = queryParams[i].split('=');
            //除去高级搜索条件
            if (advanceFilterFields.indexOf(paramArr[0].toLowerCase() > -1))
                pageParams[paramArr[0].toLowerCase()] = paramArr.length > 1 ? decodeURI(paramArr[1]).replace(/\+/ig, ' ').replace(/%2B/ig, ' ').replace(/%20/ig, ' ') : '';
        }
        return pageParams;
    }

    var pageParams = getPageParamObj();
    var recentDates = new RecentDates();
    var recentDateNames = recentDates.Dates;
    if (pageParams.recentdatecode) {
        pageParams.recentDateRange = recentDates.getBeginEndDate({ "code": pageParams.recentdatecode });
    } else {
        pageParams.recentdatecode = 'all';
    }

    var service = di.resolve("SearchResult");
    var searchuiconfig = service.getConfig();

    var searchbarService = di.resolve("SearchBar");
    var sourcenamefield = searchbarService.getConfig().sourcename.field;
    var dataConfig = $.extend(true,
        {},
        pageParams,
        {
            segmentWords:[],//对象分词后重新组装的搜索字符串
            pageInfo: {
                pageIndex: pageParams.pageindex ? parseInt(pageParams.pageindex) : 1,
                pageSize: searchuiconfig.searchlistconfig.pagesize,
                total: 0,
                totalPage: 1,
                showPageNums: [] //页面显示页码
            },
            sidefilters: [],
            searchResultList: [],
            recentDates: recentDateNames,
            //左侧热词
            hotWords: [],
            //执行时间
            executionTime: 0,
            leftSideConfig: [],
            aggdata:{},//左侧聚合数据
            searchuiconfig: searchuiconfig,
            sourcenamefield: sourcenamefield //数据源名称过滤
        });

    var vueConfig = {
        el: "#pagecontent",
        data: dataConfig,
        methods: {
            getPropertyValue:function(propertyName) {
                return this[propertyName];
            },
            /**
             * 分页导航
             * @param {} pageIndex 
             * @returns {} 
             */
            navigateToPage: function (pageIndex) {
                if (pageIndex < 1)
                    pageIndex = 1;
                if (pageIndex > this.pageInfo.totalPage)
                    pageIndex = this.pageInfo.totalPage;
                this.pageInfo.pageIndex = pageIndex;
                var params = {};
                params.pageindex = pageIndex;
                var url = $.url.concat(decodeURI(location.href), params);
                location.href = url;
            },
            /**
             * 获取详细页地址
             * @param {} iiid 
             * @param {} pageid 
             * @returns {} 
             */
            getPageDetailUrl:function(iiid) {
                return $G.portalUrl + "/Render/Content?iiid=" + iiid;
            },
            /**
             * 高亮摘要
             * @param {} abstract 
             * @returns {} 
             */
            hightLightAbstract: function (abstractText) {
                var html = "<span style=\"color: #666\">摘要：</span>";
                if (abstractText) {
                    if (!this.w)
                        return html+abstractText;
                    //html += (this.w ? highlight2(abstractText, this.w) : abstractText);
                    if (this.segmentWords.length > 0) {
                        var hightText = abstractText;
                        for (var l = 0; l < this.segmentWords.length; l++) {
                            var hightResult = highlight2(abstractText, this.segmentWords[l]);
                            if (hightResult.length > hightText.length)
                                hightText = hightResult;
                        }
                        html += hightText;
                    }
                }
                return html;
            },
            /**
             * 高亮标题
             * @param {} sourceName 
             * @param {} title 
             * @returns {} 
             */
            hightLightTitle: function (sourceName, title) {
                var html = "";
                if (sourceName) {
                    html += "<span style=\"color: #F0C44E\">[" + sourceName + "]</span>";
                }
                if (!this.w)
                    return html + title;
                //html += (this.w ? highlight2(title, this.w) : title);
                if (this.segmentWords.length > 0) {
                    var hightText = title;
                    for (var l = 0; l < this.segmentWords.length; l++) {
                        var hightResult = highlight2(title, this.segmentWords[l]);
                        if (hightResult.length > hightText.length)
                            hightText = hightResult;
                    }
                    html += hightText;
                }
                return html;
            },
            /**
             * 最近**跳转链接
             * @param {} code 
             * @returns {} 
             */
            getRecentDataNavigateUrl:function(code) {
                return $.url.concat(decodeURI(location.href), { "recentdatecode": code });
            },
            /*是否聚合字段
            */
            isAggField: function (fieldName) {
                var leftSiders = this.searchuiconfig.leftsideconfig.leftsiders;
                for (var i = 0; i < leftSiders.length; i++) {
                    if (leftSiders[i].field == fieldName)
                        return true;
                }
                return false;
            },
            /*获取聚合导航Url
            */
            getAggregationNavigateUrl: function (fieldName,fieldValue) {
                if (this.isadvance)
                {
                    var hasFilter = false;
                    var dataFilter = [];
                    var advanceConditions = service.getAdvanceConditions();
                    for (var i = 0; i < advanceConditions.length; i++) {
                        if (advanceConditions[i].field == fieldName)
                        {
                            advanceConditions[i].operator = "equal";
                            advanceConditions[i].matchtext1 = fieldValue;
                            hasFilter = true;                          
                        }
                        dataFilter.push("ischecked=on&field=" + advanceConditions[i].field + "&operator=" + advanceConditions[i].operator + "&matchtext1=" + advanceConditions[i].matchtext1);
                    }
                    if (!hasFilter)
                        dataFilter.push("ischecked=on&field=" + fieldName + "&operator=equal&matchtext1=" + fieldValue);
                    return "/search/list?isadvance=true&" + dataFilter.join('&');
                }
                else {
                    return "/search/list?dimension=" + fieldName + "&" + fieldName + "=" + fieldValue + "&w=" + (this.w?this.w:'');
                }
            }
        }
    }
    
   
    var vm = new Vue(vueConfig);
    vm.segmentWords = service.getSearchWordsBySegment(pageParams['w']);
    //todo 整理分页相关运算
    /**
    * 加载列表数据及左侧聚合信息
    */
    var searchResult = service.getSearchDataList(vm);
    if (searchResult.success) {
        vm.searchResultList = searchResult.data.list.data;
        //聚合属性赋值
        vm.aggdata = searchResult.data.aggdata;
        //分页
        vm.pageInfo.total = searchResult.data.list.total;
        if (vm.pageInfo.total == 0)
            vm.pageInfo.totalPage = 0;
        else {
            if (vm.pageInfo.total < vm.pageInfo.pageSize)
                vm.pageInfo.totalPage = 1;
            else {
                if (vm.pageInfo.total % vm.pageInfo.pageSize == 0) {
                    vm.pageInfo.totalPage = vm.pageInfo.total / vm.pageInfo.pageSize;
                } else {
                    vm.pageInfo.totalPage = parseInt(vm.pageInfo.total/vm.pageInfo.pageSize) + 1;
                }
            }
        }
        //分页：查找起始页码和结束页码
        var totalPage = vm.pageInfo.totalPage;
        var curPageIndex = vm.pageInfo.pageIndex;
        var middlePageNum = totalPage - 3;//页面只显示7个页码
        var showPageNums = [];
        if (vm.pageInfo.totalPage <= 7) {
            for (var i = 0; i < vm.pageInfo.totalPage; i++) {
                showPageNums.push(i + 1);
            }
        } else {
            if (curPageIndex <= middlePageNum) {
                var preNum = 0;
                for (var j = curPageIndex - 1; j >= 1; j--) {
                    //当前页前最多取3个数
                    if (preNum < 3) {
                        showPageNums.push(j);
                        preNum++;
                    } else
                        break;
                }
                showPageNums.push(curPageIndex);
                //剩余的页码数
                var leftPageNums = 7 - preNum - 1;
                var afterNum = 0;
                for (var j = curPageIndex + 1; j < totalPage; j++) {
                    if (afterNum < leftPageNums) {
                        showPageNums.push(j);
                        afterNum++;
                    } else
                        break;
                }
            } else {
                var afterNum = 0;
                for (var k = curPageIndex + 1; k < totalPage + 1; k++) {
                    afterNum++;
                    showPageNums.push(k);
                }
                showPageNums.push(curPageIndex);
                //剩余的页码数
                var leftPageNums = 7 - afterNum - 1;
                var preNum = 0;
                for (var m =curPageIndex-1 ; m>=1 ; m--) {
                    if (preNum < leftPageNums) {
                        showPageNums.push(m);
                        preNum++;
                    }
                }
            }
        }
        vm.pageInfo.showPageNums = showPageNums.sort(function(a,b) { return a - b; });
        vm.executionTime = searchResult.executionTime;
    } else {
        if (vm.w)
            alert('加载数据失败');
    }

    /**
   * 加载左侧下方热词【异步加载】
   */
    service.getTopHotWords(function(data) {
        vm.hotWords = data;
    });
});



