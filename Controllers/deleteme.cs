using Microsoft.AspNetCore.Mvc;

namespace LineService.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
