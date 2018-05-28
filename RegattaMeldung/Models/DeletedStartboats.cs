using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class DeletedStartboats
    {
        [Key]
        public int DeletedStartboatId { get; set; }
        public int ReportedStartboatId { get; set; }
        public string Gender { get; set; }
        public DateTime deleteDate { get; set; }
        public bool wasLate { get; set; }
        public int ClubId { get; set; }        
        public int RegattaId { get; set; }        
        public int ReportedRaceId { get; set; }        
    }
}
