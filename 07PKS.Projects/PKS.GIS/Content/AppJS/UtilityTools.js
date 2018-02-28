//取URL中参数
(function ($) {
    $.getUrlParam = function (name) {
        try {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) return unescape(r[2]); return null;
        } catch (e) {

        }
    }
})(jQuery);




/*************************************************
Function:		Base64
Description:	Base64加密解密
Input:			无		
Output:			无
return:			无				
*************************************************/
var Base64 = {

    // private property
    _keyStr: "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=",

    // public method for encoding
    encode: function (input) {
        var output = "";
        var chr1, chr2, chr3, enc1, enc2, enc3, enc4;
        var i = 0;

        input = Base64._utf8_encode(input);

        while (i < input.length) {

            chr1 = input.charCodeAt(i++);
            chr2 = input.charCodeAt(i++);
            chr3 = input.charCodeAt(i++);

            enc1 = chr1 >> 2;
            enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
            enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
            enc4 = chr3 & 63;

            if (isNaN(chr2)) {
                enc3 = enc4 = 64;
            } else if (isNaN(chr3)) {
                enc4 = 64;
            }

            output = output +
            this._keyStr.charAt(enc1) + this._keyStr.charAt(enc2) +
            this._keyStr.charAt(enc3) + this._keyStr.charAt(enc4);

        }

        return output;
    },

    // public method for decoding
    decode: function (input) {
        var output = "";
        var chr1, chr2, chr3;
        var enc1, enc2, enc3, enc4;
        var i = 0;

        input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

        while (i < input.length) {

            enc1 = this._keyStr.indexOf(input.charAt(i++));
            enc2 = this._keyStr.indexOf(input.charAt(i++));
            enc3 = this._keyStr.indexOf(input.charAt(i++));
            enc4 = this._keyStr.indexOf(input.charAt(i++));

            chr1 = (enc1 << 2) | (enc2 >> 4);
            chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
            chr3 = ((enc3 & 3) << 6) | enc4;

            output = output + String.fromCharCode(chr1);

            if (enc3 != 64) {
                output = output + String.fromCharCode(chr2);
            }
            if (enc4 != 64) {
                output = output + String.fromCharCode(chr3);
            }

        }

        output = Base64._utf8_decode(output);

        return output;

    },

    // private method for UTF-8 encoding
    _utf8_encode: function (string) {
        string = string.replace(/\r\n/g, "\n");
        var utftext = "";

        for (var n = 0; n < string.length; n++) {

            var c = string.charCodeAt(n);

            if (c < 128) {
                utftext += String.fromCharCode(c);
            }
            else if ((c > 127) && (c < 2048)) {
                utftext += String.fromCharCode((c >> 6) | 192);
                utftext += String.fromCharCode((c & 63) | 128);
            }
            else {
                utftext += String.fromCharCode((c >> 12) | 224);
                utftext += String.fromCharCode(((c >> 6) & 63) | 128);
                utftext += String.fromCharCode((c & 63) | 128);
            }

        }

        return utftext;
    },

    // private method for UTF-8 decoding
    _utf8_decode: function (utftext) {
        var string = "";
        var i = 0;
        var c = c1 = c2 = 0;

        while (i < utftext.length) {

            c = utftext.charCodeAt(i);

            if (c < 128) {
                string += String.fromCharCode(c);
                i++;
            }
            else if ((c > 191) && (c < 224)) {
                c2 = utftext.charCodeAt(i + 1);
                string += String.fromCharCode(((c & 31) << 6) | (c2 & 63));
                i += 2;
            }
            else {
                c2 = utftext.charCodeAt(i + 1);
                c3 = utftext.charCodeAt(i + 2);
                string += String.fromCharCode(((c & 15) << 12) | ((c2 & 63) << 6) | (c3 & 63));
                i += 3;
            }

        }

        return string;
    }
}

//新窗口打开
function ShowNewWindow(url) {
    var iWidth=1200; //弹出窗口的宽度;
    var iHeight=700; //弹出窗口的高度;
    var iTop = (window.screen.availHeight - 30 - iHeight) / 2; //获得窗口的垂直位置;
    var iLeft = (window.screen.availWidth - 10 - iWidth) / 2; //获得窗口的水平位置;
    window.open(url, "_blank", "height=" + iHeight + ",width=" + iWidth + ",top=" + iTop + ",left=" + iLeft + ",scrollbars=yes, titlebar=yes,  toolbar=no, menubar=no, scrollbars=yes,resizable=yes, location=no, status=no");
}

