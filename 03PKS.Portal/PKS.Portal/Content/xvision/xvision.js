(function(global,factory){
    if (!global.jQuery) throw new Error("未能找到依赖项jQuery");
    if (typeof exports === "object") {
        module.exports = factory(global.jQuery);
    } else if (typeof define === "function" && define.amd) {
        define([global.jQuery], factory);
    } else {
        global.xvs = factory(global.jQuery);
    }
})(this,function($){
    // my  xVision
    var xvision = {};

    /**
     * event type
     * @type {{base: {click: string, msUp: string, msDn: string, msMv: string}}}
     */
    xvision.evtType = {
        base:{
            click:"click",
            msUp:"mouseup",
            msDn:"mousedown",
            msMv:"mousemove"
        }
    };

    /**
     * tab function
     * @param tabCls
     * @param tabBtnCls
     * @param tabCtnCls
     * @param eventType
     */
    xvision.tabFunc = function(tabCls,tabBtnCls,tabCtnCls,eventType){
        $(function(){
            $(document).on(eventType,tabBtnCls,function(e){
                var target$ = $(e.currentTarget);
                target$.addClass("active").siblings().removeClass("active");
                var anchor = target$.data() && target$.data().anchor;
                if(!anchor) return;
                var tab$ = target$.closest(tabCls);
                tab$.find(tabCtnCls + "[data-anchor=" + anchor + "]").addClass("active").siblings().removeClass("active");
            });
            $(tabCls).find(tabBtnCls + ":first-child").not(".active").trigger(eventType);
        })
    };
    xvision.tabFunc2 = function (tabCls, tabBtnCls, tabCtnCls, eventType) {
        $(function () {
            $(document).on(eventType, tabBtnCls, function (e) {
                var target$ = $(e.currentTarget);
                target$.addClass("active").parent().siblings().find(".active").removeClass("active");
                var anchor = target$.data() && target$.data().anchor;
                if (!anchor) return;
                var tab$ = target$.closest(tabCls);
                tab$.find(tabCtnCls + "[data-anchor=" + anchor + "]").addClass("active").siblings().removeClass("active");
            });
            $(tabCls).find(tabBtnCls + ":first-child").not(".active").trigger(eventType);
        })
    };

    //******************************************************************************************************************
    // tab
    //******************************************************************************************************************
    (function(){
        xvision.tabFunc(".jurassic-tab",".jurassic-tab-btn",".jurassic-tab-container",xvision.evtType.base.msMv);
    })();


    //******************************************************************************************************************
    // tab panel
    //******************************************************************************************************************
    (function(){
        xvision.tabFunc2(".jurassic-panel",".jurassic-tab-panel-title",".jurassic-tab-panel-container",xvision.evtType.base.msMv);
    })();


});
























