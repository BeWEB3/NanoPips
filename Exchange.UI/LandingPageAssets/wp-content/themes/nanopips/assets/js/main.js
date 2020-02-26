jQuery(document).ready(function ($) {
    "use strict";

    /*-------------------------------------------------------------------------------
    javaScript for tab
    -------------------------------------------------------------------------------*/
    $('.tab-a').click(function () {
        $(".tab").removeClass('tab-active');
        $(".tab[data-id='" + $(this).attr('data-id') + "']").addClass("tab-active");
        $(this).parent().find(".tab-a").addClass('active-a');
    });

    // $(".tab-a").on("click", function () {
    //     $(".tab-a").removeClass("active-a");
    //     $(".tab-a").addClass("active-a");
    // });
});