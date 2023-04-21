public class MyHttpClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;

    public MyHttpClient(string baseAddress)
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
    }

    public async Task<T> GetAsync<T>(string requestUri)
    {
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, _serializerOptions);
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string requestUri, TRequest request)
    {
        var jsonContent = JsonSerializer.Serialize(request, _serializerOptions);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(requestUri, httpContent);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResponse>(content, _serializerOptions);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}

public void ConfigureServices(IServiceCollection services)
{
    // Register MyHttpClient as a singleton service
    services.AddSingleton<MyHttpClient>(sp => new MyHttpClient("https://api.example.com"));
}

using httpclient :
public class MyService
{
    private readonly MyHttpClient _httpClient;

    public MyService(MyHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _httpClient.GetAsync<List<Product>>("/products");
    }
}

public async Task<HttpResponseMessage> PostFileAsync(string uri, Stream stream, string fileName)
    {
        var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(streamContent, "file", fileName);
        HttpResponseMessage response = await _httpClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
        return response;
    }

    using (var stream = new FileStream("path/to/excel/file.xlsx", FileMode.Open))
{
    HttpResponseMessage response = await myHttpClient.PostFileAsync("/upload", stream, "data.xlsx");
}