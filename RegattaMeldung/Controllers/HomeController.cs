using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegattaMeldung.Models;

namespace RegattaMeldung.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(string guid)
        {
            ViewBag.Guid = guid;
            return View();
        }

        public IActionResult About(string guid)
        {
            ViewBag.Guid = guid;
            return View();
        }

        public IActionResult Help(string guid)
        {
            ViewBag.Guid = guid;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
