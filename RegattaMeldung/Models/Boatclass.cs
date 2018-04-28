using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegattaMeldung.Models
{
    public class Boatclass
    {
        [Key]
        public int BoatclassId { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public List<Competition> Competitions { get; set; }
    }
}
