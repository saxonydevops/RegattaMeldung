using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;

namespace RegattaMeldung.Controllers
{
    [Authorize]
    public class ReportedRacesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportedRacesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReportedRaces
        public IActionResult Index(int rid)
        {
            var firstregatta = _context.Regattas.First();
            IEnumerable<ReportedRace> applicationDbContext = _context.ReportedRaces.
                Include(r => r.Competition.Boatclasses).Include(r => r.Competition.Raceclasses).
                Include(r => r.Oldclass).
                Where(e => e.RegattaId == firstregatta.RegattaId).
                OrderBy(c => c.Competition.Raceclasses.Length).
                ThenBy(b => b.Competition.Boatclasses.Name).
                ThenBy(g => g.Gender).
                ThenBy(a => a.Oldclass.FromAge);

            if (rid > 0)
            {
                applicationDbContext = _context.ReportedRaces.
                    Include(r => r.Competition.Boatclasses).
                    Include(r => r.Competition.Raceclasses).
                    Include(r => r.Oldclass).
                    Where(e => e.RegattaId == rid).
                    OrderBy(c => c.Competition.Raceclasses.Length).
                    ThenBy(b => b.Competition.Boatclasses.Name).
                    ThenBy(g => g.Gender).ThenBy(a => a.Oldclass.FromAge);
            }
            else
            {
                rid = firstregatta.RegattaId;
            }
            IEnumerable<ReportedStartboat> reportedstartboats = _context.ReportedStartboats.Where(e => e.RegattaId == rid);
            ViewBag.reportedstartboats = reportedstartboats;
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "Name", rid);
            ViewBag.rid = rid;            

            return View(applicationDbContext);
        }

        // GET: ReportedRaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportedRace = await _context.ReportedRaces
                .Include(r => r.Competition.Boatclasses)
                .Include(r => r.Competition.Raceclasses)
                .Include(r => r.Oldclass)
                .SingleOrDefaultAsync(m => m.ReportedRaceId == id);
            if (reportedRace == null)
            {
                return NotFound();
            }

            return View(reportedRace);
        }

        // GET: ReportedRaces/Create
        public IActionResult Create()
        {
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "Name");
            ViewData["CompetitionId"] = new SelectList(_context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses).OrderBy(e => e.Boatclasses.Name), "CompetitionId", "Name");
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses.OrderBy(e => e.FromAge), "OldclassId", "Name");
            return View();
        }

        // POST: ReportedRaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportedRaceId,OldclassId,CompetitionId,Gender,RaceCode,RegattaId,Comment")] ReportedRace reportedRace)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportedRace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "Name",reportedRace.RegattaId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "CompetitionId", "Name", reportedRace.CompetitionId);
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name", reportedRace.OldclassId);
            return View(reportedRace);
        }

        // GET: ReportedRaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportedRace = await _context.ReportedRaces.Include(e => e.Competition.Boatclasses).Include(e => e.Competition.Raceclasses).Include(e => e.Oldclass).SingleOrDefaultAsync(m => m.ReportedRaceId == id);
            if (reportedRace == null)
            {
                return NotFound();
            }
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "Name", reportedRace.RegattaId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions.Include(e => e.Raceclasses).Include(e => e.Boatclasses), "CompetitionId", "Name", reportedRace.CompetitionId);
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name", reportedRace.OldclassId);
            return View(reportedRace);
        }

        // POST: ReportedRaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportedRaceId,OldclassId,CompetitionId,Gender,RaceCode,RegattaId,Comment")] ReportedRace reportedRace)
        {
            if (id != reportedRace.ReportedRaceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportedRace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportedRaceExists(reportedRace.ReportedRaceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new { rid = reportedRace.RegattaId });
            }
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "Name", reportedRace.RegattaId);
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "CompetitionId", "Name", reportedRace.CompetitionId);
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name", reportedRace.OldclassId);
            return View(reportedRace);
        }

        // GET: ReportedRaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportedRace = await _context.ReportedRaces
                .Include(r => r.Competition.Boatclasses)
                .Include(r => r.Competition.Raceclasses)
                .Include(r => r.Oldclass)
                .SingleOrDefaultAsync(m => m.ReportedRaceId == id);
            if (reportedRace == null)
            {
                return NotFound();
            }

            return View(reportedRace);
        }

        // POST: ReportedRaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportedRace = await _context.ReportedRaces.SingleOrDefaultAsync(m => m.ReportedRaceId == id);
            var rid = reportedRace.RegattaId;
            _context.ReportedRaces.Remove(reportedRace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index),new { rid = rid });
        }

        private bool ReportedRaceExists(int id)
        {
            return _context.ReportedRaces.Any(e => e.ReportedRaceId == id);
        }

        private string getGenderString(string gender, int OldclassId)
        {
            var oc = _context.Oldclasses.FirstOrDefault(e => e.OldclassId == OldclassId);

            if(gender == "M")
            {                
                if(oc.FromAge >= 0 && oc.ToAge <= 14)
                {
                    return oc.Name;
                }
                else if (oc.FromAge >=15 && oc.ToAge <= 16)
                {
                    return string.Format("männliche {0}", oc.Name);
                }
                else if(oc.FromAge >= 17 && oc.ToAge <= 99)
                {
                    return string.Format("Herren {0}", oc.Name);
                }
                else
                {
                    return string.Format("männliche {0}", oc.Name);
                }
            }
            else if(gender == "W")
            {
                if(oc.FromAge == 0 && oc.ToAge == 6)
                {
                    return "Schülerinnen D";
                }
                else if(oc.FromAge == 7 && oc.ToAge == 9)
                {
                    return "Schülerinnen C";
                }
                else if (oc.FromAge == 10 && oc.ToAge == 12)
                {
                    return "Schülerinnen B";
                }
                else if (oc.FromAge == 13 && oc.ToAge == 14)
                {
                    return "Schülerinnen A";
                }
                else if (oc.FromAge == 7 && oc.ToAge == 7)
                {
                    return "Schülerinnen C7";
                }
                else if (oc.FromAge == 8 && oc.ToAge == 8)
                {
                    return "Schülerinnen C8";
                }
                else if (oc.FromAge == 9 && oc.ToAge == 9)
                {
                    return "Schülerinnen C9";
                }
                else if (oc.FromAge == 10 && oc.ToAge == 10)
                {
                    return "Schülerinnen B10";
                }
                else if (oc.FromAge == 11 && oc.ToAge == 11)
                {
                    return "Schülerinnen B11";
                }
                else if (oc.FromAge == 12 && oc.ToAge == 12)
                {
                    return "Schülerinnen B12";
                }
                else if (oc.FromAge == 13 && oc.ToAge == 13)
                {
                    return "Schülerinnen A13";
                }
                else if (oc.FromAge == 14 && oc.ToAge == 14)
                {
                    return "Schülerinnen A14";
                }
                else if(oc.FromAge >= 15 && oc.ToAge <= 16)
                {
                    return string.Format("weibliche {0}", oc.Name);
                }
                else if(oc.FromAge >= 17 && oc.ToAge <= 99)
                {
                    return string.Format("Damen {0}", oc.Name);
                }
                else
                {
                    return string.Format("weibliche {0}", oc.Name);
                }
            }
            else
            {
                return string.Format("mixed {0}", oc.Name);
            }
        }
    }
}
