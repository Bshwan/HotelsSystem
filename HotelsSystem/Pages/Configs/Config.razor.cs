
namespace HotelsSystem.Pages.Configs
{
    public partial class Config
    {
        [Inject]
        protected ISqlDataAccess DB { get; set; } = default!;
        [Inject]
        protected IJSRuntime jSRuntime { get; set; } = default!;
        [Inject]
        protected IToaster Toaster { get; set; } = default!;
        [Inject]
        protected NavigationManager nav { get; set; } = default!;
        [Inject]
        protected IDialogService DialogService { get; set; } = default!;
        [Inject]
        public ISessionStorageService storage { get; set; } = default!;


        private ClS_Config config = default!;
        SPResult? session;

        protected override async Task OnInitializedAsync()
        {
            session = await Protection.GetDecryptedSession(jSRuntime, DB,storage);
            config = new ClS_Config(DB, session);
        }
    }
}
