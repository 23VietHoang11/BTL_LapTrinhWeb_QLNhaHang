// File: HoaDonIndexViewModel.cs

using RestaurantManagement.Models.Entities;
using System.Collections.Generic;
using System.Linq; // Thêm thư viện này nếu cần dùng Enumerable.Empty<HoaDon>()

namespace RestaurantManagement.Areas.Admin.Models.ViewModels // <--- PHẢI KHỚP
{
    public class HoaDonIndexViewModel
    {
        public IEnumerable<HoaDon> HoaDons { get; set; } = Enumerable.Empty<HoaDon>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
    }
}