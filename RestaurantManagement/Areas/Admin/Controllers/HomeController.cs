using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoginAdmin()
        {
            return View();
        }
    }
}
