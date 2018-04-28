using RegattaMeldung.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.ViewModels
{
    public class RegattaVM
    {
        public int RegattaId { get; set; }
        [Display(Name = "Name")]
        public string RegattaName { get; set; }
        [Display(Name = "Von")]
        public DateTime RegattaVon { get; set; }
        [Display(Name = "Bis")]
        public DateTime RegattaBis { get; set; }
        [Display(Name = "Wassertiefe")]
        public int Waterdepth { get; set; }
        [Display(Name = "Startbahnen")]
        public int Startslots { get; set; }
        [Display(Name = "Meldung")]
        [DataType(DataType.MultilineText)]
        public string ReportText { get; set; }
        [Display(Name = "Meldetermin")]
        public DateTime ReportSchedule { get; set; }
        [Display(Name = "Meldeeröffnung")]
        public DateTime ReportOpening { get; set; }
        [Display(Name = "Meldeanschrift")]
        [DataType(DataType.MultilineText)]
        public string ReportAddress { get; set; }
        [Display(Name = "Telefon")]
        public string ReportTel { get; set; }
        [Display(Name = "Fax")]
        public string ReportFax { get; set; }
        [Display(Name = "E-Mail")]
        public string ReportMail { get; set; }
        [Display(Name = "Kampfrichter")]
        public string Judge { get; set; }
        [Display(Name = "Auszeichnungen")]
        [DataType(DataType.MultilineText)]
        public string Awards { get; set; }
        [Display(Name = "Sicherheit")]
        [DataType(DataType.MultilineText)]
        public string Security { get; set; }
        [Display(Name = "Zeitplan")]
        [DataType(DataType.MultilineText)]
        public string ScheduleText { get; set; }
        [Display(Name = "Teilnehmergebühren")]
        public float SubscriberFee { get; set; }
        [Display(Name = "Unterkunft")]
        [DataType(DataType.MultilineText)]
        public string Accomodation { get; set; }
        [Display(Name = "Bemerkungen")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        [Display(Name = "Verpflegung")]
        [DataType(DataType.MultilineText)]
        public string Catering { get; set; }
        [Display(Name = "Gewässer")]
        public int WaterId { get; set; }
        [Display(Name = "Ausrichtender Verein")]
        public int ClubId { get; set; }
        public IEnumerable<Oldclass> Oldclasses { get; set; }
        [Display(Name = "Startklassen")]
        public IEnumerable<RegattaOldclass> RegattaOldclasses { get; set; }
        public IEnumerable<CampingFee> CampingFees { get; set; }
        [Display(Name = "Zeltplatzgebühren")]
        public IEnumerable<RegattaCampingFee> RegattaCampingFees { get; set; }
        public IEnumerable<StartingFee> StartingFees { get; set; }
        [Display(Name = "Startgebühren")]
        public IEnumerable<RegattaStartingFee> RegattaStartingFees { get; set; }
        public IEnumerable<Competition> Competitions { get; set; }
        [Display(Name = "Wettkämpfe")]
        public IEnumerable<RegattaCompetition> RegattaCompetitions { get; set; }
        public IEnumerable<Raceclass> Raceclasses { get; set; }
        [Display(Name = "Teilnehmende Vereine")]
        public IEnumerable<RegattaClub> RegattaClubs { get; set; }        
    }
}
