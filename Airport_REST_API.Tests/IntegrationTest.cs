
using System.Collections.Generic;
using System.Linq;
using Airport_API_CodeTesting;
using Airport_API_CodeTesting.Controllers;
using Airport_REST_API.DataAccess;
using Airport_REST_API.DataAccess.Models;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Services.Service;
using Airport_REST_API.Shared.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
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
        private ICrewService crewSer;
        private CrewController crewCon;
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
                cfg.CreateMap<CrewDTO, Crew>()
                    .ForMember(i => i.Id, opt => opt.Ignore())
                    .ForMember(i => i.Stewardesses, opt => opt.Ignore())
                    .ForMember(i => i.Pilot, opt => opt.Ignore());
                cfg.CreateMap<Crew, CrewDTO>()
                    .ForMember(i => i.StewardessesId, opt => opt.MapFrom(m => m.Stewardesses.Select(s => s.Id)))
                    .ForMember(i => i.PilotId, opt => opt.MapFrom(m => m.Pilot.Id));
                cfg.CreateMap<Ticket, TicketDTO>();
            }).CreateMapper();
            uow = new UnitOfWork(context);
            service = new TicketService(uow,_mapper);
            crewSer = new CrewService(uow, _mapper);
            crewCon = new CrewController(crewSer);
            controller = new TicketController(service);
        }
        [Test]
        public void Get_ReturnOkStatusCode()
        {
            //Act
            var result = controller.GetAll() as OkObjectResult;
            //Assert
            Assert.True(result.StatusCode == 200);
        }      
        [Test]
        public void Get_Should_ReturnObject_When_IdIsCorrect()
        {
            //Act
            var result = controller.Get(2) as OkObjectResult;
            //Assert
            Assert.True(result.Value != null);
        }
        [Test]
        public void Get_Should_ReturnObject_When_IdIsNegative()
        {
            //Act
            var result = controller.Get(-1) as OkObjectResult;
            //Assert
            Assert.True(result.Value == null);
        }
        [Test]
        public void AddItem_When_InputCorrect_Than_CountIncrease()
        {
            //Arrange
            var correctItem = new TicketDTO {Number = "TestAdd2", Price = 1000};
            var initialCount = context.Tickets.ToList().Count;
            //Act
            controller.Post(correctItem);
            var afterCount = context.Tickets.ToList().Count;
            //Assert
            Assert.IsFalse(initialCount == afterCount);
            //Reset
            controller.Delete(context.Tickets.Last().Id);
        }
        [Test]
        public void AddItem_ReturnOKStatus_When_ItemAdded()
        {
            //Arrange
            var correctItem = new TicketDTO { Number = "TestAdd1", Price = 1500 };
            //Act
            var result = controller.Post(correctItem) as StatusCodeResult;
            //Assert
            Assert.AreEqual(result.StatusCode,200);
            //Reset
            controller.Delete(context.Tickets.Last().Id);
        }
        [Test]
        public void CheckIgnoreID_When_CreateNewObjectFromDTO()
        {
            //Arrange
            var correctItem = new TicketDTO { Id = 15, Number = "HRB100", Price = 1000 };
            //Act
            var mapper = _mapper.Map<Ticket>(correctItem);
            //Assert
            Assert.IsFalse(correctItem.Id == mapper.Id);
        }
        [Test]
        public void Post_Return500Status_WhenModelIsInvalid()
        {
            var result = controller.Post(new TicketDTO()) as StatusCodeResult;
            //Assert
            Assert.True(result.StatusCode == 500);
        }
        [Test]
        public void Remove_Return_OkStatusCode()
        {
            controller.Post(new TicketDTO { Number = "RemoveTest", Price = 100 });
            var lastIndex = context.Tickets.Last().Id;
            //Act 
            var result = controller.Delete(lastIndex) as StatusCodeResult;
            //Assert
            Assert.True(result.StatusCode == 200);
            //Reset
            controller.Delete(lastIndex);
        }
        [Test]
        public void Remove_DecreaseCountOfSet()
        {
            //Arrange
            controller.Post(new TicketDTO {Number = "RemoveTest", Price = 100});
            var initialCount = context.Tickets.ToList().Count;
            var lastIndex = context.Tickets.Last().Id;
            //Act
            controller.Delete(lastIndex);
            var afterCount = context.Tickets.ToList().Count;
            //Assert
            Assert.IsFalse(initialCount == afterCount);
        }
        [Test]
        public void Add_Crew_With_NestedListOfStewardess_ReturnOk()
        {
            //Arrange
            var correctItem = new CrewDTO { PilotId = 1,StewardessesId = new List<int> { 1,2}};
            //Act
            var result = crewCon.Post(correctItem) as StatusCodeResult;
            //Assert
            Assert.AreEqual(result.StatusCode, 200);
            //Reset
            crewCon.Delete(context.Crews.Last().Id);
        }
        [Test]
        public void Remove_Return500_When_CrewWithoutStewardesses()
        {
            //Arrange
            var correctItem = new CrewDTO { PilotId = 1 };
            //Act
            var result = crewCon.Put(1,correctItem) as StatusCodeResult;
            //Assert
            Assert.AreEqual(result.StatusCode, 500);
        }


        [Test]
        public void TestInnerListAdd()
        {

        }
        [Test]
        public void TestInnerListRemove()
        {

        }


        [Test]
        public void Update_Return_OkStatusCode()
        {
            var ticket = new TicketDTO {Id = 1 ,Number = "Test", Price = 1000};
            var result = controller.Put(3,ticket);
            Assert.AreEqual(new StatusCodeResult(200).StatusCode,((StatusCodeResult)result).StatusCode);
        }
        [Test]
        public void Update_ChangedObject_Should_NotEqualInitialObject()
        {
            //Arrange
            var temp = uow.Tickets.Get(2);
            var initial = new { temp.Id, temp.Number, temp.Price};
            var ticket = new TicketDTO { Number = "Test", Price = 1000 };
            //Act
            controller.Put(2, ticket);
            var current = uow.Tickets.Get(2);
            //Assert
            Assert.IsFalse(current.Number == initial.Number);
            //Reset
            controller.Put(2, new TicketDTO { Number = initial.Number, Price = initial.Price});
        }
        [Test]
        public void ReturnFalse_When_UpdateObjectWithNegativeId()
        {
            //Act
            var result = service.Update(-1, It.IsAny<TicketDTO>());
            //Assert
            Assert.IsFalse(result);    
        }

        [TearDown]
        public void Reset()
        {
            controller = null;
            uow = null;
            _mapper = null;
            service = null;
        }
    }
}
