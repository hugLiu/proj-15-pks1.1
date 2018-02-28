function RecentDates() {
    this.Dates = [{
        "name":"全部","value":0,"code":"all"
    },{
       "name":"今天","value":1,"code":"today"
    },{
        "name":"最近一天","value":2,"code":"day"
    }, {
        "name": "最近一周", "value": 3, "code": "week"
    }, {
        "name":"最近一个月","value":4,"code":"month"
    },{
        "name":"最近一年","value":5,"code":"year"
    }];
}
RecentDates.prototype.getBeginEndDate = function (item) {
    
    if (!item || !item.code||item.code == "all")
        return { "startDate": null, "endDate": null };
    var startDate = new Date();
    startDate.setHours(0);
    startDate.setMinutes(0);
    startDate.setSeconds(0);

    var endDate = new Date();
    endDate.setHours(23);
    endDate.setMinutes(59);
    endDate.setSeconds(59);

    if (item.code == "day") {
        startDate.setDate(startDate.getDate() - 1);
    }
    if (item.code == "week") {
        startDate.setDate(startDate.getDate() - 7);
    }
    if (item.code == "month") {
        var y = (startDate.getMonth() == 0) ? (startDate.getFullYear() - 1) : startDate.getFullYear();
        var m = (startDate.getMonth() == 0) ? 11 : startDate.getMonth() - 1;
        var day = startDate.getDate();

        startDate = new Date(y, m, day);
        startDate.setHours(0);
        startDate.setMinutes(0);
        startDate.setSeconds(0);

    }
    if (item.code == "year") {
        var y = startDate.getFullYear() - 1;
        var m = startDate.getMonth();
        var day = startDate.getDate();

        startDate = new Date(y, m, day);
        startDate.setHours(0);
        startDate.setMinutes(0);
        startDate.setSeconds(0);
    }
    if (item.code == "today") {
    }
    return { "startDate": startDate, "endDate": endDate };
}