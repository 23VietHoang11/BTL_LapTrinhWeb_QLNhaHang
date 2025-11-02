using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity; // (Dùng cho Hashing)
using RestaurantManagement.Data; // (DbContext của bạn)
using RestaurantManagement.Models;
using RestaurantManagement.Models.Entities; // (Các Model của bạn)

// (Tạo 2 Model này để nhận dữ liệu từ Form)
public class LoginViewModel
{
    public string TenDangNhap { get; set; }
    public string MatKhau { get; set; }
}

public class RegisterViewModel
{
    public int IDNhanVien { get; set; } // Giả sử bạn có 1 nhân viên ID
    public string TenDangNhap { get; set; }
    public string MatKhau { get; set; }
    public int IDQuyen { get; set; } // Giả sử đăng ký 1 quyền
}
namespace RestaurantManagement.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly QLNhaHangContext _context;
        private readonly PasswordHasher<TaiKhoan> _hasher;

        public TaiKhoanController(QLNhaHangContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<TaiKhoan>();
        }
        // ========== CHỨC NĂNG ĐĂNG KÝ (SIGN UP) ==========

        [HttpGet]
        public IActionResult Register()
        {
            return View(); // Trả về View "Register.cshtml" (form đăng ký)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Kiểm tra xem Tên đăng nhập đã tồn tại chưa
            if (await _context.TaiKhoans.AnyAsync(tk => tk.TenDangNhap == model.TenDangNhap))
            {
                ModelState.AddModelError("TenDangNhap", "Tên đăng nhập này đã tồn tại.");
                return View(model);
            }

            // 1. Tạo Tài khoản mới
            var newAccount = new TaiKhoan
            {
                IDNhanVien = model.IDNhanVien,
                TenDangNhap = model.TenDangNhap,
                TrangThai = "Đang hoạt động", // Kích hoạt luôn
                NgayTao = DateTime.Now
            };

            // 2. BĂM MẬT KHẨU (Rất quan trọng)
            newAccount.MatKhau = _hasher.HashPassword(newAccount, model.MatKhau);

            _context.TaiKhoans.Add(newAccount);
            await _context.SaveChangesAsync(); // Lưu để lấy IDTaiKhoan

            // 3. TÌM QUYỀN (từ database)
            // (Dùng tên DbSet số nhiều "Quyens" mà bạn vừa gửi ảnh)
            var role = await _context.Quyens.FindAsync(model.IDQuyen);

            // 4. KẾT NỐI TÀI KHOẢN VÀ QUYỀN
            if (role != null)
            {
                // Dùng thuộc tính điều hướng bạn đã hỏi
                // EF Core sẽ tự động tạo 1 dòng trong bảng TaiKhoanQuyen
                newAccount.Idquyens.Add(role);
            }
            else
            {
                // (Nên thêm báo lỗi nếu không tìm thấy Quyền)
                ModelState.AddModelError("IDQuyen", "Quyền được chọn không hợp lệ.");
                return View(model);
            }

            // 5. LƯU TẤT CẢ (Chỉ cần 1 lần SaveChanges)
            // (Dùng tên DbSet số nhiều "TaiKhoans" mà bạn vừa gửi ảnh)
            _context.TaiKhoans.Add(newAccount);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login"); // Đăng ký xong, chuyển về trang đăng nhập
        }

        // ========== CHỨC NĂNG ĐĂNG NHẬP (LOGIN) ==========

        [HttpGet]
        public IActionResult Login()
        {
            return View(); // Trả về View "Login.cshtml" (form đăng nhập)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Tìm tài khoản
            var user = await _context.TaiKhoans
                .FirstOrDefaultAsync(tk => tk.TenDangNhap == model.TenDangNhap);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            // 2. KIỂM TRA MẬT KHẨU ĐÃ BĂM
            var passwordVerificationResult = _hasher.VerifyHashedPassword(user, user.MatKhau, model.MatKhau);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản hoặc mật khẩu không đúng.");
                return View(model);
            }

            // 3. Lấy Quyền (Roles) của user
            var roles = user.Idquyens         // <-- Lấy danh sách Quyen từ user
                   .Select(q => q.TenQuyen) // <-- Chọn ra TenQuyen
                   .ToList();

            // 4. TẠO COOKIE
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.TenDangNhap),
            new Claim(ClaimTypes.NameIdentifier, user.IDTaiKhoan.ToString())
            // Thêm các Quyền vào Cookie
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // "Remember me"
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
            };

            // 5. Thực hiện đăng nhập, tạo cookie
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home", new { area = "Admin" }); // Chuyển đến trang Dashboard Admin
        }

        // ========== CHỨC NĂNG ĐĂNG XUẤT (LOGOUT) ==========

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account"); // Về trang đăng nhập
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View(); // Trả về trang "AccessDenied.cshtml"
        }

    }
}
