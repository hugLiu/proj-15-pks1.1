//封装GIS控制所有操作
var gisCtrl = {
    //配置
    config: undefined,
    //地图
    map: undefined,
    //底图
    baseMaps: {},
    //Mini底图
    miniMaps: {},
    //分组图层
    groupedLayers: undefined,
    //zoom变化时显示或隐藏的图层数组
    zoomFilterLayers: [],
    //BOT数组
    bots: [],
    //BOT成员
    botMembers: {
        "name": "井",
        "code": "well",
        "ishbo": false,
        "locationtype": "Point",
        "properties": [{
            "name": "二级构造单元1",
            "displayname": "二级构造单元",
            "type": "String",
            "options": ["海南隆起", "北部隆起"],
            "sequence": 1,
            "scenario": "Data",
        }],
        bos: [{
            "bot": "盆地",
            "bo": "珠江口盆地",
            "alias": [],
            "properties": {
                "二级构造单元1": "白云凹陷",
                "二级构造单元2": "白云凹陷",
                "作业方式1": "自营",
                "作业方式2": "自营",
                "井型": "直井",
                "井别": "预探井",
                "水深": "653",
                "水深类型": "半深水井",
                "设计井深": "主要：珠江组下段ZJ210深水重力流沉积砂岩体、ZJ110陆架三角洲前缘沉积砂岩体",
                "设计目的层": "3076",
                "设计完钻层位": "珠海组",
                "开钻日期1": "2014/7/28 0:00:00",
                "开钻日期2": "2014",
                "完钻日期": "2014/8/11 0:00:00",
                "完钻层位": "前古近系",
                "完钻深度": "3350",
                "井况1": "油气层井",
                "井况2": "油气层井"
            },
            "location": {
                "type": "Polygon",
                "coordinates": [],
                "geometry": [],
            },
            //关联bot
            type: undefined,
            //关联GeoJson层
            layer: undefined,
        }],
        //关联层
        layer: undefined,
        //sidebar控制
        sidebar: undefined,
    },
    //BOT计数
    botCounter: 0,
    //初始化
    init: function (config) {
        this.config = config;
        this.adjustHeight();
        //leaflet支持适宜浏览器窗口resize变化
        //this.registerWindowResize();
        this.initMap();
        this.loadBOTs();
        this.addTileLayers();
        this.addMiniMap();
        this.showTime();
        this.addLatlngShow();
    },
    //AJAX POST
    ajaxPost: function (settings, success) {
        var data = settings.data;
        if (typeof data !== "string") data = JSON.stringify(data);
        $.ajax({
            type: "POST",
            url: settings.url,
            data: data,
            dataType: "json",
            contentType: 'application/json',
            success: success
        });
    },
    //获得浏览器客户端高度
    getBrowserClientHeight: function () {
        var winHeight = 0;
        if (document.documentElement && document.documentElement.clientHeight) {
            winHeight = document.documentElement.clientHeight;
        } else if (window.innerHeight) {
            winHeight = window.innerHeight;
        } else if ((document.body) && (document.body.clientHeight)) {
            winHeight = document.body.clientHeight;
        }
        return winHeight;
    },
    //调整高度
    adjustHeight: function () {
        var height = this.getBrowserClientHeight();
        $("#" + this.config.elementId).height(height - 2);
    },
    //浏览器窗口发生变化时同时变化DIV高度
    registerWindowResize: function () {
        window.onresize = function () {
            gisCtrl.adjustHeight();
        };
    },
    //删除时间为0的部分
    trimTime: function (value) {
        return value.replace(/ 0?0:0?0:0?0$/, "");
    },
    //初始化地图
    initMap: function () {
        this.map = L.map(this.config.elementId, this.config.mapOptions);
    },
    //加入底图层
    addTileLayers: function () {
        for (var i = 0; i < this.config.tileLayers.length; i++) {
            var tileConfig = this.config.tileLayers[i];
            var tileLayer = L.tileLayer(tileConfig.url, tileConfig.options);
            this.baseMaps[tileConfig.name] = tileLayer;
        }
        if (this.baseMaps.length === 0) return;
        var firstName = this.config.tileLayers[0].name;
        this.baseMaps[firstName].addTo(this.map);
    },
    //加入Mini底图
    addMiniMap: function () {
        for (var i = 0; i < this.config.tileLayers.length; i++) {
            var tileConfig = this.config.tileLayers[i];
            var tileLayer = L.tileLayer(tileConfig.url, tileConfig.miniMapOptions);
            this.miniMaps[tileConfig.name] = tileLayer;
        }
        if (this.miniMaps.length === 0) return;
        var firstName = this.config.tileLayers[0].name;
        var miniMapControl = new L.Control.MiniMap(this.miniMaps[firstName], this.config.controlOptions.miniMap);
        miniMapControl.addTo(this.map);
        var that = this;
        this.map.on("baselayerchange", function (e) {
            var map = that.map;
            var tileConfig = that.config.tileLayers.find(function (layer) {
                return layer.name === e.name;
            });
            var config = tileConfig.miniMapControl;
            map.options.crs = config.crs;
            //miniMapControl._layer._map.options.crs = L.CRS.EPSG3857;
            map.setMaxBounds(config.maxBounds);
            miniMapControl.changeLayer(that.miniMaps[e.name]);
            map.setView(center, config.zoom);
        });
    },
    //显示时间
    showTime: function () {
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
        $("#" + this.config.miniMapConfig.timeElementId).text(d + "年" + e + "月" + g + "日 " + f + " " + b + ":" + c + ":" + a);
        var that = this;
        setTimeout(function () {
            that.showTime()
        }, 1000);
    },
    //加载BOT集合
    loadBOTs: function () {
        var settings = this.config.bo_FilterBOTs;
        var that = this;
        this.ajaxPost(settings, function (bots) {
            bots = bots.filter(function (bot) {
                return that.config.getBOTConfig(bot).loaded;
            });
            bots.sort(function (x, y) {
                return that.config.getBOTConfig(x).order - that.config.getBOTConfig(y).order;
            });
            that.bots = bots;
            that.loadBOs();
        });
    },
    //加载BO集合
    loadBOs: function () {
        var settings = this.config.bo_FilterBOs;
        var bot = this.bots[this.botCounter];
        settings.setQuery(bot.name);
        var that = this;
        this.ajaxPost(settings, function (bos) {
            that.loadBOLayer(bot, bos);
        });
    },
    //载入BO层
    loadBOLayer: function (bot, bos) {
        bot.bos = bos;
        this.botCounter++;
        if (this.botCounter < this.bots.length) {
            this.loadBOs();
        }
        var geoBos = this.toGeoJson(bot, bos);
        var geoOptions;
        if (bot.locationtype === "Point") {
            geoOptions = this.buildGeoOptions(bot);
            this.buildGeoPointOptions(bot, geoOptions);
        } else {
            geoOptions = this.buildGeoOptions(bot);
        }
        var botLayer = L.geoJSON(geoBos, geoOptions);
        var botConfig = this.config.getBOTConfig(bot);
        botLayer.addTo(this.map);
        bot.layer = botLayer;
        botLayer.bot = bot;
        if (this.botCounter >= this.bots.length) {
            this.loadControlLayers();
        }
    },
    //转换为GeoJson
    toGeoJson: function (bot, bos) {
        var geoBos = [];
        for (var i = 0; i < bos.length; i++) {
            var bo = bos[i];
            bo.type = bot;
            if (!bo.location) continue;
            if (!bo.location.coordinates && !bo.location.geometries) continue;
            for (var j = 0; j < bot.properties.length; j++) {
                var metaProperty = bot.properties[j];
                if (metaProperty.type !== "ISODate") continue;
                var propertyValue = bo.properties[metaProperty.name];
                if (typeof propertyValue !== "string") continue;
                //带时间的日期最小长度为12
                if (propertyValue.length < 12) continue;
                bo.properties[metaProperty.name] = this.trimTime(propertyValue);
            }
            var geoBo = {
                type: "Feature",
                properties: {
                    bot: bot,
                    bo: bo,
                    searchText: this.buildSearchText(bo),
                },
                geometry: bo.location,
            }
            geoBos.push(geoBo);
        }
        return geoBos;
    },
    //生成GeoJson选项
    buildGeoOptions: function (bot) {
        var options = {};
        var botConfig = this.config.getBOTConfig(bot);
        var geoOptions = botConfig.geoOptions;
        if (!geoOptions) return options;
        var that = this;
        if (geoOptions.styles) {
            options.style = function (feature) {
                var styleKey = geoOptions.getStyleKey(feature, feature.properties.bo);
                return geoOptions.styles[styleKey];
            };
        }
        options.onEachFeature = function (feature, layer) {
            feature.properties.bo.layer = layer;
            if (feature.geometry.type === "GeometryCollection") {
                var children = layer.getLayers();
                children.sort(function (x, y) {
                    return L.Util.stamp(x) - L.Util.stamp(y);
                });
                for (var i = 0; i < children.length; i++) {
                    var child = children[i];
                    child.childFeature = {
                        geometry: feature.geometry.geometries[i],
                        type: 'Feature',
                        properties: feature.properties,
                    }
                    child.setStyle(child.options.style(child.childFeature));
                }
            }
            if (geoOptions.tooltip) {
                layer.bindTooltip(feature.properties.bo.bo, geoOptions.tooltip);
            }
            if (geoOptions.enablePopup) {
                var popup = that.config.controlOptions.popup;
                var popupContent = that.buildPopupContent(feature, popup);
                if (popupContent) layer.bindPopup(popupContent, popup.options);
            }
            if (geoOptions.contextMenu) {
                var contextMenu = that.buildContextMenu(feature, geoOptions.contextMenu);
                if (layer.eachLayer) {
                    layer.eachLayer(function (child) {
                        child.bindContextMenu(contextMenu);
                    });
                } else {
                    layer.bindContextMenu(contextMenu);
                }
            }
            if (feature.geometry.type !== "Point") {
                layer.on({
                    mouseover: function (e) {
                        if (e.target.setStyle) {
                            var hightLightStyle = that.config.controlOptions.hightLight.style;
                            e.target.setStyle(hightLightStyle);
                        }
                    },
                    mouseout: function (e) {
                        var layer = e.target;
                        if (!layer.setStyle) return;
                        //resetStyle有bug，级联菜单会丢失
                        //e.target.resetStyle(e.target);
                        if (layer.eachLayer) {
                            layer.eachLayer(function (child) {
                                if (child.childFeature) {
                                    child.setStyle(child.options.style(child.childFeature));
                                } else {
                                    child.setStyle(child.options.style(layer.feature));
                                }
                            });
                        } else {
                            layer.setStyle(layer.options.style(layer.feature));
                        }
                    }
                });
            }
        };
        return options;
    },
    //生成GeoJson点类型选项
    buildGeoPointOptions: function (bot, options) {
        var that = this;
        options.pointToLayer = function (feature, latlng) {
            var geoOptions = that.config.getBOTConfig(bot).geoOptions;
            var icon;
            if (geoOptions.icon) {
                var iconOptions = L.Util.extend({}, geoOptions.icon);
                iconOptions.iconUrl = geoOptions.getIconUrl(feature.properties.bo);
                icon = L.icon(iconOptions);
            }
            var markerOptions = {
                icon: icon,
                title: feature.properties.bo.bo,
                alt: feature.properties.bo.bot + ":" + feature.properties.bo.bo,
                riseOnHover: true,
            }
            return L.marker(latlng, markerOptions);
        };
    },
    //生成BO的搜索文本
    buildSearchText: function (bo) {
        var searchText = bo.bo;
        if (bo.alias && bo.alias.length > 0) {
            searchText += "[";
            searchText += bo.alias.toString();
            searchText += "]";
        }
        return searchText;
    },
    //是否支持图层过滤按钮Html
    enableFilterButton: function (bot) {
        if (!bot.properties) return false;
        if (bot.properties.length === 0) return false;
        var found = bot.properties.filter(function (property) {
            return property.scenario === "Filter" || property.scenario === "Both";
        });
        return found.length > 0;
    },
    //生成图层过滤按钮Html
    buildFilterButton: function (bot) {
        var config = this.config.controlOptions.filterButton;
        var name = bot.name;
        var botConfig = this.config.getBOTConfig(bot);
        if (botConfig.enableZoomFilter) name += ">" + (botConfig.minShowZoom - 1);
        var html = this.config.csFormat(config.template, ["gisCtrl.toogleFilter", name, config.imgUrl]);
        return html;
    },
    //生成目标点击弹出Html
    buildPopupContent: function (feature, popup) {
        var bot = feature.properties.bot;
        var bo = feature.properties.bo;
        if (!bot.metaProperties) {
            var metaProperties = bot.properties.filter(function (metaProperty) {
                return metaProperty.scenario === "Data" || metaProperty.scenario === "Both";
            });
            metaProperties = metaProperties.sort(function (x, y) {
                return x.sequence - y.sequence;
            });
            var rem = metaProperties.length % popup.cellCountPerRow;
            if (rem > 0) {
                for (var i = 0; i < rem; i++) {
                    metaProperties.push({ name: "", displayname: "" });
                }
            }
            bot.metaProperties = metaProperties;
        }
        if (bot.metaProperties.length === 0) return;
        var count = bot.metaProperties.length / popup.cellCountPerRow;
        var innerHtml = "";
        for (var i = 0; i < count; i++) {
            var rowInnerHtml = "";
            for (var j = 0; j < popup.cellCountPerRow; j++) {
                var metaProperty = bot.metaProperties[i * popup.cellCountPerRow + j];
                var displayValue = (metaProperty.name.length > 0 ? bo.properties[metaProperty.name] : "");
                var cellHtml = this.config.csFormat(popup.templateCell, [metaProperty.displayname, displayValue]);
                rowInnerHtml += cellHtml;
            }
            var rowHtml = this.config.csFormat(popup.templateRow, [rowInnerHtml]);
            innerHtml += rowHtml;
        }
        var html = this.config.csFormat(popup.templateContainer, [bo.bot, bo.bo, innerHtml]);
        return html;
    },
    //生成目标级联菜单
    buildContextMenu: function (feature, boContextMenu) {
        var bot = feature.properties.bot;
        if (!bot.contextMenuItems) {
            var contextMenuItems = [];
            contextMenuItems.push({ separator: true });
            var that = this;
            for (var i = 0; i < boContextMenu.items.length; i++) {
                var item = boContextMenu.items[i];
                contextMenuItems.push({
                    text: item.title, callback: function (e) {
                        that.onContextMenuClick(feature, item);
                    }
                });
            }
            bot.contextMenuItems = contextMenuItems;
        }
        var options = {
            contextmenu: true,
            contextmenuItems: [],
            contextmenuInheritItems: false,
        };
        var menuItems = options.contextmenuItems;
        var bo = feature.properties.bo;
        var title = this.config.csFormat(this.config.controlOptions.contextMenu.templateTitle, [bo.bot, bo.bo]);
        menuItems.push({ text: title });
        options.contextmenuItems = menuItems.concat(bot.contextMenuItems);
        return options;
    },
    //载入控制层
    loadControlLayers: function () {
        var that = this;
        var boLayers = {};
        var searchLayers = [];
        for (var i = 0; i < this.bots.length; i++) {
            var bot = this.bots[i];
            var layerName = bot.name;
            var botConfig = this.config.getBOTConfig(bot);
            if (botConfig.enableFilter && this.enableFilterButton(bot)) {
                layerName = this.buildFilterButton(bot);
                this.addSidebarFilter(bot);
            }
            if (botConfig.enableZoomFilter) {
                bot.layer.canAddLayers = function () {
                    return that.canAddLayersOnZoomEnd(this);
                };
                this.zoomFilterLayers.push(bot.layer);
            }
            boLayers[layerName] = bot.layer;
            if (botConfig.enableSearch) searchLayers.push(bot.layer);
        }
        var overlayLayers = {};
        overlayLayers[this.config.bots.controlTitle] = boLayers;
        //图层控制
        this.groupedLayers = L.control.groupedLayers(this.baseMaps, overlayLayers, this.config.controlOptions.layers);
        this.groupedLayers.addTo(this.map);
        //比例控制
        L.control.scale(this.config.controlOptions.scale)
            .addTo(this.map);
        //放大缩小控制
        L.Control.zoomHome(this.config.controlOptions.zoom)
            .addTo(this.map);
        //搜索控制
        var searchOptions = $.extend(this.config.controlOptions.search, {
            layer: L.layerGroup(searchLayers),
            propertyName: "searchText",
            moveToLocation: this.onSearchMoveToLocation,
        });
        L.control.search(searchOptions)
            .on('search:locationfound', function (e) { that.onSearch_locationfound(e, that.searchStates); })
            .on('search:collapsed', function (e) { that.onSearch_collapsed(e, that.searchStates); })
            .addTo(this.map);
        //处理地图事件
        this.map.on({
            click: function (e) {
                if (this.contextmenu) this.contextmenu.hide();
            },
            dblclick: function (e) {
                if (this.contextmenu) this.contextmenu.hide();
            },
            //处理zoom变化完成事件
            zoomend: function (e) {
                that.onZoomEnd(e);
                that.onLatlngShow(e);
            },
            //overlayadd: function (e) {
            //    console.log("overlayadd");
            //},
            //overlayremove: function (e) {
            //    console.log("overlayremove");
            //},
        });
        this.map.fire("zoomend");
    },
    //保存搜索状态
    searchStates: [],
    //搜索成功后移动到位置
    onSearchMoveToLocation: function (latlng, title, map) {
        //map.fitBounds( latlng.layer.getBounds() );
        var zoom = 16;
        if (latlng.layer.getBounds) {
            zoom = map.getBoundsZoom(latlng.layer.getBounds());
        }
        map.setView(latlng, zoom);
    },
    //搜索的发现位置事件
    onSearch_locationfound: function (e, states) {
        var found = states.find(function (state) {
            return state.layer === e.layer;
        });
        if (!found) {
            states.push({ layer: e.layer, style: { fillColor: e.layer.options.fillColor } });
        }
        e.layer.setStyle({ fillColor: this.config.controlOptions.searchFound.fillColor });
        //if (e.layer._popup) e.layer.closePopup();
    },
    //搜索的结束收缩事件
    onSearch_collapsed: function (e, states) {
        states.forEach(function (state) {
            state.layer.setStyle(state.style);
        });
        states.splice(0, states.length);
    },
    //坐标显示控制
    latlngControl: undefined,
    //加入坐标显示控制
    addLatlngShow: function () {
        var config = this.config.controlOptions.latlngShow;
        var control = L.control({ position: config.position });
        control.onAdd = function (map) {
            return L.DomUtil.create("div", config.el_class);
        };
        this.latlngControl = control;
        control.addTo(this.map);
        var that = this;
        this.map.on({
            mousemove: function (e) {
                that.onLatlngShow(e, config);
            }
        });
    },
    //处理坐标显示事件
    onLatlngShow: function (e, config) {
        if (config == undefined) config = this.config.controlOptions.latlngShow;
        var zoom = this.map.getZoom();
        var latlng = e.latlng || this.map.getCenter();
        var lat = this.latlngToDegree(latlng.lat);
        var lng = this.latlngToDegree(latlng.lng);
        var text = this.config.csFormat(config.template, [zoom, lat, lng]);
        this.latlngControl.getContainer().innerText = text;
    },
    //经纬度小数转化为度分秒
    latlngToDegree: function (a) {
        var b = Math.floor(a);
        var c = 60 * (a - b);
        a = Math.floor(c);
        c = Math.round(60 * (c - a));
        60 == c && (a++ , c = 0);
        60 == a && (b++ , a = 0);
        return "" + b + "°" + a + "'" + c + '"'
    },
    //加入sidebar筛选控制
    addSidebarFilter: function (bot) {
        var el = L.DomUtil.create("div");
        var html = this.config.csFormat(this.config.controlOptions.sidebarFilter.template, [bot.name]);
        el.innerHTML = html;
        document.body.appendChild(el);
        var sidebar = L.control.sidebar(el, this.config.controlOptions.sidebar);
        bot.sidebar = sidebar;
        sidebar.addTo(this.map);
        var that = this;
        var filterData = this.config.getBOTFilterData(bot);
        filterData.targetType = bot.name;
        filterData.filterItems = this.buildFilterList(bot);
        PKSUI.bind({
            el: el,
            data: function () {
                return filterData;
            },
            methods: {
                onQuery: function (e) {
                    that.onFilterQuery(e, bot);
                },
            },
            model: ["pks:filterlist"]
        });
    },
    //过滤按钮控制
    toogleFilter: function (botName) {
        for (var i = 0; i < this.bots.length; i++) {
            var bot = this.bots[i];
            if (!bot.sidebar) continue;
            if (!bot.layer) continue;
            if (!this.map.hasLayer(bot.layer) || bot.name !== botName) {
                bot.sidebar.hide();
            } else {
                bot.sidebar.toggle();
            }
        }
    },
    //生成过滤项集合
    buildFilterList: function (bot) {
        var properties = bot.properties.filter(function (property) {
            return property.scenario === "Filter" || property.scenario === "Both";
        });
        properties.sort(function (x, y) {
            return x.sequence - y.sequence;
        });
        var items = [];
        for (var i = 0; i < properties.length; i++) {
            var property = properties[i];
            var item = {
                catelog: property.name,
                displayName: property.displayname,
                type: "checkbox",
                list: property.options,
            };
            items.push(item);
        }
        return items;
    },
    //过滤查询
    onFilterQuery: function (e, bot) {
        var matchFn, groups;
        if (e.length === 0) {
            matchFn = function (groups, bo) {
                return true;
            }
        } else {
            groups = new Map();
            for (var i = 0; i < e.length; i++) {
                var item = e[i];
                var group = groups.get(item.catelog);
                if (!group) {
                    group = [];
                    groups.set(item.catelog, group);
                }
                group.push(item);
            }
            matchFn = this.isMatchFilter;
        }
        for (var i = 0; i < bot.bos.length; i++) {
            var bo = bot.bos[i];
            if (!bo.layer) continue;
            if (matchFn(groups, bo)) {
                if (!bot.layer.hasLayer(bo.layer)) bot.layer.addLayer(bo.layer);
            } else {
                if (bot.layer.hasLayer(bo.layer)) bot.layer.removeLayer(bo.layer);
            }
        }
    },
    //地质目标是否匹配过滤条件
    isMatchFilter: function (groups, bo) {
        groups.forEach(function (items, key) {
            var propertyValue = bo.properties[key];
            var match = items.find(function (item) {
                return propertyValue === item.value;
            });
            if (!match) return false;
        });
        return true;
    },
    //处理级联菜单点击事件
    onContextMenuClick: function (feature, item) {
        var bo = feature.properties.bo;
        var settings = this.config.es_Search;
        settings.setQuery(bo, item.query);
        var that = this;
        this.ajaxPost(settings, function (result) {
            if (result.hits) {
                var hits = result.hits.hits;
                if (hits.length > 0) {
                    var iiid = hits[0]["_source"]["iiid"];
                    window.open(that.config.boSearchRenderUrl + "?iiid=" + iiid);
                    return;
                }
            }
            alert("未发现" + bo.bo + "的" + item.title + "!");
        });
    },
    //zoom过滤后是否加入层
    canAddLayersOnZoomEnd: function (layer) {
        var bot = layer.bot;
        var botConfig = this.config.getBOTConfig(bot);
        var zoom = this.map.getZoom();
        return zoom >= botConfig.minShowZoom;
    },
    //处理zoom完成事件
    onZoomEnd: function (e) {
        var layers = this.zoomFilterLayers;
        if (layers.length === 0) return;
        var zoom = this.map.getZoom();
        for (var i = 0; i < layers.length; i++) {
            var layer = layers[i];
            var bot = layer.bot;
            var botConfig = this.config.getBOTConfig(bot);
            if (zoom >= botConfig.minShowZoom && this.groupedLayers.isLayerChecked(layer)) {
                if (!this.map.hasLayer(layer)) this.map.addLayer(layer);
            } else {
                if (this.map.hasLayer(layer)) this.map.removeLayer(layer);
            }
        }
    },
};
