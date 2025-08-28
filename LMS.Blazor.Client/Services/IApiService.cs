namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<T?> CallApiGetAsync<T>(string endpoint, CancellationToken ct = default);
    Task<TResponse?> CallApiPostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken ct = default);
    Task<TResponse?> CallApiPutAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken ct = default);
    Task<TResponse?> CallApiPostMultipartAsync<TResponse>(string endpoint, MultipartFormDataContent data, CancellationToken ct = default);

}
