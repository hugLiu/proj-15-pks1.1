/****************************************************************************
leaflet-control-position.js,

(c) 2016, FCOO

https://github.com/FCOO/leaflet-control-position
https://github.com/FCOO

****************************************************************************/
(function (L, window, document, undefined) {
    "use strict";

    if (typeof console == "undefined") {
        this.console = { log: function (/*msg*/) { /* do nothing since it would otherwise break IE */} };
    }

    L.Control.Position = L.Control.extend({
        options: {
            collapsed: true,
            position: 'topleft',
            text: 'Zoom to (lat, lon) position',
            bounds: null, // L.LatLngBounds
            className: 'leaflet-control-position',
            icon: 'leaflet-control-position-icon'
        },

        initialize: function (options) {
            L.Util.setOptions(this, options);
            this._container = L.DomUtil.create('div', this.options.className);
            //this._container = L.DomUtil.create('div', 'leaflet-bar ' + className);
        },

        onAdd: function (map) {
            this._map = map;

            var className = this.options.className;
            var container = this._container;

            L.DomEvent.disableClickPropagation(container);

            var form = this._form = L.DomUtil.create('form', className + '-form');

            var input_lon = this._input_lon = document.createElement('input');
            input_lon.className = className + '-form-input-lon';
            input_lon.name = "lon";
            input_lon.type = "text";
            input_lon.placeholder = "Lon (ddd mm ss.sss W/E)";
            var input_lat = this._input_lat = document.createElement('input');
            input_lat.className = className + '-form-input-lat';
            input_lat.type = "text";
            input_lat.name = "lat";
            input_lat.placeholder = "Lat (dd mm ss.sss N/S)";

            var submit = document.createElement('input');
            submit.type = "submit";
            submit.value = this.options.text;

            form.appendChild(input_lat);
            form.appendChild(input_lon);
            form.appendChild(submit);

            L.DomEvent.addListener(form, 'submit', this._position, this);

            if (this.options.collapsed) {
                L.DomEvent.addListener(container, 'mouseover', this._expand, this);
                L.DomEvent.addListener(container, 'mouseout', this._collapse, this);

                var button = this._layersButton = L.DomUtil.create('a', className + ' ' + this.options.icon, container);
                button.title = 'Input latitude and longitude';

                L.DomEvent.addListener(button, L.Browser.touch ? 'click' : 'focus', this._expand, this);

                this._map.on('movestart', this._collapse, this);
            } else {
                this._expand();
            }

            container.appendChild(form);

            return container;
        },

        _position : function (event) {
            var lon, lat;
            L.DomEvent.preventDefault(event);
            try {
                lon = window.magellan(this._input_lon.value).longitude();
                if (lon === null) {
                    throw new Error('Invalid longitude.');
                }
                lon = parseFloat(lon.toDD());
                lat = window.magellan(this._input_lat.value).latitude();
                if (lat === null) {
                    throw new Error('Invalid latitude.');
                }
                lat = parseFloat(lat.toDD());
            } catch(err) {
                window.noty({text: err.message, type: "error", timeout: 3500});
                return;
            }
            this._map.panTo([lat, lon]);

            if (this._marker !== undefined) {
                this._map.removeLayer(this._marker);
            }
            this._marker = L.circleMarker([lat, lon], {
                color: '#136AEC',
                fillColor: '#2A93EE',
                fillOpacity: 0.7,
                weight: 2,
                opacity: 0.9,
                radius: 5
            });

            var _remove_marker = function(arg) {
                this._map.removeLayer(arg.target);
            };
            this._marker.on('click', _remove_marker);
            this._marker.addTo(this._map);
        },

        _expand: function () {
            L.DomUtil.addClass(this._container, 'leaflet-control-position-expanded');
            //this._layersLink.className = this._layersLink.className.replace(' ' + this.options.icon, '');
        },

        _collapse: function () {
            this._container.className = this._container.className.replace(' leaflet-control-position-expanded', '');
        }
    });

}(L, this, document));
