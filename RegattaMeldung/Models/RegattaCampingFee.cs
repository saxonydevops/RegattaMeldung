using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class RegattaCampingFee
    {
        [Key]
        public int CampingFeeId { get; set; }
        public CampingFee CampingFee { get; set; }
        public int RegattaId { get; set; }
        public Regatta Regatta { get; set; }
    }
}
