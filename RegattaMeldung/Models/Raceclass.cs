using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Raceclass
    {
        [Key]
        [Display(Name = "Rennklasse")]
        public int RaceclassId { get; set; }
        [Display(Name = "Rennklasse")]
        public string Name { get; set; }
        [Display(Name = "Streckenlänge")]
        public int Length { get; set; }
        public List<Competition> Competitions { get; set; }
    }
}
