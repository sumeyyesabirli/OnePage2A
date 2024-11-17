using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnePage2AEntity.Entites;

namespace OnePage2ADataAccess.Contexts
{
    public class DbContext2A : DbContext
    {
        public DbContext2A(DbContextOptions<DbContext2A> options) : base(options)
        {

        }

        //private readonly IHttpContextAccessor _httpContextAccessor;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<References> References { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<AboutUs> AboutUs { get; set; }

        /*public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var entity = (BaseEntity)entityEntry.Entity;
                var currentUserName = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedByName = currentUserName;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();
        }*/
    }
}
