namespace HotelsSystem.Pages
{
    public partial class Login
    {
        [Inject]
        public NavigationManager nav { get; set; } = default!;
        [Inject]
        public ISqlDataAccess DB { get; set; } = default!;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        protected IToaster Toaster { get; set; } = default!;

        //[Inject]
        //protected IBlazorDownloadFileService Downloader { get; set; }

        UserLogin Credentials = new UserLogin();
        ClS_Config config = default!;
        ClS_UserManagement mgmt = default!;
        MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
        protected override void OnInitialized()
        {
            mgmt = new ClS_UserManagement(DB, new SPResult { });
        }

        private async Task login()
        {
            var result = await mgmt.Login(
                SelectPro: 1,
                UserName: Credentials.UserName.ToEmptyOnNull(),
                UserPass: func.encr_pass(Credentials.Password.ToEmptyOnNull()));

            if (result.Result > 0)
            {
                string Language = "en";
                if (result.Userlanguage == 1)
                    Language = "ku";
                else if (result.Userlanguage == 2)
                    Language = "en";
                else if (result.Userlanguage == 3)
                    Language = "ar";

                await Protection.SetEncryptedSession(result, JSRuntime);
                RequestCultureChange(Language, Routing.hotels);

                return;
            }
            Toaster.Error(".", result.MSG);
        }
        private void RequestCultureChange(string Culture, string RedirectTo = "")
        {
            if (string.IsNullOrWhiteSpace(Culture))
            {
                return;
            }
            System.Console.WriteLine(new Uri(nav.BaseUri + RedirectTo));
            var uri = new Uri(nav.BaseUri + RedirectTo).GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            var query = $"?culture={Uri.EscapeDataString(Culture)}&" +
                $"redirectUri={Uri.EscapeDataString(uri)}";

            nav.NavigateTo("/Culture/SetCulture" + query, forceLoad: true);
        }
    }
}
