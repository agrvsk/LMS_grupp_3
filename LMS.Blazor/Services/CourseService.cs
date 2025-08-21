//using System.Text.Json;
//using LMS.Shared.DTOs;
//using LMS.Shared.DTOs.EntityDto;
//using Microsoft.AspNetCore.Mvc;

//namespace LMS.Blazor.Services;

//public class CourseService
//{
//    private readonly HttpClient httpClient;

//    public CourseService(IHttpClientFactory httpClientFactory)
//    {
//        httpClient = httpClientFactory.CreateClient("LmsAPIClient");
//    }

//    public async Task<IEnumerable<CourseDto>> GetAllCourses()
//    {
//        var request = new HttpRequestMessage(HttpMethod.Get, $"/courses");
//        var response = await httpClient.SendAsync(request);

//        var content = await response.Content.ReadAsStringAsync();
//        var result = JsonSerializer.Deserialize<IEnumerable<CourseDto>>(content);

//        return result;
//    }
    
//    public async Task<IActionResult> GetCourseById([FromQuery]Guid id)
//    {
//        var request = new HttpRequestMessage(HttpMethod.Get, $"/courses/{id}");
//        var response = await httpClient.SendAsync(request);
//        if (response.IsSuccessStatusCode)
//        {
//            var content = await response.Content.ReadAsStringAsync();
//            var result = JsonSerializer.Deserialize<CourseDto>(content);
//            return new OkObjectResult(result);
//        }
//        return new NotFoundResult();
//    }
//    /*
//    public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto courseDto)
//    {
//        var request = new HttpRequestMessage(HttpMethod.Post, $"/courses");
//        var response = await httpClient.SendAsync(request);
//        //if (response.IsSuccessStatusCode)
//        //{
//        //    var content = await response.Content.ReadAsStringAsync();
//        //    var result = JsonSerializer.Deserialize<CourseDto>(content);
//        //    return new OkObjectResult(result);
//        //}
//        //return new NotFoundResult();

//    }
//    public async Task<IActionResult> UpdateCourse([FromQuery]Guid id, [FromBody] CourseDto courseDto)
//    {
//        var request = new HttpRequestMessage(HttpMethod.Put, $"/courses/{id}");
//        var response = await httpClient.SendAsync(request);

//    }

//    public async Task<IActionResult> DeleteCourse([FromQuery] Guid id)
//    {
//        var request = new HttpRequestMessage(HttpMethod.Delete, $"/courses/{id}");
//        var response = await httpClient.SendAsync(request);

//    }
//*/



//}
