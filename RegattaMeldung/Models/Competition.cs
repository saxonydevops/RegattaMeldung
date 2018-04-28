using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Competition
    {
        [Key]
        public int CompetitionId { get; set; }
        public int BoatclassId { get; set; }
        public Boatclass Boatclasses { get; set; }
        public int RaceclassId { get; set; }
        public Raceclass Raceclasses { get; set; }
        public string Name
        {
            get
            {
                return string.Format("{0} {1}", Boatclasses.Name, Raceclasses.Name);
            }
        }
        public List<RegattaCompetition> RegattaCompetitions { get; set; }
        public List<ReportedStartboat> ReportedStartboats { get; set; }
    }
}
