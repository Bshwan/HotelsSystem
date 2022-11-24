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
    ClS_UserManagement mgmt = default!;
    ClS_Config config = default!;
    MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
    UserCombos combos=new UserCombos();

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
    async Task GetCombos(){
        combos=await mgmt.UserCombos(SelectPro:1,ValID:SelectedUser.peo_UserID);
        // System.Console.WriteLine(Util.SelectByID<LanguageInfo>(SelectedUser.peo_Language,"lang_ID","lang_Name",combos.Languages).lang_Name);
        // var c=Util.SelectByID<LanguageInfo>(1,"lang_ID","lang_Name",combos.Languages);
        // System.Console.WriteLine(c.Count());
        // System.Console.WriteLine(c.First().lang_Name);
    }
}