using Core.Security.Entities;
using Core.Security.Hashing;
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
        public DbSet<LanguageTechnology> LanguageTechnologies { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public BaseDbContext(DbContextOptions dbContextOptions, IConfiguration configuration) : base(dbContextOptions)
        {
            Configuration = configuration;
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
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
            #endregion

            #region OperationClaim Model Creation
            modelBuilder.Entity<OperationClaim>(o =>
            {
                o.ToTable("OperationClaims").HasKey(k => k.Id);
                o.Property(o => o.Id).HasColumnName("Id");
                o.Property(o => o.Name).HasColumnName("Name");
            });
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

            #region RefreshToken Model Creation
            modelBuilder.Entity<RefreshToken>(u =>
            {
                u.ToTable("RefreshTokens").HasKey(k => k.Id);
                u.Property(u => u.Id).HasColumnName("Id");
                u.Property(u => u.UserId).HasColumnName("UserId");
                u.Property(u => u.Token).HasColumnName("Token");
                u.Property(u => u.Expires).HasColumnName("ExpiresTime");
                u.Property(u => u.Created).HasColumnName("CreatedTime");
                u.Property(u => u.CreatedByIp).HasColumnName("CreatedByIp");
                u.Property(u => u.Revoked).HasColumnName("RevokedTime");
                u.Property(u => u.RevokedByIp).HasColumnName("RevokedByIp");
                u.Property(u => u.ReplacedByToken).HasColumnName("ReplacedByToken");
                u.Property(u => u.ReasonRevoked).HasColumnName("ReasonRevoked");
                u.HasOne(u => u.User);                
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

            #region Developer Model Creation
            modelBuilder.Entity<Developer>(g =>
            {
                g.ToTable("Developers");
                g.Property(g => g.GitHub).HasColumnName("GitHub");
            });
            #endregion


            #region Language Entity Seeds
            Language[] languageEntitySeeds = { new(1, "C#"), new(2, "Java"), new(3, "JavaScript") };
            modelBuilder.Entity<Language>().HasData(languageEntitySeeds);
            #endregion

            #region LanguageTechnology Entity Seeds
            LanguageTechnology[] languageTechnologyEntitySeeds = { new(1, 1, "WPF"), new(2, 1, "ASP.NET"),
                                                                   new(3, 2, "Spring"), new(4, 2, "JSP"),
                                                                   new(5, 3, "Vue"), new(6, 3, "React") };
            modelBuilder.Entity<LanguageTechnology>().HasData(languageTechnologyEntitySeeds);
            #endregion

            #region OperationClaim Entity Seeds
            OperationClaim[] operationClaimEntitySeeds = { new(1, "Admin"), new(2, "User"), 
                                                           new(3, "Visitor"), new(4, "Developer") };
            modelBuilder.Entity<OperationClaim>().HasData(operationClaimEntitySeeds);
            #endregion

            #region User Entity Seeds
            User[] userEntitySeeds = { GetAdminUser() };
            modelBuilder.Entity<User>().HasData(userEntitySeeds);
            #endregion

            #region UserOperationClaim Entity Seeds
            UserOperationClaim[] userOperationClaimEntitySeeds = { new(1, 1, 1) };
            modelBuilder.Entity<UserOperationClaim>().HasData(userOperationClaimEntitySeeds);
            #endregion
        }

        private User GetAdminUser()
        {
            byte[] passwordHash, passwordSalt;
            string password = configuration().GetSection("AdminUser:Password").Value;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);

            return new()
            {
                Id = 1,
                FirstName = configuration().GetSection("AdminUser:FirstName").Value,
                LastName = configuration().GetSection("AdminUser:LastName").Value,
                Email = configuration().GetSection("AdminUser:Email").Value,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true,
                AuthenticatorType = Core.Security.Enums.AuthenticatorType.Email,
            };
        }

        private IConfigurationRoot configuration()
        {
            return new ConfigurationManager()
                .AddJsonFile("appsettings.json").Build();
        }
    }
}