//滚动
(function ($) {
    $.fn.extend({
        ScrollButton: function (opt, callback) {
            if (!opt) var opt = {};
            var jqContainer = this.parent("div");
            var _btnUp = this.parent("div").siblings("#" + opt.up).first(); //Shawphy:向上按钮 
            var _btnDown = this.parent("div").siblings("#" + opt.down).first(); //Shawphy:向下按钮 
            //var _this = this.eq(0).find("ul:first");
            var _this = this;//.find("ul:first");  //UL
            var lineH = _this.find("li:first").height(); //获取行高 
            var line = opt.line ? parseInt(opt.line, 10) : parseInt(_this.height() / lineH, 10); //每次滚动的行数，默认为一屏，即父容器高度 
            var speed = opt.speed ? parseInt(opt.speed, 10) : 600; //卷动速度，数值越大，速度越慢（毫秒） 
            var m = line; //用于计算的变量 
            var count = _this.children("li").length; //总共的<li>元素的个数 
            var totalHeight = lineH * count;
            
            var upHeight = line * lineH;
            //最高显示内容
            var maxShowContentHeight = opt.maxHeight || 550;
            //jqContainer.height(maxShowContentHeight);

            resetControlButtonStatus();

            function scrollUp() {
                //if (!_this.is(":animated")) { //判断元素是否正处于动画，如果不处于动画状态，则追加动画。 
                //    if (m < count) { //判断 m 是否小于总的个数 
                //        m += line;
                //        //_this.animate({ marginTop: "-=" + upHeight + "px" }, speed);
                        
                       
                //        _this.animate({ scrollTop: "-" + upHeight + "px" }, speed);
                //    }
                //}
                var scollHeight =_this.scrollTop() - Number( upHeight) ;
                var scrollUpResult = _this.scrollTop(scollHeight);
                
                resetControlButtonStatus();
            }
            function scrollDown() {
                //if (!_this.is(":animated")) {
                //    if (m > line) { //判断m 是否大于一屏个数 
                //        m -= line;
                //        //_this.animate({ marginTop: "+=" + upHeight + "px" }, speed);                
                        
                //        //_this.scrollTop(upHeight);
                //        _this.animate({ scrollTop:  upHeight + "px" }, speed);
                //    }
                //}
                var scrollDownResult = _this.scrollTop(_this.scrollTop() + upHeight);

                resetControlButtonStatus();
               
            }
            function resetControlButtonStatus() {
                //_this.height(maxShowContentHeight);
                _this.css({ "max-height": maxShowContentHeight });
                //if((totalHeight-20)>= _this.height()){
                if ((totalHeight - 20) >= maxShowContentHeight) {
                    //向上滚动
                    if (_this.scrollTop() == 0) {
                        //_btnUp.hide();
                        showUpBtn(false);
                    }
                    else {
                        //_btnUp.show();
                        showUpBtn(true);
                    }
                    //向下滚动
                    if (_this.scrollTop() >= totalHeight - _this.height()) {
                        //_btnDown.hide();
                        showDownBtn(false);
                    }
                    else {
                        // _btnDown.show();
                        showDownBtn(true);
                    }
                }
                else {
                    //_btnUp.hide();
                    //_btnDown.hide();
                    showUpBtn(false);
                    showDownBtn(false);
                }
                
            }
            function showUpBtn(isShow) {
                if (isShow) {
                    _btnUp.children("img").attr("src", "../Images/ToolBar/上箭头Up.png");
                }
                else {
                    _btnUp.children("img").attr("src", "../Images/ToolBar/上箭头Down.png");
                    //停止滚动
                    stopScrollByTick();
                }
                
            }
            function showDownBtn(isShow) {
                if (isShow) {
                    _btnDown.children("img").attr("src", "../Images/ToolBar/下箭头Up.png");
                }
                else {
                    _btnDown.children("img").attr("src", "../Images/ToolBar/下箭头Down.png");
                    //停止滚动
                    stopScrollByTick();
                }

            }

            //滚动定时器
            var scrollInterval = null;
            function stopScrollByTick() {
                if (scrollInterval) {
                    clearInterval(scrollInterval);
                    scrollInterval = null;
                }
            }
            //向上tick滚动
            function scrollUpByTick() {
                try {
                    stopScrollByTick();
                    scrollInterval = setInterval(function () {
                        scrollUp();
                    }, 800);
                } catch (e) {

                }
            }
            //向下tick滚动
            function scrollDownByTick() {
                try {
                    stopScrollByTick();
                    scrollInterval = setInterval(function () {
                        scrollDown();
                    }, 800);
                } catch (e) {

                }
            }

            _btnUp.unbind("click");
            _btnUp.bind("click", scrollUp);
            _btnUp.unbind("mouseleave");
            _btnUp.bind("mouseleave", stopScrollByTick);
            _btnUp.unbind("mouseenter");
            _btnUp.bind("mouseenter", scrollUpByTick);

            _btnDown.unbind("click");
            _btnDown.bind("click", scrollDown);
            _btnDown.unbind("mouseleave");
            _btnDown.bind("mouseleave", stopScrollByTick);
            _btnDown.unbind("mouseenter");
            _btnDown.bind("mouseenter", scrollDownByTick);
        }
    });
})(jQuery);

