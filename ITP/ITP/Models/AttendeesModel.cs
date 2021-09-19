using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class AttendeesModel
    {
        [Key]
        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public string WorkDate { get; set; }

        public string WorkStartTime { get; set; }

        public string WorkEndTime { get; set; }

        public string Availability { get; set; }

        public int OverTime { get; set; }
    }
}
