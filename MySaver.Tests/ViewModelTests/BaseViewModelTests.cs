using MySaver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver.Tests.ViewModelTests;

public class BaseViewModelTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsNotBusy_ShouldWork(bool busy)
    {
        // Arrange
        var viewModel = new BaseViewModel();

        // Act
        viewModel.IsBusy = busy;

        // Assert
        Assert.Equal(!busy, viewModel.IsNotBusy);
    }
}
