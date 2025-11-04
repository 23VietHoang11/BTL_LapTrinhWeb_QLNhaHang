using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Entities;
using System.Diagnostics;

namespace RestaurantManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLNhaHangContext _context;
        private readonly ILogger<HomeController> _logger;
        public class BookingViewModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public int NoOfPeople { get; set; }
            public DateTime DateTime { get; set; }
            public string SpecialRequest { get; set; }
        }
        public HomeController(QLNhaHangContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Booking(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Kiểm tra/Tạo Khách hàng mới
                // Logic tìm KhachHang qua Email/Tên (Hoặc tạo mới)
                var khachHang = _context.KhachHangs.FirstOrDefault(k => k.Email == model.Email);
                if (khachHang == null)
                {
                    khachHang = new KhachHang
                    {
                        HoTenKh = model.Name,
                        Email = model.Email,
                        // Các trường khác như Sdt, DiaChi, GioiTinh có thể được thêm vào sau
                    };
                    _context.KhachHangs.Add(khachHang);
                    _context.SaveChanges(); // Lưu Khách hàng để có IdKhachHang
                }

                // 2. Tạo Đặt bàn mới
                var datBan = new DatBan
                {
                    IdkhachHang = khachHang.IdkhachHang,
                    ThoiGian = model.DateTime,
                    SoLuongKh = model.NoOfPeople,
                    GhiChu = model.SpecialRequest,
                    TrangThai = "Chờ xác nhận" // Trạng thái mặc định
                };

                _context.DatBans.Add(datBan);
                _context.SaveChanges();

                // Chuyển hướng về trang thành công hoặc Booking (với thông báo)
                TempData["SuccessMessage"] = "Bạn đã đặt bàn thành công! Chúng tôi sẽ liên hệ để xác nhận.";
                return RedirectToAction("Booking");
            }

            // Nếu model không hợp lệ, quay lại view
            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Service()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public IActionResult Testimonials()
        {
            return View();
        }
    }
}
