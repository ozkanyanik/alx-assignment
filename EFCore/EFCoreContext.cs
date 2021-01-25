using EFCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class EFCoreContext : DbContext
    {
        public EFCoreContext(DbContextOptions<EFCoreContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PromoCode> PromoCodes { get; set; }
    }
}
