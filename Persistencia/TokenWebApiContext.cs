
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistencia;

public class TokenWebApiContext : DbContext
{
    public TokenWebApiContext(DbContextOptions<TokenWebApiContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
