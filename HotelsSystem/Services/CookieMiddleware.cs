namespace HotelsSystem.Services
{
    public class CookieMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public CookieMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Path.Value.ToEmptyOnNull().Equals("/"))
                {
                    var session = Protection.Decrypt<SPResult>(context.Request.Cookies[Util.CookieName] ?? "");

                    if (session == null)
                        context.Response.Redirect(Routing.defaultpage);
                    else
                        context.Response.Redirect(Routing.userlist);
                }
            }
            catch
            {
                context.Response.Redirect("");
            }
            await _requestDelegate(context);
        }
    }
}