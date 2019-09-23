using Demo.GraphQLService.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.GraphQLService.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options) { }


        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }


        public override int SaveChanges()
        {
            this.ChangeTracker.DetectChanges();
            //var changedEntityNames = this.GetChangedEntityNames();

            var result = base.SaveChanges();
            //this.GetService<IEFCacheServiceProvider>().InvalidateCacheDependencies(changedEntityNames);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasAlternateKey(u => u.Title);
            base.OnModelCreating(modelBuilder);
        }
    }
}
