using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class RegattaOldclass
    {
        public int OldclassId { get; set; }
        public Oldclass Oldclasses { get; set; }
        public int RegattaId { get; set; }
        public Regatta Regattas { get; set; }
    }
}
