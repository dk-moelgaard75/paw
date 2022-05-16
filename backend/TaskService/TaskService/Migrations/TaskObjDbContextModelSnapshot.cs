﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TaskService.Data;

namespace TaskService.Migrations
{
    [DbContext(typeof(TaskObjDbContext))]
    partial class TaskObjDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.16")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("TaskService.Models.TaskObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("CustomerGuid")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("EstimatedHours")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("StartHour")
                        .HasColumnType("integer");

                    b.Property<Guid>("TaskGuid")
                        .HasColumnType("uuid");

                    b.Property<string>("TaskName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TaskObjs");
                });

            modelBuilder.Entity("TaskService.Models.TaskXEmployee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<Guid>("EmployeeGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TaskGuid")
                        .HasColumnType("uuid");

                    b.Property<int?>("TaskObjectId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TaskObjectId");

                    b.ToTable("TaskXEmployee");
                });

            modelBuilder.Entity("TaskService.Models.TaskXEmployee", b =>
                {
                    b.HasOne("TaskService.Models.TaskObject", null)
                        .WithMany("Employees")
                        .HasForeignKey("TaskObjectId");
                });

            modelBuilder.Entity("TaskService.Models.TaskObject", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
