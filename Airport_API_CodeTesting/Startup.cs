﻿using System;
using System.Linq;
using Airport_REST_API.DataAccess;
using Airport_REST_API.DataAccess.Models;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Services.Service;
using Airport_REST_API.Shared.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airport_API_CodeTesting
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork,UnitOfWork>();
            services.AddTransient<ITicketService,TicketService>();
            services.AddTransient<IAircraftService, AircraftService>();
            services.AddTransient<IFlightService, FlightService>();
            services.AddTransient<IAircraftTypeService, AircraftTypeService>();
            services.AddTransient<ICrewService, CrewService>();
            services.AddTransient<IStewardessService, StewardessService>();
            services.AddTransient<IPilotService, PilotService>();
            services.AddTransient<IDepartureService,DepartureService>();
            services.AddSingleton<IUnitOfWork,UnitOfWork>();
            services.AddScoped<UnitOfWork>();
            var mapper = MapperConfiguration().CreateMapper();
            services.AddSingleton(_ => mapper);
            var connection = @"Server=(localdb)\mssqllocaldb;Database=AirportDB;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<AirportContext>(options => options.UseSqlServer(connection));
            services.AddTransient<DataInitializer>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataInitializer Seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            Seeder.Seed().Wait();
        }

        public MapperConfiguration MapperConfiguration()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //Into Model
                cfg.CreateMap<TicketDTO,Ticket>()
                    .ForMember(i => i.Id, opt => opt.Ignore());
                cfg.CreateMap<AircraftDTO, Aircraft>()
                    .ForMember(i => i.Id, opt => opt.Ignore())
                    .ForMember(i => i.Type, opt => opt.Ignore())
                    .ForMember(i => i.Lifetime,opt => opt.MapFrom(m => DateTime.Now - DateTime.Parse(m.Lifetime)));
                cfg.CreateMap<PilotDTO, Pilot>()
                    .ForMember(i => i.Id, opt => opt.Ignore());
                cfg.CreateMap<StewardessDTO, Stewardess>()
                    .ForMember(i => i.Id, opt => opt.Ignore());
                cfg.CreateMap<AircraftTypeDTO, AircraftType>()
                    .ForMember(i => i.Id, opt => opt.Ignore());
                cfg.CreateMap<FlightDTO, Flight>()
                    .ForMember(i => i.Id, opt => opt.Ignore())
                    .ForMember(i => i.ArrivalTime, opt => opt.MapFrom(m => DateTime.Parse(m.ArrivelTime)));
                cfg.CreateMap<DeparturesDTO, Departures>()
                    .ForMember(i => i.Id, opt => opt.Ignore())
                    .ForMember(i => i.Aircraft, opt => opt.Ignore())
                    .ForMember(i => i.Crew, opt => opt.Ignore())
                    .ForMember(i => i.DepartureTime, opt => opt.MapFrom(m => DateTime.Parse(m.DepartureTime)));
                cfg.CreateMap<CrewDTO, Crew>()
                    .ForMember(i => i.Id, opt => opt.Ignore())
                    .ForMember(i => i.Stewardesses, opt => opt.Ignore())
                    .ForMember(i => i.Pilot, opt => opt.Ignore());
                //Into DTO
                cfg.CreateMap<Ticket,TicketDTO>();
                cfg.CreateMap<Aircraft, AircraftDTO>()
                    .ForMember(i => i.ReleseDate, opt => opt.MapFrom(m => m.ReleseDate.ToLongDateString()))
                    .ForMember(i => i.Lifetime, opt => opt.MapFrom(m => m.Lifetime.TotalDays));
                cfg.CreateMap<Pilot, PilotDTO>();
                cfg.CreateMap<Stewardess, StewardessDTO>();
                cfg.CreateMap<AircraftType, AircraftTypeDTO>();
                cfg.CreateMap<Flight, FlightDTO>()
                    .ForMember(i => i.ArrivelTime, opt => opt.MapFrom(m => m.ArrivalTime.ToLongDateString()))
                    .ForMember(i => i.DepartureTime, opt => opt.MapFrom(m => m.DepartureTime.ToLongDateString()))
                    .ForMember(i => i.TicketsId, opt => opt.MapFrom(m => m.Ticket.Select(s => s.Id)));
                cfg.CreateMap<Departures, DeparturesDTO>()
                    .ForMember(i => i.AircraftId, opt => opt.MapFrom(m => m.Aircraft.Id))
                    .ForMember(i => i.CrewId, opt => opt.MapFrom(m => m.Crew.Id))
                    .ForMember(i => i.DepartureTime, opt => opt.MapFrom(m => m.DepartureTime.ToLongDateString()));
                cfg.CreateMap<Crew, CrewDTO>()
                    .ForMember(i => i.StewardessesId, opt => opt.MapFrom(m => m.Stewardesses.Select(s => s.Id)))
                    .ForMember(i => i.PilotId, opt => opt.MapFrom(m => m.Pilot.Id));

            });
            return config;
        }
    }
}
