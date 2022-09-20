using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver
{
    internal class DataClass
    {
        string[] productList;
        string data;

        private async Task ReadDataFile()
        {
            var inputFile = await FilePicker.Default.PickAsync();
            using StreamReader reader = new StreamReader(inputFile.FullPath);

            data = await reader.ReadToEndAsync();
            data = data.ToLower();
            productList = data.Split('\n');
        }
        public async Task <string[]> GetSearchResultsAsync(String input)
        {
            if (input == null) return null;
            if (data == null)
            {
                await ReadDataFile();
            }
            input = input.ToLower();
            string[] resultList = Array.FindAll(productList, element => element.Contains(input));
            if (resultList == null) return null;
            else
            return resultList;
        }
    }
}
