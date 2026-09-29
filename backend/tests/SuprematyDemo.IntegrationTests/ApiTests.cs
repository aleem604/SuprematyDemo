using System.Net.Http.Json; using System.Text.Json; using System.Net; using Microsoft.AspNetCore.Mvc.Testing;
namespace SuprematyDemo.IntegrationTests;
public sealed class ApiTests:IClassFixture<WebApplicationFactory<Program>>
{private readonly HttpClient _client; public ApiTests(WebApplicationFactory<Program> f)=>_client=f.CreateClient();
[Fact] public async Task Health_is_anonymous(){var r=await _client.GetAsync("/health");Assert.Equal(HttpStatusCode.OK,r.StatusCode);}
[Fact] public async Task Products_require_authentication(){var r=await _client.GetAsync("/api/products");Assert.Equal(HttpStatusCode.Unauthorized,r.StatusCode);}}

public sealed class AuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public AuthenticationTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task Demo_user_can_login()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { email = "demo@suprematy.local", password = "Demo123!" });
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.False(string.IsNullOrWhiteSpace(payload.GetProperty("accessToken").GetString()));
    }
}
