This is a simple leaflet control that allows you to draw a box to select a number of markers. It then allows you to perform an action on those markers (e.g. export to file). This control is heavily influenced by the built-in box-zoom (shift + click and drag draws a box and then zooms to it) so the selection area looks very similar.

[Live demo](https://tstibbs.github.io/Leaflet.BoxSelector/examples/index.html)

[Live demo with selectable clusters](https://tstibbs.github.io/Leaflet.BoxSelector/examples/clustering.html)

## How to use it

A built in action that just displays the markers in an 'alert' popup is registered for testing purposes, so to get started all you need to do is add the control:
```
(new L.Control.BoxSelector()).addTo(map);
```

If you want to register any additional actions, add them to `options.actions` like this:
```
var options = {
	actions: {
		alert: {
			display: "Display selected coords",
			action:	L.Control.BoxSelector.Actions.alert()
		},
		GPX: {
			display: "Export to GPX",
			action: L.Control.BoxSelector.Actions.gpx('points.gpx')
		}
	}
};
(new L.Control.BoxSelector(options)).addTo(map);
```

Note that the `alert` and `gpx` actions are created via factory functions which return the action functions.

### Adding custom actions

Actions are simply functions which take a single argument which is an array of `L.Marker` objects. Passing a function as the value of  `options.[action_name].action` will register it as an action and it will be available in the drop down to select when you have selected markers. If you create an action which you think may be useful to others, please consider submitting a pull request to have it included in this repository.
