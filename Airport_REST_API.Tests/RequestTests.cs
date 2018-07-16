using System.IO;
using System.Net;
using Airport_REST_API.Shared.DTO;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Airport_REST_API.Tests
{
    [TestFixture]
    public class RequestTests
    {
        [Test]
        public void ValidationReturnOK_When_DTOIsCorrect()
        {
            //Arrange
            var ticket = new TicketDTO { Id = 5, Number = "FRGT32" , Price = 1000};
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp("http://localhost:55820/api/ticket");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(JsonConvert.SerializeObject(ticket));
            streamWriter.Close();
            //Act
            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            //Assert
            Assert.AreEqual(HttpStatusCode.OK,response.StatusCode);
        }
        [Test]
        public void ValidationReturnBadRequest_When_DTOIsIncorrect()
        {
            //Arrange
            var ticket = new TicketDTO { Id = 5, Number = "FRGT32", Price = 1001000};
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp("http://localhost:55820/api/ticket");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(JsonConvert.SerializeObject(ticket));
            streamWriter.Close();
            //Act
            HttpStatusCode result = HttpStatusCode.OK;
            try
            {
                HttpWebResponse response = (HttpWebResponse) httpRequest.GetResponse();
            }
            catch (WebException e)
            {
                result = HttpStatusCode.BadRequest;
            }

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }
    }
}
