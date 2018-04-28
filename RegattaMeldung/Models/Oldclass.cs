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
        public int OldclassId { get; set; }
        public string Name { get; set; }
        public int FromAge { get; set; }
        public int ToAge { get; set; }
        public List<RegattaOldclass> RegattaOldclasses { get; set; }
    }
}
