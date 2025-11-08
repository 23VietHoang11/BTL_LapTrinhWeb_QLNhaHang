using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Areas.Admin.Models.ViewModels;
using System;
using System.Linq;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupportController : Controller
    {
        private readonly QLNhaHangContext _context;

        public SupportController(QLNhaHangContext context)
        {
            _context = context;
        }

        // ✅ Hiển thị danh sách liên hệ (có phân trang + tìm kiếm)
        public IActionResult Index(string searchTerm, int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.LienHes.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    x.HoTen.Contains(searchTerm) ||
                    x.Email.Contains(searchTerm) ||
                    x.TieuDe.Contains(searchTerm) ||
                    x.NoiDung.Contains(searchTerm));
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize); // Tính tổng số trang

            // 🛑 BẮT ĐẦU SỬA LỖI: CHUẨN HÓA pageNumber 

            // 1. Đảm bảo pageNumber không nhỏ hơn 1
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            // 2. Đảm bảo pageNumber không vượt quá tổng số trang (trừ trường hợp không có mục nào)
            if (totalPages > 0 && pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            // 🛑 KẾT THÚC SỬA LỖI

            // Tính toán OFFSET an toàn
            var lienHes = query
                .OrderByDescending(x => x.NgayGui)
                // Phép tính .Skip() giờ đây đảm bảo không bao giờ âm
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new SupportIndexViewModel
            {
                LienHes = lienHes,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages, // Sử dụng biến totalPages đã tính
                SearchTerm = searchTerm
            };

            return View(vm);
        }

        // ✅ Xóa liên hệ
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var lienHe = _context.LienHes.Find(id);
            if (lienHe != null)
            {
                _context.LienHes.Remove(lienHe);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}