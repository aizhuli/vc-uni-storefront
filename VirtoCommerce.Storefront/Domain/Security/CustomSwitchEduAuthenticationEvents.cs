using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Customer;
using VirtoCommerce.Storefront.Model.Security;

namespace VirtoCommerce.Storefront.Domain.Security
{
    public class CustomSwitchEduAuthenticationEvents : OpenIdConnectEvents
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IdentityOptions _identityOptions;
        private readonly IWorkContextAccessor _workContextAccessor;

        public CustomSwitchEduAuthenticationEvents(
            SignInManager<User> signInManager,
            IOptions<IdentityOptions> identityOptions,
            IWorkContextAccessor workContextAccessor)
            
        {
            _signInManager = signInManager;
            _identityOptions = identityOptions.Value;
            _workContextAccessor = workContextAccessor;
        }

        public override async Task TicketReceived(TicketReceivedContext context)
        {
            var principal = context.Principal;
            var user = await _signInManager.UserManager.FindByNameAsync(principal.Claims.Where(x => x.Type == "email").FirstOrDefault().Value);
            if (user == null)
            {
                user = new User
                {
                    Email = principal.Claims.Where(x => x.Type == "email").FirstOrDefault().Value,
                    UserName = principal.Claims.Where(x => x.Type == "email").FirstOrDefault().Value,
                    Password = "Password",
                    UserType = "Customer"
                };
                var contact = new Contact
                {
                    Name = user.UserName,
                    FullName = principal.Claims.Where(x => x.Type == "name").FirstOrDefault().Value,
                    FirstName = principal.Claims.Where(x => x.Type == "given_name").FirstOrDefault().Value,
                    LastName = principal.Claims.Where(x => x.Type == "family_name").FirstOrDefault().Value
                };
                user.StoreId = "University";
                user.Contact = contact;

                var identityResult = await _signInManager.UserManager.CreateAsync(user, user.Password);
            }
            var userRetrieved = await _signInManager.UserManager.FindByNameAsync(user.UserName);
            await _signInManager.SignInAsync(userRetrieved, context.Properties);
        }

        public override Task AuthorizationCodeReceived(AuthorizationCodeReceivedContext context)
        {
            context.Backchannel.SetBasicAuthenticationOAuth(context.TokenEndpointRequest.ClientId, context.TokenEndpointRequest.ClientSecret);
            return Task.CompletedTask;
        }
    }
}
