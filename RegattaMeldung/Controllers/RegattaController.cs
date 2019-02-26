using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;
using RegattaMeldung.ViewModels;

namespace RegattaMeldung.Controllers
{
    [Authorize]
    public class RegattaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegattaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Regatta
        public async Task<IActionResult> Index(int? id)
        {
            var applicationDbContext = _context.Regattas.Include(r => r.Club).Include(r => r.Waters);

            if(id == null)
            {
                IEnumerable<RegattaClub> invitedclubs = _context.RegattaClubs.Include(e => e.Club).Include(e => e.Regatta).OrderBy(e => e.RegattaId).ThenBy(e => e.Club.Name);
                ViewBag.Invited = invitedclubs;
            }            
            else
            {
                IEnumerable<RegattaClub> invitedclubs = _context.RegattaClubs.Include(e => e.Club).Include(e => e.Regatta).Where(e => e.RegattaId == id).OrderBy(e => e.RegattaId).ThenBy(e => e.Club.Name);
                ViewBag.Invited = invitedclubs;
            }

            var reportedstartboats = _context.ReportedStartboats.ToList();
            ViewBag.reportedstartboats = reportedstartboats;

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Regatta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regatta = await _context.Regattas
                .Include(r => r.Club)
                .Include(r => r.Waters)
                .SingleOrDefaultAsync(m => m.RegattaId == id);
            if (regatta == null)
            {
                return NotFound();
            }

            return View(regatta);
        }

