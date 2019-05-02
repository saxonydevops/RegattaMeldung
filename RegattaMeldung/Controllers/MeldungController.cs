using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Data;
using RegattaMeldung.Models;

namespace RegattaMeldung.Controllers
{
    public class MeldungController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeldungController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Meldung
        public ActionResult Index(string guid, string sortby)
        {
            if(guid == null)
            {
                return RedirectToAction("Index","Home");
            }

            var rc = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid);

            if(rc == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var club = _context.Clubs.FirstOrDefault(e => e.ClubId == rc.ClubId);
            var regatta = _context.Regattas.Include(e => e.Club).FirstOrDefault(e => e.RegattaId == rc.RegattaId);

            IEnumerable<ReportedRace> reportedraces = _context.ReportedRaces.
                Include(e => e.Oldclass).
                Include(e => e.Competition.Boatclasses).
                Include(e => e.Competition.Raceclasses).
                Where(e => e.RegattaId == rc.RegattaId).
                OrderBy(e => e.Oldclass.FromAge).
                ThenBy(e => e.Oldclass.Name).
                ThenBy(e => e.Gender).
                ThenBy(e => e.Competition.Boatclasses.Name).
                ThenBy(e => e.Competition.Raceclasses.Length);

            if (sortby == "Renncode")
            {
                reportedraces = reportedraces.OrderBy(e => e.RaceCode).ThenBy(e => e.Competition.Raceclasses.Length).ThenBy(e => e.Competition.Boatclasses.Name).ThenBy(e => e.Oldclass.FromAge);
            }            
            else if(sortby == "Alter")
            {
                reportedraces = reportedraces.OrderBy(e => e.Oldclass.FromAge).ThenBy(e => e.Competition.Raceclasses.Length).ThenBy(e => e.Competition.Boatclasses.Name);
            }
            else if (sortby == "AlterDesc")
            {
                reportedraces = reportedraces.OrderByDescending(e => e.Oldclass.FromAge).ThenBy(e => e.Competition.Raceclasses.Length).ThenBy(e => e.Competition.Boatclasses.Name);
            }
            else if (sortby == "Bootsklasse")
            {
                reportedraces = reportedraces.OrderBy(e => e.Competition.Boatclasses.Name).ThenBy(e => e.Competition.Raceclasses.Length).ThenBy(e => e.Oldclass.FromAge);
            }
            else if (sortby == "Strecke")
            {
                reportedraces = reportedraces.OrderBy(e => e.Competition.Raceclasses.Length).ThenBy(e => e.Competition.Boatclasses.Name).ThenBy(e => e.Oldclass.FromAge);
            }

            IEnumerable<ReportedStartboat> reportedstartboats = _context.ReportedStartboats.Where(e => e.RegattaId == rc.RegattaId && e.ClubId == rc.ClubId);

            ViewBag.RegattaClubs = rc;
            ViewBag.Club = club;
            ViewBag.Regatta = regatta;            
            ViewBag.ReportedRaces = reportedraces;
            ViewBag.ReportedStartboats = reportedstartboats;
            ViewBag.Guid = guid;
            ViewBag.ClubComment = rc.Comment;

            return View();
        }

