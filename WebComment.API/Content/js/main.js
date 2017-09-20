(function($) {
  "use strict";

  /*Scripts initialization*/

  $(window).load(function() {

    init_scroll_navigate();

    $(window).trigger("scroll");
    $(window).trigger("resize");
  });

  $(document).ready(function() {
    $(window).trigger("resize");

    initPageSliders();
    init_lazyLoad();
    init_masonry();
    handleFancybox();
    chosenSelect();
    goToTop.init();
    init_choose_color();
    handlePerfectscroll();
    handleTabs();
    ttcdNav();
    init_wow();
  });

  $(window).resize(function() {

  });


  /*Platform detect*/

  var mobileTest;

  if (/Android|webOS|iPhone|iPad|iPod|BlackBerry/i.test(navigator.userAgent)) {
    mobileTest = true;
    $("html").addClass("mobile");
  } else {
    mobileTest = false;
    $("html").addClass("no-mobile");
  }

  var mozillaTest;
  if (/mozilla/.test(navigator.userAgent)) {
    mozillaTest = true;
  } else {
    mozillaTest = false;
  }
  var safariTest;
  if (/safari/.test(navigator.userAgent)) {
    safariTest = true;
  } else {
    safariTest = false;
  }

  // Detect touch devices
  if (!("ontouchstart" in document.documentElement)) {
    document.documentElement.className += " no-touch";
  }

  //Fuction for block height 100%
  function height_line(height_object, height_donor) {
    height_object.height(height_donor.height());
    height_object.css({
      "line-height": height_donor.height() + "px"
    });
  }

  // Function equal height
  ! function(a) {
    a.fn.equalHeights = function() {
      var b = 0,
        c = a(this);
      return c.each(function() {
        var c = a(this).innerHeight();
        c > b && (b = c)
      }), c.css("height", b)
    }, a("[data-equal]").each(function() {
      var b = a(this),
        c = b.data("equal");
      b.find(c).equalHeights()
    })
  }(jQuery);
  //
  // /*Sticky search*/
  //
  // $("#sticky-search-bar").sticky({
  //   topSpacing: 56,
  //   className: "havesticker"
  // });
  //
  // $('#sticky-search-bar').on('sticky-start', function() {
  //   main_nav.addClass("no-shadow");
  // });
  //
  // $('#sticky-search-bar').on('sticky-end', function() {
  //   main_nav.removeClass("no-shadow");
  // });


  // $("#detail-menu").sticky({
  //   topSpacing: 0,
  //   className: "havesticker-detail"
  // });

  // $(".main-navigation").sticky({
  //   topSpacing: 0,
  // });



  /*Collapse */

  $('#toichon-content').on('show.bs.collapse', function(e) {
    $("#detail-buttons-uudai").slideUp();
    $('.scroll').perfectScrollbar('update');
  });

  $('#toichon-content').on('shown.bs.collapse', function(e) {
    $('.scroll').perfectScrollbar('update');
  });

  $('#toichon-content').on('hide.bs.collapse', function(e) {
    $("#detail-buttons-uudai").slideDown();
  });

  $('#combo-accordion').find(".panel-collapse").on('show.bs.collapse', function(e) {
    e.stopPropagation();
  })

  $('#combo-accordion').find(".panel-collapse").on('hide.bs.collapse', function(e) {
    e.stopPropagation();
  })

  /*************************  $One Page Scroll  **************************/
  $("#detail-menu").find(".list-inline").onePageNav({
    currentClass: 'active',
    filter: ':not(.exclude)',
  });



  /*Thanh toan poopup form*/
  $('input[name=thanhtoan]').on('change', function() {
    if (!this.checked) return;

    if ($(this).hasClass("show-type")) {
      $('#popup-payment-type').slideDown();
    } else {
      $('#popup-payment-type').slideUp();
    }
  });

    $('input[name=choosePayment]').on('change', function() {
        if (!this.checked) return;
        if ($(this).hasClass("choose-visa")) {
            $('#choose-visa-info').slideDown();
        } else {
            $('#choose-visa-info').slideUp();
        }
    });
  // Giao hang form
  $('input[name=giaohang]').on('change', function() {
    if (!this.checked) return;

    if ($(this).hasClass("giao-hang")) {
      $('#form-giaohang').fadeIn();
    } else {
      $('#form-giaohang').hide();
    }

    if ($(this).hasClass("nhan-hang")) {
      $('#form-nhanhang').fadeIn();
    } else {
      $('#form-nhanhang').hide();
    }
  });



  /*Thanh toan poopup form*/
  $('input[name=httt]').on('change', function() {
    if (!this.checked) return;

    var name = $(this).data("httt");
    $(".combo-httt").hide();
    $("." + name).fadeIn();
  });




  /*Popup search close when click outside*/
  $(document).on("click touch", function(e) {
    if (!$(e.target).parents().addBack().is('#support-popup')) {
      $(".call-back").removeClass("show");
      $(".support-online").removeClass("show");
    }
  });


  /*Go to top*/

  var goToTop = {
    goToTopEl: $('#gotoTop'),

    init: function() {
      goToTop.goToTopEl.click(function() {
        $('body,html').stop(true).animate({
          scrollTop: 0
        }, 400);
        return false;
      });

      $(window).scroll(function() {
        if ($(window).scrollTop() > 450) {
          goToTop.goToTopEl.fadeIn();
        } else {
          goToTop.goToTopEl.fadeOut();
        }
      });
    },
  };


  //initialize datepicker
  $('.date-picker').datepicker({
    autoclose: true,
    language: 'vi'
  });





  /*Dropdown perfectscroll init*/

  $('.product-pop-up-toichon').on('shown.bs.dropdown', function() {
    $('.scroll').perfectScrollbar('update');
  })


  $("#popup-bottom-left").find(".exit").on("click", function() {
    $("#popup-bottom-left").fadeOut("fast");
  });

  $("#trainghiem").find(".exit").on("click", function() {
    $("#trainghiem").fadeOut("fast");
  });

  $("div.raty").raty({
    cancel: true,
    cancelOff: 'images/raty/cancel-off.png',
    cancelOn: 'images/raty/cancel-on.png',
    starHalf: 'images/raty/star-half.png',
    starOff: 'images/raty/star-off.png',
    starOn: 'images/raty/star-on.png'
  });

  $(".comment-answer").on("click", function(e) {
    e.preventDefault();
    $(this).parent().parent().siblings(".sub-comment-input").slideToggle("fast");
  });



  /*cho trang doanh nghiep, remove it don't use*/

  $(".dn-ctkm").find(".view-more").on("click", function(e) {
    e.preventDefault();

    $(this).toggleClass("active");
    $(this).parent().parent().siblings(".item-detail").slideToggle("fast");
  });

  /*end cho trang doanh nghiep, remove it don't use*/




})(jQuery); // End of use strict

