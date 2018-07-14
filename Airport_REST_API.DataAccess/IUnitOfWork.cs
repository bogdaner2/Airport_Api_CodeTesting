using Airport_REST_API.DataAccess.Repositories;

namespace Airport_REST_API.DataAccess
{
    public interface IUnitOfWork
    {
        TicketRepository Tickets { get;}
        AircraftRepository Aircrafts { get; }
        AircraftTypeRepository Types { get; }
        CrewRepository Crews { get; }
        StewardessRepository Stewardess { get; }
        PilotRepository Pilots { get; }
        FlightRepository Flights { get; }
        DepartureRepository Departures{ get;}
        void Save();
    }
}
