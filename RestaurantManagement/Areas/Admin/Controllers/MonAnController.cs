using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Controllers
{
    [Route("api/monan")]
    [ApiController]
    public class MonAnController : ControllerBase
    {
        private readonly QLNhaHangContext _context;

        public MonAnController(QLNhaHangContext context)
        {
            _context = context;
        }

        // GET: api/monan (Lấy danh sách)
        [HttpGet]
        public async Task<IActionResult> GetAllMonAn()
        {
            var monAns = await _context.MonAns
                .Select(m => new {
                    m.IdmonAn,
                    m.TenMon,
                    m.Gia,
                    m.Loai,
                    m.HinhAnh,
                    m.DonViTinh
                })
                .OrderBy(m => m.TenMon)
                .ToListAsync();
            return Ok(monAns);
        }

        // GET: api/monan/{id} (Lấy 1 món để sửa)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonAn(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn == null)
            {
                return NotFound();
            }
            return Ok(monAn); // Trả về đầy đủ entity để edit
        }

        // POST: api/monan (Thêm mới)
        [HttpPost]
        public async Task<IActionResult> CreateMonAn([FromBody] MonAn model)
        {
            if (await _context.MonAns.AnyAsync(m => m.TenMon == model.TenMon))
            {
                return Conflict(new { message = "Tên món ăn đã tồn tại." });
            }

            var newMonAn = new MonAn
            {
                TenMon = model.TenMon,
                Gia = model.Gia,
                DonViTinh = model.DonViTinh,
                Loai = model.Loai,
                HinhAnh = model.HinhAnh // Tạm thời lưu URL ảnh
            };

            _context.MonAns.Add(newMonAn);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm món ăn thành công!", id = newMonAn.IdmonAn });
        }

        // PUT: api/monan/{id} (Cập nhật)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMonAn(int id, [FromBody] MonAn model)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn == null)
            {
                return NotFound(new { message = "Không tìm thấy món ăn." });
            }

            // Kiểm tra trùng tên (nhưng trừ chính nó ra)
            if (await _context.MonAns.AnyAsync(m => m.TenMon == model.TenMon && m.IdmonAn != id))
            {
                return Conflict(new { message = "Tên món ăn này đã bị trùng với một món khác." });
            }

            monAn.TenMon = model.TenMon;
            monAn.Gia = model.Gia;
            monAn.DonViTinh = model.DonViTinh;
            monAn.Loai = model.Loai;
            monAn.HinhAnh = model.HinhAnh;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công." });
        }

        // DELETE: api/monan/{id} (Xóa)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonAn(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn == null)
            {
                return NotFound(new { message = "Không tìm thấy món ăn." });
            }

            _context.MonAns.Remove(monAn);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Xóa món ăn thành công." });
        }
    }
}