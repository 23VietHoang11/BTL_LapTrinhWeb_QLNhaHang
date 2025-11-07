
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.ViewModels; // <-- Thêm ViewModel
using Microsoft.AspNetCore.Hosting;
using RestaurantManagement.Areas.Admin.Models.ViewModels; // <-- Thêm để lấy đường dẫn wwwroot

namespace RestaurantManagement.Controllers
{
    [Area("Admin")] 
    public class AdminMonAnController : Controller
    {
        private readonly QLNhaHangContext _context;

        // Dùng IWebHostEnvironment để xử lý upload file
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Yêu cầu (inject) cả 2 dịch vụ này
        public AdminMonAnController(QLNhaHangContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /AdminMonAn/Index
        // (Trang "Danh sách món ăn" - Ảnh 1)
        public async Task<IActionResult> Index()
        {
            var monAns = await _context.MonAns.ToListAsync();
            // Bạn cần tạo View cho trang Index này
            return View(monAns);
        }

        // GET: /AdminMonAn/Create
        // (Trang "Thêm mặt hàng mới" - Ảnh 2)
        public IActionResult Create()
        {
            // Chỉ cần trả về View rỗng với ViewModel
            return View(new MonAnCreateViewModel());
        }

        // POST: /AdminMonAn/Create
        // (Đây là backend khi bạn nhấn nút "Lưu lại")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonAnCreateViewModel vm)
        {
            // Kiểm tra xem dữ liệu form có hợp lệ không
            if (ModelState.IsValid)
            {
                string uniqueFileName = null; // Tên file ảnh sẽ lưu trong CSDL

                // ===== 1. XỬ LÝ UPLOAD HÌNH ẢNH =====
                if (vm.ImageFile != null)
                {
                    // Lấy đường dẫn thư mục wwwroot/images/monan
                    // Bạn cần tạo thư mục "images" và "monan" trong wwwroot
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/monan");

                    // Đảm bảo thư mục tồn tại
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Tạo tên file duy nhất (để tránh trùng lặp)
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.ImageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Lưu file vào máy chủ
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.ImageFile.CopyToAsync(fileStream);
                    }

                    // Cập nhật đường dẫn để lưu vào CSDL
                    uniqueFileName = "/images/monan/" + uniqueFileName;
                }
                // ======================================

                // 2. Map ViewModel sang Model (MonAn)
                MonAn newMonAn = new MonAn
                {
                    TenMon = vm.TenMon,
                    Loai = vm.Loai,
                    DonViTinh = vm.DonViTinh,
                    Gia = vm.Gia, // "Giá bán" -> "Gia"
                    HinhAnh = uniqueFileName // Lưu đường dẫn tương đối

                    // Lưu ý: Model MonAn của bạn không có "GhiChu" và "GiaVon"
                    // Nếu muốn lưu, bạn phải thêm 2 trường này vào MonAn.cs
                    // và chạy migration một lần nữa.
                };

                // 3. Lưu vào CSDL
                _context.MonAns.Add(newMonAn);
                await _context.SaveChangesAsync();

                // 4. Chuyển hướng về trang Danh sách (Ảnh 1)
                return RedirectToAction(nameof(Index));
            }


            // Nếu model không hợp lệ (ví dụ thiếu Tên), trả lại form
            return View(vm);
        }
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

            // Chuyển dữ liệu từ Entity sang ViewModel để hiển thị lên form
            var vm = new MonAnEditViewModel
            {
                Id = monAn.IdmonAn,
                TenMon = monAn.TenMon,
                Loai = monAn.Loai,
                DonViTinh = monAn.DonViTinh,
                Gia = monAn.Gia,
                ExistingImage = monAn.HinhAnh
            };

            return View(vm);
        }

        // POST: /AdminMonAn/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonAnEditViewModel vm)
        {
            if (id != vm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var monAnToUpdate = await _context.MonAns.FindAsync(id);
                    if (monAnToUpdate == null)
                    {
                        return NotFound();
                    }

                    // 1. Cập nhật thông tin cơ bản
                    monAnToUpdate.TenMon = vm.TenMon;
                    monAnToUpdate.Loai = vm.Loai;
                    monAnToUpdate.DonViTinh = vm.DonViTinh;
                    monAnToUpdate.Gia = vm.Gia;

                    // 2. Xử lý ảnh mới (NẾU CÓ upload)
                    if (vm.ImageFile != null)
                    {
                        
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/monan");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.ImageFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await vm.ImageFile.CopyToAsync(fileStream);
                        }

                        // Cập nhật đường dẫn mới vào CSDL
                        monAnToUpdate.HinhAnh = "/images/monan/" + uniqueFileName;
                    }
                    // Nếu không up ảnh mới, giữ nguyên monAnToUpdate.HinhAnh cũ

                    _context.Update(monAnToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MonAns.Any(e => e.IdmonAn == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn != null)
            {
                // (Tùy chọn) Xóa file ảnh khỏi server nếu cần
                // if (!string.IsNullOrEmpty(monAn.HinhAnh))
                // {
                //     string filePath = Path.Combine(_webHostEnvironment.WebRootPath, monAn.HinhAnh.TrimStart('/'));
                //     if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);
                // }

                _context.MonAns.Remove(monAn);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}