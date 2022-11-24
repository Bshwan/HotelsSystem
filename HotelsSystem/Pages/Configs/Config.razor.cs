namespace HotelsSystem.Pages.Configs
{
    public partial class Config
    {
        [Inject]
        public ISqlDataAccess DB { get; set; } = default!;

        [Inject]
        public NavigationManager nav { get; set; } = default!;

        [Inject]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        public IToaster Toaster { get; set; } = default!;

        private ClS_Config config = default!;
        private SPResult? session;

        protected override async Task OnInitializedAsync()
        {
            // try
            // {
            session = await Protection.GetDecryptedSession(JSRuntime, DB);

            //if (Protection.isSessionInvalid(session))
            //{
            //    nav.NavigateTo("");
            //    return;
            //}
            config = new ClS_Config(DB, session);

            //if(!await config.GetPermissionInstance().AdminPer())
            //{
            //    nav.NavigateTo(Routing.defaultpage);
            //    return;
            //}
            // }
            // catch
            // {
            //     nav.NavigateTo("");
            // }
        }
    }
}
