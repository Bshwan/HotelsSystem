﻿
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


        private ClS_Config config = default!;

        protected override async Task OnInitializedAsync()
        {
            var session = await Protection.GetDecryptedSession(jSRuntime, DB);
            config = new ClS_Config(DB, session);
        }
    }
}
