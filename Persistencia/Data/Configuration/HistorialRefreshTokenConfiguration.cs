using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class HistorialRefreshTokenConfiguration : IEntityTypeConfiguration<HistorialRefreshToken>
{
    public void Configure(EntityTypeBuilder<HistorialRefreshToken>builder)
    {
        builder.ToTable("historialRefreshToken");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasMaxLength(10);
        
        builder.Property(o => o.Token)
            .HasColumnName("token")
            .IsRequired();
        builder.Property(o => o.FechaCreacion)
            .HasColumnName("fechaCreacion")
            .IsRequired();   
        builder.Property(o => o.FechaExpiracion)
            .HasColumnName("fechaExpiracion")
            .IsRequired(); 
        
        builder.Property(o => o.Activo)
            .HasColumnName("activo")
            .IsRequired();
        
        builder.HasOne(o => o.User)
            .WithMany(o => o.RefreshTokens)
            .HasForeignKey(o => o.IdUserFK);
            
    }   
}
