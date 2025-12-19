using Microsoft.AspNetCore.Mvc;

namespace PatientApi.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
