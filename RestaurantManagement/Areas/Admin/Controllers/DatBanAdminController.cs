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

        // Hiển thị danh sách đặt bàn cho admin
        public IActionResult Index()
        {
            var ds = _context.DatBans
                .Include(d => d.IdkhachHangNavigation)
                .Include(d => d.IdbanAnNavigation)
                .Select(d => new DatBanAdminViewModel
                {
                    IDDatBan = d.IddatBan,
                    TenKhachHang = d.IdkhachHangNavigation.HoTenKh,
                    SDT = d.IdkhachHangNavigation.Sdt,
                    Email = d.IdkhachHangNavigation.Email,
                    ThoiGian = d.ThoiGian,
                    SoLuongKH = d.SoLuongKh ?? 0,
                    GhiChu = d.GhiChu,
                    TrangThai = d.TrangThai,
                    IDBanAn = d.IdbanAn
                })
                .ToList();

            return View(ds);
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