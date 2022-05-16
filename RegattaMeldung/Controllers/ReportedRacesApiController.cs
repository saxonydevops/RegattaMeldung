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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportedRacesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportedRacesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ReportedRacesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportedRace>>> GetReportedRaces()
        {
            return await _context.ReportedRaces.ToListAsync();
        }

        // GET: api/ReportedRacesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReportedRace>> GetReportedRace(int id)
        {
            var reportedRace = await _context.ReportedRaces.FindAsync(id);

            if (reportedRace == null)
            {
                return NotFound();
            }

            return reportedRace;
        }

        // PUT: api/ReportedRacesApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReportedRace(int id, ReportedRace reportedRace)
        {
            if (id != reportedRace.ReportedRaceId)
            {
                return BadRequest();
            }

            _context.Entry(reportedRace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportedRaceExists(id))
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

        // POST: api/ReportedRacesApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ReportedRace>> PostReportedRace(ReportedRace reportedRace)
        {
            _context.ReportedRaces.Add(reportedRace);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReportedRace", new { id = reportedRace.ReportedRaceId }, reportedRace);
        }

        // DELETE: api/ReportedRacesApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ReportedRace>> DeleteReportedRace(int id)
        {
            var reportedRace = await _context.ReportedRaces.FindAsync(id);
            if (reportedRace == null)
            {
                return NotFound();
            }

            _context.ReportedRaces.Remove(reportedRace);
            await _context.SaveChangesAsync();

            return reportedRace;
        }

        private bool ReportedRaceExists(int id)
        {
            return _context.ReportedRaces.Any(e => e.ReportedRaceId == id);
        }
    }
}
