using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Admin.Models.ViewModels;
using RestaurantManagement.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Admin.Models.ViewModels;
using RestaurantManagement.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using System; // <-- CẦN THIẾT cho DateTime.Now

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    // Thêm Route API vào đây để Controller xử lý cả request MVC và API
    [Route("api/[controller]")]
    [ApiController] // <-- Đánh dấu là API Controller để xử lý JSON
    public class LienHeController : Controller
    {
        private readonly QLNhaHangContext _context;

        // DTO để nhận dữ liệu từ Form Home (Giữ nguyên)
        public class LienHeSubmitModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
        }

        public LienHeController(QLNhaHangContext context)
        {
            _context = context;
        }

        // GET: Admin/Support/Index (Action mặc định)
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber, int pageSize = 10)
            {
                if (pageSize < 1) pageSize = 10;

                var lienHeQuery = _context.LienHes.AsQueryable();

                // 1. Xử lý TÌM KIẾM
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    lienHeQuery = lienHeQuery.Where(lh =>
                        lh.HoTen.Contains(searchTerm) ||
                        lh.Email.Contains(searchTerm) ||
                        lh.NoiDung.Contains(searchTerm)
                    );
                }

                // 2. Xử lý PHÂN TRANG
                int totalItems = await lienHeQuery.CountAsync();
                int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                int actualPageNumber = pageNumber ?? 1;
                if (actualPageNumber < 1) actualPageNumber = 1;
                if (actualPageNumber > totalPages && totalPages > 0) actualPageNumber = totalPages;

                int skipAmount = (actualPageNumber - 1) * pageSize;

                var paginatedLienHes = await lienHeQuery
                    .OrderByDescending(lh => lh.NgayGui)
                    .Skip(skipAmount)
                    .Take(pageSize)
                    .ToListAsync();

                // 3. Tạo ViewModel và trả về View
                var viewModel = new SupportIndexViewModel
                {
                    LienHes = paginatedLienHes,
                    PageNumber = actualPageNumber,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    SearchTerm = searchTerm ?? string.Empty
                };

                // Trả về View trong thư mục Views/Support/Index.cshtml
                return View(viewModel);
            }
        // ------------------------------------------------------------------
        // API POST: Gửi tin nhắn (Contact Form)
        // ------------------------------------------------------------------

        [HttpPost]
        [Route("~/api/lienhe")] // <-- Đảm bảo route tuyệt đối khớp với AJAX
        public async Task<IActionResult> SubmitContact([FromBody] LienHeSubmitModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
            }

            try
            {
                var newContact = new LienHe
                {
                    HoTen = model.Name,
                    Email = model.Email,
                    TieuDe = model.Subject,
                    NoiDung = model.Message,
                    NgayGui = DateTime.Now, // <-- Đã có using System;
                    DaDoc = false
                };

                _context.LienHes.Add(newContact);
                await _context.SaveChangesAsync();

                // Trả về mã 200 OK
                return Ok(new { success = true, message = "Gửi tin nhắn thành công!" });
            }
            catch (Exception ex)
            {
                // Bắt lỗi Database hoặc lỗi khác và trả về 500
                return StatusCode(500, new { success = false, message = "Lỗi khi lưu Database: " + ex.Message });
            }
        }

        // ------------------------------------------------------------------
        // API GET: Lấy danh sách tin nhắn (Dùng cho trang Admin)
        // ------------------------------------------------------------------

        [HttpGet] // 🛑 ĐÃ SỬA LỖI: Chỉ giữ lại một [HttpGet] duy nhất
            public async Task<IActionResult> GetAllMessages()
            {
                var messages = await _context.LienHes
                    .OrderByDescending(m => m.NgayGui)
                    .Select(m => new {
                        // ĐẢM BẢO TÊN THUỘC TÍNH KHỚP VỚI JAVASCRIPT
                        idLienHe = m.Id,
                        hoTen = m.HoTen,
                        email = m.Email,
                        tieuDe = m.TieuDe,
                        noiDung = m.NoiDung,
                        ngayGui = m.NgayGui,
                        daDoc = m.DaDoc
                    })
                    .ToListAsync();

                return Ok(messages);
            }

            // ------------------------------------------------------------------
            // API PUT: Đánh dấu đã đọc (Chức năng SỬA)
            // ------------------------------------------------------------------

            // PUT: api/lienhe/markread/5
            [HttpPut("markread/{id}")]
            public async Task<IActionResult> MarkAsRead(int id)
            {
                var message = await _context.LienHes.FindAsync(id);

                if (message == null)
                {
                    return NotFound(new { message = "Không tìm thấy tin nhắn." });
                }

                message.DaDoc = true;
                _context.LienHes.Update(message);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Tin nhắn đã được đánh dấu là đã đọc." });
            }

            // ------------------------------------------------------------------
            // API DELETE: Xóa tin nhắn
            // ------------------------------------------------------------------

            // DELETE: api/lienhe/5
            [HttpPost, ActionName("Delete")] // Bắt yêu cầu POST từ form có asp-action="Delete"
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var message = await _context.LienHes.FindAsync(id);

                if (message != null)
                {
                    _context.LienHes.Remove(message);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Tin nhắn đã được xóa thành công."; // Tùy chọn
                }

                // Chuyển hướng trở lại trang Index để tải lại danh sách
                return RedirectToAction(nameof(Index));
            }
        }
    }
