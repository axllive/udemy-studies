using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext. HttpContext. User. Identity. IsAuthenticated) return;

            var usrId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices. GetRequiredService<IUserRepository>();
            var user = await repo.GetUserByIdAsync(usrId) ;
            user. LastActive = DateTime.UtcNow;
            repo.Update(user);
            await repo.SaveAllAsync();
        }
    }
}