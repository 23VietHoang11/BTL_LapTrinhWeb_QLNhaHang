using System.Collections.Generic;
using RestaurantManagement.Models.Entities;

namespace RestaurantManagement.Models.ViewModels
{
    public class DatBanViewModel
    {
        public DatBan DatBan { get; set; }
        public KhachHang KhachHang { get; set; }
        public List<BanAn> DanhSachBan { get; set; }
    }
}
