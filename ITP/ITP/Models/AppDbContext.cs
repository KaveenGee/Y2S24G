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
        public DbSet<OrderListClass> DeliveryList { get; set;}


        public DbSet<ItemModel> Item { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }

        public DbSet<Admin> Admin{ get; set; }


        public DbSet<EmployeeClass> Employee { get; set; }
        public DbSet<AttendeesModel> AttendeesTable { get; set; }

       
    }
}
