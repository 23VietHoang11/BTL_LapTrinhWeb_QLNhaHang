// File: LienHeIndexViewModel.cs

using RestaurantManagement.Models.Entities;
using System;
using System.Collections.Generic;

namespace RestaurantManagement.Areas.Admin.Models.ViewModels
{
    public class SupportIndexViewModel
    {
        public IEnumerable<LienHe> LienHes { get; set; } = Enumerable.Empty<LienHe>();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; }
        public string SearchTerm { get; set; } = string.Empty;
    }
}