(function (q) {

    //添加control实施显示
    function x(a) {
        a = L.control({
            position: "bottomright"
        });
        a.onAdd = function (a) {
            this._div = L.DomUtil.create("div", "");
            this.update();
            return this._div
        }
            ;
        a.update = function (a) {
            a && (this._div.innerHTML = '<div style="padding:10px;opacity:0.8;background:#BAC2D8;border-radius:5px;color:#12385F;font-size:12px;font-weight:bolder;">(' + p(a.latlng.lat) + "," + p(a.latlng.lng) + ")</div>")
        }
            ;
        a.addTo(map);
        return a
    }

    //经纬度小数转化为度分秒
    function p(a) {
        var b = Math.floor(a)
            , c = 60 * (a - b);
        a = Math.floor(c);
        c = Math.round(60 * (c - a));
        60 == c && (a++ ,
            c = 0);
        60 == a && (b++ ,
            a = 0);
        return "" + b + "°" + a + "'" + c + '"'
    }

    q.reverseColor0x = function (color0x) {
        var prefix = "#";
        var c0xMax = 15;
        var clr = color0x.replace(prefix, "");
        var c0x, b = [];
        for (var i = 0, j = clr.length; i < j; i++) {
            c0x = parseInt(clr[i], 16);
            b.push((c0xMax - c0x).toString(16));
        }
        return "#" + b.join("");
    };

    q.layerCon = {
        boSvc: pksGlobalStore.config.apiServiceUrl + "BO2Service",
        esSvc: pksGlobalStore.config.apiServiceUrl + "SearchService",
        smSvc: pksGlobalStore.config.apiServiceUrl + "SemanticService/Segment",
        renderSvc: pksGlobalStore.config.portalSiteUrl + "/Render/Content",
        //支持的BOTS
        bots: {
            "盆地": 1,
            "一级构造单元": 2,
            "二级构造单元": 3,
            "地震工区": 4,
            "圈闭": 5,
            "井": 6,
        },
        botMeta: {
            盆地: null,
            一级构造单元: null,
            二级构造单元: null,
            井: null,
            圈闭: null,
            地震工区: null
        },
        fieldMeta: {
            盆地: {
                display: null,
                filter: null
            },
            一级构造单元: {
                display: null,
                filter: null
            },
            二级构造单元: {
                display: null,
                filter: null
            },
            井: {
                display: null,
                filter: null
            },
            圈闭: {
                display: null,
                filter: null
            },
            地震工区: {
                display: null,
                filter: null
            }
        },
        manyFieldsOneRow: 2,
        displayType: ["Data", "Both"],
        filterType: ["Filter", "Both"],
        botFilterTypeConfig: {
            sidebarTrap: "圈闭",
            sidebarWell: "井",
            sidebarWorkArea: "地震工区"
        },
        onReverseFeatureColor: function (event, layer, clrArray) {
            var opt = {};
            var type = typeof clrArray;
            var target = event.target;
            var rawOpt = target && target.options;
            switch (type) {
                case "string":
                    opt[clrArray] = q.reverseColor0x(rawOpt[clrArray]);
                    break;
                default:
                    for (var i = 0, j = clrArray.length; i < j; i++) {
                        opt[clrArray[i]] = q.reverseColor0x(rawOpt[clrArray[i]]);
                    }
            }
            if (!layer || !layer.setStyle) return;
            layer.setStyle(opt);
        },
        getMetaDataOfAllBot: function (bots) {
            for (var i = 0, j = bots.length; i < j; i++) {
                layerCon.getBotMetaData(bots[i]);
            }
            return layerCon.botMeta;
        },
        getBotMetaData: function (bot) {
            var meta = layerCon.botMeta[bot];
            if (meta == null) {
                layerCon.queryMetaOfBot(bot)
                    .done(function (data) {
                        meta = layerCon.botMeta[bot] = data;
                    }).fail(function (data) {
                        console && console.log && console.log(data);
                    })
            }
            return meta
        },
        queryMetaOfBot: function (bot) {
            var url = layerCon.boSvc + "/GetBOT?bot=" + encodeURIComponent(bot);
            var data = bot;
            var query = $.ajax({
                url: url,
                type: "GET",
                dataType: "json",
                async: false
            });
            return query;
        },
        getBos: function (args) {
            var url = layerCon.boSvc + "/FilterBOs";
            var data = JSON.stringify(args);
            var query = $.ajax({
                type: "POST",
                url: url,
                data: data,
                dataType: "json",
                contentType: 'application/json'
            });
            return query;
        },
        getBots: function () {
            var url = layerCon.boSvc + "/FilterBOTs";
            var data = {
                "query": {},
                "fields": { "name": 1 },
            };
            var query = $.ajax({
                type: "POST",
                url: url,
                data: JSON.stringify(data),
                dataType: "json",
                contentType: "application/json"
            });
            return query;
        },
        isTypeField: function (fieldMeta, type) {
            var res = false;
            var scenario = fieldMeta["scenario"];
            for (var i = 0, j = type.length; i < j; i++) {
                if (type[i] == scenario) {
                    res = true;
                    break;
                }
            }
            return res;
        },
        getFieldMetaForCommon: function (botMeta, type, cacheKey) {
            var res = [];
            if (!botMeta)
                return res;
            //query cache
            if (layerCon.fieldMeta[botMeta["name"]][cacheKey])
                return layerCon.fieldMeta[botMeta["name"]][cacheKey];
            //  tailor
            var fields = botMeta.properties;
            if (!fields) return res;
            for (var i = 0, j = fields.length; i < j; i++) {
                if (layerCon.isTypeField(fields[i], type)) {
                    res.push(fields[i]);
                }
            }
            //caching
            layerCon.fieldMeta[botMeta["name"]][cacheKey] = res;
            return res;
        },
        getFieldMetaForDisplay: function (botMeta) {
            var displayType = layerCon.displayType;
            var cacheKey = "display";
            return layerCon.getFieldMetaForCommon(botMeta, displayType, cacheKey);
        },
        getFieldMetaForFilter: function (botMeta) {
            var displayType = layerCon.filterType;
            var cacheKey = "filter";
            return layerCon.getFieldMetaForCommon(botMeta, displayType, cacheKey);
        },
        getContextMenuItemCB: function (query) {
            return function () {
                var url = layerCon.esSvc + "/ESSearch";
                $.ajax({
                    url: url,
                    type: "POST",
                    data: query,
                    dataType: "json",
                    contentType: 'application/json'
                }).done(function (data, status, xhr) {
                    var hits = data["hits"]["hits"];
                    if (hits.length && hits.length > 0) {
                        var iiid = hits[0]["_source"]["iiid"];
                        window.open(layerCon.renderSvc + "?iiid=" + iiid);
                    } else {
                        alert("查不到该目标的该成果！");
                    }
                }).fail(function (data, status, xhr) {
                    console && console.log("访问" + url + "服务失败");
                })
            };
        },
        buildContextMenuItems: function (feature, config) {
            var res = [];
            var pt, item, pts = config["keypts"], must, o;
            var id = config.code + ".keyword";
            for (var i = 0, j = pts.length; i < j; i++) {
                pt = pts[i];
                must = pt["query"]["query"]["bool"]["must"];
                o = must[must.length - 1];
                o["term"][id] = feature.bo;
                item = {};
                item.text = pt["title"];
                item.callback = layerCon.getContextMenuItemCB(JSON.stringify(pt["query"]));
                res.push(item);
            }
            return res;
        },
        buildContextMenuOfFeature: function (feature, config) {
            var menuItems = [];
            menuItems.push({
                text: '<span style="color:red;"><b>' + feature.bo + '</b></span>',
                disabled: true
            });
            menuItems.push("-");
            menuItems = menuItems.concat(layerCon.buildContextMenuItems(feature, config));
            return {
                contextmenu: true,
                contextmenuWidth: 140,
                contextmenuItems: menuItems
            };
        },
        setOpts: function (source, styleFn, styleMap, colorKey, ctxConfig, action) {
            L.setOptions(source, {
                style: function (feature) {
                    return styleFn(feature, styleMap);
                },
                onEachFeature: function (feature, layer) {
                    layer.bindTooltip(feature.bo, {
                        direction: 'top',
                        permanent: true,
                        interactive: false,
                        sticky: false//随鼠标移动
                    });
                    layer.on("mouseover", function (e) {
                        layerCon.onReverseFeatureColor(e, layer, colorKey);
                    }).on("mouseout", function (e) {
                        layerCon.onReverseFeatureColor(e, layer, colorKey);
                    });
                    if (ctxConfig)
                        layer.bindContextMenu(layerCon.buildContextMenuOfFeature(feature, ctxConfig));
                    if (action && typeof action == 'function')
                        action(feature, layer);
                }
            });
        },
        setOptsOfPt: function (source, styleFn, styleMap, colorKey, ctxConfig, action) {
            L.setOptions(source, {
                pointToLayer: function (feature, latlng) {
                    var style = styleFn(feature, styleMap);
                    var contextMenu = layerCon.buildContextMenuOfFeature(feature, ctxConfig);
                    var opt = $.extend({}, style, contextMenu);
                    var marker = L.circleMarker(latlng, opt);
                    marker.title = feature.bo;
                    marker.markID = feature.boid;
                    marker.on("mouseover", function (e) {
                        layerCon.onReverseFeatureColor(e, marker, colorKey);
                    }).on("mouseout", function (e) {
                        layerCon.onReverseFeatureColor(e, marker, colorKey);
                    });
                    if (action && typeof action == 'function')
                        action(feature, latlng, marker);
                    return marker;
                }
            })
        },
        setOptsOfTrap: function (source, ctxConfig, action) {
            L.setOptions(source, {
                onEachFeature: function (feature, layer) {
                    layer.bindTooltip(feature.bo, {
                        direction: 'top',
                        interactive: false,
                        sticky: false//随鼠标移动
                    });
                    if (ctxConfig)
                        layer.bindContextMenu(layerCon.buildContextMenuOfFeature(feature, ctxConfig));
                    if (action && typeof action == 'function')
                        action(feature, layer);
                }
            });
        },
        buildPopup: function (feature) {
            var key, value;
            var fieldsMeta = layerCon.getFieldMetaForDisplay(layerCon.getBotMetaData(feature.bot));
            var html = '<div>';
            html += '<h3>' + feature.bo + '</h3>';
            html += '<table class="table table-bordered popup-table">';
            for (var i = 0, j = fieldsMeta.length; i < j; i = i + layerCon.manyFieldsOneRow) {
                html += '<tr>';
                for (k = 0; k < layerCon.manyFieldsOneRow; k++) {
                    if (k + i < fieldsMeta.length) {
                        key = fieldsMeta[i + k]["displayname"];
                        value = (feature.properties[fieldsMeta[i + k]["name"]] || "");
                    } else {
                        key = value = "";
                    }
                    html += '<td class="popup-label" style="font-size:13px !important;font-weight:bold">' + key + '</td>';
                    html += '<td class="popup-label">' + value + '</td>';
                }
                html += '</tr>';
            }
            html += '</table>';
            html += '</div>';
            return html;
        },
        buildFilterItem: function (fieldMeta) {
            var criterias = fieldMeta.options;
            var html = '<div class="row" style="margin-bottom:10px;">';
            html += '<label for="basic" class="control-label" style="margin-left:15px;width:70px">' + fieldMeta["displayname"] + '</label>';
            html += '<span>';
            html += '<select id="' + fieldMeta["name"] + '" class="selectpicker" name="test multiselect" title="请选择" multiple>';
            for (var i = 0, j = criterias.length; i < j; i++) {
                html += '<option value="' + criterias[i] + '" selected>' + criterias[i] + '</option>';
            }
            html += '</select>'
            html += '<span>';
            html += '</div>';
            return html;
        },
        buildFilterItemOfBot: function (bot) {
            var fieldMetas = layerCon.getFieldMetaForFilter(layerCon.getBotMetaData(bot));
            var html = '<h4>' + bot + '的过滤</h4>';
            for (var i = 0, j = fieldMetas.length; i < j; i++) {
                html += layerCon.buildFilterItem(fieldMetas[i]);
            }
            return html;
        },
        buildAllFilter: function (filterTypeConfig) {
            var html = "";
            for (var key in filterTypeConfig) {
                if (filterTypeConfig.hasOwnProperty(key)) {
                    html = layerCon.buildFilterItemOfBot(filterTypeConfig[key]);
                    $("#" + key).append(html);
                }
            }
        },
        getBotFilterScript: function (target) {
            var map = layerCon.getFilterValues(target);
            var bot = layerCon.botFilterTypeConfig[$(target).closest(".filter-sidebar").attr("id")];
            var scp = "";
            for (var key in map) {
                if (map.hasOwnProperty(key)) {
                    scp += layerCon.getFilterScript(key, map[key], bot);
                    scp += "&&";
                }
            }
            scp = scp.replace(/&&$/g, "");
            return scp;
        },
        getFilterScript: function (key, criterias, bot) {
            var scp = "";
            var t = [];
            var criteria = "";
            if (!criterias) {
                scp = layerCon.getNullFilterScript(key, bot);
            } else {
                scp = layerCon.getFilterScriptCommon(key, criterias);
            }
            return scp;
        },
        getNullFilterScript: function (key, bot) {
            var criterias = layerCon.getFilterCriteris(key, bot);
            var scp = layerCon.getFilterScriptCommon(key, criterias);
            return "(!" + scp + ")";
        },
        getFilterScriptCommon: function (key, criterias) {
            var scp = "";
            var t = [];
            var criteria = "";
            for (var i = 0, j = criterias.length; i < j; i++) {
                // should do some check before concat
                criteria = 'f.properties["' + key + '"] == "' + criterias[i] + '"';
                t.push(criteria);
            }
            scp = "(" + t.join(" || ") + ")";
            return scp;
        },
        getFilterCriteris: function (key, bot) {
            var filterFields = layerCon.getFieldMetaForFilter(layerCon.getBotMetaData(bot));
            for (var i = 0, j = filterFields.length; i < j; i++) {
                if (filterFields[i]["name"] == key)
                    return filterFields[i]["options"];
            }
        },
        getFilterValues: function (target) {
            var map = {};
            $(target).closest(".filter-sidebar")
                .find(".selectpicker")
                .each(function (i, el) {
                    map[$(el).attr("id")] = $(el).val();
                });
            return map;
        },
        getSourceById: function (botid) {
            var source = null;
            switch (botid) {
                case "sidebarTrap":
                    source = cnoocGis.h.traps;
                    break;
                case "sidebarWell":
                    source = cnoocGis.h.wells;
                    break;
                case "sidebarWorkArea":
                    source = cnoocGis.h.workAreas;
                    break;
                default:
                    // may be  should throw error
                    source = null;
            }
            return source
        }
    };

    layerCon.buildAllFilter(layerCon.botFilterTypeConfig);

    q.cnoocGis = {};

    q.cnoocGis.h = {
        basins: L.geoJSON(),
        firstSus: L.geoJSON(),
        secondSus: L.geoJSON(),
        wells: L.geoJSON(),
        traps: L.geoJSON(),
        workAreas: L.geoJSON()
    };
    var sl = {
        "地质目标": {
            "圈闭&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img id=\"imgTrapFilter\" src=\"../Content/Images/Icon/funnel.png\"  />": q.cnoocGis.h.traps,
            "井&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img id=\"imgWellFilter\" src=\"../Content/Images/Icon/funnel.png\" />": q.cnoocGis.h.wells,
            "地震工区&nbsp;<img id=\"imgWorkAreaFilter\" src=\"../Content/Images/Icon/funnel.png\"  />": q.cnoocGis.h.workAreas
        }
    };
    q.cnoocGis.initialMap = function () {
        map = L.map("mapContent", {
            center: [20.0508, 116.2066],
            //center: [51.2, 7],
            zoom: 6,
            zoomControl: !1,
            crs: L.CRS.EPSG4326,
            measureControl: !0,
            layers: [q.cnoocGis.h.basins, q.cnoocGis.h.firstSus, q.cnoocGis.h.secondSus, q.cnoocGis.h.traps, q.cnoocGis.h.workAreas, q.cnoocGis.h.wells],
            contextmenu: true,
            maxBounds: [[8.4375, 91], [43.59375, 145]]
        });
        //var dtUrl1 = "http://10.78.165.180:8080/geoserver/gwc/service/tms/1.0.0/cite%3Achina@EPSG%3A4326@jpeg/{z}/{x}/{y}.jpg";
        var dtUrl1 = "http://shangetu0.map.bdimg.com/it/u=x={x};y={y};z={z};v=017;type=web&fm=44&udt=20130712";
        //var dtUrl1 = "http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png";
        //var dtUrl2 = "http://10.78.165.180:8081/newtile/Tiles/Z{z}/{x}/{y}/{x}_{y}.png";
        var dtUrl2 = "http://shangetu1.map.bdimg.com/it/u=x={x};y={y};z={z};v=017;type=web&fm=44&udt=20130712";
        var tbDiv = "<div id='tdiv' style=\"padding-right:10px;\"></div> &copy <a>总公司地理信息系统平台v1.0</a>&nbsp&nbsp&nbsp";
        var a = {
            "影像底图": L.tileLayer(dtUrl1, {
                minZoom: 5,
                maxZoom: 13,
                attribution: tbDiv,
                tms: !0
            }),
            "电子海图": L.tileLayer(dtUrl2, {
                minZoom: 7,
                maxZoom: 14,
                attribution: tbDiv
            })
        }
            , b = {
                "影像底图": L.tileLayer(dtUrl1, {
                    minZoom: 3,
                    attribution: tbDiv,
                    tms: !0
                }),
                "电子海图": L.tileLayer(dtUrl2, {
                    minZoom: 3,
                    attribution: tbDiv
                })
            };
        map.addLayer(a["影像底图"]);

        L.control.groupedLayers(a, sl).addTo(map);

        var c = (new L.Control.MiniMap(b["影像底图"], {
            zoomLevelFixed: 5,
            toggleDisplay: !0
        })).addTo(map);
        L.Map.prototype.setCrs = function (a) {
            this.options.crs = a
        };

        map.on("baselayerchange", function (a) {
            var d = map.getCenter();
            if ("影像底图" == a.name) {
                map.setCrs(L.CRS.EPSG4326);
                c._layer._map.options.crs = L.CRS.EPSG4326;
                map.setMaxBounds([[8.4375, 91], [43.59375, 145]]);
                map.setView(d, "6");
            }
            else {
                map.setCrs(L.CRS.EPSG3857),
                    map.setView(d, "7"),
                    c._layer._map.options.crs = L.CRS.EPSG3857,
                    map.setMaxBounds([[3, 106.907], [41.34805555, 133]]);
            }
            c.changeLayer(b[a.name]);
        });

        q.cnoocGis.h.wells.remove();

        map.on("zoomend", function (e) {
            //if (e.target._zoom > 3) {
            //    switchTip(q.cnoocGis.h.basins, true);
            //    switchTip(q.cnoocGis.h.firstSus, true);
            //    switchTip(q.cnoocGis.h.secondSus, true);
            //} else {
            //    switchTip(q.cnoocGis.h.basins, false);
            //    switchTip(q.cnoocGis.h.firstSus, false);
            //    switchTip(q.cnoocGis.h.secondSus, false);
            //}
            //if (e.target._zoom > 5) {
            //    switchTip(q.cnoocGis.h.traps, false);
            //    switchTip(q.cnoocGis.h.traps, true);
            //} else {
            //    switchTip(q.cnoocGis.h.traps, false);
            //    switchTip(q.cnoocGis.h.workAreas, false);
            //}
            //if (e.target._zoom > 6) {
            //    switchTip(q.cnoocGis.h.workAreas, true);
            //} else {
            //    switchTip(q.cnoocGis.h.workAreas, false);
            //}
            if (e.target._zoom > 7) {
                q.cnoocGis.h.wells.addTo(map);
                switchTip(q.cnoocGis.h.wells, true);
            } else {
                switchTip(q.cnoocGis.h.wells, false);
                q.cnoocGis.h.wells.remove();
            }
        });

        function switchTip(gl, nf) {
            gl.eachLayer(function (layer) {
                if (nf) {
                    if (layer.getTooltip())
                        return;
                    var feature = layer.feature;
                    layer.bindTooltip(feature.bo, {
                        direction: 'top',
                        permanent: true,
                        interactive: false,
                        sticky: false//随鼠标移动
                    });
                } else {
                    if (layer.getTooltip())
                        layer.unbindTooltip();
                }
            })
        }

        L.control.scale().addTo(map);
        //添加control指定位置定位
        //map.addControl(new L.Control.Position);

        L.Control.zoomHome({
            position: "topright"
        }).addTo(map);

        //框选
        /*
        a = {
            actions: {
                alert: {
                    display: "返回所选图层ID",
                    action: L.Control.BoxSelector.Actions.alert()
                }
            }
        };
        (new L.Control.BoxSelector(a)).addTo(map);
        */

        var d = x();
        map.on("mousemove", function (a) {
            d.update(a)
        });



        var sc = L.control.search({
            layer: [q.cnoocGis.h.wells, q.cnoocGis.h.traps, q.cnoocGis.h.workAreas],
            initial: false,
            propertyName: 'bo',//,
            moveToLocation: function (latlng, title, map) {
                //map.fitBounds( latlng.layer.getBounds() );
                var zoom = -1;
                if (latlng.layer.getBounds) {
                    zoom = map.getBoundsZoom(latlng.layer.getBounds());
                } else {
                    zoom = 16;
                }
                map.setView(latlng, zoom); // access the zoom
            }
            /*
            buildTip: function (text, val) {
                var type = val.layer.feature.bo;
                return '<a href="#" class="' + type + '">' + text + '<b>' + type + '</b></a>';
            }
             */
        });

        var preS = null;
        var reco = '';
        sc.on('search:locationfound', function (e) {

            //console.log('search:locationfound', );

            //map.removeLayer(this._markerSearch)
            if (preS && reco)
                preS.layer.setStyle({ fillColor: reco });
            preS = e;
            reco = preS.layer.options.fillColor;
            if (e.layer.options.fillColor)
                e.layer.setStyle({ fillColor: "#007ACC" });
            if (e.layer._popup)
                e.layer.closePopup();

        });

        sc.addTo(map);

        q.cnoocGis.sidebarTrap = L.control.sidebar('sidebarTrap', {
            closeButton: true,
            autoPan: false,
            position: 'topleft'
        });
        //map.addControl(sidebar);
        q.cnoocGis.sidebarTrap.addTo(map);
        //sidebar.show();

        q.cnoocGis.sidebarWell = L.control.sidebar('sidebarWell', {
            closeButton: true,
            autoPan: false,
            position: 'topleft'
        });
        //map.addControl(sidebar);
        q.cnoocGis.sidebarWell.addTo(map);
        //sidebar.show();

        q.cnoocGis.sidebarWorkArea = L.control.sidebar('sidebarWorkArea', {
            closeButton: true,
            autoPan: false,
            position: 'topleft'
        });
        //map.addControl(sidebar);
        q.cnoocGis.sidebarWorkArea.addTo(map);
        //sidebar.show();

        //$('.leaflet-control-layers span').unbind();
        //$('.leaflet-control-layers label').bind('click');
        $("#imgTrapFilter").click(function (event) {

            cnoocGis.sidebarWell.hide();
            cnoocGis.sidebarWorkArea.hide();
            cnoocGis.sidebarTrap.toggle();
            event.stopPropagation();
        });
        $("#imgWellFilter").click(function (event) {
            cnoocGis.sidebarTrap.hide();
            cnoocGis.sidebarWorkArea.hide();
            cnoocGis.sidebarWell.toggle();
            event.stopPropagation();
        });
        $("#imgWorkAreaFilter").click(function (event) {
            cnoocGis.sidebarTrap.hide();
            cnoocGis.sidebarWell.hide();
            cnoocGis.sidebarWorkArea.toggle();
            event.stopPropagation();
        });

        q.layerCon.getSourceByMap = function (d) {
            var o = null;
            switch (d.name) {
                case "盆地":
                    o = q.cnoocGis.basins;
                    break;
                case "一级构造单元":
                    o = q.cnoocGis.firstSus;
                    break;
                case "二级构造单元":
                    o = q.cnoocGis.secondSus;
                    break;
                case "圈闭":
                    o = q.cnoocGis.traps;
                    break;
                case "地震工区":
                    o = q.cnoocGis.workAreas;
                    break;
                case "井":
                    o = q.cnoocGis.wells;
                    break;
            }
            return o;
        };

        q.layerCon.getPreProcessor = function (d) {
            var f = null;
            switch (d.name) {
                case "圈闭":
                    f = bizGeometryCollectionsToGeoJson;
                    break;
                case "井":
                    f = bizPointsToGeoJson;
                    break;
                default:
                    f = bizPolygonsToGeoJson;
            }
            return f;
        };
    };

    layerCon.getBots().then(function (data) {
        data = data.filter(function (value) {
            return layerCon.bots[value.name] > 0;
        });
        data.sort(function (x, y) {
            return layerCon.bots[x.name] - layerCon.bots[y.name];
        });
        var i = 0, j = data.length, d = void 0, qArg = void 0;
        for (; i < j; i++) {
            d = data[i];
            qArg = {
                "query": {
                    "bot": d.name
                },
                "sort": { "boid": 1 },
            };
            layerCon.getBos(qArg).done(function (ds) {
                var source = layerCon.getSourceByMap(d);
                if (!source) {
                    source = L.geoJSON();
                    source.addTo(map);
                }
                var por = layerCon.getPreProcessor(d);
                source.addData(por(ds));
            });
        }
    }).fail(function (data, status, xhr) {
        console && console.log && console.log(data);
    });

    q.cnoocGis.Legend2 = function () {
        var a = L.control({
            position: "left"
        });
        a.onAdd = function (a) {
            this._div = L.DomUtil.create("div", "");
            this.update();
            return this._div
        }
            ;
        a.update = function (a) {
            this._div.innerHTML = '<div style="padding:10px;background:rgba(255, 255, 255, 0.2) !important;filter: alpha(opacity=20);  background:#BAC2D8;border-radius:5px;color:#12385F;font-size:14px;font-weight:bold;"><span style="color:#fafc0d;">————天然气</span></br><span  style="color:red;">————混输</span></br><span  style="color:#000000;">————原油</span></br><span  style="color:#0000EE;">————注水</span></div>'
        }
            ;
        a.addTo(map);
        return a
    },

        //显示图示     
        q.cnoocGis.Legend = function () {
            var a = L.control({
                position: "bottomleft"
            });
            a.onAdd = function (a) {
                this._div = L.DomUtil.create("div", "");
                this.update();
                return this._div
            }
                ;
            a.update = function (a) {
                this._div.innerHTML = '<div style="padding:10px;background:rgba(255, 255, 255, 0.2) !important;filter: alpha(opacity=20);  background:#BAC2D8;border-radius:5px;color:#12385F;font-size:14px;font-weight:bold;"><span style="color:#fafc0d;">————天然气</span></br><span  style="color:red;">————混输</span></br><span  style="color:#000000;">————原油</span></br><span  style="color:#0000EE;">————注水</span></div>'
            }
                ;
            a.addTo(map);
            return a
        },

        q.cnoocGis.showtime = function () {
            var a, b, c, d, e, g, f;
            a = new Date;
            switch (a.getDay()) {
                case 0:
                    f = "星期日";
                    break;
                case 1:
                    f = "星期一";
                    break;
                case 2:
                    f = "星期二";
                    break;
                case 3:
                    f = "星期三";
                    break;
                case 4:
                    f = "星期四";
                    break;
                case 5:
                    f = "星期五";
                    break;
                case 6:
                    f = "星期六";
                    break;
                case 7:
                    f = "星期日"
            }
            d = a.getFullYear();
            e = a.getMonth() + 1;
            g = a.getDate();
            b = a.getHours();
            c = a.getMinutes();
            a = a.getSeconds();
            $("#tdiv").text(d + "年" + e + "月" + g + "日 " + f + " " + b + ":" + c + ":" + a);
            setTimeout("cnoocGis.showtime();", 1E3);
        },

        q.cnoocGis.locationPoint = function () {
            var a = L.marker([39.3530555, 119.641111]);
            a.on("click", function (a) {
                a.target && map.setView([40.1013888, 121.151666], 9)
            });
            var b = !1;
            map.on("zoomend", function () {
                6 == map._zoom ? 0 == b && (a.addTo(map),
                    b = !0) : 6 < map._zoom && (a.remove(),
                        b = !1)
            })
        }
}
)(window);
