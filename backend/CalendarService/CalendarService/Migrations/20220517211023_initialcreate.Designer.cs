﻿// <auto-generated />
using System;
using CalendarService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CalendarService.Migrations
{
    [DbContext(typeof(CalendarDbContext))]
    [Migration("20220517211023_initialcreate")]
    partial class initialcreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("CalendarService.Models.CalendarEmployeeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("CalenderGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EmployeeGuid")
                        .HasColumnType("uuid");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CalendarEmployee");
                });

            modelBuilder.Entity("CalendarService.Models.CalendarModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("CalenderGuid")
                        .HasColumnType("uuid");

                    b.Property<int>("EmployeeDone")
                        .HasColumnType("integer");

                    b.Property<Guid>("TaskDone")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Calendar");
                });

            modelBuilder.Entity("CalendarService.Models.CalendarTaskObjModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("CalenderGuid")
                        .HasColumnType("uuid");

                    b.Property<int>("EstimatedHours")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDatetime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("StartHour")
                        .HasColumnType("integer");

                    b.Property<Guid>("TaskGuid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("CalendarTaskObj");
                });
#pragma warning restore 612, 618
        }
    }
}
