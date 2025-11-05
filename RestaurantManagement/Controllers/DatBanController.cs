using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.ViewModels;
using System.Linq;

public class DatBanAdminController : Controller
{
    private readonly QLNhaHangContext _context;

    public DatBanAdminController(QLNhaHangContext context)
    {
        _context = context;
    }

    // GET: /DatBan/Booking
    [HttpGet]
    public IActionResult Booking()
    {
        var vm = new DatBanViewModel
        {
            DatBan = new DatBan(),
            KhachHang = new KhachHang(),
            DanhSachBan = _context.BanAns
                .Where(b => b.TrangThai == "Trống")
                .ToList()
        };
        return View(vm);
    }

    // POST: /DatBan/Booking
    [HttpPost]
    public IActionResult Booking(DatBanViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.DanhSachBan = _context.BanAns
                .Where(b => b.TrangThai == "Trống")
                .ToList();
            return View(vm);
        }

        // 🔹 Kiểm tra khách hàng đã tồn tại chưa (theo SDT)
        var kh = _context.KhachHangs.FirstOrDefault(x => x.Sdt == vm.KhachHang.Sdt);
        if (kh == null)
        {
            _context.KhachHangs.Add(vm.KhachHang);
            _context.SaveChanges();
            kh = vm.KhachHang;
        }

        // 🔹 Tạo bản ghi đặt bàn
        var datBan = vm.DatBan;
        datBan.IdkhachHang = kh.IdkhachHang;
        datBan.TrangThai = "Chờ xác nhận";

        _context.DatBans.Add(datBan);

        // 🔹 Cập nhật trạng thái bàn
        var ban = _context.BanAns.FirstOrDefault(b => b.IdbanAn == datBan.IdbanAn);
        if (ban != null)
        {
            ban.TrangThai = "Đã đặt";
            _context.Entry(ban).State = EntityState.Modified;
        }

        _context.SaveChanges();

        TempData["SuccessMessage"] = "Đặt bàn thành công! Chúng tôi sẽ liên hệ để xác nhận.";
        return RedirectToAction("Booking");
    }
}
