using Moq;
using MySaver.Models;
using MySaver.Services;
using MySaver.ViewModels;
using System.Collections.ObjectModel;

namespace MySaver.Tests
{
    public class StoresViewModelTests
    {
        [Fact]
        public async void UpdateStoresAsync_ShouldWork()
        {
            // Arrange
            var expected = new ObservableCollection<Store>(GetStores());

            var mockWeb = new Mock<IWebService>();
            var mockLocation = new Mock<IGeolocation>();
            var mockAlert = new Mock<IAlert>();

            mockWeb.Setup(x => x.GetObjectListAsync<Store>().Result).Returns(GetStores());

            var viewModel = new StoresViewModel(mockWeb.Object, mockAlert.Object, mockLocation.Object);

            // Act
            await viewModel.UpdateStoresAsync();
            var actual = viewModel.Stores;

            // Assert
            mockWeb.Verify(x => x.GetObjectListAsync<Store>(), Times.Once);

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Address, actual[i].Address);
                Assert.Equal(expected[i].DefaultSchedule, actual[i].DefaultSchedule);
                Assert.Equal(expected[i].SaturdaySchedule, actual[i].SaturdaySchedule);
                Assert.Equal(expected[i].SundaySchedule, actual[i].SundaySchedule);
                Assert.Equal(expected[i].Latitude, actual[i].Latitude);
                Assert.Equal(expected[i].Longitude, actual[i].Longitude);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async void GetClosestStoreAsync_CachedLocationShouldWork(int storeNumber)
        {
            // Arrange
            var expected = GetStores()[storeNumber];

            var mockWeb = new Mock<IWebService>();
            var mockLocation = new Mock<IGeolocation>();
            var mockAlert = new Mock<IAlert>();

            mockLocation.Setup(x => x.GetLastKnownLocationAsync().Result)
                .Returns(new Location(expected.Latitude, expected.Longitude));

            var viewModel = new StoresViewModel(mockWeb.Object, mockAlert.Object, mockLocation.Object);
            viewModel.Stores = new ObservableCollection<Store>(GetStores());

            // Act
            await viewModel.GetClosestStoreAsync();

            // Assert
            mockAlert.Verify(x => x.DisplayAlert("Closest store",
                $"{expected.Name} in {expected.Address}", "OK"), Times.Once);
        }

        private List<Store> GetStores()
        {
            List<Store> output = new List<Store>
            {
                new Store()
                {
                    Name = "Store Name 1",
                    Address = "Store Address 1",
                    DefaultSchedule = "Store Schedule 1",
                    SaturdaySchedule = "Saturday Schedule 1",
                    SundaySchedule = "Sunday Schedule 1",
                    Latitude = 15,
                    Longitude = 20
                },
                new Store()
                {
                    Name = "Store Name 2",
                    Address = "Store Address 2",
                    DefaultSchedule = "Store Schedule 2",
                    SundaySchedule = "Sunday Schedule 2",
                    Latitude = 30,
                    Longitude = 40
                },
                new Store()
                {
                    Name = "Store Name 3",
                    Address = "Store Address 3",
                    DefaultSchedule = "Store Schedule 3",
                    Latitude = 50,
                    Longitude = 30
                }
            };

            return output;
        }
    }
}
