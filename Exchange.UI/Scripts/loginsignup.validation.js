<script src="~/Scripts/jquery.validate.new.js"></script>
<script src="~/Scripts/jquery-1.10.2.min.js"></script>
<script src="~/Scripts/jquery.validate.min.js"></script>
<script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>

<script type="text/javascript">

    $("#l-form1").submit(function (e) {
    var form = $(this);
    form.validate();
if (form.valid()) {}
    });
$("#l-form2").submit(function (e) {
    var form = $(this);
    form.validate();
if (form.valid()) {}
    });
        
$("#s-form1").submit(function (e) {
        e.preventDefault();
    if ($('#termsCondition').is(':checked')) {
        $('.error-message').text('');
    var form = $(this);
    form.validate();
    if (form.valid()) {
        $.post(form.attr('action'), form.serialize(), function (msg) {
            if (msg == "reCaptcha validation failed, please try again") {
                $("#rec").text(msg);
            }
            else {
                $("#s-form1").html("<h6 >" + msg + "</h6><br>")
            }
        });
    }
    else e.preventDefault();
}
else {
        e.preventDefault();
    $('.error-message').text('Please accept the Term & Conditions');
}
});

$("#s-form2").submit(function (e) {
        e.preventDefault();
    if ($('#termsCondition2').is(':checked')) {
        $('.error-message').text('');
    var form = $(this);
    form.validate();
    if (form.valid()) {
        $.post(form.attr('action'), form.serialize(), function (msg) {
            $("#s-form2").html("<h6 >" + msg + "</h6><br>")
        });
    }
    else e.preventDefault();
}
else {
        e.preventDefault();
    $('.error-message').text('Please accept the Term & Conditions');
}
});


$("#f-form1").submit(function (e) {
        e.preventDefault();
    var form = $(this);
    form.validate();
    if (form.valid()) {
        var email = $("#forgotEmail1").val();
        $.post(form.attr('action'), form.serialize(), function (msg) {
        $("#f-form1").html("<h6 >" + msg + "</h6><br>")
    });
}

})
    $("#f-form2").submit(function (e) {
        e.preventDefault();
    var form = $(this);
    form.validate();
    if (form.valid()) {
        var email = $("#forgotEmail2").val();
        $.post(form.attr('action'), form.serialize(), function (msg) {
        $("#f-form2").html("<h6 >" + msg + "</h6><br>")
    });
}
})


$("#ae").hide();
$("#email").blur(function () {
var value = $(this).val();
$.get("@Url.Action("CheckEmail", "Account")", {email: value }, function (flag) {
    if (flag == "True") {
        $("#ae").show();
    $("#email").val("");
}
    else {
        $("#ae").hide();
    }
})
})

</script>