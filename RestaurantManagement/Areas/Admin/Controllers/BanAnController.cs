using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;

namespace RestaurantManagement.Controllers
{
    [Route("api/banan")]
    [ApiController]
    public class BanAnController : ControllerBase
    {
        private readonly QLNhaHangContext _context;
        public BanAnController(QLNhaHangContext context) { _context = context; }

        // GET: api/banan/available
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableTables()
        {
            // Chỉ lấy các bàn đang "Trống"
            var tables = await _context.BanAns
                .Where(b => b.TrangThai == "Trống")
                .Select(b => new { b.IdbanAn, b.LoaiBan, b.SucChua }) // Thêm SucChua nếu cần
                .ToListAsync();
            return Ok(tables);
        }
    }
}