using Moq;
using Moq.AutoMock;
using MySaver.ViewModels;
using MySaver.Models;
using MySaver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver.Tests.ViewModelTests;

public class ListEditorViewModelTests
{
    [Fact]
    public void ReadList_ShouldWork()
    {
        // Arrange
        var expectedName = "test";

        var mocker = new AutoMocker();
        var productMock = mocker.GetMock<IProductListService>();
        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
        {
            { "ListName", expectedName }
        });

        // Act
        viewModel.ReadList();

        // Assert
        productMock.Verify(x => x.OpenList(expectedName), Times.AtLeastOnce);
    }

    [Fact]
    public void SaveList_ShouldWork()
    {
        // Arrange
        var expectedName = "test";

        var mocker = new AutoMocker();
        var productMock = mocker.GetMock<IProductListService>();
        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
        {
            { "ListName", expectedName }
        });

        // Act
        viewModel.SaveList();

        // Assert
        productMock.Verify(x => x.SaveList(expectedName, It.IsAny<IEnumerable<Product>>()), Times.AtLeastOnce);
    }

    [Fact]
    public void CalculateTotal_ShouldWork()
    {
        // Arrange
        var product1 = new Product()
        {
            StoreName = "Maxima",
            Name = "Coca-Cola",
            Amount = 3,
            UnitPrice = 1.89f
        };

        var product2 = new Product()
        {
            StoreName = "Maxima",
            Name = "Pepsi",
            Amount = 2,
            UnitPrice = 1.75f
        };

        var expected = 9.17f;

        var mocker = new AutoMocker();
        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        viewModel.AddProduct(product1);
        viewModel.AddProduct(product2);

        // Act
        var actual = viewModel.CalculateTotal();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RenameList_ShouldWork()
    {
        // Arrange
        var expectedOldName = "testOld";
        var expectedNewName = "testNew";

        var mocker = new AutoMocker();
        var productMock = mocker.GetMock<IProductListService>();
        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
        {
            { "ListName", expectedOldName }
        });

        viewModel.ListName = expectedNewName;

        // Act
        viewModel.RenameList();

        // Assert
        productMock.Verify(x => x.RenameList(expectedOldName, expectedNewName), Times.AtLeastOnce);
    }

    [Fact]
    public void ShouldOpenAlreadyExistingList()
    {
        // Arrange
        var existingListName = "TestName";

        var expected = GetProducts();

        var mocker = new AutoMocker();
        var productMock = mocker.GetMock<IProductListService>();

        productMock.Setup(x => x.ListExists(existingListName)).Returns(true);
        productMock.Setup(x => x.OpenList(existingListName)).Returns(expected);

        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        // Act
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
        {
            { "ListName", existingListName }
        });

        var actual = viewModel.SelectedProducts.ToList();

        // Assert
        productMock.Verify(x => x.OpenList(existingListName), Times.AtLeastOnce);

        Assert.Equal(expected.Count(), actual.Count());

        for (int i = 0; i < expected.Count(); i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }

    [Fact]
    public void PropertyChanged_ShouldWork()
    {
        // Arrange
        List<string?> changedProperties = new List<string?>();

        var mocker = new AutoMocker();
        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        viewModel.PropertyChanged +=
            (sender, args) => changedProperties.Add(args.PropertyName);

        // Act
        viewModel.ListName = "TestName";

        // Assert
        Assert.Contains(nameof(viewModel.ListName), changedProperties);
    }

    [Fact]
    public void ApplyQueryAttributes_ShouldWork()
    {
        // Arrange
        var expected = "TestName";

        var mocker = new AutoMocker();
        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        // Act
        viewModel.ApplyQueryAttributes(new Dictionary<string, object>
        {
            { "ListName", expected }
        });

        var actual = viewModel.ListName;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task UpdateStorePrices_ShouldWork()
    {
        // Arrange
        var expected = new List<string>
        {
            $"Maxima: {3.64:0.00}€",
            $"Iki: {1.79:0.00}€ (trūksta produktų)"
        };

        var mocker = new AutoMocker();
        var webMock = mocker.GetMock<IWebService>();

        webMock.Setup(x => x.GetObjectListAsync<Product>().Result).Returns(GetProducts());

        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        viewModel.AddProduct(new Product { Name = "Pepsi" });
        viewModel.AddProduct(new Product { Name = "Coca-Cola" });

        var result = new CollectionView();

        // Act
        await viewModel.UpdateStorePrices(result);
        List<string> actual = (List<string>)result.ItemsSource;

        // Assert
        Assert.Equal(expected.Count(), actual.Count());

        for (int i = 0; i < expected.Count(); i++)
        {
            Assert.Equal(expected[i], actual[i]);
        }
    }

    [Theory]
    [InlineData("a")]
    [InlineData("e")]
    [InlineData("coca")]
    [InlineData("pepsi")]
    [InlineData("free")]
    public async Task GetSearchResultsAsync_ShouldWorkAsync(string query)
    {
        // Arrange
        var expected = from product in GetProducts()
                       where product.Name.ToLower().Contains(query)
                       group product by product.Name into productGroup
                       select productGroup.Key;

        var mocker = new AutoMocker();
        var webMock = mocker.GetMock<IWebService>();

        webMock.Setup(x => x.GetObjectListAsync<Product>().Result).Returns(GetProducts());

        var viewModel = mocker.CreateInstance<ListEditorViewModel>();

        // Act
        var actual = await viewModel.GetSearchResultsAsync(query);

        // Assert
        Assert.Equal(expected.Count(), actual.Count());

        for (int i = 0; i < expected.Count(); i++)
        {
            Assert.Equal(expected.ElementAt(i), actual.ElementAt(i).Name);
        }
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
