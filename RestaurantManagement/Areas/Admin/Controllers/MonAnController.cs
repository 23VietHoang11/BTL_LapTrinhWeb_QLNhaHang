//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using RestaurantManagement.Models.Entities;
//using RestaurantManagement.Models.ViewModels; // Đảm bảo bạn đã có ViewModel này
//using Microsoft.AspNetCore.Hosting;
//using System;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;

//namespace RestaurantManagement.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class MonAnController : Controller
//    {
//        private readonly QLNhaHangContext _context;
//        private readonly IWebHostEnvironment _webHostEnvironment;

//        public MonAnController(QLNhaHangContext context, IWebHostEnvironment webHostEnvironment)
//        {
//            _context = context;
//            _webHostEnvironment = webHostEnvironment;
//        }

//        // =====================================================================
//        // PHẦN 1: MVC ACTIONS (Trả về View & Xử lý Form truyền thống)
//        // =====================================================================

//        // GET: Admin/MonAn/Index
//        [HttpGet]
//        public IActionResult Index()
//        {
//            return View(); // Trả về view trống, Javascript sẽ gọi API để tải dữ liệu sau
//        }

//        // GET: Admin/MonAn/Create
//        [HttpGet]
//        public IActionResult Create()
//        {
//            return View(new MonAnCreateViewModel());
//        }

//        // POST: Admin/MonAn/Create
//        // Xử lý submit form thêm mới (bao gồm upload file ảnh)
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(MonAnCreateViewModel vm)
//        {
//            if (ModelState.IsValid)
//            {
//                string uniqueFileName = null;
//                if (vm.ImageFile != null)
//                {
//                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/monan");
//                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
//                    uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.ImageFile.FileName;
//                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
//                    using (var fileStream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await vm.ImageFile.CopyToAsync(fileStream);
//                    }
//                    uniqueFileName = "/images/monan/" + uniqueFileName;
//                }

//                var newMonAn = new MonAn
//                {
//                    TenMon = vm.TenMon,
//                    Loai = vm.Loai,
//                    DonViTinh = vm.DonViTinh,
//                    Gia = vm.GiaBan,
//                    HinhAnh = uniqueFileName
//                };

//                _context.MonAns.Add(newMonAn);
//                await _context.SaveChangesAsync();
//                TempData["SuccessMessage"] = "Thêm món ăn thành công!";
//                return RedirectToAction(nameof(Index));
//            }
//            return View(vm);
//        }

//        // =====================================================================
//        // PHẦN 2: API ACTIONS (Trả về JSON cho JavaScript gọi)
//        // =====================================================================

//        // GET: /api/monan
//        // API lấy danh sách món ăn
//        [HttpGet("/api/monan")]
//        public async Task<IActionResult> GetAllMonAnApi()
//        {
//            var monAns = await _context.MonAns
//                .Select(m => new {
//                    m.IdmonAn,
//                    m.TenMon,
//                    m.Gia,
//                    m.Loai,
//                    m.HinhAnh,
//                    m.DonViTinh
//                })
//                .OrderBy(m => m.TenMon)
//                .ToListAsync();
//            return Ok(monAns);
//        }

//        // GET: /api/monan/{id}
//        // API lấy chi tiết 1 món ăn
//        [HttpGet("/api/monan/{id}")]
//        public async Task<IActionResult> GetMonAnApi(int id)
//        {
//            var monAn = await _context.MonAns.FindAsync(id);
//            if (monAn == null) return NotFound(new { message = "Không tìm thấy món ăn." });
//            return Ok(monAn);
//        }

//        // DELETE: /api/monan/{id}
//        // API xóa món ăn
//        [HttpDelete("/api/monan/{id}")]
//        public async Task<IActionResult> DeleteMonAnApi(int id)
//        {
//            var monAn = await _context.MonAns.FindAsync(id);
//            if (monAn == null) return NotFound(new { message = "Không tìm thấy món ăn." });

//            // Tùy chọn: Xóa file ảnh cũ nếu cần
//            // if (!string.IsNullOrEmpty(monAn.HinhAnh)) { ... logic xóa file ... }

//            _context.MonAns.Remove(monAn);
//            await _context.SaveChangesAsync();
//            return Ok(new { message = "Xóa món ăn thành công." });
//        }

//        // PUT: /api/monan/{id}
//        // API cập nhật (nếu bạn muốn dùng JS để sửa nhanh tên, giá...)
//        [HttpPut("/api/monan/{id}")]
//        public async Task<IActionResult> UpdateMonAnApi(int id, [FromBody] MonAn model)
//        {
//            if (id != model.IdmonAn) return BadRequest();
//            var monAn = await _context.MonAns.FindAsync(id);
//            if (monAn == null) return NotFound();

//            monAn.TenMon = model.TenMon;
//            monAn.Gia = model.Gia;
//            monAn.DonViTinh = model.DonViTinh;
//            monAn.Loai = model.Loai;
//            // Lưu ý: API này nhận JSON nên khó xử lý upload file mới.
//            // Nếu sửa có ảnh, nên dùng Form MVC hoặc API upload riêng.
//            if (!string.IsNullOrEmpty(model.HinhAnh)) monAn.HinhAnh = model.HinhAnh;

//            await _context.SaveChangesAsync();
//            return Ok(new { message = "Cập nhật thành công." });
//        }
//    }
//}