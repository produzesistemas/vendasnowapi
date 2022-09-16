using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().HasKey(c => c.Id);
            modelBuilder.Entity<PaymentCondition>().HasKey(c => c.Id);
            modelBuilder.Entity<Sale>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleProduct>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleService>().HasKey(c => c.Id);
            modelBuilder.Entity<Account>().HasKey(c => c.Id);
            modelBuilder.Entity<Sale>().HasMany(c => c.SaleProducts);
            modelBuilder.Entity<Sale>().HasMany(c => c.SaleServices);
            modelBuilder.Entity<Sale>().HasMany(c => c.Accounts);
            modelBuilder.Entity<Product>().HasMany(c => c.SaleProducts)
                .WithOne(b => b.Product)
                .HasForeignKey(c => c.ProductId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<PaymentCondition> PaymentCondition { get; set; }
        public DbSet<SaleProduct> SaleProduct { get; set; }
        public DbSet<SaleService> SaleService { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Sale> Sale { get; set; }

    }
}
