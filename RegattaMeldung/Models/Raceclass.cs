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
        public int RaceclassId { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public List<Competition> Competitions { get; set; }
    }
}
