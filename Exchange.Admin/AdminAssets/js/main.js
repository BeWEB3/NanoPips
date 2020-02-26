"use strict";

/*demo to do list*/
$('#form-todolist').on('submit', function(e){
  e.preventDefault();
  today = new Date();
  var dateTime= today.getDate()+'-'+(today.getMonth()+1)+'-'+today.getFullYear()+' '+today.getHours() + ":" + today.getMinutes() + ":" + today.getSeconds();
  var a ="";
    a+='<a href="#" class="list-group-item list-group-item-action flex-column align-items-start">';
      a+='<div class="d-flex w-100 justify-content-between">';
        a+='<div>'+$('#text_todolist').val()+'</div>';
        a+='<button type="button" class="close btn btn-del-todolist" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
      a+='</div>';
      a+='<small>'+dateTime+'</small>';
    a+='</a>';
    $('.list-todo').append(a);
    $('#text_todolist').val("");
  $('.btn-del-todolist').on('click', function(){
    $(this).parent().parent().remove();
  });
});
  $('.btn-del-todolist').on('click', function(){
    $(this).parent().parent().remove();
  });
/*end demo to do list*/

$('.btn-layout-theme_light').on('click', function(){
  $('.views').removeClass('sidebar-dark');
});
$('.btn-layout-theme_dark').on('click', function(){
  $('.views').addClass('sidebar-dark');
});

var theme_used = ["blue", "primary", "indigo", "purple", "pink", "red", "danger", "orange", "yellow", "warning", "green", "success", "teal", "cyan", "info", "white", "dark"];
if($('.views').attr('class') !== undefined){
  var find_theme = $('.views').attr('class').split(" ");
  for (var i = 0; i < theme_used.length; i++) {
    for (var j = 0; j < find_theme.length; j++) {
      if (find_theme[j] === "theme-"+theme_used[i]) {
        var theme_used = "theme-"+theme_used[i];
      }
    }
    }  
}
$('.gg-skins__btn').on('click', function(){
  $('.views').removeClass(theme_used);
  var bg=$(this).attr('class').split('gg-skins__btn');
  bg = bg[1].split('bg-');
  theme_used = "theme-"+bg[1];
  $(".views").addClass("theme-"+bg[1]);
});

$('.check-navbar-shadow').on('change', function(){
 $('.views .navbar').toggleClass('shadow-1');
});
$('.check-brand').on('change', function(){
 $('.navbar-brand').toggleClass('brand-1');
});
$('.check-fixed-layout').on('change', function(){
 $('.views .navbar').toggleClass('fixed-top sticky-top');
});

$('.check-fixed-navbar-header').on('change', function(){
 $('.views .navbar').toggleClass('hold-position');
});

$('.check-toggle-sidebar').on('change', function(){
 $('.views').toggleClass('toggle_sidebar');
});

$('input[id-search="input-search-list"]').keyup(function(){
  var filter, ul, li, content,no = 0;
  filter = $(this).val().toUpperCase();
  ul = $(this).attr("data-search");
  li = $("."+ul).children(".list-group-item");
  for (var i = 0; i < li.length; i++) {
      content = $("."+ul).children(".list-group-item")[i];
      data = content.innerHTML.toUpperCase().indexOf(filter);
      if ( data > -1) {
        li[i].style.display="";
        $('.list-search-not-found').css("display","none");
        no +=1;
      }else{
        $('.list-search-not-found').css("display","none");
        li[i].style.display="none";
      }
      if (no === 0) {
        $('.list-search-not-found').css("display","block");
      }
    }
});