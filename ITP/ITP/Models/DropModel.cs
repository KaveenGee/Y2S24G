using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITP.Models
{
    public enum Type
    {
        HR,IM,DM,SM,Owner
    }
    public class DropModel
    {
        public Type Type { get; set; }

        public static IEnumerable<SelectListItem> GetSelectItems()
        {
            yield return new SelectListItem { Text = "HR Manager", Value = "HR" };
            yield return new SelectListItem { Text = "Inventory Manager", Value = "IM" };
            yield return new SelectListItem { Text = "Delivery Manager", Value = "DR" };
            yield return new SelectListItem { Text = "Supplier Manager", Value = "SR" };
            yield return new SelectListItem { Text = "Owner", Value = "OW" };
        }
    }
}
