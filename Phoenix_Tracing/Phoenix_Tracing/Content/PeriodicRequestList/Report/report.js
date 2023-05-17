//PeriyodikRequestList
function PeriodicGetReport() {

    var _periodic_request_id = $("input[type=radio][name=periodik_request_id]:checked").val();

    if (_periodic_request_id != null) {
        //alert(_periodic_request_id);
        $.ajax({
            url: '/PeriodicRequestList/RequestID',
            type: 'GET',
            dataType: 'JSON',
            data: {
                'request_id': _periodic_request_id
            },
            success: function (data) {
                //alert(_periodic_request_id)
                alert(data);
                window.location.reload();
            }

        });

    }        
    else alert('Rapor oluşturmak için lütfen seçim yapınız.');

}

$('#documentary_periodic').click(function (e) {
    e.preventDefault();
    PeriodicGetReport();
});







