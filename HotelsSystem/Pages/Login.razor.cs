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
        ClS_Config? config;

        private async Task login()
        {
            var session = Protection.GetDecryptedSession(JSRuntime, DB);
            await Task.Delay(1000);
        }
    }
}
