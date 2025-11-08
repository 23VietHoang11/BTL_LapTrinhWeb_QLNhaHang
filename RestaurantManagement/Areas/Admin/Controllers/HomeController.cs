using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Admin.Models.ViewModels;
using RestaurantManagement.Models.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http; // <--- CẦN THIẾT CHO COOKIE

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller // Controller MVC chuẩn
    {
        private readonly QLNhaHangContext _context;

        public HomeController(QLNhaHangContext context)
        {
            _context = context;
        }

        // =======================================================
        // CÁC ACTION THAO TÁC COOKIE MẪU
        // =======================================================

        // GET: /Admin/Home/SetThemeCookie?theme=dark
        [HttpGet]
        public IActionResult SetThemeCookie(string theme = "light")
        {
            string cookieKey = "AdminTheme";
            string cookieValue = theme;

            // Cấu hình Cookie: Hết hạn sau 30 ngày, chỉ HTTP, bảo mật (HTTPS)
            var cookieOptions = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                Secure = true,
                Path = "/"
            };

            // Ghi (Tạo) Cookie
            Response.Cookies.Append(cookieKey, cookieValue, cookieOptions);

            // Chuyển hướng lại trang Dashboard hoặc trang hiện tại
            TempData["SuccessMessage"] = $"Đã đặt Theme Cookie thành: {theme}";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Home/ReadThemeCookie
        [HttpGet]
        public IActionResult ReadThemeCookie()
        {
            string cookieKey = "AdminTheme";

            // Đọc Cookie
            string userTheme = Request.Cookies[cookieKey];

            if (string.IsNullOrEmpty(userTheme))
            {
                ViewData["CurrentTheme"] = "Chưa đặt (mặc định)";
            }
            else
            {
                ViewData["CurrentTheme"] = userTheme;
            }

            // Có thể hiển thị kết quả trên một View hoặc Console Log
            return RedirectToAction(nameof(Index)); // Giả định hiển thị trên Dashboard
        }

        // GET: /Admin/Home/DeleteThemeCookie
        [HttpGet]
        public IActionResult DeleteThemeCookie()
        {
            string cookieKey = "AdminTheme";

            // Xóa Cookie
            Response.Cookies.Delete(cookieKey);

            TempData["SuccessMessage"] = "Đã xóa Theme Cookie thành công.";
            return RedirectToAction(nameof(Index));
        }


        // =======================================================
        // ACTION ĐÃ CÓ (GIỮ NGUYÊN)
        // =======================================================

        // GET: Hiển thị form đăng nhập
        [HttpGet]
        public IActionResult LoginAdmin()
        {
            return View(new LoginViewModel()); // Cần khởi tạo ViewModel
        }

        // POST: Xử lý dữ liệu form
        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // ... Logic xác thực tài khoản và thiết lập Cookie/Session ...
                // Sau khi đăng nhập thành công, chuyển hướng đến Dashboard:
                // return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            // Nếu thất bại, quay lại form
            return View(model);
        }

        // GET: Admin/Home/Index (Action hiển thị Dashboard)
        public async Task<IActionResult> Index()
        {
            // 1. Thực hiện truy vấn đếm số lượng từ các bảng
            var totalAccounts = await _context.NhanViens.CountAsync();
            var totalContacts = await _context.LienHes.CountAsync();
            var totalMenuItems = await _context.MonAns.CountAsync();
            var totalInvoices = await _context.HoaDons.CountAsync();
            var totalCustomers = await _context.DatBans.CountAsync();

            // 2. Tạo ViewModel
            var viewModel = new DashboardViewModel
            {
                TotalAccounts = totalAccounts,
                TotalContacts = totalContacts,
                TotalMenuItems = totalMenuItems,
                TotalInvoices = totalInvoices,
                TotalCustomers = totalCustomers
            };

            // 3. Trả về View với dữ liệu động
            return View(viewModel);
        }

        // Các Action khác (nếu có)
    }
}