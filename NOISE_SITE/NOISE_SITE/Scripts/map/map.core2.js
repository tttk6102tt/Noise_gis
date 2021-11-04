(function ($, vgsMap) {

    'use strict';

    vgsMap = vgsMap || {};

    var instance;
    var m_TemplateEngine = new TemplateEngine({
        templateDir: '/templates/',
        templateExt: '.tpl'
    });
    //var urlService = "http://222.252.17.86:6080/arcgis/rest/services/VIGAC_BanDo/";
    //var baseMapUri = urlService + "/Base_QuangNinh/MapServer";
    var baseMapUriTramDo = "http://tiengontructuyen.vn/arcgis/rest/services/DuLieuChuyenDe/TramQuanTracCoDinh/MapServer"
    var m_BoundaryLayer;
    var m_TempLayer;
    var m_RoundLayer;
    var m_BaseLayerGroup;
    var m_WMS = [];
    var m_WFS = [];
    var m_DefaultService;
    var m_FeatureLayerGroups;
    var m_TempInteraction = [];
    var m_History = {
        data: [],
        cIdx: -1
    };
    var m_Bookmarker;
    var m_Editor;
    var m_Toolbar;
    var m_Export;
    var m_Print;
    var m_Popup;
    var m_Measurement;
    var m_PointerStyle = 'default';
    var regionGraphic, regionGL, regionSymbol;
    var tramDoGraphic, tramDoGL, tramDoSymbol;
    var mMapLoadCb = [];
    var mEditGraphic = [];
    var mDrawer;
    var editToolbar;// = new Edit(mMap);
    var tool = 0;
    var mLineSymbol, mMarkerSymbol, mFillSymbol, mSnapSymbol, mHighLightSymbol;
    var mIdentifyTasks = {};
    var that;
    var urlRes = 'http://tiengontructuyen.vn/arcgis/rest/services/PhanHoi/NguoiDung/FeatureServer'
    function Core(extendOptions, $deferred) {
        that = this;

        if (!extendOptions.map || !extendOptions.map.container) {
            throw 'Map div not found!';
        } else {
            $deferred = $deferred || $.Deferred();
            var options = $.extend(true, {}, defaultOptions, extendOptions);
            this._geometryService = null;
            mMap = null;
            this._overViewMap = null;
            this._overViewLayer = null;
            this._popup = null;
            this._markerSymbol = this._highLightSymbol = this._lineSymbol = this._fillSymbol = this._startRouteSymbol = this._endRouteSymbol = null;
            this._tileLayer = null;
            this._tileSatLayer = null;
            this._routeDirectionLayer = null;
            this._identifyTask = null;
            this._identifyParams = null;
            mDrawer = null;
            this._editor = null;
            this._navigator = null;
            this._isDrawing = false;
            mIdentifyTasks = {};
            this._moduleInfos = [];
            this._activeLayer = null;
            this._areaSelectWindowClass = 'agJsMap_AreaSelectWindow';
            this._areaSelectGridClass = 'agJsMap_AreaSelectGrid';
            this._damagedAssetResultWindowClass = 'agJsMap_DamagedAssetResulttWindow';
            this._damagedAssetResultGridClass = 'agJsMap_DamagedAssetResultGrid';
            this._attributesTable = null;
            this._labelConfigRedact = null;
            this._opacityConfigRedact = null;
            this._bufferFeatureRedact = null;
            this._areaSelectGrid = null;
            this._damagedAssetResultWin = null;
            this._damagedAssetResultGrid = null;
            this.bookmarkRedact = null;
            this.measurementRedact = null;
            this.options = options;
            return this;
        }
    }



    var mMap, mNavigation;
    var m_overViewMap;

    var defaultOptions = {
        mapView: {
            category: [],
            queryWithRole: '',
            role: false,
            minZoom: 11,
            maxZoom: 19,
            featureZoom: 16,
            defaultZoom: 8,
            center: [21.1718046, 107.2012742],
            saveLastExtent: false,
            overViewMap: false,
            webServiceType: 'wms',
            defaultWebService: 'wms',
            mapName: '',
            mapBase: []
        }
    };

    var useLocalStorage = 'localStorage' in window && window['localStorage'] !== null;
    var lastExtentStorage = 'ol_last_extent';

    function init(element, extendOptions) {
        instance = this;
        window.cat = instance;
        if (!element)
            throw 'Map container not found!';
        this.element = element;
        this.options = $.extend(true, {}, defaultOptions, extendOptions);

        return this._begin();
    }

    vgsMap.MapView = init;

    init.prototype = {
        _begin: function () {
            var me = this;
            //

            require(["dojo/_base/array",
                'esri/InfoTemplate',
                "esri/dijit/Legend",
                "esri/geometry/Extent",
                "esri/config",
                "esri/map",
                "esri/dijit/BasemapToggle",
                "esri/toolbars/navigation",
                "esri/layers/WMSLayer",
                "esri/layers/FeatureLayer",
                "esri/layers/ArcGISTiledMapServiceLayer",
                "esri/layers/ArcGISDynamicMapServiceLayer",
                "esri/layers/GraphicsLayer",
                "esri/symbols/SimpleLineSymbol",
                "esri/symbols/SimpleFillSymbol",
                "esri/symbols/SimpleMarkerSymbol",
                "esri/symbols/PictureMarkerSymbol",
                "esri/Color", "esri/tasks/geometry",
                "esri/tasks/query",
                "esri/geometry/Point",
                "esri/dijit/InfoWindow",
                'esri/geometry/screenUtils',
                "esri/dijit/Popup",
                "esri/tasks/IdentifyTask", "esri/tasks/IdentifyParameters",
                "dojo/domReady!"
            ],
                function (arrayUtils, InfoTemplate, Legend, Extent, esriConfig, Map, BasemapToggle, Navigation, WMSLayer, FeatureLayer, ArcGISTiledMapServiceLayer,
                    ArcGISDynamicMapServiceLayer, GraphicsLayer, SimpleLineSymbol, SimpleFillSymbol, SimpleMarkerSymbol, PictureMarkerSymbol, Color, Geometry, Query, Point,
                    InfoWindow,
                    screenUtils, Popup, IdentifyTask, IdentifyParameters,
                    domConstruct
                ) {

                    //esriConfig.defaults.geometryService = new esri.tasks.GeometryService("http://222.252.17.86:6080/arcgis/rest/services/Utilities/Geometry/GeometryServer");
                    esriConfig.defaults.geometryService = new esri.tasks.GeometryService("http://tiengontructuyen.vn/arcgis/rest/services/Utilities/Geometry/GeometryServer");


                    mLineSymbol = new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                        new Color([255, 0, 0]), 3);
                    mFillSymbol = new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
                        new SimpleLineSymbol(SimpleLineSymbol.STYLE_DASHDOT,
                            new Color([255, 0, 0]), 3), new Color([255, 255, 0, 0.25])
                    );
                    mSnapSymbol = new SimpleMarkerSymbol(SimpleMarkerSymbol.STYLE_SQUARE, 15,
                        new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                            new Color([0, 0, 0]), 1),
                        new Color([0, 0, 0, 0])
                    );
                    mMarkerSymbol = new SimpleMarkerSymbol(
                        SimpleMarkerSymbol.STYLE_CIRCLE,
                        16,
                        new SimpleLineSymbol(
                            SimpleLineSymbol.STYLE_SOLID,
                            new Color([0, 0, 0, 1]),
                            2
                        ),
                        new Color([0, 0, 0, 0])
                    );
                    mHighLightSymbol = new SimpleMarkerSymbol(
                        SimpleMarkerSymbol.STYLE_CIRCLE,
                        18,
                        new SimpleLineSymbol(
                            SimpleLineSymbol.STYLE_SOLID,
                            new Color('red'),
                            2
                        ),
                        new Color('red')
                    );

                    mHighLightSymbol.setPath(`M58 164 c-51 -27 -50 -102 2 -129 86 -45 175 17 144 100 -16 40 -96
56 -146 29z m116 -16 c20 -17 24 -27 16 -38 -9 -13 -11 -13 -20 0 -8 13 -11
12 -23 -4 -13 -18 -14 -18 -21 5 l-7 24 -11 -23 -12 -23 -13 23 -13 23 -6 -22
c-3 -13 -10 -20 -15 -17 -5 3 -9 1 -9 -5 0 -21 19 -20 26 1 l7 23 8 -23 c10
-25 29 -21 29 6 1 12 5 10 15 -8 13 -23 14 -24 21 -6 7 18 7 18 21 0 12 -16
15 -17 23 -4 6 9 10 10 10 3 0 -6 -12 -21 -26 -32 -56 -44 -147 -11 -140 51 7
64 89 90 140 46z`);
                    //xmax: 
                    //xmin: 
                    //ymax: 
                    //ymin:
                    var customExtentAndSR = new esri.geometry.Extent(5120900, -9998100, -583280.9042999996, 2328200, new esri.SpatialReference({ "wkid": 3405 }));

                    var popup = new Popup({
                        fillSymbol: new SimpleFillSymbol(SimpleFillSymbol.STYLE_SOLID,
                            new SimpleLineSymbol(SimpleLineSymbol.STYLE_SOLID,
                                new Color([255, 0, 0]), 2), new Color([255, 255, 0, 0.25]))
                    }, domConstruct.createElement("div"));

                    mMap = new Map(me.element.attr('id'), {
                        autoResize: true,
                        slider: false,
                        showAttribution: false,
                        logo: false,
                        minScale: 600000,
                        center: [105.790473, 21.046137],
                        //extent: customExtentAndSR,

                        //center: [105, 21],
                        zoom: 16,
                        //basemap: "topo",
                        infoWindow: popup
                    });

                    window.map2 = mMap;

                    //var toggle = new BasemapToggle({
                    //    map: mMap,
                    //    basemap: "satellite"
                    //}, "BasemapToggle").startup();


                    var layerInfo = [];

                    var url_map_nen = "http://tiengontructuyen.vn/arcgis/rest/services/BanDoNen/RanhGioiQuanTrac/MapServer";
                    //var layerInfo = [];

                    var layer_nen = new ArcGISDynamicMapServiceLayer(url_map_nen, {
                        id: "BanDoNen",
                        //opacity: $("#checkRas").is(":checked") ? 1 : 0,
                    });

                    // 
                    mMap.addLayer(layer_nen);

                    layerInfo.push({
                        layer: layer_nen,
                        title: "Bản đồ nền"
                    });

                    me.initMeasurement(mMap);


                    //me._beginIdentify(mMap);
                    me._beginDrawer(mMap);

                    regionSymbol = new SimpleFillSymbol().setColor(null).outline.setColor("blue");

                    //tramDoSymbol = new SimpleMarkerSymbol();

                    tramDoSymbol = new SimpleMarkerSymbol({
                        "color": [255, 255, 255, 64],
                        "size": 12,
                        "angle": -30,
                        "xoffset": 0,
                        "yoffset": 0,
                        "type": "esriSMS",
                        "style": "esriSMSCircle",
                        "outline": {
                            "color": [0, 0, 0, 255],
                            "width": 1,
                            "type": "esriSLS",
                            "style": "esriSLSSolid"
                        }
                    });

                    mNavigation = new Navigation(mMap);


                    //tramDoGL = new GraphicsLayer({
                    //    id: "regions"
                    //});
                    //mMap.addLayer(tramDoGL);

                    // fit boundary
                    /*
                    $.get("/Home/GetDataLastMap", (xhr) => {

                        $("#legend").empty();

                        var tenbando = "bando_" + xhr.data.Ngay + "_" + (xhr.data.Gio < 10 ? "0" + xhr.data.Gio : xhr.data.Gio);

                        //  tenbando = "USA";
                        var url_map = "http://tiengontructuyen.vn/arcgis/rest/services/RasterNoise/" + tenbando + "/MapServer";

                        //layerInfo = layerInfo.filter(s => s.ID != "BanDoNoiSuy");
                        var layer = new ArcGISDynamicMapServiceLayer(url_map, {
                            id: "LastBanDoNoiSuy",
                            opacity: 0.6,
                        });
                        var layerInfo = [];

                        mMap.addLayer(layer);
                        layerInfo.push({
                            layer: layer,
                            title: "Bản đồ nội suy",
                            ID: "LastBanDoNoiSuy"
                        });

                        //me._beginIdentify(mMap);


                        //var ngaydoMMddYYYY = xhr.data.Ngay.substring(4, 6) + "/" + xhr.data.Ngay.substring(6, 8) + "/" + xhr.data.Ngay.substring(0, 4);

                        //let time_filter = `ngayQuanTrac = '${ngaydoMMddYYYY}' AND thoiGianQuanTrac = ${xhr.data.Gio}`;

                        //me.loadTramDo(time_filter);


                        //var id = generateUUID();

                        //$('<div class="legends-container" style="padding:10px;" id="' + id + '" />').appendTo($("#legend"));
                        //new Legend({
                        //    map: me.getAgsMap(),
                        //    layerInfos: layerInfo
                        //}, id).startup();

                        //me._beginIdentifyRaster(mMap, url_map);

                    });
                    */
                    function generateUUID() { // Public Domain/MIT
                        var d = new Date().getTime();//Timestamp
                        var d2 = (performance && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
                        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                            var r = Math.random() * 16;//random number between 0 and 16
                            if (d > 0) {//Use timestamp until depleted
                                r = (d + r) % 16 | 0;
                                d = Math.floor(d / 16);
                            } else {//Use microseconds since page-load if supported
                                r = (d2 + r) % 16 | 0;
                                d2 = Math.floor(d2 / 16);
                            }
                            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
                        });
                    }
                    $("#noisuy").on('click', () => {
                        if (mMap.getLayer('LastBanDoNoiSuy')) {
                            mMap.removeLayer(mMap.getLayer('LastBanDoNoiSuy'));
                        }

                        $("#legend").empty();
                        // Add raster
                        var tansuatlaymau = document.getElementById("tan_suat_lay_mau").value;
                        var time_filter = "1=1";

                        var now,
                            thoigian_from,
                            thoigian_to,
                            d,
                            m,
                            day,
                            year,
                            ngaydo,
                            khunggio,
                            dd,
                            mm,
                            yyyy,
                            h,
                            time_to;

                        var ngaydoStr = $("#ngaydo").val();

                        var ngaydoSplit = ngaydoStr.split("/");


                        var ngaydoMMddYYYY = ngaydoSplit[1] + "/" + ngaydoSplit[0] + "/" + ngaydoSplit[2];


                        d = new Date(ngaydoSplit[2], parseInt(ngaydoSplit[1]) - 1, ngaydoSplit[0]);

                        khunggio = document.getElementById('khunggio').value;

                        if (khunggio != 23) {
                            if (khunggio < 10) {
                                if (khunggio == 9) {
                                    thoigian_from = ngaydoMMddYYYY + ' 00:00:00';
                                    thoigian_to = ngaydoMMddYYYY + ' 01:00:00';
                                } else {
                                    thoigian_to = ngaydoMMddYYYY + ' 0' + (parseInt(khunggio) + 1) + ':00:00';
                                    thoigian_from = ngaydoMMddYYYY + ' 0' + khunggio + ':00:00';
                                }
                            } else {
                                if (khunggio == 9) {
                                    thoigian_from = ngaydoMMddYYYY + ' 09:00:00';
                                    thoigian_to = ngaydoMMddYYYY + ' 10:00:00';
                                } else {
                                    thoigian_to = ngaydoMMddYYYY + ' ' + (parseInt(khunggio) + 1) + ':00:00';
                                    thoigian_from = ngaydoMMddYYYY + ' ' + khunggio + ':00:00';
                                }
                            }
                        } else {
                            thoigian_from = ngaydoMMddYYYY + ' 23:00:00';
                            thoigian_to = ngaydoMMddYYYY + ' 23:59:59';
                        }

                        time_filter = `ngayQuanTrac = '${ngaydoMMddYYYY}' AND thoiGianQuanTrac = ${document.getElementById('khunggio').value}`;

                        //var bando_ngay = m.toString() + day.toString() + year.toString();
                        //var tenbando = "bando_" + bando_ngay + "_" + khunggio.replace("-", "_");

                        if (khunggio < 10) {
                            khunggio = "0" + khunggio
                        }
                        var month = ngaydoSplit[1];
                        //if (month < 10) {
                        //    month = "0" + month;
                        //}
                        var day = ngaydoSplit[0];
                        //if (day < 10) {
                        //    day = "0" + day;
                        //}
                        var tenbando = "bando_" + ngaydoSplit[2] + month + day + "_" + khunggio;

                        //  tenbando = "USA";
                        var url_map = "http://tiengontructuyen.vn/arcgis/rest/services/RasterNoise/" + tenbando + "/MapServer";



                        // 
                        // add Feature


                        var template = new InfoTemplate();
                        template.setContent(getTextContent);
                        template.setTitle("Trạm quan trắc");

                        function pointToExtent(map, point, toleranceInPixel) {
                            var pixelWidth = map.extent.getWidth() / map.width;
                            var toleranceInMapCoords = toleranceInPixel * pixelWidth;
                            return new Extent(point.x - toleranceInMapCoords,
                                point.y - toleranceInMapCoords,
                                point.x + toleranceInMapCoords,
                                point.y + toleranceInMapCoords,
                                map.spatialReference);
                        }

                        function getTextContent(graphic) {
                            var pattern = /(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})/;
                            var attributes = graphic.attributes;

                            var date = new Date(attributes.TIME);
                            var hour = date.getHours() > 9 ? date.getHours() : "0" + date.getHours();
                            var minute = date.getMinutes() > 9 ? date.getMinutes() : "0" + date.getMinutes();
                            var second = date.getSeconds() > 9 ? date.getSeconds() : "0" + date.getSeconds();
                            var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
                            var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0" + (date.getMonth() + 1);
                            var strDate = `${day}/${month}/${date.getFullYear()} ${hour}:${minute}:${second} `;

                            return `<b>Tên trạm</b> : ${attributes.maTram} <br />
                                    <b>Thời gian thu thập: ${strDate} <br />
                                    <b>Giá trị độ ồn</b> : ${attributes.dB}`;
                        }

                        mMap.on("click", function (event) {
                            //var query = new Query();
                            //query.geometry = pointToExtent(mMap, event.mapPoint, 10);
                            //var deferred = featureLayer.selectFeatures(query,
                            //    FeatureLayer.SELECTION_NEW);
                            //mMap.infoWindow.setFeatures([deferred]);
                            //mMap.infoWindow.show(event.mapPoint);
                        });

                        if (mMap.getLayer('TramDo')) {
                            mMap.removeLayer(mMap.getLayer('TramDo'));

                        }
                        var layerInfo = [];

                        //    if (mMap.getLayer('BanDoNen')) {
                        //        mMap.removeLayer(mMap.getLayer('BanDoNen'));
                        //        }
                        //var url_map_nen = "http://tiengontructuyen.vn/arcgis/rest/services/BanDoNen/VN2000_Nen/MapServer";
                        //    //var layerInfo = [];

                        //var layer_nen = new ArcGISDynamicMapServiceLayer(url_map_nen, {
                        //    id: "BanDoNen",
                        //    //opacity: $("#checkRas").is(":checked") ? 1 : 0,
                        //});
                        //mMap.addLayer(layer_nen);
                        //layerInfo.push({
                        //    layer: layer_nen,
                        //    title: "Bản đồ nền"
                        //});

                        if ($("#checkTram").is(":checked")) {
                            //layerInfo = layerInfo.filter(s => s.ID != "TramDo");
                            //mMap.addLayer(featureLayer);
                            //layerInfo.push({
                            //    layer: featureLayer,
                            //    title: "Trạm đo",
                            //    ID:"TramDo"
                            //});

                            me.loadTramDo(time_filter);
                        }
                        if (mMap.getLayer('BanDoNoiSuy')) {
                            mMap.removeLayer(mMap.getLayer('BanDoNoiSuy'));

                        }

                        if ($("#checkRas").is(":checked")) {
                            layerInfo = layerInfo.filter(s => s.ID != "BanDoNoiSuy");
                            var layer = new ArcGISDynamicMapServiceLayer(url_map, {
                                id: "BanDoNoiSuy",
                                opacity: 0.6,
                            });

                            mMap.addLayer(layer);
                            layerInfo.push({
                                layer: layer,
                                title: "Bản đồ nội suy",
                                ID: "BanDoNoiSuy"
                            });
                            me._beginIdentifyRaster(mMap, url_map);
                        }
                        var id = generateUUID();
                        $('<div class="legends-container" style="padding:10px;" id="' + id + '" />').appendTo($("#legend"));
                        if (layerInfo.length > 0) {
                            new Legend({
                                map: me.getAgsMap(),
                                layerInfos: layerInfo
                            }, id).startup();
                        }

                    });
                });
            $(this.element).data('vgsMapView', this);
            //
            return $(this.element);
        },



        _beginIdentify: function (map) {
            var that = this;

            require([
                "dojo/_base/array",
                "esri/tasks/IdentifyTask",
                "esri/tasks/IdentifyParameters",
                "esri/dijit/InfoWindow",
                "dojo/on"
            ], function (
                arrayUtils,
                IdentifyTask, IdentifyParameters, InfoWindow, on) {
                var infoWindow = new InfoWindow();
                var params = new IdentifyParameters();
                //params.tolerance = 3;
                //params.returnGeometry = true;
                //params.returnFieldName = true;
                //params.returnUnformattedValues = true;
                //params.layerOption = IdentifyParameters.LAYER_OPTION_VISIBLE;
                //params.width = map.width;
                //params.height = map.height;

                var identifyParams = new IdentifyParameters();
                identifyParams.tolerance = 1;
                identifyParams.returnGeometry = true;
                identifyParams.layerIds = [0];
                identifyParams.layerOption = IdentifyParameters.LAYER_OPTION_ALL;
                identifyParams.width = map.width;
                identifyParams.height = map.height;

                //mIdentifyTasks["identifyParams"] = params;
                mIdentifyTasks["identifyParams"] = identifyParams;
                //
                //mIdentifyTasks[that.options.mapView.mapName] = new IdentifyTask(urlService + that.options.mapView.mapName + "/MapServer");

                //mIdentifyTasks[that.options.mapView.mapName] = new IdentifyTask("http://tiengontructuyen.vn/arcgis/rest/services/bando/tramdo/FeatureServer/0");
                mIdentifyTasks[that.options.mapView.mapName] = new IdentifyTask("http://tiengontructuyen.vn/arcgis/rest/services/RasterNoise/bando_20211028_20/MapServer");
                //
                mIdentifyTasks["identifyClickHandle"] = on.pausable(map, 'click', that._onIdentifyClick.bind(that));
                mIdentifyTasks["identifyClickHandle"].pause();
                mIdentifyTasks["identifyExecuteHandle"] = function (geometry, $deferred) {
                    //
                    $deferred = $deferred || $.Deferred();

                    var that = this;

                    mMap.infoWindow.clearFeatures();
                    mIdentifyTasks.features = null;

                    require(['esri/InfoTemplate', "dojo/Deferred", "dojo/promise/all"], function (InfoTemplate, Deferred, all) {
                        mIdentifyTasks["identifyParams"].geometry = geometry;
                        mIdentifyTasks["identifyParams"].mapExtent = mMap.extent;
                        mIdentifyTasks["identifyParams"].layerOption = esri.tasks.IdentifyParameters.LAYER_OPTION_ALL;
                        var defArray = [];
                        defArray.push(mIdentifyTasks[that.options.mapView.mapName].execute(mIdentifyTasks["identifyParams"]));
                        all(defArray).then(function (results) {
                            console.log(results)
                            $deferred.resolve([].concat.apply([], results));
                        });
                    });

                    return $deferred.promise();

                }.bind(that);

                mIdentifyTasks["parseResults"] = function (results) {
                    console.log(results)
                    var globalIds = [];
                    var features = [];
                    //
                    require(['esri/InfoTemplate'], function (InfoTemplate) {
                        results.map(function (result) {
                            var feature = result.feature;

                            //    feature.setInfoTemplate(new InfoTemplate(
                            //        "THÔNG TIN " + title,
                            //        "<b>Loại tài sản gắn liền với đất: </b>" + TAISAN[loaiTaiSanGanLienVoiDat] + "<br />" +
                            //        "<b>Tên tài sản: </b>" + tenTaiSan + "<br />" +
                            //        "<b>Địa điểm biến động: </b>" + diaDiemBienDong + "<br />" +
                            //        "<b>Đối tượng phát hiện: </b>" + doiTuongPhatHien + "<br />" +
                            //        "<b>Dữ liệu đính kèm: </b><a download href='/upload/" + duLieuDinhKem + "'>" + duLieuDinhKem + "</a><br />" +
                            //        "<b>Thời điểm biến động: </b>" + new Date(feature.attributes["thoiDiemBienDong"]).toLocaleDateString("fr", { year: "numeric", day: "2-digit", month: "2-digit" }) + "<br />" +
                            //        "<b>Tọa độ: </b> X = " + feature.attributes["toaDoX"] + " - Y = " + feature.attributes["toaDoY"] + "<br />" +
                            //        "<b>Thông tin khác: </b>" + thongTinKhac + "<br />"));
                        });

                        mMap.infoWindow.setFeatures(features);
                        mMap.infoWindow.show();
                    });
                };

                //
                console.log('finished initial identify.');
            });
        },

        _beginDrawer: function (map) {
            var that = this;
            //
            require(['esri/toolbars/draw', 'dojo/on'], function (Draw, on) {
                var drawer = new Draw(map, {
                    tooltipOffset: 20,
                    drawTime: 90
                });
                drawer.on('activate', function (e) {
                    this.isDrawing = true;
                    this.map.disablePan();
                    this.map.setMapCursor('crosshair');
                    //
                    if (this.drawHandle)
                        this.drawHandle.remove();
                });
                drawer.on('deactivate', function (e) {
                    this.isDrawing = false;
                    this.map.enablePan();
                    this.map.setMapCursor('default');
                    //
                    if (this.drawHandle)
                        this.drawHandle.remove();
                });
                drawer.isDrawing = false;
                drawer.drawHandle = null;
                drawer.draw = function (tool, onMemory, drawCall) {
                    var that = this;

                    if (tool === esri.toolbars.Draw.POLYGON)
                        mDrawer.activate(esri.toolbars.Draw.POLYLINE);
                    else
                        mDrawer.activate(tool);

                    require(["dojo/on"], function (on) {
                        drawer.drawHandle = on(mDrawer, 'draw-complete', function (evt) {
                            mDrawer.getDrawGeometryHandle.call(undefined, {
                                tool: tool,
                                drawEvt: evt
                            }).done(function (e) {
                                drawCall.call(this, e.graphic);
                                if (!onMemory)
                                    e.target.map.graphics.add(e.graphic);
                            });
                        });
                    });
                }.bind(that);
                drawer.drawOnce = function (tool) {
                    var that = this;
                    var $deferred = $.Deferred();

                    if (tool === esri.toolbars.Draw.POLYGON)
                        mDrawer.activate(esri.toolbars.Draw.POLYLINE);
                    else
                        mDrawer.activate(tool);

                    require(["dojo/on"], function (on) {
                        drawer.drawHandle = on(mDrawer, 'draw-complete', function (evt) {
                            mDrawer.getDrawGeometryHandle.call(undefined, {
                                tool: tool,
                                drawEvt: evt
                            }).done(function (e) {
                                if (e.target.drawHandle)
                                    e.target.drawHandle.remove();
                                e.target.deactivate();
                                $deferred.resolve(e.graphic);
                                //
                                mMap.graphics.add(e.graphic);
                            });
                        });
                    });

                    return $deferred.promise();
                }.bind(that);
                drawer.getDrawGeometryHandle = function (e) {
                    var that = this;
                    this.clear();
                    var $deferred = $.Deferred();
                    var drawEvt = e.drawEvt,
                        tool = e.tool;
                    var graphic;
                    require([
                        "esri/graphic",
                        "esri/geometry/Polygon",
                        "esri/tasks/query"
                    ], function (Graphic, Polygon, Query) {
                        if (tool === esri.toolbars.Draw.POLYGON) {
                            var paths = drawEvt.geometry.paths[0];
                            if (paths.length < 3) {
                                return;
                            }
                            var rings = $.merge([paths[0]], paths);
                            var polygon = new esri.geometry.Polygon(mMap.spatialReference);
                            polygon.addRing(rings);
                            graphic = new Graphic(polygon, mFillSymbol);
                        } else {
                            if (drawEvt.geometry.type === 'polyline' || drawEvt.geometry.type === 'line') {
                                graphic = new Graphic(drawEvt.geometry, mLineSymbol);
                            } else if (drawEvt.geometry.type === 'point') {
                                graphic = new Graphic(drawEvt.geometry, mMarkerSymbol);
                            } else {
                                graphic = new Graphic(drawEvt.geometry, mFillSymbol);
                            }
                        }
                        $deferred.resolve({
                            target: drawEvt.target,
                            graphic: graphic
                        });
                    });

                    return $deferred.promise();
                }.bind(that);
                mIdentifyTasks["identifyDrawHandle"] = on.pausable(drawer, 'draw-complete', that._onIdentifyDrawComplete.bind(that));
                mIdentifyTasks["identifyDrawHandle"].pause();
                mDrawer = drawer;
                //
                console.log('finished initial drawer.');
            });
        },

        _initEditor: function (map) {

        },

        _onIdentifyClick: function (evt) {
            var that = this;
            console.log('click');
            mMap.infoWindow.show(evt.mapPoint);
            if (mIdentifyTasks["identifyDrawHandle"]) {
                mIdentifyTasks["identifyDrawHandle"].pause();
            }

            mIdentifyTasks.identifyExecuteHandle.call(undefined, evt.mapPoint).then(function (results) {
                mIdentifyTasks.parseResults(results);
                if (mIdentifyTasks["identifyDrawHandle"]) {
                    mIdentifyTasks["identifyDrawHandle"].resume();
                }

            });
        },

        _beginIdentifyRaster: function (map, url_map) {
            require(["dojo/_base/array",
                'esri/InfoTemplate',
                "esri/dijit/Legend",
                "esri/geometry/Extent",
                "esri/config",
                "esri/map",
                "esri/dijit/BasemapToggle",
                "esri/toolbars/navigation",
                "esri/layers/WMSLayer",
                "esri/layers/FeatureLayer",
                "esri/layers/ArcGISTiledMapServiceLayer",
                "esri/layers/ArcGISDynamicMapServiceLayer",
                "esri/layers/GraphicsLayer",
                "esri/symbols/SimpleLineSymbol",
                "esri/symbols/SimpleFillSymbol",
                "esri/symbols/SimpleMarkerSymbol",
                "esri/symbols/PictureMarkerSymbol",
                "esri/Color", "esri/tasks/geometry",
                "esri/tasks/query",
                "esri/geometry/Point",
                "esri/dijit/InfoWindow",
                'esri/geometry/screenUtils',
                "esri/dijit/Popup",
                "esri/tasks/IdentifyTask", "esri/tasks/IdentifyParameters",
                "dojo/domReady!"
            ],
                function (arrayUtils, InfoTemplate, Legend, Extent, esriConfig, Map, BasemapToggle, Navigation, WMSLayer, FeatureLayer, ArcGISTiledMapServiceLayer,
                    ArcGISDynamicMapServiceLayer, GraphicsLayer, SimpleLineSymbol, SimpleFillSymbol, SimpleMarkerSymbol, PictureMarkerSymbol, Color, Geometry, Query, Point,
                    InfoWindow,
                    screenUtils, Popup, IdentifyTask, IdentifyParameters,
                    domConstruct
                ) {
                    //let map = mMap;
                    
                    var identifyTask, identifyParams;
                    var parcelsURL = url_map;//"http://tiengontructuyen.vn/arcgis/rest/services/RasterNoise/bando_20211028_20/MapServer";
                    identifyTask = new IdentifyTask(parcelsURL);

                    identifyParams = new IdentifyParameters();
                    identifyParams.tolerance = 1;
                    identifyParams.returnGeometry = true;
                    identifyParams.layerIds = [0];
                    identifyParams.layerOption = IdentifyParameters.LAYER_OPTION_ALL;
                    identifyParams.width = map.width;
                    identifyParams.height = map.height;
                    map.on("click", executeIdentifyTask);

                    function executeIdentifyTask(event) {
                        identifyParams.geometry = event.mapPoint;
                        identifyParams.mapExtent = map.extent;

                        var deferred = identifyTask
                            .execute(identifyParams);
                        deferred.addCallback(function (response) {
                            console.log(response);
                            // response is an array of identify result objects
                            // Let's return an array of features.
                            return arrayUtils.map(response, function (result) {
                                console.log(result);
                                var feature = result.feature;
                                var layerName = result.layerName;
                                //alert(result);
                                feature.attributes.layerName = layerName;
                                var taxParcelTemplate = new InfoTemplate('Giá trị tiếng ồn',
                                    "Giá trị : " + parseFloat(feature.attributes['Pixel Value']).toFixed(2) + "(dBA)");
                                feature.setInfoTemplate(taxParcelTemplate);
                                // feature.setInfoTemplate("Pixel Value is : " + feature.attributes['Pixel Value']);
                                feature.setInfoTemplate(taxParcelTemplate);
                                return feature;
                            });
                        });

                        // InfoWindow expects an array of features from each deferred
                        // object that you pass. If the response from the task execution
                        // above is not an array of features, then you need to add a callback
                        // like the one above to post-process the response and return an
                        // array of features.
                        map.infoWindow.setFeatures([deferred]);

                        map.infoWindow.show(event.mapPoint);//
                    }
                });
        },
        //_onInitEditting: function (evt) {

        //},

        _onIdentifyDrawComplete: function (evt) {
            var that = this;
            mMap.infoWindow.show(evt.geometry.getCentroid());
            mIdentifyTasks.identifyExecuteHandle.call(undefined, evt.geometry).done(function (results) {
                mIdentifyTasks.parseResults(results);
            });
        },
    };


    init.prototype.Intersect = function (areaCode, centroid, $deferred) {
        var that = this;
        var map = mMap;
        $deferred = $deferred || $.Deferred();
        if (map)
            map.graphics.clear();

        require(['esri/tasks/query', 'esri/graphic', "esri/geometry/geometryEngine"], function (Query, Graphic, geometryEngine) {
            var queryTask;
            var query = new esri.tasks.Query();
            query.returnGeometry = true;
            $.ajax({
                url: "/front/region/GetCommune?districtId=196",
                dataType: 'json',
                type: "GET"
            }).done(function (xhr) {
                let check = false;
                xhr.forEach(function (item, index) {
                    queryTask = new esri.tasks.QueryTask(baseMapUri + "/3");
                    query.where = "maXa  = " + item.COMMUNEID;
                    queryTask.execute(query, function (results) {
                        var pointPolygon = new Graphic(centroid);
                        var intersects = geometryEngine.intersects(results.features[0].geometry, pointPolygon.geometry);
                        if (intersects && !check) {
                            check = true;
                            $deferred.resolve(item);
                        }
                        if (index == xhr.length - 1 && !check)
                            $deferred.resolve(null);
                    });
                });
            });
        });

        return $deferred.promise();
    },


        init.prototype.addAnhVeTinh = function (mapId, opacity) {
            require([
                "esri/layers/ArcGISDynamicMapServiceLayer",
            ], function (ArcGISDynamicMapServiceLayer) {

                var result = mMap.addLayer(new ArcGISDynamicMapServiceLayer(urlService + mapId + "/MapServer", {
                    id: mapId,
                    opacity: opacity
                }));
                if (result.loaded) {
                    mMap.on("update-start", function () {
                        App.startPageLoading({
                            message: "Đang tải bản đồ ..."
                        });
                    });
                    mMap.on("update-end", function () {
                        App.stopPageLoading();
                    });
                } else {
                    App.stopPageLoading();
                    bootbox.alert({
                        message: "Chưa có thông tin ảnh vệ tinh!",
                        buttons: {
                            ok: {
                                label: "Đồng ý"
                            }
                        }
                    });
                }

            });
        };
    init.prototype.addMapLayer = function (mapId, opacity) {
        require([
            "esri/layers/ArcGISDynamicMapServiceLayer",
        ], function (ArcGISDynamicMapServiceLayer) {

            var result = mMap.addLayer(new ArcGISDynamicMapServiceLayer(urlService + mapId + "/MapServer", {
                id: mapId,
                opacity: opacity,
            }));

            mMap.on("update-start", function () {
                App.startPageLoading({
                    message: "Đang tải bản đồ ..."
                });
            });

            mMap.on("update-end", function () {

                App.stopPageLoading();
            });
        });
    };
    init.prototype.addMapLayerNewest = function (mapId, opacity) {
        require([
            "esri/layers/ArcGISDynamicMapServiceLayer",
        ], function (ArcGISDynamicMapServiceLayer) {
            $.ajax({
                url: `${urlService}?f=pjson`,
                success: function (xhr) {
                    let data = JSON.parse(xhr);
                    let services = data.services;

                    for (var i = 0; i < 5; i++) {
                        //for (var j = 1; j < 13; j++) {
                        var mapName = mapId + "_" + ((new Date()).getFullYear() - i);//+ j

                        //if (j < 10) {
                        //    mapName = mapId + "_0" + i;//+ j
                        //}
                        window.services = services;

                        if (services.filter(s => s.name.includes("HienTrangSDDCapHuyen_2017")).length > 0) {
                            var result = mMap.addLayer(new ArcGISDynamicMapServiceLayer(urlService + mapName + "/MapServer", {
                                id: mapId,
                                opacity: opacity,
                            }));
                            mMap.on("update-start", function () {
                                App.startPageLoading({
                                    message: "Đang tải bản đồ ..."
                                });
                            });
                            mMap.on("update-end", function () {
                                App.stopPageLoading();
                            });
                            return;
                        }
                        //}
                    }
                }
            });
        });
    };

    init.prototype.startDraw = function (tool, onMemory, drawCall) {
        return mDrawer.draw(tool, onMemory, drawCall);
    };
    init.prototype.stopDraw = function () {
        mDrawer.deactivate();
    };

    init.prototype.loadBoundaryWithNote = function (value, $deferred) {
        var that = this;
        var map = mMap;
        $deferred = $deferred || $.Deferred();
        if (map)
            map.graphics.clear();

        require(['esri/tasks/query', 'esri/graphic'], function (Query, Graphic) {
            var queryTask;
            var query = new esri.tasks.Query();
            query.returnGeometry = true;
            queryTask = new esri.tasks.QueryTask(baseMapUri + "/20");// Tỉnh
            query.where = "ghiChu = " + areaCode;

            queryTask.execute(query, function (results) {
                regionGL.remove(regionGraphic);
                regionGraphic = new esri.Graphic(results.features[0].geometry, regionSymbol);
                regionGL.add(regionGraphic);
                var extent = esri.graphicsExtent(results.features);
                if (extent) {
                    if (map)
                        map.setExtent(extent);
                }
            });
            $deferred.resolve();
        });

        return $deferred.promise();
    };

    init.prototype.loadBoundary = function (areaCode, $deferred) {
        var that = this;
        var map = mMap;
        $deferred = $deferred || $.Deferred();
        if (map)
            map.graphics.clear();

        require(['esri/tasks/query', 'esri/graphic'], function (Query, Graphic) {
            var queryTask;
            var query = new esri.tasks.Query();
            query.returnGeometry = true;
            if (areaCode.toString().length == 5) {
                queryTask = new esri.tasks.QueryTask(baseMapUri + "/3");
                query.where = "maXa  = " + areaCode;
            } else if (areaCode.toString().length == 3) {
                queryTask = new esri.tasks.QueryTask(baseMapUri + "/4");
                query.where = "maHuyen  = " + areaCode;
            } else {
                queryTask = new esri.tasks.QueryTask(baseMapUri + "/5");
                query.where = "maTinh = " + areaCode;
            }
            queryTask.execute(query, function (results) {
                if (results) {
                    regionGL.remove(regionGraphic);
                    regionGraphic = new esri.Graphic(results.features[0].geometry, regionSymbol);
                    regionGL.add(regionGraphic);
                    var extent = esri.graphicsExtent(results.features);
                    if (extent) {
                        if (map)
                            map.setExtent(extent);
                    }
                }
            });
            $deferred.resolve();
        });

        return $deferred.promise();
    };

    init.prototype.loadTramDo = function (pQuery, $deferred) {
        return;
        var that = this;
        var map = mMap;
        $deferred = $deferred || $.Deferred();
        //if (map)
        //    map.graphics.clear();

        require(['esri/tasks/query', 'esri/graphic', 'esri/InfoTemplate',], function (Query, Graphic, InfoTemplate) {
            //return;
            var queryTask;
            var query = new esri.tasks.Query();
            query.returnGeometry = true;
            queryTask = new esri.tasks.QueryTask(baseMapUriTramDo + "/0");
            query.where = pQuery;
            query.outFields = ["tenDiemQuanTrac"];
            queryTask.execute(query, function (featureSet) {
                //console.log(results);
                //console.log(map.graphics);
                //if (results) {
                //    if (tramDoGraphic) {
                //        tramDoGL.remove(tramDoGraphic);
                //    }
                //    for (var i = 0; i < results.features.length; i++) {
                //        //tramDoGraphic = new esri.Graphic(results.features[i].geometry, tramDoSymbol);
                //        //tramDoGL.add(tramDoGraphic);
                //        map.graphics.add(new esri.Graphic(results.features[i].geometry, tramDoSymbol));
                //    }


                //}

                map.graphics.clear();

                //Performance enhancer - assign featureSet array to a single variable.
                var resultFeatures = featureSet.features;

                //Loop through each feature returned
                for (var i = 0, il = resultFeatures.length; i < il; i++) {
                    //Get the current feature from the featureSet.
                    //Feature is a graphic
                    console.log(resultFeatures[i].geometry)
                    var graphic = resultFeatures[i];
                    graphic.setSymbol(mHighLightSymbol);

                    var attributes = graphic.attributes;

                    //var date = new Date(attributes.ngayQuanTrac);
                    //var hour = date.getHours() > 9 ? date.getHours() : "0" + date.getHours();
                    //var minute = date.getMinutes() > 9 ? date.getMinutes() : "0" + date.getMinutes();
                    //var second = date.getSeconds() > 9 ? date.getSeconds() : "0" + date.getSeconds();
                    //var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
                    //var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0" + (date.getMonth() + 1);
                    //var strDate = `${day}/${month}/${date.getFullYear()} ${hour}:${minute}:${second} `;


                    var infoTemplate = new InfoTemplate(`<b>Địa điểm</b> : ${attributes.tenDiemQuanTrac} <br />`);
                    //<b>Thời gian thu thập: ${strDate} <br />
                    //<b>Giá trị độ ồn</b> : ${attributes.dB}`);

                    //Set the infoTemplate.
                    graphic.setInfoTemplate(infoTemplate);

                    //Add graphic to the map graphics layer.
                    map.graphics.add(graphic);
                }

            });
            $deferred.resolve();
        });

        return $deferred.promise();
    };

    init.prototype.initPrint = (map) => {
        require([
            "dojo/dom",
            "esri/dijit/Print"
        ], function (dom, Print) {
            m_Print = new Print({
                map: map,
                url: "https://sampleserver6.arcgisonline.com/arcgis/rest/services/Utilities/PrintingTools/GPServer/Export%20Web%20Map%20Task"
            }, dom.byId("printButton"));
            m_Print.deactivate = function () {
                $('#printButton').css('display', 'none');
            };
            m_Print.activate = function () {
                $('#printButton').css('display', 'block');
            };
            m_Print.startup();
        });
    };
    init.prototype.initMeasurement = (map) => {
        require([
            "dojo/dom",
            "esri/units",
            "esri/dijit/Measurement",
            "dojo/parser",
        ], function (dom, Units, Measurement, parser) {

            parser.parse()

            window.Unit_1 = Units;
            m_Measurement = new Measurement({
                map: map,
                defaultAreaUnit: Units.SQUARE_METERS,
                defaultLengthUnit: Units.METERS
            }, dom.byId("measurementDiv"));//"measurementDiv"
            m_Measurement.deactivate = function () {
                if (m_Measurement.activeTool) {
                    m_Measurement.setTool(m_Measurement.activeTool, false);
                }
                $('#measurementDiv').css('display', 'none');
            };
            m_Measurement.activate = function () {
                $('#measurementDiv').css('display', 'block');
            };
            m_Measurement.setVisibility = function (visible) {
                if ($('.frame-measurement')) {
                    if (visible)
                        $('.frame-measurement').fadeIn();
                    else
                        $panel.fadeOut();
                    var tool = m_Measurement.getTool();
                    if (tool)
                        m_Measurement.setTool(tool.toolName, false);
                }
            };
            window.measurement = m_Measurement;
            m_Measurement.startup();
        });
    };

    init.prototype.activateIdentify = function () {
        if (mIdentifyTasks["identifyDrawHandle"])
            mIdentifyTasks["identifyDrawHandle"].resume();
        if (mIdentifyTasks["identifyClickHandle"]) {
            mIdentifyTasks["identifyClickHandle"].resume();
        }

        mDrawer.activate(esri.toolbars.Draw.RECTANGLE);
    };
    init.prototype.deactivateIdentify = function () {
        if (mIdentifyTasks["identifyDrawHandle"])
            mIdentifyTasks["identifyDrawHandle"].pause();
        if (mIdentifyTasks["identifyClickHandle"]) {
            mIdentifyTasks["identifyClickHandle"].pause();
        }
        mDrawer.deactivate();
        if (mMap.infoWindow.isShowing)
            mMap.infoWindow.hide();
    };
    init.prototype.activateZoomIn = function () {
        require([
            "esri/toolbars/navigation",
        ], function (Navigation) {
            mNavigation.activate(Navigation.ZOOM_IN);
            mMap.setMapCursor('zoom-in');
        });
    };

    init.prototype.toggleEditTool = function (isActive) {
        if (this.options.mapView.role) {
            $("#form-submit").hide();
        }
        $("#form-accept").show();
        $('#form-delete').show();

        var that = this;
        require([
            "dojo/_base/array",
            "esri/toolbars/edit",
            "dojo/_base/event",
            'esri/tasks/query', 'esri/graphic'
        ], function (arrayUtils, Edit, event, Query, Graphic) {
            $("#form-dismiss").unbind('click');
            $("#form-accept").unbind('click');
            $("#form-delete").unbind('click');

            var firePerimeterFL = mMap.getLayer("doiTuongVung");

            if (isActive) {
                firePerimeterFL.on("click", function (evt) {

                    event.stop(evt);
                    if ($(".mdi-check").parent().parent().hasClass('active')) {
                        if (!editToolbar)
                            editToolbar = new Edit(mMap);
                        editToolbar.activate(Edit.EDIT_VERTICES | Edit.MOVE, evt.graphic);

                        var graphic = evt.graphic;
                        var attr = graphic.attributes;

                        $("#toaDoX").html("Tọa độ X: " + graphic.geometry.x);//.getLongitude());
                        $("#toaDoY").html("Tọa độ Y: " + graphic.geometry.y);//getLatitude());

                        $("#phanHoi").val(attr.phanHoi);

                        $("#infoModal").modal();
                        mEditGraphic.push(graphic);
                        /*
                        $.ajax({
                            url: "/front/region/GetCommuneById?id=" + attr.maXa,
                            dataType: 'json',
                            type: "GET"
                        }).done(function (xhr) {

                            $("#toaDoX").html("Tọa độ X: " + graphic.geometry.getCentroid().x);
                            $("#toaDoY").html("Tọa độ Y: " + graphic.geometry.getCentroid().y);

                            $("#district_name").html("Quận/Huyện: " + xhr.DISTRICT);
                            $("#commune_name").html("Phường/Xã: " + xhr.NAME_VN);

                            if (attr.thongTinKhac)
                                $("#thongTinKhac").val(attr.thongTinKhac);
                            // bảo vệ tài nguyên đất
                            if (attr.loaiGiamSat)
                                $("#loaiGiamSat").val(attr.loaiGiamSat);
                            if (attr.diaDiemPhatHien)
                                $("#diaDiemPhatHien").val(attr.diaDiemPhatHien);
                            if (attr.thongTinKhac)
                                $("#thongTinKhac").val(attr.thongTinKhac);
                            if (attr.DaDuyet == 'yes') {
                                $("#DaDuyet").prop('checked', true);
                            } else {
                                $("#ChuaDuyet").prop('checked', true);
                            }
                            // Giám sát tài sản 
                            if (attr.diaDiemBienDong)
                                $("#diaDiemBienDong").val(attr.diaDiemBienDong);
                            if (attr.doiTuongPhatHien)
                                $("#doiTuongPhatHien").val(attr.doiTuongPhatHien);
                            if (attr.loaiTaiSanGanLienVoiDat)
                                $("#loaiTaiSanGanLienVoiDat").val(attr.loaiTaiSanGanLienVoiDat);
                            if (attr.tenTaiSan)
                                $("#tenTaiSan").val(attr.tenTaiSan);
                            // Vi phạm đất đai
                            if (attr.loaiHinhViPham) {
                                $("#loaiHinhViPham").val(attr.loaiHinhViPham);
                            }
                            if (attr.diaDiemViPham)
                                $("#diaDiemViPham").val(attr.diaDiemViPham);
                            if (attr.doiTuongViPham)
                                $("#doiTuongViPham").val(attr.doiTuongViPham);
                            if (attr.loaiDatViPham)
                                $("#loaiDatViPham").val(attr.loaiDatViPham);
                            if (attr.loaiViPham)
                                $("#loaiViPham").val(attr.loaiViPham);
                            $("#infoModal").modal();

                            mEditGraphic.push(graphic);
                        });
                        */
                    }
                });
            }
            else {
                if (editToolbar)
                    editToolbar.deactivate();
            }

            /*
        $("#form-dismiss").on('click', function () {
            $('#captcha').click();
            $("input[name=__submit_captcha]").val('');
            $("#toaDoX").html("");
            $("#toaDoY").html("");
            // bảo vệ tài nguyên đất
            $("#loaiGiamSat").val(0);
            $("#loaiHinhViPham").val(0);
            $("#diaDiemPhatHien").val(0);
            // Tài sản gắn liền với đất
            $("#tenTaiSan").val('');
            $("#diaDiemBienDong").val('');
            $("#doiTuongPhatHien").val('');
            $("#loaiDatViPham").val('NNP');
            $("#loaiTaiSanGanLienVoiDat").val(0);
            //Vi phạm đất đai
            $("#diaDiemViPham").val('');
            $("#doiTuongPhatHien").val('');
            $("#doiTuongViPham").val('');
            $("#tenTaiSan").val('');
            $("#loaiHinhViPham").val(0);

            $("#thongTinKhac").val("");
            $("#infoModal").modal('hide');
            mMap.graphics.clear();
            mEditGraphic = [];
            if (this.options.mapView.role) {
                $("#form-submit").hide();
            }
            $("#form-accept").show();
            $('#form-delete').show();
        });
            */
            $("#form-accept").on('click', function (e) {
                e.preventDefault();
                App.startPageLoading({
                    message: "Đang xử lý dữ liệu ..."
                });
                $.get("/home/CheckCaptchaRegister?__submit_captcha_register=" + $("input[name=__submit_captcha]").val()).done(function (r) {
                    if (r.toUpperCase() == "FALSE") {
                        $('#captcha').click();
                        $("#error-captcha").show();
                        App.stopPageLoading();
                        return;
                    }
                    $("#error-captcha").hide();

                    mEditGraphic[0].attributes.danhGia = "1";
                    mEditGraphic[0].attributes.phanHoi = $("#phanHoi").val();
                    var date = new Date();
                    var hour = date.getHours() > 9 ? date.getHours() : "0" + date.getHours();
                    var minute = date.getMinutes() > 9 ? date.getMinutes() : "0" + date.getMinutes();
                    var second = date.getSeconds() > 9 ? date.getSeconds() : "0" + date.getSeconds();
                    var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
                    var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0" + (date.getMonth() + 1);
                    var strDate = `${day}/${month}/${date.getFullYear()} ${hour}:${minute}:${second} `;

                    mEditGraphic[0].attributes.thoiGianTraLoi = strDate;
                    mEditGraphic[0].attributes.thongTinNguoiTraLoi = $("#thongTinNguoiDung").val();
                    firePerimeterFL.applyEdits(null, [mEditGraphic[0]], null, function () {
                        App.stopPageLoading();
                        mMap.graphics.clear();

                        $("#infoModal").modal('hide');
                        mEditGraphic = [];
                        bootbox.alert({
                            message: 'Lưu thông tin thành công!',
                            buttons: {
                                ok: {
                                    label: "Đồng ý"
                                }
                            }
                        });

                        if (this.options.mapView.role) {
                            $("#form-submit").hide();
                        }
                        $("#form-accept").show();
                        $('#form-delete').show();
                    });

                });
            });
            function pad2(n) {
                return n < 10 ? '0' + n : n;
            }
            $("#form-delete").on('click', function (e) {
                e.preventDefault();
                App.startPageLoading({
                    message: "Đang xử lý dữ liệu ..."
                });
                $.get("/home/CheckCaptchaRegister?__submit_captcha_register=" + $("input[name=__submit_captcha]").val()).done(function (r) {
                    if (r.toUpperCase() == "FALSE") {
                        $('#captcha').click();
                        $("#error-captcha").show();
                        App.stopPageLoading();
                        return;
                    }
                    $("#error-captcha").hide();

                    firePerimeterFL.applyEdits(null, null, [mEditGraphic[0]], function () {
                        //$("#form-accept").show();
                        //$('#form-delete').show();
                        //App.stopPageLoading();
                        //mMap.graphics.clear();
                        //$('#captcha').click();
                        //$("input[name=__submit_captcha]").val('');
                        //$("#toaDoX").html("");
                        //$("#toaDoY").html("");


                        $("#infoModal").modal('hide');
                        mEditGraphic = [];
                        bootbox.alert({
                            message: 'Xóa thông tin thành công!',
                            buttons: {
                                ok: {
                                    label: "Đồng ý"
                                }
                            }
                        });
                    });
                });
            });

            $('#infoModal').on('hidden.bs.modal', function () {
                mMap.graphics.clear();
                $("input[name=__submit_captcha]").val("")
                $("#toaDoX").html("");
                $("#toaDoY").html("");
                $("#phanHoi").val("");
                $('#captcha').click();
            });
        });
    };

    init.prototype.activateZoomOut = function () {
        require([
            "esri/toolbars/navigation",
        ], function (Navigation) {
            mNavigation.activate(Navigation.ZOOM_OUT);
            mMap.setMapCursor('zoom-out');
        });
    };
    init.prototype.clearInteractions = function () {
        if (mNavigation) {
            mNavigation.deactivate();
        }
        //
        mMap.setMapCursor('default');
    };
    init.prototype.viewFullExtent = function () {
        console.log(mMap.getZoom());
        mMap.setZoom(16);
        //console.log(mMap.fullExtent);
        //mNavigation.zoomToFullExtent();
        //this.zoomTo(this.options.mapView.center);
    };
    init.prototype.zoomTo = function (coord, zoomlevel) {
        var that = this;
        if (!zoomlevel)
            zoomlevel = mMap.getMinZoom();
        require(["esri/geometry/Point"], function (Point) {


            zoomlevel = 15;
            mMap.centerAt(new Point(coord[0], coord[1]));
            //mMap.centerAndZoom(new Point(coord[0], coord[1]), zoomlevel);//, mMap.spatialReference
            //mMap.centerAndZoom(new Point(21, 105), zoomlevel);
        });
    };
    init.prototype.zoomToExtent = function (extent) {

        if (mMap && extent) {
            require(['esri/geometry/screenUtils'], function (screenUtils) {
                var sideBarWidth = $('.page-sidebar').width();
                var centerPoint = extent.getCenter();

                var screenPoint = screenUtils.toScreenPoint(extent, mMap.width, mMap.height, centerPoint);
                screenPoint.x += sideBarWidth;
                var mapPoint = screenUtils.toMapPoint(extent, mMap.width, mMap.height, screenPoint);
                extent = extent.offset(Math.abs(mapPoint.x - centerPoint.x) * -1, 0);
                mMap.setExtent(extent.expand(2));
            });
        }
    };
    init.prototype.clear = function () {

    };
    init.prototype.getAgsMap = function () {
        return mMap;
    };
    init.prototype.putMapLoadCb = function (cb) {
        mMapLoadCb.push(cb);
    };
    init.prototype.showMeasurement = function () {
        if (this.measurementRedact)
            this.measurementRedact.setVisibility(true);
    };
    init.prototype.getMeasurement = function () {
        return m_Measurement;
    };
    init.prototype.Print = function () {
        return m_Print;
    };
    init.prototype.AddPhanHoi = function () {
        require(["esri/layers/FeatureLayer"], function (FeatureLayer) {
            mMap.addLayer(new FeatureLayer(urlRes + `/0`, {
                mode: FeatureLayer.MODE_AUTO,
                visible: true,
                outFields: ['*'],
                displayOnPan: true,
                id: 'doiTuongVung',
            }));
        })
    };

    init.prototype.RemoveLayer = function (LayerId) {
        mMap.removeLayer(mMap.getLayer(LayerId));
    };
    $.extend($.fn, {
        vgsMapView: function (opt) {
            return new init(this, opt);
        }
    });
})(jQuery, window.vgsMap);