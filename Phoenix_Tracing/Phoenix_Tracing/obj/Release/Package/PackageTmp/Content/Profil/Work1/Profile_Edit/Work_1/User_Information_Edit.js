function UserInformationEdit() {
    var _userName = $("#userName").val();
    var _userSurname = $("#userSurname").val();
    var _userPhone = $("#userPhone").val();
    var _userMail = $("#userMail").val();

    $.ajax({
        url: "/xProfileEdit/UserInformationEdit",
        type: "POST",
        dataType: "json",
        data: {
            'name': _userName,
            'surname': _userSurname,
            'phone': _userPhone,
            'mail': _userMail
        },
        success: function (result) {
            //alert("Değişiklik Yapıldı");
            window.location.reload();
        }
    });
}

$("#btnUserEdit").click(function (e) {
    e.preventDefault();
    UserInformationEdit();
});