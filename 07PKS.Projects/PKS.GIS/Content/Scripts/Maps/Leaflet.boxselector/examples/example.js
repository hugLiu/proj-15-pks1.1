var group = new L.LayerGroup();

var markers = {
	abc: [51.503984, -0.118253],
	def: [51.497466, -0.124905],
	ghi: [51.506061, -0.116966],
	jkl: [51.499102, -0.124648],
	mno: [51.503797, -0.143649],
	pqr: [51.530535, -0.153362],
	stu: [51.500784, -0.143052],
	vwx: [51.497770, -0.101477]
};

Object.keys(markers).forEach(function(name) {
	var latLng = markers[name];
	var marker = L.marker(latLng);
	marker.name = name;
	marker.addTo(group);
});

var streets = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
	attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
});

var map = L.map('map', {
	center: [51.515, -0.13],
	zoom: 13,
	layers: [streets, group]
});

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
