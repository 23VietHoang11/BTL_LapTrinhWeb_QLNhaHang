using RestaurantManagement.Models.Entities;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class MonAnIndexViewModel
    {
        public IEnumerable<MonAn> MonAns { get; set; } // Danh sách món ăn đã được phân trang
        public int PageNumber { get; set; } = 1; // Số trang hiện tại
        public int PageSize { get; set; } = 10; // Kích thước trang
        public int TotalPages { get; set; } // Tổng số trang
        public string SearchTerm { get; set; } // Từ khóa tìm kiếm hiện tại
    }
}