﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using hhSalonAPI.Domain.Concrete;

#nullable disable

namespace hhSalon.Domain.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230430191408_chats")]
    partial class chats
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("hhSalon.Domain.Entities.Attendance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("client_id");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasColumnName("date");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.Property<string>("IsPaid")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("paid");

                    b.Property<string>("IsRendered")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("rendered");

                    b.Property<double>("Price")
                        .HasColumnType("double")
                        .HasColumnName("price");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int")
                        .HasColumnName("service_id");

                    b.Property<TimeSpan?>("Time")
                        .HasColumnType("time")
                        .HasColumnName("time");

                    b.Property<string>("WorkerId")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasColumnName("worker_id");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("ClientId", "Date", "ServiceId")
                        .IsUnique();

                    b.HasIndex("WorkerId", "Date", "ServiceId")
                        .IsUnique();

                    b.ToTable("Attendances");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Chat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Content")
                        .HasColumnType("longtext")
                        .HasColumnName("content");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date");

                    b.Property<string>("FromId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("from_id");

                    b.Property<bool>("IsRead")
                        .HasColumnType("tinyint(1)")
                        .HasColumnName("is_read");

                    b.Property<string>("ToId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("to_id");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.HasIndex("ToId");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.GroupOfServices", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("ImgUrl")
                        .HasColumnType("longtext")
                        .HasColumnName("img_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("groups_of_services");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Schedule", b =>
                {
                    b.Property<string>("WorkerId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("worker_id");

                    b.Property<string>("Day")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("day");

                    b.Property<TimeSpan>("End")
                        .HasColumnType("time")
                        .HasColumnName("end");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("time")
                        .HasColumnName("start");

                    b.HasKey("WorkerId", "Day");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double")
                        .HasColumnName("price");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Services");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.ServiceGroup", b =>
                {
                    b.Property<int>("ServiceId")
                        .HasColumnType("int")
                        .HasColumnName("service_id");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.HasKey("ServiceId", "GroupId");

                    b.HasIndex("GroupId");

                    b.HasIndex("ServiceId")
                        .IsUnique();

                    b.ToTable("Services_Groups");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .HasColumnType("longtext")
                        .HasColumnName("password");

                    b.Property<string>("Role")
                        .HasColumnType("longtext")
                        .HasColumnName("role");

                    b.Property<string>("Token")
                        .HasColumnType("longtext")
                        .HasColumnName("token");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Worker", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("address");

                    b.Property<string>("Gender")
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)")
                        .HasColumnName("gender");

                    b.HasKey("Id");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.WorkerGroup", b =>
                {
                    b.Property<string>("WorkerId")
                        .HasColumnType("varchar(255)")
                        .HasColumnName("worker_id");

                    b.Property<int>("GroupId")
                        .HasColumnType("int")
                        .HasColumnName("group_id");

                    b.HasKey("WorkerId", "GroupId");

                    b.HasIndex("GroupId");

                    b.ToTable("Workers_Groups");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Attendance", b =>
                {
                    b.HasOne("hhSalon.Domain.Entities.User", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hhSalon.Domain.Entities.GroupOfServices", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("hhSalon.Domain.Entities.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId");

                    b.HasOne("hhSalon.Domain.Entities.Worker", "Worker")
                        .WithMany()
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Group");

                    b.Navigation("Service");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Chat", b =>
                {
                    b.HasOne("hhSalon.Domain.Entities.User", "FromUser")
                        .WithMany()
                        .HasForeignKey("FromId");

                    b.HasOne("hhSalon.Domain.Entities.User", "ToUser")
                        .WithMany()
                        .HasForeignKey("ToId");

                    b.Navigation("FromUser");

                    b.Navigation("ToUser");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Schedule", b =>
                {
                    b.HasOne("hhSalon.Domain.Entities.Worker", "Worker")
                        .WithMany("Schedules")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.ServiceGroup", b =>
                {
                    b.HasOne("hhSalon.Domain.Entities.GroupOfServices", "Group")
                        .WithMany("Services_Groups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hhSalon.Domain.Entities.Service", "Service")
                        .WithOne("ServiceGroup")
                        .HasForeignKey("hhSalon.Domain.Entities.ServiceGroup", "ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Worker", b =>
                {
                    b.HasOne("hhSalon.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.WorkerGroup", b =>
                {
                    b.HasOne("hhSalon.Domain.Entities.GroupOfServices", "Group")
                        .WithMany("Workers_Groups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hhSalon.Domain.Entities.Worker", "Worker")
                        .WithMany("Workers_Groups")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.GroupOfServices", b =>
                {
                    b.Navigation("Services_Groups");

                    b.Navigation("Workers_Groups");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Service", b =>
                {
                    b.Navigation("ServiceGroup");
                });

            modelBuilder.Entity("hhSalon.Domain.Entities.Worker", b =>
                {
                    b.Navigation("Schedules");

                    b.Navigation("Workers_Groups");
                });
#pragma warning restore 612, 618
        }
    }
}