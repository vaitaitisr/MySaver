namespace MySaver.Controls;

internal class DataClass
{
    private string[] productList;
    private async Task<string[]> ReadDataFileAsync()
    {
        var inputFile = await FilePicker.Default.PickAsync();
        using StreamReader reader = new StreamReader(inputFile.FullPath);

        string data = await reader.ReadToEndAsync();
        data = data.ToLower();

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
