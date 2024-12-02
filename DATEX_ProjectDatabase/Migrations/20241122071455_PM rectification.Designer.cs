﻿// <auto-generated />
using System;
using DATEX_ProjectDatabase.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241122071455_PM rectification")]
    partial class PMrectification
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastLoginTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("EmployeeId");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.ProjectManagers", b =>
                {
                    b.Property<int>("ProjectManagerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectManagerId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("ProjectManagerId");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectManagers");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.VOCAnalysis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Communication")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerFocus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EngageService")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Knowledge")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlanningAndControl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Quality")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("VocAnalyses");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"));

                    b.Property<string>("CloudUsed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DU")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DUHead")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DatabaseUsed")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Domain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FeedbackStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ForecastedEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("MailStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumberOfResources")
                        .HasColumnType("int");

                    b.Property<DateTime?>("PMInitiateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PMMails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectDurationInDays")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("DATEDIFF(day, ProjectStartDate, ProjectEndDate)");

                    b.Property<int>("ProjectDurationInMonths")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("int")
                        .HasComputedColumnSql("DATEDIFF(month, ProjectStartDate, ProjectEndDate)");

                    b.Property<DateTime?>("ProjectEndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectManager")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ProjectStartDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ProjectType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SQA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Technology")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("VOCEligibilityDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("VOCFeedbackReceivedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VocRemarks")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.Employee", b =>
                {
                    b.HasOne("DATEX_ProjectDatabase.Model.Role", "Role")
                        .WithMany("Employees")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.ProjectManagers", b =>
                {
                    b.HasOne("DATEX_ProjectDatabase.Models.Project", "Project")
                        .WithMany("ProjectManagers")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Model.Role", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DATEX_ProjectDatabase.Models.Project", b =>
                {
                    b.Navigation("ProjectManagers");
                });
#pragma warning restore 612, 618
        }
    }
}