/*Lazy load*/
function init_lazyLoad() {
  $("img.lazy").lazyload({
    threshold: 500,
    effect: "fadeIn",
    placeholder: ""
  });
}

/*Perfect scroll*/
/*Global search*/
//$('.scroll').perfectScrollbar();


function handlePerfectscroll() {
  $('.scroll').each(function() {
    $(this).perfectScrollbar({
      suppressScrollX: true
    });
  });
}

/*Active tab if tab id provided in the URL*/
function handleTabs() {
  if (location.hash) {
    var tabid = location.hash.substr(1);
    $('a[href="#' + tabid + '"]').click();
    $(window).scrollTop(0);
  }

  $(window).on('hashchange', function() {
    var tabid = location.hash.substr(1);
    $('a[href="#' + tabid + '"]').click();
    $(window).scrollTop(0);
  });
}


/*Masonry layout*/
function init_masonry() {
  (function($) {
    var $shopMasonry = $('.shop-masonry').masonry({
      // options
      itemSelector: '.masonry-item',
      columnWidth: '.masonry-sizer',
    });
  })(jQuery);
}



/* ---------------------------------------------
 Icon choose color
 --------------------------------------------- */
function init_choose_color() {
  "use strict";
  var iconcolor = $(".detail-product .product-color:not(.disable)");
  var imageChange = $(".detail-product").find(".detail-img img");
  iconcolor.on("click", function() {

    if ($(this).hasClass("selected")) iconcolor.each(function() {
      $(this).sibling.removeClass("selected");
    });
    else {
      iconcolor.each(function() {
          $(this).removeClass("selected");
      });
        $(this).addClass("selected");
    }
    
    var url;
    var color = $(this).data("href");
    var colorid = $(this).data("idcolor");
    var productid = $('#productId').val();
    $.ajax({
        type: 'get',
        data: { "id": productid, "color": color },
        url: '/Detail/GetImageByColor',
        success: function (data) {
            try {
                url = data;
                $('#ColorProduct').val(colorid);
            } catch (e) {
            }
            imageChange.prop("src", url);
        }

    });
    return false;
  });

}




/* ---------------------------------------------
 Scroll navigation
 --------------------------------------------- */

function init_scroll_navigate() {


  $(".local-scroll").localScroll({
    target: "body",
    duration: 1500,
    offset: 0
  });

  $(".local-scroll2").localScroll({
    target: "body",
    duration: 1500,
    offset: -50
  });

}




