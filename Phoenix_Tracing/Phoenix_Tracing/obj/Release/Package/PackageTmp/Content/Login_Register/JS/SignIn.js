function Login() {
	var data = $("#loginForm").serialize();
	$.ajax({
		type: "POST",
		dataType: "JSON",
		url: "/Login/CheckValidUser",
		data: data,
		success: function (result) {
			if (result != null) {
				if (result.Ref == 0) {
					$("#loginForm")[0].reset();
					alert(result.Message);
					window.location.reload();
				}
				else {
					//alert(result.Message);
					window.location.href = "/Home/Index";

				}
			}
			else {
				alert(result.Message);
				window.location.reload();
			}
		}
	});
}
$('#SignIn').click(function (e) {
	e.preventDefault();
	Login();
});