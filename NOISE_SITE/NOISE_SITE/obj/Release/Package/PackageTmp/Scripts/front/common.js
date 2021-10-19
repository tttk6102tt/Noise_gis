var App = function () {

    var _sessionVars, _vars;

    // Handle Tooltip
    function handleToolTip() {
        //$('[data-toggle="tooltip"]').tooltip();
        //$('.tooltip').filter(function (idx) {
        //    return $(this).data('tooltipsterNs') === undefined;
        //}).tooltipster({
        //    side: 'top',
        //    animation: 'fade',
        //    theme: 'tooltipster-borderless'
        //});
        //$(document).on('appear', '.tooltip', function (e) {
        //    console.log(e);
        //});
        $('body').on('mouseenter', '.tooltip:not(.tooltipstered)', function () {
            if ($(this).data('tooltipsterNs') === undefined) {
                $(this).tooltipster({
                    side: 'top',
                    animation: 'fade',
                    theme: 'tooltipster-light'
                }).tooltipster('open');
            }

        });
    };
    // handleNavResponsive 
    var handleNavResponsive = function () {
        $('.menulink .menu__title').click(function (e) {
            $('.menulink .menu__title').removeClass('menu--expanded');
            if ($('.navbar-hor .navbar-nav').is(':visible')) {
                $('.navbar-hor .navbar-nav').fadeOut(350);
            } else {
                $('.menulink .menu__title').addClass('menu--expanded');
                $('.navbar-hor .navbar-nav').slideDown(250);
            }
            e.preventDefault();
        });
    };
    // Handle Click Slide Left 
    var handleSlideLeft = function () {
        $('.slide-left').click(function (e) {
            //
            //$('.navbar-hor .navbar-nav').fadeOut(700);
            $('.navbar-hor .menulink').removeClass('menu--expanded');
            //$('.page-search-result').delay(5000).addClass('is-expand');
            $('.page-sidebar').delay(2000).toggleClass('is-expand');
            //
            handleScaleBarPosition();
            //
            handleOverViewMapPosition();
            //
            e.preventDefault();
        });
    };
    // Handle sidebar menu
    var handleSidebarMenu = function () {
        jQuery('.js-page-sidebar').on('click', 'li > a', function (e) {
            if (!$(this).next().hasClass('sub-menu')) {
                if (!$('.btn-navbar').hasClass('collapsed')) {
                    $('.btn-navbar').click();
                }
                return;
            }

            var hgtWindow = $(window).height();
            var hgtHeader = $('.header').height();
            var hgtFooter = $('.footer').outerHeight();
            var hgtNavigation = $('#navigation').height();
            var distanceTop = hgtHeader + hgtNavigation + hgtFooter;

            if ($('body').hasClass('format-top')) {
                if ($(this).parent().find('.search-action').length > 0 || $(this).parent().find('.geometry-grid-container').length > 0) {
                    var minus = 0;
                    if ($(this).parent().find('.search-action').length > 0)
                        minus += 61;
                    if ($(this).parent().find('.geometry-grid-container').length > 0)
                        minus += 201;
                    $('#scroll-content').css('height', hgtWindow - distanceTop - minus + 'px');
                } else
                    $('#scroll-content').css('height', hgtWindow - distanceTop + 'px');
            }

            if ($(this).next().hasClass('sub-menu always-open')) {
                return;
            }
            var iscurrent = $(this).parent().hasClass('active');
            var parent = $(this).parent().parent();
            var the = $(this);
            var menu = $('.js-page-sidebar-menu');
            var sub = jQuery(this).next();

            var slideSpeed = menu.data("slide-speed") ? parseInt(menu.data("slide-speed")) : 200;

            parent.children('li.is-open').children('a').children('.arrow').removeClass('is-open');
            parent.children('li.is-open').children('.sub-menu:not(.always-open)').slideUp(200);
            parent.children('li.is-open').removeClass('is-open');

            var slideOffeset = -200;

            if (sub.is(":visible")) {
                jQuery('.arrow', jQuery(this)).removeClass("is-open");
                jQuery(this).parent().removeClass("is-open");
                sub.slideUp(slideSpeed);
            } else {
                jQuery('.arrow', jQuery(this)).addClass("is-open");
                jQuery(this).parent().addClass("is-open");
                sub.slideDown(slideSpeed);
            }

            e.preventDefault();
        });
    };

    var handleShowSideBar = function (ele) {
        $('.js-page-sidebar').find('li.preroll').each(function () {
            var sub = $(this).children('a').next();
            var slideSpeed = 200;
            if ($(this).is($(ele))) {
                //
                var hgtWindow = $(window).height();
                var hgtHeader = $('.header').height();
                var hgtFooter = $('.footer').outerHeight();
                var hgtNavigation = $('#navigation').height();
                var distanceTop = hgtHeader + hgtNavigation + hgtFooter;
                //
                if ($(this).parent().find('.search-action').length > 0 || $(this).parent().find('.geometry-grid-container').length > 0) {
                    var minus = 0;
                    if ($(this).parent().find('.search-action').length > 0)
                        minus += 61;
                    if ($(this).parent().find('.geometry-grid-container').length > 0)
                        minus += 201;
                    $('#scroll-content').css('height', hgtWindow - distanceTop - minus + 'px');
                } else
                    $('#scroll-content').css('height', hgtWindow - distanceTop + 'px');

                $(this).addClass('is-open');
                $(this).show();
                if (!sub.is(":visible")) {
                    jQuery('.arrow', jQuery(this)).addClass("is-open");
                    jQuery(this).parent().addClass("is-open");
                    sub.slideDown(slideSpeed);
                }
            } else {
                $(this).removeClass('is-open');
                if (sub.is(":visible")) {
                    jQuery('.arrow', jQuery(this)).removeClass("is-open");
                    jQuery(this).parent().removeClass("is-open");
                    sub.slideUp(slideSpeed);
                }
            }
        });
    };

    var handleHideSideBar = function (ele) {
        var $ele = $('.js-page-sidebar').find(ele);
        if ($ele.length > 0) {
            $('.js-page-sidebar > li.preroll').removeClass('is-open');
            $ele.removeClass('is-open');
            $ele.hide();
            var slideSpeed = 200;
            var sub = $ele.children('a').next();
            if (sub.is(":visible")) {
                jQuery('.arrow', jQuery(this)).removeClass("is-open");
                jQuery(this).parent().removeClass("is-open");
                sub.slideUp(slideSpeed);
            }
        }
    };

    // Handle Scrollbar
    function handleScrollbar() {
        $("#scroll-content").mCustomScrollbar({
            theme: "minimal-dark",
            advanced: {
                updateOnBrowserResize: true,
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
        });
        $(".panel-body").mCustomScrollbar({
            theme: "minimal-dark",
            advanced: {
                updateOnBrowserResize: true,
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
        });
        $(".redact-type").mCustomScrollbar({
            theme: "minimal-dark",
            setHeight: 500,
            advanced: {
                updateOnBrowserResize: true,
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
        });
        $(".js-usage-cnt").mCustomScrollbar({
            theme: "minimal-dark",
            setHeight: 500,
            advanced: {
                updateOnBrowserResize: true,
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
        });
        /*$(".scroll-table").mCustomScrollbar({
          theme: "minimal-dark",
          setHeight: 500,
          advanced: {
            updateOnBrowserResize: true,
            updateOnContentResize: true
          }
        });*/
    };
    // Handle OnResize
    var handleOnResizeScrollbar = function () {
        $(window).resize(function () {
            handleScrollbar();
        }).load(function () {
            handleScrollbar();
        });
    };
    // handleRedactData
    var handleRedactData = function () {
        //$('.frame-redact .redact-close').click(function (e) {
        //    if (!$(this).closest('.frame-redact').is(':visible')) {
        //        $(this).closest('.frame-redact').slideUp(500);
        //    }
        //    e.preventDefault();
        //});
    };
    // Handle Height Element
    var handleHeightElement = function () {
        var hgtWindow = $(window).height();
        var hgtHeader = $('.header').height();
        var hgtFooter = $('.footer').outerHeight();
        var hgtNavigation = $('#navigation').height();
        var distanceTop = hgtHeader + hgtNavigation + hgtFooter;
        var restInt = hgtWindow - distanceTop;
        var rest = restInt + 'px';
        var hgtPanelHead = $('.page-search-result .panel .panel-heading').outerHeight();
        var hgtPanelFooter = $('.page-search-result .panel .panel-back').outerHeight();
        var hgtPanelBody = hgtWindow - distanceTop - hgtPanelHead - hgtPanelFooter;
        $('.page-sidebar').height(restInt);
        $('#scroll-content').css('height', rest);
        $('.page-search-result').css('height', hgtPanelBody);
        $('.page-search-result .panel .panel-body').css('height', hgtPanelBody);
        $('.page-content').css('height', rest);
        $('.page-content .iframe-map').css('height', rest);
        $('.page-content #map').css('height', hgtWindow - distanceTop + 'px');
        //$('.page-sidebar-menu > li').each(function () {
        //    var $li = $(this),
        //        $ul = $li.children('ul.sub-menu'),
        //        liHeight = $li.outerHeight();
        //    $ul.height(restInt - ($('.page-sidebar-menu > li').length) * liHeight - 8);
        //});
    };
    // Handle Height Element
    var handleChildHeightElement = function () {
        var hgtWindow = $(window).height();
        var hgtHeader = $('.header').height();
        var hgtFooter = $('.footer').outerHeight();
        var hgtNavigation = $('#navigation').height();
        var distanceTop = hgtHeader + hgtNavigation + hgtFooter;
        var restInt = hgtWindow - distanceTop;
        var rest = restInt + 'px';
        var hgtPanelHead = $('.page-search-result .panel .panel-heading').outerHeight();
        var hgtPanelFooter = $('.page-search-result .panel .panel-back').outerHeight();
        var hgtPanelBody = hgtWindow - distanceTop - hgtPanelHead - hgtPanelFooter;
        $('.page-sidebar').height(restInt - 40);
        $('.page-content').height(restInt + 2);
        $('.page-content .page-inner').height(restInt - 38);
        $('.page-content .element-child').height(restInt - 38);
        //$('.page-sidebar-menu > li').each(function () {
        //    var $li = $(this),
        //        $ul = $li.children('ul.sub-menu'),
        //        liHeight = $li.outerHeight();
        //    $ul.height(restInt - ($('.page-sidebar-menu > li').length) * liHeight - 8);
        //});
    };
    // Handle OnResize
    var handleOnResizeHeightElement = function () {
        $(window).resize(function () {
            handleHeightElement();
        }).load(function () {
            handleHeightElement();
        });
    };
    // only top
    var handleOnlyTop = function () {
        var topFlg = $('body').hasClass('format-top');
        if (topFlg) {
            handleHeightElement();
            // handleOnResizeHeightElement();
            $('.frame-redact').draggable({
                scroll: false,
                iframeFix: true,
                cursor: 'move',
                handle: '.redact-title'
            });
        }
    };

    var handleOnlyChild = function () {
        var childFlg = $('body').hasClass('format-child');
        if (childFlg) {
            handleChildHeightElement();
        }
    };

    var handleGetAjaxToken = function () {
        var form = $('#__AjaxAntiForgeryForm');
        if (form) {
            var token = $('input[name="__RequestVerificationToken"]', form).val();
            if (token) {
                return token;
            }
        }
    };

    var handleScaleBarPosition = function () {
        var ele = '.scalebar_bottom-left.esriScalebar',
            $ele = $(ele),
            $pageSidebar = $('.page-sidebar');
        $pageSidebar.on("transitionend webkitTransitionEnd oTransitionEnd MSTransitionEnd", function () {
            if ($ele.length > 0)
                $ele.css('cssText', 'left: ' + ($pageSidebar.outerWidth() + $pageSidebar.position().left + 10) + 'px !important');
        });
    };

    var handleOverViewMapPosition = function () {
        var ele = '.esriOverviewMap.ovwBL',
            $ele = $(ele),
            $pageSidebar = $('.page-sidebar');
        if ($pageSidebar.length !== 0) {
            $pageSidebar.on("transitionend webkitTransitionEnd oTransitionEnd MSTransitionEnd", function () {
                if ($ele.length > 0)
                    $ele.css('cssText', 'left: ' + ($pageSidebar.outerWidth() + $pageSidebar.position().left + 10) + 'px !important');
            });
        } else {
            $ele.css('cssText', 'left: ' + (10) + 'px !important');
        }
    };

    var handleUser = function () {
        var $changePasswdForm = $('#changePasswdForm');
        var $userInfoForm = $('#userInfoForm');
        $changePasswdForm.on('submit', function (e) {
            e.preventDefault();
            $(this).ajaxSubmit({
                success: function (xhr) {
                    if (xhr.Succeeded) {
                        $.kendoAlert('Đổi mật khẩu thành công');
                        $changePasswdForm.closest('.modal').modal('hide');
                        $changePasswdForm[0].reset();
                    } else {
                        $.kendoAlert(xhr.Errors ? xhr.Errors[0].Description : "Lỗi không xác định");
                    }
                }
            });
        });
        $userInfoForm.on('submit', function (e) {
            e.preventDefault();
            $(this).ajaxSubmit({
                success: function (xhr) {
                    if (xhr.Succeeded) {
                        $.kendoAlert('Cập nhật thông tin thành công');
                        $userInfoForm.closest('.modal').modal('hide');
                        $userInfoForm[0].reset();
                        App.getOA().signinSilent().then(function (user) {
                            console.log("Renewed", user);
                            kendo.bind($userInfoForm, user.profile);
                        }).catch(function (err) {
                            console.log(err);
                            App.getOA().getUser().then(function (user) {
                                if (user) {
                                    kendo.bind($userInfoForm, user.profile);
                                }
                            });
                        });
                    } else {
                        $.kendoAlert(xhr.Errors ? xhr.Errors[0].Description : "Lỗi không xác định");
                    }
                }
            });
        });
        App.getOA().getUser().then(function (user) {
            if (user) {
                kendo.bind($userInfoForm, user.profile);
            }
        });
    };

    return {
        init: function () {
            App.getOA().getUser().then(function (user) {
                if (user) {
                    localStorage.setItem("user", user.toStorageString());
                }
            });
            $.ajaxSetup({
                beforeSend: function (xhr) {
                    var user = JSON.parse(localStorage.getItem('user'));
                    if (user)
                        xhr.setRequestHeader("Authorization", user.token_type + " " + user.access_token);
                },
                error: function (xhr, status, error) {
                    if (status === 401) {
                        App.getOA().signInRedirect();
                    }
                }
            });
            handleToolTip();
            handleNavResponsive();
            handleSlideLeft();
            handleSidebarMenu();
            handleScrollbar();
            handleOnResizeScrollbar();
            handleRedactData();
            handleOnlyTop();
            handleOnlyChild();
            handleUser();
            kendo.ui.progress.messages = {
                loading: "Đang tải, vui lòng chờ..."
            };
            $(document).ajaxStart(function () {
                kendo.ui.progress($('body'), true);
            });
            $(document).ajaxComplete(function () {
                kendo.ui.progress($('body'), false);
            });
            $('a.signout-link').on('click', function (e) {
                $.kendoConfirm('Bạn có muốn thoát khỏi phiên đăng nhập không?').done(function () {
                    window.location = '/Home/Logout'
                });
            });
            $('a.home-link').on('click', function (e) {
                $.kendoConfirm('Bạn có muốn thoát khỏi phân hệ này để quay về trang chủ không?').done(function () {
                    window.location = '/';
                });
            });
            //$('.form-group.search-action > button').bind('click', function (e) {
            //    e.preventDefault();
            //    e.stopImmediatePropagation();
            //    e.stopPropagation();
            //});
        },

        getAjaxToken: function () {
            return handleGetAjaxToken();
        },

        getPageContentHeight: function () {
            return $('.page-content').outerHeight();
        },

        ajaxPostWithToken: function (url, data, successCallback, errorCallback, completeCb) {
            if (!data["__RequestVerificationToken"])
                data["__RequestVerificationToken"] = handleGetAjaxToken();
            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                beforeSend: function () {
                    App.blockUI({
                        message: 'Đang xử lý'
                    })
                },
                success: successCallback ? successCallback : function () { },
                error: errorCallback ? errorCallback : function () { },
                complete: completeCb ? completeCb : function () { }
            });
        },

        ajaxPostNoBlockWithToken: function (url, data, successCallback, errorCallback, completeCb) {
            if (!data)
                data = {};
            if (!data["__RequestVerificationToken"])
                data["__RequestVerificationToken"] = handleGetAjaxToken();
            $.ajax({
                url: url,
                type: 'POST',
                data: data,
                success: successCallback ? successCallback : function () { },
                error: errorCallback ? errorCallback : function () { },
                complete: completeCb ? completeCb : function () { }
            });
        },

        ajaxPostJsonNoBlock: function (url, data, successCallback, errorCallback, completeCb) {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: successCallback ? successCallback : function () { },
                error: errorCallback ? errorCallback : function () { },
                complete: completeCb ? completeCb : function () { }
            });
        },

        ajaxGetWithToken: function (url, data, successCallback, errorCallback) {
            if (!data["__RequestVerificationToken"])
                data["__RequestVerificationToken"] = handleGetAjaxToken();
            $.ajax({
                url: url,
                type: 'GET',
                data: data,
                success: successCallback ? successCallback : function () { },
                error: errorCallback ? errorCallback : function () { }
            });
            return true;
        },

        ajaxGetDownload: function (url, data, contentType, fileName) {
            var user = JSON.parse(localStorage.getItem('user'));
            var xhr = new XMLHttpRequest();
            xhr.open('GET', url, true);
            xhr.responseType = 'blob';
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            xhr.setRequestHeader('Authorization', user.token_type + " " + user.access_token);
            xhr.responseType = 'blob';
            xhr.onloadstart = function () {
                kendo.ui.progress($('body'), true);
            };
            xhr.onload = function (e) {
                if (this.status === 200) {
                    var blob = xhr.response;
                    var disposition = xhr.getResponseHeader('Content-Disposition');
                    var reg = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
                    var a = document.createElement("a");
                    a.href = window.URL.createObjectURL(blob);
                    a.download = reg.exec(disposition)[1] || "";
                    a.click();
                    a.remove();
                } else {
                    $.kendoAlert('Không thể tạo đường dẫn tới tệp tin!');
                }
            };
            xhr.onloadend = function () {
                kendo.ui.progress($('body'), false);
            }
            xhr.send(data);
        },

        ajaxDownload: function (url, data, contentType, fileName, rContentType) {
            var user = JSON.parse(localStorage.getItem('user'));
            var xhr = new XMLHttpRequest();
            xhr.open('POST', url, true);
            xhr.responseType = 'blob';
            xhr.setRequestHeader('Content-type', rContentType || 'application/json; charset=utf-8');
            xhr.setRequestHeader('Authorization', user.token_type + " " + user.access_token);
            xhr.onloadstart = function () {
                kendo.ui.progress($('body'), true);
            };
            xhr.onload = function (e) {
                if (this.status === 200) {
                    var blob = new Blob([this.response], {
                        type: contentType
                    });
                    var downloadUrl = URL.createObjectURL(blob);
                    var a = document.createElement("a");
                    a.href = downloadUrl;
                    a.download = fileName;
                    document.body.appendChild(a);
                    a.click();
                    a.remove();
                } else {
                    $.kendoAlert('Không thể tạo đường dẫn tới tệp tin!');
                }
            };
            xhr.onloadend = function () {
                kendo.ui.progress($('body'), false);
            }
            xhr.send(JSON.stringify(data));
        },

        showSideBar: function (ele) {
            handleShowSideBar(ele);
        },

        hideSideBar: function (ele) {
            handleHideSideBar(ele);
        },

        getAPI: function (Action) {
            return '/' + App.getSessionVar('Controller') + '/' + Action;
        },

        calScaleBarPosition: function () {
            handleScaleBarPosition();
        },

        calOverViewMapPosition: function () {
            handleOverViewMapPosition();
        },

        reTooltip: function () {
            handleToolTip();
        },

        getSessionVar: function (name) {
            try {
                if (isNullOrEmpty(_sessionVars))
                    _sessionVars = JSON.parse(atob(Cookies.get('SessionVars')));
                return _sessionVars[name];
            } catch (e) {
                return undefined;
            }
        },

        getVar: function (name) {
            try {
                if (isNullOrEmpty(_vars))
                    _vars = JSON.parse(atob(Cookies.get('__Vars')));
                return _vars[name];
            } catch (e) {
                return undefined;
            }
        },

        isSA: function () {
            return App.getVar('isSA');
        },

        isAdmin: function () {
            return App.getVar('isAdmin');
        },

        isEditor: function () {
            return App.getVar('isEditor');
        },

        isUser: function () {
            return App.getVar('isUser');
        },

        download: function (url, filename) {
            var pom = document.createElement('a');
            pom.setAttribute('href', url);
            pom.setAttribute('target', '_blank');
            if ($.isNullOrEmpty(filename) === false)
                pom.setAttribute('download', filename);

            if (document.createEvent) {
                var event = document.createEvent('MouseEvents');
                event.initEvent('click', true, true);
                pom.dispatchEvent(event);
            } else {
                pom.click();
            }
        },

        getMapPadding: function () {
            var paddingLeft = parseInt($('.page-sidebar').offset().left + $('.page-sidebar').width());
            return [0, 0, 0, paddingLeft];
        },

        getOA: function () {
            return new Oidc.UserManager({
                authority: "/",
                client_id: "htgt.bn",
                redirect_uri: window.location.origin + "/account/loggedin",
                silent_redirect_uri: window.location.origin + "/account/loggedin",
                response_type: "id_token token",
                scope: "openid profile htgt",
                post_logout_redirect_uri: "/",
            });
        },

        getDPI: function () {
            return 96 * window.devicePixelRatio;
        }

    };

}();

$(document).ready(function () {
    App.init();
});