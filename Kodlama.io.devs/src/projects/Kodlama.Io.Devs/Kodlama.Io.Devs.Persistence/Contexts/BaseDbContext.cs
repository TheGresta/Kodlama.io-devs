using Core.Security.Entities;
using Kodlama.Io.Devs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kodlama.Io.Devs.Persistence.Contexts
{
    public class BaseDbContext : DbContext
    {
        protected IConfiguration Configuration { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GitHub> GitHubs { get; set; }
        public DbSet<LanguageTechnology> LanguageTechnologies { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //    base.OnConfiguring(
            //        optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SomeConnectionString")));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Language Model Creation
            modelBuilder.Entity<Language>(l =>
            {
                l.ToTable("Languages").HasKey(k => k.Id);
                l.Property(l => l.Id).HasColumnName("Id");
                l.Property(l => l.Name).HasColumnName("Name");
                l.HasMany(l => l.LanguageTechnologies);
            });

            Language[] languageEntitySeeds = { new(1, "Java"), new(2, "C#"), new(3, "JavaScript") };
            modelBuilder.Entity<Language>().HasData(languageEntitySeeds);
            #endregion

            #region LanguageTechnology Model Creation
            modelBuilder.Entity<LanguageTechnology>(l =>
            {
                l.ToTable("LanguageTechnologies").HasKey(k => k.Id);
                l.Property(l => l.Id).HasColumnName("Id");
                l.Property(l => l.Name).HasColumnName("Name");
                l.Property(l => l.LanguageId).HasColumnName("LanguageId");
                l.HasOne(l => l.Language);
            });

            LanguageTechnology[] languageTechnologyEntitySeeds = {
                new(1, 1, "Spring"), new(2, 1, "JSP"), new(3, 2, "WPF"),
                new(4, 2, "ASP.NET"), new(5, 3, "Vue"), new(6, 3, "React")};
            modelBuilder.Entity<LanguageTechnology>().HasData(languageTechnologyEntitySeeds);
            #endregion

            #region OperationClaim Model Creation
            modelBuilder.Entity<OperationClaim>(o =>
            {
                o.ToTable("OperationClaims").HasKey(k => k.Id);
                o.Property(o => o.Id).HasColumnName("Id");
                o.Property(o => o.Name).HasColumnName("Name");
            });

            OperationClaim[] operationClaimEntitySeeds = { new(1, "Admin"), new(2, "User"), new(3, "Visitor") };
            modelBuilder.Entity<OperationClaim>().HasData(operationClaimEntitySeeds);
            #endregion

            #region User Model Creation
            modelBuilder.Entity<User>(u =>
            {
                u.ToTable("Users").HasKey(k => k.Id);
                u.Property(u => u.Id).HasColumnName("Id");
                u.Property(u => u.FirstName).HasColumnName("FirstName");
                u.Property(u => u.LastName).HasColumnName("LastName");
                u.Property(u => u.Email).HasColumnName("Email");
                u.Property(u => u.PasswordHash).HasColumnName("PasswordHash");
                u.Property(u => u.PasswordSalt).HasColumnName("PasswordSalt");
                u.Property(u => u.Status).HasColumnName("Status").HasDefaultValue(true);
                u.Property(u => u.AuthenticatorType).HasColumnName("AuthenticatorType");
                u.HasMany(u => u.UserOperationClaims);
                u.HasMany(u => u.RefreshTokens);
            });
            #endregion

            #region UserOperationClaim Model Creation
            modelBuilder.Entity<UserOperationClaim>(u =>
            {
                u.ToTable("UserOperationClaims").HasKey(k => k.Id);
                u.Property(u => u.Id).HasColumnName("Id");
                u.Property(u => u.UserId).HasColumnName("UserId");
                u.Property(u => u.OperationClaimId).HasColumnName("OperationClaimId");
                u.HasOne(u => u.User);
                u.HasOne(u => u.OperationClaim);
            });
            #endregion

            #region GitHub Model Creation
            modelBuilder.Entity<GitHub>(g =>
            {
                g.ToTable("GitHubs").HasKey(k => k.Id);
                g.Property(g => g.Id).HasColumnName("Id");
                g.Property(g => g.Name).HasColumnName("Name");
                g.Property(g => g.UserId).HasColumnName("UserId");                
                g.HasOne(g => g.User);
            });
            #endregion
        }
    }
}
