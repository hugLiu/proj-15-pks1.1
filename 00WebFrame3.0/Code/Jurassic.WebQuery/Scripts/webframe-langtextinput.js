$.langTextInput = function (opt) {

    function showPopBox(e) {
        var popBox = $('#multilang_' + opt.id);
        var btn = mini.get(opt.id);
        e.stopPropagation();
        popBox.width($('#tbl_'+ opt.id).width()+1);
        popBox.show();
    }

    $('[name=' + opt.name + ']').bind("click focus", showPopBox);
    
    // var t;
    //btn.on("textchanged", function (e) {
    // clearTimeout(t);
    // t = setTimeout(...)
    //    //todo:调用自动翻译接口
    //});
};
