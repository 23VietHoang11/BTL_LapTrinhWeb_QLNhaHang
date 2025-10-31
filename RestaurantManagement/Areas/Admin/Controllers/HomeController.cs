using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Areas.Admin.Models;

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
    }
}
