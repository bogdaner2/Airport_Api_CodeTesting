
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
        public void Get_Should_ReturnObject_When_IdIsCorrect()
        {
            //Act
            var result = controller.Get(1) as OkObjectResult;
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
            var correctItem = new TicketDTO {Number = "TestAdd2", Price = 1000};
            var initialCount = context.Tickets.ToList().Count;
            var step = controller.Post(correctItem);
            var afterCount = context.Tickets.ToList().Count;
            Assert.IsFalse(initialCount == afterCount);
        }
        [Test]
        public void AddItem_ReturnOKStatus_When_ItemAdded()
        {
            var correctItem = new TicketDTO { Number = "TestAdd1", Price = 1500 };
            var result = controller.Post(correctItem) as StatusCodeResult;
            Assert.AreEqual(result.StatusCode,200);
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
        public void Remove_Return_OkStatusCode()
        {
            
        }
        [Test]
        public void Remove_DecreaseCountOfSet()
        {
            controller.Post(new TicketDTO {Number = "RemoveTest", Price = 100});
            var initialCount = context.Tickets.ToList().Count;
            var lastIndex = context.Tickets.Last().Id;
            var step = controller.Delete(lastIndex);
            var afterCount = context.Tickets.ToList().Count;
            Assert.IsFalse(initialCount == afterCount);
        }

        [Test]
        public void TestInner()
        {

        }
        [Test]
        public void TestRemoveInner()
        {

        }
        [Test]
        public void Update_Return_OkStatusCode()
        {
            var ticket = new TicketDTO {Id = 1 ,Number = "Test", Price = 1000};
            var result = controller.Put(1,ticket);
            Assert.AreEqual(new StatusCodeResult(200).StatusCode,((StatusCodeResult)result).StatusCode);
        }
        [Test]
        public void Update_ChangedObject_Shouldnt_EqualInitialObject()
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
