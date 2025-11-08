using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Areas.Admin.Models.ViewModels;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NguyenLieuController : Controller
    {
        private readonly QLNhaHangContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NguyenLieuController(QLNhaHangContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/NguyenLieu
        // ĐÃ SỬA: Thêm tham số tìm kiếm và phân trang
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber, int pageSize = 10)
        {
            if (pageSize < 1) pageSize = 10;

            // 1. Chuẩn bị truy vấn ban đầu
            var nguyenLieuQuery = _context.NguyenLieus.AsQueryable();

            // 2. Xử lý TÌM KIẾM (Filtering)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Lọc theo Tên nguyên liệu hoặc Loại
                nguyenLieuQuery = nguyenLieuQuery.Where(nl =>
                    nl.TenNl.Contains(searchTerm) ||
                    nl.Loai.Contains(searchTerm)
                );
            }

            // 3. Xử lý PHÂN TRANG (Pagination)

            int totalItems = await nguyenLieuQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int actualPageNumber = pageNumber ?? 1;
            if (actualPageNumber < 1) actualPageNumber = 1;
            if (actualPageNumber > totalPages && totalPages > 0) actualPageNumber = totalPages;

            int skipAmount = (actualPageNumber - 1) * pageSize;

            // Lấy dữ liệu cho trang hiện tại
            var paginatedNguyenLieus = await nguyenLieuQuery
                .OrderByDescending(nl => nl.NgayNhap) // Giữ nguyên cách sắp xếp cũ
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

            // 4. Tạo ViewModel và trả về View
            var viewModel = new NguyenLieuIndexViewModel
            {
                NguyenLieus = paginatedNguyenLieus,
                PageNumber = actualPageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                SearchTerm = searchTerm ?? string.Empty
            };

            return View(viewModel);
        }

        // GET: Admin/NguyenLieu/Create
        public IActionResult Create()
        {
            return View(new NguyenLieuCreateViewModel());
        }

        // POST: Admin/NguyenLieu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NguyenLieuCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (vm.HinhAnh != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/nguyenlieu");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.HinhAnh.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.HinhAnh.CopyToAsync(fileStream);
                    }
                    uniqueFileName = "/images/nguyenlieu/" + uniqueFileName;
                }

                var nguyenLieu = new NguyenLieu
                {
                    TenNl = vm.TenNl,
                    DonVi = vm.DonVi,
                    Loai = vm.Loai,
                    SoLuong = vm.SoLuong,
                    NgayNhap = vm.NgayNhap,
                    HinhAnh = uniqueFileName
                };
                _context.Add(nguyenLieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: Admin/NguyenLieu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nl = await _context.NguyenLieus.FindAsync(id);
            if (nl == null) return NotFound();

            var vm = new NguyenLieuEditViewModel
            {
                IdnguyenLieu = nl.IdnguyenLieu,
                TenNl = nl.TenNl,
                DonVi = nl.DonVi,
                Loai = nl.Loai,
                SoLuong = nl.SoLuong ?? 0,
                NgayNhap = nl.NgayNhap,
                ExistingImage = nl.HinhAnh
            };
            return View(vm);
        }

        // POST: Admin/NguyenLieu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguyenLieuEditViewModel vm)
        {
            if (id != vm.IdnguyenLieu) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var nlToUpdate = await _context.NguyenLieus.FindAsync(id);
                    if (nlToUpdate == null) return NotFound();

                    nlToUpdate.TenNl = vm.TenNl;
                    nlToUpdate.DonVi = vm.DonVi;
                    nlToUpdate.Loai = vm.Loai;
                    nlToUpdate.SoLuong = vm.SoLuong;
                    nlToUpdate.NgayNhap = vm.NgayNhap;

                    if (vm.HinhAnh != null)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/nguyenlieu");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.HinhAnh.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await vm.HinhAnh.CopyToAsync(fileStream);
                        }
                        nlToUpdate.HinhAnh = "/images/nguyenlieu/" + uniqueFileName;
                    }

                    _context.Update(nlToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.NguyenLieus.Any(e => e.IdnguyenLieu == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // POST: Admin/NguyenLieu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nl = await _context.NguyenLieus.FindAsync(id);
            if (nl != null)
            {
                _context.NguyenLieus.Remove(nl);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}