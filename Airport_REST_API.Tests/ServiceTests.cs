using System;
using System.Collections.Generic;
using System.Linq;
using Airport_REST_API.DataAccess;
using Airport_REST_API.DataAccess.Models;
using Airport_REST_API.DataAccess.Repositories;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Services.Service;
using Airport_REST_API.Shared.DTO;
using AutoMapper;
using Moq;
using NUnit.Framework;

namespace Airport_REST_API.Tests
{
    [TestFixture]
    public class ServiceTests
    {
        private Mock<IRepository<Ticket>> ticketRepository;
        private Mock<IUnitOfWork> mockUoW;
        private Mock<IMapper> mapper;
        private ITicketService service;

        [SetUp]
        public void Initialize()
        {
            mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<TicketDTO>(It.IsAny<Ticket>())).Returns(new TicketDTO());
            ticketRepository = new Mock<IRepository<Ticket>>();
            mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(m => m.Tickets).Returns(ticketRepository.Object);
            service = new TicketService(mockUoW.Object, mapper.Object);
        }


        [Test]
        public void Service_Should_ReturnFalse_When_UpdateNoExistingObject()
        {
            List<Ticket> tickets = new List<Ticket>
            {
                new Ticket {Id = 1,Number = "AAABRT",Price = 100},
                new Ticket {Id = 2,Number = "AABBRT",Price = 120}
            };
            ///
            var result = service.Update(1, It.IsAny<TicketDTO>());
            //Assert
            Assert.True(result == false);
        }

        [Test]
        public void ReturnSave()
        {
            service.Add(new TicketDTO());
            mockUoW.Verify(x => x.Save());
        }

        [Test]
        public void GetCollection_Test()
        {
            List<Ticket> tickets = new List<Ticket>
            {
                new Ticket {Id = 1,Number = "AAABRT",Price = 100},
                new Ticket {Id = 2,Number = "AABBRT",Price = 120},
                new Ticket {Id = 3,Number = "AAABR2",Price = 180},
            };
            mockUoW.Setup(x => x.Tickets.GetAll()).Returns(tickets);

            // Act
            var result = service.GetCollection();

            // Assert
            Assert.AreEqual(tickets.Count,result.ToList().Count);
        }
        [Test]
        public void GetTicketById_WithNegativeId_ShouldReturnNull()
        {
            // Arrange
            mockUoW.Setup(x => x.Tickets.Get(It.Is<int>(y => y < 0))).Returns((Ticket)null);

            // Act
            var result = service.GetObject(-10);

            // Assert
            Assert.IsNull(result);
        }

    }
}
