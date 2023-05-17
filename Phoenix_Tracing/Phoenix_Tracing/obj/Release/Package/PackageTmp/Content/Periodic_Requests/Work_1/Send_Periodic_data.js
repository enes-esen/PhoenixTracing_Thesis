function PeriodicSend() {

    var _periodic_name = $("#periodic_name").val();
    //Periodic_type
    var _periodic_select = document.getElementById('periodic_select');
    var _periodic_select_type = _periodic_select.options[_periodic_select.selectedIndex].value;
    //Address
    var _address_select = document.getElementById('address_select');
    var _address_select_value = _address_select.options[_address_select.selectedIndex].value;
    //Detail
    var _periodic_period = $("#periodic_period").val();
    var _periodic_detail = $('#periodic_detail').val();

    $.ajax({
        url: '/PeriodicRequests/SendPeriodicData',
        type: 'POST',
        dataType: "JSON",
        data: {
            'Name': _periodic_name,
            'Request_type_id': _periodic_select_type,
            'Address_id': _address_select_value,
            'Period': _periodic_period,
            'Detail': _periodic_detail,
        },
        success: function (data) {
            $("Name").html(data.Name);
            $("Request_type_id").html(data.Request_type_id);
            $("Address_id").html(data.Address_id);
            $("Period").html(data.Period);
            $("Detail").html(data.Detail);
            alert("Periyodic İşiniz talebiniz alınmıştır");
            window.location.reload();
        }
    });
}

$('#periodic_send').click(function (e) {
    e.preventDefault();
    PeriodicSend();
});