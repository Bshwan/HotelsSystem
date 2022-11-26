using Blazored.Modal.Services;
using HotelsSystem.Data;
using Microsoft.Identity.Client;

namespace HotelsSystem.Shared.Modals
{
    public partial class AddRole
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public int ID { get; set; } = 0;

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
        public GroupInfo SelectedGroup = new GroupInfo();

        private PermissionsPerGroups SelectedPermission = new PermissionsPerGroups();
        private IEnumerable<PermissionsPerGroups> combo = Enumerable.Empty<PermissionsPerGroups>();
        MudForm? NameForm;
        MudForm? PermissionForm;


        void Cancel() => MudDialog.Cancel();

        protected override async Task OnInitializedAsync()
        {
            var session = await Protection.GetDecryptedSession(jSRuntime, DB);
            config = new ClS_Config(DB, session);
            mgmt = new ClS_UserManagement(DB, session);
            await GetGroupByID(ID);

            await GetCombo();

        }
        private async Task GetCombo()
        {
            combo = await config.GetCMB<PermissionsPerGroups>(SelectPro: 4, ValID: ID);
        }

        private async Task GetGroupByID(int id)
        {
            SelectedGroup = await config.GetOneInfo<GroupInfo>(SelectPro: 1, ValID: id);
        }



        public async Task InsertUpdateGroup()
        {
            await NameForm!.Validate();
            if (!NameForm.IsValid)
                return;

            SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 1,
            PermissionID: SelectedGroup.group_ID,
            GroupName: SelectedGroup.group_Name.ToEmptyOnNull()
            );

            if (result.Result == 1)
            { 
                if(int.TryParse(result.LastValue , out int val )&& val > 0)
                {
                    await GetGroupByID(val);

                }
                else
                {
                    await GetGroupByID(SelectedGroup.group_ID);
                }
                //SelectedGroup = new GroupInfo();
                //MudDialog.Close(DialogResult.Ok(true));
                Toaster.Success(".", result.MSG);
                return;
            }
            Toaster.Error(".", result.MSG);
        }

        private async Task<IEnumerable<PermissionsPerGroups>> SearchPermission(string val)
        {
            return await Task.FromResult(combo.Where(x => x.peo_DataRole.ToEmptyOnNull().Contains(val.ToEmptyOnNull()) && x.HasRole == 0));
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
            await PermissionForm!.Validate();
            if (!PermissionForm.IsValid)
                return;

            SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 4,
            PermissionID: ID,
            UsersID: SelectedPermission.peo_DataRoleID);

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
