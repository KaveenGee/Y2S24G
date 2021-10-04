using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITP.Models
{
    public enum Type2
    {
        HR, IM, DM, SM
    }
    public class Dropmodel2
    {
        public Type Type2 { get; set; }


        public static IEnumerable<SelectListItem> GetSelectItems2()
        {
            yield return new SelectListItem { Text = "HR Manager", Value = "HR" };
            yield return new SelectListItem { Text = "Inventory Manager", Value = "IM" };
            yield return new SelectListItem { Text = "Delivery Manager", Value = "DR" };
            yield return new SelectListItem { Text = "Supplier Manager", Value = "SR" };
           
        }
    }
}
