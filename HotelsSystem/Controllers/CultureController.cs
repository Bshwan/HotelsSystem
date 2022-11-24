namespace HotelsSystem.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]/[action]")]
    public class CultureController : Controller
    {
        public IActionResult SetCulture(string culture, string redirectUri)
        {
            if (culture != null)
            {
                HttpContext.Response.Cookies.Append(
                        CookieRequestCultureProvider.DefaultCookieName,
                        CookieRequestCultureProvider.MakeCookieValue(
                            new RequestCulture(culture)));
                //  new RequestCulture(culture)), new CookieOptions { MaxAge = TimeSpan.FromDays(6 * 30) });
            }

            return LocalRedirect(redirectUri);
        }

    }
}