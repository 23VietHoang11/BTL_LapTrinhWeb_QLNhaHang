using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class Quyen
{
    public int IDQuyen { get; set; }

    public string TenQuyen { get; set; } = null!;

    public string? MoTa { get; set; }

    public virtual ICollection<TaiKhoan> IdtaiKhoans { get; set; } = new List<TaiKhoan>();
}
