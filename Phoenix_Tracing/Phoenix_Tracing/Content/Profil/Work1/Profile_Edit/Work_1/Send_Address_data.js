//Profil adres ekleme
function AddressSave() {
    var _address_name = $("#address_name").val();
    var _address_city = $("#address_city").val();
    var _address_phone = $("#address_phone").val();
    var _address = $("#address").val();
    $.ajax({
        url: "/Addresses/SendAddressData",
        type: "POST",
        dataType: "JSON",
        data: {
            "Name": _address_name,
            "City": _address_city,
            "Phone": _address_phone,
            "Address": _address,
        },
        success: function (data) {
            $("Name").html(data.Name);
            $("City").html(data.City);
            $("Phone").html(data.Phone);
            $("Address").html(data.Address);
            alert("Kayıt yapıldı.")
            window.location.reload();
        },
        error: function (data) {
            alert(data.Message);
            window.location.reload();
        }
    });
}

$('#profile_address_save').click(function (e) {
    e.preventDefault();
    AddressSave();
});