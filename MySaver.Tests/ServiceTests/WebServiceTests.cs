using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Moq.AutoMock;
using Moq.Protected;
using MySaver.Models;
using MySaver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySaver.Tests.ServiceTests;

public class WebServiceTests
{
    [Fact]
    public async Task GetObjectListAsync_ShouldWork()
    {
        // Arrange
        var expected = GetProducts();

        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize<List<Product>>(expected))
        };

        var mocker = new AutoMocker();

        var mockHttp = mocker.GetMock<HttpMessageHandler>();

        mockHttp.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);

        var service = mocker.CreateInstance<WebService>();

        // Act
        var actual = await service.GetObjectListAsync<Product>();

        // Assert
        Assert.Equal(expected.Count(), actual.Count());

        for (int i = 0; i < expected.Count(); i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }

    [Fact]
    public async Task GetObjectListAsync_ShouldntWork()
    {
        // Arrange
        var mocker = new AutoMocker();

        var mockHttp = mocker.GetMock<HttpMessageHandler>();

        mockHttp.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new WebException());

        var mockAlert = mocker.GetMock<IAlert>();

        mockAlert.SetupSequence(x => x.DisplayAlert(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()).Result)
            .Returns(true)
            .Returns(false);

        var service = mocker.CreateInstance<WebService>();

        // Act
        var actual = await service.GetObjectListAsync<Product>();

        // Assert
        Assert.Null(actual);

        mockAlert.Verify(x => x.DisplayAlert(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task IntegrationTestWithWebApi()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateDefaultClient();

        var mockAlert = new Mock<IAlert>();

        mockAlert.Setup(x => x.DisplayAlert(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()).Result)
            .Returns(false);

        var service = new WebService(factory.CreateDefaultClient(), mockAlert.Object);

        // Act
        var products = await service.GetObjectListAsync<Product>();

        // Assert
        Assert.NotNull(products);
    }

    private List<Product> GetProducts()
    {
        List<Product> output = new List<Product>
        {
            new Product()
            {
                StoreName = "Maxima",
                Name = "Coca-Cola",
                Amount = 1,
                UnitPrice = 1.89f
            },
            new Product()
            {
                StoreName = "Maxima",
                Name = "Pepsi",
                Amount = 1,
                UnitPrice = 1.75f
            },
            new Product()
            {
                StoreName = "Iki",
                Name = "Pepsi",
                Amount = 1,
                UnitPrice = 1.79f
            },
            new Product()
            {
                StoreName = "Lidl",
                Name = "Freeway Cola",
                Amount = 1,
                UnitPrice = 1.39f
            }
        };

        return output;
    }
}
