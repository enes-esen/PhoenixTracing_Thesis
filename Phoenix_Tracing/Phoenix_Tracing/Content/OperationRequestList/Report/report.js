//OperationRequestList
function OperationGetReport() {
    //document.getElementById('file_operation').click();

    var _operation_request_id = $("input[type=radio][name=operation_request_id]:checked").val();
    //alert(_operation_request_id);

    if (_operation_request_id != null) {
        //alert(_operation_request_id);
        $.ajax({
            url: '/OperationRequestList/RequestID',
            type: 'GET',
            dataType: 'JSON',
            data: {
                'request_id': _operation_request_id
            },
            success: function (data) {
                //alert(_operation_request_id)
                alert(data);
                window.location.reload();
            }

        });

    }
    else alert('Rapor oluşturmak için lütfen seçim yapınız.');

}

$('#documentary_operation').click(function (e) {
    e.preventDefault();
    OperationGetReport();
});