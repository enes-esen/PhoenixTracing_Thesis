function Contacts() {
    var _contact_fullName = $("#contact_fullName").val();
    var _contact_mail = $("#contact_mail").val();
    var _contact_phone = $("#contact_phone").val();
    var _contact_message_title = $("#contact_message_title").val();
    var _contact_message = $("#contact_message").val();
    $.ajax({
        url: '/Changless/SendDataContacts',
        type: 'POST',
        dataType: 'JSON',
        data: {
            "full_name": _contact_fullName,
            "mail": _contact_mail,
            "phone": _contact_phone,
            "message_title": _contact_message_title,
            "message": _contact_message,
        },
        success: function (data) {
            alert(full_name);
            $("full_name").html(data.full_name);
            $("mail").html(data.mail);
            $("phone").html(data.phone);
            $("message_title").html(data.message_title);
            $("message").html(data.message);
            window.location.reload();
        },
        error: function (data) {
            alert(data.message)
            window.location.reload();
        }
    });
}
$('#destek_talebinde_bulun').click(function (e) {
    e.preventDefault();
    Contacts();
});