        // GET: Meldung/Details/5
        public ActionResult Details(int id, string guid, bool doppelt, bool allAvailable, bool isracegroup, int RGClubId)
        {
            if(guid == null)
            {
                return RedirectToAction("Index","Home");
            }
            var model = _context.ReportedRaces.Include(e => e.Competition.Boatclasses).Include(e => e.Competition.Raceclasses).Include(e => e.Oldclass).Include(e => e.Regatta).FirstOrDefault(e => e.ReportedRaceId == id);            

            int yearnow = DateTime.Now.Year;
            int ageFrom = 0;
            int ageTo = yearnow - model.Oldclass.ToAge;

            bool isLate = false;

            if(DateTime.Compare(DateTime.Now,model.Regatta.ReportOpening) > 0)
            {
                isLate = true;
            }

            if(allAvailable == true)
            {
                ageFrom = getAgeFrom(model.Oldclass.FromAge, true);
                ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, true);
            }
            else
            {
                ageFrom = getAgeFrom(model.Oldclass.FromAge, false);
                ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, false);
            }

            var clubid = _context.RegattaClubs.Include(e => e.Club).FirstOrDefault(e => e.Guid == guid).ClubId;            
            var sbMembers = _context.ReportedStartboatMembers.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == id).Select(e => e.MemberId).ToList();
            var sbStandbys = _context.ReportedStartboatStandbys.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == id).Select(e => e.MemberId).ToList();
            var availMembers = _context.Members.Where(e => (e.ClubId == clubid || e.RentedToClubId == clubid) && (!sbMembers.Contains(e.MemberId))).ToList();
            var allMembers = _context.Members.Include(e => e.Club).ToList();
            var vStartboats = _context.ReportedStartboats.Where(e => e.ReportedRaceId == id && e.ClubId == clubid).ToList();

            var freestartslots = model.Regatta.Startslots;

            List<int> clubids = _context.RegattaClubs.Select(e => e.ClubId).ToList();

            if(RGClubId > 0)
            {
                var rgclubmembers = _context.Members.Where(e => e.ClubId == RGClubId);                  
                ViewBag.SelectedRGClub = _context.Clubs.FirstOrDefault(e => e.ClubId == RGClubId);              
            }
            else
            {
                ViewBag.RGClubs = new SelectList(_context.Clubs.Where(e => clubids.Contains(e.ClubId) && e.Members.Count > 0).OrderBy(e => e.Name).ToList(), "ClubId", "Name");                    
            }

            if(_context.RRFreeStartslots.Any(e => e.ReportedRaceId == id))
            {
                freestartslots = _context.RRFreeStartslots.FirstOrDefault(e => e.ReportedRaceId == id).FreeStartslots;
            }

            if (model.Gender == "M" || model.Gender == "W")
            {
                IEnumerable<Member> mbl1;
                IEnumerable<Member> sbl1;
                
                if(RGClubId > 0 && model.Competition.Boatclasses.Seats > 1)
                {
                    sbMembers = _context.ReportedStartboatMembers.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.ClubId == RGClubId || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == id).Select(e => e.MemberId).ToList();
                    sbStandbys = _context.ReportedStartboatStandbys.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.ClubId == RGClubId || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == id).Select(e => e.MemberId).ToList();                    
                    availMembers = _context.Members.Where(e => (e.ClubId == clubid || e.ClubId == RGClubId || e.RentedToClubId == clubid) && (!sbMembers.Contains(e.MemberId))).ToList();

                    mbl1 = availMembers.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || e.ClubId == RGClubId || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                    sbl1 = _context.Members.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || e.ClubId == RGClubId || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                }
                else
                {
                    mbl1 = availMembers.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                    sbl1 = _context.Members.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                }   

                var memberlist1 = new SelectList(mbl1, "MemberId", "FullName");             
                
                if(memberlist1.Count() == 0)
                {
                    ViewBag.MemberCount = 0;
                }
                
                ViewBag.MemberId = memberlist1;
                ViewBag.StandbyId = new SelectList(sbl1, "MemberId", "FullName");
            }
            else
            {
                IEnumerable<Member> mbl2;
                IEnumerable<Member> sbl2;

                if(RGClubId > 0)
                {
                    sbMembers = _context.ReportedStartboatMembers.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.ClubId == RGClubId || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == id).Select(e => e.MemberId).ToList();
                    sbStandbys = _context.ReportedStartboatStandbys.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.ClubId == RGClubId || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == id).Select(e => e.MemberId).ToList();                    
                    availMembers = _context.Members.Where(e => (e.ClubId == clubid || e.ClubId == RGClubId || e.RentedToClubId == clubid) && (!sbMembers.Contains(e.MemberId))).ToList();
                    
                    mbl2 = availMembers.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || e.ClubId == RGClubId || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                    sbl2 = _context.Members.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || e.ClubId == RGClubId || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                }
                else
                {
                    mbl2 = availMembers.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                    sbl2 = _context.Members.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                } 

                var memberlist2 = new SelectList(mbl2, "MemberId", "FullName");

                if (memberlist2.Count() == 0)
                {
                    ViewBag.MemberCount = 0;
                }

                ViewBag.MemberId = memberlist2;
                ViewBag.StandbyId = new SelectList(sbl2, "MemberId", "FullName");
            }            

            IEnumerable<Club> allClubs = _context.Clubs;

            ViewBag.startboats = vStartboats;
            ViewBag.startboatmembers = _context.ReportedStartboatMembers;
            ViewBag.startboatstandbys = _context.ReportedStartboatStandbys;
            ViewBag.members = allMembers;
            ViewBag.AllClubs = allClubs;
            ViewBag.Guid = guid;
            ViewBag.ThisYear = yearnow;
            ViewBag.Club = _context.RegattaClubs.Include(e => e.Club).FirstOrDefault(e => e.Guid == guid).Club;
            ViewBag.doppelt = doppelt;
            ViewBag.allAvailable = allAvailable;
            ViewBag.FreeStartslots = freestartslots;
            ViewBag.isLate = isLate;
            ViewBag.RGClubId = RGClubId;                      
            

            if(model.Gender == "M")
            {
                ViewBag.genderdesc = "männliche";
            }
            if (model.Gender == "W")
            {
                ViewBag.genderdesc = "weibliche";
            }
            if (model.Gender == "X")
            {
                ViewBag.genderdesc = "mixed";
            }

            return View(model);
        }

        public IActionResult RegattaInfos(string guid)
        {
            if(guid == null)
            {
                return RedirectToAction("Index","Home");
            }

            var rc = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid);
            var club = _context.Clubs.FirstOrDefault(e => e.ClubId == rc.ClubId);
            var regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == rc.RegattaId);

            ViewBag.Guid = guid;

            return View(regatta);
        }

        public IActionResult AddStartboat(int id, int seat1, int seat2, int seat3, int seat4, int seat5, int seat6, int seat7, int seat8, int standby1, int standby2, int standby3, int standby4,
            int standby5, int standby6, int standby7, int standby8, bool standbycheck1, bool standbycheck2, bool standbycheck3, bool standbycheck4, bool standbycheck5,
            bool standbycheck6, bool standbycheck7, bool standbycheck8, int clubid, int seatnumber, bool isracegroup, string guid)
        {
            var race = _context.ReportedRaces.FirstOrDefault(e => e.ReportedRaceId == id);
            var rid = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid).RegattaId;
            var regatta = _context.Regattas.FirstOrDefault(e => e.RegattaId == rid);
            bool isLate = false;
            bool nostartslot = false;            

            if(DateTime.Compare(DateTime.Now,regatta.ReportOpening) > 0)
            {
                isLate = true;
            }

            if(_context.RRFreeStartslots.Any(e => e.ReportedRaceId == race.ReportedRaceId))
            {
                var rrfree = _context.RRFreeStartslots.FirstOrDefault(e => e.ReportedRaceId == race.ReportedRaceId);

                if (rrfree.FreeStartslots <= 0)
                {
                    if(isLate == false)
                    {
                        rrfree.FreeStartslots = rrfree.FreeStartslots + regatta.Startslots -1;
                    }
                    else
                    {
                        nostartslot = true;
                        rrfree.FreeStartslots = rrfree.FreeStartslots - 1;
                    }                    
                }
                else
                {
                    rrfree.FreeStartslots = rrfree.FreeStartslots - 1;
                }                

                _context.RRFreeStartslots.Update(rrfree);
            }  
            else
            {
                RRFreeStartslots newrrfree = new RRFreeStartslots();
                newrrfree.ReportedRaceId = race.ReportedRaceId;
                newrrfree.FreeStartslots = regatta.Startslots - 1;
                _context.RRFreeStartslots.Add(newrrfree);
            }              

            _context.ReportedStartboats.Add(new ReportedStartboat { ClubId = clubid, ReportedRaceId = id, RegattaId = rid, Gender = race.Gender, isLate = isLate, modifiedDate = DateTime.Now, NoStartslot = nostartslot });            

            List<int> seats = new List<int>();

            bool isDouble = false;

            if (seatnumber == 1)
            {
                if(seat1 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }

                seats.Add(seat1);

                if(standbycheck1 == true)
                {
                    if(standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)  
                {
                    isDouble = true;
                }      

                if(isDouble == false)
                {
                    _context.SaveChanges();
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }

                    _context.SaveChanges();
                }
            }
            else if (seatnumber == 2)
            {
                if(seat1 == 0 || seat2 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }

                seats.Add(seat1); 
                seats.Add(seat2);

                if(standbycheck1 == true)
                {
                    if(standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }
                if(standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby2);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1) 
                {
                    if(seat2 == 1)
                    {
                        seat2 = 2;
                    }
                    else
                    {
                        isDouble = true;
                    }
                }                        
                
                if(isDouble == false)
                {
                    _context.SaveChanges();

                    if(standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if(standbycheck2 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby2, Standbynumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat2, Seatnumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    _context.SaveChanges();
                }                
            }
            else if (seatnumber == 4)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }

                seats.Add(seat1); 
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);

                if(standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }
                if(standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby2);
                }
                if(standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby3);
                }
                if(standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby4);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    if(seat2 == 1)
                    {
                        seat2 = 2;
                    }
                    else if(seat3 == 1)
                    {
                        seat3 = 3;
                    }
                    else if(seat4 == 1)
                    {
                        seat4 = 4;
                    }
                    else
                    {
                        isDouble = true;
                    }
                }  

                if(isDouble == false)
                {
                   _context.SaveChanges();

                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat2, Seatnumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat3, Seatnumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat4, Seatnumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck2 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby2, Standbynumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck3 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby3, Standbynumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck4 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby4, Standbynumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }

                    _context.SaveChanges();
                }
            }
            else if (seatnumber == 8)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0 || seat5 == 0 || seat6 == 0 || seat7 == 0 || seat8 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }

                seats.Add(seat1); 
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);
                seats.Add(seat5); 
                seats.Add(seat6);
                seats.Add(seat7);
                seats.Add(seat8);

                if(standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }
                if(standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby2);
                }
                if(standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby3);
                }
                if(standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby4);
                }
                if(standbycheck5 == true)
                {
                    if (standby5 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby5);
                }
                if(standbycheck6 == true)
                {
                    if (standby6 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby6);
                }
                if(standbycheck7 == true)
                {
                    if (standby7 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby7);
                }
                if(standbycheck8 == true)
                {
                    if (standby8 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby8);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                }  

                if(isDouble == false)
                {
                    _context.SaveChanges();
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat1, Seatnumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat2, Seatnumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat3, Seatnumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat4, Seatnumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat5, Seatnumber = 5, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat6, Seatnumber = 6, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat7, Seatnumber = 7, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Last().ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seat8, Seatnumber = 8, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });

                    if (standbycheck1 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby1, Standbynumber = 1, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck2 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby2, Standbynumber = 2, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck3 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby3, Standbynumber = 3, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck4 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby4, Standbynumber = 4, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck5 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby5, Standbynumber = 5, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck6 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby6, Standbynumber = 6, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck7 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby7, Standbynumber = 7, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }
                    if (standbycheck8 == true)
                    {
                        _context.ReportedStartboats.Include(e => e.ReportedStartboatStandbys).Last().ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standby8, Standbynumber = 8, ReportedStartboatId = _context.ReportedStartboats.Max(i => i.ReportedStartboatId) });
                    }

                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Details", "Meldung", new { id = id , guid = guid, doppelt = isDouble});
        }

        public IActionResult DeleteStartboat(int id, string guid)
        {
            var sbmembers = _context.ReportedStartboatMembers.Where(e => e.ReportedStartboatId == id);
            var sbstandby = _context.ReportedStartboatStandbys.Where(e => e.ReportedStartboatId == id);

            foreach (var sbm in sbmembers)
            {
                _context.ReportedStartboatMembers.Remove(sbm);
            }

            foreach (var sbs in sbstandby)
            {
                _context.ReportedStartboatStandbys.Remove(sbs);
            }            

            var original = _context.ReportedStartboats.FirstOrDefault(e => e.ReportedStartboatId == id);
            if (original != null)
            {
                _context.DeletedStartboats.Add(new DeletedStartboats { ReportedStartboatId = original.ReportedStartboatId, ClubId = original.ClubId, Gender = original.Gender, RegattaId = original.RegattaId, ReportedRaceId = original.ReportedRaceId, wasLate = original.isLate, deleteDate = DateTime.Now });
                var rrfreesl = _context.RRFreeStartslots.FirstOrDefault(e => e.ReportedRaceId == original.ReportedRaceId);
                rrfreesl.FreeStartslots = rrfreesl.FreeStartslots + 1;
                _context.RRFreeStartslots.Update(rrfreesl);
                _context.ReportedStartboats.Remove(original);
                _context.SaveChanges();
            }
            return RedirectToAction("Details", "Meldung", new { id = original.ReportedRaceId, guid = guid });
        }

        [HttpGet]
        public ActionResult EditStartboat(int id, string guid, bool allAvailable)
        {
            if(guid == null)
            {
                return RedirectToAction("Index","Home");
            }            

            var startboat = _context.ReportedStartboats.Include(e => e.ReportedStartboatMembers).Include(e => e.ReportedStartboatStandbys).FirstOrDefault(e => e.ReportedStartboatId == id);
            var model = _context.ReportedRaces.Include(e => e.Competition.Boatclasses).Include(e => e.Competition.Raceclasses).Include(e => e.Oldclass).FirstOrDefault(e => e.ReportedRaceId == startboat.ReportedRaceId);

            int yearnow = DateTime.Now.Year;
            int ageFrom = 0;
            int ageTo = yearnow - model.Oldclass.ToAge;

            if (allAvailable == true)
            {
                ageFrom = getAgeFrom(model.Oldclass.FromAge, true);
                ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, true);
            }
            else
            {
                ageFrom = getAgeFrom(model.Oldclass.FromAge, false);
                ageTo = getAgeTo(model.Oldclass.FromAge, model.Oldclass.ToAge, false);
            }           

            var clubid = _context.RegattaClubs.Include(e => e.Club).FirstOrDefault(e => e.Guid == guid).ClubId;   

            var sbMembers = _context.ReportedStartboatMembers.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == startboat.ReportedRaceId).Select(e => e.MemberId).ToList();
            var sbStandbys = _context.ReportedStartboatStandbys.Include(e => e.Member).Where(e => (e.Member.ClubId == clubid || e.Member.RentedToClubId == clubid) && e.ReportedStartboat.ReportedRaceId == startboat.ReportedRaceId).Select(e => e.MemberId).ToList();
            var allMembers = _context.Members.Include(e => e.Club).Where(e => e.ClubId == clubid || e.RentedToClubId == clubid);
            var vStartboats = _context.ReportedStartboats.Where(e => e.ReportedRaceId == id && e.ClubId == clubid).ToList();
            var editSBMember = _context.ReportedStartboatMembers.Where(e => e.ReportedStartboatId == startboat.ReportedStartboatId).OrderBy(e => e.Seatnumber);            
            var editSBStandby = _context.ReportedStartboatStandbys.Where(e => e.ReportedStartboatId == startboat.ReportedStartboatId).OrderBy(e => e.Standbynumber);
            
            
            IQueryable<Member> tempmemberlist = _context.Members;
            IQueryable<Member> tempstandbylist = _context.Members;

            int i = 0;        
            Member[] member = new Member[model.Competition.Boatclasses.Seats];
            SelectList[] memberlist = new SelectList[model.Competition.Boatclasses.Seats];
            Member[] standby = new Member[model.Competition.Boatclasses.Seats];
            SelectList[] standbylist = new SelectList[model.Competition.Boatclasses.Seats];
            string listitem = "";
            string sbscheck = "";
            string oldmemberid = "";
            string oldstandbyid = "";

            foreach(var esbm in editSBMember)
            {                    
                member[i] = _context.Members.FirstOrDefault(e => e.MemberId == esbm.MemberId);
                oldmemberid = "oldmemberid" + i;
                ViewData[oldmemberid] = member[i].MemberId;
                tempmemberlist = _context.Members.Where(e => (e.ClubId == clubid || e.RentedToClubId == clubid) && (!sbMembers.Contains(e.MemberId)) || e.MemberId == esbm.MemberId);

                if(model.Gender == "M" || model.Gender == "W")
                {                    
                    tempmemberlist = tempmemberlist.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                    memberlist[i] = new SelectList(tempmemberlist, "MemberId", "FullName",member[i].MemberId);                    
                }  
                else
                {
                    tempmemberlist = tempmemberlist.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                    memberlist[i] = new SelectList(tempmemberlist, "MemberId", "FullName",member[i].MemberId);
                }

                listitem = "memberlist" + i;
                ViewData[listitem] = memberlist[i];                

                if(editSBStandby.Any(e => e.Standbynumber == i+1))
                {
                    var sbs = editSBStandby.FirstOrDefault(e => e.Standbynumber == i+1);
                    standby[i] = _context.Members.FirstOrDefault(e => e.MemberId == sbs.MemberId);
                    tempstandbylist = _context.Members.Where(e => (e.ClubId == clubid || e.RentedToClubId == clubid) && (!sbMembers.Contains(e.MemberId)) || e.MemberId == sbs.MemberId);

                    if(model.Gender == "M" || model.Gender == "W")
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName", standby[i].MemberId);                    
                    }  
                    else
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName", standby[i].MemberId);
                    }                  

                    sbscheck = "sbscheck" + i;
                    ViewData[sbscheck] = "checked";
                    listitem = "standbylist" + i;
                    ViewData[listitem] = standbylist[i];                    
                    oldstandbyid = "oldstandbyid" + i;
                    ViewData[oldstandbyid] = standby[i].MemberId;
                }
                else
                {
                    tempstandbylist = _context.Members.Where(e => (e.ClubId == clubid || e.RentedToClubId == clubid) && !sbMembers.Contains(e.MemberId));
                    
                    if(model.Gender == "M" || model.Gender == "W")
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Gender == model.Gender && e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName");                    
                    }  
                    else
                    {
                        tempstandbylist = tempstandbylist.Where(e => e.Birthyear >= ageTo && e.Birthyear <= ageFrom && (e.ClubId == clubid || (e.RentedToClubId == clubid && e.isRented == true && e.RentYear == yearnow))).OrderBy(e => e.LastName).Distinct();
                        standbylist[i] = new SelectList(tempstandbylist, "MemberId", "FullName");
                    }  

                    listitem = "standbylist" + i;
                    ViewData[listitem] = standbylist[i];
                    sbscheck = "sbscheck" + i;
                    ViewData[sbscheck] = "";                    
                }

                i++;
            }     

            IEnumerable<Club> allClubs = _context.Clubs;

            ViewBag.startboats = vStartboats;
            ViewBag.startboatmembers = _context.ReportedStartboatMembers;
            ViewBag.startboatstandbys = _context.ReportedStartboatStandbys;
            ViewBag.members = allMembers;
            ViewBag.AllClubs = allClubs;
            ViewBag.Guid = guid;
            ViewBag.ThisYear = yearnow;
            ViewBag.Club = _context.RegattaClubs.Include(e => e.Club).FirstOrDefault(e => e.Guid == guid).Club;
            ViewBag.Startboat = startboat;
            ViewBag.ReportedStartboatId = startboat.ReportedStartboatId;
            ViewBag.allAvailable = allAvailable;

            if (model.Gender == "M")
            {
                ViewBag.genderdesc = "männliche";
            }
            if (model.Gender == "W")
            {
                ViewBag.genderdesc = "weibliche";
            }
            if (model.Gender == "X")
            {
                ViewBag.genderdesc = "mixed";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult EditStartboat(int id, int seat1, int seat2, int seat3, int seat4, int seat5, int seat6, int seat7, int seat8, int standby1, int standby2, int standby3, int standby4,
            int standby5, int standby6, int standby7, int standby8, bool standbycheck1, bool standbycheck2, bool standbycheck3, bool standbycheck4, bool standbycheck5,
            bool standbycheck6, bool standbycheck7, bool standbycheck8, int oldmemberid1, int oldmemberid2, int oldmemberid3, int oldmemberid4, int oldmemberid5, int oldmemberid6,
            int oldmemberid7, int oldmemberid8, int oldstandbyid1, int oldstandbyid2, int oldstandbyid3, int oldstandbyid4, int oldstandbyid5, int oldstandbyid6, int oldstandbyid7,
            int oldstandbyid8, int clubid, int seatnumber, string guid)
        {
            var rid = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid).RegattaId;
            var startboat = _context.ReportedStartboats.FirstOrDefault(e => e.ReportedStartboatId == id);
            var startboatmembers = _context.ReportedStartboatMembers.Where(e => e.ReportedStartboatId == id);
            var startboatstandbys = _context.ReportedStartboatStandbys.Where(e => e.ReportedStartboatId == id);

            List<int> seats = new List<int>();

            bool isDouble = false;

            if (seatnumber == 1)
            {
                if (seat1 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }
                seats.Add(seat1);

                if(standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }      

                if(isDouble == false)
                {                    
                    EditSeat(1,seat1,standbycheck1,1,standby1,oldmemberid1,oldstandbyid1,startboat,startboatmembers,startboatstandbys);
                }
            }
            else if (seatnumber == 2)
            {
                if (seat1 == 0 || seat2 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }
                seats.Add(seat1); 
                seats.Add(seat2);

                if(standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }
                if(standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby2);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }                        
                
                if(isDouble == false)
                {                    
                    EditSeat(1,seat1,standbycheck1,1,standby1,oldmemberid1,oldstandbyid1,startboat,startboatmembers,startboatstandbys);                  
                    EditSeat(2,seat2,standbycheck2,2,standby2,oldmemberid2,oldstandbyid2,startboat,startboatmembers,startboatstandbys);
                }              
            }
            else if (seatnumber == 4)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }
                seats.Add(seat1); 
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);

                if(standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }
                if(standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby2);
                }
                if(standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby3);
                }
                if(standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby4);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }  

                if(isDouble == false)
                {
                    EditSeat(1, seat1, standbycheck1, 1, standby1, oldmemberid1, oldstandbyid1, startboat, startboatmembers, startboatstandbys);
                    EditSeat(2, seat2, standbycheck2, 2, standby2, oldmemberid2, oldstandbyid2, startboat, startboatmembers, startboatstandbys);
                    EditSeat(3, seat3, standbycheck3, 3, standby3, oldmemberid3, oldstandbyid3, startboat, startboatmembers, startboatstandbys);
                    EditSeat(4, seat4, standbycheck4, 4, standby4, oldmemberid4, oldstandbyid4, startboat, startboatmembers, startboatstandbys);
                }
            }
            else if (seatnumber == 8)
            {
                if (seat1 == 0 || seat2 == 0 || seat3 == 0 || seat4 == 0 || seat5 == 0 || seat6 == 0 || seat7 == 0 || seat8 == 0)
                {
                    return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                }
                seats.Add(seat1); 
                seats.Add(seat2);
                seats.Add(seat3);
                seats.Add(seat4);
                seats.Add(seat5); 
                seats.Add(seat6);
                seats.Add(seat7);
                seats.Add(seat8);

                if(standbycheck1 == true)
                {
                    if (standby1 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby1);
                }
                if(standbycheck2 == true)
                {
                    if (standby2 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby2);
                }
                if(standbycheck3 == true)
                {
                    if (standby3 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby3);
                }
                if(standbycheck4 == true)
                {
                    if (standby4 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby4);
                }
                if(standbycheck5 == true)
                {
                    if (standby5 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby5);
                }
                if(standbycheck6 == true)
                {
                    if (standby6 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby6);
                }
                if(standbycheck7 == true)
                {
                    if (standby7 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby7);
                }
                if(standbycheck8 == true)
                {
                    if (standby8 == 0)
                    {
                        return RedirectToAction("Details", "Meldung", new { id = id, guid = guid });
                    }
                    seats.Add(standby8);
                }

                if (seats.GroupBy(s => s).SelectMany(grp => grp.Skip(1)).Count() >= 1)
                {
                    isDouble = true;
                    ViewBag.doppelt = true;
                }  

                if(isDouble == false)
                {
                    EditSeat(1, seat1, standbycheck1, 1, standby1, oldmemberid1, oldstandbyid1, startboat, startboatmembers, startboatstandbys);
                    EditSeat(2, seat2, standbycheck2, 2, standby2, oldmemberid2, oldstandbyid2, startboat, startboatmembers, startboatstandbys);
                    EditSeat(3, seat3, standbycheck3, 3, standby3, oldmemberid3, oldstandbyid3, startboat, startboatmembers, startboatstandbys);
                    EditSeat(4, seat4, standbycheck4, 4, standby4, oldmemberid4, oldstandbyid4, startboat, startboatmembers, startboatstandbys);
                    EditSeat(5, seat5, standbycheck5, 5, standby5, oldmemberid5, oldstandbyid5, startboat, startboatmembers, startboatstandbys);
                    EditSeat(6, seat6, standbycheck6, 6, standby6, oldmemberid6, oldstandbyid6, startboat, startboatmembers, startboatstandbys);
                    EditSeat(7, seat7, standbycheck7, 7, standby7, oldmemberid7, oldstandbyid7, startboat, startboatmembers, startboatstandbys);
                    EditSeat(8, seat8, standbycheck8, 8, standby8, oldmemberid8, oldstandbyid8, startboat, startboatmembers, startboatstandbys);
                }
            }

            return RedirectToAction("Details", "Meldung", new { id = startboat.ReportedRaceId , guid = guid});
        }

        [HttpPost]
        public IActionResult AddComment(int id, string guid, string comment)
        {
            var rc = _context.RegattaClubs.FirstOrDefault(e => e.Guid == guid);

            if(rc != null)
            {
                rc.Comment = comment;
                _context.RegattaClubs.Update(rc);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Meldung", new { guid = rc.Guid });
        }

        private void EditSeat(int seatnumber, int seatmemberid, bool standbycheck, int standbynumber, int standbymemberid, int oldmemberid, int oldstandbyid
        , ReportedStartboat startboat, IQueryable<ReportedStartboatMember> startboatmembers, IQueryable<ReportedStartboatStandby> startboatstandbys)
        {         
            if(startboatmembers.Any(e => e.Seatnumber == seatnumber) && oldmemberid != seatmemberid)
            {
                var sbm = _context.ReportedStartboatMembers.FirstOrDefault(e => e.MemberId == oldmemberid && e.ReportedStartboatId == startboat.ReportedStartboatId && e.Seatnumber == seatnumber);
                if(sbm != null)
                {                    
                    _context.ReportedStartboatMembers.Remove(sbm);
                    _context.ReportedStartboatMembers.Add(new ReportedStartboatMember { MemberId = seatmemberid, Seatnumber = seatnumber, ReportedStartboatId = startboat.ReportedStartboatId });
                }
            }

            if (standbycheck == true)
            {
                if (startboatstandbys.Any(e => e.Standbynumber == standbynumber) && oldstandbyid != standbymemberid)
                {
                    var sbs = _context.ReportedStartboatStandbys.FirstOrDefault(e => e.Standbynumber == standbynumber && e.MemberId == oldstandbyid && e.ReportedStartboatId == startboat.ReportedStartboatId);

                    if(sbs != null)
                    {
                        _context.ReportedStartboatStandbys.Remove(sbs);
                        _context.ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standbymemberid, Standbynumber = standbynumber, ReportedStartboatId = startboat.ReportedStartboatId });
                    }                    
                }
                else if(!startboatstandbys.Any(e => e.Standbynumber == standbynumber))
                {
                    _context.ReportedStartboatStandbys.Add(new ReportedStartboatStandby { MemberId = standbymemberid, Standbynumber = standbynumber, ReportedStartboatId = startboat.ReportedStartboatId });                                
                }
            }   
            else
            {
                var sbs = _context.ReportedStartboatStandbys.FirstOrDefault(e => e.Standbynumber == standbynumber && e.MemberId == oldstandbyid && e.ReportedStartboatId == startboat.ReportedStartboatId);

                if (sbs != null)
                {
                    _context.ReportedStartboatStandbys.Remove(sbs);                    
                }
            }

            _context.SaveChanges();
        }

        private int getAgeFrom(int MemberFromAge, bool withAllAvailable)
        {
            int yearnow = DateTime.Now.Year;            
            int ageFrom = yearnow - MemberFromAge;

            if (withAllAvailable == true)
            {
                if (MemberFromAge == 0)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
                else if (MemberFromAge == 7)
                {
                    ageFrom = yearnow - 0;
                }
                else if (MemberFromAge == 8)
                {
                    ageFrom = yearnow - 0;
                }
                else if (MemberFromAge == 9)
                {
                    ageFrom = yearnow - 0;
                }
                else if (MemberFromAge == 10)
                {
                    ageFrom = yearnow - 7;
                }
                else if (MemberFromAge == 11)
                {
                    ageFrom = yearnow - 11;
                }
                else if (MemberFromAge == 12)
                {
                    ageFrom = yearnow - 12;
                }
                else if (MemberFromAge == 13)
                {
                    ageFrom = yearnow - 9;
                }
                else if (MemberFromAge == 14)
                {
                    ageFrom = yearnow - 13;
                }
                else if (MemberFromAge == 15)
                {
                    ageFrom = yearnow - 13;
                }
                else if (MemberFromAge == 17)
                {
                    ageFrom = yearnow - 12;
                }
                else if (MemberFromAge == 19)
                {
                    ageFrom = yearnow - 17;
                }
                else if (MemberFromAge == 32)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
                else if (MemberFromAge == 40)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
                else if (MemberFromAge == 50)
                {
                    ageFrom = yearnow - MemberFromAge;
                }
            }

            return ageFrom;
        }

        private int getAgeTo(int MemberFromAge, int MemberToAge, bool withAllAvailable)
        {
            int yearnow = DateTime.Now.Year;
            int ageFrom = 0;
            int ageTo = yearnow - MemberToAge;

            if(withAllAvailable == true)
            {
                if (MemberFromAge == 19)
                {
                    ageFrom = yearnow - 17;
                    ageTo = yearnow - 49;
                }
                else if (MemberFromAge == 32)
                {
                    ageFrom = yearnow - MemberFromAge;
                    ageTo = yearnow - 99;
                }
                else if (MemberFromAge == 40)
                {
                    ageFrom = yearnow - MemberFromAge;
                    ageTo = yearnow - 99;
                }
                else if (MemberFromAge == 50)
                {
                    ageFrom = yearnow - MemberFromAge;
                    ageTo = yearnow - 99;
                }
            }

            return ageTo;
        }
    }
}