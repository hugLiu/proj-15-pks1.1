//封装GIS所有配置
var gisConfig = {
    //使用元素ID
    elementId: "mapContent",
    //地图选项
    mapOptions: {
        crs: L.CRS.EPSG4326,
        //center: [51.2, 7],
        center: [20.0508, 116.2066],
        zoom: 6,
        maxBounds: [[8.4375, 91], [43.59375, 145]],
        zoomControl: false,
        //自适宜窗口大小变化
        trackResize: true,
        //缩放到双击点
        doubleClickZoom: "center",
        //Leaflet.Measure插件支持
        measureControl: true,
        //Leaflet.contextmenu插件支持
        contextmenu: true,
    },
    //Mini底图描述配置
    miniMapConfig: {
        elementId: "miniMapContent",
        timeElementId: "timeShow",
        copyright: "&copy总公司地理信息系统平台v1.0",
        template: "<div id='{0}'><span id='{1}' class='time'></span><span class='copyright'>{2}</span></div>",
    },
    //底图集合
    tileLayers: [{
        name: "影像底图",
        url: "http://10.78.165.180:8080/geoserver/gwc/service/tms/1.0.0/cite%3Achina@EPSG%3A4326@jpeg/{z}/{x}/{y}.jpg",
        options: {
            minZoom: 5,
            maxZoom: 13,
            tms: true,
            attribution: undefined,
        },
        miniMapOptions: {
            minZoom: 3,
            tms: true,
            attribution: undefined,
        },
        miniMapControl: {
            crs: L.CRS.EPSG4326,
            maxBounds: [[8.4375, 91], [43.59375, 145]],
            zoom: 6,
        }
    }, {
        name: "电子海图",
        url: "http://10.78.165.180:8081/newtile/Tiles/Z{z}/{x}/{y}/{x}_{y}.png",
        options: {
            minZoom: 7,
            maxZoom: 14,
            attribution: undefined
        },
        miniMapOptions: {
            minZoom: 3,
            attribution: undefined,
        },
        miniMapControl: {
            crs: L.CRS.EPSG3857,
            maxBounds: [[3, 106.907], [41.34805555, 133]],
            zoom: 7,
        }
    }],
    //控制选项
    controlOptions: {
        //图层控制
        layers: {
            position: 'topleft',
            collapsed: false,
        },
        //Mini底图控制
        miniMap: {
            zoomLevelFixed: 5,
            toggleDisplay: true,
            position: 'bottomright',
        },
        //搜索控制
        search: {
            position: 'topleft',
            initial: false,
            textErr: '未找到',
            textCancel: '取消',
            textPlaceholder: '搜索...',
        },
        //搜索成功
        searchFound: {
            fillColor: "#007ACC",
        },
        //比例控制
        scale: {
            position: 'bottomleft',
        },
        //放大缩小控制
        zoom: {
            position: 'topright',
        },
        //坐标显示控制
        latlngShow: {
            position: 'bottomright',
            el_class: "latlngShow",
            template: "{0}:({1},{2})",
        },
        //BO筛选控制配置
        filterButton: {
            imgUrl: "../Content/Gis/Images/funnel.png",
            template: "<span class='filter-button'><span class='title'>{1}</span><img class='control' src='{2}' onclick='{0}(\"{1}\")'/></span>",
        },
        //sidebar控制
        sidebar: {
            closeButton: true,
            autoPan: false,
            position: 'topleft'
        },
        //sidebar显示筛选
        sidebarFilter: {
            template:
            "<div class='h4 filter-sidebar-title'>{0}</div>" +
            "<pks:filterlist :showsearchbtn='showSearchBtn' :targettype='targetType' :items='filterItems' :showqueryhbtn='showQueryBtn' :querybtnclick='onQuery' :resultnum='resultNum' :shownum='showNum'></pks:filterlist>"
            ,
        },
        //点击弹出配置
        popup: {
            //每行单元数量
            cellCountPerRow: 2,
            //容器模板
            templateContainer: "<div>" +
            "<h4>{0}:{1}</h4>" +
            "<table class='table table-bordered popup-table'>{2}</table>" +
            "</div>",
            //行模板
            templateRow: "<tr>{0}</tr>",
            //单元模板
            templateCell: "<td class='popup-title'>{0}</td><td class='popup-label'>{1}</td>",
            options: {
                maxWidth: 500,
            },
        },
        //级联菜单配置
        contextMenu: {
            templateTitle: "<span class='context-menu-title'>{0}:{1}</span>",
        },
        //高亮配置
        hightLight: {
            style: {
                stroke: true,
                color: "#333333",
                weight: 2,
                dashArray: "5,5",
            }
        },
    },
    //API网站服务
    apiServiceUrl: undefined,
    //门户网站
    portalSiteUrl: undefined,
    //地质目标服务
    boServiceUrl: undefined,
    //搜索服务
    esServiceUrl: undefined,
    //语义服务
    smServiceUrl: undefined,
    //地质目标成果搜索渲染
    boSearchRenderUrl: undefined,
    //BOT配置
    bots: {
        controlTitle: "地质目标",
        "default": {
            //是否加载目标
            loaded: false,
            //加载顺序
            order: 2,
            //支持搜索
            enableSearch: false,
            //支持筛选
            enableFilter: false,
            //滤选数据
            filterData: {
                showSearchBtn: false,
                showQueryBtn: false,
                showNum: 5,
                resultNum: 0,
            },
            //是否支持Zoom过滤
            enableZoomFilter: false,
            //最小显示的Zoom
            minShowZoom: 0,
            //GeoJSON选项
            geoOptions: {
                //获得风格键
                getStyleKey: function (feature, bo) {
                    return "default";
                },
                //风格字典
                styles: {
                    default: {
                        stroke: true,
                        color: "#333333",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    },
                },
                //图元提示选项
                tooltip: {
                    direction: 'top',
                    permanent: true,
                    interactive: false,
                    sticky: false,
                },
                //是否支持左键弹出框
                enablePopup: false,
                //级联菜单选项
                contextMenu: undefined,
            },
        },
        "盆地": {
            loaded: true,
            order: 1,
            enableSearch: false,
            enableFilter: false,
            filterData: undefined,
            geoOptions: {
                getStyleKey: function (feature, bo) {
                    return "default";
                },
                styles: {
                    default: {
                        stroke: true,
                        color: "#333333",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    }
                },
                tooltip: {
                    direction: 'top',
                    permanent: true,
                    interactive: false,
                    sticky: false,
                },
                enablePopup: false,
                contextMenu: {
                    items: [
                        {
                            "title": "勘探成果图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "勘探成果图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "basin.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "勘探成果表",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "勘探成果表"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "basin.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "勘探形式图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "勘探形式图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "basin.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        }
                    ]
                },
            },
        },
        "断裂": {
            loaded: true,
            order: 2,
            enableSearch: false,
            enableFilter: false,
            filterData: undefined,
            geoOptions: {
                getStyleKey: function (feature, bo) {
                    return "default";
                },
                styles: {
                    default: {
                        stroke: true,
                        color: "#ff1400",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    },
                },
                tooltip: undefined,
                enablePopup: false,
                contextMenu: undefined,
            },
        },
        "一级构造单元": {
            loaded: true,
            order: 3,
            enableSearch: true,
            enableFilter: false,
            geoOptions: {
                getStyleKey: function (feature, bo) {
                    if (/低隆起/g.test(bo.bo))
                        return "低隆起";
                    if (/隆起/g.test(bo.bo))
                        return "隆起";
                    if (/低凸起/g.test(bo.bo))
                        return "低凸起";
                    if (/凸起/g.test(bo.bo))
                        return "凸起";
                    return "default";
                },
                styles: {
                    default: {
                        stroke: true,
                        color: "#333333",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0.5,
                    },
                    "隆起": {
                        stroke: true,
                        color: "#ffdbaa",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#ffdbaa',
                        fillOpacity: 0.5,
                    },
                    "低隆起": {
                        stroke: true,
                        color: "#ffe2bc",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#ffe2bc',
                        fillOpacity: 0.5,
                    },
                    "凸起": {
                        stroke: true,
                        color: "#ffeda0",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#ffeda0',
                        fillOpacity: 0.5,
                    },
                    "低凸起": {
                        stroke: true,
                        color: "#fff7d6",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#fff7d6',
                        fillOpacity: 0.5,
                    }
                },
                tooltip: {
                    direction: 'top',
                    permanent: true,
                    interactive: false,
                    sticky: false//随鼠标移动
                },
                enablePopup: true,
            },
        },
        "二级构造单元": {
            loaded: true,
            order: 4,
            enableSearch: true,
            enableFilter: false,
            geoOptions: {
                getStyleKey: function (feature, bo) {
                    if (/低隆起/g.test(bo.bo))
                        return "低隆起";
                    if (/隆起/g.test(bo.bo))
                        return "隆起";
                    if (/低凸起/g.test(bo.bo))
                        return "低凸起";
                    if (/凸起/g.test(bo.bo))
                        return "凸起";
                    return "default";
                },
                styles: {
                    default: {
                        stroke: true,
                        color: "#bfddff",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#bfddff',
                        fillOpacity: 0.5,
                    },
                    "隆起": {
                        stroke: true,
                        color: "#ffdbaa",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#ffdbaa',
                        fillOpacity: 0.5,
                    },
                    "低隆起": {
                        stroke: true,
                        color: "#ffe2bc",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#ffe2bc',
                        fillOpacity: 0.5,
                    },
                    "凸起": {
                        stroke: true,
                        color: "#ffeda0",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#ffeda0',
                        fillOpacity: 0.5,
                    },
                    "低凸起": {
                        stroke: true,
                        color: "#fff7d6",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#fff7d6',
                        fillOpacity: 0.5,
                    }
                },
                tooltip: {
                    direction: 'top',
                    permanent: true,
                    interactive: false,
                    sticky: false,
                },
                enablePopup: true,
                contextMenu: {
                    items: [
                        {
                            "title": "勘探成果图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "勘探成果图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "secondlevel.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "勘探成果表",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "勘探成果表"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "secondlevel.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "已入库圈闭汇总表",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "已入库圈闭汇总表"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "secondlevel.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        }
                    ]
                },
            },
        },
        "地震工区": {
            loaded: true,
            order: 5,
            enableSearch: true,
            enableFilter: true,
            geoOptions: {
                getStyleKey: function (feature, bo) {
                    var value = bo.properties["作业类型"];
                    if (value === "三维") {
                        return "三维";
                    }
                    if (value === "二维") {
                        return "二维";
                    }
                    return "default";
                },
                styles: {
                    default: {
                        stroke: true,
                        color: "#333333",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    },
                    "三维": {
                        stroke: true,
                        color: "#0000ff",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    },
                    "二维": {
                        stroke: true,
                        color: "#009300",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    },
                },
                tooltip: {
                    direction: 'top',
                    permanent: true,
                    interactive: false,
                    sticky: false,
                },
                enablePopup: true,
                contextMenu: {
                    items: [
                        {
                            "title": "工区位置图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "工区位置图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "swa.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "地震工区认识",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pc.keyword": "地震工区百科"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "swa.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        }
                    ]
                },
            },
        },
        "圈闭": {
            loaded: true,
            order: 6,
            enableSearch: true,
            enableFilter: true,
            geoOptions: {
                getStyleKey: function (feature, bo) {
                    var featureType = feature.geometry.type;
                    if (featureType == "LineString" || featureType == "MultiLineString") {
                        return "Line";
                    }
                    return "default";
                },
                styles: {
                    default: {
                        stroke: true,
                        color: "#fefe00",
                        opacity: 0.5,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#fefe00',
                        fillOpacity: 0.5,
                    },
                    "Line": {
                        stroke: true,
                        color: "#ff1400",
                        opacity: 1,
                        weight: 1,
                        dashArray: null,
                        fillColor: '#dddddd',
                        fillOpacity: 0,
                    },
                },
                tooltip: {
                    direction: 'top',
                    interactive: false,
                    permanent: false,
                    sticky: false,
                },
                enablePopup: true,
                contextMenu: {
                    items: [
                        {
                            "title": "圈闭要素表",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "圈闭要素表"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "trap.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "十字剖面图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "十字剖面图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "trap.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "蒙太奇图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "蒙太奇图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "trap.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "油藏剖面图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "油藏剖面图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "trap.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "圈闭认识",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pc.keyword": "圈闭百科"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "trap.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        }
                    ]
                },
            },
        },
        "井": {
            loaded: true,
            order: 7,
            enableFilter: true,
            enableSearch: true,
            //是否支持Zoom过滤
            enableZoomFilter: true,
            //最小显示的Zoom
            minShowZoom: 8,
            geoOptions: {
                getStyleKey: undefined,
                styles: undefined,
                tooltip: {
                    direction: "auto",
                    interactive: false,
                    permanent: false,
                    sticky: false,
                },
                enablePopup: true,
                contextMenu: {
                    items: [
                        {
                            "title": "蒙太奇图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "蒙太奇图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "well.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "单井综合柱状图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "单井综合柱状图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "well.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "地层分层数据表",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "地层分层数据表"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "well.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "井身结构图",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pt.keyword": "井身结构图"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "well.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        },
                        {
                            "title": "井认识",
                            "query": {
                                "_source": [
                                    "iiid",
                                ],
                                "query": {
                                    "bool": {
                                        "must": [
                                            {
                                                "term": {
                                                    "pc.keyword": "井百科"
                                                }
                                            },
                                            {
                                                "term": {
                                                    "well.keyword": "{boName}"
                                                }
                                            }
                                        ]
                                    }
                                },
                                "size": 1
                            }
                        }
                    ]
                },
                //图标选项
                icon: {
                    iconSize: [16, 16],
                    //iconAnchor: [22, 94],
                    //popupAnchor: [-3, -76],
                },
                //获得图标url
                getIconUrl: function (bo) {
                    var path = "../Content/Gis/Icons/井/";
                    var name = this.getIconName(bo);
                    if (name.length === 0) name = "探井.png";
                    return path + name;
                },
                //获得图标名称
                getIconName: function (bo) {
                    if (this.isZZWell(bo)) return "正钻井.png";
                    var type = bo.properties["井况1"];
                    if (type) {
                        switch (type) {
                            case "气层井": return "气层井.png";
                            case "气流井": return "气流井.png";
                            case "无显示井": return "无显示井.png";
                            case "油层井": return "油层井.png";
                            case "油流井": return "油流井.png";
                            case "油气层井": return "油气层井.png";
                            case "油气显示井": return "油气显示井.png";
                        }
                    }
                    var cat = bo.properties["井别"];
                    if (cat) {
                        switch (cat) {
                            case "预探井": return "预探井.png";
                            case "评价井": return "评价井.png";
                        }
                    }
                    return "";
                },
                //是否正钻井
                isZZWell: function (bo) {
                    var kzDate = bo.properties["开钻日期1"];
                    var wjDate = bo.properties["完井日期"];
                    kzDate = pksGlobalStore.toMomentDate(kzDate);
                    if (kzDate == undefined) return false;
                    wjDate = pksGlobalStore.toMomentDate(wjDate);
                    if (wjDate == undefined) return false;
                    var now = moment();
                    return now >= wjDate && now < wjDate;
                },
            },
        },
    },
    //获得BOT配置
    getBOTConfig: function (bot) {
        return this.bots[bot.name] || this.bots.default;
    },
    //获得BOT默认配置
    getBOTDefaultConfig: function (bot) {
        return this.bots.default;
    },
    //获得BOT滤选数据
    getBOTFilterData: function (bot) {
        var config = this.getBOTConfig(bot);
        var result = this.bots.default.filterData;
        if (config.filterData) {
            return L.Util.extend({}, result, config.filterData);
        }
        return L.Util.extend({}, result);
    },
    //获得BOT配置
    bo_FilterBOTs: {
        url: undefined,
        data: {
            query: {},
        }
    },
    //获得BO配置
    bo_FilterBOs: {
        url: undefined,
        data: {
            query: {
                bot: undefined
            },
        },
        setQuery: function (bot) {
            this.data.query.bot = bot;
        }
    },
    //ES搜索
    es_Search: {
        url: undefined,
        data: undefined,
        setQuery: function (bo, query) {
            var data = JSON.stringify(query);
            this.data = data.replace(/\{boName\}/g, bo.bo);
        }
    },
    //初始化
    init: function (apiServiceUrl, portalSiteUrl) {
        var attribution = this.miniMapConfig.template;
        attribution = this.csFormat(attribution, [this.miniMapConfig.elementId, this.miniMapConfig.timeElementId, this.miniMapConfig.copyright]);
        for (var i = 0; i < this.tileLayers.length; i++) {
            var tileConfig = this.tileLayers[i];
            tileConfig.options.attribution = attribution;
            //tileConfig.miniMapOptions.attribution = attribution;
        }
        this.apiServiceUrl = apiServiceUrl;
        this.portalSiteUrl = portalSiteUrl;
        this.boServiceUrl = apiServiceUrl + "BO2Service";
        this.esServiceUrl = apiServiceUrl + "SearchService";
        this.smServiceUrl = apiServiceUrl + "SemanticService/Segment";
        this.boSearchRenderUrl = portalSiteUrl + "/Render/Content";
        this.bo_FilterBOTs.url = this.boServiceUrl + "/FilterBOTs";
        this.bo_FilterBOs.url = this.boServiceUrl + "/FilterBOs";
        this.es_Search.url = this.esServiceUrl + "/ESSearch";
    },
    //CSharp版格式化
    csFormat: function (value, args) {
        return value.replace(/\{(\d+)\}/g, function (match, index, offset, string) {
            return args[parseInt(index)];
        });
    },
}