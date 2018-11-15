using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Club
    {
        public int ClubId { get; set; }
        public string Name { get; set; }
        [Display(Name="Stadt")]
        public string City { get; set; }
        [Display(Name="Vereinsnummer")]
        public string VNr { get; set; }        
        [EmailAddress]
        public string EMail { get; set; }
        public List<Member> Members { get; set; }
        public List<Regatta> Regattas { get; set; }
        public List<ReportedStartboat> ReportedStartboats { get; set; }
        public List<RegattaClub> RegattaClubs { get; set; }
    }
}
