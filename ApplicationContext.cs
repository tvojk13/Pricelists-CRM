using Microsoft.EntityFrameworkCore;
using Price_lists_CRM.Models;

namespace Price_lists_CRM
{
    public class ApplicationContext : DbContext
    {
        public DbSet<PricelistModel> Pricelists { get; set; } = null!;
        public DbSet<ColumnModel> Columns { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PricelistModel>().HasData(
                new PricelistModel { Id = 1, Title = "asd" },
                new PricelistModel { Id = 2, Title = "qwe" },
                new PricelistModel { Id = 3, Title = "zxc" }
            );

            modelBuilder.Entity<ColumnModel>().HasData(
                new ColumnModel { Id = 1, Title = "Value 1", Type = "Text", PricelistId = 1, Value = "Sample Value 1" },
                new ColumnModel { Id = 2, Title = "Value 2", Type = "Number", PricelistId = 1, Value = "123" },
                new ColumnModel { Id = 3, Title = "Value 3", Type = "Text", PricelistId = 1, Value = "Sample Value 2" },
                new ColumnModel { Id = 4, Title = "Value 4", Type = "Date", PricelistId = 1, Value = "2022-10-01" }
            );

            modelBuilder.Entity<PricelistModel>()
                .HasMany(p => p.Columns)
                .WithOne(c => c.Pricelist)
                .HasForeignKey(c => c.PricelistId);
        }
    }
}
