using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
