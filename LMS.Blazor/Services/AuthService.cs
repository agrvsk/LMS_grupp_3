using LMS.Shared.DTOs.AuthDtos;

namespace LMS.Blazor.Services;

public class AuthService
{
    //FROM COPILOT...
    //private readonly HttpClient _httpClient;

    //public AuthService(IHttpClientFactory factory)
    //{
    //    _httpClient = factory.CreateClient("LmsAPIClient");
    //}

    //public async Task<TokenDto?> LoginAsync(string email, string password)
    //{
    //    var response = await _httpClient.PostAsJsonAsync("api/auth/login",
    //        new UserAuthDto { UserName = email, Password = password });

    //    if (!response.IsSuccessStatusCode)
    //        return null;

    //    return await response.Content.ReadFromJsonAsync<TokenDto>();
    //}
}
