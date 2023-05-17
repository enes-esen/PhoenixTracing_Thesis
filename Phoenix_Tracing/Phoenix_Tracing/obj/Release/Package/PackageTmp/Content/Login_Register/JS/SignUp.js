$(function () {
    $("#SignUp").click(function () {
        var _name = $("#reg_name").val();
        var _surname = $("#reg_surname").val();
        var _mail = $("#reg_mail").val();
        var _password = $("#reg_pass").val();
        var _phone = $("#reg_phone").val();
        $.ajax({
            url: '/Login/Register',
            type: 'POST',
            dataType: 'json',
            data: {
                "mail": _mail,
                "pass": _password,
                "name": _name,
                "surname": _surname,
                "phone": _phone
            },
            success: function (data) {
                //$("mail").html(data.email)
                //$("pass").html(data.pass)
                //$("name").html(data.name)
                //$("surname").html(data.surname)
                //$("phone").html(data.phone)
                window.location.reload();
            }

        });
    });
});
//$('#SignUp').click(function (e) {
//    e.preventDefault();
    
//});