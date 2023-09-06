using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Data.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(p=> p.Id);
        builder.Property(p=>p.Id)
            .HasMaxLength(10);

        builder.Property(p=>p.Username)
            .HasColumnName("username")
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p=>p.Password)
            .HasColumnName("password")
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(p=>p.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasMaxLength(50);
        

        builder.HasMany(p=>p.Rols)
            .WithMany(p=>p.Users)
            .UsingEntity<UserRol>(

                j => j
                    .HasOne(p=>p.Rol)
                    .WithMany(p=>p.UserRols)
                    .HasForeignKey(p=>p.IdRolFK),

                j => j
                    .HasOne(p=>p.User)
                    .WithMany(p=>p.UserRols)
                    .HasForeignKey(p=>p.IdUserFK),

                j =>
                {   j.ToTable("userRol");
                    j.HasKey(t => new {t.IdUserFK, t.IdRolFK});
                }
            );

            
    }
}
