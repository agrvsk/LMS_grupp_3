using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using LMS.Blazor.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LMS.Blazor.Client.Services;

public class ClientApiService(IHttpClientFactory httpClientFactory, NavigationManager navigationManager, IAuthReadyService authReady) : IApiService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient("BffClient");

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<bool> CallApiDeleteAsync(string endpoint, CancellationToken ct = default)
    {
        await authReady.WaitAsync();

        var requestMessage = new HttpRequestMessage(HttpMethod.Delete, $"proxy?endpoint={endpoint}");
        var response = await httpClient.SendAsync(requestMessage, ct);

        return response.IsSuccessStatusCode;
    }

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

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(errorJson);
        }

        response.EnsureSuccessStatusCode();

        var demoDtos = await JsonSerializer.DeserializeAsync<T>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions, CancellationToken.None) ?? default;
        return demoDtos;
    }
    public async Task<TResponse?> CallApiPostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken ct = default)
    {
        await authReady.WaitAsync();

        var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync($"proxy?endpoint={endpoint}", content, ct);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(errorJson))
            {
                var errors = JsonSerializer.Deserialize<Dictionary<string, string[]>>(errorJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (errors != null)
                {
                    foreach (var kvp in errors)
                    {
                      Console.WriteLine($"{kvp.Key}: {string.Join(", ", kvp.Value)}");
                        
                        throw new ValidationException(string.Join(", ", kvp.Value ));
                        break;
                        //var field = new FieldIdentifier(Module, kvp.Key);
                        //foreach (var error in kvp.Value)
                        //{
                        //    messageStore?.Add(field, error);
                        //}
                    }
                    //editContext?.NotifyValidationStateChanged();
                }

                //throw new ValidationException(errors);

            }
        }
        /*
    if (response.StatusCode == HttpStatusCode.BadRequest)
    {
        if (errors != null)
        {
            foreach (var kvp in errors)
            {
                var field = new FieldIdentifier(Module, kvp.Key);
                foreach (var error in kvp.Value)
                {
                    messageStore?.Add(field, error);
                }
            }

            editContext?.NotifyValidationStateChanged();
        }
    }
        */

        response.EnsureSuccessStatusCode();

        // Deserialize response
        var result = await JsonSerializer.DeserializeAsync<TResponse>(
            await response.Content.ReadAsStreamAsync(),
            _jsonSerializerOptions,
            CancellationToken.None
        );

        return result;
    }
    public async Task<TResponse?> CallApiPostMultipartAsync<TResponse>(string endpoint, MultipartFormDataContent data, CancellationToken ct = default)
    {
        await authReady.WaitAsync();
        var response = await httpClient.PostAsync($"proxy/upload?endpoint={endpoint}", data, ct);
        response.EnsureSuccessStatusCode();
        
        var result = await JsonSerializer.DeserializeAsync<TResponse>(
            await response.Content.ReadAsStreamAsync(),
            _jsonSerializerOptions,
            CancellationToken.None
        );
        return result;
    }

    public async Task<TResponse?> CallApiPutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken ct = default)
    {
        await authReady.WaitAsync();

        var json = JsonSerializer.Serialize(data, _jsonSerializerOptions);

        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PutAsync($"proxy?endpoint={endpoint}", content, ct);



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
