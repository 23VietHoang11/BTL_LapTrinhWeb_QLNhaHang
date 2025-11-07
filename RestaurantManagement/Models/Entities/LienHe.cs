using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models.Entities
{
    public class LienHe
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string TieuDe { get; set; } // Subject

        [Required]
        public string NoiDung { get; set; } // Message

        public DateTime NgayGui { get; set; } = DateTime.Now;

        public bool DaDoc { get; set; } = false; // Để Admin theo dõi
    }
}