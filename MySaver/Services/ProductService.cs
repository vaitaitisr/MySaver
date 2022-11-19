using Microsoft.VisualBasic;
using MySaver.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MySaver.Services;
public class ProductService
{
    private HttpClient _client;
    public ProductService()
    {
        _client = new HttpClient();
    }

    public async Task<List<Product>> ReadDataFileAsync()
    {
        Uri productUri = new Uri("http://10.0.2.2:5272/api/products/");


        //var data = await reader.ReadToEndAsync();

        try
        {
            HttpResponseMessage response = await _client.GetAsync(productUri);
            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                content.Replace(" ", "");
                return JsonSerializer.Deserialize<List<Product>>(content);
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }
}