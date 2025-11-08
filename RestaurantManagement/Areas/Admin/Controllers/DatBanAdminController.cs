using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.ViewModels;
using System; // Thêm namespace này cho DateTime
using System.Linq;
using System.Threading.Tasks; // Thêm namespace này cho Async

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

        // --- Bắt đầu: Các Action MỚI và SỬA ĐỔI ---

        // NEW: Action Xác nhận đơn đặt bàn (Chuyển sang trạng thái "Đã xác nhận")
        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var datban = await _context.DatBans.FindAsync(id);

            if (datban == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn đặt bàn để xác nhận.";
                return RedirectToAction("Index");
            }

            if (datban.TrangThai == "Chờ xác nhận")
            {
                datban.TrangThai = "Đã xác nhận";
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Đã xác nhận đơn đặt bàn ID **{id}**. Vui lòng **Phân bàn** nếu cần.";
                }
                catch
                {
                    TempData["ErrorMessage"] = "Lỗi khi lưu Database, không thể xác nhận.";
                }
            }
            else
            {
                TempData["WarningMessage"] = $"Đơn đặt bàn ID {id} đã ở trạng thái **{datban.TrangThai}** và không cần xác nhận.";
            }

            return RedirectToAction("Index");
        }

        // NEW: Action Xóa đơn đặt bàn
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var datban = await _context.DatBans.FindAsync(id);

            if (datban == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn đặt bàn để xóa.";
                return RedirectToAction("Index");
            }

            // Nếu đơn đã có bàn được gán, cần giải phóng bàn trước khi xóa
            if (datban.IdbanAn.HasValue)
            {
                var ban = await _context.BanAns.FindAsync(datban.IdbanAn.Value);
                if (ban != null && ban.TrangThai == "Đã đặt")
                {
                    ban.TrangThai = "Trống"; // Giải phóng bàn
                }
            }

            try
            {
                _context.DatBans.Remove(datban);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa thành công đơn đặt bàn ID **{id}**.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa đơn đặt bàn ID {id}: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // --- Các Action còn lại (Giữ nguyên hoặc tinh chỉnh nhẹ) ---

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
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn đặt bàn hoặc bàn ăn.";
                return RedirectToAction("Index");
            }

            if (ban.TrangThai != "Trống")
            {
                TempData["ErrorMessage"] = "Bàn đã chọn không còn trống.";
                // Cần phải tải lại dữ liệu cho AssignTable View nếu không muốn Redirect
                return RedirectToAction("Index");
            }

            datban.IdbanAn = idBanAn;
            datban.TrangThai = "Đã xác nhận";

            ban.TrangThai = "Đã đặt";

            _context.SaveChanges();
            TempData["SuccessMessage"] = $"Đã gán bàn **{idBanAn}** và xác nhận đơn đặt bàn ID **{id}**.";

            return RedirectToAction("Index");
        }

        // Khách nhận bàn (SỬA ĐỔI để trả về JSON cho xử lý AJAX)
        [HttpPost]
        public IActionResult KhachNhanBan(int id)
        {
            // Sử dụng Transaction để đảm bảo tính toàn vẹn
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var datban = _context.DatBans
                        .Include(d => d.IdbanAnNavigation)
                        .FirstOrDefault(d => d.IddatBan == id);

                    if (datban == null)
                        return Json(new { success = false, message = "Không tìm thấy đơn đặt bàn." });

                    // Cập nhật trạng thái Đặt bàn
                    datban.TrangThai = "Đã nhận bàn";

                    // Cập nhật trạng thái Bàn
                    if (datban.IdbanAnNavigation != null)
                        datban.IdbanAnNavigation.TrangThai = "Đang sử dụng";

                    // Tạo mới hóa đơn
                    var hoadon = new HoaDon
                    {
                        IddatBan = datban.IddatBan,
                        IdbanAn = datban.IdbanAn ?? 0,
                        IdkhachHang = datban.IdkhachHang,
                        NgayLap = DateTime.Now,
                        TongTien = 0,
                        TrangThai = "Chưa thanh toán"
                    };

                    _context.HoaDons.Add(hoadon);
                    _context.SaveChanges();

                    transaction.Commit(); // Commit Transaction

                    return Json(new { success = true, idHoaDon = hoadon.IdhoaDon, message = "Khách đã nhận bàn, hóa đơn mới đã được tạo." });
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback nếu có lỗi
                    return Json(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
                }
            }
        }
    }
}