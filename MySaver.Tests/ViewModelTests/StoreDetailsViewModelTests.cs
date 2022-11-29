using Moq;
using MySaver.Models;
using MySaver.Services;
using MySaver.ViewModels;

namespace MySaver.Tests.ViewModelTests;

public class StoreDetailsViewModelTests
{
    [Fact]
    public async Task StoreDetailsViewModel_OpenMapShouldWorkAsync()
    {
        // Arrange
        var mockMap = new Mock<IMap>();
        var mockAlert = new Mock<IAlert>();

        var store = new Store()
        {
            Name = "Store Name",
            Address = "Store Address",
            DefaultSchedule = "Default Schedule",
            SaturdaySchedule = "Saturday Schedule",
            SundaySchedule = "Sunday Schedule",
            Latitude = 15,
            Longitude = 20
        };

        var viewModel = new StoreDetailsViewModel(mockMap.Object, mockAlert.Object);
        viewModel.Store = store;

        // Act
        await viewModel.OpenMapAsync();

        // Assert
        mockMap.Verify(x => x.OpenAsync(store.Latitude, store.Longitude,
            It.IsAny<MapLaunchOptions>()), Times.Once);
    }

    [Fact]
    public async Task StoreDetailsViewModel_OpenMapShouldNotWorkAsync()
    {
        // Arrange
        var mockMap = new Mock<IMap>();
        var mockAlert = new Mock<IAlert>();

        var ex = new Exception("error");

        mockMap.Setup(x => x.OpenAsync(It.IsAny<double>(), It.IsAny<double>(),
            It.IsAny<MapLaunchOptions>())).Throws(ex);

        var store = new Store()
        {
            Name = "Store Name",
            Address = "Store Address",
            DefaultSchedule = "Default Schedule",
            SaturdaySchedule = "Saturday Schedule",
            SundaySchedule = "Sunday Schedule",
            Latitude = 15,
            Longitude = 20
        };

        var viewModel = new StoreDetailsViewModel(mockMap.Object, mockAlert.Object);
        viewModel.Store = store;

        // Act
        await viewModel.OpenMapAsync();

        // Assert
        mockAlert.Verify(x => x.DisplayAlert("Error!",
            $"Unable to open map: {ex.Message}", "OK"), Times.Once);
    }
}
