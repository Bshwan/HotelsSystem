
namespace HotelsSystem.Pages.UserManagement;
public partial class UserList
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

    PagedResult<UserInfo> PaginatedItems = PagedResult<UserInfo>.EmptyPagedResult();
    // SortDirection sort = SortDirection.Descending;
    ClS_UserManagement mgmt = default!;
    ClS_Config config = default!;
    UserInfo FilterUser = new UserInfo();
    // UserInfo SelectedUser = new UserInfo();
    private MudTable<UserInfo>? table;
    SPResult? session;

    protected override async Task OnInitializedAsync()
    {
        session = await Protection.GetDecryptedSession(jSRuntime, DB,storage);
        mgmt = new ClS_UserManagement(DB, session);
        config = new ClS_Config(DB, session);
        // await GetPaginatedUsers();
    }
    private async Task<TableData<UserInfo>> GetPaginatedItems(TableState state)
    {
        PaginatedItems = await mgmt.UserList<UserInfo>(
            SelectPro: 1,
            PageNumber: state.Page + 1,
            PageSize: state.PageSize,
            UserTypeID: FilterUser.peo_UserTypeID,
            FullName: FilterUser.peo_UserName.ToEmptyOnNull(),
            DirectorateID: FilterUser.peo_DirectorateID,
            WorkPlaceID: FilterUser.peo_UserID,
            SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "peo_createdDate" : state.SortLabel,
            SortDirection: Util.ResolveSort(state.SortDirection));

        return new TableData<UserInfo>() { TotalItems = PaginatedItems.TotalItems, Items = PaginatedItems.Items };
    }
    async Task OnSearch(string e)
    {
        FilterUser.peo_UserName = e;
        await table!.ReloadServerData();
    }
}