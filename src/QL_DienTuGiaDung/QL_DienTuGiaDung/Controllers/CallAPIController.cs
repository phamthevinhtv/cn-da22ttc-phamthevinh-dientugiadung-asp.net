using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

[Route("api/")]
[ApiController]
public class CallAPIController : ControllerBase
{
    private readonly HttpClient _client;

    public CallAPIController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
    }

    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces()
    {
        var res = await _client.GetAsync("https://production.cas.so/address-kit/2025-07-01/provinces");
        var content = await res.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }

    [HttpGet("provinces/{provinceCode}/communes")]
    public async Task<IActionResult> GetCommunes(string provinceCode)
    {
        var res = await _client.GetAsync($"https://production.cas.so/address-kit/2025-07-01/provinces/{provinceCode}/communes");
        var content = await res.Content.ReadAsStringAsync();
        return Content(content, "application/json");
    }
}
