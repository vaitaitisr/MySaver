using System.Net;
using System.Text.Json;

namespace MySaver.Services;

public class WebService : IWebService
{
    private HttpClient _client;

    public WebService()
    {
        _client = new HttpClient();
    }

    public async Task<List<T>> GetObjectListAsync<T>()
    {
        bool retry = false;
        string shortType = typeof(T).Name;

        Uri uri = new Uri("http://10.0.2.2:5272/api/" + shortType + 's');
        do
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var result = JsonSerializer.Deserialize<List<T>>(content, options);
                    return result;
                }
                return null;
            }
            catch (WebException ex)
            {
                bool answer = await Shell.Current.DisplayAlert("Error!",
                    $"{ex.Message}", "Retry", "Cancel");
                if (answer)
                {
                    retry = true;
                }
                else return null;
            }
        while (retry);
        return null;
    }
}
