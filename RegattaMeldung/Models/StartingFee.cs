using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class StartingFee
    {
        [Key]
        public int StartingFeeId { get; set; }
        [Display(Name = "Gebühr")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Display(Name = "Bootsklasse")]
        public int BoatclassId { get; set; }
        [Display(Name = "Bootsklasse")]
        public Boatclass Boatclasses { get; set; }
        [Display(Name = "Von Altersklasse")]
        public int FromOldclassId { get; set; }
        [Display(Name = "Bis Altersklasse")]
        public int ToOldclassId { get; set; }        
        public List<RegattaStartingFee> RegattaStartingFees { get; set; }
    }
}
