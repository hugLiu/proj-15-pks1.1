var BaseService = Class.extend({
    init: function() {
        //var esServiceUrlPrefix = "http://192.168.1.236:9200/szpks/metadata";
        //var esServiceUrlPrefix = apiServiceUrl;
        this.apiServiceUrl = $G.apiServiceUrl;
        //this.esService = new ESService(this.urlPrefix);
        this.searchUrl = $G.apiServiceUrl + "/searchservice/essearch";
    },
    getPageParam: function(paramname) {
        if (!paramname)
            return '';
        if (!$.util.pageParams[paramname.toLowerCase()])
            return '';
        return decodeURI($.util.pageParams[paramname.toLowerCase()]);
    },
    getToken: function() {
        var token = '';
        var url = '/token/gettoken';
        $.ajax({
            async: false,
            type: "post",
            url: url,
            cache: false,
            success: function(data) {
                token = data;
            },
            error: function() {
                alert('获取Token失败')
            }
        });
        return token;
    },

    searchResult: function(paramInfos) {
        //var token = this.getToken();
        //var authorization = { "Authorization": "Bear " + token };
        var result = {};
        var url = this.searchUrl;
        $.ajax({
            async: false,
            type: "post",
            url: url,
            crossDomain: true,
            data: JSON.stringify(paramInfos),
            //headers: authorization,
            dataType: 'json',
            cache: false,
            beforeSend: function() {

            },
            success: function(data) {
                result.success = true,
                    result.data = data;
            },
            error: function() {
                result.error = arguments;
            }
        });
        return result;
    },

    getAggregation: function(field, size) {
        //var token = this.getToken();
        //var authorization = { "Authorization": "Bear " + token };
        var aggregationValues = [];
        var params = {
            "size": "0",
            "aggs": {
                "sourcename": {
                    "terms": {
                        "field": field + ".keyword"
                        //"size": size
                    }
                }
            }
        }
        var url = this.searchUrl;
        $.ajax({
            async: false,
            type: "post",
            url: url,
            crossDomain: true,
            data: JSON.stringify(params),
            //headers: authorization,
            dataType: 'json',
            cache: false,
            beforeSend: function() {

            },
            success: function(data) {
                if (data && data.aggregations && data.aggregations.sourcename.buckets) {
                    var buckets = data.aggregations.sourcename.buckets;
                    for (var i = 0; i < buckets.length; i++) {
                        aggregationValues.push({ "text": buckets[i].key, "value": (i + 1).toString() });
                    }
                }
            },
            error: function() {

            }
        });
        return aggregationValues;
    },
    /** 获取输入关键字分词
    * @returns {}
    */
    getWordsPhrase: function (text) {
        var data = {
            "sentence": text,
            "option": {
                "cc": ["BO"],
                "includetrans": true,
                "includealias": true,
                "matchrule": "MaxWords"
            }
        };
        var config = [];
        var url = $G.apiServiceUrl + "/SemanticService/Segment";
        $.ajax({
            async: false,
            type:'post',
            url: url,
            dataType: "json",
            data: data,
            success: function(data) {
                config = data;
            },
            error: function() {

            }
        });
        return config;
    }
});