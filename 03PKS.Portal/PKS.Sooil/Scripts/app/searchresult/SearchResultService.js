var SearchResultService = BaseService.extend({
    init: function() {
        this._super();
    },
   /** 获取配置
     * @returns {} 
     */
    getConfig: function() {
        var config = null;
        $.ajax({
            async: false,
            url: "/scripts/app/searchresult/config.json",
            dataType: "json",
            success: function(data) {
                config = data;
            },
            error:function() {
                
            }
        });
        return config;
    },
    /** 获取高级搜索配置
     * @returns {} 
     */
    getAdvanceSearchConfig: function () {
        var config = null;
        $.ajax({
            async: false,
            url: "/scripts/app/advancesearch/config.json",
            dataType: "json",
            success: function (data) {
                config = data;
            },
            error: function () {

            }
        });
        return config;
    },
    convertToResultItem: function(data) {
        var result = {};
        //数据列表
        result.list = { data: [], total: 0 };
        if (data && data.hits) {
            result.list.total = data.hits.total;
            var metadtas = data.hits.hits;
            result.list.data = [];
            for (var i = 0; i < metadtas.length; i++) {
                //metadtas[i]._source.datatype = "html";
                if (metadtas[i]._source.createddate)
                    metadtas[i]._source.createddate = moment(metadtas[i]._source.createddate, "YYYY-MM-DDTHH:mm:ss.SSSZ").toDate().format("yyyy-MM-dd hh:mm:ss");
                result.list.data.push(metadtas[i]._source);
            }
        //for (var i = 0; i < metadtas.length; i++) {
            //    var dataItem = {

            //    };
            //    var metadta = metadtas[i]._source;
            //    dataItem.id = metadta._id;
            //    dataItem.iiid = metadta.iiid;
            //    //标题
            //    dataItem.title = this.getTitle(metadta);
            //    //来源
            //    dataItem.source = this.getSourceName(metadta);
            //    //文件格式
            //    dataItem.sourceFormat = this.getSourceFormat(metadta);
            //    //作者
            //    dataItem.creator = this.getCreator(metadta);
            //    //创建时间
            //    dataItem.createdDate = this.getCreatedDate(metadta);
            //    //摘要
            //    dataItem.abstract = this.getAbstract(metadta);
            //    var thumbnail = metadta.thumbnail;
            //    dataItem.showThumbnail = thumbnail ? true : false;
            //    dataItem.thumbnail = thumbnail;
            //    if (thumbnail) {
            //        //暂时通过判断不带图片后缀时为图片base64
            //        if (thumbnail.indexOf('.png') == - 1 &&
            //            thumbnail.indexOf('.jpg') == - 1 &&
            //            thumbnail.indexOf('.bmp') == -1)
            //            dataItem.thumbnail = 'data:image/png;base64,' + dataItem.thumbnail;
            //    }
            //    result.list.data.push(dataItem);
            //}
        }

        //聚合赋值
        result.aggdata = {};
        for (var aggName in data.aggregations) {
            var buckets = data.aggregations[aggName].buckets;
            if (!buckets)
                continue;
            result.aggdata[aggName] = [];
            for (var j = 0; j < buckets.length; j++) {
                result.aggdata[aggName].push({
                    "name": buckets[j].key,
                    "value": (j + 1).toString(),
                    "count": buckets[j]["doc_count"],
                    "linkname": (buckets[j].key ? buckets[j].key:"未知") + "(" + buckets[j]["doc_count"] + ")"
                });
            }
        }
        return result;
    },
    getAdvanceFieldMatch:function(condition) {
        var matchMethod = condition.accuracy == "M" ? "match" : "term";
        var field = condition.articleClassiy == "title" ? "dc.title.text" : "dc.description.text";
        var fieldKeyword = condition.accuracy == "M" ? field : (field + ".keyword");

        var fieldMatch = {};
        fieldMatch[fieldKeyword] = condition.text;
        var fieldMethod = {};
        fieldMethod[matchMethod] = fieldMatch;
        return fieldMethod;
    },
    getEsCondition:function() {
        
    },
    buildAdvanceSearchFilter: function(boolFilter) {
        var advanceConditions = this.getAdvanceConditions();
        var mustFilters = [];
        var shouldFilters = [];
        var mustNotFilters = [];
        var advanceSearchConditionsConfig = this.getAdvanceSearchConfig().defaultconditions;
        //for (var j = 0; j < advanceSearchConditions.length; j++) {
        //    var name = advanceSearchConditions[j].name;
        //    var searchFields = advanceSearchConditions[j].searchfields;
           
        //}
        for (var i = 0; i < advanceConditions.length; i++) {
            var field = advanceConditions[i].field;
            var operator = advanceConditions[i].operator;
            if (field.indexOf('m_') == 0) {
                //如果为多字段搜索
                for (var j = 0; j < advanceSearchConditionsConfig.length; j++) {
                    if (advanceSearchConditionsConfig[j].name == field) {
                        if (operator == "equal") {
                            var boolshould = [];
                            for (var k = 0; k < advanceSearchConditionsConfig[j].searchfields.length; k++) {
                                var searchField = advanceSearchConditionsConfig[j].searchfields[k];
                                var termFilter = { "term": {} };
                                termFilter.term[searchField + ".keyword"] = advanceConditions[i].matchtext1;
                                boolshould.push(termFilter);
                            }
                            mustFilters.push({ "bool": { "should": boolshould}});
                        }
                        else if (operator == "contain") {
                            var matchContainText = advanceConditions[i].matchtext1;
                            if (matchContainText) {
                                var matchContainTextArr = matchContainText.split(';');
                                var boolshould = [];
                                for (var index = 0; index < matchContainTextArr.length; index++) {
                                    if (matchContainTextArr[index]) {
                                        for (var k = 0; k < advanceSearchConditionsConfig[j].searchfields.length; k++) {
                                            var searchField = advanceSearchConditionsConfig[j].searchfields[k];
                                            var containFilter = { "match_phrase": {} };
                                            containFilter.match_phrase[searchField] = matchContainTextArr[index];
                                            boolshould.push(containFilter);
                                        }
                                    }
                                }
                                if (boolshould.length > 0)
                                    mustFilters.push({ "bool": { "should": boolshould } });
                            }
                        }
                        else if (operator == ">=") {
                            var boolshould = [];
                            for (var k = 0; k < advanceSearchConditionsConfig[j].searchfields.length; k++) {
                                var searchField = advanceSearchConditionsConfig[j].searchfields[k];
                                var gteFilter = { "range": {} };
                                var matchText = advanceConditions[i].matchtext1;
                                if (advanceConditions[i].type == "DateString")
                                    matchText = moment(new Date(matchText)).toISOString();
                                gteFilter.range[searchField + ".keyword"] = { "gte": matchText };
                                boolshould.push(gteFilter);
                            }
                            mustFilters.push({ "bool": { "should": boolshould } });
                        }
                        else if (operator == "<=") {
                            var boolshould = [];
                            for (var k = 0; k < advanceSearchConditionsConfig[j].searchfields.length; k++) {
                                var searchField = advanceSearchConditionsConfig[j].searchfields[k];
                                var lteFilter = { "range": {} };
                                var matchText = advanceConditions[i].matchtext1;
                                if (advanceConditions[i].type == "DateString")
                                    matchText = moment(new Date(matchText)).toISOString();
                                gteFilter.range[searchField + ".keyword"] = { "lte": matchText };
                                boolshould.push(lteFilter);
                            }
                            mustFilters.push({ "bool": { "should": boolshould } });
                        }
                        else if (operator == "between") {
                            var boolshould = [];
                            for (var k = 0; k < advanceSearchConditionsConfig[j].searchfields.length; k++) {
                                var searchField = advanceSearchConditionsConfig[j].searchfields[k];
                                var betweenFilter = { "range": {} };
                                var matchText1 = advanceConditions[i].matchtext1;
                                var matchText2 = advanceConditions[i].matchtext2;
                                if (advanceConditions[i].type == "DateString") {
                                    matchText1 = moment(new Date(matchText1)).toISOString();
                                    matchText2 = moment(new Date(matchText2)).toISOString();
                                }
                                betweenFilter.range[searchField] = {
                                    "gte": matchText1,
                                    "lte": matchText2
                                };
                                boolshould.push(betweenFilter);
                            }
                            mustFilters.push({ "bool": { "should": boolshould } });
                        }
                        break;
                    }
                }
                continue;
            }

            for (var j = 0; j < advanceSearchConditionsConfig.length; j++) {
                if (advanceSearchConditionsConfig[j].name == field) {
                    advanceConditions[i].ctype = advanceSearchConditionsConfig[j].ctype;
                    break;
                }
            }

            if (operator == "equal") {
                var termFilter = { "term": {} };
                termFilter.term[field + ".keyword"] = advanceConditions[i].matchtext1;
                mustFilters.push(termFilter);
            }
            else if (operator == "terms") {
                var termsFilter = {};
                termsFilter.terms = {};
                termsFilter.terms[field + ".keyword"] = advanceConditions[i].matchtext1.split(",");
                mustFilters.push(termsFilter);
            }
            else if (operator == "notequal") {
                var mustNotTerm = { "term": {} };
                mustNotTerm.term[field + ".keyword"] = advanceConditions[i].matchtext1;
                mustNotFilters.push(mustNotTerm);
            }
            else if (operator == "contain") {
                //var containFilter = { "match_phrase": {} };
                //containFilter.match_phrase[field] = advanceConditions[i].matchtext1;
                //mustFilters.push(containFilter);
                var matchContainText = advanceConditions[i].matchtext1;
                if (matchContainText) {
                    var matchContainTextArr = matchContainText.split(',');
                    for (var index = 0; index < matchContainTextArr.length; index++) {
                        if (matchContainTextArr[index]) {
                            var containFilter = { "match_phrase": {} };
                            containFilter.match_phrase[field] = matchContainTextArr[index];
                            shouldFilters.push(containFilter);
                        }
                    }
                }

            }
            else if (operator == "notcontain") {
                var notcontainFilter = { "match": {} };
                notcontainFilter.match[field] = advanceConditions[i].matchtext1;
                mustNotFilters.push(notcontainFilter);
            }
            else if (operator == ">=") {
                var gteFilter = { "range": {} };
                var matchText = advanceConditions[i].matchtext1;
                if (advanceConditions[i].ctype == "date")
                    matchText = moment(new Date(matchText)).toISOString();
                gteFilter.range[field + ".keyword"] = { "gte": matchText };
                mustFilters.push(gteFilter);
            }
            else if (operator == "<=") {
                var lteFilter = { "range": {} };
                var matchText = advanceConditions[i].matchtext1;
                if (advanceConditions[i].ctype == "date")
                    matchText = moment(new Date(matchText)).toISOString();
                lteFilter.range[field + ".keyword"] = { "lte": matchText };
                mustFilters.push(lteFilter);
            }
            else if (operator == "between") {
                var betweenFilter = { "range": {} };
                var matchText1 = advanceConditions[i].matchtext1;
                var matchText2 = advanceConditions[i].matchtext2;
                if (advanceConditions[i].ctype == "date") {
                    matchText1 = moment(new Date(matchText1)).toISOString();
                    matchText2 = moment(new Date(matchText2)).toISOString();
                }
                betweenFilter.range[field] = {
                    "gte": matchText1,
                    "lte": matchText2
                };
                mustFilters.push(betweenFilter);
            }
        }
        //term 加keyword
     
        if (mustNotFilters.length > 0) {
            if (!boolFilter.must_not)
                boolFilter.must_not = [];
            boolFilter.must_not = boolFilter.must_not.concat(mustNotFilters);
        }
        if (mustFilters.length > 0) {
            if (!boolFilter.must)
                boolFilter.must = [];
            boolFilter.must = boolFilter.must.concat(mustFilters);
        }
        if (shouldFilters.length > 0)
        {
            if (!boolFilter.should)
                boolFilter.should = [];
            boolFilter.should = boolFilter.should.concat(shouldFilters);
        }
    },

    /**
    * 构造查询条件 https://elasticsearch.cn/book/elasticsearch_definitive_guide_2.x/combining-filters.html
    * @param {} metadataItem 
    * @returns {} 
    */
    buildSearchFilter: function(paramInfos) {
        var searchText = paramInfos.searchText;
        var filter = {"query":{"bool":{}}};
        var postFilters = {"bool":{"must":[]}};
        //查询
        //数据来源精确匹配
        if (paramInfos.sourceName && paramInfos.sourceName !== "全部") {
            if (!filter.query.bool.must)
                filter.query.bool.must = [];
            filter.query.bool.must.push({ "term": { "source.name.keyword": paramInfos.sourceName } });
        }
        //左侧查询条件匹配
        if (paramInfos.dimension) {
            //todo
            var dimensionValue = paramInfos[paramInfos.dimension];
            if (dimensionValue) {
                //if (paramInfos.dimension === "producttype") {
                //    mustParams.push({ "term": { "ep.producttype.keyword": dimensionValue } });
                //}
                //if (paramInfos.dimension === "creator") {
                //    mustParams.push({ "term": { "dc.contributor.name.keyword": dimensionValue } });
                //}
                //左侧分类作为后置查询条件【查询结果不能影响左侧数据】
                //即输入框输入后，只影响查询结果列表，不影响聚合结果
                //post_filter是一个顶层元素，只会对搜索结果进行过滤
                if (paramInfos.dimension === "producttype") {
                    postFilters.bool.must.push({
                        "term": { "ep.producttype.keyword": dimensionValue }
                    });
                }
                if (paramInfos.dimension === "creator") {
                    postFilters.bool.must.push({
                        "term": { "dc.contributor.name.keyword": dimensionValue }
                    });
                }
            }
        }
        //时间范围过滤
        // if (paramInfos.dimension === "recentdatecode") {
        if (paramInfos.recentDateRange && paramInfos.recentDateRange.startDate) {

            var startDate = paramInfos.recentDateRange.startDate.format("yyyy-MM-ddThh:mm:ss");
            var endDate = paramInfos.recentDateRange.endDate.format("yyyy-MM-ddThh:mm:ss");
            //todo:创建时间字段dc.data.value 过滤无效，暂用indexeddate过滤
            //等元数据定义修改后，修改下方的indexeddate
            //时间过滤也作为post_filter过滤
            var dateRangeFilter = {
                "range":
                {
                    "indexeddate": { "gte": startDate, "lte": endDate }
                }
            };
            postFilters.bool.must.push(dateRangeFilter);
        }
        if (postFilters.bool.must.length > 0)
            filter.post_filter = postFilters;
        //文本匹配
       
        var isAdvanceSearch = this.getPageParam("isadvance");
        if (isAdvanceSearch == "true" || isAdvanceSearch == "1") {
            //高级搜索
            this.buildAdvanceSearchFilter(filter.query.bool);
        } else {
            var searchFields = [];
            if (!paramInfos.seachFields) {
                searchFields = ["dc.title.text", "dc.description.text"];
            }
            var boolShouldParams = [];
            for (var i = 0; i < searchFields.length; i++) {
                var paramName = searchFields[i];
                var matchParam = {};
                matchParam[paramName] = searchText;
                boolShouldParams.push({
                    "match": matchParam
                });
            }
            filter.query.bool = { "should": boolShouldParams };
        }

        //聚合
        var aggFields = [];
        var aggFilter = {};
        if (!paramInfos.aggsFields) {
            aggFields = [
                { "name": "productTypes", "field": "ep.producttype" },
                { "name": "creators", "field": "dc.contributor.name" }
            ];
            //{ "name": "createdDates", "field": "dc.date.value" }  时间无法聚合
        }
        for (var j = 0; j < aggFields.length; j++) {
            var aggName = aggFields[j].name;
            aggFilter[aggName] = { "terms": { "field": aggFields[j].field+".keyword" } };
        }
        filter.aggs = aggFilter;

        //分页
        var pageInfo = paramInfos.pageInfo;
        if (pageInfo) {
            filter.from = (pageInfo.pageIndex - 1) * pageInfo.pageSize;
            filter.size = pageInfo.pageSize;
        }
        return filter;
    },
    buildSearchFilter2: function (paramInfos) {
        var searchuiconfig = paramInfos.searchuiconfig;
        var searchText = paramInfos.w;
        var filter = { "query": { "bool": {} } };
        var postFilters = { "bool": { "must": [] } };
        //查询
        //数据来源精确匹配
        if (paramInfos.sourcename && paramInfos.sourcename !== "全部") {
            if (!filter.query.bool.must)
                filter.query.bool.must = [];
            var sourcnamefield = paramInfos.sourcenamefield;
            var termQuery = { "term": {} };
            termQuery["term"][sourcnamefield + ".keyword"] = paramInfos.sourcename;
            filter.query.bool.must.push(termQuery);

        }
        //左侧查询条件匹配
        if (paramInfos.dimension) {
            var dimensionValue = paramInfos[paramInfos.dimension];
            if (dimensionValue) {
                //左侧分类作为后置查询条件【查询结果不能影响左侧数据】
                //即输入框输入后，只影响查询结果列表，不影响聚合结果
                //post_filter是一个顶层元素，只会对搜索结果进行过滤
                var leftSiders = searchuiconfig.leftsideconfig.leftsiders;
                for (var leftsiderindex = 0; leftsiderindex < leftSiders.length; leftsiderindex++) {
                    if (leftSiders[leftsiderindex].field == paramInfos.dimension) {
                        var sideTermQuery = { "term": {} };
                        sideTermQuery["term"][paramInfos.dimension+".keyword"] = dimensionValue;
                        postFilters.bool.must.push(sideTermQuery);
                        break;
                    }
                        
                }
            }
        }
        //时间范围过滤
        // if (paramInfos.dimension === "recentdatecode") {
        if (paramInfos.recentDateRange && paramInfos.recentDateRange.startDate) {
            var startDate = moment(paramInfos.recentDateRange.startDate).toISOString();
            var endDate = moment(paramInfos.recentDateRange.endDate).toISOString(); 
           
            var dateRangeFilter = {"range":{}};
            var dataFilterField = searchuiconfig.leftsideconfig.datesider.field;
            dateRangeFilter["range"][dataFilterField] = { "gte": startDate, "lte": endDate }
            postFilters.bool.must.push(dateRangeFilter);
        }
        if (postFilters.bool.must.length > 0)
            filter.post_filter = postFilters;

        //文本匹配
        var isAdvanceSearch = this.getPageParam("isadvance");
        if (isAdvanceSearch == "true" || isAdvanceSearch == "1") {
            //高级搜索
            this.buildAdvanceSearchFilter(filter.query.bool);
        } else {
            if (searchuiconfig.searchlistconfig.isfulltext) {
                //全文匹配
                if (searchText) {
                    var querystringQuery = {
                        "query_string": {
                            "query": searchText,
                            "use_dis_max": false
                        }
                    };
                    if (filter.query.bool.must == null)
                        filter.query.bool.must = [];
                    filter.query.bool.must.push(querystringQuery);
                }
               
            } else {
                if (searchText)
                {
                    var searchFields = searchuiconfig.searchlistconfig.searchfields;
                    if (!searchFields) {
                        searchFields = ["title", "abstract"];
                    }
                    var boolShouldParams = [];
                    var searchPhrases = this.getSearchWordsBySegment(searchText);
                    for (var i = 0; i < searchFields.length; i++) {
                        var paramName = searchFields[i];
                        for (var j = 0; j < searchPhrases.length; j++) {
                            var matchParam = {};
                            if (paramName == "title") {
                                matchParam[paramName] = { "query": searchPhrases[j], "boost": 3 };
                                boolShouldParams.push({
                                    "match": matchParam
                                });
                                continue;
                            }
                            matchParam[paramName] = searchPhrases[j];
                            boolShouldParams.push({
                                "match": matchParam
                            });
                        }
                    }
                    if (!filter.query.bool.must)
                        filter.query.bool.must = [];
                    filter.query.bool.must.push({ "bool": { "should": boolShouldParams } });

                }              
            }
        }
        //聚合
        var aggFilter = {};
        var leftSiderAggs = searchuiconfig.leftsideconfig.leftsiders;
        for (var leftsiderAggindex = 0; leftsiderAggindex < leftSiderAggs.length; leftsiderAggindex++) {
            var aggName = leftSiderAggs[leftsiderAggindex].title;
            var aggSize = leftSiderAggs[leftsiderAggindex].top ? leftSiderAggs[leftsiderAggindex].top : 10;
            aggFilter[aggName] = { "terms": { "field": leftSiderAggs[leftsiderAggindex].field + ".keyword", "size": aggSize } };

        }
        filter.aggs = aggFilter;
        //分页
        var pageInfo = paramInfos.pageInfo;
        if (pageInfo) {
            filter.from = (pageInfo.pageIndex - 1) * pageInfo.pageSize;
            filter.size = pageInfo.pageSize;
        }
        //字段排序【非分值排序】
        var fieldOrders = this.getPageParam("order");
        if (fieldOrders)
        {
            this.buildOrderFilter(filter);
        }
        //filter.query.bool.minimum_should_match = 1;
        return filter;
    },
    buildOrderFilter: function (filter) {
        var fieldOrders = this.getPageParam("order");
        if (!filter.sort)
            filter.sort = [];
        var fieldOrdersArr = fieldOrders.split(',');
        for (var i = 0; i < fieldOrdersArr.length; i++) {
            var field = fieldOrdersArr[i].split('$')[0];
            var ascMode = "asc";
            if (fieldOrdersArr[i].split('$').length > 1)
                ascMode = fieldOrdersArr[i].split('$')[1];
            var sort = {};
            sort[field] = { "order": ascMode };
            filter.sort.push(sort);
        }
    },
    /**
     * 根据查询条件从ES搜索数据
     * @param {} metadataItem 
     * @returns {} 
     */
    getSearchDataList: function(paramInfos) {
        var filter = this.buildSearchFilter2(paramInfos);
        var startTime = new Date().getTime();
        var result = this.searchResult(filter);
        result.executionTime = (new Date().getTime() - startTime);//单位：毫秒
       
        if (result.success) {
            result.data = this.convertToResultItem(result.data);
        }
        return result;
    },
    /**
     * 获取热词
     * @param {} 靠前多少个 
     * @returns {} 
     */
    getTopHotWords:function(succCallback) {
        var searchService = new SearchHistoryService();
        searchService.getTopHotWords(succCallback, 5);
    },
    /**
     * 获取BO分词后重新组装的文本
     * @returns {} 
     */
    getSearchWordsBySegment: function (text) {
        if (this.segmentWords)
            return this.segmentWords;
        if (!text)
            return [];
        var aliablePhrases = this.getAliablePhrases(text);
        var length = aliablePhrases.length;
        if (length == 0)
            return [text];
        function getSegTexts(preSegTexts, aliablePhrase) {
            var result = [];
            for (var j = 0; j < aliablePhrase.aliases.length; j++) {
                for (var k = 0; k < preSegTexts.length; k++) {
                    var reg = new RegExp(aliablePhrase.term, "gi");
                    var newText = preSegTexts[k].replace(reg, aliablePhrase.aliases[j]);
                    result.push(newText);
                }
            }
            return result;
        }

        var preSegTexts = [text];
        for (var i = 0; i < aliablePhrases.length; i++) {
            preSegTexts = getSegTexts(preSegTexts,aliablePhrases[i]);
        }
        if (aliablePhrases.length > 0) {
            preSegTexts.push(text);
        }
        this.segmentWords = preSegTexts;
        return preSegTexts;
    },
    getAliablePhrases: function(text) {
        var phrases = this.getWordsPhrase(text);
        var aliablePhrases = [];
        for (var i = 0; i < phrases.length; i++) {
            if (phrases[i].aliases.length > 0)
                aliablePhrases.push({ "term": phrases[i].term, "aliases": phrases[i].aliases });
        }
        return aliablePhrases;
    },
    getAdvanceConfig:function() {
       // articleClassiy=title&accuracy=M&text=q&idOr=and&articleClassiy=title&accuracy=M&text=q&idOr=and&articleClassiy=title&accuracy=M&text=q&idOr=and
        var query = decodeURI(location.search);
        if (query.indexOf('?') == 0)
            query = query.substring(1);
        var paramArray = query.split('&');

        var conditions = [];

        var condition = {};
        for (var i = 0; i < paramArray.length; i++) {
            var conditionArray = paramArray[i].split('=');
            if (conditionArray[0] == "articleClassiy" ||
                conditionArray[0] == "accuracy" ||
                conditionArray[0] == "text" || conditionArray[0] == "idOr") {
                condition[conditionArray[0]] = conditionArray[1];
                if (conditionArray[0] == "idOr") {
                    conditions.push($.extend(true, {}, condition));
                }
            }
            else
                continue;
        }
        return conditions;
    },
    /**
     * 获取高级搜索查询条件
     * @returns {} 
     */
    getAdvanceConditions: function () {
        // articleClassiy=title&accuracy=M&text=q&idOr=and&articleClassiy=title&accuracy=M&text=q&idOr=and&articleClassiy=title&accuracy=M&text=q&idOr=and
        var query = decodeURI(location.search);
        if (query.indexOf('?') == 0)
            query = query.substring(1);
        var paramArray = query.split('&');

       

        var condition = {};
        var params = [];
        for (var i = 0; i < paramArray.length; i++) {
            var conditionArray = paramArray[i].split('=');
            if (conditionArray[0] == "ischecked" ||
                conditionArray[0] == "field" ||
                conditionArray[0] == "operator" ||
                conditionArray[0] == "matchtext1" ||
                conditionArray[0] == "matchtext2") {
                params.push(conditionArray);
            }
            else
                continue;
        }
        var conditions = [];
        for (var j = 0; j < params.length; j++) {
            condition[params[j][0]] = params[j].length>1?params[j][1]:"";
            //如果下一个参数为ischecked,则加入集合
            if (j < params.length - 1) {
                if (params[j+1][0] == "ischecked") {
                    conditions.push($.extend(true, {}, condition));
                    condition = {};
                }
            } else {
                conditions.push($.extend(true, {}, condition));
            }
        }
        return conditions;
    }
});
di.register("SearchResult", SearchResultService);