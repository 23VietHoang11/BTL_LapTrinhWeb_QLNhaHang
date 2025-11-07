using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;

namespace RestaurantManagement.Controllers
{
    [Route("api/khachhang")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly QLNhaHangContext _context;
        public KhachHangController(QLNhaHangContext context) { _context = context; }

        // GET: api/khachhang
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _context.KhachHangs
                .Select(k => new { k.IdkhachHang, k.HoTenKh, k.Sdt })
                .ToListAsync();
            return Ok(customers);
        }
    }
}