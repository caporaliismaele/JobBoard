using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class JobOfferController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
