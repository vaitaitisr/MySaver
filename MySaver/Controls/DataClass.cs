using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver.Controls;

internal class DataClass
{
    private string[] ProductList;
    private async Task<string[]> ReadDataFileAsync()
    {
        var InputFile = await FilePicker.Default.PickAsync();
        using StreamReader reader = new StreamReader(InputFile.FullPath);

        string Data = await reader.ReadToEndAsync();
        Data = Data.ToLower();

        ProductList = Data.Split('\n');

        return ProductList;
    }

    public async Task<string[]> GetSearchResultsAsync(string input)
    {
        if (input == null) return null;

        if (ProductList == null)
        {
            ProductList = await ReadDataFileAsync();
        }

        input = input.ToLower();

        var ResultQuery =
            (from Product in ProductList
             where Product.Contains(input)
             select Product).ToArray();

        return ResultQuery;
    }
}
