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
    public class CompetitionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Competition
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Competitions.Include(c => c.Boatclasses).Include(c => c.Raceclasses);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Competition/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.Boatclasses)
                .Include(c => c.Raceclasses)
                .SingleOrDefaultAsync(m => m.CompetitionId == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // GET: Competition/Create
        public IActionResult Create()
        {
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "BoatclassId");
            ViewData["RaceclassId"] = new SelectList(_context.Raceclasses, "RaceclassId", "RaceclassId");
            return View();
        }

        // POST: Competition/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompetitionId,BoatclassId,RaceclassId")] Competition competition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(competition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "BoatclassId", competition.BoatclassId);
            ViewData["RaceclassId"] = new SelectList(_context.Raceclasses, "RaceclassId", "RaceclassId", competition.RaceclassId);
            return View(competition);
        }

        [HttpGet]
        public ActionResult CreateAll(int id)
        {
            var regatta = _context.Regattas.Include(e => e.RegattaOldclasses).Include(e => e.RegattaCompetitions).FirstOrDefault(e => e.RegattaId == id);
            var comp = _context.Competitions;
            var bclist = _context.Boatclasses.ToList();
            var rclist = _context.Raceclasses.ToList();

            foreach (var bc in bclist)
            {
                foreach (var rc in rclist)
                {
                    if (comp.Where(e => e.BoatclassId == bc.BoatclassId && e.RaceclassId == rc.RaceclassId).Count() == 0)
                    {
                        _context.Competitions.Add(new Competition { BoatclassId = bc.BoatclassId, RaceclassId = rc.RaceclassId });
                    }
                }
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Competition/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions.SingleOrDefaultAsync(m => m.CompetitionId == id);
            if (competition == null)
            {
                return NotFound();
            }
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "BoatclassId", competition.BoatclassId);
            ViewData["RaceclassId"] = new SelectList(_context.Raceclasses, "RaceclassId", "RaceclassId", competition.RaceclassId);
            return View(competition);
        }

        // POST: Competition/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CompetitionId,BoatclassId,RaceclassId")] Competition competition)
        {
            if (id != competition.CompetitionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.CompetitionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "BoatclassId", competition.BoatclassId);
            ViewData["RaceclassId"] = new SelectList(_context.Raceclasses, "RaceclassId", "RaceclassId", competition.RaceclassId);
            return View(competition);
        }

        // GET: Competition/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.Boatclasses)
                .Include(c => c.Raceclasses)
                .SingleOrDefaultAsync(m => m.CompetitionId == id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        // POST: Competition/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competition = await _context.Competitions.SingleOrDefaultAsync(m => m.CompetitionId == id);
            _context.Competitions.Remove(competition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionExists(int id)
        {
            return _context.Competitions.Any(e => e.CompetitionId == id);
        }
    }
}
