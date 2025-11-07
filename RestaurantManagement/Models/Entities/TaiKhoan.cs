using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class TaiKhoan
{
    public int IDTaiKhoan { get; set; }
    public int IDNhanVien { get; set; }
    public string TenDangNhap { get; set; } = null!;

    // --- BẮT ĐẦU SỬA ---
    // XÓA DÒNG NÀY:
    // public string MatKhau { get; set; } = null!;

    // THÊM 2 DÒNG NÀY:
    public byte[] MatKhauHash { get; set; } = null!;
    public byte[] MatKhauSalt { get; set; } = null!;
    // --- KẾT THÚC SỬA ---

    public string? TrangThai { get; set; }
    public DateTime? NgayTao { get; set; }
    public virtual NhanVien IdnhanVienNavigation { get; set; } = null!;
    public virtual ICollection<Quyen> Idquyens { get; set; } = new List<Quyen>();
}