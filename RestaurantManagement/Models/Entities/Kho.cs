using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class Kho
{
    public int Idkho { get; set; }

    public string TenKho { get; set; } = null!;

    public string? DiaChi { get; set; }

    public virtual ICollection<TonKho> TonKhos { get; set; } = new List<TonKho>();
}
