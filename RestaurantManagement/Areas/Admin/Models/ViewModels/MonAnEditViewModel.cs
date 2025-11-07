using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    // Kế thừa từ MonAnCreateViewModel để tái sử dụng các trường như TenMon, GiaBan...
    public class MonAnEditViewModel : MonAnCreateViewModel
    {
        public int Id { get; set; }

        // Đường dẫn ảnh hiện tại (để hiển thị nếu không up ảnh mới)
        public string? ExistingImage { get; set; }
    }
}
