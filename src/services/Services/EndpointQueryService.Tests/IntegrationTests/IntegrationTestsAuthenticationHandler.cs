using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EndpointQueryService.Tests.IntegrationTests
{
    public class IntegrationTestsAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string Schema = "test-schema";
        public IntegrationTestsAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            Request.Headers.TryGetValue("Authorization", out var value);
            if (value == StringValues.Empty)
                return Task.FromResult(AuthenticateResult.Fail("missing authorization header"));

            var token = value[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1];
            if(!Tokens.TryGetValue(token, out var claims))
                return Task.FromResult(AuthenticateResult.Fail("missing token for " + token));

            var identity = new ClaimsIdentity(claims, Schema);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Schema);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
        private static readonly Dictionary<string, IEnumerable<Claim>> Tokens = new()
        {
            { "registered", new[] { new Claim(ClaimsIdentity.DefaultRoleClaimType, "registered") } },
        };
    }
}
