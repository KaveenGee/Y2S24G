using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class ViewItemDetailsModel
    {
        public int IItemId { get; set; }

        public string IBrand { get; set; }
        public decimal IUPrice { get; set; }
        public string IDescription { get; set; }
        [Range(1, 50)]
        public int Quntity { get; set; }
        public string ImageName { get; set; }

    }
}
