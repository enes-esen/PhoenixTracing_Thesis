function OperationSend() {
    var _operation_name = $("#operation_name").val();
    //OPERATİON
    var _operation_select = document.getElementById('operation_select');
    var _operation_select_type = _operation_select.options[_operation_select.selectedIndex].value;
    //ADDRESS
    var _address_select = document.getElementById('address_select');
    var _address_select_value = _address_select.options[_address_select.selectedIndex].value;
    var _operation_detail = $("#operation_detail").val();

    $.ajax({
        url: '/OperationRequests/SendOperationData',
        type: "POST",
        dataType: "JSON",
        data: {
            'Request_type_id': _operation_select_type,
            'Name': _operation_name,
            'Detail': _operation_detail,            
            'Address_id': _address_select_value
        },
        success: function (data) {
            $("Request_type_id").html(data.Request_type_id);
            $("Name").html(data.Name);
            $("Detail").html(data.Detail);
            $("Address_id").html(data.Address_id);
            alert("İş talebiniz alındı");
            window.location.reload();
        },
        error: function (data) {
            alert(data.Message);
            window.location.reload();
        }
    });
}

$('#operation_send').click(function (e) {
    e.preventDefault();
    OperationSend();
});