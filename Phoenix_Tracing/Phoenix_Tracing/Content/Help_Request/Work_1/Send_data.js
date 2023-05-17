function HelpRequest() {
    var _help_Request_Name = $("#help_Request_Name").val();
    var _help_Request_Detail = $("#help_Request_Detail").val();
    $.ajax({
        url: '/HelpRequest/SendDataHelpRequest',
        type: 'POST',
        dataType: 'JSON',
        data: {
            "name": _help_Request_Name,
            "detail": _help_Request_Detail
        },
        success: function (data) {
            $("name").html(data.name);
            $("detail").html(data.detail);
            window.location.reload();
        },
        error: function () {
            //alert("Veri gönderilemedi");
            alert(data.rst);
            window.location.reload();
        }
    });
}