using FluentAssertions;
using JWTSimpleServer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace FunctionalTests.JwtSimpleServer
{

    public class JwtSimpleServerMiddlewareTestShould
    {
        [Fact]
        public async Task reject_invalid_grant_type()
        {
            var server = new TestServerBuilder()
                     .WithSuccessAuthentication()
                     .Build();

            var response = await server.CreateClient().PostAsync("/token", GrantTypes.AnInvalidGrantType());
            var content = await response.Content.ReadAsStringAsync();

            content.Should().Be(ServerMessages.InvalidGrantType);
            response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task return_a_valid_jwt_token_when_grant_type_password()
        {
            var server = new TestServerBuilder()
                   .WithSuccessAuthentication()
                   .Build();

            var response = await server.CreateClient().PostAsync("/token", GrantTypes.APasswordGrantType());
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jwtTokenResponse = JsonConvert.DeserializeObject<JwtTokenResponse>(content);

            jwtTokenResponse.Should().NotBeNull();
            jwtTokenResponse.AccessToken.Should().NotBeNull();
            jwtTokenResponse.ExpiresIn.Should().BeGreaterThan(0);

        }

        [Fact]
        public async Task allow_invoking_authorized_controller_with_a_token()
        {
            var server = new TestServerBuilder()
                   .WithSuccessAuthentication()
                   .Build();

            var client = server.CreateClient();

            var response = await client.PostAsync("/token", GrantTypes.APasswordGrantType());
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var jwtTokenResponse = JsonConvert.DeserializeObject<JwtTokenResponse>(content);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtTokenResponse.AccessToken);
            response = await client.GetAsync("/api/test");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task allow_refresh_an_accessToken_from_refresh_token()
        {

            var server = new TestServerBuilder()
                   .WithSuccessAuthentication()
                   .WithInMemoryStore()
                   .Build();

            var client = server.CreateClient();

            var response = await client.PostAsync("/token", GrantTypes.APasswordGrantType());
            response.EnsureSuccessStatusCode();            
            var tokenResponse = await ReadRequestResponseToJwtTokenResponse(response);
            response = await client.PostAsync("/token", GrantTypes.ARefreshTokenGranType(tokenResponse.RefreshToken));
            response.EnsureSuccessStatusCode();            
            var refreshTokenResponse = await ReadRequestResponseToJwtTokenResponse(response);

            refreshTokenResponse.AccessToken.Should().NotBeEmpty();
            refreshTokenResponse.RefreshToken.Should().NotBeEmpty();

            refreshTokenResponse.AccessToken.Should().NotBe(tokenResponse.AccessToken);
            refreshTokenResponse.RefreshToken.Should().NotBe(tokenResponse.RefreshToken);
        }

        private async Task<JwtTokenResponse> ReadRequestResponseToJwtTokenResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<JwtTokenResponse>(content);            
        }

    }
}
