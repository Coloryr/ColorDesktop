using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ColorDesktop.ToDoPlugin.Objs;

public record OAuthObj
{
    [JsonProperty("user_code")]
    public string UserCode { get; set; }
    [JsonProperty("device_code")]
    public string DeviceCode { get; set; }
    [JsonProperty("verification_uri")]
    public string VerificationUri { get; set; }
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonProperty("interval")]
    public int Interval { get; set; }
    [JsonProperty("message")]
    public string Message { get; set; }
}

public record OAuthGetCodeObj
{
    [JsonProperty("token_type")]
    public string TokenType { get; set; }
    [JsonProperty("scope")]
    public string Scope { get; set; }
    [JsonProperty("access_token")]
    public string AccessToken { get; set; }
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonProperty("ext_expires_in")]
    public int ExtExpiresIn { get; set; }
    [JsonProperty("id_token")]
    public string IdToken { get; set; }
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}