document.getElementById("login").addEventListener("submit", function(event) {
    event.preventDefault(); 

    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;

    if (username === "admin" && password === "12345678") {
        window.location.href = "/Admin/Home/Index";
    } else {
        alert("Tài khoản hoặc mật khẩu không đúng!");
    }
});
