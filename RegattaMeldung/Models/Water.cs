using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class Water
    {
        [Key]
        public int WaterId { get; set; }
        public string Name { get; set; }
    }
}
