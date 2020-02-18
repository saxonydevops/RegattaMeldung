using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;

namespace RegattaMeldung.Controllers
{
    public class StammdatenController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StammdatenController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Stammdaten
        public ActionResult Index(string guid, string sortby, int RentedFromClubId)
        {           
            if (guid == null)
            {
                return RedirectToAction("Index","Home");
            }

            var rc = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid);
            var club = _context.Clubs.Include(e => e.Members).FirstOrDefault(e => e.ClubId == rc.ClubId);
            var regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == rc.RegattaId);
            IEnumerable<Club> allClubs = _context.Clubs;            

            var member = _context.Members.Where(e => e.ClubId == rc.ClubId).OrderByDescending(e => e.Birthyear).ThenBy(e => e.Gender).ThenBy(e => e.LastName);

            if (sortby == "Nachname")
            {
                member = member.OrderBy(e => e.LastName).ThenByDescending(e => e.Birthyear).ThenBy(e => e.Gender);
            }          
            else if(sortby == "NachnameDesc")
            {
                member = member.OrderByDescending(e => e.LastName).ThenByDescending(e => e.Birthyear).ThenBy(e => e.Gender);
            }
            else if(sortby == "Geschlecht")
            {
                member = member.OrderBy(e => e.Gender).ThenByDescending(e => e.Birthyear).ThenBy(e => e.LastName);
            }
            else if(sortby == "Alter")
            {
                member = member.OrderByDescending(e => e.Birthyear).ThenBy(e => e.Gender).ThenBy(e => e.LastName);
            }
            else if(sortby == "AlterAsc")
            {
                member = member.OrderBy(e => e.Birthyear).ThenBy(e => e.Gender).ThenBy(e => e.LastName);
            }

            ViewBag.AllClubs = allClubs;
            ViewBag.sortby = sortby;
            ViewBag.Guid = guid;
            ViewBag.Members = member;
            ViewBag.MemberYear = DateTime.Now.Year - 6;            

            ViewData["RentedToClubId"] = new SelectList(_context.Clubs.Where(e => e.ClubId != rc.ClubId).OrderBy(e => e.Name), "ClubId", "ShortName");           

            return View(club);
        }

        public IActionResult UnrentMember(int id, string guid)
        {
            var member = _context.Members.FirstOrDefault(e => e.MemberId == id);

            if (member != null)
            {
                member.isRented = false;
                member.RentedToClubId = 0;                
                _context.Members.Update(member);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Stammdaten", new { guid = guid });
        }

        public IActionResult ChooseClub(int id, int RentedFromClubId, string guid)
        {
            return RedirectToAction("Index","Stammdaten",new {guid = guid, RentedFromClubId = RentedFromClubId, choosed = true});
        }

        public IActionResult AddMember(int id, string lastname, string firstname, string gender, int birthyear, string guid, bool isRented, int RentedToClubId, int RentYear)
        {
            if(isRented == true)
            {
                _context.Clubs.Include(e => e.Members).FirstOrDefault(e => e.ClubId == id).Members.Add(new Member { LastName = lastname, FirstName = firstname, Gender = gender, Birthyear = birthyear, isRented = isRented, RentedToClubId = RentedToClubId });
            }
            else
            {
                _context.Clubs.Include(e => e.Members).FirstOrDefault(e => e.ClubId == id).Members.Add(new Member { LastName = lastname, FirstName = firstname, Gender = gender, Birthyear = birthyear });
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Stammdaten", new { guid = guid });
        }

        public IActionResult DeleteMember(int id, string guid)
        {
            var member = _context.Members.SingleOrDefault(m => m.MemberId == id);
            _context.Members.Remove(member);
            _context.SaveChanges();

            return RedirectToAction("Index", "Stammdaten", new { guid = guid });
        }

        [HttpGet]
        public IActionResult EditMember(int id, string guid)
        {
            var member = _context.Members.SingleOrDefault(m => m.MemberId == id);
            var rentedtoclub = _context.Clubs.FirstOrDefault(e => e.ClubId == member.RentedToClubId);
            var rc = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid);

            ViewBag.Guid = guid;
            if(rentedtoclub != null)
            {
                ViewData["RentedToClubId"] = new SelectList(_context.Clubs.Where(e => e.ClubId != rc.ClubId).OrderBy(e => e.Name), "ClubId", "ShortName", rentedtoclub.ClubId);
            }
            else
            {
                ViewData["RentedToClubId"] = new SelectList(_context.Clubs.Where(e => e.ClubId != rc.ClubId).OrderBy(e => e.Name), "ClubId", "ShortName");
            }
            
            ViewBag.ThisYear = DateTime.Now.Year;

            return View(member);
        }

        [HttpPost]
        public IActionResult EditMember(int id, string guid, [Bind("MemberId,LastName,FirstName,Birthyear,Gender,ClubId,isRented,RentedToClubId,RentYear")] Member member)
        {
            if(id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction("Index","Stammdaten",new { guid = guid });
            }
            return View(member);
        }
    }
}