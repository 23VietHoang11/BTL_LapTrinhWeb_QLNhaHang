using RestaurantManagement.Models.Entities;
using System.Collections.Generic;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class NguyenLieuIndexViewModel
    {
        public IEnumerable<NguyenLieu> NguyenLieus { get; set; } = Enumerable.Empty<NguyenLieu>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
    }
}