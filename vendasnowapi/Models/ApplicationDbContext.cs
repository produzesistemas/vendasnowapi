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
            modelBuilder.Entity<Plan>().HasKey(c => c.Id);
            modelBuilder.Entity<Subscription>().HasKey(c => c.Id);
            modelBuilder.Entity<Service>().HasKey(c => c.Id);

            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().HasKey(c => c.Id);
            modelBuilder.Entity<PaymentCondition>().HasKey(c => c.Id);
            modelBuilder.Entity<Sale>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleProduct>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleService>().HasKey(c => c.Id);
            modelBuilder.Entity<Account>().HasKey(c => c.Id);
            modelBuilder.Entity<Establishment>().HasKey(c => c.Id);
            modelBuilder.Entity<AspNetUsersEstablishment>().HasKey(c => c.Id);

            modelBuilder.Entity<SaleProduct>().HasOne(c => c.Sale);
            modelBuilder.Entity<SaleService>().HasOne(c => c.Sale);
            modelBuilder.Entity<AspNetUsersEstablishment>().HasOne(c => c.Establishment);
            modelBuilder.Entity<AspNetUsersEstablishment>().HasOne(c => c.ApplicationUser);

            modelBuilder.Entity<Subscription>().HasOne(c => c.Plan);
            modelBuilder.Entity<Sale>().HasOne(c => c.PaymentCondition);
            modelBuilder.Entity<Sale>().HasOne(c => c.Client);

            modelBuilder.Entity<Establishment>().HasMany(c => c.Subscriptions).WithOne(b => b.Establishment).HasForeignKey(c => c.EstablishmentId);

            modelBuilder.Entity<Sale>().HasMany(c => c.SaleProduct).WithOne(b => b.Sale).HasForeignKey(c => c.SaleId);
            modelBuilder.Entity<Sale>().HasMany(c => c.SaleService).WithOne(b => b.Sale).HasForeignKey(c => c.SaleId); 
            modelBuilder.Entity<Sale>().HasMany(c => c.Account).WithOne(b => b.Sale).HasForeignKey(c => c.SaleId);
            modelBuilder.Entity<Product>().HasMany(c => c.SaleProduct).WithOne(b => b.Product).HasForeignKey(c => c.ProductId);

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
        public DbSet<Plan> Plan { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Establishment> Establishment { get; set; }
        public DbSet<AspNetUsersEstablishment> AspNetUsersEstablishment { get; set; }
        public DbSet<Service> Service { get; set; }

    }
}
