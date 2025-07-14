namespace lmsApi.jwt;

public static class JWTSetting
{
    private static IConfiguration? _configuration;
    
    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public static string PrivateKey => _configuration?["JwtSettings:Key"] ?? 
        throw new InvalidOperationException("JWT Key is not configured in appsettings.json or JWTSetting is not initialized");
    
    public static string Issuer => _configuration?["JwtSettings:Issuer"] ?? "lmsApi";
    
    public static string Audience => _configuration?["JwtSettings:Audience"] ?? "lmsApi";
    
    public static int ExpiryInHours => int.TryParse(_configuration?["JwtSettings:ExpiryInHours"], out var hours) ? hours : 1;
}