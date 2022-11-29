using MySaver.Models;

namespace MySaver.Tests.ModelTests;

public class StoreModelTests
{
    [Fact]
    public void StoreModel_DefaultScheduleShouldWork()
    {
        // Arrange
        var store = new Store()
        {
            Name = "Store Name",
            Address = "Store Address",
            DefaultSchedule = "Default Schedule",
            Latitude = 15,
            Longitude = 20
        };

        var expected = store.DefaultSchedule;

        // Act
        var actual = store.TodaysSchedule;

        //Assert
        Assert.Equal(expected, actual);
    }
}
