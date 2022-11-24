
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
    protected IDialogService DialogService{get;set;}=default!;

    PagedResult<UserInfo> PaginatedUsers = PagedResult<UserInfo>.EmptyPagedResult();
    SortDirections sort = SortDirections.DESC;
    string SelectedColumnToSort = "peo_createdDate";
    ClS_UserManagement mgmt = default!;
    ClS_Config config = default!;
    UserInfo FilterUser = new UserInfo();

    protected override async Task OnInitializedAsync()
    {
        var session = await Protection.GetDecryptedSession(jSRuntime, DB);
        mgmt = new ClS_UserManagement(DB, session);
        config = new ClS_Config(DB, session);
        await GetPaginatedUsers();
    }

    async Task GetPaginatedUsers(int page = 1, string ColumnName = "")
    {
        if (!string.IsNullOrWhiteSpace(ColumnName))
        {
            if (sort == SortDirections.ASC)
                sort = SortDirections.DESC;
            else
                sort = SortDirections.ASC;

            SelectedColumnToSort = ColumnName;
        }

        PaginatedUsers = await mgmt.UserList<UserInfo>(
            SelectPro: 1,
            PageNumber: page,
            PageSize: config.RowNumber,
            UserTypeID: FilterUser.peo_UserTypeID,
            FullName: FilterUser.peo_UserName.ToEmptyOnNull(),
            DirectorateID: FilterUser.peo_DirectorateID,
            WorkPlaceID: FilterUser.peo_UserID,
            SortColumn: SelectedColumnToSort,
            SortDirection: sort.ToString());
    }
}