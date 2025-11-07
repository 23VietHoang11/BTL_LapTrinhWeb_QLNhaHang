using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Admin.Models;
using RestaurantManagement.Models.Entities;
using System.Data.Common;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : Controller
    {
        private readonly QLNhaHangContext _context;
        public BookingController(QLNhaHangContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Lấy danh sách đặt bàn
            var danhSachDatBan = await _context.DatBans // 2. Thêm từ khóa "await"
                                         .Include(d => d.IdkhachHangNavigation)
                                         .OrderByDescending(d => d.ThoiGian)
                                         .ToListAsync(); // 3. Đổi thành ToListAsync()

            return View(danhSachDatBan); 
        }
    }
}
