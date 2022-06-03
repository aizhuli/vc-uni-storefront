using System;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using SwitchEduAuthentication;

namespace SwitchIdAuthentication
{
    public static class SwitchEduExtensions
    {
       public static void AddSwitchEdu(this AuthenticationBuilder builder, Action<OpenIdConnectOptions> configureOptions)
       {
            builder.AddOpenIdConnect(SwitchEduDefaults.AuthenticationScheme, configureOptions);
       }
    }
}
