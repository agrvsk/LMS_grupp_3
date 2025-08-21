namespace LMS.Blazor.Client.Services;

public interface IApiService
{
    Task<T?> CallApiAsync<T>();
    Task<T?> CallApiAsync<T>(string sRouting);

}
