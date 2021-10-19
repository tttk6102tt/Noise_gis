require([
    "esri/widgets/Expand",
    "esri/views/SceneView",
    "esri/config",
    "esri/Map",
    "esri/views/MapView",
    "esri/layers/FeatureLayer",
    "esri/layers/BaseDynamicLayer",
    "esri/widgets/LayerList",
    "esri/widgets/BasemapToggle",
    "esri/widgets/BasemapGallery",
    "esri/core/urlUtils", "esri/layers/MapImageLayer",
    "esri/widgets/Legend", "esri/layers/support/RasterFunction", "esri/layers/ImageryLayer", "esri/widgets/DistanceMeasurement2D",
    "esri/widgets/AreaMeasurement2D",
    "esri/tasks/Geoprocessor",
    "dijit/form/DateTextBox",
    "dojo/dom",
    "dojo/dom-construct",
    "dojo/on",
    "dojo/date/locale",
    "dojo/parser",
    "dijit/registry",
    "dojo/domReady!"
], function (Expand, SceneView, esriConfig, Map, MapView, FeatureLayer, BaseDynamicLayer, LayerList, BasemapToggle, BasemapGallery,
    urlUtils, MapImageLayer, Legend,
    RasterFunction, ImageryLayer, DistanceMeasurement2D, AreaMeasurement2D, Geoprocessor,
    DateTextBox, dom, domConstruct, on, locale, parser, registry) {
        parser.parse();

        esriConfig.apiKey = "AAPK8b347fdd72a2424085f5446381c7196fm91VYkw2ch2t0AV0X-yZcpfP25aJeZHCNyHolPveHbZh0GSjvE7TmnWZobfAdaoG";
        const map = new Map({
            basemap: "arcgis-topographic" // Basemap layer
        });
        //var map = new Map({
        //    basemap: "topo"
        //   // , ground: "world-elevation"
        //});

        const view = new MapView({
            container: "viewDiv",
            map: map,
            center: [105.793, 21.052], // longitude, latitude
            zoom: 15

        });
        const basemapToggle = new BasemapToggle({
            view: view,
            nextBasemap: "arcgis-imagery"
        });
        view.ui.add("map-option", "top-right");
        //view.ui.add(basemapToggle, "bottom-right");

        document
            .getElementById("distanceButton")
            .addEventListener("click", function () {
                setActiveWidget(null);
                if (!this.classList.contains("active")) {
                    setActiveWidget("distance");
                } else {
                    setActiveButton(null);
                }
            });

        document
            .getElementById("areaButton")
            .addEventListener("click", function () {
                setActiveWidget(null);
                if (!this.classList.contains("active")) {
                    setActiveWidget("area");
                } else {
                    setActiveButton(null);
                }
            });

        function queryTramdo() {
            // alert('aaa');
            //var query = featureLayer.createQuery();
            //var ngaydo_str = document.getElementById('ngaydo').value;
            var d = new Date(document.getElementById("ngaydo").value);
            var m = d.getMonth() + 1;
            var day = d.getDate();
            var year = d.getFullYear();
            var ngaydo = m + "/" + day + "/" + year;
            var khunggio = document.getElementById('khunggio').value;
            //alert(d.getMonth()+ " " + d.getDate());
            //alert(ngaydo+  " " + khunggio);
            if (khunggio === '6h-9h')
                thoigian_from = ngaydo + " 06:00:00";

            else if (khunggio === '9h-11h') {
                thoigian_from = ngaydo + " 09:00:00";
                thoigian_to = ngaydo + " 11:00:00";
            }

            else if (khunggio === '11h-13h') {
                thoigian_from = ngaydo + " 11:00:00";
                thoigian_to = ngaydo + " 13:00:00";
            }
            else if (khunggio === '13h-16h') {
                thoigian_from = ngaydo + " 13:00:00";
                thoigian_to = ngaydo + " 16:00:00";
            }
            else if (khunggio === '16h-19h30') {
                thoigian_from = ngaydo + " 16:00:00";
                thoigian_to = ngaydo + " 19:30:00";
            }
            else if (khunggio === '0h-6h') {
                thoigian_from = ngaydo + " 00:00:00";
                thoigian_to = ngaydo + " 06:00:00";
            }
            else {
                thoigian_from = ngaydo + " 06:00:00";
                thoigian_to = ngaydo + " 09:00:00";
            }
            // alert(thoigian_from + " " + thoigian_to);
            sql = ' TIME >=' + "'" + thoigian_from + "'" + ' AND TIME < ' + "'" + thoigian_to + "'"
            // query.where = sql;
            // alert(sql);
            featureLayer.definitionExpression = sql;
            //query.geometry = wellBuffer;
            //query.spatialRelationship = "intersects";
            // hien thi ban do
            var bando_ngay = m.toString() + day.toString() + year.toString();
            var tenbando = "bando_" + bando_ngay + "_" + khunggio.replace("-", "_");
            var url_map = "http://tiengontructuyen.vn/arcgis/rest/services/bando/" + tenbando + "/MapServer";
            cleanup();
            var maplayer = new MapImageLayer({
                url: url_map, /*"http://tiengontructuyen.vn/arcgis/rest/services/bando/bando_5102021_9h_11h/MapServer"*/

                title: "BanDoNoiSuy"


            });
            //view.center = maplayer.e;
            map.add(maplayer);
            // zoomToLayer(maplayer);

            //return featureLayer.queryFeatures(query);
        }
        function zoomToLayer(layer) {
            return layer.queryExtent().then((response) => {
                view.goTo(response.extent)
                    .catch((error) => {
                        console.error(error);
                    });
            });
        }
        function setActiveWidget(type) {
            switch (type) {
                case "distance":
                    activeWidget = new DistanceMeasurement2D({
                        view: view
                    });

                    // skip the initial 'new measurement' button
                    activeWidget.viewModel.newMeasurement();

                    view.ui.add(activeWidget, "top-right");
                    setActiveButton(document.getElementById("distanceButton"));
                    break;
                case "area":
                    activeWidget = new AreaMeasurement2D({
                        view: view
                    });

                    // skip the initial 'new measurement' button
                    activeWidget.viewModel.newMeasurement();

                    view.ui.add(activeWidget, "top-right");
                    setActiveButton(document.getElementById("areaButton"));
                    break;
                case null:
                    if (activeWidget) {
                        view.ui.remove(activeWidget);
                        activeWidget.destroy();
                        activeWidget = null;
                    }
                    break;
            }
        }


        function setActiveButton(selectedButton) {
            // focus the view to activate keyboard shortcuts for sketching
            view.focus();
            var elements = document.getElementsByClassName("active");
            for (var i = 0; i < elements.length; i++) {
                elements[i].classList.remove("active");
            }
            if (selectedButton) {
                selectedButton.classList.add("active");
            }
        }
        function cleanup() {
            // remove the geoprocessing result layer from the map
            map.layers.forEach(function (layer, i) {
                if (layer.title === "BanDoNoiSuy") {
                    map.layers.remove(layer);
                }
            });
        }
        var tansuatlaymau = document.getElementById("tan_suat_lay_mau").value;
        var time_filter = "1=1";
        if (tansuatlaymau > 0) {
            var now = new Date();
            var dd = now.getDate();
            var mm = now.getMonth() + 1;
            var yyyy = now.getFullYear();
            var h = now.getHours(); var m = now.getMinutes(); var s = now.getSeconds();
            var time_to = mm + "/" + dd + "/" + yyyy + " " + h + ":" + m + ":" + s;

            var from = new Date(now);

            from.setMinutes(now.getMinutes() - tansuatlaymau);
            var d_f = from.getDate(); var m_f = from.getMonth() + 1; var y_f = from.getFullYear();
            var time_from = m_f + "/" + d_f + "/" + y_f + " " + from.getHours().toString() + ":" + from.getMinutes().toString() + ":" + from.getSeconds().toString();

            time_filter = ' TIME >=' + "'" + time_from + "'" + ' AND TIME < ' + "'" + time_to + "'";
            //alert(time_filter + "  "+tansuatlaymau)
        }

        //============================add du lieu ban do==================
        var featureLayer = new FeatureLayer({
            url:
                //"http://localhost/arcgis/rest/services/noisy/quantractiengon/FeatureServer/1",
                "http://tiengontructuyen.vn/arcgis/rest/services/bando/tramdo/FeatureServer/0",
            title: "Trạm quan trắc",
            outFields: ["maTram", "dB", "TIME"],
            definitionExpression: time_filter,
            visible: false,
            popupTemplate: {
                title: "{Tên trạm đo}",
                actions: [
                    {
                        id: "find-maTram",
                        //image:
                        title: "Thông tin trạm quan trắc"
                    }
                ],
                content: [
                    {
                        type: "fields",
                        fieldInfos: [
                            {
                                fieldName: "maTram",
                                label: "Mã Trạm đo"
                            },
                            {
                                fieldName: "dB",
                                label: "Độ ồn"
                            },
                            {
                                fieldName: "TIME",
                                label: "Thời gian đo"
                            }
                        ]
                    }
                ]
            }
        });

        var imagePopupTemplate = {
            // autocasts as new PopupTemplate()
            title: "Giá trị độ ồn nội suy",
            content:
                "Rendered RGB values: <b>{Raster.ServicePixelValue} </b>" +
                "<br>Original values (B, G, R, NIR): <b>{Raster.ItemPixelValue} </b>"
        };

        map.add(featureLayer);

        view.when(function () {
            var popup = view.popup;
            popup.viewModel.on("trigger-action", function (event) {
                if (event.action.id === "find-maTram") {
                    var attributes = popup.viewModel.selectedFeature.attributes;
                    // Get the 'website' field attribute
                    //var info = attributes.website;
                    //// Make sure the 'website' field value is not null
                    //if (info) {
                    //    // Open up a new browser using the URL value in the 'website' field
                    //    window.open(info.trim());
                    //}
                }
            });

            const chartExpand = new Expand({
                view: view,
                content: document.getElementById("controls"),
                expandIconClass: "esri-icon-filter",
                expanded: true
            });

            view.ui.add(chartExpand, "top-left");

            const filterExpand = new Expand({
                view: view,
                content: document.getElementById("dv-filter"),
                expandIconClass: "esri-icon-filter",
                expanded: true
            });

            view.ui.add(filterExpand, "top-right");
            //
            const legendExpand = new Expand({
                view: view,
                content: document.getElementById("dv-legend"),
                expandIconClass: "esri-icon-legend",
                expanded: false
            });

            view.ui.add(legendExpand, "bottom-right");

            document
                .getElementById("noisuy")
                .addEventListener("click", function () {
                    queryTramdo();
                    legendExpand.expanded = true;
                });
        });

        /////////////////

        //// create a new layer list
        var layerList = new LayerList({
            view: view,
            listItemCreatedFunction: function (event) {
                const item = event.item;
                if (item.layer.type !== "group") {
                    // don't show legend twice
                    item.panel = {
                        content: "legend",
                        open: true
                    };
                }
            }
        }, "legend");
    });