        // GET: Regatta/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs.OrderBy(e => e.Name), "ClubId", "Name");
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name");
            ViewData["OldclassIds"] = new MultiSelectList(_context.Oldclasses.OrderBy(e => e.FromAge), "OldclassId", "Name");
            ViewData["CompetitionIds"] = new MultiSelectList(_context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses).OrderBy(e => e.Boatclasses.Name).ThenBy(e => e.Raceclasses.Length), "CompetitionId", "Name");
            ViewData["StartingFeeIds"] = new MultiSelectList(_context.StartingFees, "StartingFeeId", "Name");
            ViewData["CampingFeeIds"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "LongName");
            return View();
        }

        // POST: Regatta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegattaName,RegattaVon,RegattaBis,Waterdepth,Startslots,ReportText,ReportSchedule,ReportOpening," +
            "ReportAddress,ReportTel,ReportFax,ReportMail,Judge,Awards,Security,ScheduleText,SubscriberFee,Accomodation,Comment,Catering,ClubId,WaterId")] RegattaVM regattaVM,
            IEnumerable<int> OldclassIds, IEnumerable<int> CompetitionIds, IEnumerable<int> StartingFeeIds, IEnumerable<int> CampingFeeIds)
        {
            Regatta regatta = new Regatta();

            regatta.Name = regattaVM.RegattaName;
            regatta.FromDate = regattaVM.RegattaVon;
            regatta.ToDate = regattaVM.RegattaBis;
            regatta.Waterdepth = regattaVM.Waterdepth;
            regatta.Startslots = regattaVM.Startslots;
            regatta.ReportText = regattaVM.ReportText;
            regatta.ReportSchedule = regattaVM.ReportSchedule;
            regatta.ReportOpening = regattaVM.ReportOpening;
            regatta.ReportAddress = regattaVM.ReportAddress;
            regatta.ReportTel = regattaVM.ReportTel;
            regatta.ReportFax = regattaVM.ReportFax;
            regatta.ReportMail = regattaVM.ReportMail;
            regatta.Judge = regattaVM.Judge;
            regatta.Awards = regattaVM.Awards;
            regatta.Security = regattaVM.Security;
            regatta.ScheduleText = regattaVM.ScheduleText;
            regatta.SubscriberFee = regattaVM.SubscriberFee;
            regatta.Accomodation = regattaVM.Accomodation;
            regatta.Comment = regattaVM.Comment;
            regatta.Catering = regattaVM.Catering;
            regatta.ClubId = regattaVM.ClubId;
            regatta.WaterId = regattaVM.WaterId;
            regatta.Organizer = regattaVM.Organizer;
            regatta.StartersLastYear = regattaVM.StartersLastYear;
            regatta.Category = regattaVM.Category;

            if (ModelState.IsValid)
            {
                _context.Add(regatta);
                await _context.SaveChangesAsync();

                IEnumerable<RegattaOldclass> roc = _context.RegattaOldclasses.Where(e => e.RegattaId == regattaVM.RegattaId);
                IEnumerable<RegattaCampingFee> rcf = _context.RegattaCampingFees.Where(e => e.RegattaId == regattaVM.RegattaId);
                IEnumerable<RegattaCompetition> rc = _context.RegattaCompetitions.Where(e => e.RegattaId == regattaVM.RegattaId);
                IEnumerable<RegattaStartingFee> rsf = _context.RegattaStartingFees.Where(e => e.RegattaId == regattaVM.RegattaId);

                var reg = _context.Regattas.Last();

                foreach (var oc in OldclassIds)
                {
                    _context.Regattas.Include(e => e.RegattaOldclasses).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaOldclasses.Add(new RegattaOldclass { RegattaId = regattaVM.RegattaId, OldclassId = oc });
                }

                foreach (var cf in CampingFeeIds)
                {
                    _context.Regattas.Include(e => e.RegattaCampingFees).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaCampingFees.Add(new RegattaCampingFee { RegattaId = regattaVM.RegattaId, CampingFeeId = cf });
                }

                foreach (var rcid in CompetitionIds)
                {
                    _context.Regattas.Include(e => e.RegattaCompetitions).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaCompetitions.Add(new RegattaCompetition { RegattaId = regattaVM.RegattaId, CompetitionId = rcid });
                }

                foreach (var rsfid in StartingFeeIds)
                {

                    _context.Regattas.Include(e => e.RegattaStartingFees).FirstOrDefault(m => m.RegattaId == reg.RegattaId).RegattaStartingFees.Add(new RegattaStartingFee { RegattaId = regattaVM.RegattaId, StartingFeeId = rsfid });
                }

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name", regatta.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name", regatta.WaterId);
            ViewData["OldclassIds"] = new MultiSelectList(_context.Oldclasses, "OldclassId", "Name");
            ViewData["CompetitionIds"] = new MultiSelectList(_context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses), "CompetitionId", "Name");
            ViewData["StartingFeeIds"] = new MultiSelectList(_context.StartingFees, "StartingFeeId", "Name");
            ViewData["CampingFeeIds"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "LongName");
            return View(regattaVM);
        }

        // GET: Regatta/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            RegattaVM rvm = populateRegattaVM(id);

            if (rvm == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "Name", rvm.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "Name", rvm.WaterId);
            ViewData["OldclassIds"] = new MultiSelectList(rvm.Oldclasses, "OldclassId", "Name", rvm.RegattaOldclasses.Select(e => e.OldclassId).ToList());
            ViewData["CompetitionIds"] = new MultiSelectList(rvm.Competitions, "CompetitionId", "Name", rvm.RegattaCompetitions.Select(e => e.CompetitionId).ToList());
            ViewData["StartingFeeIds"] = new MultiSelectList(rvm.StartingFees, "StartingFeeId", "Name", rvm.RegattaStartingFees.Select(e => e.StartingFeeId).ToList());
            ViewData["CampingFeeIds"] = new MultiSelectList(rvm.CampingFees, "CampingFeeId", "LongName", rvm.RegattaCampingFees.Select(e => e.CampingFeeId).ToList());
            return View(rvm);
        }

        // POST: Regatta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegattaId,RegattaName,RegattaVon,RegattaBis,Waterdepth,Startslots,ReportText,ReportSchedule,ReportOpening," +
            "ReportAddress,ReportTel,ReportFax,ReportMail,Judge,Awards,Security,ScheduleText,SubscriberFee,Accomodation,Comment,Catering,ClubId,WaterId,Organizer,Category,StartersLastYear")] RegattaVM regattaVM,
            IEnumerable<int> OldclassIds, IEnumerable<int> CompetitionIds, IEnumerable<int> StartingFeeIds, IEnumerable<int> CampingFeeIds)
        {
            if (id != regattaVM.RegattaId)
            {
                return NotFound();
            }

            Regatta regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == regattaVM.RegattaId);

            regatta.Name = regattaVM.RegattaName;
            regatta.FromDate = regattaVM.RegattaVon;
            regatta.ToDate = regattaVM.RegattaBis;
            regatta.Waterdepth = regattaVM.Waterdepth;
            regatta.Startslots = regattaVM.Startslots;
            regatta.ReportText = regattaVM.ReportText;
            regatta.ReportSchedule = regattaVM.ReportSchedule;
            regatta.ReportOpening = regattaVM.ReportOpening;
            regatta.ReportAddress = regattaVM.ReportAddress;
            regatta.ReportTel = regattaVM.ReportTel;
            regatta.ReportFax = regattaVM.ReportFax;
            regatta.ReportMail = regattaVM.ReportMail;
            regatta.Judge = regattaVM.Judge;
            regatta.Awards = regattaVM.Awards;
            regatta.Security = regattaVM.Security;
            regatta.ScheduleText = regattaVM.ScheduleText;
            regatta.SubscriberFee = regattaVM.SubscriberFee;
            regatta.Accomodation = regattaVM.Accomodation;
            regatta.Catering = regattaVM.Catering;
            regatta.Comment = regattaVM.Comment;
            regatta.ClubId = regattaVM.ClubId;
            regatta.WaterId = regattaVM.WaterId;
            regatta.Organizer = regattaVM.Organizer;
            regatta.StartersLastYear = regattaVM.StartersLastYear;
            regatta.Category = regattaVM.Category;

            IEnumerable<RegattaOldclass> roc = _context.RegattaOldclasses.Where(e => e.RegattaId == regattaVM.RegattaId);
            IEnumerable<RegattaCampingFee> rcf = _context.RegattaCampingFees.Where(e => e.RegattaId == regattaVM.RegattaId);
            IEnumerable<RegattaCompetition> rc = _context.RegattaCompetitions.Where(e => e.RegattaId == regattaVM.RegattaId);
            IEnumerable<RegattaStartingFee> rsf = _context.RegattaStartingFees.Where(e => e.RegattaId == regattaVM.RegattaId);

            foreach (var oc in OldclassIds)
            {
                if (roc.Where(e => e.OldclassId == oc && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaOldclasses).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaOldclasses.Add(new RegattaOldclass { RegattaId = regattaVM.RegattaId, OldclassId = oc });
                }
            }

            foreach (var cf in CampingFeeIds)
            {
                if (rcf.Where(e => e.CampingFeeId == cf && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaCampingFees).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaCampingFees.Add(new RegattaCampingFee { RegattaId = regattaVM.RegattaId, CampingFeeId = cf });
                }
            }

            foreach (var rcid in CompetitionIds)
            {
                if (rc.Where(e => e.CompetitionId == rcid && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaCompetitions).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaCompetitions.Add(new RegattaCompetition { RegattaId = regattaVM.RegattaId, CompetitionId = rcid });
                }
            }

            foreach (var rsfid in StartingFeeIds)
            {
                if (rsf.Where(e => e.StartingFeeId == rsfid && e.RegattaId == regattaVM.RegattaId).Count() == 0)
                {
                    _context.Regattas.Include(e => e.RegattaStartingFees).FirstOrDefault(m => m.RegattaId == regattaVM.RegattaId).RegattaStartingFees.Add(new RegattaStartingFee { RegattaId = regattaVM.RegattaId, StartingFeeId = rsfid });
                }
            }

            _context.SaveChanges();

            foreach (var oldoc in roc)
            {
                if (!OldclassIds.Contains(oldoc.OldclassId))
                {
                    regatta.RegattaOldclasses.Remove(oldoc);
                }
            }

            foreach (var oldcf in rcf)
            {
                if (!CampingFeeIds.Contains(oldcf.CampingFeeId))
                {
                    regatta.RegattaCampingFees.Remove(oldcf);
                }
            }

            foreach (var oldrcid in rc)
            {
                if (!CompetitionIds.Contains(oldrcid.CompetitionId))
                {
                    regatta.RegattaCompetitions.Remove(oldrcid);
                }
            }

            foreach (var oldrsfid in rsf)
            {
                if (!StartingFeeIds.Contains(oldrsfid.StartingFeeId))
                {
                    regatta.RegattaStartingFees.Remove(oldrsfid);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(regatta);
                    await _context.SaveChangesAsync();                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegattaExists(regatta.RegattaId))
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "ClubId", "ClubId", regatta.ClubId);
            ViewData["WaterId"] = new SelectList(_context.Waters, "WaterId", "WaterId", regatta.WaterId);
            ViewData["RegattaOldclasses"] = new MultiSelectList(_context.Oldclasses, "OldclassId", "Name", regattaVM.RegattaOldclasses.Select(e => e.OldclassId).ToList());
            ViewData["RegattaCompetitions"] = new MultiSelectList(_context.Competitions, "CompetitionId", "Name", regattaVM.RegattaCompetitions.Select(e => e.CompetitionId).ToList());
            ViewData["RegattaStartingFees"] = new MultiSelectList(_context.StartingFees, "StartingFeeId", "Name", regattaVM.RegattaStartingFees.Select(e => e.StartingFeeId).ToList());
            ViewData["RegattaCampingFees"] = new MultiSelectList(_context.CampingFees, "CampingFeeId", "Name", regattaVM.RegattaCampingFees.Select(e => e.CampingFeeId).ToList());
            return View(regattaVM);
        }

        // GET: Regatta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regatta = await _context.Regattas
                .Include(r => r.Club)
                .Include(r => r.Waters)
                .SingleOrDefaultAsync(m => m.RegattaId == id);
            if (regatta == null)
            {
                return NotFound();
            }

            return View(regatta);
        }

        // POST: Regatta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {          
            var regatta = _context.Regattas.SingleOrDefault(m => m.RegattaId == id);

            var reportedstartboats = _context.ReportedStartboats.Where(e => e.RegattaId == id);

            var reportedstartboatmembers = _context.ReportedStartboatMembers.Where(e => e.ReportedStartboatId == reportedstartboats.FirstOrDefault().ReportedStartboatId);
            var reportedstartboatstandbys = _context.ReportedStartboatStandbys.Where(e => e.ReportedStartboatId == reportedstartboats.FirstOrDefault().ReportedStartboatId);

            foreach (var rsb in reportedstartboats)
            {
                reportedstartboatmembers = _context.ReportedStartboatMembers.Where(e => e.ReportedStartboatId == rsb.ReportedStartboatId);
                reportedstartboatstandbys = _context.ReportedStartboatStandbys.Where(e => e.ReportedStartboatId == rsb.ReportedStartboatId);

                foreach(var rsbm in reportedstartboatmembers)
                {
                    _context.ReportedStartboatMembers.Remove(rsbm);
                }

                foreach(var rsbs in reportedstartboatstandbys)
                {
                    _context.ReportedStartboatStandbys.Remove(rsbs);
                }
                _context.SaveChanges();
            }

            _context.SaveChanges();

            foreach(var rsb in reportedstartboats)
            {
                _context.ReportedStartboats.Remove(rsb);
            }

            _context.SaveChanges();

            var regattacompetitions = _context.RegattaCompetitions.Where(e => e.RegattaId == id);

            foreach(var rc in regattacompetitions)
            {
                _context.RegattaCompetitions.Remove(rc);
            }

            var reportedraces = _context.ReportedRaces.Where(e => e.RegattaId == id);

            foreach(var rr in reportedraces)
            {
                _context.ReportedRaces.Remove(rr);
            }

            var regattaclubs = _context.RegattaClubs.Where(e => e.RegattaId == id);
            
            foreach(var rc in regattaclubs)
            {
                _context.RegattaClubs.Remove(rc);
            }

            var regattacampingfee = _context.RegattaCampingFees.Where(e => e.RegattaId == id);

            foreach(var rcf in regattacampingfee)
            {
                _context.RegattaCampingFees.Remove(rcf);
            }

            var regattastartingfee = _context.RegattaStartingFees.Where(e => e.RegattaId == id);

            foreach(var rsf in regattastartingfee)
            {
                _context.RegattaStartingFees.Remove(rsf);
            }

            _context.Regattas.Remove(regatta);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Invite(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ViewBag.RegattaId = id;
            IEnumerable<RegattaClub> rcenum = _context.RegattaClubs;
            ViewBag.RegattaClubs = rcenum;
            var clubs = await _context.Clubs.Include(e => e.RegattaClubs).ToListAsync();

            return View(clubs);
        }

        [HttpPost, ActionName("Invite")]
        public async Task<IActionResult> InviteClub(int? id, int? ClubId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Guid g;

            g = Guid.NewGuid();

            var regatta = await _context.Regattas.Include(e => e.RegattaClubs).SingleOrDefaultAsync(m => m.RegattaId == id);
            var club = await _context.Clubs.SingleOrDefaultAsync(m => m.ClubId == ClubId);
            _context.RegattaClubs.Add(new RegattaClub { RegattaId = regatta.RegattaId, ClubId = club.ClubId, Guid = g.ToString() });
            await _context.SaveChangesAsync();

            var clubs = await _context.Clubs.Include(e => e.RegattaClubs).ToListAsync();

            return RedirectToAction("Invite", "Regatta", new { id = id });
        }

        public async Task<IActionResult> SendInvite(int ClubId, int RegattaId)
        {
            var club = _context.Clubs.FirstOrDefault(e => e.ClubId == ClubId);
            var regattaclub = _context.RegattaClubs.FirstOrDefault(e => e.ClubId == ClubId && e.RegattaId == RegattaId);

            var smtpClient = new SmtpClient
            {
                Host = "mailgate.tbls.biz",
                Port = 25,                
                Credentials = new NetworkCredential("regattaplanernet@regattaplaner.net","t/MRR0rPFzeoL/Gy9gK3uqr/XAexDN")                
            };

            using (var message = new MailMessage("info@regattaplaner.net", club.EMail)
            {
                Subject = "Dein Meldelink",
                Body = "Hallo " + club.Name + "!\n\nDein Meldelink ist https://meldung.regattaplaner.net/Meldung/?guid=" + regattaclub.Guid + "\n\nViele Grüße,\nDas SKSV Mittweida e.V. Team"
            })
            {
                await smtpClient.SendMailAsync(message);
            }

            regattaclub.Invited = true;

            _context.RegattaClubs.Update(regattaclub);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult DeleteInvite(int ClubId, int RegattaId)
        {
            var regattaclub = _context.RegattaClubs.FirstOrDefault(e => e.ClubId == ClubId && e.RegattaId == RegattaId);

            if(regattaclub != null)
            {
                var rsb = _context.ReportedStartboats.Where(x => x.ClubId == ClubId);
                var rsbm = _context.ReportedStartboatMembers.ToList();
                var rsbs = _context.ReportedStartboatStandbys.ToList();

                foreach(var sb in rsb)
                {
                    rsbm = _context.ReportedStartboatMembers.Where(e => e.ReportedStartboatId == sb.ReportedStartboatId).ToList();

                    foreach(var sbm in rsbm)
                    {
                        _context.ReportedStartboatMembers.Remove(sbm);
                    }

                    rsbs = _context.ReportedStartboatStandbys.Where(e => e.ReportedStartboatId == sb.ReportedStartboatId).ToList();

                    foreach(var sbs in rsbs)
                    {
                        _context.ReportedStartboatStandbys.Remove(sbs);
                    }

                    _context.ReportedStartboats.Remove(sb);
                }

                _context.RegattaClubs.Remove(regattaclub);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult createReportedRaces(int id)
        {
            var regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == id);
            var oldclasses = _context.RegattaOldclasses.Include(e => e.Oldclasses).Where(e => e.RegattaId == id);
            var competitions = _context.RegattaCompetitions.Include(e => e.Competitions).Where(e => e.RegattaId == id);
            string rc = "00000E";

            foreach(var oc in oldclasses)
            {
                foreach(var c in competitions)
                {
                    if(!_context.ReportedRaces.Where(e => e.CompetitionId == c.CompetitionId && e.OldclassId == oc.OldclassId).Any())
                    {
                        rc = getRaceCode(oc.OldclassId, c.CompetitionId, "M");
                        _context.ReportedRaces.Add(new ReportedRace { OldclassId = oc.OldclassId, CompetitionId = c.CompetitionId, Gender = "M", RegattaId = id, RaceCode = rc });
                        rc = getRaceCode(oc.OldclassId, c.CompetitionId, "W");
                        _context.ReportedRaces.Add(new ReportedRace { OldclassId = oc.OldclassId, CompetitionId = c.CompetitionId, Gender = "W", RegattaId = id, RaceCode = rc });                        
                    }                    
                }
            }

            _context.SaveChanges();

            return RedirectToAction("ReportedRaces", "Index");
        }

        private string getRaceCode(int ocid, int competitionid, string gender)
        {
            string raceCode = "";

            string c1, c2, c3, c5, c6 = "0";

            var competition = _context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses).FirstOrDefault(e => e.CompetitionId == competitionid);
            var oldclass = _context.Oldclasses.FirstOrDefault(e => e.OldclassId == ocid);

            string bcname = competition.Boatclasses.Name;
            string ocname = oldclass.Name;
            int racelength = competition.Raceclasses.Length;

            switch(bcname)
            {
                case "K1":
                    c1 = "1";
                    break;
                case "K2":
                    c1 = "2";
                    break;
                case "K4":
                    c1 = "4";
                    break;
                case "C1":
                    c1 = "5";
                    break;
                case "C2":
                    c1 = "6";
                    break;
                case "S4":
                    c1 = "8";
                    break;
                case "S8":
                    c1 = "9";
                    break;
                default:
                    c1 = "0";
                    break;
            }

            switch(gender)
            {
                case "M":
                    c2 = "1";
                    break;
                case "W":
                    c2 = "2";
                    break;
                case "X":
                    c2 = "3";
                    break;
                default:
                    c2 = "0";
                    break;
            }

            switch(ocname)
            {
                case "Schüler C/B10":
                    c3 = "01";
                    break;
                case "Schüler C7":
                    c3 = "16";
                    break;
                case "Schüler C8":
                    c3 = "17";
                    break;
                case "Schüler C9":
                    c3 = "18";
                    break;
                case "Schüler B10":
                    c3 = "02";
                    break;
                case "Schüler B11":
                    c3 = "03";
                    break;
                case "Schüler B12":
                    c3 = "04";
                    break;
                case "Schüler B":
                    c3 = "05";
                    break;
                case "Schüler A13":
                    c3 = "06";
                    break;
                case "Schüler A14":
                    c3 = "07";
                    break;
                case "Schüler A":
                    c3 = "08";
                    break;
                case "Jugend":
                    c3 = "09";
                    break;
                case "Junioren":
                    c3 = "10";
                    break;
                case "Leistungsklasse":
                    c3 = "11";
                    break;
                case "Senioren A":
                    c3 = "12";
                    break;
                case "Senioren B":
                    c3 = "13";
                    break;
                case "Senioren C":
                    c3 = "14";
                    break;
                case "Senioren D":
                    c3 = "15";
                    break;
                default:
                    c3 = "00";
                    break;
            }

            switch(racelength)
            {
                case 250:
                    c5 = "1";
                    break;
                case 100:
                    c5 = "0";
                    break;
                case 200:
                    c5 = "3";
                    break;
                case 2000:
                    c5 = "4";
                    break;
                case 4000:
                    c5 = "5";
                    break;
                default:
                    c5 = "0";
                    break;
            }

            c6 = "E";

            raceCode = c1 + c2 + c3 + c5 + c6;

            return raceCode;
        }

        private RegattaVM populateRegattaVM(int? id)
        {
            RegattaVM rvm = new RegattaVM();
            var regatta = _context.Regattas.Include(e => e.Waters).Include(e => e.Club).FirstOrDefault(e => e.RegattaId == id);
            var roc = _context.RegattaOldclasses.Where(e => e.RegattaId == id);
            var rcf = _context.RegattaCampingFees.Where(e => e.RegattaId == id);
            var comp = _context.RegattaCompetitions.Where(e => e.RegattaId == id);
            var rsf = _context.RegattaStartingFees.Where(e => e.RegattaId == id);

            rvm.RegattaId = regatta.RegattaId;
            rvm.RegattaName = regatta.Name;
            rvm.RegattaVon = regatta.FromDate;
            rvm.RegattaBis = regatta.ToDate;
            rvm.Waterdepth = regatta.Waterdepth;
            rvm.Startslots = regatta.Startslots;
            rvm.ReportText = regatta.ReportText;
            rvm.ReportSchedule = regatta.ReportSchedule;
            rvm.ReportOpening = regatta.ReportOpening;
            rvm.ReportAddress = regatta.ReportAddress;
            rvm.ReportTel = regatta.ReportTel;
            rvm.ReportFax = regatta.ReportFax;
            rvm.ReportMail = regatta.ReportMail;
            rvm.Judge = regatta.Judge;
            rvm.Awards = regatta.Awards;
            rvm.Security = regatta.Security;
            rvm.ScheduleText = regatta.ScheduleText;
            rvm.SubscriberFee = regatta.SubscriberFee;
            rvm.Accomodation = regatta.Accomodation;
            rvm.Comment = regatta.Comment;
            rvm.Catering = regatta.Catering;
            rvm.WaterId = regatta.WaterId;
            rvm.ClubId = regatta.ClubId;
            rvm.Category = regatta.Category;
            rvm.StartersLastYear = regatta.StartersLastYear;
            rvm.Organizer = regatta.Organizer;

            rvm.RegattaOldclasses = roc;
            rvm.RegattaCampingFees = rcf;
            rvm.RegattaCompetitions = comp;
            rvm.RegattaStartingFees = rsf;

            rvm.Oldclasses = _context.Oldclasses.ToList();
            rvm.CampingFees = _context.CampingFees.ToList();
            rvm.StartingFees = _context.StartingFees.Include(e => e.Boatclasses).Include(e => e.Oldclasses).ToList();
            rvm.Raceclasses = _context.Raceclasses.ToList();
            rvm.Competitions = _context.Competitions.Include(e => e.Boatclasses).Include(e => e.Raceclasses).ToList();

            return rvm;
        }

        private bool RegattaExists(int id)
        {
            return _context.Regattas.Any(e => e.RegattaId == id);
        }
    }
}
