using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
