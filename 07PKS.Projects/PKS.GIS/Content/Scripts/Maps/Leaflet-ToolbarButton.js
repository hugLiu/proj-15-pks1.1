
//工具栏最外层Control
var mapRightToolbarStation = null;
L.Control.ToolbarStationControl = L.Control.extend({
    options: {
        position: 'topright',
        autoZIndex: true
    },

    onAdd: function (map) {
        // create the control container with a particular class name
        var container = L.DomUtil.create('div', 'leaflet-toolbarbutton-station');
        var jqContainer = $(container);
        // ... initialize other DOM elements, add listeners, etc.
        jqContainer.append('<div id="btn1" class="leaflet-toolbarbutton-scrollup"><img src="../Images/ToolBar/上箭头Up.png" /></div>');
        jqContainer.append('<div class="leaflet-toolbarbutton-content"><ul class="leaflet-toolbarbutton-ul"></ul></div>');
        jqContainer.append('<div id="btn2" class="leaflet-toolbarbutton-scrolldown"><img src="../Images/ToolBar/下箭头Up.png" /></div> ');

        L.DomEvent.disableClickPropagation(container);
        jqContainer.scroll(function (event) {
            event.preventDefault();
            return false;
        });
        return container;
    },
    update: function (domContent) {
        if (domContent) {
            $(this.container).append($(domContent));
        }
    },
    initialize: function (foo, options) {
        // ...

        L.Util.setOptions(this, options);
        this._handlingClick = true;
        this._lastZIndex = 2000;
    },
    // 添加按钮
    addToobarButton: function (jqButton) {
        try {
            var jqContainer = $(this._container);
            var jqUL = jqContainer.children(".leaflet-toolbarbutton-content:first").children("ul").first();
            jqUL.append(jqButton);
            //alert("添加按钮");
            //设置滚动
            jqUL.ScrollButton({ line: 1, speed: 500, up: "btn1", down: "btn2",maxHeight:450 });
            //jqUL.ace_scroll(jqUL);
        } catch (e) {

        }
    },
    setToobarButtonSelected: function (className,isSelected) {
        try {

        } catch (e) {

        }
    }
});

//工具栏按钮
L.ToolbarButton = function (btnClass, btnTitle, btnFunction,btnSelectedClass) {
    var newControl = null;
    var btnHTML = "";
    //var newButtonHTML = '<li>\
    //                    <a href="javascript:return false;" option-toolbarbutton-selected="false" class="leaflet-toolbarbutton-btn ' + btnClass + '">\
    //                        <div></div>\
    //                        <div>' + btnTitle + '</div>\
    //                    </a>\
    //                </li>';
    var newButtonHTML = '<li>\
                        <a href="#" option-toolbarbutton-selected="false" class="leaflet-toolbarbutton-btn ' + btnClass + '">\
                            <div></div>\
                            <div>' + btnTitle + '</div>\
                        </a>\
                    </li>';
    var jqNewBtn = $(newButtonHTML);
    if (typeof btnFunction === 'function') {
        var jqBtnA = jqNewBtn.children("a").first();
        //设置选中标记
        jqBtnA.click(function (e) {
            var isSelected = false;
            try {
                var jqThis = $(this);
                //读取是否选择
                var jqOptionToolbarButtonSelected = jqThis.attr("option-toolbarbutton-selected");
                if (jqOptionToolbarButtonSelected) {
                    if (jqOptionToolbarButtonSelected == "true") {
                        isSelected = true;
                    }
                    else {
                        isSelected = false;
                    }
                }
                isSelected = !isSelected;
                jqThis.attr("option-toolbarbutton-selected", isSelected);
                //设置选中样式
                try {
                    var selecedClassName = "";
                    if (btnSelectedClass) {
                        selecedClassName = btnSelectedClass;
                    }
                    else {
                        selecedClassName = btnClass + "-selected";
                    }
                    if (isSelected) {
                        jqThis.addClass(selecedClassName);
                    }
                    else {
                        jqThis.removeClass(selecedClassName);
                    }
                } catch (e) {

                }
                

            } catch (e) {

            }
        });

        jqBtnA.click(btnFunction);
    }

    //初始化工具栏最外层
    if (mapRightToolbarStation == null) {
        mapRightToolbarStation = new L.Control.ToolbarStationControl();        
        mapRightToolbarStation.addTo(map);
    }    
    mapRightToolbarStation.addToobarButton(jqNewBtn);
    return jqNewBtn;
};
