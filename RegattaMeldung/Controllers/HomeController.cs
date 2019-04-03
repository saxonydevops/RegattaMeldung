using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;
using RegattaMeldung.ViewModels;

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

        [HttpGet]
        public IActionResult Thankyou()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Subscribe(int id)
        {
            ClubVM clubvm = new ClubVM();            

            List<int> clubids = _context.RegattaClubs.Where(e => e.RegattaId == id).Select(e => e.ClubId).ToList();
            
            ViewData["ClubId"] = new SelectList(_context.Clubs.Where(e => !clubids.Contains(e.ClubId)).OrderBy(e => e.Name).ToList(), "ClubId", "Name");                    

            ViewBag.regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == id);

            return View(clubvm);
        }

        [HttpPost]
        public IActionResult Subscribe(ClubVM clubvm)
        {
            var club = _context.Clubs.FirstOrDefault(e => e.ClubId == clubvm.ClubId);

            club.EMail = clubvm.EMail;

            Guid g;

            g = Guid.NewGuid();

            var regatta = _context.Regattas.Include(e => e.RegattaClubs).SingleOrDefault(m => m.RegattaId == clubvm.RegattaId);      

            if(!_context.RegattaClubs.Any(e => e.RegattaId == regatta.RegattaId && e.ClubId == club.ClubId))
            {
                _context.RegattaClubs.Add(new RegattaClub { RegattaId = regatta.RegattaId, ClubId = club.ClubId, Guid = g.ToString() });
                _context.SaveChanges();

                return RedirectToAction("Thankyou");
            }            

            return RedirectToAction("Subscribe");
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
