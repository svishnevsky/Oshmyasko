using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Oshmyasko.Clients.Web.Controllers
{
    [Authorize]
    public class CategoriesController  : Controller
    {
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }
    }
}
