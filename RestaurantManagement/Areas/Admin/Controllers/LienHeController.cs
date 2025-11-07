using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Controllers
{
    [Route("api/lienhe")]
    [ApiController]
    public class LienHeController : ControllerBase
    {
        private readonly QLNhaHangContext _context;
        public LienHeController(QLNhaHangContext context) { _context = context; }

        // DTO để nhận dữ liệu từ Form Home
        public class LienHeSubmitModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }

        // POST: api/lienhe (Dùng cho trang Home)
        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] LienHeSubmitModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }

            var newContact = new LienHe
            {
                HoTen = model.Name,
                Email = model.Email,
                TieuDe = model.Subject,
                NoiDung = model.Message,
                NgayGui = DateTime.Now,
                DaDoc = false
            };

            _context.LienHes.Add(newContact);
            await _context.SaveChangesAsync();

            // Trả về Ok() (200) thay vì JSON để tránh lỗi "Unexpected end of JSON"
            return Ok();
        }

        // GET: api/lienhe (Dùng cho trang Admin)
        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _context.LienHes
                .OrderByDescending(m => m.NgayGui)
                .Select(m => new {
                    m.Id,
                    m.HoTen,
                    m.Email,
                    m.TieuDe,
                    m.NoiDung,
                    m.NgayGui,
                    m.DaDoc
                })
                .ToListAsync();

            return Ok(messages);
        }
    }
}