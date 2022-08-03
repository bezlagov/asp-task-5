using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace SetState.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string data = null, string date=null)
        {
            if(data!=null && date!=null)
            {
                HttpContext.Response.Cookies.Append("test", data, new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Parse(date) });
                ViewBag.Data = data + " " + date.ToString();
            }

            return View();
        }
    }
}
