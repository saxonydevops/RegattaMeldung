using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegattaMeldung.Models
{
    public class CampingFee
    {
        [Key]
        public int CampingFeeId { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
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
