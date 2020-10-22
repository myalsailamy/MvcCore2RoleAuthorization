using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Mvc.RoleAuthorization.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc.RoleAuthorization.Handlers
{
	public class AuthorizationRequirement : IAuthorizationRequirement { }

	public class PermissionHandler : AuthorizationHandler<AuthorizationRequirement>
	{
		private readonly IDataAccessService _dataAccessService;

		public PermissionHandler(IDataAccessService dataAccessService)
		{
			_dataAccessService = dataAccessService;
		}

        //protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        //{
        //    if (context.Resource is AuthorizationFilterContext mvcContext)
        //    {
        //        if (mvcContext.Filters.Any(filter => filter is MyFilter))
        //        {
        //            context.Succeed(requirement);
        //            return Task.CompletedTask;
        //        }
        //    }
        //}

        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
		{

            // Work for api And mvc
            //var endpoint = context.Resource as RouteEndpoint;
            //var descriptor = endpoint?.Metadata?.SingleOrDefault(md => md is ControllerActionDescriptor) as ControllerActionDescriptor;
            //if (descriptor == null) throw new InvalidOperationException("Unable to retrieve current action descriptor.");


            //only work on mvc
            var mvcContext = context.Resource as AuthorizationFilterContext;
            var descriptor = mvcContext?.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var ctrlName = descriptor.ControllerName;
                var actionName = descriptor.ActionName;

                var isAuthenticated = context.User.Identity.IsAuthenticated;

                if (isAuthenticated && ctrlName != null && actionName != null &&
                    await _dataAccessService.GetMenuItemsAsync(context.User, ctrlName.ToString(), actionName.ToString()))
                {
                    context.Succeed(requirement);
                }

            }

   //         if (context.Resource is RouteEndpoint endpoint)
			//{
			//	endpoint.RoutePattern.Defaults.TryGetValue("controller", out var _controller);
			//	endpoint.RoutePattern.Defaults.TryGetValue("action", out var _action);
				
			//	//endpoint.RoutePattern.Defaults.TryGetValue("page", out var _page);
			//	//endpoint.RoutePattern.Defaults.TryGetValue("area", out var _area);

			//}
		}
	}
}