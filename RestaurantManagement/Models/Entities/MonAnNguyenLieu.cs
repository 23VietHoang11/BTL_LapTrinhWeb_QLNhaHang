using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class MonAnNguyenLieu
{
    public int IdmonAn { get; set; }

    public int IdnguyenLieu { get; set; }

    public double SoLuongCan { get; set; }

    public virtual MonAn IdmonAnNavigation { get; set; } = null!;

    public virtual NguyenLieu IdnguyenLieuNavigation { get; set; } = null!;
}
