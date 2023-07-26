using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using ZarenTest.API.DTOs;

namespace ZarenTest.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        #region Props
        private readonly HttpClient _httpClient;
        #endregion


        #region Constructor
        public HotelController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        } 
        #endregion


        [HttpPost]
        public async Task<IActionResult>Location(RequestDTO model)
        {
            string apiUrl = "https://api.zarentravel.net/api/v1/zaren-travel/hotel/location";
            var jsonContent=JsonSerializer.Serialize(model);

            var content=new StringContent(jsonContent,Encoding.UTF8,"application/json");
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseData= await response.Content.ReadAsStringAsync();

                //json dataları property'lere dönüştürürken büyük/küçük harf duyarlılığını sağlaması için options
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result=JsonSerializer.Deserialize<ResponseDTO>(responseData, options);
                return Ok(result);
            }
            {

                return BadRequest();
            }

        }


    }

}
