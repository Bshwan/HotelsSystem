using HotelsSystem.Models;
using HotelsSystem.Pages.Configs;
using static Azure.Core.HttpHeader;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using MudBlazor;

namespace HotelsSystem.Shared.Modals
{
    public partial class AddRole
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public int ID { get; set; }= 0;

        [Inject]
        protected ISqlDataAccess DB { get; set; } = default!;
        [Inject]
        protected IJSRuntime jSRuntime { get; set; } = default!;
        [Inject]
        protected IToaster Toaster { get; set; } = default!;
        [Inject]
        protected NavigationManager nav { get; set; } = default!;

        private ClS_Config config = default!;
        ClS_UserManagement mgmt = default!;

        private PermissionsPerGroups SelectedPermission = new PermissionsPerGroups();
        private IEnumerable<PermissionsPerGroups> combo = Enumerable.Empty<PermissionsPerGroups>();
        MudForm? AddForm;

        void Submit() => MudDialog.Close(DialogResult.Ok(true));
        void Cancel() => MudDialog.Cancel();

        private int UserID = 0;
        protected override async Task OnInitializedAsync()
        {
            var session = await Protection.GetDecryptedSession(jSRuntime, DB);
            config = new ClS_Config(DB, session);
            mgmt = new ClS_UserManagement(DB, session);
            await GetCombo();
        }
        private async Task GetCombo()
        {
            combo = await config.GetCMB<PermissionsPerGroups>(SelectPro: 4,ValID:ID);
        }
        
        private async Task<IEnumerable<PermissionsPerGroups>> SearchPermission(string val)
        {
            return await Task.FromResult(combo.Where(x => x.peo_DataRole.ToEmptyOnNull().Contains(val.ToEmptyOnNull()) && x.HasRole==0));
        }

        private void OnPermissionChanged(PermissionsPerGroups e)
        {
            if (e == null)
            {
                SelectedPermission.peo_DataRole = string.Empty;
                SelectedPermission.peo_DataRoleID = 0;
            }
            else
            {
                SelectedPermission.peo_DataRole = e.peo_DataRole;
                SelectedPermission.peo_DataRoleID = e.peo_DataRoleID;
            }
        }

        public async Task InsertPermission()
        {
            await AddForm!.Validate();
            if (!AddForm.IsValid)
                return;

            SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 4,
            PermissionID:ID,
            UsersID: SelectedPermission.peo_DataRoleID
            );

            if (result.Result == 1)
            {
                SelectedPermission = new PermissionsPerGroups();
                await GetCombo();
                Toaster.Success(".", result.MSG);
                return;
            }
            Toaster.Error(".", result.MSG);
        }

        public async Task DeletePermission(int perID)
        {
           

            SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 5,
            PermissionID: perID
            );

            if (result.Result == 1)
            {
                await GetCombo();
                Toaster.Success(".", result.MSG);
                return;
            }
            Toaster.Error(".", result.MSG);
        }

    }
}
