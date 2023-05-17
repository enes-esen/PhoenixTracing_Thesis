let sidebar = document.querySelector(".sidebar");
let sidebarBtn = document.querySelector(".sidebarBtn");
sidebarBtn.onclick = function () {
    sidebar.classList.toggle("active");
    if (sidebar.classList.contains("active")) {
        sidebarBtn.classList.replace("bx-menu", "bx-menu-alt-right");
    } else
        sidebarBtn.classList.replace("bx-menu-alt-right", "bx-menu");
}

let nav_links = document.querySelectorAll(".nav-links a");
let activeIndex;

function changeLink() {
    console.log("hello");
    nav_links.forEach((sideLink) => sideLink.classList.remove("active"));
    this.classList.add("active");

    activeIndex = this.dataset.active;
	console.log(activeIndex);
}

nav_links.forEach((link) => link.addEventListener("click", changeLink));

window.onload = function () {

	var h_total = document.getElementById("help_total").innerHTML;
	var op_total = document.getElementById("operation_total").innerHTML;
	var per_total = document.getElementById("periodik_total").innerHTML;

	var chart = new CanvasJS.Chart("chartContainer",
		{
			theme: "light2",
			//title: {
			//	text: "......."
			//},
			data: [
				{
					type: "pie",
					showInLegend: true,
					toolTipContent: "{y} - #percent %",
					yValueFormatString: "#### Adet",
					legendText: "{indexLabel}",
					dataPoints: [
						{ y: h_total, indexLabel: "Destek Talebi" },
						{ y: op_total, indexLabel: "İş Talebi" },
						{ y: per_total, indexLabel: "Periyodik İş Talepleri" },
					]
				}
			]
		});
	chart.render();
}