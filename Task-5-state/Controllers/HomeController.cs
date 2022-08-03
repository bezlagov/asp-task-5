using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Task_5_state.Models;

namespace Task_5_state.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache _memoryCache;
        private const string Key = "users";
        private const string Session = "Session";
        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            List<string> value = new List<string>();
            if (!_memoryCache.TryGetValue(Key, out value))
            {
                value = new List<string>();
                HttpContext.Response.Cookies.Append(Session, HttpContext.Session.Id);
                value.Add(HttpContext.Session.Id);
                _memoryCache.Set(Key, value, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(1) });
            }
            else
            {
                if (!HttpContext.Request.Cookies.ContainsKey(Session))
                {
                    if (!value.Contains(HttpContext.Session.Id))
                    {
                        HttpContext.Response.Cookies.Append(Session, HttpContext.Session.Id, new Microsoft.AspNetCore.Http.CookieOptions() { Expires = new DateTimeOffset(DateTime.Now, TimeSpan.FromMinutes(1))});
                        value.Add(HttpContext.Session.Id);
                        _memoryCache.Set(Key, value, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(1) });
                    }
                }


            }
            return View(value.Count);
        }

        //Можно удалить юзера по закрытой вкладке или браузеру, но тогда набо решить проблему с обновлением страницы. По этому лучше использовать время жизни куки и сессии.
        /*
        public void Logout(string id)
        {
            List<string> value = new List<string>();
            if (_memoryCache.TryGetValue(Key, out value))
            {
                if (value.Contains(id))
                {
                    value.Remove(id);
                }
                _memoryCache.Set(Key, value, new MemoryCacheEntryOptions() { SlidingExpiration = TimeSpan.FromMinutes(5) });
            }
        }*/
    }
}
