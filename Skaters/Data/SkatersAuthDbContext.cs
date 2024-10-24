using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Skaters.Domain.Model;
using Skaters.Models.CustomClass;
using Skaters.Models.Domain;
using System.Reflection.Emit;

namespace Skaters.Data
{
    public class SkatersAuthDbContext:IdentityDbContext<Applicationuser>
    {
        public SkatersAuthDbContext(DbContextOptions<SkatersAuthDbContext> options):base(options)
        {
            
        }
        public DbSet<Store> Store { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartProduct> CartProduct { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var sellerId = "13bf031b-0653-439d-9255-c7fcb26e7c1b";
            var customerId = "fcdc4017-5b0e-4038-ac3f-aaee580f4cc8";

            var roles = new List<IdentityRole>
            {
              new IdentityRole()
              {
                Id = sellerId,
                ConcurrencyStamp=sellerId,
                Name="Seller",
                NormalizedName="Reader".ToUpper()
              },

               new IdentityRole()
              {
                Id = customerId,
                ConcurrencyStamp=customerId,
                Name="Customer",
                NormalizedName="Customer".ToUpper()
              }
            };

            builder.Entity<IdentityRole>().HasData(roles);
          
        }
    }
}
