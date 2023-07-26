using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ZarenTest.API.Controllers;
using ZarenTest.API.DTOs;

namespace ZarenTest.TEST
{
    public class HotelControllerTest
    {

        #region Props
        private readonly ResponseDTO response;
        private RequestDTO requestDTO;
        private readonly HotelController _hotelController;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        #endregion


        #region Constructor
        public HotelControllerTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _hotelController = new HotelController(new HttpClient(_mockHttpMessageHandler.Object));
            response = new ResponseDTO
            {
                Data = new Data
                {
                    Items = new List<ResponseItem>
                {
                    new ResponseItem
                    {
                        Title = "Istanbul (province), Turkey",
                        Value = "10830",
                        Description = "description",
                        Type = 2,
                        Provider = 1
                    },
                    new ResponseItem
                    {
                        Title = "District of Columbia, USA",
                        Value = "10849",
                        Description = "description",
                        Type = 1,
                        Provider = 1
                    },
                    new ResponseItem
                    {
                        Title = "Christchurch, New Zealand",
                        Value = "19677",
                        Description = "description",
                        Type = 1,
                        Provider = 1
                    },
                    new ResponseItem
                    {
                        Title = "Istanbul, Turkey",
                        Value = "23472",
                        Description = "Istanbul (province)",
                        Type = 1,
                        Provider = 1
                    }

                }
                },
                Message = "Message",
                Status = 2
            };
            requestDTO = new RequestDTO()
            {
                Query = "ist",
                Provider = 1,
                Type = 1
            };
        }
        #endregion


        #region MockHttpClient
        public void MockHttpClient(HttpStatusCode statusCode)
        {
            var jsonContent = JsonSerializer.Serialize(requestDTO);

            _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            });
        } 
        #endregion


        #region LocationTests
        [Fact]
        public async void PostLocation_HttpResponseSuccess_ReturnOkWithContainData()
        {
            //Arrange
            string searchWord = "Istanbul";

            MockHttpClient(HttpStatusCode.OK);

            //Act
            var result = await _hotelController.Location(requestDTO);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseDTO = Assert.IsAssignableFrom<ResponseDTO>(okResult.Value);
            responseDTO = response;

            var filteredItems = responseDTO.Data.Items
            .Where(x => x.Title.Contains(searchWord, StringComparison.OrdinalIgnoreCase))
            .ToList();

            Assert.True(filteredItems.Count() > 0);
        }


        [Fact]
        public async void PostLocation_HttpResponseFail_ReturnBadRequest()
        {
            //Arrange
            MockHttpClient(HttpStatusCode.BadRequest);

            //Act
            var result = await _hotelController.Location(requestDTO);

            //Assert
            Assert.IsType<BadRequestResult>(result);

        }
        #endregion

    }
}
