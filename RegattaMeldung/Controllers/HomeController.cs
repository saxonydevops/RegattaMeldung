using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;

namespace RegattaMeldung.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string guid)
        {
            var model = _context.Regattas.Include(r => r.Club).Include(r => r.Waters);

            ViewBag.Guid = guid;

            return View(model);
        }

        public IActionResult About(string guid)
        {
            ViewBag.Guid = guid;
            return View();
        }

        public IActionResult GDPR(string guid)
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
