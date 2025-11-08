using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RestaurantManagement.Areas.Admin.Models.ViewModels; // <--- PHẢI CÓ DÒNG NÀY
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Areas.Admin.Models.ViewModels; // Giả sử ViewModel của bạn nằm ở đây
using System.Linq;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonController : Controller
    {
        private readonly QLNhaHangContext _context;

        public HoaDonController(QLNhaHangContext context)
        {
            _context = context;
        }
        // GET: Admin/HoaDon/Index
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber, int pageSize = 10)
        {
            if (pageSize < 1) pageSize = 10;

            // 1. Chuẩn bị truy vấn ban đầu
            var hoaDonQuery = _context.HoaDons
                                      .Include(hd => hd.IdkhachHangNavigation) // Giả sử bạn muốn hiển thị tên KH
                                      .AsQueryable();

            // 2. Xử lý TÌM KIẾM
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Chuyển searchTerm thành dạng chữ thường để so sánh nếu bạn muốn tìm kiếm không phân biệt chữ hoa/thường
                string lowerSearchTerm = searchTerm.ToLower();

                hoaDonQuery = hoaDonQuery.Where(hd =>
                    // Điều kiện 1: Tìm kiếm trong Trạng thái
                    hd.TrangThai.Contains(searchTerm) ||

                    // Điều kiện 2: Tìm kiếm trong Tên Khách hàng (nếu có)
                    (hd.IdkhachHangNavigation != null && hd.IdkhachHangNavigation.HoTenKh.Contains(searchTerm)) ||

                    // ĐIỀU KIỆN MỚI (RẤT QUAN TRỌNG): 
                    // Nếu người dùng tìm kiếm "guest" HOẶC "khách", và IdKhachHang là NULL
                    // Chỉ tìm kiếm ID NULL nếu người dùng gõ "guest" hoặc các từ khóa tương đương
                    (hd.IdkhachHang == null && (lowerSearchTerm.Contains("guest") || lowerSearchTerm.Contains("khách")))
                );
            }

            // 3. Xử lý PHÂN TRANG
            int totalItems = await hoaDonQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int actualPageNumber = pageNumber ?? 1;
            if (actualPageNumber < 1) actualPageNumber = 1;
            if (actualPageNumber > totalPages && totalPages > 0) actualPageNumber = totalPages;

            int skipAmount = (actualPageNumber - 1) * pageSize;

            // Lấy dữ liệu cho trang hiện tại
            var paginatedHoaDons = await hoaDonQuery
                .OrderByDescending(hd => hd.NgayLap) // Sắp xếp theo ngày lập mới nhất
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

            // 4. Tạo ViewModel và trả về View
            var viewModel = new HoaDonIndexViewModel // Đảm bảo bạn sử dụng ViewModel đã tạo
            {
                HoaDons = paginatedHoaDons,
                PageNumber = actualPageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                SearchTerm = searchTerm ?? string.Empty
            };

            return View(viewModel);
        }
        // GET: Admin/HoaDon/Create
        //public IActionResult Create()
        //{
        //    return View(new HoaDonCreateEditViewModel());
        //}

        //// POST: Admin/HoaDon/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(HoaDonCreateEditViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var hoaDon = new HoaDon
        //        {
        //            IdkhachHang = model.IdkhachHang,
        //            NgayLap = DateTime.Now,
        //            TongTien = 0, // Tổng tiền khởi tạo bằng 0
        //            TrangThai = model.TrangThai
        //        };

        //        _context.Add(hoaDon);
        //        await _context.SaveChangesAsync();

        //        // Sau khi tạo, chuyển hướng đến trang Chi tiết (Details) để tiếp tục thêm món ăn
        //        return RedirectToAction(nameof(Details), new { id = hoaDon.IdhoaDon });
        //    }
        //    return View(model);
        //}

        // ------------------------------------------------------------------
        // SỬA (EDIT)
        // ------------------------------------------------------------------

        //// GET: Admin/HoaDon/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var hoaDon = await _context.HoaDons.FindAsync(id);
        //    if (hoaDon == null) return NotFound();

        //    var model = new HoaDonCreateEditViewModel
        //    {
        //        IdhoaDon = hoaDon.IdhoaDon,
        //        IdkhachHang = hoaDon.IdkhachHang,
        //        TrangThai = hoaDon.TrangThai,
        //        NgayLap = hoaDon.NgayLap,
        //        TongTien = hoaDon.TongTien ?? 0
        //    };
        //    return View(model);
        //}

        //// POST: Admin/HoaDon/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, HoaDonCreateEditViewModel model)
        //{
        //    if (id != model.IdhoaDon) return NotFound();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var hoaDon = await _context.HoaDons.FindAsync(id);
        //            if (hoaDon == null) return NotFound();

        //            // Cập nhật các trường được phép sửa
        //            hoaDon.IdkhachHang = model.IdkhachHang;
        //            hoaDon.TrangThai = model.TrangThai;
        //            // Không sửa NgayLap và TongTien trực tiếp qua form này

        //            _context.Update(hoaDon);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!_context.HoaDons.Any(e => e.IdhoaDon == model.IdhoaDon)) return NotFound();
        //            throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}

        // ------------------------------------------------------------------
        // XÓA (DELETE)
        // ------------------------------------------------------------------

        // GET: Admin/HoaDon/Delete/5 (Trang xác nhận)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var hoaDon = await _context.HoaDons
                .Include(hd => hd.IdkhachHangNavigation)
                .FirstOrDefaultAsync(m => m.IdhoaDon == id);

            if (hoaDon == null) return NotFound();

            return View(hoaDon); // Truyền Entity để hiển thị thông tin xác nhận
        }

        // POST: Admin/HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ------------------------------------------------------------------
        // CHI TIẾT (DETAILS)
        // ------------------------------------------------------------------

        //// GET: Admin/HoaDon/Details/5 (Quan trọng để quản lý Chi tiết Hóa đơn)
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null) return NotFound();

        //    var hoaDon = await _context.HoaDons
        //        .Include(hd => hd.IdkhachHangNavigation)
        //        // Bạn nên include cả các chi tiết hóa đơn (HoaDonChiTiets) ở đây
        //        .FirstOrDefaultAsync(m => m.IdhoaDon == id);

        //    if (hoaDon == null) return NotFound();

        //    return View(hoaDon);
        //}

