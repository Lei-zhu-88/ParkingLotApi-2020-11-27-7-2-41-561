using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ParkingLotApi;
using ParkingLotApi.Dtos;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parkingLot_successfully()
        {
            var client = GetClient();
            ParkingLotDto parkingLotDto = new ParkingLotDto();
            parkingLotDto.Name = "LiverpoolLot";
            parkingLotDto.Location = "Liverpool";
            parkingLotDto.Capacity = 100;

            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);

            var allParkingLotResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotResponse.Content.ReadAsStringAsync();

            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Equal(1, returnParkingLots.Count);
        }

        [Fact]
        public async Task Should_not_create_parkingLot_if_name_exists()
        {
            var client = GetClient();
            ParkingLotDto parkingLotDto = new ParkingLotDto();
            parkingLotDto.Name = "LiverpoolLot";
            parkingLotDto.Location = "Liverpool";
            parkingLotDto.Capacity = 100;
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);
            var response = await client.PostAsync("/parkingLots", content);

            var allParkingLotResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Equal(1, returnParkingLots.Count);
            Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        }
    }
}