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
					yValueFormatString: "####",
					legendText: "{indexLabel}",
					dataPoints: [
						{ y: 4181563, indexLabel: "Destek Talebi" },
						{ y: 2175498, indexLabel: "İş Talebi" },
						{ y: 3125844, indexLabel: "Bayi Kayıt Talebi" },

					]
				}
			]
		});
	chart.render();
}