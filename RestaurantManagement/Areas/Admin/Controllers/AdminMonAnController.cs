// Trong file: Controllers/AdminMonAnController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Models.ViewModels; // <-- Thêm ViewModel
using Microsoft.AspNetCore.Hosting; // <-- Thêm để lấy đường dẫn wwwroot

namespace RestaurantManagement.Controllers
{
    // Giả sử đây là trang Admin
    // [Area("Admin")] 
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
                    Gia = vm.GiaBan, // "Giá bán" -> "Gia"
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
    }
}