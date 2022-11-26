namespace HotelsSystem.Pages.UserManagement;
using HotelsSystem.Data;
using System.Reflection;

namespace HotelsSystem.Pages.Admins;
public partial class Roles
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
    ClS_UserManagement mgmt = default!;


    private PagedResult<GroupInfo> PaginatedGroups = PagedResult<GroupInfo>.EmptyPagedResult();
    // private string SelectedColumnToSort = "group_ID";
    // private SortDirections sort = SortDirections.ASC;
    public GroupInfo SelectedGroup = new GroupInfo();
    public GroupInfo Filter = new GroupInfo();

    private MudTable<GroupInfo>? tableRef = new MudTable<GroupInfo>();

    private ClS_Config config = default!;
    MudForm? AddForm;


    protected override async Task OnInitializedAsync()
    {
        var session = await Protection.GetDecryptedSession(jSRuntime, DB);
        config = new ClS_Config(DB, session);
        mgmt = new ClS_UserManagement(DB, session);

    }


    private async Task<TableData<GroupInfo>> GetPaginatedGroups(TableState state)
    {
        PaginatedGroups = await config.GetGridPaging<GroupInfo>(
                SelectPro: 1,
                PageNumber: state.Page + 1,
                PageSize: state.PageSize,
                SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "group_ID" : state.SortLabel,
                Search: Filter.group_Name.ToEmptyOnNull(),
                SortDirection: Util.ResolveSort(state.SortDirection));

        return new TableData<GroupInfo>() { TotalItems = PaginatedGroups.TotalItems, Items = PaginatedGroups.Items };
    }

    private async void AddRoleModal(int ID)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = false,
            Position = DialogPosition.TopCenter,
            FullWidth = true,
            MaxWidth = MaxWidth.Small,


        };
        var parms = new DialogParameters();
        parms.Add("ID", ID);
        var modal = DialogService.Show<AddRole>("Add Role", parms, options);

        var ModalResult = await modal.Result;

        if ( tableRef != null)
           await tableRef.ReloadServerData();
    }
}