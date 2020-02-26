"use strict";

$('.btn-toggle-navbar-head').on('click', function(){
  $('.navbar-head').slideToggle();
});
/*toggle dropdown nav item*/
$('.layout-default .nav-toggle .nav-link, .layout-1 .nav-toggle .nav-link').on('click',function(){
  if($(this).parent('li').attr('class') !== undefined ){
    $(this).parent('li').toggleClass('is_slide');
    $(this).parent('li').children('ul').slideToggle();
  }
});

$('.views .nav-toggle .nav-link.active').map(function(i) {
  $(this).parent().children('.nav-dropdown').children('li .nav-link').css("border-left","3px solid #000");
  $(this).parent().addClass("is_slide");
  $(this).parent().children("ul").slideDown();
}).get();

/*end toggle dropdown nav item*/

/*sidebar toggle*/
$('.sb-toggle').on('click', function(){
  $(".views").toggleClass("toggle_sidebar");
});
var overflow_sb_right = true;

var w = $('.sidebar-right').width();
$('.sidebar-right').css({
  "right":"-"+w+"px",
  "opacity":"1"
});
$(window).resize(function(){
  if ($(this).width() > w ) {
    $('.sidebar-right').css({
      "width":"300px",
      "right":"-"+w+"px",
      "opacity":"1"
    });
  }
});
$('[data-toggle="sidebar-right"]').on('click', function(){
  $('.sidebar-right').animate({right:'0'});
  if (overflow_sb_right === true) {
    overflow_sb_right = false;
    $('.views').append('<div class="overflow-sidebar-right"></div>');
    $('.overflow-sidebar-right').on('click', function(){
      $('.sidebar-right').animate({right:'-'+ w});
      overflow_sb_right = true;
      $('.overflow-sidebar-right').remove();
    });
  }else{
    overflow_sb_right = true;
    $('.overflow-sidebar-right').remove();
  }
});


/*end sidebar toggle*/

var t_overflow = true;
$('.navbar-toggle').on('click', function(){
  $(".views").toggleClass("toggle_sidebar");
  if (t_overflow === true) {
    t_overflow = false;
    $('.views').append('<div class="toggle-overflow"></div>');
    $('.toggle-overflow').on('click', function(){
      $(".views").toggleClass("toggle_sidebar");
      t_overflow = true;
      $('.toggle-overflow').remove();
    });

  }else{
    t_overflow = true;
    $('.toggle-overflow').remove();
  }
});


/*fab*/
$('.floating-btn').on('click', function(){
  $(this).toggleClass("is_open");
  $(this).parent().children('.fab-menu').toggleClass("fab_menu_show");
  $(this).children('.fab-open').css({
    "-webkit-animation": "fab_close 0.5s ease",
      "animation": "fab_close 0.5s ease"
  });
});
/*end fab*/

/*   MAD-RIPPLE  (jQ+CSS)*/
jQuery(function($) {
  $(document).on("mousedown", ".wave", function(e) {
    var $self = $(this);
    if($self.is(".btn-disabled")) {
      return;
    }
    if($self.closest("[wave-color]")) {
      e.stopPropagation();
    }
    var initPos = $self.css("position"),
        offs = $self.offset(),
        x = e.pageX - offs.left,
        y = e.pageY - offs.top,
        dia = Math.min(this.offsetHeight, this.offsetWidth, 100),
        $ripple = $('<div/>', {class:"ripple", appendTo:$self});
    if(!initPos || initPos==="static") {
      $self.css({position:"relative"});
    }
    $('<div/>', {
      class : "rippleWave",
      css : {
        background: $self.data("ripple"),
        width: dia,
        height: dia,
        left: x - (dia/2),
        top: y - (dia/2),
      },
      appendTo : $ripple,
      one : {
        animationend : function(){
          $ripple.remove();
        }
      }
    });
  });
});
/*popovers and tooltips*/
$(function () {
  $('[data-toggle="popover"]').popover({
     container: 'body'
  });
});
$(function () {
  $('[data-toggle="tooltip"]').tooltip()
});
/*end popovers*/

/*float form material*/
  function float_form(){
    $('.label-float-form').css({
      "top" :"-16px",
      "color" :"#007bff",
      "font-size": "12px", 
      "transition": "0.3s"
    });
  }
  function normal_form(){
    $('.label-float-form').css({
      "position": "absolute",
       "left": "0", 
       "width": "100%",
        "font-size": "unset", 
        "top": "9px",
        "color": "#777", 
        "transition": "0.3s", 
        "z-index": "-1"
    });    
  }
  function float_form_material(){
    if ($('.float-form-material .form-control').val()!=="") {
      float_form();
    }else{
      normal_form();
    }
  }
  $(".float-form-material").map(function(key) {
    var kelas = $(this).children('.form-control').val();
    if (kelas !=="") {
      $(this).children('.label-float-form').css({
      "top" :"-16px",
      "color" :"#007bff",
      "font-size": "12px", 
      "transition": "0.3s"
    });
    }else{
      $(this).children('.label-float-form').css({
      "position": "absolute",
       "left": "0", 
       "width": "100%",
        "font-size": "unset", 
        "top": "9px",
        "color": "#777", 
        "transition": "0.3s", 
        "z-index": "-1"
    });
    }
  }).get();

  $('.float-form-material .form-control').on({
    focus: function(data){
    $(this).parent().children(".label-float-form").css({
      "top" :"-16px",
      "color" :"#007bff",
      "font-size": "12px", 
      "transition": "0.3s"
    });
    }, 
    focusout: function(data){
      if ($(this).val()=== "") {
        $(this).parent().children(".label-float-form").css({
          "position": "absolute",
           "left": "0", 
           "width": "100%",
            "font-size": "unset", 
            "top": "9px",
            "color": "#777", 
            "transition": "0.3s", 
            "z-index": "-1"
        });
      }
    }, 
    change:function(data){
    $(this).parent().children(".label-float-form").css({
      "top" :"-16px",
      "color" :"#007bff",
      "font-size": "12px"
    });
    }
  });
/*end float form material*/

function txt_area(a) {
  a.style.height = "auto";
  a.style.height = (a.scrollHeight)+"px";
}
$('.write-message').keyup(function(){
  txt_area(this);
});