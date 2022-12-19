using MySaver.Models;

namespace MySaver.Tests.ModelTests;

public class ProductModelTests
{
    [Fact]
    public void ProductModel_PropertyChangedShouldWork()
    {
        // Arrange
        List<string?> changedProperties = new List<string?>();

        var product = new Product()
        {
            StoreName = "Store Name",
            Name = "Product Name",
            Amount = 1,
            UnitPrice = 4.40f
        };

        product.PropertyChanged +=
            (sender, args) => changedProperties.Add(args.PropertyName);

        // Act
        product.Amount = 3;

        // Assert
        Assert.Contains(nameof(product.Amount), changedProperties);
        Assert.Contains(nameof(product.Price), changedProperties);
    }

    [Fact]
    public void ProductModel_PriceShouldWork()
    {
        // Arrange
        var expected = String.Format("{0:0.00}€", 8.8);

        var product = new Product()
        {
            StoreName = "Store Name",
            Name = "Product Name",
            Amount = 2,
            UnitPrice = 4.40f
        };

        // Act
        var actual = product.Price;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ProductModel_EqualsShouldWork()
    {
        // Arrange
        var product1 = new Product()
        {
            StoreName = "Store Name",
            Name = "Product Name",
            Amount = 2,
            UnitPrice = 4.40f
        };

        var product2 = new Product()
        {
            StoreName = "Store Name",
            Name = "Product Name",
            Amount = 1,
            UnitPrice = 3.40f
        };

        // Assert
        Assert.True(product1.Equals(product2));
    }

    [Fact]
    public void ProductModel_EqualsShouldntWorkIfProductNameIsNull()
    {
        // Arrange
        var product1 = new Product()
        {
            StoreName = "Maxima",
            Name = null,
            Amount = 1,
            UnitPrice = 1.89f
        };

        var product2 = new Product()
        {
            StoreName = "Maxima",
            Name = "Coca-Cola",
            Amount = 1,
            UnitPrice = 1.89f
        };

        // Assert
        Assert.False(product1.Equals(product2));
    }

    [Fact]
    public void ProductModel_EqualsShouldntWorkIfStoreNameIsNull()
    {
        // Arrange
        var product1 = new Product()
        {
            StoreName = null,
            Name = "Coca-Cola",
            Amount = 1,
            UnitPrice = 1.89f
        };

        var product2 = new Product()
        {
            StoreName = "Maxima",
            Name = "Coca-Cola",
            Amount = 1,
            UnitPrice = 1.89f
        };

        // Assert
        Assert.False(product1.Equals(product2));
    }
}
