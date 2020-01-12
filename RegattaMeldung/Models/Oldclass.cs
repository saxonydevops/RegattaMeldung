using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Oldclass
    {
        [Key]
        [Display(Name = "Altersklasse")]
        public int OldclassId { get; set; }
        [Display(Name = "Altersklasse")]
        public string Name { get; set; }
        [Display(Name = "Von")]
        public int FromAge { get; set; }
        [Display(Name = "Bis")]
        public int ToAge { get; set; }
        public List<RegattaOldclass> RegattaOldclasses { get; set; }
    }
}
