using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using TRAFFIK_API.Models;

namespace TRAFFIK_API.Controllers
{
    [Route("api/InstagramPost")]
    [ApiController]
    public class InstagramFeedController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public InstagramFeedController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstagramPost>>> GetInstagramPosts()
        {
            var accessToken = "";
            var url = $"https://graph.instagram.com/me/media?fields=id,caption,media_url,media_type,timestamp&access_token={accessToken}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return StatusCode((int)response.StatusCode, "Failed to fetch Instagram posts");

                var json = await response.Content.ReadAsStringAsync();
                var posts = JsonSerializer.Deserialize<List<InstagramPost>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(posts);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving Instagram posts.");
            }
        }
    }
}