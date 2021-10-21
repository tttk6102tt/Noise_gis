(function ($, vgsMap) {

    'use strict';

    vgsMap = vgsMap || {};

    MapControls.DEFAULT = 'default';
    MapControls.ZOOM_IN = 'zoom_in';
    MapControls.ZOOM_OUT = 'zo_out';
    MapControls.FULL_EXTENT = 'full_extent';
    MapControls.BACK_EXTENT = 'back_extent';
    MapControls.NEXT_EXTENT = 'next_extent';
    MapControls.REFRESH = 'refresh';
    MapControls.IDENTIFY = 'identify';
    MapControls.MEASUREMENT_LENGTH = 'measurement_length';
    MapControls.MEASUREMENT_AREA = 'measurement_area';
    MapControls.BOOKMARKER = 'bookmarker';
    MapControls.EDITOR = 'editor';
    MapControls.SELECT_AREA = 'select_area';
    MapControls.SELECT_RECTANGLE = 'select_rectangle';
    MapControls.PRINT = 'print';
    MapControls.EXPORT_IMAGE = 'export_image';
    MapControls.DRAW_POINT = 'draw_point';
    MapControls.DRAW_POLYLINE = 'draw_polyline';
    MapControls.DRAW_POLYGON = 'draw_polygon';
    MapControls.DRAW_POLYGON_LAND_USING = 'draw_polygon_landusing';
    MapControls.DRAW_POLYGON_PROTECT = 'draw_polygon_protect';
    MapControls.DRAW_POLYGON_ASSETS = 'draw_polygon_assets';
    MapControls.FILTER = 'filter';
    MapControls.SHOW_CHART = 'show_chart';
    MapControls.REPORT = 'report';
    //MapControls.BUFFER_FEATURE = 'buffer_feature';

    var _measurement;

    var _timeInterval;

    var defControls = [{
        name: MapControls.DEFAULT,
        title: 'Mặc định'
    }];

    var defOptions = {
        default: MapControls.DEFAULT,
        controls: defControls,
        position: 'top-right',
        direction: 'vertical'
    };

    var SELECT = 'select',
        CLICK = 'click';

    var events = [
        SELECT,
        CLICK
    ];

    var instance;

    var controlClass = 'frame-control',
        controlTRClass = 'frame-control-tr',
        controlTLClass = 'frame-control-tl',
        controlBRClass = 'frame-control-br',
        controlBLClass = 'frame-control-bl',
        controlListClass = 'frame-control-list',
        controlListHozClass = 'frame-control-list-horizontal',
        controlListVerClass = 'frame-control-list-vertical';

    var mMap;

    function MapControls(element, extendOptions) {
        console.log(element);
        if (!element)
            throw 'Element is null';
        else {
            instance = this;
            this.element = element;
            this.options = $.extend(true, {}, defOptions, extendOptions);
            this._list = null;
            this._listData = {};
            this._active = null;
            mMap = this.options.map;
            $(this.element).data('vgsMapToolbar', this);
            return this._init();
        }
    }

    vgsMap.MapToolBar = MapControls;

    MapControls.config = {};
    MapControls.config[MapControls.DEFAULT] = {
        name: MapControls.DEFAULT,
        title: 'Mặc định',
        iconClass: 'mdi mdi-cursor-default',
        toggle: true
    };
    MapControls.config[MapControls.ZOOM_IN] = {
        name: MapControls.ZOOM_IN,
        title: 'Phóng to theo vùng',
        iconClass: 'mdi mdi-magnify-plus-outline',
        toggle: true
    };
    MapControls.config[MapControls.ZOOM_OUT] = {
        name: MapControls.ZOOM_OUT,
        title: 'Thu nhỏ theo vùng',
        iconClass: 'mdi mdi-magnify-minus-outline',
        toggle: true
    };
    MapControls.config[MapControls.FULL_EXTENT] = {
        name: MapControls.FULL_EXTENT,
        title: 'Toàn cảnh',
        iconClass: 'mdi mdi-earth',
        toggle: false
    };
    MapControls.config[MapControls.BACK_EXTENT] = {
        name: MapControls.BACK_EXTENT,
        title: 'Trạng thái trước',
        iconClass: 'icons-ctrl-prev',
        toggle: false
    };
    MapControls.config[MapControls.NEXT_EXTENT] = {
        name: MapControls.NEXT_EXTENT,
        title: 'Trạng thái kế tiếp',
        iconClass: 'icons-ctrl-next',
        toggle: false
    };
    MapControls.config[MapControls.REFRESH] = {
        name: MapControls.REFRESH,
        title: 'Làm mới bản đồ',
        iconClass: 'mdi mdi-autorenew',
        toggle: false
    };
    MapControls.config[MapControls.IDENTIFY] = {
        name: MapControls.IDENTIFY,
        title: 'Thông tin đối tượng',
        iconClass: 'mdi mdi-information-outline',
        toggle: true
    };
    MapControls.config[MapControls.MEASUREMENT_LENGTH] = {
        name: MapControls.MEASUREMENT_LENGTH,
        title: 'Đo khoảng cách',
        iconClass: 'mdi mdi-map-marker-distance',
        toggle: true
    };
    MapControls.config[MapControls.MEASUREMENT_AREA] = {
        name: MapControls.MEASUREMENT_AREA,
        title: 'Đo đạc',
        iconClass: 'mdi mdi-map-marker-distance',
        toggle: true
    };
    MapControls.config[MapControls.BOOKMARKER] = {
        name: MapControls.BOOKMARKER,
        title: 'Đánh dấu',
        iconClass: 'icons-ctrl-book',
        toggle: true
    };
    MapControls.config[MapControls.EDITOR] = {
        name: MapControls.EDITOR,
        title: 'Xác nhận',
        iconClass: 'mdi  mdi-check',
        toggle: true
    };
    MapControls.config[MapControls.SELECT_AREA] = {
        name: MapControls.SELECT_AREA,
        title: 'Khoanh vùng để truy vấn',
        iconClass: 'icons-ctrl-polygon',
        toggle: true
    };
    MapControls.config[MapControls.SELECT_RECTANGLE] = {
        name: MapControls.SELECT_RECTANGLE,
        title: 'Quét chữ nhật để truy vấn',
        iconClass: 'icons-ctrl-square',
        toggle: true
    };
    MapControls.config[MapControls.PRINT] = {
        name: MapControls.PRINT,
        title: 'In bản đồ',
        iconClass: 'mdi mdi-printer',
        toggle: true
    };
    MapControls.config[MapControls.EXPORT_IMAGE] = {
        name: MapControls.EXPORT_IMAGE,
        title: 'Xuất ảnh bản đồ',
        iconClass: 'icons-ctrl-photo',
        toggle: true
    };
    MapControls.config[MapControls.DRAW_POINT] = {
        name: MapControls.DRAW_POINT,
        title: 'Vẽ đối tượng điểm',
        iconClass: 'mdi mdi-map-marker-plus',
        toggle: true
    };
    MapControls.config[MapControls.DRAW_POLYLINE] = {
        name: MapControls.DRAW_POLYLINE,
        title: 'Vẽ đối tượng đường',
        iconClass: 'mdi mdi-shape-rectangle-plus',
        toggle: true
    };
    MapControls.config[MapControls.DRAW_POLYGON] = {
        name: MapControls.DRAW_POLYGON,
        title: 'Vẽ đối tượng vùng',
        iconClass: 'mdi mdi-shape-polygon-plus',
        toggle: true
    };
    MapControls.config[MapControls.DRAW_POLYGON_LAND_USING] = {
        name: MapControls.DRAW_POLYGON_LAND_USING,
        title: 'Người dùng phản hồi',
        iconClass: 'mdi mdi-shape-polygon-plus',
        toggle: true
    };
    MapControls.config[MapControls.DRAW_POLYGON_PROTECT] = {
        name: MapControls.DRAW_POLYGON_PROTECT,
        title: 'Vẽ đối tượng vùng',
        iconClass: 'mdi mdi-shape-polygon-plus',
        toggle: true
    };
    MapControls.config[MapControls.DRAW_POLYGON_ASSETS] = {
        name: MapControls.DRAW_POLYGON_ASSETS,
        title: 'Vẽ đối tượng vùng',
        iconClass: 'mdi mdi-shape-polygon-plus',
        toggle: true
    };
    MapControls.config[MapControls.FILTER] = {
        name: MapControls.FILTER,
        title: 'Tra cứu dữ liệu',
        iconClass: 'mdi mdi-filter-outline',
        toggle: true
    };
    MapControls.config[MapControls.SHOW_CHART] = {
        name: MapControls.SHOW_CHART,
        title: 'Biểu đồ',
        iconClass: 'mdi mdi-chart-areaspline',
        toggle: true
    };
    MapControls.config[MapControls.REPORT] = {
        name: MapControls.REPORT,
        title: 'Biểu đồ',
        iconClass: 'mdi mdi-poll-box',
        toggle: true
    };
    MapControls.prototype = {
        _init: function () {

            var $element = $(this.element);
            if (!$element.hasClass(controlClass))
                $element.addClass(controlClass);
            if (this.options.position === 'top-right')
                $element.addClass(controlTRClass);
            else if (this.options.position === 'top-left')
                $element.addClass(controlTLClass);
            else if (this.options.position === 'bottom-right')
                $element.addClass(controlBRClass);
            else if (this.options.position === 'bottom-left')
                $element.addClass(controlBLClass);
            else
                $element.addClass(controlTRClass);
            $element.empty();
            //
            this._list = $('<ul></ul>');
            this._list.addClass(controlListClass);
            if (this.options.direction === 'horizontal')
                this._list.addClass(controlListHozClass);
            else
                this._list.addClass(controlListVerClass);
            this.options.controls.map(function (control) {
                if (typeof control === 'string') {
                    var $li = $('<li></li>'),
                        $a = $('<a></a>'),
                        ctr = MapControls.config[control];
                    if (!ctr)
                        return;
                    //$a.addClass('tooltip');
                    $a.attr('title', ctr.title);
                    $a.attr('href', '#');
                    $a.data('vgsMapControl', ctr);
                    $li.html($a);
                    $a.html('<i class="' + ctr.iconClass + '"></i>');
                    instance._listData[control] = $a;
                    $li.appendTo(instance._list);
                    if (instance.options.default === control)
                        $li.addClass('active');
                } else if (typeof control === 'object') {
                    var $li = $('<li></li>'),
                        $a = $('<a></a>');
                    //$a.addClass('tooltip');
                    $a.attr('title', control.title || MapControls.config[control.name].title);
                    $a.attr('href', '#');
                    $a.data('vgsMapControl', MapControls.config[control.name]);
                    $li.html($a);
                    $a.html('<i class="' + (control.iconClass || MapControls.config[control.name].iconClass) + '"></i>');
                    instance._listData[control.name] = $a;
                    $li.appendTo(instance._list);
                    if (instance.options.default === control.name)
                        $li.addClass('active');
                }
            });
            this._list.appendTo($element);
            $('a', $element).on('mousedown', function (e) {
                var data = $(this).data('vgsMapControl');
                instance.select.call(instance, {
                    sender: instance,
                    target: e,
                    newTool: data,
                    lastTool: instance._active ? MapControls.config[instance._active] : undefined
                });
                if (e.isDefaultPrevented()) { } else {
                    if (data) {
                        var lastTool = instance._active;
                        instance.activate(data.name).done(function (tool) {

                        }).fail(function () {

                        });
                    } else {

                    }
                }
            });
            if (typeof (this.options.change) === 'function')
                this.change = this.options.change;
            if (typeof (this.options.select) === 'function')
                this.select = this.options.select;
            return $(this.element);
        },
    };
    MapControls.prototype.activate = function (tool) {
        var $deferred = $.Deferred();
        var instance = this;
        var node = this._listData[tool],
            data = $(node).data('vgsMapControl');
        console.log(data.toggle);
        if (this._active === tool && data.toggle) {
            if (tool === MapControls.DEFAULT)
                $deferred.reject();
            else
                return this.activate(MapControls.DEFAULT);
        } else if (data.toggle) {
            var lastTool = instance._active;
            switch (lastTool) {
                case MapControls.IDENTIFY:
                    mMap.deactivateIdentify();
                    break;
                case MapControls.ZOOM_IN:
                case MapControls.ZOOM_OUT:
                case MapControls.SELECT_AREA:
                case MapControls.SELECT_RECTANGLE:
                    mMap.clearInteractions();
                    break;
                case MapControls.EDITOR:
                    mMap.RemoveLayer('doiTuongVung');
                    mMap.toggleEditTool(false);
                    break;
                case MapControls.BOOKMARKER:
                    break;
                case MapControls.MEASUREMENT_LENGTH:
                case MapControls.MEASUREMENT_AREA:

                    mMap.getMeasurement().deactivate();
                    break;
                case MapControls.PRINT:
                    mMap.Print().deactivate();
                    break;
                case MapControls.EXPORT_IMAGE:
                    break;
                case MapControls.DRAW_POINT:
                case MapControls.DRAW_POLYLINE:
                case MapControls.DRAW_POLYGON:
                case MapControls.DRAW_POLYGON_LAND_USING:
                case MapControls.DRAW_POLYGON_PROTECT:
                case MapControls.DRAW_POLYGON_ASSETS:
                    mMap.RemoveLayer('doiTuongVung');
                    mMap.stopDraw();
                    break;
                case MapControls.FILTER:
                    $('#dv-filter').css('display', 'none');
                    $('#legend').css('display', 'none');
                    break;
                case MapControls.REPORT:
                    $('#dv-report').css('display', 'none');
                    break;
                case MapControls.SHOW_CHART:
                    $('#controls').css('display', 'none');
                    break;
               
                default:
                    break;
            }
            instance.deactivate();
            $(node).parent().addClass('active');
            instance._active = tool;
            switch (tool) {

                case MapControls.DEFAULT:
                    break;
                case MapControls.ZOOM_IN:
                    mMap.activateZoomIn();
                    break;
                case MapControls.ZOOM_OUT:
                    mMap.activateZoomOut();
                    break;
                case MapControls.IDENTIFY:
                    mMap.activateIdentify();
                    break;
                case MapControls.EDITOR:
                    mMap.AddPhanHoi();
                    mMap.toggleEditTool(true);
                    break;
                case MapControls.SELECT_AREA:
                    break;
                case MapControls.SELECT_RECTANGLE:
                    break;
                case MapControls.MEASUREMENT_LENGTH:
                    mMap.measurement().measureLength();
                    break;
                case MapControls.MEASUREMENT_AREA:
                    mMap.getMeasurement().activate();

                    break;
                // case MapControls.BUFFER_FEATURE:
                //     mMap.showBufferFeature();
                //     break;
                case MapControls.BOOKMARKER:
                    // mMap.bookmarker().show();
                    break;
                case MapControls.PRINT:
                    //mMap.takeMapImage(function (image) {
                    //    mMap.print().restore(image);
                    //});
                    //mMap.print().show();
                    mMap.Print().activate();
                    break;
                case MapControls.EXPORT_IMAGE:
                    // mMap.toImg();
                    break;
                case MapControls.DRAW_POINT:

                    break;
                case MapControls.DRAW_POLYLINE:

                    break;
                case MapControls.DRAW_POLYGON_LAND_USING:
                    mMap.AddPhanHoi();

                    mMap.startDraw('point', false, function (graphic) {
                        mMap.getAgsMap().graphics.add(graphic);
                        $("#infoModal").modal();
                        $("#toaDoX").html("Tọa độ X: " + graphic.geometry.x);// graphic.geometry.getLongitude());
                        $("#toaDoY").html("Tọa độ Y: " + graphic.geometry.y);//graphic.geometry.getLatitude());

                        $("#form-submit").on('click', function (e) {
                            App.startPageLoading({
                                message: "Đang xử lý dữ liệu ..."
                            });

                            e.preventDefault();
                            $.get("/home/CheckCaptchaRegister?__submit_captcha_register=" + $("input[name=__submit_captcha]").val()).done(function (r) {
                                console.log(r)
                                if (r.toUpperCase() == "FALSE") {
                                    $('#captcha-register').click();
                                    $("#error-captcha").show();
                                    return;
                                }

                                $("#error-captcha").hide();
                                var date = new Date();
                                var hour = date.getHours() > 9 ? date.getHours() : "0" + date.getHours();
                                var minute = date.getMinutes() > 9 ? date.getMinutes() : "0" + date.getMinutes();
                                var second = date.getSeconds() > 9 ? date.getSeconds() : "0" + date.getSeconds();
                                var day = date.getDate() > 9 ? date.getDate() : "0" + date.getDate();
                                var month = (date.getMonth() + 1) > 9 ? (date.getMonth() + 1) : "0" + (date.getMonth() + 1);
                                var strDate = `${day}/${month}/${date.getFullYear()} ${hour}:${minute}:${second} `;

                                graphic.setAttributes({
                                    thoiGianPhanHoi: strDate,
                                    phanHoi: $("#phanHoi").val(),
                                    thongTinNguoiDung: $("#thongTinNguoiDung").val(),
                                    toaDoX: graphic.geometry.x,//graphic.geometry.getLongitude(),
                                    toaDoY: graphic.geometry.y,//graphic.geometry.getLatitude(),
                                });
                                mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                                    $("#infoModal").modal('hide');
                                    mMap.getAgsMap().graphics.clear();
                                    App.stopPageLoading();
                                    bootbox.alert({
                                        message: 'Lưu thông tin thành công!',
                                        buttons: {
                                            ok: {
                                                label: "Đồng ý"
                                            }
                                        }
                                    });
                                });

                            });
                        });
                        //$("#form-dismiss").on('click', function () {
                        //    mMap.getAgsMap().graphics.clear();
                        //    $("#toaDoX").html("");
                        //    $("#toaDoY").html("");
                        //    $("#thongTinNguoiDung").val("");
                        //    $('#captcha').click();
                        //});

                        $('#infoModal').on('hidden.bs.modal', function () {
                            if (mMap.getAgsMap().graphics) {
                                mMap.getAgsMap().graphics.clear();
                            }
                            $("input[name=__submit_captcha]").val("")
                            $("#toaDoX").html("");
                            $("#toaDoY").html("");
                            $("#phanHoi").val("");
                            $('#captcha').click();
                        });
                    });//esri.toolbars.Draw.POLYGON
                    //mMap.startDraw(esri.toolbars.Draw.POLYGON, false, function (graphic) {
                    //    mMap.Intersect($('#commune').val(), graphic.geometry.getCentroid()).then((res) => {
                    //        if (res) {
                    //            $("#form-submit").show();
                    //            $("#form-accept").hide();
                    //            $('#form-delete').hide();


                    //            $("#toaDoX").html("Tọa độ X: " + graphic.geometry.getCentroid().x);
                    //            $("#toaDoY").html("Tọa độ Y: " + graphic.geometry.getCentroid().y);

                    //            $("#district_name").html("Quận/Huyện: " + res.DISTRICT);
                    //            $("#commune_name").html("Phường/Xã: " + res.NAME_VN);

                    //            $("#infoModal").modal();
                    //            $("#form-submit").unbind('click');

                    //            $("#form-submit").on('click', function (e) {
                    //                e.preventDefault();
                    //                App.startPageLoading({
                    //                    message: "Đang xử lý dữ liệu ..."
                    //                });
                    //                $.get("/api/fileupload/checkcaptcha?__submit_captcha=" + $("input[name=__submit_captcha]").val()).done(function (r) {
                    //                    if (!r) {
                    //                        App.stopPageLoading();
                    //                        $('#captcha').click();
                    //                        $("#error-captcha").show();
                    //                        return;
                    //                    }
                    //                    $("#error-captcha").hide();
                    //                    if ($("#loaiHinhViPham").val() == 0 || !$("#loaiDatViPham").val() || !$("#doiTuongViPham").val() || !$("#doiTuongPhatHien").val()) {
                    //                        App.stopPageLoading();
                    //                        bootbox.alert({
                    //                            message: "Vui lòng nhập đầy đủ thông tin!",
                    //                            buttons: {
                    //                                ok: {
                    //                                    label: "Đồng ý"
                    //                                }
                    //                            }
                    //                        });
                    //                        return;
                    //                    }
                    //                    else {
                    //                        if ($('input[name=file]')[0].files.length > 0) {
                    //                            $('#form-info').ajaxSubmit({
                    //                                success: function (xhr) {
                    //                                    if (xhr) {
                    //                                        graphic.setAttributes({
                    //                                            maXa: res.COMMUNEID,
                    //                                            maHuyen: res.PARENT_ID,
                    //                                            UserID: globs.user_id,
                    //                                            loaiHinhViPham: $("#loaiHinhViPham").val(),
                    //                                            loaiDatViPham: $("#loaiDatViPham").val(),
                    //                                            doiTuongViPham: $("#doiTuongViPham").val(),
                    //                                            doiTuongPhatHien: $("#doiTuongPhatHien").val(),
                    //                                            diaDiemViPham: $("#diaDiemViPham").val(),
                    //                                            thoiDiemViPham: new Date().toLocaleDateString("vi", { year: "numeric", day: "2-digit", month: "2-digit" }),
                    //                                            thongTinKhac: $("#thongTinKhac").val(),
                    //                                            toaDoX: graphic.geometry.getCentroid().x,
                    //                                            toaDoY: graphic.geometry.getCentroid().y,
                    //                                            duLieuDinhKem: xhr,

                    //                                        });
                    //                                        mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                    //                                            $("#form-submit").show();
                    //                                            $("#form-accept").show();
                    //                                            $('#form-delete').show();

                    //                                            mMap.getAgsMap().graphics.add(graphic);
                    //                                            $("#form-info").find("input[type=number]").val("");
                    //                                            $('span.fileinput-filename').text("")
                    //                                            $("#toaDoX").html("");
                    //                                            $("#toaDoY").html("");
                    //                                            $("#loaiHinhViPham").val(0);
                    //                                            $("#loaiDatViPham").val('NNP');
                    //                                            $("#doiTuongPhatHien").val("");
                    //                                            $("#doiTuongViPham").val("");
                    //                                            $("#diaDiemViPham").val("");
                    //                                            $("#thongTinKhac").val("");
                    //                                            $('#captcha').click();
                    //                                            $("#infoModal").modal('hide');

                    //                                            $.get("/api/fileUpload/sendEmail").done(function (xhr) {

                    //                                            });
                    //                                            App.stopPageLoading();
                    //                                            bootbox.alert({
                    //                                                message: 'Lưu thông tin thành công!',
                    //                                                buttons: {
                    //                                                    ok: {
                    //                                                        label: "Đồng ý"
                    //                                                    }
                    //                                                }
                    //                                            });
                    //                                        });
                    //                                    }
                    //                                }
                    //                            });
                    //                        }
                    //                        else {
                    //                            graphic.setAttributes({
                    //                                maXa: res.COMMUNEID,
                    //                                maHuyen: res.PARENT_ID,
                    //                                UserID: globs.user_id,
                    //                                loaiHinhViPham: $("#loaiHinhViPham").val(),
                    //                                loaiDatViPham: $("#loaiDatViPham").val(),
                    //                                doiTuongViPham: $("#doiTuongViPham").val(),
                    //                                doiTuongPhatHien: $("#doiTuongPhatHien").val(),
                    //                                diaDiemViPham: $("#diaDiemViPham").val(),
                    //                                thoiDiemViPham: new Date().toLocaleDateString("vi", { year: "numeric", day: "2-digit", month: "2-digit" }),
                    //                                thongTinKhac: $("#thongTinKhac").val(),
                    //                                toaDoX: graphic.geometry.getCentroid().x,
                    //                                toaDoY: graphic.geometry.getCentroid().y,
                    //                            });
                    //                            mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                    //                                $("#form-submit").show();
                    //                                $("#form-accept").show();
                    //                                $('#form-delete').show();

                    //                                mMap.getAgsMap().graphics.add(graphic);
                    //                                $("#form-info").find("input[type=number]").val("");
                    //                                $('span.fileinput-filename').text("")
                    //                                $("#toaDoX").html("");
                    //                                $("#toaDoY").html("");
                    //                                $("#loaiHinhViPham").val(0);
                    //                                $("#loaiDatViPham").val('NNP');
                    //                                $("#doiTuongPhatHien").val("");
                    //                                $("#doiTuongViPham").val("");
                    //                                $("#diaDiemViPham").val("");
                    //                                $("#thongTinKhac").val("");
                    //                                $('#captcha').click();
                    //                                $("#infoModal").modal('hide');
                    //                                $.get("/api/fileUpload/sendEmail").done(function (xhr) {

                    //                                });
                    //                                App.stopPageLoading();

                    //                                bootbox.alert({
                    //                                    message: 'Lưu thông tin thành công!',
                    //                                    buttons: {
                    //                                        ok: {
                    //                                            label: "Đồng ý"
                    //                                        }
                    //                                    }
                    //                                });
                    //                            });

                    //                        }
                    //                    }
                    //                });
                    //            });

                    //            $("#form-dismiss").on('click', function () {
                    //                mMap.getAgsMap().graphics.clear();
                    //                $("#form-info").find("input[type=number]").val("");
                    //                $('span.fileinput-filename').text("")
                    //                $("#toaDoX").html("");
                    //                $("#toaDoY").html("");
                    //                $("#loaiHinhViPham").val(0);
                    //                $("#loaiDatViPham").val('NNP');
                    //                $("#doiTuongPhatHien").val("");
                    //                $("#doiTuongViPham").val("");
                    //                $("#diaDiemViPham").val("");
                    //                $("#thongTinKhac").val("");
                    //                $('#captcha').click();

                    //                $("#form-submit").show();
                    //                $("#form-accept").show();
                    //                $('#form-delete').show();
                    //            });
                    //        } else {
                    //            bootbox.alert("Vui lòng tạo khu vực trong vùng thử nghiệm");
                    //            mMap.getAgsMap().graphics.clear();
                    //        }
                    //    });
                    //});
                    break;
                case MapControls.DRAW_POLYGON_PROTECT:
                    mMap.startDraw(esri.toolbars.Draw.POLYGON, false, function (graphic) {
                        mMap.Intersect($('#commune').val(), graphic.geometry.getCentroid()).then((res) => {
                            if (res) {
                                $("#form-submit").show();
                                $("#form-accept").hide();
                                $('#form-delete').hide();


                                $("#district_name").html("Quận/Huyện: " + res.DISTRICT);
                                $("#commune_name").html("Phường/Xã: " + res.NAME_VN);
                                $("#toaDoX").html("Tọa độ X: " + graphic.geometry.getCentroid().x);
                                $("#toaDoY").html("Tọa độ Y: " + graphic.geometry.getCentroid().y);
                                $("#infoModal").modal();
                                $("#form-submit").unbind('click');
                                $("#form-submit").on('click', function (e) {
                                    App.startPageLoading({
                                        message: "Đang xử lý dữ liệu ..."
                                    });
                                    e.preventDefault();
                                    $.get("/api/fileupload/checkcaptcha?__submit_captcha=" + $("input[name=__submit_captcha]").val()).done(function (r) {
                                        if (!r) {
                                            App.stopPageLoading();
                                            $('#captcha').click();
                                            $("#error-captcha").show();
                                            return;
                                        }
                                        $("#error-captcha").hide();
                                        if (!$("#loaiGiamSat").val()) {
                                            App.stopPageLoading();
                                            bootbox.alert({
                                                message: "Vui lòng nhập đầy đủ thông tin!",
                                                buttons: {
                                                    ok: {
                                                        label: "Đồng ý"
                                                    }
                                                }
                                            });
                                        }
                                        else {
                                            if ($('input[name=file]')[0].files.length > 0) {
                                                $('#form-info').ajaxSubmit({
                                                    success: function (xhr) {
                                                        graphic.setAttributes({
                                                            maXa: res.COMMUNEID,
                                                            maHuyen: res.PARENT_ID,
                                                            UserID: globs.user_id,
                                                            loaiGiamSat: $("#loaiGiamSat").val(),
                                                            diaDiemPhatHien: $("#diaDiemPhatHien").val(),
                                                            thoiDiemPhatHien: new Date().toLocaleDateString("vi", { year: "numeric", day: "2-digit", month: "2-digit" }),
                                                            thongTinKhac: $("#thongTinKhac").val(),
                                                            toaDoX: graphic.geometry.getCentroid().x,
                                                            toaDoY: graphic.geometry.getCentroid().y,
                                                            duLieuDinhKem: xhr
                                                        });
                                                        mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                                                            $("#form-submit").show();
                                                            $("#form-accept").show();
                                                            $('#form-delete').show();

                                                            mMap.getAgsMap().graphics.add(graphic);
                                                            $("#toaDoX").html("");
                                                            $("#toaDoY").html("");
                                                            $("#loaiGiamSat").val(0);
                                                            $("#diaDiemPhatHien").val("");
                                                            $("#thongTinKhac").val("");
                                                            $("#infoModal").modal('hide');
                                                            $('#captcha').click();
                                                            $("#form-info").find("input[type=number]").val("");
                                                            $('span.fileinput-filename').text("")

                                                            $.get("/api/fileUpload/sendEmail").done(function (xhr) {

                                                            });
                                                            App.stopPageLoading();
                                                            bootbox.alert({
                                                                message: 'Lưu thông tin thành công!',
                                                                buttons: {
                                                                    ok: {
                                                                        label: "Đồng ý"
                                                                    }
                                                                }
                                                            });
                                                        });
                                                    }
                                                });
                                            } else {
                                                graphic.setAttributes({
                                                    maXa: res.COMMUNEID,
                                                    maHuyen: res.PARENT_ID,
                                                    UserID: globs.user_id,
                                                    loaiGiamSat: $("#loaiGiamSat").val(),
                                                    diaDiemPhatHien: $("#diaDiemPhatHien").val(),
                                                    thoiDiemPhatHien: new Date().toLocaleDateString("vi", { year: "numeric", day: "2-digit", month: "2-digit" }),
                                                    thongTinKhac: $("#thongTinKhac").val(),
                                                    toaDoX: graphic.geometry.getCentroid().x,
                                                    toaDoY: graphic.geometry.getCentroid().y,
                                                });
                                                mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                                                    $("#form-submit").show();
                                                    $("#form-accept").show();
                                                    $('#form-delete').show();


                                                    mMap.getAgsMap().graphics.add(graphic);
                                                    $("#toaDoX").html("");
                                                    $("#toaDoY").html("");
                                                    $("#loaiGiamSat").val(0)
                                                    $("#diaDiemPhatHien").val("");
                                                    $("#thongTinKhac").val("");
                                                    $("#infoModal").modal('hide');
                                                    $("#form-info").find("input[type=number]").val("");
                                                    $('span.fileinput-filename').text("")
                                                    $('#captcha').click();

                                                    $.get("/api/fileUpload/sendEmail").done(function (xhr) {

                                                    });
                                                    App.stopPageLoading();
                                                    bootbox.alert({
                                                        message: 'Lưu thông tin thành công!',
                                                        buttons: {
                                                            ok: {
                                                                label: "Đồng ý"
                                                            }
                                                        }
                                                    });
                                                });
                                            }
                                        }
                                    });
                                });
                                $("#form-dismiss").on('click', function () {
                                    mMap.getAgsMap().graphics.clear();
                                    $("#form-info").find("input[type=number]").val("");
                                    $('span.fileinput-filename').text("")
                                    $("#toaDoX").html("");
                                    $("#toaDoY").html("");
                                    $("#loaiGiamSat").val(0);
                                    $("#diaDiemPhatHien").val("");
                                    $("#thongTinKhac").val("");
                                    $('#captcha').click();

                                    $("#form-submit").show();
                                    $("#form-accept").show();
                                    $('#form-delete').show();
                                });
                            } else {
                                bootbox.alert("Vui lòng tạo khu vực trong vùng thử nghiệm");
                                mMap.getAgsMap().graphics.clear();
                            }
                        });
                    });
                    break;
                case MapControls.DRAW_POLYGON_ASSETS:
                    mMap.startDraw(esri.toolbars.Draw.POLYGON, false, function (graphic) {
                        mMap.Intersect($('#commune').val(), graphic.geometry.getCentroid()).then((res, reject) => {
                            if (res) {
                                $("#form-submit").show();
                                $("#form-accept").hide();
                                $('#form-delete').hide();

                                $("#district_name").html("Quận/Huyện: " + res.DISTRICT);
                                $("#commune_name").html("Phường/Xã: " + res.NAME_VN);


                                $("#toaDoX").html("Tọa độ X: " + graphic.geometry.getCentroid().x);
                                $("#toaDoY").html("Tọa độ Y: " + graphic.geometry.getCentroid().y);
                                $("#infoModal").modal();
                                $("#form-submit").unbind('click');
                                $("#form-submit").on('click', function (e) {
                                    App.startPageLoading({
                                        message: "Đang xử lý dữ liệu ..."
                                    });
                                    e.preventDefault();
                                    $.get("/api/fileupload/checkcaptcha?__submit_captcha=" + $("input[name=__submit_captcha]").val()).done(function (r) {
                                        if (!r) {
                                            App.stopPageLoading();
                                            $('#captcha').click();
                                            $("#error-captcha").show();
                                            return;
                                        }
                                        $("#error-captcha").hide();
                                        if ($("#loaiTaiSanGanLienVoiDat").val() == 0 || !$("#tenTaiSan").val() || !$("#diaDiemBienDong").val()) {
                                            App.stopPageLoading();
                                            bootbox.alert({
                                                message: "Vui lòng nhập đầy đủ thông tin!",
                                                buttons: {
                                                    ok: {
                                                        label: "Đồng ý"
                                                    }
                                                }
                                            });
                                        }
                                        else {
                                            if ($('input[name=file]')[0].files.length > 0) {
                                                $('#form-info').ajaxSubmit({
                                                    success: function (xhr) {
                                                        graphic.setAttributes({
                                                            maXa: res.COMMUNEID,
                                                            maHuyen: res.PARENT_ID,
                                                            UserID: globs.user_id,
                                                            loaiTaiSanGanLienVoiDat: $("#loaiTaiSanGanLienVoiDat").val(),
                                                            tenTaiSan: $("#tenTaiSan").val(),
                                                            doiTuongPhatHien: $("#doiTuongPhatHien").val(),
                                                            diaDiemBienDong: $("#diaDiemBienDong").val(),
                                                            thoiDiemBienDong: new Date().toLocaleDateString("vi", { year: "numeric", day: "2-digit", month: "2-digit" }),
                                                            toaDoX: graphic.geometry.getCentroid().x,
                                                            toaDoY: graphic.geometry.getCentroid().y,
                                                            thongTinKhac: $("#thongTinKhac").val(),
                                                            duLieuDinhKem: xhr
                                                        });
                                                        mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                                                            $("#form-submit").show();
                                                            $("#form-accept").show();
                                                            $('#form-delete').show();


                                                            mMap.getAgsMap().graphics.add(graphic);
                                                            $("#form-info").find("input[type=number]").val("");
                                                            $('span.fileinput-filename').text("")
                                                            $("#toaDoX").html("");
                                                            $("#toaDoY").html("");
                                                            $("#loaiTaiSanGanLienVoiDat").val(0);
                                                            $("#tenTaiSan").val("");
                                                            $("#doiTuongPhatHien").val("");
                                                            $("#diaDiemBienDong").val("");
                                                            $("#thongTinKhac").val("");
                                                            $("#infoModal").modal('hide');
                                                            $('#captcha').click();
                                                            $.get("/api/fileUpload/sendEmail").done(function (xhr) {

                                                            });
                                                            App.stopPageLoading();
                                                            bootbox.alert({
                                                                message: 'Lưu thông tin thành công!',
                                                                buttons: {
                                                                    ok: {
                                                                        label: "Đồng ý"
                                                                    }
                                                                }
                                                            });
                                                        });
                                                    }
                                                });
                                            } else {
                                                graphic.setAttributes({
                                                    maXa: res.COMMUNEID,
                                                    maHuyen: res.PARENT_ID,
                                                    UserID: globs.user_id,
                                                    loaiTaiSanGanLienVoiDat: $("#loaiTaiSanGanLienVoiDat").val(),
                                                    tenTaiSan: $("#tenTaiSan").val(),
                                                    doiTuongPhatHien: $("#doiTuongPhatHien").val(),
                                                    diaDiemBienDong: $("#diaDiemBienDong").val(),
                                                    thoiDiemBienDong: new Date().toLocaleDateString("vi", { year: "numeric", day: "2-digit", month: "2-digit" }),
                                                    toaDoX: graphic.geometry.getCentroid().x,
                                                    toaDoY: graphic.geometry.getCentroid().y,
                                                    thongTinKhac: $("#thongTinKhac").val(),
                                                });
                                                mMap.getAgsMap().getLayer('doiTuongVung').applyEdits([graphic], null, null, function () {
                                                    $("#form-submit").show();
                                                    $("#form-accept").show();
                                                    $('#form-delete').show();


                                                    mMap.getAgsMap().graphics.add(graphic);
                                                    $("#form-info").find("input[type=number]").val("");
                                                    $('span.fileinput-filename').text("")
                                                    $("#toaDoX").html("");
                                                    $("#toaDoY").html("");
                                                    $("#loaiTaiSanGanLienVoiDat").val(0);
                                                    $("#tenTaiSan").val("");
                                                    $("#doiTuongPhatHien").val("");
                                                    $("#diaDiemBienDong").val("");
                                                    $("#thongTinKhac").val("");
                                                    $("#infoModal").modal('hide');
                                                    $('#captcha').click();
                                                    $.get("/api/fileUpload/sendEmail").done(function (xhr) {

                                                    });
                                                    App.stopPageLoading();

                                                    bootbox.alert({
                                                        message: 'Lưu thông tin thành công!',
                                                        buttons: {
                                                            ok: {
                                                                label: "Đồng ý"
                                                            }
                                                        }
                                                    });
                                                });
                                            }
                                        }

                                    });
                                });
                                $("#form-dismiss").on('click', function () {
                                    $("#form-submit").show();
                                    $("#form-accept").show();
                                    $('#form-delete').show();

                                    mMap.getAgsMap().graphics.clear();
                                    $("#form-info").find("input[type=number]").val("");
                                    $('span.fileinput-filename').text("")
                                    $("#toaDoX").html("");
                                    $("#toaDoY").html("");
                                    $("#loaiTaiSanGanLienVoiDat").val(0);
                                    $("#tenTaiSan").val("");
                                    $("#doiTuongPhatHien").val("");
                                    $("#diaChiCungCap").val("");
                                    $("#thongTinKhac").val("");
                                    $('#captcha').click();
                                });
                            } else {
                                bootbox.alert("Vui lòng tạo khu vực trong vùng thử nghiệm");
                                mMap.getAgsMap().graphics.clear();
                            }
                        });
                    });
                    break;
                case MapControls.FILTER:
                    $('#dv-filter').css('display', 'block');
                    $('#legend').css('display', 'block');
                    break;
                case MapControls.REPORT:
                    $('#dv-report').css('display', 'block');
                    break;
                case MapControls.SHOW_CHART:
                    $('#controls').css('display', 'block');
                    break;
                default:
                    break;
            }
            instance.change.call(this, {
                lastTool: MapControls.config[lastTool],
                newTool: MapControls.config[tool]
            });
            $deferred.resolve(instance._active);
        } else if (!data.toggle) {
            switch (data.name) {
                case MapControls.REFRESH:
                    // mMap.clear();
                    break;
                case MapControls.FULL_EXTENT:
                    //$("#map").data('vgsMapView').loadBoundary(22);
                    mMap.viewFullExtent();
                    break;
                case MapControls.MEASUREMENT_LENGTH:
                case MapControls.MEASUREMENT_AREA:
                    console.log('new');
                    mMap.getMeasurement().deactivate();
                    break;

                case MapControls.NEXT_EXTENT:
                    //   mMap.nextExtent();
                    break;
                case MapControls.BACK_EXTENT:
                    //  mMap.previousExtent();
                    break;
                case MapControls.FILTER:
                    $('#dv-filter').css('display', 'none');
                    $('#legend').css('display', 'block');
                    break;
                case MapControls.SHOW_CHART:
                    $('#controls').css('display', 'block');
                    break;

                default:
                    break;
            }
            $deferred.reject();
        } else
            $deferred.reject();
        return $deferred.promise();
    };
    MapControls.prototype.deactivate = function ($deferred) {
        $deferred = $deferred || $.Deferred();

        var instance = this;
        $.each(this._listData, function (tool, node) {
            $(node).parent().removeClass('active');
        });
        this._active = null;
        //
        return $deferred.promise();
    };
    MapControls.prototype.hide = function () {
        $(this.element).hide();
    };
    MapControls.prototype.show = function () {
        $(this.element).show();
    };
    MapControls.prototype.hasControl = function (name) {
        return $.grep(this.options.controls, function (ctr) {
            return ctr.name === name || ctr === name;
        }).length > 0;
    };
    MapControls.prototype.disable = function () {
        $(this.element).addClass('k-state-disabled');
    };
    MapControls.prototype.enable = function () {
        $(this.element).removeClass('k-state-disabled');
    };
    $.extend($.fn, {
        vgsMapToolBar: function (opt) {
            return new MapControls(this, opt);
        }
    });

}(jQuery, window.vgsMap));