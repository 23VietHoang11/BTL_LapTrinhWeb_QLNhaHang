// File: NhanVienIndexViewModel.cs

using RestaurantManagement.Models.Entities;
using System.Collections.Generic;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class NhanVienIndexViewModel
    {
        // Danh sách nhân viên (đã được lọc và phân trang)
        public IEnumerable<NhanVien> NhanViens { get; set; } = Enumerable.Empty<NhanVien>();

        // Số trang hiện tại
        public int PageNumber { get; set; } = 1;

        // Kích thước trang (số mục/trang)
        public int PageSize { get; set; } = 10;

        // Tổng số trang có thể có
        public int TotalPages { get; set; }

        // Từ khóa tìm kiếm hiện tại (để duy trì trạng thái)
        public string SearchTerm { get; set; } = string.Empty;
    }
}