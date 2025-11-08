using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models.Entities
{
    public class LienHe
    {
        // Khóa chính (Primary Key) và là Identity Column
        [Key]
        public int Id { get; set; }

        // Tên người gửi (Bắt buộc)
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty; // Khắc phục CS8618

        // Email (Bắt buộc)
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [StringLength(100)]
        [EmailAddress] // Thêm thuộc tính validation Email
        public string Email { get; set; } = string.Empty;

        // Tiêu đề
        [StringLength(200)]
        public string TieuDe { get; set; } = string.Empty;

        // Nội dung tin nhắn (Bắt buộc)
        [Required(ErrorMessage = "Vui lòng nhập nội dung.")]
        public string NoiDung { get; set; } = string.Empty;

        // Ngày gửi (Giá trị mặc định sẽ được đặt trong EF Core hoặc SQL Server)
        public DateTime NgayGui { get; set; } = DateTime.Now;

        // Trạng thái đã đọc/chưa đọc
        public bool DaDoc { get; set; } = false;

        // 🛑 ĐÃ XÓA: public object IdlienHe { get; internal set; }
    }
}