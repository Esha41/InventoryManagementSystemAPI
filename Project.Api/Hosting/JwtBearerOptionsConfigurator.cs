namespace Ettad.Api.Hosting;

internal static class JwtBearerOptionsConfigurator
{
    public static void Configure(JwtBearerOptions options, IConfiguration configuration)
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JWT:Secret"]
                    ?? throw new InvalidOperationException("JWT Secret is missing")))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = OnTokenValidatedAsync
        };
    }

    private static async Task OnTokenValidatedAsync(TokenValidatedContext context)
    {
        var purposeClaim = context.Principal?.FindFirst(JwtServices.TokenPurposeClaim);
        if (purposeClaim != null && purposeClaim.Value == JwtServices.TokenPurposeRoleSelection)
            return;

        var tokenBlacklistService = context.HttpContext.RequestServices.GetRequiredService<ITokenBlacklistService>();

        var jtiClaim = context.Principal?.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

        if (jtiClaim == null || string.IsNullOrWhiteSpace(jtiClaim.Value))
            return;

        var isBlacklisted = await tokenBlacklistService.IsTokenBlacklistedAsync(
            jtiClaim.Value,
            context.HttpContext.RequestAborted);

        if (isBlacklisted)
        {
            context.Fail("This token has been revoked.");
            Log.Warning("Blacklisted token rejected. TokenId: {TokenId}", jtiClaim.Value);
            return;
        }

        var userIdClaim = context.Principal?.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub
                || c.Type == ClaimTypes.NameIdentifier);
        var userId = userIdClaim?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return;

        var userManager = context.HttpContext.RequestServices
            .GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByIdAsync(userId);
        if (user == null || user.CurrentTokenId != jtiClaim.Value)
        {
            context.Fail("Session invalidated by new login.");
            Log.Warning("Token rejected - session invalidated. UserId: {UserId}, TokenId: {TokenId}", userId,
                jtiClaim.Value);
        }
    }
}
