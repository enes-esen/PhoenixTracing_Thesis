const signUpButton = document.getElementById('signUp');
const signInButton = document.getElementById('signIn');
const container = document.getElementById('container');

signUpButton.addEventListener('click', () => {
	container.classList.add("right-panel-active");
});

signInButton.addEventListener('click', () => {
	container.classList.remove("right-panel-active");
});



function myFunction() {
	var x = document.getElementById("reg_pass");
	var y = document.querySelector("#p1")
	if (x.type === "password") {
		x.type = "text";
		y.classList.replace("bxs-lock-alt", "bx-lock-open-alt");
	} else {
		x.type = "password";
		y.classList.replace("bx-lock-open-alt", "bxs-lock-alt");
	}
}

function myFunction2() {
	var x = document.getElementById("pass");
	var y = document.querySelector("#p2")
	if (x.type === "password") {
		x.type = "text";
		y.classList.replace("bxs-lock-alt", "bx-lock-open-alt");
	} else {
		x.type = "password";
		y.classList.replace("bx-lock-open-alt", "bxs-lock-alt");
	}
}