/*Fancy box*/
var handleFancybox = function() {
  if (!jQuery.fancybox) {
    return;
  }

  $(".fancybox-fast-view").fancybox({
    padding: [5, 5, 5, 5],
    tpl: {
      error: '<p class="fancybox-error">Nội dung không hiển thị<br/>Vui lòng xem lại.</p>',
      closeBtn: '<a title="Đóng cửa sổ" class="pop-up-close" href="javascript:;"><i class="vta-close"></i></a>'
    }
  });

    $(".fancybox").fancybox({
        openEffect	: 'none',
        closeEffect	: 'none'
    });

  $(".fancybox-store").fancybox({
    prevEffect: 'none',
    nextEffect: 'none',
    title: '',
    helpers: {

    },
    tpl: {
      error: '<p class="fancybox-error">Nội dung không hiển thị<br/>Vui lòng xem lại.</p>',
      closeBtn: '<a title="Đóng cửa sổ" class="pop-up-close" href="javascript:;"><i class="vta-close"></i></a>'
    }
  });

  if ($(".fancybox-button").size() > 0) {
    $(".fancybox-button").fancybox({
      groupAttr: 'data-rel',
      prevEffect: 'none',
      nextEffect: 'none',
      closeBtn: true,
      helpers: {
        title: {
          type: 'inside'
        }
      }
    });

    $('.fancybox-video').fancybox({
      type: 'iframe'
    });
  }

}





/*Chosen input*/
function chosenSelect() {
  $(".chosen-select").chosen({
    no_results_text: "Không tìm thấy kết quả",
    placeholder_text_single: "fewfewfewf"
  });

  $(".chosen-select-n").chosen({
    no_results_text: "Không tìm thấy kết quả",
    disable_search: true
  });
}

/*Thong tin co dong*/

function ttcdNav() {
  var nav = $("#ttcd-nav");
  var content = $('.ttcd-content');
  $(".ttcd-item").on("click", function(e) {
    e.preventDefault();
    $(window).scrollTop(0);
    var item = $(this).attr("href");
    $(item).fadeIn("fast");
    nav.fadeOut(0);
  });

  $(".ttcd-back").on("click", function(e) {

    $(window).scrollTop(0);
    e.preventDefault();
    nav.fadeIn("fast");
    content.fadeOut(0);
  });
}




