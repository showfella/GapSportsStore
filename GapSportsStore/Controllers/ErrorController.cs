using Microsoft.AspNetCore.Mvc;

namespace GapSportsStore.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Error() => View();
    }
}