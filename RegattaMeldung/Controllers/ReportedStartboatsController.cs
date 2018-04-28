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
    public class ReportedStartboatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportedStartboatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ReportedStartboats
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ReportedStartboats.Include(r => r.Club).Include(r => r.ReportedRace).Include(r => r.Regatta);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ReportedStartboats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportedStartboat = await _context.ReportedStartboats
                .Include(r => r.Club)
                .Include(r => r.ReportedRace)
                .Include(r => r.Regatta)
                .SingleOrDefaultAsync(m => m.ReportedStartboatId == id);
            if (reportedStartboat == null)
            {
                return NotFound();
            }

            return View(reportedStartboat);
        }

        // GET: ReportedStartboats/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubId");
            ViewData["CompetitionId"] = new SelectList(_context.Competitions, "CompetitionId", "CompetitionId");
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "RegattaId");
            return View();
        }

        // POST: ReportedStartboats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportedStartboatId,Gender,ClubId,RegattaId,CompetitionId")] ReportedStartboat reportedStartboat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reportedStartboat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubId", reportedStartboat.ClubId);
            ViewData["RegattaCompetitionId"] = new SelectList(_context.ReportedRaces, "ReportedRaceId", "ReportedRaceId", reportedStartboat.ReportedRaceId);
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "RegattaId", reportedStartboat.RegattaId);
            return View(reportedStartboat);
        }

        // GET: ReportedStartboats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportedStartboat = await _context.ReportedStartboats.SingleOrDefaultAsync(m => m.ReportedStartboatId == id);
            if (reportedStartboat == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubId", reportedStartboat.ClubId);
            ViewData["RegattaCompetitionId"] = new SelectList(_context.ReportedRaces, "ReportedRaceId", "ReportedRaceId", reportedStartboat.ReportedRaceId);
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "RegattaId", reportedStartboat.RegattaId);
            return View(reportedStartboat);
        }

        // POST: ReportedStartboats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportedStartboatId,Gender,ClubId,RegattaId,CompetitionId")] ReportedStartboat reportedStartboat)
        {
            if (id != reportedStartboat.ReportedStartboatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportedStartboat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportedStartboatExists(reportedStartboat.ReportedStartboatId))
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubId", reportedStartboat.ClubId);
            ViewData["RegattaCompetitionId"] = new SelectList(_context.ReportedRaces, "ReportedRaceId", "ReportedRaceId", reportedStartboat.ReportedRaceId);
            ViewData["RegattaId"] = new SelectList(_context.Regattas, "RegattaId", "RegattaId", reportedStartboat.RegattaId);
            return View(reportedStartboat);
        }

        // GET: ReportedStartboats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportedStartboat = await _context.ReportedStartboats
                .Include(r => r.Club)
                .Include(r => r.ReportedRace)
                .Include(r => r.Regatta)
                .SingleOrDefaultAsync(m => m.ReportedStartboatId == id);
            if (reportedStartboat == null)
            {
                return NotFound();
            }

            return View(reportedStartboat);
        }

        // POST: ReportedStartboats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportedStartboat = await _context.ReportedStartboats.SingleOrDefaultAsync(m => m.ReportedStartboatId == id);
            _context.ReportedStartboats.Remove(reportedStartboat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportedStartboatExists(int id)
        {
            return _context.ReportedStartboats.Any(e => e.ReportedStartboatId == id);
        }
    }
}
