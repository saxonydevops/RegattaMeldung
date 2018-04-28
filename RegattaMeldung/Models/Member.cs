using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Member
    {
        [Key]
        public int MemberId { get; set; }
        [Display(Name ="Nachname")]
        public string LastName { get; set; }
        [Display(Name ="Vorname")]
        public string FirstName { get; set; }
        [Display(Name ="Geburtsjahr")]
        public int Birthyear { get; set; }
        [Display(Name ="Geschlecht")]
        public string Gender { get; set; }
        public bool isRented { get; set; }
        public int RentedToClubId { get; set; }
        public int RentYear { get; set; }
        public string FullName
        {
            get
            {
                return string.Format("{0}, {1}", LastName, FirstName);
            }
        }
        public int ClubId { get; set; }
        public Club Club { get; set; }
        public List<ReportedStartboatMember> ReportedStartboatMembers { get; set; }
        public List<ReportedStartboatStandby> ReportedStartboatStandbys { get; set; }
    }
}
