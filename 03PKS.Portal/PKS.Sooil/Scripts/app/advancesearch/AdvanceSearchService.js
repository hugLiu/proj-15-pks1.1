
var AdvanceSearchService = BaseService.extend({
    init: function () {
        this._super();
    },
    /** 获取配置
    * @returns {} 
    */
    getConfig: function () {
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
    getAdvanceSearchSetting2:function() {
        var query = decodeURI(parent.location.search);
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
        var fields = [];
        for (var j = 0; j < params.length; j++) {

            condition[params[j][0]] = params[j].length > 1 ? params[j][1].replace("+", '') : "";//将+号替换成空格，点搜索时，值中有空格的被自动替换为+
            //如果下一个参数为ischecked,则加入集合
            if (j < params.length - 1) {
                if (params[j + 1][0] == "ischecked") {
                    fields.push(condition["field"]);
                    conditions.push($.extend(true, {}, condition));
                    condition = {};
                }
            } else {
                fields.push(condition["field"]);
                conditions.push($.extend(true, {}, condition));
            }
        }
        //
        var metadataConditions = [];
        var defaultConditions = this.getDefaultConditions();
        for (var k = 0; k < defaultConditions.length; k++) {
            var metadataCondition = $.extend(true, {"ischecked":false}, defaultConditions[k]);
            var name = metadataCondition.name;
            if (metadataCondition.ctype == "select") {
                var dataSource = this.getAggregation(name);
                metadataCondition.datasource = dataSource;
                if (dataSource && dataSource.length > 0) {
                    metadataCondition.matchtext1 = dataSource[0].text;
                }
            }
            for (var l = 0; l < conditions.length; l++) {

                if (name == conditions[l].field || (metadataCondition.searchfields.indexOf(conditions[l].field)>-1)) {
                    metadataCondition.ischecked = conditions[l].ischecked;
                    metadataCondition.selectedoperator = conditions[l].operator;
                   
                    metadataCondition.matchtext1 = conditions[l].matchtext1;
                    if (conditions[l].matchtext2) {
                        metadataCondition.matchtext2 = conditions[l].matchtext2;
                    }
                    
                    break;
                }
            }

            metadataConditions.push(metadataCondition);
        }
        if (conditions.length == 0)
            metadataConditions[0].ischecked = true;
        return metadataConditions;
    },
    getAdvanceSearchSetting:function() {
        var query = decodeURI(parent.location.search);
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
        var fields = [];
        for (var j = 0; j < params.length; j++) {
            
            condition[params[j][0]] = params[j].length > 1 ? params[j][1].replace("+",'') : "";//将+号替换成空格，点搜索时，值中有空格的被自动替换为+
            //如果下一个参数为ischecked,则加入集合
            if (j < params.length - 1) {
                if (params[j + 1][0] == "ischecked") {
                    fields.push(condition["field"]);
                    conditions.push($.extend(true, {}, condition));
                    condition = {};
                }
            } else {
                fields.push(condition["field"]);
                conditions.push($.extend(true, {}, condition));
            }
        }

        var allMetadataOperators = this.getAllMetadataOperators();
        var titleMetadataOperator = {};
        for (var j = 0; j < allMetadataOperators.length; j++) {
            if (allMetadataOperators[j].name.toLowerCase() == "title") {
                allMetadataOperators[j].isadded = true;
                titleMetadataOperator = allMetadataOperators[j];
                break;
            }
        }
        var metadataOperators = [];
        if (conditions.length == 0) {
            //如果无高级搜索设置，默认标题
            metadataOperators.push(titleMetadataOperator);
        }
        else {
            //获取所有高级搜索字段相关元数据
            for (var l = 0; l < conditions.length; l++) {
                for (var k = 0; k < allMetadataOperators.length; k++) {
                    if (conditions[l].field == allMetadataOperators[k].name) {
                        metadataOperators.push(allMetadataOperators[k]);
                        allMetadataOperators[k].isadded = true;
                        allMetadataOperators[k].selectedoperator = conditions[l].operator;
                        if (conditions[l].matchtext1) {
                            allMetadataOperators[k].matchtext1 = conditions[l].matchtext1;
                        }
                        if (conditions[l].matchtext2) {
                            allMetadataOperators[k].matchtext2 = conditions[l].matchtext2;
                        }
                        break;
                    }
                }
            }
           
            //将title调至第一个
            if (fields.indexOf("Title") == -1 &&fields.indexOf("title") == -1) {
                titleMetadataOperator.ischecked = false;
                metadataOperators.splice(0, 0, titleMetadataOperator);
            }
        }
        return metadataOperators;
    },
    /**
     * 获取元数据定义
     * @returns {} 
     */
    getMetadataDefinitions: function () {
        var metadataDeinition = {};
        var metadataSearchUrl = this.apiServiceUrl + "/searchservice/GetMetadataDefinitions";
        //var token = this.getToken();
        //var authorization = { "Authorization": "Bear " + token };
        $.ajax({
            async: false,
            url: metadataSearchUrl,
            crossDomain: true,
            //headers: authorization,
            dataType: 'json',
            cache: false,
            beforeSend: function () {

            },
            success: function (data) {
                metadataDeinition = data;
            },
            error: function () {
               // result.error = arguments;
            }
        });
        var result = [];
        for (var item in metadataDeinition) {
            if (metadataDeinition[item])
                result.push(metadataDeinition[item]);
        }
        return result;
    },
    getDefaultConditions:function() {
        var config = this.getConfig();
        return config.defaultconditions;
    },
    /**
     * 获取元数据及各元数据对应运算符、数据源
     * @returns {} 
     */
    getAllMetadataOperators: function () {
        if (this.allMetadataOperators)
            return this.allMetadataOperators;
        var metadataDefinitions = this.getMetadataDefinitions();
        var allOperators = this.getAllOperators();
        var metadataOperators = [];
        for (var i = 0; i < metadataDefinitions.length; i++) {
            var metadataOperator = $.extend(true, {isadded:false, ischecked: "checked", operators: [], datasource: [], matchtext1: "", matchtext2: "" }, metadataDefinitions[i]);
            //todo 改为一次请求取回所有聚合
            var uitype = metadataDefinitions[i].uitype;
            if (uitype == "Image")
                continue;
            var datatype = metadataDefinitions[i].type;
            if (uitype == "DropdownList" || uitype == "List") {
                metadataOperator.selectedoperator = "equal";
                metadataOperator.ctype = "select";//数据范围控件类型
                metadataOperator.operators.push(allOperators["equal"]);
                metadataOperator.operators.push(allOperators["notequal"]);
                var dataSource = this.getAggregation(metadataDefinitions[i].name);
                metadataOperator.datasource = dataSource;
                if (dataSource && dataSource.length > 0) {
                    metadataOperator.matchtext1 = dataSource[0].text;
                }
                metadataOperators.push(metadataOperator);
            }
            else if (uitype == "Date") {

                metadataOperator.selectedoperator = "equal";
                metadataOperator.ctype = "date"; //数据范围控件类型
                metadataOperator.operators.push(allOperators["equal"]);
                metadataOperator.operators.push(allOperators["gte"]);
                metadataOperator.operators.push(allOperators["lte"]);
                metadataOperator.operators.push(allOperators["between"]);
                metadataOperator.operators.push(allOperators["all"]);
                metadataOperator.datasource = null;
                metadataOperators.push(metadataOperator);
            } else {
                metadataOperator.selectedoperator = "contain";
                metadataOperator.ctype = "text"; //文本框
                metadataOperator.operators.push(allOperators["equal"]);
                metadataOperator.operators.push(allOperators["contain"]);
                metadataOperator.operators.push(allOperators["all"]);
                metadataOperator.datasource = null;
                metadataOperators.push(metadataOperator);
            }
        }
        this.allMetadataOperators = metadataOperators;
        return metadataOperators;
    },

    getAllOperators:function() {
        var allOperators = {};
        allOperators.equal = { "name": "equal", "text": "等于","operator":"eq" };
        allOperators.notequal = { "name": "notequal", "text": "不等于","operator":"must_not" };
        allOperators.all = { "name": "all", "text": "全部" };
        allOperators.contain = { "name": "contain", "text": "包含", "operator": "match" };
        allOperators.notcontain = { "name": "notcontain", "text": "不包含", "operator": "notcontain" };
        //allOperators.notcontain = { "name": "contain", "text": "包含", "operator": "match" };
        allOperators.gte = { "name": ">=", "text": ">=", "operator": "gte" };
        allOperators.lte = { "name": "<=", "text": "<=", "operator": "lte" };
        allOperators.between = { "name": "between", "text": "介于", "operator": "between" };
        return allOperators;
    }
});