//切分字符串成多行
function SplitStringToMultiLine(strData,lineLength) {
    var result = [];
    if (!strData || lineLength < 1) {
        return result;
    }
    try {
        if (strData.length > 0) {
            var lineCount = Math.ceil(strData.length / lineLength);
            for (var i = 1; i <= lineCount; i++) {
                try {
                    var startStringIndex = (i - 1) * lineLength;
                    //var endStringIndex = i * lineLength - 1;
                    var contentItem = strData.substr(startStringIndex, lineLength);
                    result.push(contentItem);

                } catch (e2) {

                }
            }
        }
    } catch (e) {

    }
    return result;
}

//计算天数差的函数，通用  
function DateDiff(sDate1, sDate2) {    //sDate1和sDate2是2006-12-18格式  
    var aDate, oDate1, oDate2, iDays
    aDate = sDate1.split("-")
    oDate1 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])    //转换为12-18-2006格式  
    aDate = sDate2.split("-")
    oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])
    iDays = parseInt(Math.abs(oDate1 - oDate2) / 1000 / 60 / 60 / 24)    //把相差的毫秒数转换为天数  
    return iDays
}

//jQuery EasyUI DataGrid分页方法
function pagerFilter(data) {
    if (typeof data.length == 'number' && typeof data.splice == 'function') { // is array  
        data = {
            total: data.length,
            rows: data
        }
    }
    var dg = jQuery(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            //            pageCount_1 = pageNum;
            //alert(pageCount);
            pager.pagination('refresh', {
                pageNumber: pageNum,
                pageSize: pageSize
            });
            dg.datagrid('loadData', data);
        },
        onRefresh: function (pageNum, pageSize) {
            // queryBookingOrder();
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
}

//转换成是否
function ConvertDataToShiFouReverse(data) {
    var result = data;
    try {
        if (data !=null) {
            if (data == 0 || data == "0") {
                result = "是";
            }
            else {
                result = "否";
            }
        }
    } catch (e) {

    }
    return result;
}
function ConvertDataToShiFou(data) {
    var result = data;
    try {
        if (data != null) {
            if (data == 0 || data == "0") {
                result = "否";
            }
            else {
                result = "是";
            }
        }
    } catch (e) {

    }
    return result;
}

//判断字符串非空
function IsStringNotEmptyAndNull(data) {
    var result = false;
    if (!$.isEmptyObject(data)) {
        if (typeof (data) == "string") {
            if (data != "") {
                result = true;
            }
        }
    }
    return result;
}

//将文本的换行符替换成</br>
function ReplaceStringLineBreaksByHTMLTag(data) {
    var result = "";
    try {
        if (IsStringNotEmptyAndNull(data)) {
            //result=data.replace(//r/n/g,'<br/>');
            //result = result.replace(//n/g, '<br/>');
            result = data.replace(/\r\n/g, '<br/>');
            result = result.replace(/\n/g,'<br/>');
        }
    } catch (e) {

    }

    return result;
}


//聚焦
/*
(function ($) {
    $.fn.textFocus = function (v) {
        var range, len, v = v === undefined ? 0 : parseInt(v);
        this.each(function () {
            if ($.browser.msie) {
                range = this.createTextRange(); //文本框创建范围 
                v === 0 ? range.collapse(false) : range.move("character", v); //范围折叠 
                range.select(); //选中 
            } else {
                len = this.value.length;
                v === 0 ? this.setSelectionRange(len, len) : this.setSelectionRange(v, v); //dom直接设置选区，然后focus 
            }
            this.focus();
        });
        return this;
    }
})(jQuery)
*/
$.fn.setCursorPosition = function (position) {
    if (this.lengh == 0) return this;
    return $(this).setSelection(position, position);
}

$.fn.setSelection = function (selectionStart, selectionEnd) {
    if (this.lengh == 0) return this;
    input = this[0];

    if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    } else if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }

    return this;
}

