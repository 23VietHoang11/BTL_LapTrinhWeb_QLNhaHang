document.addEventListener("DOMContentLoaded", function () {
    // Tập hợp các phần tử của từng phần trong giao diện
    const sections = {
        dashboard: document.getElementById("dashboard"),
        tableBookingContent: document.getElementById("tableBookingContent"),
        invoiceContent: document.getElementById("invoiceContent"),
        menuContent: document.getElementById("menuContent"),
        staffContent: document.getElementById("staffContent")  // Thêm dòng này
    };

    // Hàm ẩn tất cả các phần trong giao diện
    function hideAllSections() {
        for (let key in sections) {
            sections[key].style.display = "none";
        }
    }

    // Ẩn tất cả các phần và chỉ hiện phần dashboard khi tải trang
    hideAllSections();
    sections.dashboard.style.display = "block";

    // Gán sự kiện click cho từng mục trong sidebar để hiển thị phần tương ứng
    document.querySelector("#statistics").onclick = () => { 
        hideAllSections(); 
        sections.dashboard.style.display = "block"; 
    };
    document.querySelector("#invoices").onclick = () => { 
        hideAllSections(); 
        sections.invoiceContent.style.display = "block"; 
    };
    document.querySelector("#tableBooking").onclick = () => { 
        hideAllSections(); 
        sections.tableBookingContent.style.display = "block"; 
    };
    document.querySelector("#menu").onclick = () => { 
        hideAllSections(); 
        sections.menuContent.style.display = "block"; 
    };
    document.querySelector("#staff").onclick = () => {  // Thêm sự kiện cho phần "Nhân viên"
        hideAllSections(); 
        sections.staffContent.style.display = "block"; 
    };
});
