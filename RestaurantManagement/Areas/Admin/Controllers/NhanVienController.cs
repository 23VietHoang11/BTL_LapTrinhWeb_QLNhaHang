using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models.DTOs;
// Đảm bảo using namespace của Entities và DTOs
using RestaurantManagement.Models.Entities;
using System.Security.Cryptography; // Cần cho băm mật khẩu
using System.Text;

[Route("api/nhanvien")]
[ApiController]
public class NhanVienController : ControllerBase
{
    // YourDbContext là class DbContext của bạn
    private readonly QLNhaHangContext _context;

    public NhanVienController(QLNhaHangContext context)
    {
        _context = context;
    }

    // --- 1. THÊM NHÂN VIÊN MỚI (POST) ---
    // (Xử lý nút "Thêm nhân viên")
    [HttpPost]
    public async Task<IActionResult> ThemNhanVien([FromBody] NhanVienCreateModel model)
    {
        // Kiểm tra xem TenDangNhap hoặc Sdt đã tồn tại chưa
        if (await _context.TaiKhoans.AnyAsync(tk => tk.TenDangNhap == model.TenDangNhap))
        {
            return BadRequest(new { message = "Tên đăng nhập đã tồn tại." });
        }
        if (await _context.NhanViens.AnyAsync(nv => nv.Sdt == model.Sdt))
        {
            return BadRequest(new { message = "Số điện thoại đã tồn tại." });
        }

        // 1. Tạo Salt và Hash cho mật khẩu
        CreatePasswordHash(model.MatKhau, out byte[] hash, out byte[] salt);

        // 2. Tạo đối tượng NhanVien
        var newNhanVien = new NhanVien
        {
            HoTenNv = model.HoTenNv,
            Sdt = model.Sdt,
            Email = model.Email,
            DiaChi = model.DiaChi,
            ChucVu = model.ChucVu,
            NgayVaoLam = model.NgayVaoLam,
            Luong = model.Luong,
            NgaySinh = model.NgaySinh,
            Cccd = model.Cccd,
            GioiTinh = model.GioiTinh // (Nếu bạn đã thêm cột này vào NhanVien)
        };

        // 3. Tạo đối tượng TaiKhoan
        var newTaiKhoan = new TaiKhoan
        {
            TenDangNhap = model.TenDangNhap,
            MatKhauHash = hash,
            MatKhauSalt = salt,
            TrangThai = "HoatDong", // Gán trạng thái mặc định
            NgayTao = DateTime.Now,
            // 4. Liên kết 1-1
            IdnhanVienNavigation = newNhanVien // Gán nhân viên cho tài khoản
        };

        // 5. Lưu TaiKhoan (EF Core sẽ tự động lưu cả NhanVien liên quan)
        // Hoặc bạn có thể lưu NhanVien trước, tùy cấu hình CSDL
        _context.TaiKhoans.Add(newTaiKhoan);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Thêm nhân viên và tài khoản thành công!", id = newNhanVien.IdnhanVien });
    }

    // --- 2. CẬP NHẬT NHÂN VIÊN (PUT) ---
    // (Xử lý nút "Cập nhật thông tin")
    [HttpPut("{id}")]
    public async Task<IActionResult> CapNhatNhanVien(int id, [FromBody] NhanVienUpdateModel model)
    {
        // 1. Tìm NhanVien VÀ TaiKhoan liên quan
        var nhanVien = await _context.NhanViens
                                     .Include(nv => nv.TaiKhoan) // Lấy cả TaiKhoan
                                     .FirstOrDefaultAsync(nv => nv.IdnhanVien == id);

        if (nhanVien == null)
        {
            return NotFound(new { message = "Không tìm thấy nhân viên." });
        }

        // 2. Cập nhật thông tin NhanVien
        nhanVien.HoTenNv = model.HoTenNv;
        nhanVien.Sdt = model.Sdt;
        nhanVien.Email = model.Email;
        nhanVien.DiaChi = model.DiaChi;
        nhanVien.ChucVu = model.ChucVu;
        nhanVien.NgayVaoLam = model.NgayVaoLam;
        nhanVien.Luong = model.Luong;
        nhanVien.NgaySinh = model.NgaySinh;
        nhanVien.Cccd = model.Cccd;
        nhanVien.GioiTinh = model.GioiTinh; // (Nếu có)

        // 3. Cập nhật mật khẩu NẾU được cung cấp
        if (!string.IsNullOrEmpty(model.MatKhau))
        {
            if (nhanVien.TaiKhoan == null)
            {
                // Trường hợp hiếm: Nhân viên có nhưng tài khoản không
                return BadRequest(new { message = "Nhân viên này không có tài khoản để đổi mật khẩu." });
            }

            CreatePasswordHash(model.MatKhau, out byte[] hash, out byte[] salt);

            nhanVien.TaiKhoan.MatKhauHash = hash;
            nhanVien.TaiKhoan.MatKhauSalt = salt;
        }

        // 4. Lưu thay đổi
        await _context.SaveChangesAsync();
        return Ok(new { message = "Cập nhật thành công!" });
    }

