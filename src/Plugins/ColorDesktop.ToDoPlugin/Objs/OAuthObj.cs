using System.Text.Json.Serialization;

namespace ColorDesktop.ToDoPlugin.Objs;

public record OAuthObj
{
    [JsonPropertyName("user_code")]
    public string UserCode { get; set; }
    [JsonPropertyName("device_code")]
    public string DeviceCode { get; set; }
    [JsonPropertyName("verification_uri")]
    public string VerificationUri { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("interval")]
    public int Interval { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public record OAuthGetCodeObj
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("ext_expires_in")]
    public int ExtExpiresIn { get; set; }
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
}