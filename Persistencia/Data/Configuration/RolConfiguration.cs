using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol>builder)
    {
        builder.ToTable("Rol");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasMaxLength(10);
        
        builder.Property(o => o.Nombre)
            .IsRequired()
            .HasMaxLength(50);
    }   
}
