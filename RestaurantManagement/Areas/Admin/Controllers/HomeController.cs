using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Admin.Models.ViewModels;
using RestaurantManagement.Models.Entities; // Thay thế bằng namespace chứa DbContext của bạn
using System.Threading.Tasks;

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

        // GET: Admin/Home/Index (Action hiển thị Dashboard)
        public async Task<IActionResult> Index()
        {
            // 1. Thực hiện truy vấn đếm số lượng từ các bảng
            var totalAccounts = await _context.NhanViens.CountAsync(); // Giả sử Tài khoản là bảng NhanViens
            var totalContacts = await _context.LienHes.CountAsync(); // Bảng Liên hệ
            var totalMenuItems = await _context.MonAns.CountAsync(); // Bảng Món ăn
            var totalInvoices = await _context.HoaDons.CountAsync(); // Bảng Hóa đơn
            var totalCustomers = await _context.DatBans.CountAsync(); // Đếm Khách hàng

            // 2. Tạo ViewModel
            var viewModel = new DashboardViewModel
            {
                TotalAccounts = totalAccounts,
                TotalContacts = totalContacts,
                TotalMenuItems = totalMenuItems,
                TotalInvoices = totalInvoices,
                // THÊM: Gán giá trị Khách hàng vào ViewModel
                TotalCustomers = totalCustomers
            };

            // 3. Trả về View với dữ liệu động
            return View(viewModel);
        }

        // Các Action khác (nếu có)
    }
}