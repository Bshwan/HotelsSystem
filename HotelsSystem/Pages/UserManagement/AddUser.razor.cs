namespace HotelsSystem.Pages.UserManagement;
public partial class AddUser
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
    [Parameter]
    public string? HashID { get; set; }

    int UserID;
    UserInfo SelectedUser = new UserInfo();
    GroupInfo SelectedGroup = new GroupInfo();
    DataAccessPermissions SelectedPrivilage=new DataAccessPermissions();
    ClS_UserManagement mgmt = default!;
    ClS_Config config = default!;
    MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
    UserCombos combos = new UserCombos();
    MudForm? AddGroupForm;
    MudForm? AddPrivilageForm;
    MudForm? AddUserForm;

    protected override async Task OnParametersSetAsync()
    {
        UserID = Hasher.UnHash(HashID.ToEmptyOnNull());
        var session = await Protection.GetDecryptedSession(jSRuntime, DB);
        mgmt = new ClS_UserManagement(DB, session);
        config = new ClS_Config(DB, session);

        if (UserID > 0)
        {
            await GetUserByID();
            await GetCombos();
        }

    }
    async Task GetUserByID()
    {
        SelectedUser = await config.GetOneInfo<UserInfo>(SelectPro: 2, ValID: UserID);

        if (!SelectedUser.peo_UserPassword.IsStringNullOrWhiteSpace())
        {
            SelectedUser.peo_UserPassword = func.decr_pass(SelectedUser.peo_UserPassword);
        }
    }
    async Task GetCombos()
    {
        combos = await mgmt.UserCombos(SelectPro: 1, ValID: SelectedUser.peo_UserID);
    }
    async Task GetGroups()
    {
        combos.Groups = await config.GetCMB<GroupInfo>(SelectPro: 2, ValID: SelectedUser.peo_UserID);
    }
    async Task GetPrivilages()
    {
        combos.Permissions = await config.GetCMB<DataAccessPermissions>(SelectPro: 3, ValID: SelectedUser.peo_UserID);
    }
    async Task<IEnumerable<GroupInfo>> SearchGroups(string e)
    {
        return await Task.FromResult(combos.Groups.SearchAll<GroupInfo>(e, "group_Name").Where(x => x.HasRole == 0));
    }
    async Task<IEnumerable<DataAccessPermissions>> SearchPrivilages(string e)
    {
        return await Task.FromResult(combos.Permissions.SearchAll<DataAccessPermissions>(e, "dap_Name").Where(x => x.HasRole == 0));
    }
    async Task<IEnumerable<DirectorateInfo>> SearchDirectorate(string e)
    {
        return await Task.FromResult(combos.Directorates.SearchAll<DirectorateInfo>(e, "peo_DirectorateName"));
    }
    async Task<IEnumerable<WorkingPointInfo>> SearchWorkpoint(string e)
    {
        return await Task.FromResult(combos.WorkingPoints.SearchAll<WorkingPointInfo>(e, "wp_workpointName"));
    }
    async Task OnDirectorateChange(DirectorateInfo e)
    {
        SelectedUser.wp_workpointName = string.Empty;
        SelectedUser.peo_UserWorkPoint = 0;
        combos.WorkingPoints = Enumerable.Empty<WorkingPointInfo>();

        if (e == null)
        {
            SelectedUser.peo_DirectorateName = string.Empty;
            SelectedUser.peo_DirectorateID = 0;
            return;
        }

        SelectedUser.peo_DirectorateName = e.peo_DirectorateName;
        SelectedUser.peo_DirectorateID = e.peo_DirectorateID;

        combos.WorkingPoints = await config.GetCMB<WorkingPointInfo>(SelectPro: 5, ValID: e.peo_DirectorateID);
    }
    void OnWorkpointChange(WorkingPointInfo e)
    {
        if (e == null)
        {
            SelectedUser.wp_workpointName = string.Empty;
            SelectedUser.peo_UserWorkPoint = 0;
            return;
        }
        SelectedUser.wp_workpointName = e.wp_workpointName;
        SelectedUser.peo_UserWorkPoint = e.wp_ID;
    }
    void OnGroupChange(GroupInfo e)
    {
        if (e == null)
            SelectedGroup = new GroupInfo();
        else
            SelectedGroup = e;
    }
    void OnPrivilageChange(DataAccessPermissions e)
    {
        if (e == null)
            SelectedPrivilage=new DataAccessPermissions();
        else
            SelectedPrivilage = e;
    }
    async Task InsertUpdateUser()
    {
        await AddUserForm!.Validate();
        if (!AddUserForm.IsValid)
            return;

        SPResult result = await mgmt.InsertUpdateUser<SPResult>(
            SelectPro: 1,
            ValID: SelectedUser.peo_UserID,
            UserTypeID: SelectedUser.peo_UserTypeID,
            UserName: SelectedUser.peo_UserName.ToEmptyOnNull(),
            userFullName: SelectedUser.peo_userFullName.ToEmptyOnNull(),
            UserMobile: SelectedUser.peo_UserMobile.ToEmptyOnNull(),
            UserDirectorateID: SelectedUser.peo_DirectorateID,
            UserWorkPointID: SelectedUser.peo_UserWorkPoint,
            UserPassword: func.encr_pass(SelectedUser.peo_UserPassword),
            UserActive: SelectedUser.peo_UserActive,
            Language: SelectedUser.peo_Language);

        if (result.Result == 1)
        {
            if (int.TryParse(result.LastValue, out int val) && val > 0)
                nav.NavigateTo(Go.To(Routing.adduser, val));
            else
                await GetUserByID();

            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);

    }

    async Task InsertUpdateGroup()
    {
        await AddGroupForm!.Validate();
        if (!AddGroupForm.IsValid)
            return;

        SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 6,
            PermissionID: SelectedGroup.group_ID,
            UsersID: SelectedGroup.group_ID);

        if (result.Result == 1)
        {
            await GetGroups();
            SelectedGroup=new GroupInfo();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    async Task DeleteGroup(int id)
    {
        SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 7,
            PermissionID: id);

        if (result.Result == 1)
        {
            await GetGroups();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    async Task InsertUpdatePrivilage()
    {
        await AddPrivilageForm!.Validate();
        if (!AddPrivilageForm.IsValid)
            return;

        SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 2,
            PermissionID: SelectedPrivilage.dap_ID,
            UsersID: SelectedUser.peo_UserID);

        if (result.Result == 1)
        {
            await GetPrivilages();
            SelectedPrivilage=new DataAccessPermissions();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    async Task DeletePrivilage(int id)
    {
        SPResult result = await mgmt.InsertDeletePermissions<SPResult>(
            SelectPro: 3,
            PermissionID: id);

        if (result.Result == 1)
        {
            await GetPrivilages();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
}