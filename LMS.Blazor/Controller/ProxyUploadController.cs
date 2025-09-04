using Azure.Core;
using LMS.Blazor.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace LMS.Blazor.Controller
{
    [Route("proxy/upload")]
    [ApiController]
    public class UploadProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStorage _tokenService;

        public UploadProxyController(IHttpClientFactory httpClientFactory, ITokenStorage tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument()
        {
            var endpoint = Request.Query["endpoint"].ToString();
            if (string.IsNullOrEmpty(endpoint))
                return BadRequest("No endpoint specified.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var accessToken = await _tokenService.GetAccessTokenAsync(userId);
            if (string.IsNullOrEmpty(accessToken)) return Unauthorized();

            var client = _httpClientFactory.CreateClient("LmsAPIClient");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Copy the incoming multipart request directly to the API
            using var ms = new MemoryStream();
            await Request.Body.CopyToAsync(ms);
            ms.Position = 0;

            var content = new StreamContent(ms);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(Request.ContentType);

            var response = await client.PostAsync(endpoint, content);

            var responseBody = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, responseBody);
        }
    }
}
