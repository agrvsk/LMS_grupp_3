using LMS.Blazor.Client.Services;

namespace LMS.Blazor.Services.NoOpService;

public class ServerNoopApiService : IApiService
{
    public Task<T?> CallApiGetAsync<T>(string endpoint, CancellationToken ct = default) => Task.FromResult<T?>(default);
    public Task<TResponse?> CallApiPostAsync<TRequest, TResponse>(string endpoint, TRequest data, CancellationToken ct = default) => Task.FromResult<TResponse?>(default);
}