// Bạn cũng cần các hàm Edit, Delete, Details cho Hóa đơn ở đây


// =============================================
// 2. API ACTIONS (Để JavaScript gọi dữ liệu)
// =============================================

//// GET: /api/hoadon/current
//// Thêm dấu / ở đầu để nó là đường dẫn tuyệt đối, không phụ thuộc vào Controller
//[HttpGet("/api/hoadon/current")]
//        public async Task<IActionResult> GetCurrentInvoices()
//        {
//            // ... (Giữ nguyên code xử lý bên trong của bạn)
//            var excludedStatuses = new[] { "Đã thanh toán", "Đã hủy" };
//            var invoices = await _context.HoaDons
//                .Where(h => h.TrangThai == null || !excludedStatuses.Contains(h.TrangThai))
//                .Include(h => h.IdbanAnNavigation)
//                .Include(h => h.IddatBanNavigation)
//                .OrderBy(h => h.NgayLap)
//                .Select(h => new {
//                    IdHoaDon = h.IdhoaDon,
//                    TenKhuVuc = h.IdbanAnNavigation != null ? (h.IdbanAnNavigation.LoaiBan ?? "Mang đi") : "Mang đi",
//                    SoBan = h.IdbanAn ?? 0,
//                    SoLuongKhach = h.IddatBanNavigation != null ? (h.IddatBanNavigation.SoLuongKh ?? 0) : 0,
//                    NgayLap = h.NgayLap,
//                    TongTien = h.TongTien ?? 0
//                })
//                .ToListAsync();

//            return Ok(invoices);
//        }

        //// POST: /api/hoadon
        //[HttpPost("/api/hoadon")] // Thêm dấu / ở đầu để làm đường dẫn tuyệt đối
        //public async Task<IActionResult> CreateHoaDon([FromBody] HoaDonCreateModel model)
        //{
        //    // ... (Giữ nguyên toàn bộ code xử lý bên trong của bạn)
        //    if (string.IsNullOrWhiteSpace(model.SdtKhachHang) || string.IsNullOrWhiteSpace(model.HoTenKhachHang))
        //    {
        //        return BadRequest(new { message = "SĐT và Tên khách hàng là bắt buộc." });
        //    }

        //    await using var transaction = await _context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var employee = await _context.NhanViens.FirstOrDefaultAsync();
        //        if (employee == null)
        //        {
        //            return BadRequest(new { message = "Lỗi hệ thống: Không có nhân viên nào trong CSDL." });
        //        }
        //        int loggedInEmployeeId = employee.IdnhanVien;

        //        var existingCustomer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == model.SdtKhachHang);
        //        KhachHang customer;
        //        if (existingCustomer != null)
        //        {
        //            customer = existingCustomer;
        //            customer.HoTenKh = model.HoTenKhachHang;
        //        }
        //        else
        //        {
        //            customer = new KhachHang { HoTenKh = model.HoTenKhachHang, Sdt = model.SdtKhachHang };
        //            _context.KhachHangs.Add(customer);
        //        }

        //        int defaultTableId = 1;
        //        var defaultTable = await _context.BanAns.FindAsync(defaultTableId);
        //        if (defaultTable == null)
        //        {
        //            defaultTable = await _context.BanAns.FirstOrDefaultAsync();
        //            if (defaultTable == null) return BadRequest(new { message = $"Lỗi hệ thống: Không có bàn ăn nào." });
        //            defaultTableId = defaultTable.IdbanAn;
        //        }

        //        var newDatBan = new DatBan
        //        {
        //            IdbanAn = defaultTableId,
        //            IdkhachHangNavigation = customer,
        //            SoLuongKh = model.SoLuongKhach,
        //            GhiChu = model.GhiChu,
        //            ThoiGian = model.ThoiGian,
        //            TrangThai = "Đã xác nhận"
        //        };
        //        _context.DatBans.Add(newDatBan);

        //        var newHoaDon = new HoaDon
        //        {
        //            IdkhachHangNavigation = customer,
        //            IdnhanVien = loggedInEmployeeId,
        //            IddatBanNavigation = newDatBan,
        //            NgayLap = DateTime.Now,
        //            ThoiGianGiao = model.ThoiGian,
        //            TongTien = model.TongTien,
        //            TrangThai = "Chờ xử lý"
        //        };
        //        _context.HoaDons.Add(newHoaDon);

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        return Ok(new { message = "Tạo hóa đơn thành công!", id = newHoaDon.IdhoaDon });
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
        //        {
        //            if (sqlEx.Message.Contains("UQ__KhachHan__CA1930A5"))
        //                return Conflict(new { message = "Lỗi: SĐT này đã tồn tại." });
        //        }
        //        return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.InnerException?.Message ?? ex.Message });
        //    }
        //}
    }
}