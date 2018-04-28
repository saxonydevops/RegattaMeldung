using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;

namespace RegattaMeldung.Controllers
{
    [Produces("application/json")]
    [Route("api/RegattaApi")]
    [Authorize]
    public class RegattaApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegattaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/RegattaApi
        [HttpGet]
        public IEnumerable<Regatta> GetRegattas()
        {
            return _context.Regattas;
        }

        // GET: api/RegattaApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegatta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var regatta = await _context.Regattas.SingleOrDefaultAsync(m => m.RegattaId == id);

            if (regatta == null)
            {
                return NotFound();
            }

            return Ok(regatta);
        }

        // PUT: api/RegattaApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegatta([FromRoute] int id, [FromBody] Regatta regatta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != regatta.RegattaId)
            {
                return BadRequest();
            }

            _context.Entry(regatta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegattaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RegattaApi
        [HttpPost]
        public async Task<IActionResult> PostRegatta([FromBody] Regatta regatta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Regattas.Add(regatta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegatta", new { id = regatta.RegattaId }, regatta);
        }

        // DELETE: api/RegattaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegatta([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var regatta = await _context.Regattas.SingleOrDefaultAsync(m => m.RegattaId == id);
            if (regatta == null)
            {
                return NotFound();
            }

            _context.Regattas.Remove(regatta);
            await _context.SaveChangesAsync();

            return Ok(regatta);
        }

        private bool RegattaExists(int id)
        {
            return _context.Regattas.Any(e => e.RegattaId == id);
        }
    }
}