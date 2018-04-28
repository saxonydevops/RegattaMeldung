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
    public class BoatclassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BoatclassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Boatclasses
        public async Task<IActionResult> Index()
        {
            return View(await _context.Boatclasses.ToListAsync());
        }

        // GET: Boatclasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatclass = await _context.Boatclasses
                .SingleOrDefaultAsync(m => m.BoatclassId == id);
            if (boatclass == null)
            {
                return NotFound();
            }

            return View(boatclass);
        }

        // GET: Boatclasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Boatclasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BoatclassId,Name,Seats")] Boatclass boatclass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(boatclass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(boatclass);
        }

        // GET: Boatclasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatclass = await _context.Boatclasses.SingleOrDefaultAsync(m => m.BoatclassId == id);
            if (boatclass == null)
            {
                return NotFound();
            }
            return View(boatclass);
        }

        // POST: Boatclasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BoatclassId,Name,Seats")] Boatclass boatclass)
        {
            if (id != boatclass.BoatclassId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(boatclass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoatclassExists(boatclass.BoatclassId))
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
            return View(boatclass);
        }

        // GET: Boatclasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var boatclass = await _context.Boatclasses
                .SingleOrDefaultAsync(m => m.BoatclassId == id);
            if (boatclass == null)
            {
                return NotFound();
            }

            return View(boatclass);
        }

        // POST: Boatclasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var boatclass = await _context.Boatclasses.SingleOrDefaultAsync(m => m.BoatclassId == id);
            _context.Boatclasses.Remove(boatclass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BoatclassExists(int id)
        {
            return _context.Boatclasses.Any(e => e.BoatclassId == id);
        }
    }
}
