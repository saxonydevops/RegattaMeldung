using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RegattaMeldung.Models;

namespace RegattaMeldung.Data
{
    public class RMInitializer
    {
        private ApplicationDbContext _context;

        public RMInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if(!_context.Boatclasses.Any())
            {
                _context.AddRange(_Boatclass);
                await _context.SaveChangesAsync();
            }

            if(!_context.Oldclasses.Any())
            {
                _context.AddRange(_Oldclass);
                await _context.SaveChangesAsync();
            }

            if(!_context.Raceclasses.Any())
            {
                _context.AddRange(_Raceclass);
                await _context.SaveChangesAsync();
            }

            if(!_context.CampingFees.Any())
            {
                _context.AddRange(_CampingFee);
                await _context.SaveChangesAsync();
            }
        }

        List<Boatclass> _Boatclass = new List<Boatclass>
        {
            new Boatclass()
            {
                Name = "K1",
                Seats = 1
            },
            new Boatclass()
            {
                Name = "K2",
                Seats = 2
            },
            new Boatclass()
            {
                Name = "K4",
                Seats = 4
            },
            new Boatclass()
            {
                Name = "C1",
                Seats = 1
            },
            new Boatclass()
            {
                Name = "C2",
                Seats = 2
            },
            new Boatclass()
            {
                Name = "C4",
                Seats = 4
            },
            new Boatclass()
            {
                Name = "S1",
                Seats = 1
            },
            new Boatclass()
            {
                Name = "S2",
                Seats = 2
            },
            new Boatclass()
            {
                Name = "S4",
                Seats = 4
            },
            new Boatclass()
            {
                Name = "S8",
                Seats = 8
            }
        };

        List<Oldclass> _Oldclass = new List<Oldclass>
        {
            new Oldclass()
            {
                FromAge = 0,
                ToAge = 6,
                Name = "Schüler D"
            },
            new Oldclass()
            {
                FromAge = 7,
                ToAge = 9,
                Name = "Schüler C"
            },
            new Oldclass()
            {
                FromAge = 10,
                ToAge = 12,
                Name = "Schüler B"
            },
            new Oldclass()
            {
                FromAge = 13,
                ToAge = 14,
                Name = "Schüler A"
            },
            new Oldclass()
            {
                FromAge = 15,
                ToAge = 16,
                Name = "Jugend"
            },
            new Oldclass()
            {
                FromAge = 17,
                ToAge = 18,
                Name = "Junioren"
            },
            new Oldclass()
            {
                FromAge = 19,
                ToAge = 31,
                Name = "Leistungsklasse"
            },
            new Oldclass()
            {
                FromAge = 32,
                ToAge = 39,
                Name = "Senioren A"
            },
            new Oldclass()
            {
                FromAge = 40,
                ToAge = 49,
                Name = "Senioren B"
            },
            new Oldclass()
            {
                FromAge = 50,
                ToAge = 59,
                Name = "Senioren C"
            },
            new Oldclass()
            {
                FromAge = 60,
                ToAge = 99,
                Name = "Senioren D"
            },
            new Oldclass()
            {
                FromAge = 13,
                ToAge = 13,
                Name = "Schüler A13"
            },
            new Oldclass()
            {
                FromAge = 14,
                ToAge = 14,
                Name = "Schüler A14"
            },
            new Oldclass()
            {
                FromAge = 10,
                ToAge = 10,
                Name = "Schüler B10"
            },
            new Oldclass()
            {
                FromAge = 7,
                ToAge = 7,
                Name = "Schüler C7"
            },
            new Oldclass()
            {
                FromAge = 8,
                ToAge = 8,
                Name = "Schüler C8"
            },
            new Oldclass()
            {
                FromAge = 9,
                ToAge = 9,
                Name = "Schüler C9"
            },
        };

        List<Raceclass> _Raceclass = new List<Raceclass>
        {
            new Raceclass()
            {
                Length = 200,
                Name = "200m"
            },
            new Raceclass()
            {
                Length = 500,
                Name = "500m"
            },
            new Raceclass()
            {
                Length = 1000,
                Name = "1000m"
            },
            new Raceclass()
            {
                Length = 2000,
                Name = "2000m"
            },
            new Raceclass()
            {
                Length = 4000,
                Name = "4000m"
            },
            new Raceclass()
            {
                Length = 6000,
                Name = "6000m"
            },
        };

        List<CampingFee> _CampingFee = new List<CampingFee>
        {
            new CampingFee()
            {
                Name = "Mannschaftszelte",
                Amount = 5
            },
            new CampingFee()
            {
                Name = "Wohnwagen",
                Amount = 10
            },
            new CampingFee()
            {
                Name = "Elektro-Anschluss",
                Amount = 2
            },
        };

        List<StartingFee> _StartingFee = new List<StartingFee>
        {
            new StartingFee()
            {
                Amount = 2,
                BoatclassId = 1,
                OldclassId = 2
            },
            new StartingFee()
            {
                Amount = 2,
                BoatclassId = 1,
                OldclassId = 3
            },
            new StartingFee()
            {
                Amount = 2,
                BoatclassId = 1,
                OldclassId = 4
            },
        };
    }
}
