
using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<UserAddress> UserAddresses { get; set; }
    
  
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<UserAddress>()
    //    .HasOne(x => x.User)
    //    .WithOne(a => a.UserAddress)
    //    .HasForeignKey<ApplicationUser>(a => a.UserAddressId);
    //}

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    if (!optionsBuilder.IsConfigured)
    //    {
    //        optionsBuilder.UseLazyLoadingProxies();
    //    }
    //}

}