$.fn.focusEnd = function () {
    this.setCursorPosition(this.val().length);
}

//获得随机数
function GetUrlRandomNumber() {
    var randomNum = Math.floor(Math.random() * 10);
    return "" + randomNum;
}

//获得绝对路径
function GetAbsoluteURL(partURL) {
    var result = "";
    if (partURL && partURL != "") {
        result = window.location.protocol + "//" + window.location.host + window.location.port;
        result += partURL;
    }
    return result;
}

/************************Hex取反色：***************************/   //pengjian add
function hexToReverse(h) {
    var r = 0, g = 0, b = 0;
    r = 255 - parseInt(h[1], 16) * 16 - parseInt(h[2], 16);
    g = 255 - parseInt(h[3], 16) * 16 - parseInt(h[4], 16);
    b = 255 - parseInt(h[5], 16) * 16 - parseInt(h[6], 16);
    var result = (r < 16 ? "0" + r.toString(16).toUpperCase() : r.toString(16).toUpperCase()) + (g < 16 ? "0" + g.toString(16).toUpperCase() : g.toString(16).toUpperCase()) + (b < 16 ? "0" + b.toString(16).toUpperCase() : b.toString(16).toUpperCase());
    return '#' + result;
}

//点对象集合转化为geoJSON                  //pengjian add
function bizPointsToGeoJson(bizPoints) {
    //克隆json
    var dataObjCloned = JSON.parse(JSON.stringify(bizPoints));
    //过滤掉没有坐标的
    dataObjCloned = dataObjCloned.filter(function (element, index, array) {
        return (element.location.coordinates);
    });
    for (var i = 0; i < dataObjCloned.length; i++) {
        dataObjCloned[i]["type"] = "Feature";
        dataObjCloned[i]["geometry"] = dataObjCloned[i]["location"];
        delete dataObjCloned[i]["location"];
    }
    return dataObjCloned;
}

//面对象集合转化为geoJSON               //pengjian add
function bizPolygonsToGeoJson(bizPolygons) {
    //克隆json
    var dataObjCloned = JSON.parse(JSON.stringify(bizPolygons));
    //过滤掉没有坐标的
    dataObjCloned = dataObjCloned.filter(function (element, index, array) {
        return (element.location.coordinates);
    });
    for (var i = 0; i < dataObjCloned.length; i++) {
        dataObjCloned[i]["type"] = "Feature";
        dataObjCloned[i]["geometry"] = dataObjCloned[i]["location"];
        delete dataObjCloned[i]["location"];
    }
    return dataObjCloned;
}

function bizGeometryCollectionsToGeoJson(bizGeometryCollections) {
    //克隆json
    var dataObjCloned = JSON.parse(JSON.stringify(bizGeometryCollections));
    //过滤掉没有坐标的
    dataObjCloned = dataObjCloned.filter(function (element, index, array) {
        return (element.location.geometries);
    });
    for (var i = 0; i < dataObjCloned.length; i++) {
        dataObjCloned[i]["type"] = "Feature";
        dataObjCloned[i]["geometry"] = dataObjCloned[i]["location"];
        dataObjCloned[i]["properties"]["bot"] = dataObjCloned[i]["bot"];
        dataObjCloned[i]["properties"]["bo"] = dataObjCloned[i]["bo"];
        delete dataObjCloned[i]["location"];
    }
    return dataObjCloned;
}
