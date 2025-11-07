using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StaffController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
    }
}
