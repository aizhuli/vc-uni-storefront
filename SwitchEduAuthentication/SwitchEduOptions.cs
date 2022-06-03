
using System;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace SwitchIdAuthentication
{
    public static class OpenIdConnectOptionsExtensions
    {
        public static void AddSwitchEduOptions(this OpenIdConnectOptions options) {
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.ResponseType = "code";
            options.Scope.Add("email");
            options.Scope.Add("swissEduIDGroups");
            options.Scope.Add("swissEduIDBase");
            options.Scope.Add("swissEduIDExtended");
            options.Scope.Add("swissEduIDLinkedAffiliation");
            options.ClaimActions.DeleteClaim("sid");
            options.ClaimActions.DeleteClaim("idp");
            options.ClaimActions.DeleteClaim("s_hash");
            options.ClaimActions.DeleteClaim("auth_time");
            options.SaveTokens = true;
            options.GetClaimsFromUserInfoEndpoint = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = JwtClaimTypes.Name
            };
            options.Events.OnAuthorizationCodeReceived = context =>
            {
                context.Backchannel.SetBasicAuthenticationOAuth(context.TokenEndpointRequest.ClientId, context.TokenEndpointRequest.ClientSecret);
                return Task.CompletedTask;
            };
        }
    }
}
