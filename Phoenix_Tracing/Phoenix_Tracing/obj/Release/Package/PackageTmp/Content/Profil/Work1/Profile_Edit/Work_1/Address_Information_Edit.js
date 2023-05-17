function AddressInformationEdit() {
    // Seçilen adres idsi
    var _address_select = document.getElementById('test');
    var _address_select_type = _address_select.options[_address_select.selectedIndex].value;

    //var _address_adrName
    var _address_phone = $("#adrPhone").val();
    var _address_city = $("#adrCity").val();
    var _address_adr = $("#adr").val();

    //alert(_address_select_type);

    $.ajax({
        url: "/xProfileEdit/AddressInformationEdit",
        type: "POST",
        dataType: "json",
        data: {
            'addressID': _address_select_type,
            'phone': _address_phone,
            'city': _address_city,
            'address': _address_adr
        },
        success: function (result) {
            //alert("Değişiklik Yapıldı");
            window.location.reload();
        }

    });

}

$("#btnAdrEdit").click(function (e) {
    e.preventDefault();
    AddressInformationEdit();
});