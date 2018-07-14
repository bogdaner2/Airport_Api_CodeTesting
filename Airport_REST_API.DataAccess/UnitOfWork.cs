using System;
using System.Collections.Generic;
using System.Text;
using Airport_REST_API.DataAccess.Repositories;

namespace Airport_REST_API.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private AirportContext db;
        private TicketRepository _ticketRepository;
        private AircraftRepository _aircraftRepository;
        private AircraftTypeRepository _typeRepository;
        private CrewRepository _crewRepository;
        private StewardessRepository _stewardessRepository;
        private PilotRepository _pilotRepository;
        private DepartureRepository _departureRepository;
        private FlightRepository _flightRepository;

        public UnitOfWork(AirportContext context)
        {
            db = context;
        }

        public TicketRepository Tickets
        {
            get
            {
                if (_ticketRepository == null)
                    _ticketRepository = new TicketRepository(db);
                return _ticketRepository;
            }
        }
        public AircraftRepository Aircrafts
        {
            get
            {
                if (_aircraftRepository == null)
                    _aircraftRepository = new AircraftRepository(db);
                return _aircraftRepository;
            }
        }
        public AircraftTypeRepository Types
        {
            get
            {
                if (_typeRepository == null)
                    _typeRepository = new AircraftTypeRepository(db);
                return _typeRepository;
            }
        }
        public CrewRepository Crews
        {
            get
            {
                if (_crewRepository == null)
                    _crewRepository = new CrewRepository(db);
                return _crewRepository;
            }
        }
        public StewardessRepository Stewardess
        {
            get
            {
                if (_stewardessRepository == null)
                    _stewardessRepository = new StewardessRepository(db);
                return _stewardessRepository;
            }
        }
        public PilotRepository Pilots
        {
            get
            {
                if (_pilotRepository == null)
                    _pilotRepository = new PilotRepository(db);
                return _pilotRepository;
            }
        }
        public FlightRepository Flights
        {
            get
            {
                if (_flightRepository == null)
                    _flightRepository = new FlightRepository(db);
                return _flightRepository;
            }
        }
        public DepartureRepository Departures
        {
            get
            {
                if (_departureRepository == null)
                    _departureRepository = new DepartureRepository(db);
                return _departureRepository;
            }
        }

        public void Save() => db.SaveChanges();
    }

}
