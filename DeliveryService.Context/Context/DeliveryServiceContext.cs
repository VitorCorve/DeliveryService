using DeliveryService.Context.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace DeliveryService.Context.Context
{
    public class DeliveryServiceContext : DbContext
    {
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured != true)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DeliveryServiceConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString);
            }

            base.OnConfiguring(optionsBuilder);
        }

        public DeliveryServiceContext(DbContextOptions options) : base(options)
        {
        }
    }
}
