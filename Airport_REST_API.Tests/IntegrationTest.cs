
using System.Linq;
using Airport_API_CodeTesting.Controllers;
using Airport_REST_API.DataAccess;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Services.Service;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Airport_REST_API.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        private TicketController controller;
        private ITicketService service;
        private IUnitOfWork uow;
        private AirportContext context;
        private IMapper mapper;

        [SetUp]
        public void StartUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AirportContext>();
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = AirportDB; Trusted_Connection = True; ConnectRetryCount = 0");
            context = new AirportContext(optionsBuilder.Options);
            uow = new UnitOfWork(context);       
            service = new TicketService(uow,mapper);
            controller = new TicketController(service);
        }
        [Test]
        public void Get_ItemById()
        {
        }

        [Test]
        public void AddItemTest()
        {

        }

        [Test]
        public void RemoveItemTest()
        {
            
        }

        [Test]
        public void UpdateTest()
        {

        }
    }
}
