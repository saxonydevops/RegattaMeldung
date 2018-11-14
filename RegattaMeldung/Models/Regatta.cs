using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Regatta
    {
        [Key]
        public int RegattaId { get; set; }
        public string Name { get; set; }
        [Display(Name="Von")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime FromDate { get; set; }
        [Display(Name="Bis")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime ToDate { get; set; }
        public int Waterdepth { get; set; }
        public int Startslots { get; set; }
        public string ReportText { get; set; }
        public DateTime ReportOpening { get; set; }
        public DateTime ReportSchedule { get; set; }
        public string ReportAddress { get; set; }
        public string ReportTel { get; set; }
        public string ReportFax { get; set; }
        public string ReportMail { get; set; }
        public string Judge { get; set; }
        public string Awards { get; set; }
        public string Security { get; set; }
        public string ScheduleText { get; set; }
        public float SubscriberFee { get; set; }
        public string Accomodation { get; set; }
        public string Comment { get; set; }
        public string Catering { get; set; }
        public int ClubId { get; set; }
        [Display(Name="Verein")]
        public Club Club { get; set; }
        public int WaterId { get; set; }
        public Water Waters { get; set; }
        public List<RegattaCampingFee> RegattaCampingFees { get; set; }
        public List<RegattaStartingFee> RegattaStartingFees { get; set; }
        public List<RegattaOldclass> RegattaOldclasses { get; set; }
        public List<RegattaCompetition> RegattaCompetitions { get; set; }
        public List<ReportedStartboat> ReportedStartboats { get; set; }
        public List<RegattaClub> RegattaClubs { get; set; }
        public List<ReportedRace> ReportedRaces { get; set; }
    }
}
