using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Domain.Security
{
    public class AnonymousUserForStoreAuthorizationRequirement : IAuthorizationRequirement
    {
        public const string PolicyName = "DenyAnonymousForStore";
    }

    public class AnonymousUserForStoreAuthorizationHandler : AuthorizationHandler<AnonymousUserForStoreAuthorizationRequirement>
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public AnonymousUserForStoreAuthorizationHandler(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AnonymousUserForStoreAuthorizationRequirement requirement)
        {
            var workContext = _workContextAccessor.WorkContext;


            if (workContext.CurrentStore.AnonymousUsersAllowed || context.User.Identity.IsAuthenticated)
            {
                //workContext.CurrentUser.UserName = context.User.Identity.Name;
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
