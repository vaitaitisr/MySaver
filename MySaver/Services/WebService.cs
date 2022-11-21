using System.Text.Json;

namespace MySaver.Services;

public class WebService
{
    private HttpClient _client;

    public WebService()
    {
        _client = new HttpClient();
    }

    public async Task<List<T>> GetObjectListAsync<T>()
    {
        string shortType = typeof(T).ToString();
        shortType = shortType.Replace("MySaver.Models.", "");

        Uri uri = new Uri("http://10.0.2.2:5272/api/" + shortType + 's');

        try
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();

                //  removing all spaces negatively affects store list, 
                //  products seem fine even with extra spaces
                //content = content.Replace(" ", "");

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var result = JsonSerializer.Deserialize<List<T>>(content, options);
                return result;
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
