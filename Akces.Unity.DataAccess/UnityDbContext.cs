using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Akces.Unity.Models;
using Akces.Unity.Models.SaleChannels;
using System;

namespace Akces.Unity.DataAccess
{ 
    public static class UnityConnection
    {
        public static string ConnectionString { get; set; } = "Server=DESKTOP-KD0D5SK\\INSERTNEXO;Database=Unity.DataCenter;Trusted_Connection=True;";

        public static void EnsureCreated() 
        {
            var context = new UnityDbContext();

            try
            {
                var created = context.Database.EnsureCreated();

                if (!created)
                    context.Database.Migrate();
            }
            catch 
            {
            }
        }
    }

    public class UnityDbContext : DbContext
    {
        internal DbSet<Account> Accounts { get; set; }
        internal DbSet<Harmonogram> Harmonograms { get; set; }
        internal DbSet<TaskReport> TaskReports { get; set; }
        internal DbSet<UnityUser> UnityUsers { get; set; }
        internal DbSet<WorkerStatus> WorkerStatuses { get; set; }
        internal DbSet<AccountFunction> AccountFunctions { get; set; }
        internal DbSet<AccountFunctionType> AccountFunctionTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(UnityConnection.ConnectionString, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Harmonogram>().HasMany(x => x.Positions).WithOne().IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<HarmonogramPosition>().HasOne(x => x.Account).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<TaskReport>().HasMany(x => x.Positions).WithOne().IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UnityUser>().HasMany(x => x.Authorisations).WithOne().IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AccountFunction>().HasOne(x => x.AccountFunctionType).WithMany().IsRequired(true).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<AccountFunction>().HasOne(x => x.Account).WithMany().IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AccountFunctionType>().Ignore(x => x.DefaultScript);

            modelBuilder.Entity<Account>()
                .HasDiscriminator<string>("account_type");

            //Shoper
            modelBuilder.Entity<ShoperAccount>().HasOne(x => x.NexoConfiguration)
                .WithOne()
                .HasForeignKey<NexoConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoperAccount>().HasOne(x => x.ShoperConfiguration)
                .WithOne()
                .HasForeignKey<ShoperConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            //Baselinker
            modelBuilder.Entity<BaselinkerAccount>().HasOne(x => x.NexoConfiguration)
                .WithOne()
                .HasForeignKey<NexoConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BaselinkerAccount>().HasOne(x => x.BaselinkerConfiguration)
                .WithOne()
                .HasForeignKey<BaselinkerConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            //Shopgold
            modelBuilder.Entity<ShopgoldAccount>().HasOne(x => x.NexoConfiguration)
                .WithOne()
                .HasForeignKey<NexoConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShopgoldAccount>().HasOne(x => x.ShopgoldConfiguration)
                .WithOne()
                .HasForeignKey<ShopgoldConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            //Allegro
            modelBuilder.Entity<AllegroAccount>().HasOne(x => x.NexoConfiguration)
                .WithOne()
                .HasForeignKey<NexoConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AllegroAccount>().HasOne(x => x.AllegroConfiguration)
                .WithOne()
                .HasForeignKey<AllegroConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            //Olx
            modelBuilder.Entity<OlxAccount>().HasOne(x => x.NexoConfiguration)
                .WithOne()
                .HasForeignKey<NexoConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OlxAccount>().HasOne(x => x.OlxConfiguration)
                .WithOne()
                .HasForeignKey<OlxConfiguration>("AccountId")
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<AccountFunctionType>().HasData(
                    AccountFunctionType.MatchAssormentFunction,
                    AccountFunctionType.ConcludeProductSymbolFunction,
                    AccountFunctionType.CalculateOrderPositionQuantityFunction
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
