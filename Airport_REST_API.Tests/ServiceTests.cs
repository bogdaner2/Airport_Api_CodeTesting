using System;
using System.Collections.Generic;
using System.Linq;
using Airport_REST_API.DataAccess;
using Airport_REST_API.DataAccess.Models;
using Airport_REST_API.DataAccess.Repositories;
using Airport_REST_API.Services.Service;
using Airport_REST_API.Shared.DTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using Xunit;

namespace Airport_REST_API.Tests
{
    public class CRUDTests
    {
        [Fact]
        public void Service_Should_ReturnFalse_When_UpdateNoExistingObject()
        {
            List<Ticket> tickets = new List<Ticket>
            {
                new Ticket {Id = 1,Number = "AAABRT",Price = 100},
                new Ticket {Id = 2,Number = "AABBRT",Price = 120}
            };
            var mockRepostiory = new Mock<IRepository<Ticket>>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(m => m.Tickets).Returns(mockRepostiory.Object);
            var mockMapper = new Mock<IMapper>();
            var service = new TicketService(mockUoW.Object,mockMapper.Object);
            ///
            var result = service.Update(0, It.IsAny<TicketDTO>());
            //Assert
            Assert.True(result == false);
        }

        [Fact]
        public void Initialize()
        {
        }
    }
}
