using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Web;
using LMS.Blazor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Blazor.Controller;

[Route("proxy")]
[ApiController]
public class ProxyController(IHttpClientFactory httpClientFactory, ITokenStorage tokenService) : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ITokenStorage _tokenService = tokenService;

    public async Task<IActionResult> Proxy(string endpoint)
    {
        ArgumentException.ThrowIfNullOrEmpty(endpoint);
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized("Test");

        var accessToken = await _tokenService.GetAccessTokenAsync(userId);

        //ToDo: Before continue look for expired accesstoken and call refresh enpoint instead.
        //Tip: Look in TokenStorageService whats allready implementet
        //Use delegatinghandler on HttpClient or separate service to extract this logic!

        if (string.IsNullOrEmpty(accessToken))
            return Unauthorized();

        var client = _httpClientFactory.CreateClient("LmsAPIClient");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var queryString = Request.QueryString.Value;
        var targetUriBuilder = new UriBuilder($"{client.BaseAddress}{endpoint}");
        if (!string.IsNullOrEmpty(queryString))
        {
            var queryParams = HttpUtility.ParseQueryString(queryString);
            queryParams.Remove("endpoint");

            targetUriBuilder.Query = queryParams.ToString();
        }

        var method = new HttpMethod(Request.Method);
        var requestMessage = new HttpRequestMessage(method, targetUriBuilder.Uri);

        if (method != HttpMethod.Get && Request.ContentLength > 0)
        {
            Request.EnableBuffering();
                        
            var ms = new MemoryStream(); //Creates a temporary memorystream to place the requestbody into
                        
            await Request.Body.CopyToAsync(ms); // Copies the requestbody from client into our memorystream

            // Once it's written to "ms" the "pointer" points at the end of the stream.
            ms.Position = 0; // This line moves the pointer to the beginning to start reading again
                        
            Request.Body.Position = 0; //Sets the original requestbody's position to the beginning aswell for safetymeasures
                       
            requestMessage.Content = new StreamContent(ms); // Creates a new HttpContent-object based on the memorystream. Wich we send to the backend-API

            
            if (!string.IsNullOrEmpty(Request.ContentType))
            {
                requestMessage.Content.Headers.ContentType =
                    MediaTypeHeaderValue.Parse(Request.ContentType);
            }
        }

        foreach (var header in Request.Headers)
        {
            if (!header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase))
            {
                requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
        Console.WriteLine($"Proxying to: {targetUriBuilder.Uri}");
        var response = await client.SendAsync(requestMessage);

        //Handle customized error response
        if (!response.IsSuccessStatusCode) 
        {
            var errorJson = await response.Content.ReadAsStringAsync();
            try 
            {
                // Deserialize into ProblemDetails
                var problem = JsonSerializer.Deserialize<ProblemDetails>(
                    errorJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (problem != null)
                {
                    // Now you can access problem.Title, problem.Detail, etc.
                    Console.WriteLine($"Error Title: {problem?.Title}");
                    Console.WriteLine((int)response.StatusCode);

                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return StatusCode((int)response.StatusCode, problem?.Detail);   //<- returns new Content in response - ProblemDetails is not accessible from Client.Blazor
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        return !response.IsSuccessStatusCode
            ? Unauthorized()
            : StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
