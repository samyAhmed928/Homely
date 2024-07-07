using Homely_modified_api.Models;

namespace Homely_modified_api.Data
{
    public class HomelyDbcontext : DbContext
    {
        public HomelyDbcontext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasMany(p => p.ImagesUrls)
                .WithOne(pi => pi.Property)
                .HasForeignKey(pi => pi.PropertyId);
        }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
