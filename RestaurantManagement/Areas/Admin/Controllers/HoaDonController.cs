using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace RestaurantManagement.Areas.Admin.Controllers // Đảm bảo namespace đúng chuẩn Area
{
    [Area("Admin")]
    // [Route("api/hoadon")]  <-- XÓA DÒNG NÀY để dùng routing mặc định cho MVC
    // [ApiController] <-- CÓ THỂ XÓA DÒNG NÀY nếu muốn linh hoạt hơn, hoặc giữ lại nhưng phải cẩn thận routing
    public class HoaDonController : Controller // <-- QUAN TRỌNG: Sửa ControllerBase thành Controller
    {
        private readonly QLNhaHangContext _context;

        public HoaDonController(QLNhaHangContext context)
        {
            _context = context;
        }

        // =============================================
        // 1. MVC ACTION (Để hiển thị giao diện)
        // =============================================
        // GET: Admin/HoaDon/Index
        public IActionResult Index()
        {
            // Trả về View để JavaScript sau đó sẽ gọi API tải dữ liệu
            return View();
        }

        // =============================================
        // 2. API ACTIONS (Để JavaScript gọi dữ liệu)
        // =============================================

        // GET: /api/hoadon/current
        // Thêm dấu / ở đầu để nó là đường dẫn tuyệt đối, không phụ thuộc vào Controller
        [HttpGet("/api/hoadon/current")]
        public async Task<IActionResult> GetCurrentInvoices()
        {
            // ... (Giữ nguyên code xử lý bên trong của bạn)
            var excludedStatuses = new[] { "Đã thanh toán", "Đã hủy" };
            var invoices = await _context.HoaDons
                .Where(h => h.TrangThai == null || !excludedStatuses.Contains(h.TrangThai))
                .Include(h => h.IdbanAnNavigation)
                .Include(h => h.IddatBanNavigation)
                .OrderBy(h => h.NgayLap)
                .Select(h => new {
                    IdHoaDon = h.IdhoaDon,
                    TenKhuVuc = h.IdbanAnNavigation != null ? (h.IdbanAnNavigation.LoaiBan ?? "Mang đi") : "Mang đi",
                    SoBan = h.IdbanAn ?? 0,
                    SoLuongKhach = h.IddatBanNavigation != null ? (h.IddatBanNavigation.SoLuongKh ?? 0) : 0,
                    NgayLap = h.NgayLap,
                    TongTien = h.TongTien ?? 0
                })
                .ToListAsync();

            return Ok(invoices);
        }

        // POST: /api/hoadon
        [HttpPost("/api/hoadon")] // Thêm dấu / ở đầu để làm đường dẫn tuyệt đối
        public async Task<IActionResult> CreateHoaDon([FromBody] HoaDonCreateModel model)
        {
            // ... (Giữ nguyên toàn bộ code xử lý bên trong của bạn)
            if (string.IsNullOrWhiteSpace(model.SdtKhachHang) || string.IsNullOrWhiteSpace(model.HoTenKhachHang))
            {
                return BadRequest(new { message = "SĐT và Tên khách hàng là bắt buộc." });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.NhanViens.FirstOrDefaultAsync();
                if (employee == null)
                {
                    return BadRequest(new { message = "Lỗi hệ thống: Không có nhân viên nào trong CSDL." });
                }
                int loggedInEmployeeId = employee.IdnhanVien;

                var existingCustomer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Sdt == model.SdtKhachHang);
                KhachHang customer;
                if (existingCustomer != null)
                {
                    customer = existingCustomer;
                    customer.HoTenKh = model.HoTenKhachHang;
                }
                else
                {
                    customer = new KhachHang { HoTenKh = model.HoTenKhachHang, Sdt = model.SdtKhachHang };
                    _context.KhachHangs.Add(customer);
                }

                int defaultTableId = 1;
                var defaultTable = await _context.BanAns.FindAsync(defaultTableId);
                if (defaultTable == null)
                {
                    defaultTable = await _context.BanAns.FirstOrDefaultAsync();
                    if (defaultTable == null) return BadRequest(new { message = $"Lỗi hệ thống: Không có bàn ăn nào." });
                    defaultTableId = defaultTable.IdbanAn;
                }

                var newDatBan = new DatBan
                {
                    IdbanAn = defaultTableId,
                    IdkhachHangNavigation = customer,
                    SoLuongKh = model.SoLuongKhach,
                    GhiChu = model.GhiChu,
                    ThoiGian = model.ThoiGian,
                    TrangThai = "Đã xác nhận"
                };
                _context.DatBans.Add(newDatBan);

                var newHoaDon = new HoaDon
                {
                    IdkhachHangNavigation = customer,
                    IdnhanVien = loggedInEmployeeId,
                    IddatBanNavigation = newDatBan,
                    NgayLap = DateTime.Now,
                    ThoiGianGiao = model.ThoiGian,
                    TongTien = model.TongTien,
                    TrangThai = "Chờ xử lý"
                };
                _context.HoaDons.Add(newHoaDon);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Tạo hóa đơn thành công!", id = newHoaDon.IdhoaDon });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
                {
                    if (sqlEx.Message.Contains("UQ__KhachHan__CA1930A5"))
                        return Conflict(new { message = "Lỗi: SĐT này đã tồn tại." });
                }
                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}