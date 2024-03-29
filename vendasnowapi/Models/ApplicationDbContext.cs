﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
            modelBuilder.Entity<OpeningHours>().HasKey(c => c.Id);
            modelBuilder.Entity<Professional>().HasKey(c => c.Id);
            modelBuilder.Entity<ProfessionalService>().HasKey(c => c.Id);
            modelBuilder.Entity<Scheduling>().HasKey(c => c.Id);
            modelBuilder.Entity<Establishment>().HasKey(c => c.Id);
            modelBuilder.Entity<AspNetUsersEstablishment>().HasKey(c => c.Id);
            modelBuilder.Entity<SchedulingTrack>().HasKey(c => c.Id);
            modelBuilder.Entity<SchedulingEmail>().HasKey(c => c.Id);
            modelBuilder.Entity<SchedulingService>().HasKey(c => c.Id);
            modelBuilder.Entity<SchedulingReview>().HasKey(c => c.Id);

            modelBuilder.Entity<ProfessionalService>().HasOne(c => c.Professional);
            modelBuilder.Entity<Professional>().HasMany(c => c.ProfessionalService).WithOne(b => b.Professional).HasForeignKey(c => c.ProfessionalId);
            modelBuilder.Entity<Service>().HasMany(c => c.ProfessionalService).WithOne(b => b.Service).HasForeignKey(c => c.ServiceId);

            modelBuilder.Entity<Establishment>().HasMany(c => c.Subscriptions).WithOne(b => b.Establishment).HasForeignKey(c => c.EstablishmentId);
            modelBuilder.Entity<Establishment>().HasMany(c => c.Services).WithOne(b => b.Establishment).HasForeignKey(c => c.EstablishmentId);
            modelBuilder.Entity<Establishment>().HasMany(c => c.OpeningHours).WithOne(b => b.Establishment).HasForeignKey(c => c.EstablishmentId);
            modelBuilder.Entity<Establishment>().HasMany(c => c.Schedulings).WithOne(b => b.Establishment).HasForeignKey(c => c.EstablishmentId);


            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().HasKey(c => c.Id);
            modelBuilder.Entity<PaymentCondition>().HasKey(c => c.Id);
            modelBuilder.Entity<Sale>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleProduct>().HasKey(c => c.Id);
            modelBuilder.Entity<SaleService>().HasKey(c => c.Id);
            modelBuilder.Entity<Account>().HasKey(c => c.Id);





            modelBuilder.Entity<SaleProduct>().HasOne(c => c.Sale);
            modelBuilder.Entity<SaleService>().HasOne(c => c.Sale);

            modelBuilder.Entity<AspNetUsersEstablishment>().HasOne(c => c.Establishment);


            modelBuilder.Entity<Subscription>().HasOne(c => c.Plan);
            modelBuilder.Entity<Sale>().HasOne(c => c.PaymentCondition);
            modelBuilder.Entity<Sale>().HasOne(c => c.Client);

            //modelBuilder.Entity<Establishment>().HasMany(c => c.Subscriptions).WithOne(b => b.Establishment).HasForeignKey(c => c.EstablishmentId);

            modelBuilder.Entity<Sale>().HasMany(c => c.SaleProduct).WithOne(b => b.Sale).HasForeignKey(c => c.SaleId);
            modelBuilder.Entity<Sale>().HasMany(c => c.SaleService).WithOne(b => b.Sale).HasForeignKey(c => c.SaleId); 
            modelBuilder.Entity<Sale>().HasMany(c => c.Account).WithOne(b => b.Sale).HasForeignKey(c => c.SaleId);
            modelBuilder.Entity<Product>().HasMany(c => c.SaleProduct).WithOne(b => b.Product).HasForeignKey(c => c.ProductId);


            modelBuilder.Entity<Scheduling>().HasMany(c => c.SchedulingTrack).WithOne(b => b.Scheduling).HasForeignKey(c => c.SchedulingId);
            modelBuilder.Entity<Scheduling>().HasMany(c => c.SchedulingEmail).WithOne(b => b.Scheduling).HasForeignKey(c => c.SchedulingId);
            modelBuilder.Entity<Scheduling>().HasMany(c => c.SchedulingService).WithOne(b => b.Scheduling).HasForeignKey(c => c.SchedulingId);
            modelBuilder.Entity<Scheduling>().HasMany(c => c.SchedulingReview).WithOne(b => b.Scheduling).HasForeignKey(c => c.SchedulingId);
            modelBuilder.Entity<Scheduling>().HasOne(c => c.ApplicationUser);
            modelBuilder.Entity<Scheduling>().HasOne(c => c.Establishment);

            modelBuilder.Entity<StatusScheduling>().HasMany(c => c.SchedulesTrack)
                        .WithOne(b => b.StatusScheduling)
                        .HasForeignKey(c => c.StatusSchedulingId);
            modelBuilder.Entity<SchedulingTrack>().HasOne(c => c.StatusScheduling);

            modelBuilder.Entity<SchedulingService>().HasOne(c => c.Service);
            modelBuilder.Entity<SchedulingService>().HasOne(c => c.Scheduling);

            modelBuilder.Entity<SchedulingReview>().HasKey(c => c.Id);
            modelBuilder.Entity<SchedulingReview>().HasOne(c => c.ApplicationUser);

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
        public DbSet<Professional> Professional { get; set; }
        public DbSet<ProfessionalService> ProfessionalService { get; set; }
        public DbSet<OpeningHours> OpeningHours { get; set; }
        public DbSet<Scheduling> Scheduling { get; set; }
        public DbSet<SchedulingService> SchedulingService { get; set; }
        public DbSet<SchedulingTrack> SchedulingTrack { get; set; }
        public DbSet<SchedulingEmail> SchedulingEmail { get; set; }
        public DbSet<StatusScheduling> StatusScheduling { get; set; }
        public DbSet<SchedulingReview> SchedulingReview { get; set; }
    }
}
