/*
WebRepeater的配套js文件。它提供以下功能：
点击展示更多

API：
getSelectedIdArr()
getSelectedIds()
getCurrentId()
*/
(function ($) {
    //返回所选id的数组
    $.fn.getSelectedIdArr = function () {
        var thisId = this.attr("id");
        var r = $("[name=" + thisId + "_chk]:checkbox").cast(function (chk) {
            if (chk.checked) {
                return chk.value;
            }
        });
        return r;
    };

    //返回,号分隔的所选id的字符串
    $.fn.getSelectedIds = function () {
        var thisId = this.attr("id");
        var r = this.getSelectedIdArr();
        return r.join(',');
    };

    //返回当前行数据ID
    $.fn.getCurrentId = function () {
        return this.data("currentId");
    }

    //初始化
    $.fn.repeater = function (opt) {
        if (this._isrepeater) return this;
        this._isrepeater = true;
        var thisId = this.attr("id");
        var chkAllId = "#" + thisId + "_chkAll";

        //点击表格每一行时，传递当前行数据ID
        $(document).on("click", '#' + thisId + " tr", function () {
            var currentId = $(this).find("[name=" + thisId + "_chk]:checkbox").val();
            $('#' + thisId).data("currentId", currentId);
            var thatTr = this;

            //将选中的列赋予选中的样式
            if (opt && opt.ShowFocusRowStyle) {
                $('#' + thisId + " tr").each(function () {
                    var trId = $(this).attr("id");
                    if (this == thatTr) {
                        $(this).addClass("info");
                    }
                    else {
                        $(this).removeClass("info");
                    }
                });
            }
            //var $chk = $("[name=" + thisId + "_chk]:checkbox");
            //$chk.each(function () {
            //    this.checked = (this.value == currentId);
            //});
        });

        //全选/全不选操作
        $(chkAllId).bind("click", function () {
            var that = this;
            $("[name=" + thisId + "_chk]:checkbox").each(function () {
                this.checked = that.checked;
            });
        });

        //监视每个单项复选框，决定是否改变全选框勾选
        $(document).on("click", "[name=" + thisId + "_chk]:checkbox", function (e) {
            var $chk = $("[name=" + thisId + "_chk]:checkbox");
            var allchked = $chk.length == $chk.filter(":checked").length;
            $(chkAllId)[0].checked = allchked;
            //e.stopPropagation();
        });

        //点击查看更多，加载下一页数据
        var moreId = '#' + thisId + "_more";
        var containerId = '#' + thisId + "_body";
        var bottomId = '#' + thisId + "_bottom";
        $(document).on("click", moreId, function (e) {
            e.preventDefault();
            $.newGET($(moreId).attr("href"), function (data) {
                //返回全部的分部页html
                data = $("<div>").append($.parseHTML(data, true));

                //追加下一页的表格数据
                var r = data.find(containerId); //查找名为@containerId的区域
                if (r.length == 0) r = data; //找不到则用全部内容
                $(containerId).append(r.html());

                //替换底部分页的内容
                r = data.find(bottomId);
                $(bottomId).html(r.html());
            });
        });

        //搜索框的事件处理, webframe1.0中的事件定义是基于mini-ui控件的，在此处不适合，
        //所以要重新定义
        $('.frame-search :text').val($.urlParser(location.href).get("key"));
        $(function () {
            $.regButton({
                text: '搜索',
                noSubmit: function () {
                    //获取搜索框文本
                    var key = $('.frame-search :text').val();
                    var srhUrlParser = $.urlParser(location.href);

                    //去掉页号，从1开始
                    srhUrlParser.remove('page');

                    //在url中添加搜索关键字
                    srhUrlParser.set('key', key);
                    location = srhUrlParser.url();
                }
            });
        });
        return this;
    }
})(jQuery);
