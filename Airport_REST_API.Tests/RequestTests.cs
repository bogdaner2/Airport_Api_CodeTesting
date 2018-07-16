using Airport_API_CodeTesting.Controllers;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Airport_REST_API.Tests
{
    public class RequestTests
    {
        [Fact]
        public void Test_ValidationReturnBadRequest_When_DTOIsIncorrect()
        {
            var ticket = new TicketDTO();
            var controller = new TicketController(It.IsAny<ITicketService>());
            //Act
            var result = controller.Post(ticket);
            //Assert
            Assert.Equal(new BadRequestResult(), result);
        }
    }
}
