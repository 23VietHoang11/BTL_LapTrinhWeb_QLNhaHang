using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.DTOs; // Đảm bảo bạn đã tạo DTOs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient; // Cần thêm để bắt lỗi SQL

namespace RestaurantManagement.Controllers
{
    [Route("api/hoadon")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly QLNhaHangContext _context;

        public HoaDonController(QLNhaHangContext context)
        {
            _context = context;
        }

        /**
         * API LẤY HÓA ĐƠN ĐANG CHẠY (Giữ nguyên)
         */
        // GET: api/hoadon/current
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentInvoices()
        {
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

        /**
         * API TẠO HÓA ĐƠN MỚI (ĐÃ SỬA LỖI BUILD)
         */
        // POST: api/hoadon
        [HttpPost]
        public async Task<IActionResult> CreateHoaDon([FromBody] HoaDonCreateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SdtKhachHang) || string.IsNullOrWhiteSpace(model.HoTenKhachHang))
            {
                return BadRequest(new { message = "SĐT và Tên khách hàng là bắt buộc." });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Tìm nhân viên
                var employee = await _context.NhanViens.FirstOrDefaultAsync();
                if (employee == null)
                {
                    return BadRequest(new { message = "Lỗi hệ thống: Không có nhân viên nào trong CSDL." });
                }
                int loggedInEmployeeId = employee.IdnhanVien;

                // 2. Logic Tìm-hoặc-Tạo Khách Hàng (Giữ nguyên)
                var existingCustomer = await _context.KhachHangs
                                          .FirstOrDefaultAsync(k => k.Sdt == model.SdtKhachHang);
                KhachHang customer;
                if (existingCustomer != null)
                {
                    customer = existingCustomer;
                    customer.HoTenKh = model.HoTenKhachHang;
                }
                else
                {
                    customer = new KhachHang
                    {
                        HoTenKh = model.HoTenKhachHang,
                        Sdt = model.SdtKhachHang
                    };
                    _context.KhachHangs.Add(customer);
                }

                // --- SỬA LỖI BUILD (FIX LỖI) ---
                // Vì CSDL yêu cầu 'DatBan' phải có 'IdbanAn',
                // chúng ta sẽ gán nó vào 1 bàn "ảo" (ví dụ: Bàn 1)
                // Hãy đảm bảo bạn có Bàn ID=1 trong CSDL (bạn đã có "Sảnh VIP Test" từ script SQL trước)
                int defaultTableId = 1;
                var defaultTable = await _context.BanAns.FindAsync(defaultTableId);
                if (defaultTable == null)
                {
                    // Nếu Bàn 1 không tồn tại, thử tìm bàn đầu tiên
                    defaultTable = await _context.BanAns.FirstOrDefaultAsync();
                    if (defaultTable == null) // Nếu vẫn k có bàn nào
                    {
                        return BadRequest(new { message = $"Lỗi hệ thống: Không có bàn ăn nào trong CSDL để gán." });
                    }
                    defaultTableId = defaultTable.IdbanAn; // Dùng ID bàn đầu tiên
                }
                // --- KẾT THÚC SỬA LỖI BUILD ---

                // 3. Tạo 'DatBan' (Đặt bàn) mới 
                var newDatBan = new DatBan
                {
                    IdbanAn = defaultTableId, // <-- Gán ID bàn mặc định
                    IdkhachHangNavigation = customer,
                    SoLuongKh = model.SoLuongKhach,
                    GhiChu = model.GhiChu,
                    ThoiGian = model.ThoiGian,
                    TrangThai = "Đã xác nhận"
                };
                _context.DatBans.Add(newDatBan);

                // 4. Tạo Hóa Đơn mới (ĐÃ XÓA IdbanAn)
                var newHoaDon = new HoaDon
                {
                    // Bỏ: IdbanAn
                    IdkhachHangNavigation = customer,
                    IdnhanVien = loggedInEmployeeId,
                    IddatBanNavigation = newDatBan,
                    NgayLap = DateTime.Now,
                    ThoiGianGiao = model.ThoiGian,
                    TongTien = model.TongTien,
                    TrangThai = "Chờ xử lý"
                };
                _context.HoaDons.Add(newHoaDon);

                // 5. LƯU TẤT CẢ
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
                    {
                        return Conflict(new { message = "Lỗi: SĐT này đã tồn tại. Vui lòng kiểm tra lại." });
                    }
                }

                return StatusCode(500, new { message = "Lỗi máy chủ: " + ex.InnerException?.Message ?? ex.Message });
            }
        }
    }
}