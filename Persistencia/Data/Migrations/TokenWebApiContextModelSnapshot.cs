﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistencia;

#nullable disable

namespace Persistencia.Data.Migrations
{
    [DbContext(typeof(TokenWebApiContext))]
    partial class TokenWebApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Dominio.Entities.HistorialRefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("int");

                    b.Property<bool>("Activo")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("activo");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("fechaCreacion");

                    b.Property<DateTime>("FechaExpiracion")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("fechaExpiracion");

                    b.Property<int>("IdUserFK")
                        .HasColumnType("int");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("longtext");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("token");

                    b.HasKey("Id");

                    b.HasIndex("IdUserFK");

                    b.ToTable("historialRefreshToken", (string)null);
                });

            modelBuilder.Entity("Dominio.Entities.Rol", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("Dominio.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Dominio.Entities.UserRol", b =>
                {
                    b.Property<int>("IdUserFK")
                        .HasColumnType("int");

                    b.Property<int>("IdRolFK")
                        .HasColumnType("int");

                    b.HasKey("IdUserFK", "IdRolFK");

                    b.HasIndex("IdRolFK");

                    b.ToTable("userRol", (string)null);
                });

            modelBuilder.Entity("Dominio.Entities.HistorialRefreshToken", b =>
                {
                    b.HasOne("Dominio.Entities.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("IdUserFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dominio.Entities.UserRol", b =>
                {
                    b.HasOne("Dominio.Entities.Rol", "Rol")
                        .WithMany("UserRols")
                        .HasForeignKey("IdRolFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Dominio.Entities.User", "User")
                        .WithMany("UserRols")
                        .HasForeignKey("IdUserFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Dominio.Entities.Rol", b =>
                {
                    b.Navigation("UserRols");
                });

            modelBuilder.Entity("Dominio.Entities.User", b =>
                {
                    b.Navigation("RefreshTokens");

                    b.Navigation("UserRols");
                });
#pragma warning restore 612, 618
        }
    }
}
