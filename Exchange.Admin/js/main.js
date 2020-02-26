(function ($) {
    "use strict";
    var w = $(window),
        doc = $(document),
        windowWidth = w.width(),
        nav = $(".dima-nav").outerHeight(),
        windowHeight = w.height(),
        tw = $("#tweet"),
        prefix, isTouch = Modernizr.touch,
        isTransitions = Modernizr.csstransitions,
        isFirefox = typeof InstallTrigger !== 'undefined',
        isChrome = !!window.chrome,
        isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0;
    window.App = {};
    doc.ready(function (App) {
        this.App = App || {};
        App.NAV = function () {
            var myMenu = function () {
                submenu();
                mobileNav();
                fixNav();
                MobilesubMenu();
                searchBox();
                onePage();
                inline_cenet_logo();
            };
            var submenu = function () {
                var desk_nav_menu = $('.desk-nav ul.dima-nav,.desk-nav ul.dima-nav-end');
                var desk_childe_li = 'li.menu-item-has-children';
                var desk_childe_ul = 'li.menu-item-has-children ul';
                var dima_hover = 'dima-hover';
                var dima_action = 'dima-action';
                var timer = {};

                function ShowSubMenu(element) {
                    element.addClass(dima_hover).siblings(desk_childe_li).removeClass(dima_hover);
                    if (Modernizr && Modernizr.touchevents) {
                        element.siblings(desk_childe_li).data(dima_action, 0);
                        element.find('.' + dima_hover).removeClass(dima_hover).data(dima_action, 0);
                    }
                }

                function HideSubMenu(element) {
                    element.find('.' + dima_hover).removeClass(dima_hover);
                }

                function hoverIn(e) {
                    clearTimeout(timer.id);
                    var li = $(e.target).closest('li');
                    if (li.hasClass('menu-item-has-children')) {
                        ShowSubMenu(li);
                    }
                }

                function hoverOut(e) {
                    clearTimeout(timer.id);
                    var Menu = $.contains(document.getElementsByClassName('desk-nav')[0], e.toElement);
                    var ms = (Menu) ? 500 : 1000;
                    var ul = $(this).closest('ul');
                    timer.id = setTimeout(function () {
                        HideSubMenu(desk_nav_menu);
                    }, ms);
                }

                function touchIn(e) {
                    var li = $(e.target).closest('li');
                    li.data(dima_action, li.data(dima_action) + 1);
                    if (li.hasClass('menu-item-has-children') && li.data(dima_action) === 1) {
                        e.preventDefault();
                        e.stopPropagation();
                        ShowSubMenu(li);
                    }
                }

                function touchOut(e) {
                    $(desk_childe_li).data(dima_action, 0);
                    HideSubMenu(desk_nav_menu);
                }
                if (Modernizr && Modernizr.touchevents) {
                    $(desk_childe_li).data(dima_action, 0);
                    desk_nav_menu.on('touchstart click', desk_childe_li, touchIn);
                    desk_nav_menu.on('touchstart click', function (e) {
                        e.stopPropagation();
                    });
                    $('body').on('touchstart click', touchOut);
                } else {
                    desk_nav_menu.hoverIntent({
                        over: hoverIn,
                        out: hoverOut,
                        selector: desk_childe_li
                    });
                    desk_nav_menu.on('focusin', desk_childe_li, hoverIn);
                    desk_nav_menu.on('focusout', desk_childe_li, hoverOut);
                }
            };
            var mobileNav = function () {
                var d = $(".dima-nav"),
                    i = $("a.dima-btn-nav");
                i.click(function (event) {
                    event.preventDefault();
                    if (i.hasClass("btn-active")) {
                        d.slideUp();
                        i.removeClass("btn-active");
                    } else {
                        i.addClass("btn-active");
                        d.slideDown();
                    }
                });
                $('.mobnav-subarrow').click(function () {
                    $(this).parent().toggleClass("xpopdrop");
                });
            };
            var fixNav = function () {
                $.fn.fix_navbar = function () {
                    var $body = $('body');
                    var $navbar = $('.dima-navbar');
                    var $navbarwarp = $('.dima-navbar-fixed-top-active');
                    var navbarTop = $('.dima-navbar-wrap').offset().top - $('#wpadminbar').outerHeight();
                    if (windowHeight < navbarTop) {
                        navbarTop = windowHeight;
                    }
                    $(window).scroll(function () {
                        if ($(this).scrollTop() > navbarTop) {
                            $navbar.addClass('fix_nav');
                            $navbarwarp.addClass('fixed');
                        } else {
                            $navbar.removeClass('fix_nav');
                            $navbarwarp.removeClass('fixed');
                        }
                    });
                };
                $.fn.show_navbar = function (el) {
                    el = $(this);
                    var $body = $('body');
                    var offsetBy = el.attr("data-offsetBy");
                    var oFFset = $(offsetBy).outerHeight() || 0;
                    var topBar = $('.dima-topbar').outerHeight() || 0;
                    var $navbar = $('.dima-navbar');
                    var $navbarwarp = $('.dima-navbar-wrap');
                    var menuVal = $('.dima-navbar').outerHeight() || 0;
                    var navbarTop = $('.dima-navbar-wrap').offset().top - $('#wpadminbar').outerHeight();
                    var offsetTop = topBar + oFFset + menuVal;
                    $(window).scroll(function () {
                        if ($(this).scrollTop() >= offsetTop) {
                            $navbar.addClass('fix_nav animated fadeInDown');
                            $navbarwarp.addClass('fixed');
                        } else {
                            $navbar.removeClass('fix_nav animated fadeInDown');
                            $navbarwarp.removeClass('fixed');
                        }
                    });
                };
            };
            var MobilesubMenu = function () {
                var dima_sub_icon = $('.mobile-nav .sub-icon'),
                    desktopTargets = 'li.menu-item-has-children',
                    u = "dima-active",
                    dimahover = "dima-hover",
                    h = dima_sub_icon.find(".mobile-nav li.menu-item-has-children > a"),
                    m = dima_sub_icon.find(".sub-menu");
                dima_sub_icon.find('>a').each(function (n) {
                    $(this).append('<div class="dima-sub-toggle" data-toggle="collapse" data-target=".sub-menu.sm-' + n + '"><span class="sub-icon"></span></div>')
                }), m.each(function (n) {
                    $(this).addClass("sm-" + n + " collapse");
                }), $(".dima-sub-toggle").on("click", function (n) {
                    n.preventDefault(), $(this).toggleClass(u).closest("li").toggleClass(u)
                });
            };
            var inline_cenet_logo = function () {
                var dima_header_style_split = $('.dima-navbar-center');
                if (dima_header_style_split.length) {
                    dima_header_menu_split();
                }
            };
            var dima_header_menu_split = function () {
                var $dima_top_menu = $('.dima-navbar-center');
                if ($dima_top_menu.length) {
                    var dima_nav = $('.desk-nav .dima-navbar-center nav'),
                        logo_container = $('.desk-nav .dima-navbar-center .logo-cenetr > .logo'),
                        logo_container_two = $('.desk-nav .dima-navbar-center .logo'),
                        dima_top_navigation_li_size = dima_nav.children('ul').children('li').size(),
                        dima_top_navigation_li_break_index = Math.round(dima_top_navigation_li_size / 2) - 1;
                    if (windowWidth > 989 && !logo_container.length && logo_container_two.length == 1) {
                        $('<li class="logo-cenetr"></li>').insertAfter($dima_top_menu.find('nav > ul >li:nth(' + dima_top_navigation_li_break_index + ')'));
                        logo_container_two.appendTo($dima_top_menu.find('.logo-cenetr'));
                    }
                    if (windowWidth <= 989 && logo_container.length == 1) {
                        logo_container_two.prependTo('.dima-navbar >.container');
                        $('.logo-cenetr').remove();
                    }
                }
            };
            var searchBox = function () {
                var bool = true;
                $(".search-btn").click(function (e) {
                    e.preventDefault();
                    var x = $($(this).parent().parent().parent().parent());
                    if (bool) {
                        x.find("#search-box").stop().slideDown(250, "easeOutExpo");
                        $("#search-box input[type=text]").focus();
                        bool = false;
                    } else {
                        x.find("#search-box").stop().slideUp(250, "easeOutExpo");
                        bool = true;
                    }
                });

                function closeSearch() {
                    $("#search-box").stop().slideUp(250, "easeOutExpo");
                }
                $("#close").click(function (e) {
                    e.preventDefault();
                    closeSearch();
                    bool = true;
                });
            };
            var onePage = function () {
                $('.dima-onepage').onePageNav({
                    currentClass: 'current',
                    changeHash: true,
                    scrollSpeed: 750,
                    scrollThreshold: 0.5,
                    filter: '',
                    easing: 'swing',
                    begin: function () {
                        $('body').append('<div id="device-dummy" style="height: 1px;"></div>');
                    },
                    end: function () {
                        $('#device-dummy').remove();
                    },
                    scrollChange: function () { }
                });
                $("a[data-scrollto]").click(function (event) {
                    event.preventDefault();
                    var divScrollToAnchor = $(this).attr('data-scrollto');
                    var topOffsetScroll = 0;
                    $('html, body').stop().animate({
                        'scrollTop': $(divScrollToAnchor).offset().top - topOffsetScroll
                    }, 750, 'easeOutQuint');
                    return false;
                });
            };
            var init = function () {
                myMenu();
                $("html").imagesLoaded();
            };
            var build = {
                init: init,
            };
            return build;
        }();
  
        App.SHOP = function () {
            var toggleBox = function () {
                $('a.show-box').click(function () {
                    var Val = $(this).attr("data-show");
                    $(Val).slideToggle();
                    return false;
                });
                $('.radio').click(function () {
                    var Val = $(this).attr("data-show");
                    $('.toHide').hide();
                    $(Val).slideToggle();
                });
                $('.checkbox').click(function () {
                    var Val = $(this).attr("data-show");
                    $(Val).slideToggle();
                });
            };
            var zoom = function () {
                $('.zoom-it').imagezoomsl({
                    classmagnifier: window.external ? window.navigator.vendor === 'Yandex' ? "" : 'round-loope' : "",
                    zoomrange: [2, 8],
                });
            };
            var showShopList = function () {
                $(".di-grids").click(function () {
                    $("#rows").fadeOut(1000, function () {
                        $("#rows").removeClass('products-list').addClass('products-grids');
                        $("#rows").fadeIn(1000);
                    });
                });
                $(".di-list").click(function () {
                    $("#rows").fadeOut(1000, function () {
                        $("#rows").removeClass('products-grids').addClass('products-list');
                        $("#rows").fadeIn(1000);
                    });
                });
            };
            var shopBtn = function () {
                $(".minus").click(function () {
                    var inputEl = $(this).parent().children().next();
                    var qty = inputEl.val();
                    if ($(this).parent().hasClass("minus")) qty++;
                    else qty--;
                    if (qty < 0) qty = 0;
                    inputEl.val(qty);
                });
                $(".plus").click(function () {
                    var inputEl = $(this).parent().children().next();
                    var qty = inputEl.val();
                    if ($(this).hasClass("plus")) qty++;
                    else qty--;
                    if (qty < 0) qty = 0;
                    inputEl.val(qty);
                });
            };
            var sliderRange = function () {
                $("#slider-range").slider({
                    range: true,
                    min: 0,
                    max: 40,
                    values: [5, 30],
                    slide: function (event, ui) {
                        $("#amount").val("$" + ui.values[0] + " - $" + ui.values[1]);
                    }
                });
                $("#amount").val("$" + $("#slider-range").slider("values", 0) + " - $" + $("#slider-range").slider("values", 1));
            };
            var init = function () {
                showShopList();
                toggleBox();
                shopBtn();
                sliderRange();
                zoom();
            };
            var build = {
                init: init,
            };
            return build;
        }();
        App.SCROLL = function () {
            var localScroll = function () {
                $(".dima-nav").localScroll({
                    target: "body",
                    axis: "y",
                    duration: 800,
                    margin: true,
                    offset: -(nav - 13)
                });
            };
            var callingSly = function () {
                var slyOptions = {
                    scrollBy: 200,
                    speed: 600,
                    smart: 0,
                    easing: 'easeOutQuart',
                    scrollBar: '.scrollbar',
                    dynamicHandle: 1,
                    dragHandle: 0,
                    clickBar: 1,
                    contentEditable: 1,
                    mouseDragging: 0,
                    touchDragging: 1,
                    releaseSwing: 1,
                    swingSpeed: 0.1,
                    elasticBounds: 1,
                    cycleBy: null,
                    cycleInterval: 4000
                };
                var d = new Sly('#framee', slyOptions);
                d.init();
            };
            var perfectScrollbar = function () {
                $(".quick-view-content").perfectScrollbar();
                $('.quick-view-content').perfectScrollbar('update');
            }
            var parallax = function () {
                if (isFirefox) {
                    prefix = '-moz-';
                } else if (isChrome || isSafari) {
                    prefix = '-webkit-';
                }
                $('.background-image-hide,.background-image-holder').each(function () {
                    var imgSrc = $(this).children('img').attr('src');
                    if (typeof imgSrc !== typeof undefined) {
                        $(this).css('background', 'url("' + imgSrc + '")');
                        $(this).children('img').hide();
                        $(this).css('background-position', '0% 0%');
                    }
                });
                $('.parallax-background').each(function () {
                    $(this).attr('data-bottom-top', prefix + 'transform: translate3d(0px,-200px, 0px);');
                    $(this).attr('data-center', prefix + 'transform: translate3d(0px,0px, 0px);');
                    $(this).attr('data-top-bottom', prefix + 'transform: translate3d(0px,100px, 0px);');
                    $(this).attr('data-direction', 'vertical');
                });
                if (!(/Android|iPhone|iPad|iPod|BlackBerry|Windows Phone/i).test(navigator.userAgent || navigator.vendor || window.opera)) {
                    var skr = skrollr.init({
                        forceHeight: false,
                        smoothScrolling: true,
                    });
                    skr.refresh($(".homeSlide"));
                }
            };
            var init = function () {
                localScroll();
                parallax();
                perfectScrollbar();
                callingSly();
                $.scrollToTop();
            }
            var build = {
                init: init,
            };
            return build;
        }();
        App.ANIMATION = function () {
            var animations = function () {
                var elm = $("[data-animate]");
                elm.each(function () {
                    var elm = $(this),
                        dataDelay = elm.attr("data-delay") || 0,
                        offsetVal = elm.attr("data-offset") || "100%",
                        trgger = elm.attr("data-trgger") || "false",
                        a = 0;
                    var dataAnimate = elm.attr("data-animate");
                    if (a = dataDelay ? Number(dataDelay) + 10 : 300, !elm.hasClass("animated")) {
                        $(this).addClass('opacity-zero');
                        elm.waypoint(function () {
                            var $this = $(this);
                            setTimeout(function () {
                                $this.addClass('show animated ' + dataAnimate);
                            }, a);
                        }, {
                                offset: offsetVal,
                                triggerOnce: trgger
                            });
                    }
                })
            };
            var notAnimations = function () {
                var elm = $("[data-animate]");
                elm.each(function () {
                    var elm = $(this),
                        dataDelay = elm.attr("data-delay") || 0,
                        offsetVal = elm.attr("data-offset") || "100%",
                        trgger = elm.attr("data-trgger") || "false",
                        a = 0;
                    if (a = dataDelay ? Number(dataDelay) + 300 : 300, !elm.hasClass("animated")) {
                        $(this).addClass('opacity-zero');
                        elm.waypoint(function () {
                            var $this = $(this);
                            setTimeout(function () {
                                $this.animate({
                                    opacity: 1
                                }, {
                                        step: function (now, fx) {
                                            var X = 100 * now;
                                            $(fx.elem).css("filter", "alpha(opacity=" + X + ")");
                                        }
                                    });
                            }, a);
                        }, {
                                offset: offsetVal,
                                triggerOnce: trgger
                            });
                    }
                })
            };
            var twoLinesHover = function () {
                $('.link_overlay.two_lines').each(function () {
                    var h = $(this).width();
                    var t = (h - 130) / 2;
                    $(this).find('ul').css({
                        'margin-left': t,
                        'margin-right': t
                    })
                });
                $('.work-info').each(function () {
                    var h = $(this).parent().outerHeight();
                    if (h <= 100) {
                        h = $(this).parent().outerHeight();
                    }
                    var t = h - ((h / 2) + 75);
                    $(this).css({
                        'bottom': t
                    })
                });
            };
            var init = function () {
                if (!Modernizr.mq('only all and (max-width: 480px)')) {
                    if (isTransitions) {
                        animations();
                    } else {
                        notAnimations();
                    }
                }
                twoLinesHover();
            }
            var build = {
                init: init,
                twoLinesHover: twoLinesHover,
            };
            return build;
        }();
        App.SLIDE = function () {
            var flexSlider = function () {
                var D = $(".flexslider");
                D.each(function () {
                    var elm = $(this);
                    var d = elm.attr("data-animation") || 'slide';
                    var i = elm.attr("data-easing") || 'swing';
                    var m = elm.attr("data-direction") || 'horizontal';
                    var a = elm.attr("data-autoplay");
                    var o = elm.attr("data-show") || 7000;
                    var k = elm.attr("data-speed") || 700;
                    var b = elm.attr("data-video") || 0;
                    var e = elm.attr("data-nav") || 1;
                    var z = elm.attr("data-arrows") || 1;
                    var p = elm.attr("data-loop") || !1;
                    var h = 0;
                    var r = 1;
                    elm.css('height', 'auto').flexslider({
                        selector: ".slides > .slide-item",
                        animation: d,
                        easing: i,
                        slideshow: !a,
                        direction: m,
                        slideshowSpeed: Number(o),
                        animationSpeed: Number(k),
                        video: b,
                        controlNav: e,
                        directionNav: z,
                        animationLoop: p,
                        useCSS: r,
                        smoothHeight: h,
                        pauseOnHover: h,
                        mousewheel: h,
                        multipleKeyboard: r,
                        start: function () {
                            App.LIGHTBOX.lightBox();
                        },
                        after: function () { }
                    });
                })
            };
            var owlSlider = function () {
                $(".owl-carousel").each(function () {
                    var elm = $(this),
                        d = elm.attr("data-owl-namber") || 1,
                        i = elm.attr("data-owl-autoPlay"),
                        m = elm.attr("data-owl-navigation") || false,
                        a = elm.attr("data-owl-pagination"),
                        p = elm.attr("data-owl-mousewheel"),
                        e = elm.attr("data-owl-loop") || true,
                        phone = elm.attr("data-owl-phone") || 1,
                        tablet = elm.attr("data-owl-tablet") || 1,
                        margin_val = parseInt(elm.attr("data-owl-margin")) || 0;
                    if (i === "true") {
                        i = true;
                    } else {
                        i = false;
                    }
                    if (a === "true") {
                        a = true;
                    } else {
                        a = false;
                    }
                    if (m === "true") {
                        m = true;
                        elm.next().css("display", "block");
                        $(".next").click(function () {
                            elm.trigger('next.owl.carousel');
                        });
                        $(".prev").click(function () {
                            elm.trigger('prev.owl.carousel', [300]);
                        });
                    } else {
                        m = false;
                    }
                    elm.owlCarousel({
                        items: d,
                        margin: margin_val,
                        nav: m,
                        dots: a,
                        slideSpeed: 500,
                        dotsSpeed: 400,
                        autoplay: i,
                        rtl: true,
                        lazyLoad: true,
                        loop: e,
                        responsiveClass: true,
                        responsive: {
                            250: {
                                items: phone
                            },
                            480: {
                                items: tablet
                            },
                            768: {
                                items: tablet
                            },
                            989: {
                                items: tablet
                            },
                            1199: {
                                items: d
                            }
                        }
                    });
                    if (p === "true") {
                        elm.bind('mousewheel', '.owl-stage', function (e) {
                            e.preventDefault();
                            if (e.originalEvent.wheelDelta > 0) {
                                elm.trigger('next.owl.carousel');
                            } else {
                                elm.trigger('prev.owl.carousel', [300]);
                            }
                            e.preventDefault();
                        });
                    }
                });
            };
            var revolution = function () {
                var d, i, m, a, p;
                d = $(".fullscreenOnePage").show().revolution({
                    dottedOverlay: "none",
                    delay: 9000,
                    startwidth: 1170,
                    startheight: 700,
                    hideThumbs: 200,
                    thumbWidth: 100,
                    thumbHeight: 50,
                    thumbAmount: 5,
                    navigationType: "bullet",
                    navigationArrows: "solo",
                    navigationStyle: "none",
                    touchenabled: "on",
                    onHoverStop: "on",
                    parallax: "mouse",
                    parallaxBgFreeze: "on",
                    swipe_velocity: 0.7,
                    swipe_min_touches: 1,
                    swipe_max_touches: 1,
                    drag_block_vertical: false,
                    keyboardNavigation: "on",
                    navigationHAlign: "center",
                    navigationVAlign: "bottom",
                    navigationHOffset: 0,
                    navigationVOffset: 20,
                    soloArrowLeftHalign: "left",
                    soloArrowLeftValign: "center",
                    soloArrowLeftHOffset: 20,
                    soloArrowLeftVOffset: 0,
                    soloArrowRightHalign: "right",
                    soloArrowRightValign: "center",
                    soloArrowRightHOffset: 20,
                    soloArrowRightVOffset: 0,
                    shadow: 0,
                    fullWidth: "on",
                    fullScreen: "on",
                    spinner: "spinner0",
                    stopLoop: "off",
                    stopAfterLoops: -1,
                    stopAtSlide: -1,
                    shuffle: "off",
                    forceFullWidth: "off",
                    fullScreenAlignForce: "off",
                    minFullScreenHeight: "400",
                    hideThumbsOnMobile: "off",
                    hideNavDelayOnMobile: 1500,
                    hideBulletsOnMobile: "off",
                    hideArrowsOnMobile: "off",
                    hideThumbsUnderResolution: 0,
                    hideSliderAtLimit: 0,
                    hideCaptionAtLimit: 0,
                    hideAllCaptionAtLilmit: 0,
                    startWithSlide: 0,
                    fullScreenOffsetContainer: ""
                });
                i = $(".fullscreen").show().revolution({
                    dottedOverlay: "none",
                    delay: 16000,
                    startwidth: 1140,
                    startheight: 550,
                    hideThumbs: 200,
                    thumbWidth: 100,
                    thumbHeight: 50,
                    thumbAmount: 5,
                    navigationType: "bullet",
                    navigationArrows: "solo",
                    navigationStyle: "none",
                    touchenabled: "on",
                    onHoverStop: "on",
                    swipe_velocity: 0.7,
                    swipe_min_touches: 1,
                    swipe_max_touches: 1,
                    drag_block_vertical: false,
                    parallax: "mouse",
                    parallaxBgFreeze: "on",
                    parallaxLevels: [7, 4, 3, 2, 5, 4, 3, 2, 1, 0],
                    keyboardNavigation: "off",
                    navigationHAlign: "center",
                    navigationVAlign: "bottom",
                    navigationHOffset: 0,
                    navigationVOffset: 20,
                    soloArrowLeftHalign: "left",
                    soloArrowLeftValign: "center",
                    soloArrowLeftHOffset: 20,
                    soloArrowLeftVOffset: 0,
                    soloArrowRightHalign: "right",
                    soloArrowRightValign: "center",
                    soloArrowRightHOffset: 20,
                    soloArrowRightVOffset: 0,
                    shadow: 0,
                    fullWidth: "on",
                    fullScreen: "off",
                    spinner: "spinner4",
                    stopLoop: "off",
                    stopAfterLoops: -1,
                    stopAtSlide: -1,
                    shuffle: "off",
                    autoHeight: "off",
                    forceFullWidth: "off",
                    hideThumbsOnMobile: "off",
                    hideNavDelayOnMobile: 1500,
                    hideBulletsOnMobile: "off",
                    hideArrowsOnMobile: "off",
                    hideThumbsUnderResolution: 0,
                    hideSliderAtLimit: 0,
                    hideCaptionAtLimit: 0,
                    hideAllCaptionAtLilmit: 0,
                    startWithSlide: 0,
                    fullScreenOffsetContainer: "header"
                });
                m = jQuery('.minfullwidth').show().revolution({
                    delay: 15000,
                    dottedOverlay: "none",
                    startwidth: 1170,
                    startheight: 650,
                    hideThumbs: 10,
                    fullWidth: "on",
                    navigationType: "bullet",
                    navigationArrows: "solo",
                    navigationStyle: "none",
                    touchenabled: "on",
                    onHoverStop: "on",
                    navigationHAlign: "center",
                    navigationVAlign: "bottom",
                    navigationHOffset: 0,
                    navigationVOffset: 35,
                    fullScreenOffsetContainer: "header"
                });
                a = jQuery('.boxedcontainer').show().revolution({
                    delay: 15000,
                    startwidth: 1170,
                    startheight: 450,
                    hideThumbs: 10,
                    fullWidth: "off",
                    forceFullWidth: "off",
                    navigationType: "bullet",
                    navigationArrows: "solo",
                    navigationStyle: "none",
                    touchenabled: "on",
                    onHoverStop: "on",
                    navigationHAlign: "center",
                    navigationVAlign: "bottom",
                    navigationHOffset: 0,
                    navigationVOffset: 35,
                    fullScreenOffsetContainer: ""
                });
                p = jQuery('.photoslide').show().revolution({
                    delay: 4000,
                    startwidth: 1170,
                    startheight: 500,
                    hideThumbs: 0,
                    thumbWidth: 42,
                    thumbHeight: 50,
                    thumbAmount: 10,
                    navigationType: "thumb",
                    navigationArrows: "solo",
                    navigationStyle: "round",
                    fullWidth: "off",
                    fullScreen: "on",
                    soloArrowLeftHalign: "left",
                    soloArrowLeftValign: "center",
                    soloArrowLeftHOffset: 20,
                    soloArrowLeftVOffset: 0,
                    soloArrowRightHalign: "right",
                    soloArrowRightValign: "center",
                    soloArrowRightHOffset: 20,
                    soloArrowRightVOffset: 0,
                    shadow: 0,
                    touchenabled: "on",
                    onHoverStop: "on",
                    navigationHAlign: "center",
                    navigationVAlign: "bottom",
                    navigationHOffset: 0,
                    navigationVOffset: 35,
                    fullScreenOffsetContainer: ""
                });
            };
            var init = function () {
                flexSlider();
                owlSlider();
                revolution();
            }
            var build = {
                init: init,
                flexSlider: flexSlider,
                revolution: revolution,
            };
            return build;
        }();
        App.LIGHTBOX = function () {
            var lightBox = function () {
                var d = $('[data-lightbox="image"]'),
                    i = $('[data-lightbox="iframe"]'),
                    m = $('[data-lightbox="ajax"]'),
                    a = $('[data-lightbox="gallery"]');
                d.magnificPopup({
                    type: 'image',
                    closeOnContentClick: !0,
                    closeBtnInside: !1,
                    fixedContentPos: !0,
                    mainClass: "mfp-zoom-in",
                    image: {
                        verticalFit: !0
                    }
                });
                a.each(function () {
                    $(this).magnificPopup({
                        delegate: 'a.gallery-item',
                        type: 'image',
                        overflowY: 'scroll',
                        closeOnContentClick: !0,
                        closeBtnInside: !1,
                        fixedContentPos: !0,
                        mainClass: "mfp-zoom-in",
                        image: {
                            verticalFit: !0
                        },
                        gallery: {
                            enabled: !0,
                            navigateByImgClick: !0,
                            preload: [0, 1]
                        }
                    });
                });
                i.magnificPopup({
                    disableOn: 500,
                    type: 'iframe',
                    mainClass: 'mfp-zoom-in',
                    removalDelay: 160,
                    preloader: 0,
                    fixedContentPos: 0
                });
                $('a[href*="youtube.com/watch"]').magnificPopup({
                    type: 'iframe',
                    iframe: {
                        patterns: {
                            youtube: {
                                index: 'youtube.com',
                                id: 'v=',
                                src: '//www.youtube.com/embed/%id%?rel=0&autoplay=1'
                            }
                        }
                    }
                });
                m.magnificPopup({
                    type: 'ajax',
                    closeBtnInside: 0,
                    alignTop: 1,
                    cache: 1,
                    overflowY: 'scroll',
                    mainClass: "mfp-zoom-in",
                    callbacks: {
                        ajaxContentAdded: function () {
                            App.SLIDE.flexSlider();
                            App.SHOP.init();
                            App.UI.init();
                            App.SCROLL.init();
                        }
                    }
                });
            };
            var init = function () {
                lightBox();
            }
            var build = {
                init: init,
                lightBox: lightBox,
            };
            return build;
        }();
    
        App.UI = function () {
            var flatShadow = function () {
                $(".flat-icon").flatshadow({
                    color: "#FFF",
                    angle: "SE",
                    fade: true,
                    boxShadow: false
                });
            };
            var countUp = function () {
                $(".countUp").each(function () {
                    $(this).waypoint({
                        handler: function () {
                            $(this).find("span").countTo({
                                formatter: function (e) {
                                    return e = e.toFixed(), e = e.replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                                }
                            });
                        },
                        offset: "100%",
                        triggerOnce: true
                    });
                });
            };
            var progress = function () {
                setTimeout(function () {
                    $(".progress,.dial").waypoint(function () {
                        $(".progress .progress-bar").each(function () {
                            var el = $(this),
                                perc = el.attr("aria-valuenow"),
                                current_percent = 0,
                                progress = setInterval(function () {
                                    if (current_percent >= perc) {
                                        clearInterval(progress);
                                    } else {
                                        current_percent += 1;
                                        el.css("width", (current_percent) + "%");
                                    }
                                }, 1);
                        });
                        $('.progress').append(function () {
                            var elm = $(this),
                                color = elm.attr("data-color-val");
                            elm.css('border-color', color);
                            elm.find('.progress-bar').css('background-color', color);
                            elm.find('.progress-bar .percent').css('background-color', color);
                            elm.find('span').css('border-top-color', color);
                        });
                        $('.dial').each(function () {
                            var elm = $(this),
                                width = elm.attr("data-width"),
                                perc = elm.attr("value");
                            elm.knob({
                                'value': 0,
                                'min': 0,
                                'max': 100,
                                "skin": "tron",
                                "readOnly": true,
                                "thickness": 0.09,
                                "displayInput": false,
                                "bgColor": "rgba(255,255,255,0)",
                                "linecap": ""
                            });
                            $({
                                value: 0
                            }).animate({
                                value: perc
                            }, {
                                    duration: 1000,
                                    easing: 'swing',
                                    progress: function () {
                                        elm.val(Math.ceil(this.value)).trigger('change');
                                    }
                                });
                            $(this).append(function () {
                                elm.parent().parent().find('.circular-bar-content').css('top', -(width / 2 + 10));
                                elm.parent().parent().find('.circular-bar-content label').text(perc + '%');
                            });
                        });
                    }, {
                            offset: "100%",
                            triggerOnce: true
                        });
                }, 300);
            };
            var tabs = function () {
                $("body").tooltip({
                    selector: "[data-toggle=tooltip]",
                    animation: true,
                    html: true
                });
                $("#dima-tab-nav a:first").tab("show");
                $("#dima-tab-nav a").click(function (e) {
                    e.preventDefault();
                    $(this).tab("show");
                });
                $(".dima-tabs.responsive-tab").responsiveTabs();
            };
            var notification = function () {
                $(".dima-alert button.close").click(function () {
                    $(this).parent().fadeOut(200, "easeOutExpo");
                });
            };
            var element_bg = function () {
                var elm = $("[data-element-bg]");
                elm.each(function () {
                    var b = $(this).attr("data-element-bg");
                    $(this).css({
                        "background-image": "url(" + b + ")",
                        "background-position": "100% 100%",
                        "background-repeat": "no-repeat",
                    });
                });
            };
            var datepicker = function () {
                var elm = $("[data-datepicker]");
                elm.each(function () {
                    $(this).datepicker({
                        beforeShow: function (textbox, instance) {
                            $('#ui-datepicker-div').css('min-width', $(this).outerWidth());
                        },
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        dateFormat: 'dd/mm/yy'
                    }).datepicker('widget').wrap('<div class="ll-skin-melon"/>');
                });
            }
            var init = function () {
                flatShadow();
                countUp();
                progress();
                tabs();
                notification();
                datepicker();
                element_bg();
            }
            var build = {
                init: init,
            };
            return build;
        }();
        App.EVENT = function () {
            var event = function () {
                $('input, textarea').placeholder();
                if (windowWidth > 960) {
                    if ($('.dima-navbar').hasClass('fix-one')) {
                        $(".fix-one").fix_navbar();
                    }
                    if ($('.dima-navbar').hasClass('fix-two')) {
                        $(".fix-two").show_navbar();
                    }
                }
                if (windowWidth < 980) {
                    $('.dima-nav-end ul').css("top", "-1000px");
                } else {
                    $('.dima-nav-end ul').css("top", "100px");
                }
                if (windowWidth > 767) {
                    $('.set-parent-height').each(function () {
                        var parentHeight = $(this).parent().height();
                        $(this).css("height", parentHeight);
                    });
                }
                $('.sidebar li a[href^="' + location.pathname.split("/")[2] + '"]').parent().addClass('active');
                $('.section-colored').each(function () {
                    var bg = $(this).attr("data-bg");
                    $(this).css("background-color", bg);
                });
            };
            var init = function () {
                event();
            }
            var build = {
                init: init,
            };
            return build;
        }();
        App.DOCONRESIZE = function () {
            var nav = function () {
                windowWidth = w.width();
                if ($('.dima-navbar').hasClass('fix-one')) {
                    $(".fix-one").fix_navbar();
                }
                if ($('.dima-navbar').hasClass('fix-two')) {
                    $(".fix-two").show_navbar();
                }
                if (windowWidth < 980) {
                    $('.dima-nav-end ul').css("top", "-1000px");
                } else {
                    $('.dima-nav-end ul').css("top", "100px");
                }
            };
            var responsiveFlexSlider = function () {
                var x = $(".flexslider .slides").outerHeight();
                if (x < windowHeight) {
                    $('.flexslider').height(x);
                } else {
                    $('.flexslider').height(windowHeight);
                }
            };
            var parentSize = function () {
                if (windowWidth > 767) {
                    $('.set-parent-height').each(function () {
                        var parentHeight = $(this).parent().height();
                        $(this).css("height", parentHeight);
                    });
                }
            };
            var init = function () {
                nav();
                parentSize();
                App.ANIMATION.twoLinesHover();
                responsiveFlexSlider();
            };
            var READY = {
                init: init,
            };
            return READY;
        }();
        App.PORTFOLIO = function () {
            var openAjax = function () {
                $(".ajax-portfolio .isotope-item a[data-load]").click(function (d) {
                    var id = $(this).parents(".isotope-item").attr("id");
                    var url = $(this).attr("href");
                    d.preventDefault()
                    if ($(".portfolio-ajax-expanded").is(":visible")) {
                        closeAjax();
                        setTimeout(function () {
                            loadAjax(url, id);
                        }, 700);
                    } else loadAjax(url, id);
                })
            };
            var loadAjax = function (url, id) {
                $.ajax({
                    url: url,
                    type: "get",
                    cache: false,
                    data: {},
                    beforeSend: function () {
                        $("#ajax-loader").fadeIn();
                    },
                    success: function (data) {
                        $('.portfolio-ajax-expanded').html(data);
                    },
                    complete: function () {
                        App.SLIDE.flexSlider();
                        App.LIGHTBOX.init();
                        initAjax(id);
                        openItem();
                    }
                });
            }
            var closeAjax = function () {
                $(".portfolio-ajax-expanded").find(".portfolio-ajax-content").slideUp(600, function () {
                    $(".portfolio-ajax-expanded").css({
                        display: "none"
                    });
                });
            };
            var openItem = function () {
                $("#ajax-loader").fadeOut();
                setTimeout(function () {
                    $.when($(".portfolio-ajax-expanded").slideDown(900, "easeOutQuad")).then($("html,body").stop(!0).animate({
                        scrollTop: $(".portfolio-ajax-expanded").offset().top - 150
                    }, 900, "easeOutQuad"));
                }, 400);
            };
            var initAjax = function () {
                $("#next-portfolio, #prev-portfolio").click(function (d) {
                    var id = $(this).attr("data-id");
                    var url = $("#" + id).find("a[data-load]").attr("href");
                    closeAjax();
                    setTimeout(function () {
                        loadAjax(url, id);
                    }, 700);
                    d.preventDefault()
                });
                $(".close-ajax-portfolio").click(function (d) {
                    $(".portfolio-ajax-expanded").find(".portfolio-ajax-content").slideUp(600, function () {
                        $(".portfolio-ajax-expanded").css({
                            display: "none"
                        });
                    });
                    d.preventDefault();
                })
            };
            var filterIsotop = function () {
                var $container = $('.dima-isotope-container')
                var $containers = $('#infinite');
                $container.imagesLoaded(function () {
                    $container.isotope({
                        filter: '*',
                        layoutMode: "masonry",
                        itemSelector: '.isotope-item',
                        masonry: {
                            columnWidth: '.isotope-item',
                        },
                        percentPosition: true
                    });
                });
                $('.filters a').click(function () {
                    var li_p = $(this).parent()
                    $('.filters  .current').removeClass('current')
                    $(li_p).addClass('current')
                    var selector = $(this).attr('data-filter')
                    $container.isotope({
                        filter: selector,
                    })
                    return false
                });
                $containers.infinitescroll({
                    navSelector: '#page_nav',
                    nextSelector: '#page_nav a',
                    itemSelector: '.isotope-item',
                    bufferPx: 200,
                    loading: {
                        finishedMsg: 'We\'re done here.',
                        msgText: "<em>Loading the next set of posts...</em>",
                    }
                }, function (newElements) {
                    App.LIGHTBOX.lightBox();
                    App.ANIMATION.twoLinesHover();
                    App.SLIDE.init();
                    var $newElems = jQuery(newElements).hide();
                    $newElems.imagesLoaded(function () {
                        $newElems.fadeIn();
                        $container.isotope('appended', $newElems);
                    });
                });
            };
            var init = function () {
                openAjax();
                filterIsotop();
            };
            var READY = {
                init: init,
                filterIsotop: filterIsotop,
            };
            return READY;
        }();
        App.CONTACT = function () {
            var contect = function () {
                var contact = $("#contact"),
                    url = contact.attr("action");
                contact.validate({
                    submitHandler: function () {
                        $("#contact").addClass('loading-form')
                        $.ajax({
                            type: "POST",
                            url: url,
                            datatype: "json",
                            data: {
                                "name": $("#contact #name").val(),
                                "email": $("#contact #email").val(),
                                "subject": $("#contact #subject").val(),
                                "message": $("#contact #message").val()
                            },
                            success: function (data) {
                                if (data === "success") {
                                    $("#contactSuccess").removeClass("hide").addClass("show animated bounceIn");
                                    $("#contactError").addClass("hide").removeClass("show animated bounceIn");
                                } else {
                                    $("#contactError").removeClass("hide").addClass("show animated bounceIn");
                                    $("#contactSuccess").addClass("hide").removeClass("show animated bounceIn");
                                }
                            },
                            complete: function () {
                                $("#contact").removeClass('loading-form');
                                $("#contact").find('.field').removeClass("success").removeClass("error").find("input").val("");
                            }
                        });
                    },
                    rules: {
                        name: {
                            required: true
                        },
                        email: {
                            required: true,
                            email: true
                        },
                        subject: {
                            required: true
                        },
                        message: {
                            required: true
                        }
                    },
                    highlight: function (element) {
                        $(element).parent().removeClass("success").addClass("error");
                    },
                    success: function (element) {
                        $(element).parent().removeClass("error").addClass("success").find("label.error").remove();
                    },
                });
            };
            var newsletter = function () {
                var newsletter = $("#newsletter-form");

                function mailchimpCallback(response) {
                    if (response.result === 'success') {
                        $("#newsletteSuccess").removeClass("hide").addClass("show animated bounceIn");
                        $("#newsletteError").addClass("hide").removeClass("show animated bounceIn");
                    } else if (response.result === 'error') {
                        $("#newsletteError").removeClass("hide").addClass("show animated bounceIn");
                        $("#newsletteSuccess").addClass("hide").removeClass("show animated bounceIn");
                    }
                }
                newsletter.ajaxChimp({
                    callback: mailchimpCallback,
                    url: 'http://App.us8.list-manage.com/subscribe/post?u=c6d8aa2f0a313299db3b338b8&id=847c491668'
                });
            };
            var init = function () {
                contect();
                newsletter();
            };
            var build = {
                init: init,
            };
            return build;
        }();
        App.OKABREADY = function () {
            var init = function () {
                App.SLIDE.init();
                App.LIGHTBOX.init();
                App.ANIMATION.init();
                App.NAV.init();
                App.SCROLL.init();
                App.UI.init();
                App.CONTACT.init();
                App.EVENT.init();
                w.resize(function () {
                    App.DOCONRESIZE.init();
                });
            };
            var READY = {
                init: init,
            };
            return READY;
        }();
        App.OKABREADY.init();
    });
}($));
(function ($) {
    $.extend({
        scrollToTop: function () {
            var _isScrolling = false;
            $("body").append($("<a />").addClass("scroll-to-top off").attr({
                "href": "#",
                "id": "scrollToTop"
            }).append($("<i />").addClass("fa  fa-angle-up")));
            $("#scrollToTop").click(function (e) {
                e.preventDefault();
                $("body, html").animate({
                    scrollTop: 0
                }, 500);
                return false;
            });
            $(window).scroll(function () {
                if (!_isScrolling) {
                    _isScrolling = true;
                    if ($(window).scrollTop() > 150) {
                        $("#scrollToTop").stop(true, true).removeClass("off");
                        $("#scrollToTop").stop(true, true).addClass("on");
                        _isScrolling = false;
                    } else {
                        $("#scrollToTop").stop(true, true).removeClass("on");
                        $("#scrollToTop").stop(true, true).addClass("off");
                        _isScrolling = false;
                    }
                }
            });
        }
    });
}($));