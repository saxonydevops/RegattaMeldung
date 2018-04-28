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
    public class OldclassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OldclassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Oldclasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Oldclasses.ToListAsync());
        }

        // GET: Oldclasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldclass = await _context.Oldclasses
                .SingleOrDefaultAsync(m => m.OldclassId == id);
            if (oldclass == null)
            {
                return NotFound();
            }

            return View(oldclass);
        }

        // GET: Oldclasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Oldclasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OldclassId,Name,FromAge,ToAge")] Oldclass oldclass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oldclass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(oldclass);
        }

        // GET: Oldclasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldclass = await _context.Oldclasses.SingleOrDefaultAsync(m => m.OldclassId == id);
            if (oldclass == null)
            {
                return NotFound();
            }
            return View(oldclass);
        }

        // POST: Oldclasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OldclassId,Name,FromAge,ToAge")] Oldclass oldclass)
        {
            if (id != oldclass.OldclassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oldclass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OldclassExists(oldclass.OldclassId))
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
            return View(oldclass);
        }

        // GET: Oldclasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oldclass = await _context.Oldclasses
                .SingleOrDefaultAsync(m => m.OldclassId == id);
            if (oldclass == null)
            {
                return NotFound();
            }

            return View(oldclass);
        }

        // POST: Oldclasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oldclass = await _context.Oldclasses.SingleOrDefaultAsync(m => m.OldclassId == id);
            _context.Oldclasses.Remove(oldclass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OldclassExists(int id)
        {
            return _context.Oldclasses.Any(e => e.OldclassId == id);
        }
    }
}
