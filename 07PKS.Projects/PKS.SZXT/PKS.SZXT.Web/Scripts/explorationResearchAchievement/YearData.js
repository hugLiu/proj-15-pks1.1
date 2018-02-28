//获取年
function getCurrentYear() {
    var date = new Date();
    return date.getFullYear();
}

//获取过滤数据
function getYearName() {
    var index = Math.ceil(getFiveYear() / 5) - parseInt(size) - 1;
    var yearName = [];
    var currentYear = getCurrentYear();
    for (var i = 1; i <= 10; i++)
    {
        yearName.push(currentYear);
        currentYear = currentYear - 1;
    }
    return showfiveYearName
}