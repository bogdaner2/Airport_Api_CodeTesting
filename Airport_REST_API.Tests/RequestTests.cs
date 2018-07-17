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
        public void RequstPost(object item,out HttpWebResponse response)
        {
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp("http://localhost:55820/api/ticket");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(JsonConvert.SerializeObject(item));
            streamWriter.Close();
            response = (HttpWebResponse)httpRequest.GetResponse();
        }
        [Test]
        public void ValidationReturnOK_When_DTOIsCorrect()
        {
            //Arrange
            var ticket = new TicketDTO { Id = 5, Number = "FRGT32", Price = 1000 };
            //Act
            RequstPost(ticket, out HttpWebResponse response);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [Test]
        public void Api_Return200Status_When_ItemRemoved([Values(1)]int id)
        {
            //Arrange
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp("http://localhost:55820/api/ticket/"+id);
            httpRequest.Method = "DELETE";
            httpRequest.ContentType = "application/json";
            //Act
            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [Test]
        public void ValidationReturnBadRequest_When_DTOIsIncorrect()
        {
            //Arrange
            var ticket = new TicketDTO { Id = 5, Number = "FRGT32", Price = 1001000};
            //Act
            HttpStatusCode result = HttpStatusCode.OK;
            try
            {
               RequstPost(ticket,out HttpWebResponse request);
            }
            catch (WebException e)
            {
                result = HttpStatusCode.BadRequest;
            }
            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, result);
        }
        [Test]
        public void Api_Get_ReturnList()
        {
            //Arrange
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp("http://localhost:55820/api/ticket");
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/json";
            //Act
            string result;
            using (HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            //Assert
            Assert.IsNotNull(result);
        }
        [Test]
        public void Api_Return200Status_When_UpdatedItem()
        {
            //Arrange
            var ticket = new TicketDTO{Id = 2,Number = "TestFromApi",Price = 2666};
            HttpWebRequest httpRequest = HttpWebRequest.CreateHttp("http://localhost:55820/api/ticket/2");
            httpRequest.Method = "PUT";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(JsonConvert.SerializeObject(ticket));
            streamWriter.Close();
            //Act
            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
