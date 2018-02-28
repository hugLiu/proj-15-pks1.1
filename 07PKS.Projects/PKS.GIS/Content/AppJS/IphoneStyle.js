var DockIcon, Icon, Screen, Stage,
  __hasProp = {}.hasOwnProperty,
  __extends = function (child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

Icon = (function () {
    function Icon(id, title, imageurl,numspan) {
        //onclick='iconClick(" + iconClickType + ")' 
        this.id = id;
        this.title = title;
        this.imageurl = imageurl;
        //this.markup = "<div><a><div id=icondiv_"+this.id+"  class='iphoneicon' style='background:url(../images/icon/" + this.id + ".png)  top center no-repeat; background-size: cover;' title='" + this.title + "'></div></a></div>";
        this.markup = "<a href='#'><div id=icondiv_" + this.id + "  class='iphoneicon' style='background:url(" + this.imageurl + ")  top center no-repeat; background-size: cover;' title='" + this.title + "'>" + numspan + "</div></a>";
        //传入函数绑定不上，改为传入类型，在响应界面处理iconClick 函数
        //var jqNewBtn = $(this.markup);
        //if (typeof iconfunction === 'function') {
        //    var jqBtnA = jqNewBtn.children("a").first();
            
        //    jqBtnA.click(iconfunction);
        //    //jqNewBtn.click(iconfunction);            
        //}       
    }
    return Icon;

})();

DockIcon = (function (_super) {
    __extends(DockIcon, _super);

    function DockIcon(id, title) {
        DockIcon.__super__.constructor.call(this, id, title, iconfunction);
        this.markup = this.markup.replace("class='iphoneicon'", "class='iphonedockicon'");
    }

    return DockIcon;

})(Icon);

Screen = (function () {
    function Screen(icons) {
        if (icons == null) {
            icons = [];
        }
        this.icons = icons;
    }

    Screen.prototype.attachIcons = function (icons) {
        if (icons == null) {
            icons = [];
        }
        return Array.prototype.push.apply(this.icons, icons);
    };

    Screen.prototype.generate = function () {
        var icon, markup, _i, _len, _ref;
        markup = [];
        _ref = this.icons;
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            icon = _ref[_i];
            markup.push(icon.markup);
        }
        return "<div class='screen'>" + (markup.join('')) + "</div>";
    };

    return Screen;

})();

Stage = (function () {
    Stage.prototype.screenWidth = 300;

    function Stage(icons) {
        var i, num, s;
        this.currentScreen = 0;
        this.screens = [];
        num = Math.ceil(icons.length / 9);
        i = 0;
        while (num--) {
            s = new Screen(icons.slice(i, i + 9));
            this.screens.push(s);
            i += 9;
        }
    }

    Stage.prototype.addScreensTo = function (element) {
        var screen, _i, _len, _ref, _results;
        this.element = $(element);
        this.element.width(this.screens.length * this.screenWidth);
        _ref = this.screens;
        _results = [];
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            screen = _ref[_i];
            _results.push(this.element.append(screen.generate()));
        }
        return _results;
    };

    Stage.prototype.addIndicatorsTo = function (elem) {
        var screen, _i, _len, _ref;
        this.ul = $(elem);
        _ref = this.screens;
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
            screen = _ref[_i];
            this.ul.append('<li>');
        }
        return this.ul.find('li:first').addClass('active');
    };

    Stage.prototype.goTo = function (screenNum) {
        var from, to, _ref, _ref1;
        if (this.element.is(':animated')) {
            return false;
        }
        if (this.currentScreen === screenNum) {
            _ref = ['+=15', '-=15'], from = _ref[0], to = _ref[1];
            if (this.currentScreen !== 0) {
                _ref1 = [to, from], from = _ref1[0], to = _ref1[1];
            }
            return this.element.animate({
                marginLeft: from
            }, 150).animate({
                marginLeft: to
            }, 150);
        } else {
            this.element.animate({
                marginLeft: -screenNum * this.screenWidth
            }, (function (_this) {
                return function () {
                    return _this.currentScreen = screenNum;
                };
            })(this));
            return this.ul.find('li').removeClass('active').eq(screenNum).addClass('active');
        }
    };

    Stage.prototype.next = function () {
        var toShow;
        toShow = this.currentScreen + 1;
        if (toShow === this.screens.length) {
            toShow = this.screens.length - 1;
        }
        return this.goTo(toShow);
    };

    Stage.prototype.previous = function () {
        var toShow;
        toShow = this.currentScreen - 1;
        if (toShow === -1) {
            toShow = 0;
        }
        return this.goTo(toShow);
    };

    return Stage;

})();


