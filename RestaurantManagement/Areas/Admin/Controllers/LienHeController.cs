using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using System;
using System.Threading.Tasks;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [ApiController]
    [Route("api/lienhe")] // <-- Route cho fetch('/api/lienhe')
    public class LienHeController : ControllerBase
    {
        private readonly QLNhaHangContext _context;

        public LienHeController(QLNhaHangContext context)
        {
            _context = context;
        }

        public class LienHeSubmitModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContact([FromBody] LienHeSubmitModel model)
        {
            try
            {
                if (model == null || !ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

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

                return Ok(new { success = true, message = "Gửi tin nhắn thành công!" });
            }
            catch (Exception ex)
            {
                string error = (ex as DbUpdateException)?.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { success = false, message = "Lỗi máy chủ: " + error });
            }
        }
    }
}
