using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.Entities;
using RestaurantManagement.Areas.Admin.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        private readonly QLNhaHangContext _context;

        public NhanVienController(QLNhaHangContext context)
        {
            _context = context;
        }

        // GET: Admin/NhanVien/Index
        // ĐÃ SỬA: Thêm tham số tìm kiếm và phân trang
        public async Task<IActionResult> Index(string searchTerm, int? pageNumber, int pageSize = 10)
        {
            if (pageSize < 1) pageSize = 10;

            // 1. Chuẩn bị truy vấn ban đầu (bao gồm TaiKhoan)
            var nhanViensQuery = _context.NhanViens
                .Include(nv => nv.TaiKhoan)
                .AsQueryable();

            // 2. Xử lý TÌM KIẾM (Filtering)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Lọc theo HoTenNv, Email, Sdt, ChucVu, TenDangNhap
                nhanViensQuery = nhanViensQuery.Where(nv =>
                    nv.HoTenNv.Contains(searchTerm) ||
                    nv.Email.Contains(searchTerm) ||
                    nv.Sdt.Contains(searchTerm) ||
                    nv.ChucVu.Contains(searchTerm) ||
                    (nv.TaiKhoan != null && nv.TaiKhoan.TenDangNhap.Contains(searchTerm))
                );
            }

            // 3. Xử lý PHÂN TRANG (Pagination)

            int totalItems = await nhanViensQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            int actualPageNumber = pageNumber ?? 1;
            if (actualPageNumber < 1) actualPageNumber = 1;
            if (actualPageNumber > totalPages && totalPages > 0) actualPageNumber = totalPages;

            int skipAmount = (actualPageNumber - 1) * pageSize;

            // Lấy dữ liệu cho trang hiện tại
            var paginatedNhanViens = await nhanViensQuery
                .OrderByDescending(nv => nv.IdnhanVien) // Sắp xếp
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync();

            // 4. Tạo ViewModel và trả về View
            var viewModel = new NhanVienIndexViewModel
            {
                NhanViens = paginatedNhanViens,
                PageNumber = actualPageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                SearchTerm = searchTerm ?? string.Empty
            };

            return View(viewModel);
        }
        // =================== CHỨC NĂNG THÊM MỚI (CREATE) ===================

        // GET: Admin/NhanVien/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(new NhanVienCreateViewModel());
        }

        // POST: Admin/NhanVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVienCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng tên đăng nhập
                if (await _context.TaiKhoans.AnyAsync(tk => tk.TenDangNhap == vm.TenDangNhap))
                {
                    ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã tồn tại.");
                    return View(vm);
                }

                CreatePasswordHash(vm.MatKhau, out byte[] hash, out byte[] salt);

                var newNV = new NhanVien
                {
                    HoTenNv = vm.HoTenNv,
                    Email = vm.Email,
                    Sdt = vm.Sdt,
                    DiaChi = vm.DiaChi,
                    ChucVu = vm.ChucVu,
                    NgaySinh = vm.NgaySinh,
                    Cccd = vm.Cccd,
                    GioiTinh = vm.GioiTinh,
                    NgayVaoLam = vm.NgayVaoLam,
                    Luong = vm.Luong
                };

                _context.NhanViens.Add(newNV);
                await _context.SaveChangesAsync(); // Lúc này newNV.IdnhanVien đã có giá trị

                // 4. Tạo và lưu Tài khoản (liên kết với ID nhân viên vừa tạo)
                CreatePasswordHash(vm.MatKhau, out byte[] MatKhauHash, out byte[] MatKhauSalt);
                var newTK = new TaiKhoan
                {
                    TenDangNhap = vm.TenDangNhap,
                    MatKhauHash = hash,
                    MatKhauSalt = salt,
                    TrangThai = "HoatDong",
                    NgayTao = DateTime.Now,
                    IDNhanVien = newNV.IdnhanVien
                };

                _context.TaiKhoans.Add(newTK);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Thêm mới nhân viên thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // =================== CHỨC NĂNG CẬP NHẬT (EDIT) ===================

        // GET: Admin/NhanVien/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var nv = await _context.NhanViens.Include(n => n.TaiKhoan)
                                   .FirstOrDefaultAsync(n => n.IdnhanVien == id);
            if (nv == null) return NotFound();

            var vm = new NhanVienEditViewModel
            {
                IdnhanVien = nv.IdnhanVien,
                TenDangNhap = nv.TaiKhoan?.TenDangNhap, // Chỉ để hiển thị
                HoTenNv = nv.HoTenNv,
                Email = nv.Email,
                Sdt = nv.Sdt,
                ChucVu = nv.ChucVu,
                NgaySinh = nv.NgaySinh,
                Cccd = nv.Cccd,
                GioiTinh = nv.GioiTinh,
                DiaChi = nv.DiaChi,
                NgayVaoLam = nv.NgayVaoLam,
                Luong = nv.Luong
            };
            return View(vm);
        }

        // POST: Admin/NhanVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVienEditViewModel vm)
        {
            if (id != vm.IdnhanVien) return NotFound();

            if (ModelState.IsValid)
            {
                var nvToUpdate = await _context.NhanViens.Include(n => n.TaiKhoan)
                                               .FirstOrDefaultAsync(n => n.IdnhanVien == id);
                if (nvToUpdate == null) return NotFound();

                // Cập nhật thông tin chung
                nvToUpdate.HoTenNv = vm.HoTenNv; nvToUpdate.Email = vm.Email;
                nvToUpdate.Sdt = vm.Sdt; nvToUpdate.DiaChi = vm.DiaChi;
                nvToUpdate.ChucVu = vm.ChucVu; nvToUpdate.NgaySinh = vm.NgaySinh;
                nvToUpdate.Cccd = vm.Cccd; nvToUpdate.GioiTinh = vm.GioiTinh;
                nvToUpdate.NgayVaoLam = vm.NgayVaoLam; nvToUpdate.Luong = vm.Luong;

                // Cập nhật mật khẩu NẾU người dùng có nhập vào ô MatKhauMoi
                if (!string.IsNullOrEmpty(vm.MatKhauMoi) && nvToUpdate.TaiKhoan != null)
                {
                    CreatePasswordHash(vm.MatKhauMoi, out byte[] hash, out byte[] salt);
                    nvToUpdate.TaiKhoan.MatKhauHash = hash;
                    nvToUpdate.TaiKhoan.MatKhauSalt = salt;
                }

                _context.Update(nvToUpdate);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Cập nhật thông tin thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // POST: Admin/NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv != null)
            {
                _context.NhanViens.Remove(nv);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}