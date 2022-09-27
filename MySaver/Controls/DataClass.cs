using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySaver.Controls
{
    internal class DataClass
    {
        string[] productList;
        private async Task <string[]> ReadDataFile()
        {
            string data;

            var inputFile = await FilePicker.Default.PickAsync();
            using StreamReader reader = new StreamReader(inputFile.FullPath);

            data = await reader.ReadToEndAsync();
            data = data.ToLower();

            productList = data.Split('\n');

            return productList;
        }

        public async Task <string[]> GetSearchResultsAsync(String input)
        {
            if (input == null) return null;

            if (productList == null)
            {
                productList = await ReadDataFile();
            }

            input = input.ToLower();

            string[] resultList = Array.FindAll(productList, element => element.Contains(input));
			
            return resultList;
        }
    }
}
