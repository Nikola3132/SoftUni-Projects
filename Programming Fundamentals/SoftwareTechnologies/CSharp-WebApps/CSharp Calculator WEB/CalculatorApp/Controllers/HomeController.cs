using CalculatorApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index(CalculatorModel calculator)
        {
            return View(calculator);
        }
        [HttpPost]
        public ActionResult Calculate(CalculatorModel calculator)
        {
            calculator.CalculateResult();
            return RedirectToAction("Index", calculator);
        }

    }
}
