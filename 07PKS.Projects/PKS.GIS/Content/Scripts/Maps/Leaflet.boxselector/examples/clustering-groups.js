var parentGroup = new L.MarkerClusterGroup();

var markers1 = {
	abc: [51.503984, -0.118253],
	def: [51.497466, -0.124905],
	ghi: [51.506061, -0.116966],
	jkl: [51.499102, -0.124648]
};
var markers2 = {
	mno: [51.503797, -0.143649],
	pqr: [51.530535, -0.153362],
	stu: [51.500784, -0.143052],
	vwx: [51.497770, -0.101477]
};

var arrayOfMarkers1 = [];
Object.keys(markers1).forEach(function(name) {
	var latLng = markers1[name];
	var marker = L.marker(latLng);
	marker.name = name;
	arrayOfMarkers1.push(marker);
	//marker.addTo(parentGroup);
});
var arrayOfMarkers2 = [];
Object.keys(markers2).forEach(function(name) {
	var latLng = markers2[name];
	var marker = L.marker(latLng);
	marker.name = name;
	arrayOfMarkers2.push(marker);
	//marker.addTo(parentGroup);
});

var subGroup1 = L.featureGroup.subGroup(parentGroup, arrayOfMarkers1);
var subGroup2 = L.featureGroup.subGroup(parentGroup, arrayOfMarkers2);

var streets = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
	attribution: '&copy; <a href="https://osm.org/copyright">OpenStreetMap</a> contributors'
});

var map = L.map('map', {
	center: [51.515, -0.13],
	zoom: 13,
	maxZoom: 13,
	layers: [streets, parentGroup]
});
subGroup1.addTo(map);
subGroup2.addTo(map);

var options = {
	actions: {
		alert: {
			display: "Display selected coords",
			action:	L.Control.BoxSelector.Actions.alert()
		}
	},
	highlightOnDrag: false
};
(new L.Control.BoxSelector(options)).addTo(map);
