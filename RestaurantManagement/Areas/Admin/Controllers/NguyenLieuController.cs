using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.DTOs; // Dùng DTO
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Controllers
{
    [Route("api/nguyenlieu")]
    [ApiController]
    public class NguyenLieuController : ControllerBase
    {
        private readonly QLNhaHangContext _context;

        public NguyenLieuController(QLNhaHangContext context)
        {
            _context = context;
        }

        // GET: api/nguyenlieu (Đã cập nhật)
        [HttpGet]
        public async Task<IActionResult> GetAllNguyenLieu()
        {
            var nguyenLieus = await _context.NguyenLieus
                .Select(nl => new {
                    nl.IdnguyenLieu,
                    nl.TenNl,
                    nl.DonVi,
                    nl.Loai,
                    nl.SoLuong,
                    nl.NgayNhap,
                    nl.HinhAnh
                })
                .OrderBy(nl => nl.TenNl)
                .ToListAsync();
            return Ok(nguyenLieus);
        }

        // GET: api/nguyenlieu/{id} (Đã cập nhật)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNguyenLieu(int id)
        {
            var nguyenLieu = await _context.NguyenLieus.FindAsync(id);
            if (nguyenLieu == null)
            {
                return NotFound();
            }
            // Trả về đầy đủ để Sửa
            return Ok(nguyenLieu);
        }

        // POST: api/nguyenlieu (Đã cập nhật)
        [HttpPost]
        public async Task<IActionResult> CreateNguyenLieu([FromBody] NguyenLieuDTO model)
        {
            if (await _context.NguyenLieus.AnyAsync(m => m.TenNl == model.TenNl))
            {
                return Conflict(new { message = "Tên nguyên liệu đã tồn tại." });
            }

            var newNguyenLieu = new NguyenLieu
            {
                TenNl = model.TenNl,
                DonVi = model.DonVi,
                Loai = model.Loai,
                SoLuong = model.SoLuong,
                NgayNhap = model.NgayNhap,
                HinhAnh = model.HinhAnh
            };

            _context.NguyenLieus.Add(newNguyenLieu);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm nguyên liệu thành công!", id = newNguyenLieu.IdnguyenLieu });
        }

        // PUT: api/nguyenlieu/{id} (Đã cập nhật)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNguyenLieu(int id, [FromBody] NguyenLieuDTO model)
        {
            var nguyenLieu = await _context.NguyenLieus.FindAsync(id);
            if (nguyenLieu == null)
            {
                return NotFound(new { message = "Không tìm thấy nguyên liệu." });
            }

            if (await _context.NguyenLieus.AnyAsync(m => m.TenNl == model.TenNl && m.IdnguyenLieu != id))
            {
                return Conflict(new { message = "Tên nguyên liệu này đã bị trùng." });
            }

            nguyenLieu.TenNl = model.TenNl;
            nguyenLieu.DonVi = model.DonVi;
            nguyenLieu.Loai = model.Loai;
            nguyenLieu.SoLuong = model.SoLuong;
            nguyenLieu.NgayNhap = model.NgayNhap;
            nguyenLieu.HinhAnh = model.HinhAnh;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công." });
        }

        // DELETE: api/nguyenlieu/{id} (Không đổi)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNguyenLieu(int id)
        {
            var nguyenLieu = await _context.NguyenLieus.FindAsync(id);
            if (nguyenLieu == null)
            {
                return NotFound(new { message = "Không tìm thấy nguyên liệu." });
            }

            _context.NguyenLieus.Remove(nguyenLieu);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa nguyên liệu thành công." });
        }
    }
}