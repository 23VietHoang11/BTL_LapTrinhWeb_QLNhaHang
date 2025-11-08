namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    // Model chứa các số liệu thống kê chính cho Dashboard
    public class DashboardViewModel
    {
        public int TotalAccounts { get; set; }
        public int TotalContacts { get; set; }
        public int TotalMenuItems { get; set; }
        public int TotalInvoices { get; set; }

        // Bạn có thể thêm các thuộc tính khác cho biểu đồ ở đây
    }
}