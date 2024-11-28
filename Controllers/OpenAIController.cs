using LanguageTranningSystem.API.Request;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LanguageTranningSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _openAiKey;
        public OpenAIController(IConfiguration configuration)
        {
            _configuration = configuration;
            _openAiKey = configuration["OpenAiKey"] ?? string.Empty;
        }

        [HttpPost]
        public async Task<IActionResult> TextToSpeech([FromBody] TTSRequest input)
        {
            var httpClient = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.openai.com/v1/audio/speech"),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), $"Bearer {_openAiKey}" },
                    { HttpRequestHeader.Accept.ToString(), "application/json" }
                },
                Content = JsonContent.Create(input)
            };

            var response = await httpClient.SendAsync(httpRequestMessage);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest(new { Error = errorContent });
            }

            var byteArray = await response.Content.ReadAsByteArrayAsync();
            await System.IO.File.WriteAllBytesAsync("audio.mp3", byteArray);
            return Ok("Audio file saved successfully");
        }
    }
}
