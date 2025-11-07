using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Areas.Admin.Models;
using RestaurantManagement.Models.Entities;
using System.Data.Common;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly QLNhaHangContext _context;
        public HomeController(QLNhaHangContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }


        [HttpGet]
        public IActionResult RegisterAcc()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterAcc(DangKiNV model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("LoginAdmin");
            }
            return View(model);
        }

        // Ví dụ: BookingViewModel.cs

        
    }
}
