using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class CampingFee
    {
        [Key]
        public int CampingFeeId { get; set; }
        public string Name { get; set; }
        [Display(Name = "Gebühr")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Display(Name = "Name")]
        public virtual string LongName
        {
            get
            {
                return string.Format("{0} = {1} EUR", Name, Amount);
            }
        }
        public List<RegattaCampingFee> RegattaCampingFees { get; set; }
    }
}
