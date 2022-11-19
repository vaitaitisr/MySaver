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

            var mock = new Mock<IStoreService>();
            mock.Setup(x => x.GetStoresAsync().Result).Returns(GetStores());

            var viewModel = new StoresViewModel(mock.Object);

            // Act
            await viewModel.UpdateStoresAsync();
            var actual = viewModel.Stores;

            // Assert
            mock.Verify(x => x.GetStoresAsync(), Times.Once);

            Assert.Equal(expected.Count, actual.Count);

            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Name, actual[i].Name);
                Assert.Equal(expected[i].Address, actual[i].Address);
                Assert.Equal(expected[i].DefaultSchedule, actual[i].DefaultSchedule);
                Assert.Equal(expected[i].Latitude, actual[i].Latitude);
                Assert.Equal(expected[i].Longitude, actual[i].Longitude);

                foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
                {
                    string? expectedValue = null, actualValue = null;

                    expected[i].Schedule?.TryGetValue(day, out expectedValue);
                    actual[i].Schedule?.TryGetValue(day, out actualValue);

                    Assert.Equal(expectedValue, actualValue);
                }
            }
        }

        private List<Store> GetStores()
        {
            List<Store> output = new List<Store>
            {
                new Store()
                {
                    Name = "Store Name 1",
                    Address = "Store Address 1",
                    Schedule = new Dictionary<DayOfWeek, string>
                    {
                        { DayOfWeek.Monday, "Monday Schedule 1" },
                        { DayOfWeek.Tuesday, "Tuesday Schedule 1" },
                        { DayOfWeek.Wednesday, "Wednesday Schedule 1" },
                        { DayOfWeek.Thursday, "Thursday Schedule 1" },
                        { DayOfWeek.Friday, "Friday Schedule 1" },
                        { DayOfWeek.Saturday, "Saturday Schedule 1" },
                        { DayOfWeek.Sunday, "Sunday Schedule 1" }
                    },
                    Latitude = 15,
                    Longitude = 20
                },
                new Store()
                {
                    Name = "Store Name 2",
                    Address = "Store Address 2",
                    Schedule = new Dictionary<DayOfWeek, string>
                    {
                        { DayOfWeek.Saturday, "Saturday Schedule 2" },
                        { DayOfWeek.Sunday, "Sunday Schedule 2" }
                    },
                    DefaultSchedule = "Store Schedule 2",
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