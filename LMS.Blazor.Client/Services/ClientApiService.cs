using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace LMS.Blazor.Client.Services;

public class ClientApiService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager, IAuthReadyService authReady) : IApiService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient("BffClient");

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<T?> CallApiGetAsync<T>(string endpoint, CancellationToken ct = default)
    {
        await authReady.WaitAsync();

        var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"proxy?endpoint={endpoint}");
        var response = await httpClient.SendAsync(requestMessage, ct);

        if (response.StatusCode == System.Net.HttpStatusCode.Forbidden
           || response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            navigationManager.NavigateTo("AccessDenied");
        }

        response.EnsureSuccessStatusCode();

        var demoDtos = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions, CancellationToken.None) ?? default;
        return demoDtos;
    }
    public async Task<TResponse?> CallApiPostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken ct = default)
    {
        await authReady.WaitAsync();

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"proxy?endpoint={endpoint}")
        {
            Content = new StringContent(JsonSerializer.Serialize(data, _jsonSerializerOptions), System.Text.Encoding.UTF8, "application/json")
        };

        var response = await httpClient.SendAsync(requestMessage, ct);

        response.EnsureSuccessStatusCode();

        // Deserialize response
        var result = await JsonSerializer.DeserializeAsync<TResponse>(
            await response.Content.ReadAsStreamAsync(),
            _jsonSerializerOptions,
            CancellationToken.None
        );

        return result;
    }


}