    // --- 3. XÓA NHÂN VIÊN (DELETE) ---
    // (Xử lý nút "Xóa nhân viên")
    [HttpDelete("{id}")]
    public async Task<IActionResult> XoaNhanVien(int id)
    {
        var nhanVien = await _context.NhanViens.FindAsync(id);
        if (nhanVien == null)
        {
            return NotFound(new { message = "Không tìm thấy nhân viên." });
        }

        // Khi xóa NhanVien, CSDL (với quan hệ 1-1)
        // nên được cấu hình để tự động xóa (Cascade Delete) TaiKhoan
        _context.NhanViens.Remove(nhanVien);

        await _context.SaveChangesAsync();
        return Ok(new { message = "Xóa thành công!" });
    }

    // --- HÀM BĂM MẬT KHẨU (BẮT BUỘC) ---
    // Đặt hàm này ở cuối Controller
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));

        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    // --- 4. LẤY THÔNG TIN 1 NHÂN VIÊN (ĐỂ SỬA) ---
    [HttpGet("{id}")]
    public async Task<IActionResult> GetNhanVien(int id)
    {
        var nhanVien = await _context.NhanViens
                                     .Include(nv => nv.TaiKhoan) // Lấy cả TaiKhoan
                                     .Where(nv => nv.IdnhanVien == id)
                                     .Select(nv => new {
                                         // Từ bảng NhanVien
                                         nv.IdnhanVien,
                                         nv.HoTenNv,
                                         nv.Sdt,
                                         nv.Email,
                                         nv.DiaChi,
                                         nv.ChucVu,
                                         nv.NgayVaoLam,
                                         nv.Luong,
                                         nv.NgaySinh,
                                         nv.Cccd,
                                         // GioiTinh = nv.GioiTinh (Nếu bạn đã thêm cột này)

                                         // Từ bảng TaiKhoan (Không lấy mật khẩu!)
                                         TenDangNhap = nv.TaiKhoan != null ? nv.TaiKhoan.TenDangNhap : "",
                                         TrangThai = nv.TaiKhoan != null ? nv.TaiKhoan.TrangThai : ""
                                     })
                                     .FirstOrDefaultAsync();

        if (nhanVien == null)
        {
            return NotFound(new { message = "Không tìm thấy nhân viên." });
        }

        return Ok(nhanVien);
    }

    // --- 5. LẤY TẤT CẢ NHÂN VIÊN (ĐỂ HIỂN THỊ LIST) ---
    [HttpGet] // API này sẽ là: GET /api/nhanvien
    public async Task<IActionResult> GetAllNhanVien()
    {
        var danhSachNhanVien = await _context.NhanViens
            // Không cần .Include(nv => nv.TaiKhoan) nữa
            .Select(nv => new {
                // Lấy các trường bạn cần cho bảng
                Id = nv.IdnhanVien,
                HoTen = nv.HoTenNv,
                Email = nv.Email,
                Sdt = nv.Sdt,
                ChucVu = nv.ChucVu,

                // --- ĐÂY LÀ PHẦN ĐÚNG ---
                NgaySinh = nv.NgaySinh,
                Cccd = nv.Cccd,
                GioiTinh = nv.GioiTinh
            })
            .OrderByDescending(nv => nv.Id)
            .ToListAsync();

        return Ok(danhSachNhanVien);
    }
}

