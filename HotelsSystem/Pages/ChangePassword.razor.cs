namespace HotelsSystem.Pages;

public partial class ChangePassword
{
    [Inject]
    public ISqlDataAccess DB { get; set; } = default!;

    [Inject]
    public NavigationManager nav { get; set; } = default!;

    [Inject]
    public IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    public IToaster Toaster { get; set; } = default!;



    private ClS_UserManagement mgmt = default!;
    //private LoginSystemOptions session = new LoginSystemOptions();
    private MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
    ChangePasswordInfo password = new();

    protected override async Task OnInitializedAsync()
    {
        var session = await Protection.GetDecryptedSession(JSRuntime, DB);


        mgmt = new ClS_UserManagement(DB, session);

    }

    private async Task InsertUpdateChangePassword()
    {
        SPResult result = await mgmt.InsertUpdateUser<SPResult>(
        SelectPro: 2,
        userFullName: func.encr_pass(password.OldPassword),
       UserPassword: func.encr_pass(password.NewPassword));

        if (result.Result == 1)
        {
            Toaster.Success(".", result.MSG);
            await JSRuntime.InvokeVoidAsync("blazorExtensions.delete_cookie",Util.CookieName);
            nav.NavigateTo("");
            return;
        }else
        Toaster.Error(".", result.MSG);

    }

}
