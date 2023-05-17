$(document).ready(function () {
    $("#test").change(function () {
        
        var _selectValue = $(this).val();
        $.ajax({
            url: "/xProfileEdit/GetUserAddressData/",
            dataType: "json",
            cache: false,
            type: 'GET',
            data: { addressID: _selectValue },
            success: function (result) {
                $("#adrPhone").val(result[0].phone);
                $("#adrCity").val(result[0].city);
                $("#adr").val(result[0].address);
                                
            },
            error: function (result) {
                var obj = JSON.parse(result);
            }
        });
      });

});