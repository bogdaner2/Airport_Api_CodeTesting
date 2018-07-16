using Airport_API_CodeTesting.Controllers;
using Airport_REST_API.Services.Interfaces;
using Airport_REST_API.Shared.DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Airport_REST_API.Tests
{
    public class ControllerTests
    {
        [Fact]
        public void Test_That_ControllerReturnOkStatus_When_ServiceReturnTrue()
        {
            //Arrange
            var mock = new Mock<ITicketService>();
            mock.Setup(service => service.Add(It.IsAny<TicketDTO>())).Returns(true);
            var controller = new TicketController(mock.Object);
            var ticket = new TicketDTO();
            //Act
            var result = controller.Post(ticket) as StatusCodeResult;
            //Assert
            Assert.Equal(new OkResult().StatusCode,result.StatusCode);

        }
   
    }
}