/*Sliders*/
function initPageSliders() {
  (function($) {
    "use strict";

    $('.product-carousel').owlCarousel({
      autoPlay: 10000,
      stopOnHover: true,
      slideSpeed: 1000,
      items: 4,
      itemsDesktop: [1199, 3],
      itemsDesktopSmall: [991, 2],
      itemsTablet: [767, 2],
      itemsTabletSmall: [480, 1],
      itemsMobile: [320, 1],
      navigation: true,
      scrollPerPage: true,
      pagination: false,

      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"]
    });

    var dacdiem_carousel = $("#dac-diem-carousel");

    dacdiem_carousel.owlCarousel({
      singleItem: true,
      slideSpeed: 1000,
      lazyLoad: true,
      navigation: true,
      pagination: true,
      responsiveRefreshRate: 200,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"]
    });


    $(".huongdan-carousel").owlCarousel({
      singleItem: true,
      slideSpeed: 1000,
      lazyLoad: true,
      navigation: true,
      pagination: false,
      responsiveRefreshRate: 200,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"]
    });

    $("#phukien-carousel").owlCarousel({
      autoPlay: false,
      stopOnHover: true,
      navigation: false,
      pagination: true,
      lazyLoad: true,
      paginationSpeed: 1000,
      singleItem: true,
      autoHeight: true,
      transitionStyle: "fade"
    });


    var mediaimg_sync1 = $("#media-img-sync1");
    var mediaimg_sync2 = $("#media-img-sync2");

    mediaimg_sync1.owlCarousel({
      singleItem: true,
      slideSpeed: 1000,
      navigation: true,
      pagination: false,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"],
      afterAction: syncPosition2
    });

    mediaimg_sync2.owlCarousel({
      items: 6,
      itemsDesktop: [1199, 6],
      itemsDesktopSmall: [979, 5],
      itemsTablet: [768, 5],
      itemsMobile: [479, 3],
      pagination: false,
      afterInit: function(el) {
        el.find(".owl-item").eq(0).addClass("synced");
      }
    });

    function syncPosition2(el) {
      var current = this.currentItem;
      mediaimg_sync2
        .find(".owl-item")
        .removeClass("synced")
        .eq(current)
        .addClass("synced")
      if (mediaimg_sync2.data("owlCarousel") !== undefined) {
        center2(current)
      }
    }

    mediaimg_sync2.on("click", ".owl-item", function(e) {
      e.preventDefault();
      var number = $(this).data("owlItem");
      mediaimg_sync1.trigger("owl.goTo", number);
    });

    function center2(number) {
      var sync2visible = mediaimg_sync2.data("owlCarousel").owl.visibleItems;
      var num = number;
      var found = false;
      for (var i in sync2visible) {
        if (num === sync2visible[i]) {
          var found = true;
        }
      }

      if (found === false) {
        if (num > sync2visible[sync2visible.length - 1]) {
          mediaimg_sync2.trigger("owl.goTo", num - sync2visible.length + 2)
        } else {
          if (num - 1 === -1) {
            num = 0;
          }
          mediaimg_sync2.trigger("owl.goTo", num);
        }
      } else if (num === sync2visible[sync2visible.length - 1]) {
        mediaimg_sync2.trigger("owl.goTo", sync2visible[1])
      } else if (num === sync2visible[0]) {
        mediaimg_sync2.trigger("owl.goTo", num - 1)
      }
    }



    var mediavideo_sync1 = $("#media-video-sync1");
    var mediavideo_sync2 = $("#media-video-sync2");

    mediavideo_sync1.owlCarousel({
      singleItem: true,
      slideSpeed: 1000,
      navigation: true,
      pagination: false,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"],
      afterAction: syncPosition3
    });

    mediavideo_sync2.owlCarousel({
      items: 6,
      itemsDesktop: [1199, 6],
      itemsDesktopSmall: [979, 5],
      itemsTablet: [768, 5],
      itemsMobile: [479, 3],
      pagination: false,
      afterInit: function(el) {
        el.find(".owl-item").eq(0).addClass("synced");
      }
    });

    function syncPosition3(el) {
      var current = this.currentItem;
      mediavideo_sync2
        .find(".owl-item")
        .removeClass("synced")
        .eq(current)
        .addClass("synced")
      if (mediavideo_sync2.data("owlCarousel") !== undefined) {
        center3(current)
      }
    }

    mediavideo_sync2.on("click", ".owl-item", function(e) {
      e.preventDefault();
      var number = $(this).data("owlItem");
      mediavideo_sync1.trigger("owl.goTo", number);
    });

    function center3(number) {
      var sync2visible = mediavideo_sync2.data("owlCarousel").owl.visibleItems;
      var num = number;
      var found = false;
      for (var i in sync2visible) {
        if (num === sync2visible[i]) {
          var found = true;
        }
      }

      if (found === false) {
        if (num > sync2visible[sync2visible.length - 1]) {
          mediavideo_sync2.trigger("owl.goTo", num - sync2visible.length + 2)
        } else {
          if (num - 1 === -1) {
            num = 0;
          }
          mediavideo_sync2.trigger("owl.goTo", num);
        }
      } else if (num === sync2visible[sync2visible.length - 1]) {
        mediavideo_sync2.trigger("owl.goTo", sync2visible[1])
      } else if (num === sync2visible[0]) {
        mediavideo_sync2.trigger("owl.goTo", num - 1)
      }
    }




    $("#news-carousel").owlCarousel({
      slideSpeed: 300,
      paginationSpeed: 400,
      singleItem: true,
      navigation: true,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"],
      lazyload: true,
      pagination: true,
      autoPlay: 6000,
      transitionStyle: "fade"
    });

      $(".news-carousel").owlCarousel({
          slideSpeed: 300,
          paginationSpeed: 400,
          singleItem: true,
          navigation: true,
          navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"],
          lazyload: true,
          pagination: false,
          autoPlay: 6000,
          transitionStyle: "fade"
      });



    $('.banner-main').owlCarousel({
      slideSpeed: 300,
      paginationSpeed: 400,
      singleItem: true,
      navigation: true,
      pagination: false,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"]
    });


    /*Cho trang doanh nghiep, remove it don't use*/
    $("#dn-slide").owlCarousel({

      slideSpeed: 300,
      paginationSpeed: 400,
      singleItem: true,
      navigation: false,
      autoPlay: true
    });

    var dnList = $("#dn-list");

    dnList.owlCarousel({
      itemsCustom: [
        [0, 1],
        [450, 2],
        [600, 3],
        [700, 4],
        [1000, 5],
        [1200, 6],

      ],
      navigation: true,
      pagination: false,
      autoPlay: 3000,
      navigationText: ["<div class=''><i class='vta-arrow-left'></i></div>", "<div class=''><i class='vta-arrow-right'></i></div>"]
    });
    /*End trang doanh nghiep, remove it don't use*/


  })(jQuery);


}


$(function() {
  $('[data-toggle="tooltip"]').tooltip({
    title: true
  })
})



/* ---------------------------------------------
 WOW animations
 --------------------------------------------- */

function init_wow() {
  (function($) {

    var wow = new WOW({
      boxClass: 'wow',
      animateClass: 'animated',
      offset: 90,
      mobile: false,
      live: true
    });

    /*if ($("body").hasClass("appear-animate")){
        wow.init();
    }*/

    wow.init();

  })(jQuery);
  $('.ctmuahang').on('shown.bs.tab', function(e) {
    $(window).scrollTop($(window).scrollTop() + 1);
  })

}
