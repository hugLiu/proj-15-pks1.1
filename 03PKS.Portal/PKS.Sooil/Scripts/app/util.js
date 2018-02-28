
//截取字符串 包含中文处理
//(串,长度,增加...)
function subString(str, len, hasDot) {
    var newLength = 0;
    var newStr = "";
    var chineseRegex = /[^\x00-\xff]/g;
    var singleChar = "";
    var strLength = str.replace(chineseRegex, "**").length;
    for (var i = 0; i < strLength; i++) {
        singleChar = str.charAt(i).toString();
        if (singleChar.match(chineseRegex) != null) {
            newLength += 2;
        }
        else {
            newLength++;
        }
        if (newLength > len) {
            break;
        }
        newStr += singleChar;
    }

    if (hasDot && strLength > len) {
        newStr += "...";
    }
    return newStr;
}
//高亮
function HighlightString() {
    this.KeyWords = null;
    this.CssBegin = "<font style='color:red'>";
    this.CssEnd = "</font>";
    this.Text = null;

    this.formatKeyword = function (content, keyword) {
        keyword = keyword.replace(/(^\s*)|(\s*$)/g, "");
        if (keyword == '')
            return content;
        var reg = new RegExp('(' + keyword + ')', 'gi');
        return content.replace(reg, this.CssBegin + '$1' + this.CssEnd);
    }

    this.refreshContent = function (contentID) {

        var content;
        if (this.Text == null)
            content = document.getElementById(contentID).innerHTML;
        else
            content = this.Text;

        for (var i = 0; i < this.KeyWords.length; i++) {
            var strKey = this.KeyWords[i].toString();
            var arrKey = strKey.split(',');
            for (var j = 0; j < arrKey.length; j++) {
                var key = arrKey[j];
                content = this.formatKeyword(content, key);
            }
        }
        if (this.Text == null) {
            document.getElementById(contentID).innerHTML = content;
            return null;
        } else
            return content;
    }
};

//高亮截断方法
function HighlightTextInterception(text, count) {
    var strrep = text.replace(/<font style='color:red'>/g, "「").replace(/<\/font>/g, "」");
    if (text.length <= count) return text;
    var str = strrep.substr(0, count);
    var returnstr = str.replace(/「/g, "<font style='color:red'>").replace(/」/g, "</font>");
    for (var i = str.length; i >= 0; i--) {
        if (str[i] == "」") {
            returnstr += " ...";
            break;
        }
        if (str[i] == "「") {
            returnstr += "</font> ...";
            break;
        }
    }
    return returnstr;
};


function ESHighlightToList(text) {
    if (text == "" || text == null) return null;
    var list = [];
    try {
        var s = text.split(',');
        $.each(s, function () {
            var ss = this.split(' : ');
            list.push(ss[0].split('.')[1]);
            list.push(ss[1]);
        });
    } catch (e) {
    }
    return list;
};

function HighlightByType(text, title, type) {
    try {
        var list = ESHighlightToList(text);
        var str;
        if (list != null) {
            var s1 = title.split(',');
            var s2 = [];
            $.each(list, function (i) {
                if (this == type)
                    s2.push(list[i + 1]);
            });

            $.each(s2, function (i, item2) {
                $.each(s1, function (j, item1) {
                    if (item2.replace(/<font style='color:red'>/g, "").replace(/<\/font>/g, "") == item1) {
                        s1[j] = s2[i];
                    };
                });
            });

            $.each(s1, function () {
                if (!str)
                    str = this;
                else
                    str += "，" + this;
            });
        }
        if (!str) {
            str = title;
        }
        //return str.length > 60 ? (str.substring(0, 60) + ' ...') : str;
        if (str) {
            //处理页面最终数据的额外高亮，目前只有单字高亮。
            if (highlightKey) {
                var hl = new HighlightString();
                hl.KeyWords = highlightKey;
                hl.Text = str;
                str = hl.refreshContent("");
            }

            if (str.replace(/<font style='color:red'>/g, "").replace(/<\/font>/g, "").length > 45) return HighlightTextInterception(str, 45);
            else return str;
        } else return "";

    } catch (err) {
        //WebAppToastr("error",err.name + " - " + err.message);
        return title == null ? "" : title;
    }
};


