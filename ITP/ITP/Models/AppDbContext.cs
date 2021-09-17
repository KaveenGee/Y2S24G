using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace ITP.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Customer> CustomerInfo { get; set; }
        public DbSet<supplierlog> Supplierlog { get; set; }

        public DbSet<handlesupplierClass> handlesuppliers { get; set; }
        public DbSet<excessfaultyclass> ExcessFaulty { get; set; }
    
    }
}
