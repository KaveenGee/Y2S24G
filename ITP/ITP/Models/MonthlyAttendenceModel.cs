using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class MonthlyAttendenceModel
    {
        [Key]
        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public int DayCount { get; set; }

        public int OverTime { get; set; }

    }
}
