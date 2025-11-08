// File: DatBanIndexViewModel.cs

using RestaurantManagement.Models.ViewModels;
using System.Collections.Generic;

namespace RestaurantManagement.Models.ViewModels
{
    public class DatBanIndexViewModel
    {
        // Danh sách đặt bàn (đã được lọc và phân trang)
        public IEnumerable<DatBanAdminViewModel> DatBans { get; set; } = Enumerable.Empty<DatBanAdminViewModel>();

        // Thông tin phân trang và tìm kiếm
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } = string.Empty; // Để duy trì giá trị tìm kiếm
    }
}