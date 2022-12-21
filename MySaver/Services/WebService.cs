using System.Net;
using System.Text.Json;

namespace MySaver.Services;

public class WebService : IWebService
{
    private HttpClient _client;
    private IAlert _alert;

    public WebService(HttpClient client, IAlert alert)
    {
        _client = client;
        _alert = alert;
    }

    public async Task<List<T>> GetObjectListAsync<T>()
    {
        bool retry = false;
        string shortType = typeof(T).Name;
#if ANDROID
        Uri uri = new Uri("http://10.0.2.2:5272/api/" + shortType + 's');
#else
        Uri uri = new Uri("http://localhost:5272/api/" + shortType + 's');
#endif
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
                bool answer = await _alert.DisplayAlert("Error!",
                    $"{ex.Message} ... {ex.Response}", "Retry", "Cancel");
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
