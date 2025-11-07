using System;
using System.Collections.Generic;

namespace RestaurantManagement.Models.Entities;

public partial class BanAn
{
    public int IdbanAn { get; set; }

    public int? SucChua { get; set; }

    public string? TrangThai { get; set; }

    public string? LoaiBan { get; set; }


    public virtual ICollection<DatBan> DatBans { get; set; } = new List<DatBan>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
