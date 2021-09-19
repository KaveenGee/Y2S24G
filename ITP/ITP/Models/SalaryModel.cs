using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITP.Models
{
    public class SalaryModel
    {
        [Key]
        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public int EmpNIC { get; set; }

        public int BasicSalary { get; set; }

        public int OverTime { get; set; }

        public string WorkDates { get; set; }
    }
}
