namespace MySaver.Controls;

internal class DataClass
{
    private string[] productList;

    private async Task<string[]> ReadDataFileAsync()
    {
        using Stream inputFileStream = await FileSystem.OpenAppPackageFileAsync("ProductList.txt");
        using StreamReader reader = new StreamReader(inputFileStream);

        string data = await reader.ReadToEndAsync();

        data = data.ToLower();
        data = data.Replace("\r", "");

        productList = data.Split('\n');

        return productList;
    }

    public async Task<string[]> GetSearchResultsAsync(string input)
    {
        if (input == null) return null;

        if (productList == null)
        {
            productList = await ReadDataFileAsync();
        }

        input = input.ToLower();

        var resultQuery =
            (from product in productList
             where product.Contains(input)
             select product).ToArray();

        return resultQuery;
    }
}
