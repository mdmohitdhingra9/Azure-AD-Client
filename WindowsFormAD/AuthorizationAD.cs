using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;

namespace WindowsFormAD
{
    public class AuthorizationAD
    {
        private readonly IAuthenticationSettings authSettings;
        public AuthorizationAD(IAuthenticationSettings authenticationSettings)
        {
            this.authSettings = authenticationSettings;
        }

        public async Task<string> GetToken()
        {
            var authority = String.Format(CultureInfo.InvariantCulture, this.authSettings.AADInstance, this.authSettings.TenantId);
            AuthenticationContext authContext = new AuthenticationContext(authority, new FileCache());
            Uri redirectUri = new Uri(this.authSettings.RedirectUri);

            AuthenticationResult authResultForServer1 = authContext.AcquireToken(this.authSettings.ResourceIdForServer1, this.authSettings.ClientId, redirectUri, PromptBehavior.Always);

            //AuthenticationResult authResultForServer1 = authContext.AcquireToken(this.authSettings.ResourceIdForServer1, this.authSettings.ClientId, redirectUri, PromptBehavior.Always);

            //AuthenticationResult authResultForServer2 = authContext.AcquireToken(this.authSettings.ResourceIdForServer2, this.authSettings.ClientId, redirectUri, PromptBehavior.Auto);

            //var authResult21 = GetTokenByRefreshToken(authResultForServer1, authContext, this.authSettings.ClientId, this.authSettings.ResourceIdForServer1);

            //GetClaims(authResultForServer1.AccessToken);
            return authResultForServer1.AccessToken;

            //AuthenticationResult authResult = await authContext.AcquireTokenAsync(this.authSettings.ResourceId, this.authSettings.ClientId, redirectUri, new PlatformParameters(PromptBehavior.Auto));
        }

        private AuthenticationResult GetTokenByRefreshToken(AuthenticationResult authenticationResult, AuthenticationContext authContext, string ClientId, string resourceId)
        {
            if (authenticationResult != null && !string.IsNullOrEmpty(authenticationResult.AccessToken))
            {
                JwtSecurityToken securityToken = new JwtSecurityToken(authenticationResult.AccessToken);
                if (securityToken != null)
                {
                    //// If token is going to expire in next 15 minutes. 
                    if (securityToken != null && securityToken.ValidTo.AddMinutes(-15) <= DateTime.UtcNow)
                    {
                        return authContext.AcquireTokenByRefreshToken(authenticationResult.RefreshToken, ClientId, resourceId);
                    }
                }
            }

            return null;
        }

        public async Task<string> GetTokenForSecondResource(string authority, AuthenticationResult authResult1)
        {
            AuthenticationContext authContext = new AuthenticationContext(authority, new FileCache());

            var resourceId2 = "https://enterprisedirectory.onmicrosoft.com/0e-d2dbcdb54f6e";
            var result = await authContext.AcquireTokenByRefreshTokenAsync(authResult1.RefreshToken, this.authSettings.ClientId, resourceId2);

            //OfflineTokenValue(authContext);
            return result.AccessToken;
        }

        public string OfflineTokenValue(AuthenticationContext authContext)
        {
            var data = authContext.TokenCache.ReadItems().Where(x => x.Resource.Equals("https://enterprisedirectory.onmicrosoft.com/0bcdb54f6e")).FirstOrDefault();
            return authContext.TokenCache.ReadItems().FirstOrDefault()?.AccessToken;
        }

        public void RemoveToken(AuthenticationContext authContext)
        {
            string resourceId1 = ConfigurationManager.AppSettings["ResourceId"];
            string resourceId2 = "https://entectory.onmicrosoft.com/00cdb54f6e";
            var tokenItem1 = authContext.TokenCache.ReadItems().Where(x => x.Resource == resourceId1).FirstOrDefault();
            var tokenItem2 = authContext.TokenCache.ReadItems().Where(x => x.Resource == resourceId2).FirstOrDefault();
            authContext.TokenCache.DeleteItem(tokenItem1);
            authContext.TokenCache.DeleteItem(tokenItem2);
        }

        private void GetClaims(string token)
        {
            var jwtToken = new JwtSecurityToken(token);
            var validTo = DateTime.UtcNow;
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {

            }
            var claims = jwtToken.Claims;
            var cred = jwtToken.EncryptingCredentials;
            var identityClaims = claims as Claim[] ?? claims.ToArray();
        }


    }
}
