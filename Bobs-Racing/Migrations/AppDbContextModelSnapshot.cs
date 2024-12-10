﻿// <auto-generated />
using System;
using Bobs_Racing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bobs_Racing.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bobs_Racing.Models.Athlete", b =>
                {
                    b.Property<int>("AthleteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AthleteId"), 1L, 1);

                    b.Property<double>("FastestTime")
                        .HasColumnType("float");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SlowestTime")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AthleteId");

                    b.ToTable("Athletes");
                });

            modelBuilder.Entity("Bobs_Racing.Models.Bet", b =>
                {
                    b.Property<int>("BetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BetId"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<int>("PotentialPayout")
                        .HasColumnType("int");

                    b.Property<int>("RaceAthleteId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BetId");

                    b.HasIndex("RaceAthleteId");

                    b.HasIndex("UserId");

                    b.ToTable("Bets");
                });

            modelBuilder.Entity("Bobs_Racing.Models.Race", b =>
                {
                    b.Property<int>("RaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RaceId"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.HasKey("RaceId");

                    b.ToTable("Races");
                });

            modelBuilder.Entity("Bobs_Racing.Models.RaceAthlete", b =>
                {
                    b.Property<int>("RaceAthleteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RaceAthleteId"), 1L, 1);

                    b.Property<int?>("AnimalId")
                        .HasColumnType("int");

                    b.Property<int>("AthleteId")
                        .HasColumnType("int");

                    b.Property<int>("FinalPosition")
                        .HasColumnType("int");

                    b.Property<int>("RaceId")
                        .HasColumnType("int");

                    b.HasKey("RaceAthleteId");

                    b.HasIndex("AthleteId");

                    b.HasIndex("RaceId");

                    b.ToTable("RaceAthletes");
                });

            modelBuilder.Entity("Bobs_Racing.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<int>("Credits")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Profilename")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Credits = 0,
                            Password = "$2a$11$aTFwZVoUOOOKDLoo9TUF6eGu.1HLDUZG7csJt06/4awUkPSL78XdS",
                            Profilename = "Admin",
                            Role = "Admin",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("Bobs_Racing.Models.Bet", b =>
                {
                    b.HasOne("Bobs_Racing.Models.RaceAthlete", "RaceAthlete")
                        .WithMany("Bets")
                        .HasForeignKey("RaceAthleteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bobs_Racing.Models.User", "User")
                        .WithMany("Bets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RaceAthlete");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bobs_Racing.Models.RaceAthlete", b =>
                {
                    b.HasOne("Bobs_Racing.Models.Athlete", "Athlete")
                        .WithMany("RaceAthletes")
                        .HasForeignKey("AthleteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bobs_Racing.Models.Race", "Race")
                        .WithMany("RaceAthletes")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Athlete");

                    b.Navigation("Race");
                });

            modelBuilder.Entity("Bobs_Racing.Models.Athlete", b =>
                {
                    b.Navigation("RaceAthletes");
                });

            modelBuilder.Entity("Bobs_Racing.Models.Race", b =>
                {
                    b.Navigation("RaceAthletes");
                });

            modelBuilder.Entity("Bobs_Racing.Models.RaceAthlete", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("Bobs_Racing.Models.User", b =>
                {
                    b.Navigation("Bets");
                });
#pragma warning restore 612, 618
        }
    }
}
