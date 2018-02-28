L.Control.EasyButtons = L.Control.extend({
    options: {
        position: 'topright',
        title: '',
        intentedIcon: 'fa-circle-o'
    },

    onAdd: function () {
        //var container = L.DomUtil.create('div', 'leaflet-bar leaflet-control');

        //this.link = L.DomUtil.create('a', 'leaflet-bar-part', container);
        //L.DomUtil.create('i', 'fa fa-lg ' + this.options.intentedIcon , this.link);
        var container = L.DomUtil.create('div', 'tool-bar leaflet-control');
        this.link = L.DomUtil.create('a', 'tool-bar-part', container);
        L.DomUtil.create('div', this.options.intentedIcon, this.link);
        this.link.href = '#';

        L.DomEvent.on(this.link, 'click', this._click, this);
        this.link.title = this.options.title;

        return container;
    },

    ////选中
    //toggle: function () {
    //    if (this.handler.enabled()) {
    //        this.handler.disable.call(this.handler);
    //    } else {
    //        this.handler.enable.call(this.handler);
    //    }
    //},
    ////选中--end
    intendedFunction: function(){ alert('no function selected');},

    _click: function (e) {
        L.DomEvent.stopPropagation(e);
        L.DomEvent.preventDefault(e);
        this.intendedFunction();
    },
});

L.easyButton = function( btnIcon , btnFunction , btnTitle , btnMap ) {
  var newControl = new L.Control.EasyButtons;
  if (btnIcon) newControl.options.intentedIcon = btnIcon;

  if ( typeof btnFunction === 'function'){
    newControl.intendedFunction = btnFunction;
  }

  if (btnTitle) newControl.options.title = btnTitle;

  if ( btnMap == '' ){
    // skip auto addition
  } else if ( btnMap ) {
    btnMap.addControl(newControl);
  } else {
    map.addControl(newControl);
  }
  return newControl;
};
