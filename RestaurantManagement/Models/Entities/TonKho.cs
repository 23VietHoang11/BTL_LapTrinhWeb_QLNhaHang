using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class TonKho
{
    public int Idkho { get; set; }

    public int IdnguyenLieu { get; set; }

    public double SoLuong { get; set; }

    public virtual Kho IdkhoNavigation { get; set; } = null!;

    public virtual NguyenLieu IdnguyenLieuNavigation { get; set; } = null!;
}
