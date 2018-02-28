//定起始年
var fiveYearsPlanName = ["九五", "十五", "十一五", "十二五", "十三五", "十四五", "十五五", "十六五", "十七五", "十八五", "十九五"];
var start = 1996;

//获取年
function getCurrentYear() {
    var date = new Date();
    return date.getFullYear();
}

//获取第几个五年计划
function getFiveYear() {
    var year = getCurrentYear();
    return year - parseInt(start);
}
//获取过滤数据
function getShow5thName(size) {
    var index = Math.ceil(getFiveYear() / 5) - parseInt(size)-1;
    var showfiveYearName = [];
    for (var i = parseInt(size) + index, j =index ; i >j; i--) {
        var item = {
            fiveplan: fiveYearsPlanName[i],
            year: getYearArray(i).sort().reverse()
        }
        showfiveYearName.push(item);
    }
    return showfiveYearName
}
//通过五年计划名称获取年数组
function getYearArray(index) {
    var list = [];
    start = parseInt(start);
    var _start = start + 5 * parseInt(index);
    for (var m = 0, n = 5; m < n; m++) {
        var yearIndex = _start + parseInt(m);
        if (yearIndex > parseInt(getCurrentYear())) break;
        list.push(yearIndex);
    }
    return list;
}