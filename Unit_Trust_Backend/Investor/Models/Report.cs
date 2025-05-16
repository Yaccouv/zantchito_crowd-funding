using Microsoft.AspNetCore.Mvc;

namespace Unit_Trust_Backend.Investor.Models
{
    public class Report : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
