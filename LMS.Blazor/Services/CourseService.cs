using System.Text.Json;
using LMS.Shared.DTOs;
using LMS.Shared.DTOs.EntityDto;

namespace LMS.Blazor.Services;

public class CourseService
{
    private readonly HttpClient httpClient;

    public CourseService(IHttpClientFactory httpClientFactory)
    {
        httpClient = httpClientFactory.CreateClient("LmsAPIClient");
    }

    public async Task<IEnumerable<CourseDto>> GetAllCourses()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"/courses");
        var response = await httpClient.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<IEnumerable<CourseDto>>(content);

        return result;
    }


}
