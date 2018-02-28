(function (factory, window) {

    // AMD
    if (typeof define === 'function' && define.amd) {
        define(['leaflet'], factory);

    // Common JS
    } else if (typeof exports === 'object') {
        module.exports = factory(require('leaflet'));
    }

    // global
    if (typeof window !== 'undefined' && window.L) {
        window.L.Control.BoxSelector = factory(L);
    }
}(function (L) {
	//selection logic separated into another class for clarity, but included in this file for simplicity
	var SelectionManager = L.Class.extend({

		initialize: function (map) {
			this._map = map;
		},
		
		getSelectedMarkers: function() {
			var markers = [];
			Object.keys(this._allSelectedMarkers).sort().forEach(function(markerId) { //sort is just so the order is predictable, makes debugging easier
				var marker = this._allSelectedMarkers[markerId];
				if (L.MarkerCluster != undefined && marker instanceof L.MarkerCluster) { //the actual cluster icon itself
					markers = markers.concat(marker.getAllChildMarkers());
				} else { // standard marker hopefully
					markers.push(marker);
				}
			}.bind(this));
			return markers;
		},
		
		enable: function() {
			this._clearExistingSelection();
		},
		
		_clearExistingSelection: function() {
			this._allSelectedMarkers = {};
			this._previouslySelectedMarkers = [];
			this._currentlySelectedMarkers = [];
		},
		
		disable: function() {
			for (key in this._allSelectedMarkers) {
				var marker = this._allSelectedMarkers[key];
				this._toggleSelection(marker, false);
			}
			this._clearExistingSelection();
		},
		
		start: function() {
			this._currentlySelectedMarkers = [];
			this._findAllLoadedMarkers();
		},
		
		_findAllLoadedMarkers: function() {
			this._allLoadedMarkers = {};
			var fakeBounds = {contains: function() {return true;}};//TODO refactor enumerate to remove this
			this._enumerateMarkers(fakeBounds, this._map._layers, function(marker) {
				var key = L.Util.stamp(marker);
				this._allLoadedMarkers[key] = marker; // include the cluster icons, as those are what we want to highlight
			}.bind(this));
			var jslength = 0;
			for (var js2 in this._allLoadedMarkers) {
			    jslength++;
			}
		},
		
		update: function(latLngBounds) {
			var newlySelectedMarkers = {};
			this._enumerateMarkers(latLngBounds, this._map._layers, function(marker) {
				var key = L.Util.stamp(marker);
				newlySelectedMarkers[key] = marker;
			}.bind(this));
			var mismatches = this._outerJoinArrays(Object.keys(this._currentlySelectedMarkers), Object.keys(newlySelectedMarkers));
			var idToSelect = mismatches.in2ButNot1;
			var idToDeselect = mismatches.in1ButNot2;
			for (var i = 0; i < idToSelect.length; i++) {
				var marker = this._allLoadedMarkers[idToSelect[i]];
				this._toggleSelection(marker, true);
			}
			for (var i = 0; i < idToDeselect.length; i++) {
				//if this marker was selected in a previous run, don't de-select it now
				if (this._previouslySelectedMarkers.indexOf(idToDeselect[i]) == -1) {
					var marker = this._allLoadedMarkers[idToDeselect[i]];
					this._toggleSelection(marker, false);
				}
			}
			this._currentlySelectedMarkers = newlySelectedMarkers;
		},
		
		finish: function(bounds) {
			this._enumerateMarkers(bounds, this._map._layers, function(marker) {
				if (this._allSelectedMarkers == null) {
					this._allSelectedMarkers = {};
				}
				var key = L.Util.stamp(marker);
				this._allSelectedMarkers[key] = marker; //may already be there, but just override it
			}.bind(this));
			this._previouslySelectedMarkers = this._previouslySelectedMarkers.concat(Object.keys(this._currentlySelectedMarkers));
		},

		_isIgnoredLayer: function(layer) {
			//it isn't obvious that there's a clever way to work out if we should ignore a layer, so for now we'll just have to have a list of 'known bad' layer types
			//sub groups (https://github.com/ghybs/Leaflet.FeatureGroup.SubGroup) essentially duplicate the markers and don't appear on the UI, so ignore them
			if (L.FeatureGroup.SubGroup != undefined) {
				return layer instanceof L.FeatureGroup.SubGroup;
			} else {
				return false;
			}
		},

	    // there still exist some problems :
	    // 1.L.polyine/L.polygon/L.rectangle/L.cicle are not considered , especially in L.rectangle the method 'getLatLngs' which can only receive two points
	    // 2.L.geojson only take consideration of the 'LineString'and the 'MultiPolygon' situation ,which still remained some other types.
	    // 3.exclude the marker's name now judged by the 'ProductionUnitType' which is customed in vdata.js.
		_enumerateMarkers: function(bounds, layers, callback) {
			Object.keys(layers).forEach(function (key) {
			    var layer = layers[key];
				if (!this._isIgnoredLayer(layer)) {
				    if (layer instanceof L.Marker) { //标注
				        if (layer.ProductionUnitType != undefined) //排除名称标注  //vdata.js 配置标注属性
				        {
				            if (bounds.contains(layer.getLatLng())) {
				               //var text = layer.getLatLng();
				                callback(layer);
				            }
				        }						
				    }
				    else if(layer.feature != undefined) //geojson  (如果不加此判断语句，箭头不会被排除，加此判断语句，排除了通过L.polyline L.polygon等接口直接画出的线、面图层)
				    {
				         if(layer instanceof L.Circle)  //只判断了圆心是否在区域内，相交情况没有做考虑
				         {
				            if (bounds.contains(layer.getLatLng())) {
				                callback(layer);
				            }
				         }
				         else if ((layer instanceof L.Polyline) || (layer instanceof L.Polygon)) { //线、面
				            //var test = layer.getLatLngs();
				            for (var i = 0; i < layer.getLatLngs().length; i++)
				            {
				                 if (bounds.contains(layer.getLatLngs()[i])) {
				                    callback(layer);
				                }
				            }			        
				        }   
				    }
				    
					/*else if (layer._layers != undefined)//所有覆盖层
					{	
					    if (layer._layers[key - 1] != null) {
					        if (layer._layers[key - 1].feature != null) {
					            if (layer._layers[key - 1].feature.geometry.type == "LineString") {
					                var pointstart =layer._layers[key - 1]._latlngs[0];
					                var pointend =layer._layers[key - 1]._latlngs[1];
					                if (bounds.contains(pointstart)|| bounds.contains(pointend))
					                {
					                    callback(layer);
					                }      
					             }
					        }
					    }
					    this._enumerateMarkers(bounds, layer._layers, callback);
					}*/
				
					else if (layer._layers != undefined) {
					    this._enumerateMarkers(bounds, layer._layers, callback);
					}
				} //end
			}, this);
		},
		
		_toggleSelection: function(marker, select) {
		    if (select) {
		        if (marker._icon != undefined)
		        {
		            L.DomUtil.addClass(marker._icon, 'marker-highlight');
		        }
		        else
		        {
		            L.DomUtil.addClass(marker._path, 'marker-highlight');
		        }
		    } else {
		        if (marker._icon != undefined)
		        {
		            L.DomUtil.removeClass(marker._icon, 'marker-highlight');
		        }   
                else
                {
		            L.DomUtil.removeClass(marker._path, 'marker-highlight');
                }
			}
		},
		
		//both arrays must contain only numbers
		_outerJoinArrays: function (arr1, arr2) {
			function numericalSort(a, b) {
				return a - b;
			}
			arr1 = arr1.sort(numericalSort);
			arr2 = arr2.sort(numericalSort);
			var i = 0;
			var j = 0;
			var in1ButNot2 = [];
			var in2ButNot1 = [];
			while (i < arr1.length || j < arr2.length) {
				if (i == arr1.length) {
					//run out of array 1, so everything left in array 2 must be missing from 1
					in2ButNot1.push(arr2[j]); //TODO could just merge the arrays to be more efficient here
					j++;
				} else if (j == arr2.length) {
					//run out of array 2, so everything left in array 1 must be missing from 2
					in1ButNot2.push(arr1[i]); //TODO could just merge the arrays to be more efficient here
					i++;
				} else if (arr1[i] == arr2[j]) {
					//in both, move on
					i++;
					j++;
				} else if (arr1[i] < arr2[j]) {
					//must be missing from arr2
					in1ButNot2.push(arr1[i]);
					i++;
				} else {//if (arr1[i] > arr2[j]) {
					//must be missing from arr1
					in2ButNot1.push(arr2[j]);
					j++;
				}
			}
			if (i != arr1.length || j != arr2.length) {
				console.error("not all the things were consumed");
				console.error("arr1.length=" + arr1.length + ", i=" + i);
				console.error("arr2.length=" + arr2.length + ", j=" + j);
			}
			return {
				in1ButNot2: in1ButNot2,
				in2ButNot1: in2ButNot1
			};
		}
	});

	var BoxSelector = L.Control.extend({

		options: {
			highlightOnDrag: true //on by default but could cause poor performance if there is a large number of markers
		},

		initialize: function (options) {
			L.setOptions(this, options);
			if (this.options.actions == null) {
				//set default action
				this.options.actions = {
					alert: {
						display: "Display selected coords",
						action:	L.Control.BoxSelector.Actions.alert()
					}
				}
			}
		},
		
		onAdd: function (map) {
			this._map = map;
			this._mapcontainer = map._container;
			this._pane = map._panes.overlayPane;
			this._addHooks();
			this._manager = new SelectionManager(this._map);
			
			//set up select icon
			this._selectorContainer = L.DomUtil.create('div', 'leaflet-control-box-selector leaflet-bar leaflet-control boxselector-control boxselector-hidden');
			this._buttonBar = L.DomUtil.create('div', 'leaflet-bar-part leaflet-bar-part-single boxselector-button-bar', this._selectorContainer);
			this._toggleElement = L.DomUtil.create('a', 'boxselector-icon', this._buttonBar);
			this._toggleElement.id = 'boxselector-icon';
			this._toggleElement.href = '#';
			var iconWrapper = L.DomUtil.create('div', 'icon-wrapper', this._toggleElement);
			var icon = L.DomUtil.create('div', 'icon', iconWrapper);
			
			//set up dropdown icon
			var dropdownButton = L.DomUtil.create('a', 'boxselector-dropdown-button', this._buttonBar);
			dropdownButton.id = 'boxselector-dropdown-button';
			dropdownButton.href = '#';
			var dropdownIconWrapper = L.DomUtil.create('div', 'boxselector-dropdown-icon-wrapper', dropdownButton);
			var dropdownIcon = L.DomUtil.create('div', 'boxselector-dropdown-icon', dropdownIconWrapper);
			L.DomEvent.on(dropdownButton, 'click', this._expandCollapse, this)
			
			//set up drop down (hidden to start with)
			this._expanded = false;
			this._dropdown = L.DomUtil.create('div', 'boxselector-dropdown', this._selectorContainer);
			for (var actionId in this.options.actions) {
				var dropdownEntry = L.DomUtil.create('a', 'boxselector-dropdown-entry', this._dropdown);
				dropdownEntry.actionId = actionId;
				dropdownEntry.href = '#';
				var label = L.DomUtil.create('span', '', dropdownEntry);
				label.innerHTML = this.options.actions[actionId].display;
				label.actionId = actionId;

				L.DomEvent.on(dropdownEntry, 'click', this._actionItemClick, this);
			}
			
			L.DomEvent.on(this._toggleElement, 'click', this._toggle, this);

			//ensure we tear down		
			this._map.on('unload', this._removeHooks, this);
			
			return this._selectorContainer;
		},
		
		_expandCollapse: function() {
			if (this._expanded) {
				L.DomUtil.addClass(this._selectorContainer, 'boxselector-hidden');
				L.DomUtil.removeClass(this._selectorContainer, 'boxselector-expanded');
				this._expanded = false;
			} else {
				L.DomUtil.addClass(this._selectorContainer, 'boxselector-expanded');
				L.DomUtil.removeClass(this._selectorContainer, 'boxselector-hidden');
				this._expanded = true;
			}
		},
		
		_actionItemClick: function(e) {
			var actionItemElement = e.target;
			var actionId = actionItemElement.actionId;
			var action = this.options.actions[actionId].action;
			this._expandCollapse();
			action(this.getSelectedMarkers());
		},

		_addHooks: function () {
			L.DomEvent.on(this._mapcontainer, 'mousedown', this._onMouseDown, this);
		},

		_removeHooks: function () {
			L.DomEvent.off(this._mapcontainer, 'mousedown', this._onMouseDown, this);
		},

		_toggle: function() {
			this._setEnabled(!this._isEnabled());
		},
		
		_isEnabled: function() {
			return this._selectionEnabled;
		},
		
		_setEnabled: function(enabled) {
			this._selectionEnabled = enabled;
			if (enabled) {
				//enable selection
				this._map.dragging.disable();
				this._map.boxZoom.disable();
				L.DomUtil.addClass(this._buttonBar, 'selection-enabled');
				this._manager.enable();
			} else {
				//disable selection
				this._map.dragging.enable();
				this._map.boxZoom.enable();
				L.DomUtil.removeClass(this._buttonBar, 'selection-enabled');
				this._manager.disable();
			}
		},

		_resetState: function () {
			//if you try to pinch to zoom while in selection mode, _onMouseDown never gets called. This means it never registers the selection. We ought to fix that bug, but there may be others as well, so this is a good catch all to tidy up any 'unfinished' selection boxes
			if (this._box != null) {
				L.DomUtil.remove(this._box);
			}
			this._moved = false;
		},
		
		_start: function(e) {
			if (this._moved) {
				this._finish();
			}
			this._resetState();

			L.DomUtil.disableTextSelection();
			L.DomUtil.disableImageDrag();
			this._map.scrollWheelZoom.disable();
			this._map.touchZoom.disable();
			this._map.doubleClickZoom.disable();

			this._startPoint = this._map.mouseEventToContainerPoint(e);

			L.DomEvent.on(document, {
				contextmenu: L.DomEvent.stop,
				mousemove: this._onMouseMove,
				mouseup: this._onMouseUp,
				keydown: this._onKeyDown
			}, this);

			this._manager.start();
		},
		
		_update: function(e) {
			if (!this._moved) {
				this._moved = true;
				
				if (this._box != null) {
					//must be leftover from a previous drag
				    L.DomUtil.remove(this._box);
				}

				this._box = L.DomUtil.create('div', 'leaflet-zoom-box', this._mapcontainer);
				L.DomUtil.addClass(this._mapcontainer, 'leaflet-crosshair');
			}

			this._point = this._map.mouseEventToContainerPoint(e);

			var bounds = new L.Bounds(this._point, this._startPoint),
				size = bounds.getSize();

			L.DomUtil.setPosition(this._box, bounds.min);

			this._box.style.width  = size.x + 'px';
			this._box.style.height = size.y + 'px';

			if (this.options.highlightOnDrag) {
				var latLngBounds = new L.LatLngBounds(
					this._map.containerPointToLatLng(this._startPoint),
					this._map.containerPointToLatLng(this._point));

				this._manager.update(latLngBounds);
			}
		},

		_finish: function () {
			if (this._moved) {
				L.DomUtil.remove(this._box);
				L.DomUtil.removeClass(this._mapcontainer, 'leaflet-crosshair');
				
			}

			L.DomUtil.enableTextSelection();
			L.DomUtil.enableImageDrag();
			this._map.scrollWheelZoom.enable();
			this._map.touchZoom.enable();
			this._map.doubleClickZoom.enable();

			L.DomEvent.off(document, {
				contextmenu: L.DomEvent.stop,
				mousemove: this._onMouseMove,
				mouseup: this._onMouseUp,
				keydown: this._onKeyDown
			}, this);

			if (this._moved) {
				// Postpone to next JS tick so internal click event handling
				// still see it as "moved".
				setTimeout(L.bind(this._resetState, this), 0);

				var bounds = new L.LatLngBounds(
					this._map.containerPointToLatLng(this._startPoint),
					this._map.containerPointToLatLng(this._point));
				
				if (!this.options.highlightOnDrag) {
					//if we aren't highlighting on drag, fool the manager into updating
					this._manager.update(bounds);
				}
				this._manager.finish(bounds);
			}
		},
		
		getSelectedMarkers: function() {
			return this._manager.getSelectedMarkers();
		},

		_onMouseDown: function (e) {
			if (!this._isEnabled() || (e.button !== 0)) {
				return false;
			}
			
			this._start(e);
		},

		_onMouseMove: function (e) {
			this._update(e);
		},

		_onMouseUp: function (e) {
			if ((e.which !== 1) && (e.button !== 1)) { return; }
			this._finish();
		},

		_onKeyDown: function (e) {
			if (e.keyCode === 27) {
				this._finish();
			}
		}
	});
	BoxSelector.Actions = {};
	BoxSelector.Actions.alert = function() {
		return function(selectedMarkers) {
		    var output = "";
			for (var i = 0; i < selectedMarkers.length; i++) {
			    var marker = selectedMarkers[i];

			    if (marker.feature == undefined)  // 
			    {
			        output += marker.markerTip;
			        output += ": ";
			        output += marker.markerID;
			        output += "\n";
			    }
			    else if (marker.feature != undefined) {
			        if (marker.feature.geometry.type == "MultiPolygon") //面
			        {
			            if (marker.feature.id != undefined) {
                            var strs = new Array();
			                    strs = marker.feature.id.split(","); //字符分割 
			                var id = $.trim(strs[0]);
			                var name = strs[2];

			                output += name; 
			                output += ": ";
			                output += id; 
			                output += "\n";
			            }			           
			        }
			        else if (marker.feature.geometry.type == "LineString") //线
			        {
			            if (marker.feature.name != undefined)
			            {
			                output += marker.feature.name;
			                output += ": ";
			                output += $.trim(marker.feature.id); 
			                output += "\n";
			            }			      			            
			        }				   
			    }
			}
			alert(output);
		}
	}	
	return BoxSelector;
}, window));
