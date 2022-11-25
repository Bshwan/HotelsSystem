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
        ClS_Config config=default!;
        ClS_UserManagement mgmt=default!;
        MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
        protected override void OnInitialized()
        {
            mgmt = new ClS_UserManagement(DB, new SPResult { });
        }

        private async Task login()
        {
            SPResult result = await mgmt.Login<SPResult>(
                SelectPro: 1,
                UserName: Credentials.UserName.ToEmptyOnNull(),
                UserPass: func.encr_pass(Credentials.Password.ToEmptyOnNull()));

            if (result.Result > 0)
            {
                await Protection.SetEncryptedSession(result, JSRuntime);
                nav.NavigateTo(Routing.userlist);
                return;
            }
            Toaster.Error(".", result.MSG);
        }
    }
}
