﻿// <auto-generated />
using System;
using CrossSolar.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CrossSolar.Migrations
{
    [DbContext(typeof(CrossSolarDbContext))]
    [Migration("20180824133712_ii")]
    partial class ii
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.0-rc1-32029")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CrossSolar.Domain.OneHourElectricity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("KiloWatt");

                    b.Property<int>("PanelId");

                    b.HasKey("Id");

                    b.HasIndex("PanelId");

                    b.ToTable("OneHourElectricitys");
                });

            modelBuilder.Entity("CrossSolar.Domain.Panel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Serial")
                        .IsRequired()
                        .HasMaxLength(16);

                    b.HasKey("Id");

                    b.ToTable("Panels");
                });

            modelBuilder.Entity("CrossSolar.Domain.OneHourElectricity", b =>
                {
                    b.HasOne("CrossSolar.Domain.Panel", "Panel")
                        .WithMany()
                        .HasForeignKey("PanelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
