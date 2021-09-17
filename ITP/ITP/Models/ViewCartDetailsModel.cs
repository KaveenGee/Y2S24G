using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class ViewCartDetailsModel
    {

        public int? IItemId { get; set; }
        public string IBrand { get; set; }
        public decimal IUPrice { get; set; }
        public string IDescription { get; set; }
        public int Quntity { get; set; }
        public string ImageName { get; set; }
    }
}
