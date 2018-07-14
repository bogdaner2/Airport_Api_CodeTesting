﻿// <auto-generated />
using System;
using Airport_REST_API.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Airport_REST_API.DataAccess.Migrations
{
    [DbContext(typeof(AirportContext))]
    [Migration("20180714063744_ChangeSomeListNamesToPlural")]
    partial class ChangeSomeListNamesToPlural
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Aircraft", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<TimeSpan>("Lifetime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("ReleseDate");

                    b.Property<int>("TypeId");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Aircrafts");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.AircraftType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CarryingCapacity");

                    b.Property<int>("CountOfSeats");

                    b.Property<string>("Model")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("AircraftTypes");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Crew", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PilotId");

                    b.HasKey("Id");

                    b.HasIndex("PilotId");

                    b.ToTable("Crews");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Departures", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AircraftId");

                    b.Property<int>("CrewId");

                    b.Property<DateTime>("DepartureTime");

                    b.Property<string>("Number")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AircraftId");

                    b.HasIndex("CrewId");

                    b.ToTable("Departures");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("ArrivalTime");

                    b.Property<DateTime>("DepartureTime");

                    b.Property<string>("Destination")
                        .IsRequired();

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<string>("PointOfDeparture")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Pilot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<int>("Experience");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Pilots");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Stewardess", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CrewId");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.HasIndex("CrewId");

                    b.ToTable("Stewardesses");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FlightId");

                    b.Property<string>("Number")
                        .IsRequired();

                    b.Property<double>("Price");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Aircraft", b =>
                {
                    b.HasOne("Airport_REST_API.DataAccess.Models.AircraftType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Crew", b =>
                {
                    b.HasOne("Airport_REST_API.DataAccess.Models.Pilot", "Pilot")
                        .WithMany()
                        .HasForeignKey("PilotId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Departures", b =>
                {
                    b.HasOne("Airport_REST_API.DataAccess.Models.Aircraft", "Aircraft")
                        .WithMany()
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Airport_REST_API.DataAccess.Models.Crew", "Crew")
                        .WithMany()
                        .HasForeignKey("CrewId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Stewardess", b =>
                {
                    b.HasOne("Airport_REST_API.DataAccess.Models.Crew")
                        .WithMany("Stewardesses")
                        .HasForeignKey("CrewId");
                });

            modelBuilder.Entity("Airport_REST_API.DataAccess.Models.Ticket", b =>
                {
                    b.HasOne("Airport_REST_API.DataAccess.Models.Flight")
                        .WithMany("Ticket")
                        .HasForeignKey("FlightId");
                });
#pragma warning restore 612, 618
        }
    }
}
