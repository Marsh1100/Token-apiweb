
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistencia;

public class TokenWebApiContext : DbContext
{
    public TokenWebApiContext(DbContextOptions<TokenWebApiContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Rol> Rols { get; set;}
    public DbSet<UserRol> UserRols { get; set; }
    public DbSet<HistorialRefreshToken> RefreshTokens { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
