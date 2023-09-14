using Microsoft.EntityFrameworkCore;
using Jwt.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Jwt.Helpers;


namespace Jwt.DBcontext
{
    public class JWTContext: IdentityDbContext<ApplicationUser,ApplicationRole,Guid>
    {
        public JWTContext(DbContextOptions<JWTContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Cliente { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cliente>().HasIndex(u => u.Email).IsUnique();

            string plainPassword = "yayon";
            string salt = Security.GenerateSalt();
            string hashedPassword = Security.GenerateHashedPassword(plainPassword, salt);

            modelBuilder.Entity<Cliente>().HasData(
                new Cliente
                {
                    Id = 1,
                    Name = "Teste",
                    Email = "test@gmail.com",
                    PasswordSalt = salt,
                    Password = hashedPassword
                });

        }
    }
}
