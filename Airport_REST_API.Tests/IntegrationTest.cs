
using System.Linq;
using Airport_API_CodeTesting;
using Airport_API_CodeTesting.Controllers;
using Airport_REST_API.DataAccess;
using Airport_REST_API.DataAccess.Models;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Services.Service;
using Airport_REST_API.Shared.DTO;
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
        private IMapper _mapper;

        [SetUp]
        public void StartUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<AirportContext>();
            optionsBuilder.UseSqlServer(@"Server = (localdb)\mssqllocaldb; Database = AirportDB; Trusted_Connection = True; ConnectRetryCount = 0");
            context = new AirportContext(optionsBuilder.Options);
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TicketDTO, Ticket>()
                .ForMember(x => x.Id,opt => opt.Ignore());
                cfg.CreateMap<Ticket, TicketDTO>();
            }).CreateMapper();
            uow = new UnitOfWork(context);
            service = new TicketService(uow,_mapper);
            controller = new TicketController(service);
        }
        [Test]
        public void Get_ItemById()
        {
            var result = controller.Get(1);
        }

        [Test]
        public void AddItem_When_InputCorrect()
        {
            var correctItem = new TicketDTO {Number = "HRB100", Price = 1000};
            var step = controller.Post(correctItem);
        }

        [Test]
        public void CheckIgnoreID_When_CreateNewObjectFromDTO()
        {
            var correctItem = new TicketDTO { Id = 15, Number = "HRB100", Price = 1000 };
            var mapper = _mapper.Map<Ticket>(correctItem);
            Assert.IsFalse(correctItem.Id == mapper.Id);
        }

        [Test]
        public void RemoveItemTest()
        {
            
        }

        [Test]
        public void UpdateTest()
        {

        }

        [TearDown]
        public void Reset()
        {
            
        }
    }
}
