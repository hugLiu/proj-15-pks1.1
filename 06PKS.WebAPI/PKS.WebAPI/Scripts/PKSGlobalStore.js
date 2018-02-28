var pksGlobalStore = {
    //配置
    config: {
        //API网站
        apiSiteUrl: "",
        //API服务
        apiServiceUrl: "",
        //公共页面url
        portalRenderUrl: "http://localhost:8082/Render/Content?iiid="
    },
    //规范化URL
    normalizeUrl: function (url) {
        var url2 = url;
        if (url2.length > 0) {
            if (url2[url2.length - 1] !== '/') url2 += '/';
        }
        return url2;
    },
    //初始化
    init: function (apiSiteUrl) {
        this.config.apiSiteUrl = this.normalizeUrl(apiSiteUrl);
        this.config.apiServiceUrl = this.config.apiSiteUrl + "api/";
        this.setJQueryAjaxSetup();
        this.initConsole();
        this.logService.init(this, this.config.apiServiceUrl);
    },
    //jQuery全局ajax配置
    setJQueryAjaxSetup: function () {
        if (!window.$) return;
        $.ajaxSetup({
            xhrFields: {
                withCredentials: true
            }
        });
    },
    //初始化控制台
    initConsole: function () {
        if (window.console) return;
        window.console = {
            log: function (message) {
            }
        }
    },
    //AJAX的POST方法
    ajaxPost: function (url, data, success) {
        var store = pksGlobalStore;
        if ($ && $.ajax) {
            var ajaxOption = { type: "post", url: url, success: success };
            ajaxOption.contentType = "application/json; charset=UTF-8";
            ajaxOption.data = JSON.stringify(data);
            $.ajax(ajaxOption);
        } else if (PKSUI && PKSUI.http) {
            PKSUI.http.post(url, data)
                .then(function (response) {
                    success && success(response.body);
                });
        }
    },
    //获得元素标题
    getElementTitle: function (element) {
        var title = element.tagName;
        if (element.id) {
            title += "#" + element.id;
        }
        if (element.className) {
            title += "." + element.className;
        }
        return title;
    },
    //转换为Moment日期
    toMomentDate: function (value) {
        if (value == undefined) return;
        if (typeof value === "string") {
            if (value.length == 0) return;
            value = value.replace(/\//g, "-");
            return moment(value);
        }
        if (moment.isDate(value)) {
            return moment(value);
        }
        return;
    },
    //日志服务
    logService: {
        //数据
        state: {
            //父引用
            store: null,
            //API服务
            apiServiceUrl: ""
        },
        //记录异常信息
        logError: function (logData) {
            var url = this.state.apiServiceUrl + "Log";
            this.state.store.ajaxPost(url, logData);
        },
        //初始化
        init: function (store, apiServiceUrl) {
            this.state.store = store;
            this.state.apiServiceUrl = apiServiceUrl + "LogService/";
            this.registerErrorEvent();
        },
        //注册错误事件
        registerErrorEvent: function () {
            //if (console) console.log("注册window错误事件处理器");
            if (window.addEventListener) {
                window.addEventListener("error", this.onError, true);
            } else if (window.attachEvent) {
                window.attachEvent("onerror", this.onError);
            }
        },
        //记录异常信息
        onError: function (errorEvent) {
            var error = errorEvent.error;
            if (error == null || error.stack == null) return;
            var store = pksGlobalStore;
            var logData = {};
            logData.logLevel = "Error";
            logData.message = errorEvent.message;
            logData.request = window.location.href;
            if (window.navigator.userAgent) {
                logData.request += "\r\nUserAgent:" + window.navigator.userAgent;
            }
            if (window.navigator.origin) {
                logData.request += "\r\nReferrer:" + window.navigator.origin;
            } else if (document.referrer) {
                logData.request += "\r\nReferrer:" + document.referrer;
            }
            //logData.exSource = "";
            logData.exContent = error.stack;
            if (errorEvent.path && errorEvent.path.length > 0) {
                logData.exData = "";
                for (var i = errorEvent.path.length - 1; i >= 0; i--) {
                    var title = store.getElementTitle(errorEvent.path[i]);
                    if (!title) continue;
                    logData.exData += title;
                    if (i > 0) logData.exData += " > ";
                }
            }
            store.logService.logError(logData);
        }
    },
    //根据bo，pt,date进入展示页面
    goToDetailService: {
        getquery: function (pt, well, date) {
            debugger;
            date = pksGlobalStore.goToDetailService.dateswitch(date);
            var query = {
                "_source": [
                    "pt",
                    "iiid",
                    "dataid"
                ],
                "query": {
                    "bool": {
                        "must": [
                            {
                                "match": {
                                    "pt.keyword": pt
                                }
                            },
                            {
                                "match": {
                                    "well.keyword": well
                                }
                            },
                            {
                                "prefix": {
                                    "period": {
                                        "value": date
                                    }
                                }
                            }
                        ]
                    }
                },
                "size": 1,
                "sort": [
                    {
                        "indexeddate": {
                            "order": "desc"
                        }
                    }
                ]
            };
            return query;
        },
        getdataid: function (query) {
            var dataid = "";
            $.ajax({
                url: pksGlobalStore.config.apiServiceUrl + "SearchService/ESSearch",
                type: "post",
                async: false,
                data: JSON.stringify(query),
                success: function (result) {
                    dataid = result.hits.hits[0]._source.iiid;
                },
                error: function () {
                    debugger;
                }
            });
            return dataid;
        },
        gotodetailpage: function (pt, well, date) {
            var query = pksGlobalStore.goToDetailService.getquery(pt, well, date);
            var dataid = pksGlobalStore.goToDetailService.getdataid(query);
            debugger;
            open(pksGlobalStore.config.portalRenderUrl + "" + dataid, "_blank");
        },
        dateswitch: function (dateParms) {
            var datetime;
            if (dateParms instanceof Date) {
                datetime = dateParms;
            }
            //判断是否为字符串
            if ((typeof dateParms === "string") && dateParms.constructor === String) {
                //将字符串日期转换为日期格式
                datetime = new Date(Date.parse(dateParms.replace(/-/g, "/")));
            }
            //获取年月日时分秒
            var year = datetime.getFullYear();
            var month = datetime.getMonth() + 1;
            var date = datetime.getDate();
            var hour = datetime.getHours();
            var minutes = datetime.getMinutes();
            var second = datetime.getSeconds();
            //月，日，时，分，秒 小于10时，补0
            if (month < 10) {
                month = "0" + month;
            }
            if (date < 10) {
                date = "0" + date;
            }
            if (hour < 10) {
                hour = "0" + hour;
            }
            if (minutes < 10) {
                minutes = "0" + minutes;
            }
            if (second < 10) {
                second = "0" + second;
            }
            //拼接日期格式【例如：yyyymmdd】
            return year + month + date;
        }
    }
};
