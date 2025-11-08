using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.ViewModels;
using System.Linq;

namespace Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DatBanAdminController : Controller
    {
        private readonly QLNhaHangContext _context;

        public DatBanAdminController(QLNhaHangContext context)
        {
            _context = context;
        }

        // SỬA ĐỔI CHỨC NĂNG INDEX ĐỂ HỖ TRỢ TÌM KIẾM VÀ PHÂN TRANG
        public IActionResult Index(string searchTerm, int? pageNumber, int pageSize = 10)
        {
            if (pageSize < 1) pageSize = 10;

            // 1. Chuẩn bị truy vấn và mapping sang ViewModel (AsQueryable)
            var datBanQuery = _context.DatBans
                .Include(d => d.IdkhachHangNavigation)
                .Include(d => d.IdbanAnNavigation)
                .Select(d => new DatBanAdminViewModel
                {
                    IDDatBan = d.IddatBan,
                    TenKhachHang = d.IdkhachHangNavigation.HoTenKh ?? "Khách vãng lai",
                    SDT = d.IdkhachHangNavigation.Sdt ?? "N/A",
                    Email = d.IdkhachHangNavigation.Email,
                    ThoiGian = d.ThoiGian,
                    SoLuongKH = d.SoLuongKh ?? 0,
                    GhiChu = d.GhiChu,
                    TrangThai = d.TrangThai,
                    IDBanAn = d.IdbanAn
                }).AsQueryable();

            // 2. Xử lý TÌM KIẾM (Filtering)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                datBanQuery = datBanQuery.Where(d =>
                    d.TenKhachHang.Contains(searchTerm) ||
                    d.SDT.Contains(searchTerm) ||
                    d.Email.Contains(searchTerm) ||
                    d.TrangThai.Contains(searchTerm)
                );
            }

            // 3. Xử lý PHÂN TRANG (Pagination)

            int totalItems = datBanQuery.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int actualPageNumber = pageNumber ?? 1;
            if (actualPageNumber < 1) actualPageNumber = 1;
            if (actualPageNumber > totalPages && totalPages > 0) actualPageNumber = totalPages;

            int skipAmount = (actualPageNumber - 1) * pageSize;

            // Lấy dữ liệu cho trang hiện tại
            var paginatedDatBans = datBanQuery
                .OrderByDescending(d => d.ThoiGian)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();

            // 4. Tạo ViewModel và trả về View
            var viewModel = new DatBanIndexViewModel
            {
                DatBans = paginatedDatBans,
                PageNumber = actualPageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                SearchTerm = searchTerm ?? string.Empty
            };

            return View(viewModel);
        }

        // Form gán bàn cho yêu cầu đặt chỗ
        [HttpGet]
        public IActionResult AssignTable(int id)
        {
            var datban = _context.DatBans
                .Include(d => d.IdkhachHangNavigation)
                .FirstOrDefault(d => d.IddatBan == id);

            if (datban == null)
                return NotFound();

            ViewBag.DanhSachBan = _context.BanAns
                .Where(b => b.TrangThai == "Trống")
                .ToList();

            return View(datban);
        }

        // Xử lý lưu bàn được chọn
        [HttpPost]
        public IActionResult AssignTable(int id, int idBanAn)
        {
            var datban = _context.DatBans.FirstOrDefault(d => d.IddatBan == id);
            var ban = _context.BanAns.FirstOrDefault(b => b.IdbanAn == idBanAn);

            if (datban == null || ban == null)
                return NotFound();

            datban.IdbanAn = idBanAn;
            datban.TrangThai = "Đã xác nhận";

            ban.TrangThai = "Đã đặt";

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult KhachNhanBan(int id)
        {
            var datban = _context.DatBans
                .Include(d => d.IdkhachHangNavigation)
                .Include(d => d.IdbanAnNavigation)
                .FirstOrDefault(d => d.IddatBan == id);

            if (datban == null)
                return NotFound();

            // Cập nhật trạng thái
            datban.TrangThai = "Đã nhận bàn";
            if (datban.IdbanAnNavigation != null)
                datban.IdbanAnNavigation.TrangThai = "Đang sử dụng";

            // Tạo mới hóa đơn
            var hoadon = new HoaDon
            {
                IddatBan = datban.IddatBan,
                IdbanAn = datban.IdbanAn ?? 0, // Nếu IDBanAn nullable
                IdkhachHang = datban.IdkhachHang,
                NgayLap = DateTime.Now,
                TongTien = 0, // Ban đầu chưa có món
                TrangThai = "Chưa thanh toán"
            };

            _context.HoaDons.Add(hoadon);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}