using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class StartingFeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StartingFeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StartingFee
        public async Task<IActionResult> Index()
        {
            ViewBag.oldclasses = await _context.Oldclasses.ToListAsync();
            var applicationDbContext = _context.StartingFees.Include(s => s.Boatclasses);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StartingFee/Create
        public IActionResult Create()
        {
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "Name");
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name");
            return View();
        }

        // POST: StartingFee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Amount, int BoatclassId, int FromOldclassId, int ToOldclassId)
        {
            StartingFee startingFee = new StartingFee();
            startingFee.Amount = decimal.Parse(Amount, CultureInfo.InvariantCulture);
            startingFee.BoatclassId = BoatclassId;
            startingFee.FromOldclassId = FromOldclassId;
            startingFee.ToOldclassId = ToOldclassId;

            try
            {
                _context.Add(startingFee);
                await _context.SaveChangesAsync();
            }
            catch
            {
                ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "Name");
                ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name");
                return View();
            }          
                        
            return RedirectToAction(nameof(Index));
        }

        // GET: StartingFee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var startingFee = await _context.StartingFees.SingleOrDefaultAsync(m => m.StartingFeeId == id);
            if (startingFee == null)
            {
                return NotFound();
            }

            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "Name");
            ViewData["OldclassId"] = new SelectList(_context.Oldclasses, "OldclassId", "Name");

            return View(startingFee);
        }

        // POST: StartingFee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Amount, int BoatclassId, int FromOldclassId, int ToOldclassId)
        {
            StartingFee startingFee = _context.StartingFees.FirstOrDefault(x => x.StartingFeeId == id);

            if (!await _context.StartingFees.AnyAsync(x => x.StartingFeeId == id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    startingFee.Amount = decimal.Parse(Amount, CultureInfo.InvariantCulture);
                    startingFee.BoatclassId = BoatclassId;
                    startingFee.FromOldclassId = FromOldclassId;
                    startingFee.ToOldclassId = ToOldclassId;

                    _context.Update(startingFee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoatclassId"] = new SelectList(_context.Boatclasses, "BoatclassId", "BoatclassId", startingFee.BoatclassId);            
            return View(startingFee);
        }

        // GET: StartingFee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var startingFee = await _context.StartingFees
                .Include(s => s.Boatclasses)                
                .SingleOrDefaultAsync(m => m.StartingFeeId == id);
            if (startingFee == null)
            {
                return NotFound();
            }

            return View(startingFee);
        }

        // POST: StartingFee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var startingFee = await _context.StartingFees.SingleOrDefaultAsync(m => m.StartingFeeId == id);
            _context.StartingFees.Remove(startingFee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StartingFeeExists(int id)
        {
            return _context.StartingFees.Any(e => e.StartingFeeId == id);
        }
    }
}
