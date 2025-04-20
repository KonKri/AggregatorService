using AggregatorService.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
namespace AggregatorService.Infrastructure.Tests;

public class WeatherServiceTests
{
    private string _jsonResponse = @"
    {
        ""coord"": {
            ""lon"": 23.7162,
            ""lat"": 37.9795
        },
        ""weather"": [
            {
                ""id"": 801,
                ""main"": ""Clouds"",
                ""description"": ""few clouds"",
                ""icon"": ""02d""
            }
        ],
        ""base"": ""stations"",
        ""main"": {
            ""temp"": 23.56,
            ""feels_like"": 23.12,
            ""temp_min"": 22.78,
            ""temp_max"": 25.05,
            ""pressure"": 1013,
            ""humidity"": 44,
            ""sea_level"": 1013,
            ""grnd_level"": 989
        },
        ""visibility"": 10000,
        ""wind"": {
            ""speed"": 6.17,
            ""deg"": 200
        },
        ""clouds"": {
            ""all"": 20
        },
        ""dt"": 1745148745,
        ""sys"": {
            ""type"": 2,
            ""id"": 2081401,
            ""country"": ""GR"",
            ""sunrise"": 1745120564,
            ""sunset"": 1745168697
        },
        ""timezone"": 10800,
        ""id"": 264371,
        ""name"": ""Athens"",
        ""cod"": 200
    }";

    [Fact]
    public async void GivenWeatherApiResposneIsOk_WhenFetching_ThenWeatherData_AndNotFallback()
    {

        // arrange.
        var mockHandler = new Mock<HttpClientHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(_jsonResponse),
            });

        var httpClient = new HttpClient(mockHandler.Object);
        httpClient.BaseAddress = new Uri("http://asdf.com");

        var mockLogger = new Mock<ILogger<IWeatherService>>();

        var weatherService = new WeatherService(httpClient, mockLogger.Object, "key");

        // act.
        var res = await weatherService.FetchAsync("Athens");

        // assert.
        Assert.NotNull(res.Item1); // we recieve data.
        Assert.True(!res.Item3); // data is not fallback.
    }

    [Fact]
    public async void GivenWeatherApiResposneIsNotOk_WhenFetching_ThenFallbackWeatherData()
    {

        // arrange.
        var mockHandler = new Mock<HttpClientHandler>();
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
            });

        var httpClient = new HttpClient(mockHandler.Object);
        httpClient.BaseAddress = new Uri("http://asdf.com");

        var mockLogger = new Mock<ILogger<IWeatherService>>();

        var weatherService = new WeatherService(httpClient, mockLogger.Object, "key");

        // act.
        var res = await weatherService.FetchAsync("Athens");

        // assert.
        Assert.Equal("-", res.Item1.City);
        Assert.Equal("-", res.Item1.Description);

        Assert.True(res.Item3); // data is fallback.
    }
}