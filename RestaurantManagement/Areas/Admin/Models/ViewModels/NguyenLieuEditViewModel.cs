using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class NguyenLieuEditViewModel : NguyenLieuCreateViewModel
    {
        public int IdnguyenLieu { get; set; }
        public string? ExistingImage { get; set; }
    }
}