String.format = function () {
    if (arguments.length == 0)
        return null;
    var str = arguments[0];
    for (var i = 1; i < arguments.length; i++) {
        var re = new RegExp('\\{' + (i - 1) + '\\}', 'gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
};

$.util = {

}
$.util.getPageParams = function () {
    var pageParams = {}
    var href = location.search;
    var queryParams = href.split('&');
    for (var i = 0; i < queryParams.length; i++) {
        if (!queryParams[i] || queryParams[i].length === 1)
            continue;
        if (queryParams[i].indexOf("?") === 0) {
            queryParams[i] = queryParams[i].substring(1);
        }
        var paramArr = queryParams[i].split('=');
        pageParams[paramArr[0].toLowerCase()] = paramArr.length > 1 ? paramArr[1] : '';
    }
    return pageParams;
}
$.util.getPageParam = function (paramName) {
    if (!paramName)
        return '';
    var paramValue = $.util.pageParams[paramName.toLowerCase()];
    return paramValue ? decodeURI(paramValue) : "";
}

$.util.pageParams = $.util.pageParams ? $.util.pageParams : $.util.getPageParams();

$.util.clone = function (data) {
    if (!data)
        return data;
    if (data instanceof Array) {
        var resultData = [];
        for (var j = 0; j < data.length; j++) {
            if (data[j]) {
                if (data[j].constructor.name == "Object") {
                    resultData.push($.extend(true, {}, data[j]));
                }
                else
                    resultData.push(data[j]);
            } else
                resultData.push(data[j]);
        }
        return resultData;
    }
    else if (data.constructor.name == "Object") {
        return $.extend(true, {}, data);
    } else return data;

}

//[Start] url帮助
$.url = {}
$.url.getParam = function (url, paramName) {
    if (!paramName)
        return null;
    if (!url || url == "?" || url.indexOf('?') == -1)
        return null;
    var searchQuery = url.substring(url.indexOf('?') + 1);
    var queryParams = searchQuery.split('&');
    var urlParam = null;
    for (var i = 0; i < queryParams.length; i++) {
        if (!queryParams[i] || queryParams[i].length === 1)
            continue;
        if (queryParams[i].indexOf("?") === 0) {
            queryParams[i] = queryParams[i].substring(1);
        }
        var paramArr = queryParams[i].split('=');
        if (paramArr[0].toLowerCase() == paramName.toLowerCase()) {
            urlParam = paramArr.length > 1 ? paramArr[1] : '';
            break;
        }
    }
    return urlParam;
}

$.url.concat = function (url, params) {
    if (!params)
        return url;
    if (url.indexOf('?') == -1) {
        if (!$.param(params))
            return url;
        return url + "?" + $.param(params);
    }

    var searchQuery = url.split('?')[1];
    var queryParams = searchQuery.split('&');
    var allParams = [];
    for (var i = 0; i < queryParams.length; i++) {
        if (!queryParams[i] || queryParams[i].length === 1)
            continue;
        if (queryParams[i].indexOf("?") === 0) {
            queryParams[i] = queryParams[i].substring(1);
        }
        var paramArr = queryParams[i].split('=');
        var paramVal = paramArr.length > 1 ? paramArr[1] : '';
        allParams.push({ "name": paramArr[0], "value": paramVal });
    }

    for (var item in params) {
        var hasParam = false;
        for (var j = 0; j < allParams.length; j++) {
            if (allParams[j].name == item.toLowerCase()) {
                hasParam = true;
                allParams[j].value = params[item];
            }
        }
        if (!hasParam) {
            allParams.push({ "name": item, "value": params[item] });
        }
    }
    var newParams = [];
    for (var k = 0; k < allParams.length; k++) {
        newParams.push(allParams[k].name + "=" + allParams[k].value);
    }
    return url.split('?')[0] + "?" + newParams.join('&');
}
/**
 * 
 * @param {} idVal 
 * @param {} keyword 
 * @returns {} 
 */
function highlight(text, keyword) {
    if (!text)
        return '';
    var words = unescape(keyword.replace(/\+/g, ' '));
    var temp = text;
    var tag = 'font';
    var i, len = words.length, re;
    for (i = 0; i < len; i++) {
        if (!words[i] || words[i] == ' ')
            continue;
        // 正则匹配所有的文本
        re = new RegExp(words[i], 'g');
        if (re.test(temp)) {
            temp = temp.replace(re, '<' + tag + ' color="red">$&</' + tag + '>');
        }
    }
    return temp;
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
        RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

//高亮文本
function highlight2(content, keyword) {
    var cssBegin = "<font style='color:red'>";
    var cssEnd = '</font>';
    var regKeywords = '(';
    for (var i = 0; i < keyword.length; i++) {
        if (i > 0) regKeywords += '|';
        regKeywords += keyword[i].trim().replace('|', '\|');
    }
    regKeywords += ')';
    var regex = new RegExp(regKeywords, 'gi');
    return content.replace(regex, cssBegin + '$1' + cssEnd);
};
