(function ($) {
    "use strict";
    /*--
    Commons Variables
    -----------------------------------*/
    var windows = $(window);

    /*--
        Menu Sticky
    -----------------------------------*/
    var sticky = $('.header-sticky');

    windows.on('scroll', function () {
        var scroll = windows.scrollTop();
        if (scroll < 300) {
            sticky.removeClass('is-sticky');
        } else {
            sticky.addClass('is-sticky');
        }
    });

    /*--
        Header Search 
    -----------------------------------*/
    var $headerSearchToggle = $('.header-search-toggle');
    var $headerSearchForm = $('.header-search-form');

    $headerSearchToggle.on('click', function () {
        var $this = $(this);
        if (!$this.hasClass('open')) {
            $this.addClass('open').find('i').removeClass('fa fa-search').addClass('fa fa-times');
            $headerSearchForm.slideDown();
        } else {
            $this.removeClass('open').find('i').removeClass('fa fa-times').addClass('fa fa-search');
            $headerSearchForm.slideUp();
        }
    });
    /*--
        Mobile Menu
    -----------------------------------*/
    $('#mobile-menu-trigger').on('click', function () {
        $('#offcanvas-mobile-menu').removeClass('inactive').addClass('active');
    });


    $('#offcanvas-menu-close-trigger').on('click', function () {
        $('#offcanvas-mobile-menu').removeClass('active').addClass('inactive');
    });
    /*--
        Offcanvas Menu
    -----------------------------------*/
    var $offCanvasNav = $('.offcanvas-navigation'),
        $offCanvasNavSubMenu = $offCanvasNav.find('.submenu2');

    /*Add Toggle Button With Off Canvas Sub Menu*/
    $offCanvasNavSubMenu.parent().prepend('<span class="menu-expand"><i></i></span>');

    /*Close Off Canvas Sub Menu*/
    $offCanvasNavSubMenu.slideUp();

    /*Category Sub Menu Toggle*/
    $offCanvasNav.on('click', 'li a, li .menu-expand', function (e) {
        var $this = $(this);
        if (($this.parent().attr('class').match(/\b(menu-item-has-children|has-children|has-sub-menu)\b/)) && ($this.attr('href') === '#' || $this.hasClass('menu-expand'))) {
            e.preventDefault();
            if ($this.siblings('ul:visible').length) {
                $this.parent('li').removeClass('active');
                $this.siblings('ul').slideUp();
            } else {
                $this.parent('li').addClass('active');
                $this.closest('li').siblings('li').removeClass('active').find('li').removeClass('active');
                $this.closest('li').siblings('li').find('ul:visible').slideUp();
                $this.siblings('ul').slideDown();
            }
        }
    });
    /*---------------------------- 
       3. Sidebar Search Active
    -----------------------------*/
    function sidebarSearch() {
        var searchTrigger = $('.header-search-toggle'),
            endTriggersearch = $('button.search-close'),
            container = $('.main-search-active');

        searchTrigger.on('click', function () {
            container.addClass('inside');
        });

        endTriggersearch.on('click', function () {
            container.removeClass('inside');
        });

    };
    sidebarSearch();
    /*--
        - Background Image
    ------------------------------------------*/
    var $backgroundImage = $('.bg-image');
    $backgroundImage.each(function () {
        var $this = $(this),
            $bgImage = $this.data('bg');
        $this.css('background-image', 'url(' + $bgImage + ')');
    });

    /*------------------------------ 
        Nice Select Active
    ---------------------------------*/
    $('select').niceSelect();

    /*--
        Sliders
    -----------------------------------*/
    // Hero Slider
    $('.hero-slider').slick({
        infinite: true,
        fade: true,
        dots: false,
        prevArrow: '<button class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    dots: true,
                    arrows: false,
                }
            },
        ]
    });
    // Testimonial Slider
    $('.testimonial-slider-content').slick({
        infinite: true,
        arrows: false,
        fade: false,
        dots: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        adaptiveHeight: true,
        prevArrow: '<button class="slick-prev"><i class="fa fa-chevron-left"></i></button>',
        nextArrow: '<button class="slick-next"><i class="fa fa-chevron-right"></i></button>',
        responsive: [
            {
                breakpoint: 1501,
                settings: {
                    slidesToShow: 1,
                }
            },
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 1,
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 1,
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                }
            },
            {
                breakpoint: 575,
                settings: {
                    slidesToShow: 1,
                }
            },
        ]
    });
    // Brand Slider
    $('.brand-slider').slick({
        infinite: true,
        arrows: false,
        dots: true,
        slidesToShow: 5,
        slidesToScroll: 5,
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 479,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            }
        ]
    });
    // Work Slider
    $('.product-slider').slick({
        infinite: true,
        arrows: true,
        dots: false,
        slidesToShow: 4,
        slidesToScroll: 1,
        prevArrow: '<button class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 1200,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    arrows: false,
                    autoplay: true
                }
            },
        ]
    });
    $('.slider-lg-image-1').slick({
        infinite: true,
        arrows: true,
        dots: false,
        slidesToShow: 4,
        slidesToScroll: 1,
        prevArrow: '<button class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4
                }
            },
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 479,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            }
        ]
    });
    /* Blog Slider Active */
    $('.blog-gallery').slick({
        infinite: true,
        arrows: false,
        dots: false,
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        prevArrow: '<button class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });

    /*----------------------------------- 
        Count Down Active 
    ----------------------------------*/
    $('[data-countdown]').each(function () {
        var $this = $(this),
            finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function (event) {
            $this.html(event.strftime('<div class="single-countdown big-font"><span class="single-countdown-time">%D</span><span class="single-countdown-text">days,</span></div><div class="single-countdown"><span class="single-countdown-time">%H</span><span class="single-countdown-text">h</span></div><div class="single-countdown"><span class="single-countdown-time">%M</span><span class="single-countdown-text">m</span></div><div class="single-countdown"><span class="single-countdown-time">%S</span><span class="single-countdown-text">s</span></div>'));
        });
    });
    $('[data-countdown2]').each(function () {
        var $this = $(this), finalDate = $(this).data('countdown2');
        $this.countdown(finalDate, function (event) {
            $this.html(event.strftime('<div class="single-count"><span class="single-countdown-times">%D</span><span class="single-countdown-content">Days</span></div><div class="single-count"><span class="single-countdown-times">%H</span><span class="single-countdown-content">Hours</span></div><div class="single-count"><span class="single-countdown-times">%M</span><span class="single-countdown-content">Mins</span></div><div class="single-count"><span class="single-countdown-times">%S</span><span class="single-countdown-content">Secs</span></div>'));
        });
    });

    /*----------------------------------
        ScrollUp Active
    -----------------------------------*/
    $.scrollUp({
        scrollText: '<i class="fa fa-angle-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade'
    });

    /*------------------------
        Sticky Sidebar Active
    -------------------------*/
    $('#sticky-sidebar').theiaStickySidebar({
        // Settings
        additionalMarginTop: 120
    })

    /*----- 
        Quantity
    --------------------------------*/
    $('.pro-qty').prepend('<button class="dec qtybtn">-</button>');
    $('.pro-qty').append('<button class="inc qtybtn">+</button>');
    $('.qtybtn').on('click', function () {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
    });
    /* -------------------------
        Venobox Active
    * --------------------------*/
    $('.venobox').venobox({
        border: '10px',
        titleattr: 'data-title',
        infinigall: true
    });
    /* --------------------------------------------------------
        FAQ Accordion 
    * -------------------------------------------------------*/
    $('.card-header a').on('click', function () {
        $('.card').removeClass('actives');
        $(this).parents('.card').addClass('actives');
    });

    /*----- 
        Shipping Form Toggle
    --------------------------------*/
    $('[data-shipping]').on('click', function () {
        if ($('[data-shipping]:checked').length > 0) {
            $('#shipping-form').slideDown();
        } else {
            $('#shipping-form').slideUp();
        }
    });

    /*----- 
        Payment Method Select
    --------------------------------*/
    $('[name="payment-method"]').on('click', function () {

        var $value = $(this).attr('value');

        $('.single-method p').slideUp();
        $('[data-method="' + $value + '"]').slideDown();

    });

    /*-----
        Wishlist adding/deleting product
    --------------------------------*/
    $('.wishlist-btn').click(function () {
        var _this = $(this);
        $.ajax({
            url: '/Account/Wishlist',
            method: 'POST',
            data: { productId: _this.data('product-id') },
            success: function (data) {
                if (data.success) {
                    var msg;
                    if (data.isDeleted) {
                        msg = 'Product removed from Favourites!';
                        _this.removeClass('active');
                    } else {
                        msg = 'Product added to Favourites!';
                        _this.addClass('active');
                    }
                    Swal.fire({
                        title: 'Success',
                        type: 'success',
                        html: msg
                    });
                } else {
                    Swal.fire({
                        title: 'Error',
                        type: 'error',
                        html: 'When saving to favourites an error occured!'
                    });
                }
            }
        });
    });

    $('.add-to-cart-btn').click(function () {
        var _this = $(this);
        $.ajax({
            url: '/Shop/AddToCart',
            method: 'POST',
            data: { itemId: _this.data('item-id') },
            success: function (data) {
                if (data.success) {
                    Swal.fire({
                        title: 'Success',
                        type: 'success',
                        html: data.message
                    });
                } else {
                    Swal.fire({
                        title: 'Error',
                        type: 'error',
                        html: data.message
                    });
                }
            }
        });
    });

    $('.remove-from-cart').click(function () {
        var _this = $(this);
        $.ajax({
            url: '/Shop/RemoveFromCart',
            method: 'POST',
            data: { itemId: _this.data('item-id') },
            success: function (data) {
                if (data.success) {
                    Swal.fire({
                        title: 'Success',
                        type: 'success',
                        html: data.message,
                        showConfirmButton: false,
                        timer: 1500
                    });
                } else {
                    Swal.fire({
                        title: 'Error',
                        type: 'error',
                        html: data.message,
                        showConfirmButton: false,
                        timer: 1500
                    });
                }
            }
        });
    });

    /*--
        MailChimp
    -----------------------------------*/
    $('#mc-form').ajaxChimp({
        language: 'en',
        callback: mailChimpResponse,
        // ADD YOUR MAILCHIMP URL BELOW HERE!
        url: 'http://devitems.us11.list-manage.com/subscribe/post?u=6bbb9b6f5827bd842d9640c82&amp;id=05d85f18ef'

    });
    function mailChimpResponse(resp) {

        if (resp.result === 'success') {
            $('.mailchimp-success').html('' + resp.msg).fadeIn(900);
            $('.mailchimp-error').fadeOut(400);

        } else if (resp.result === 'error') {
            $('.mailchimp-error').html('' + resp.msg).fadeIn(900);
        }
    }

})(jQuery);	