using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly QLNhaHangContext _context;

        public MenuController(QLNhaHangContext context)
        {
            _context = context;
        }

        // GET: Admin/Menu (Trang danh sách, tìm kiếm, phân trang)
        public async Task<IActionResult> Index(
            string searchString,
            string category,
            int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentCategory"] = category;

            var query = _context.MonAns.AsQueryable();

            // 1. Lọc theo Tên (Tìm kiếm)
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(m => m.TenMon.Contains(searchString));
            }

            // 2. Lọc theo Danh mục (Loại)
            if (!string.IsNullOrEmpty(category) && category != "Tất cả")
            {
                query = query.Where(m => m.Loai == category);
            }

            // 3. Phân trang
            int pageSize = 5; // Số lượng món ăn trên mỗi trang
            var data = await query.OrderByDescending(m => m.IdmonAn).ToListAsync();

            // Tạo danh sách danh mục để lọc
            ViewBag.Categories = new List<string> { "Tất cả", "Thức ăn", "Đồ uống" };

            // Sử dụng một ViewModel hoặc PagedList (tạm thời dùng ViewData/ViewBag)
            int pageIndex = pageNumber ?? 1;
            var pagedData = data.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            ViewBag.TotalPages = (int)Math.Ceiling(data.Count / (double)pageSize);
            ViewBag.PageIndex = pageIndex;

            return View(pagedData); // Trả về View Index.cshtml với dữ liệu đã phân trang
        }

        // GET: Admin/Menu/Create (Trang hiển thị form Thêm mới)
        public IActionResult Create()
        {
            // Trả về View Create.cshtml
            return View();
        }

        // POST: Admin/Menu/Create (Xử lý khi submit form Thêm mới)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("TenMon, Gia, DonViTinh, Loai")] MonAn monAn)
        {
            // Lưu ý: Form của bạn có nhiều trường (Ghi chú, Giá vốn...)
            // nhưng Model MonAn.cs chỉ có 4 trường này.
            // Tôi chỉ lưu 4 trường có trong Model.

            // Bạn cũng có form upload ảnh, nhưng Model MonAn.cs không có trường HinhAnh.
            // Bạn cần tự thêm trường HinhAnh (string) vào MonAn.cs
            // và code logic xử lý upload file (IFormFile) ở đây.

            if (ModelState.IsValid)
            {
                _context.Add(monAn);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm món ăn mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            // Nếu lỗi, trả lại form với dữ liệu đã nhập
            return View(monAn);
        }

        // GET: Admin/Menu/Edit/5 (Trang hiển thị form Sửa)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn == null)
            {
                return NotFound();
            }
            // Trả về View Edit.cshtml
            return View(monAn);
        }

        // POST: Admin/Menu/Edit/5 (Xử lý khi submit form Sửa)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("IdmonAn, TenMon, Gia, DonViTinh, Loai")] MonAn monAn)
        {
            if (id != monAn.IdmonAn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monAn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MonAns.Any(e => e.IdmonAn == monAn.IdmonAn))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Cập nhật món ăn thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(monAn);
        }

        // POST: Admin/Menu/Delete/5 (Xử lý xóa 1 món)
        // Dùng [HttpPost] để bảo mật, tránh bị xóa bằng link GET
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn != null)
            {
                _context.MonAns.Remove(monAn);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa món ăn thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Menu/DeleteSelected (Xử lý xóa nhiều mục đã chọn)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSelected(int[] selectedIds)
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn ít nhất một món ăn để xóa.";
                return RedirectToAction(nameof(Index));
            }

            var itemsToDelete = await _context.MonAns
                                        .Where(m => selectedIds.Contains(m.IdmonAn))
                                        .ToListAsync();

            if (itemsToDelete.Any())
            {
                _context.MonAns.RemoveRange(itemsToDelete);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã xóa thành công {itemsToDelete.Count} món ăn.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}