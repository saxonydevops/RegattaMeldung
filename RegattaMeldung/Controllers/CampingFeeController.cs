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
using System.Globalization;

namespace RegattaMeldung.Controllers
{
    [Authorize]
    public class CampingFeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampingFeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CampingFee
        public async Task<IActionResult> Index()
        {
            return View(await _context.CampingFees.ToListAsync());
        }

        // GET: CampingFee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CampingFee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name, string Amount)
        {
            CampingFee campingFee = new CampingFee();
            campingFee.Amount = decimal.Parse(Amount, CultureInfo.InvariantCulture);
            campingFee.Name = Name;

            if (ModelState.IsValid)
            {
                _context.Add(campingFee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(campingFee);
        }

        // GET: CampingFee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campingFee = await _context.CampingFees.SingleOrDefaultAsync(m => m.CampingFeeId == id);

            if (campingFee == null)
            {
                return NotFound();
            }

            return View(campingFee);
        }

        // POST: CampingFee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CampingFeeId,Name,Amount")] CampingFee campingFee)
        {
            if (id != campingFee.CampingFeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {                    
                    _context.Update(campingFee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CampingFeeExists(campingFee.CampingFeeId))
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
            return View(campingFee);
        }

        // GET: CampingFee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campingFee = await _context.CampingFees
                .SingleOrDefaultAsync(m => m.CampingFeeId == id);
            if (campingFee == null)
            {
                return NotFound();
            }

            return View(campingFee);
        }

        // POST: CampingFee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var campingFee = await _context.CampingFees.SingleOrDefaultAsync(m => m.CampingFeeId == id);
            _context.CampingFees.Remove(campingFee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CampingFeeExists(int id)
        {
            return _context.CampingFees.Any(e => e.CampingFeeId == id);
        }
    }
}
