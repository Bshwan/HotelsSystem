namespace HotelsSystem.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]/[action]")]
    public class MainController : Controller
    {
        private readonly ISqlDataAccess db;
        private ClS_Config config = default!;
        private readonly IWebHostEnvironment env;
        private string DefaultImagePath = "img/placeholder.jpg";

        public MainController(ISqlDataAccess db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> GetAttachment(string id)
        {
            try
            {
                 config = new ClS_Config(db,new SPResult{});
                if (id != "0")
                {
                    var bytes = await config.HotelGetOneInfo<byte[]>(SelectPro: 5, ValID: int.Parse(id));

                    if (bytes != null)
                    {
                        if (bytes.Length > 0)
                        {
                            MemoryStream str = new MemoryStream(bytes);

                            return File(str, "image/jpeg");
                        }
                        else
                        {
                            var str = System.IO.File.OpenRead(env.WebRootFileProvider.GetFileInfo(DefaultImagePath).PhysicalPath);
                            return File(str, "image/jpeg");
                        }
                    }
                    else
                    {
                        var str = System.IO.File.OpenRead(env.WebRootFileProvider.GetFileInfo(DefaultImagePath).PhysicalPath);
                        return File(str, "image/jpeg");
                    }
                }
                else
                {
                    var str = System.IO.File.OpenRead(env.WebRootFileProvider.GetFileInfo(DefaultImagePath).PhysicalPath);
                    return File(str, "image/jpeg");
                }
            }
            catch (Exception ex)
            {
                // LocalRedirect(Routing.defaultpage);
                return Json(ex.Message);
            }
        }
    }
}