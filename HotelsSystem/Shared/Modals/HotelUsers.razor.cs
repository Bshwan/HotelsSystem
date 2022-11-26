namespace HotelsSystem.Shared.Modals;
public partial class HotelUsers
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter]
    public ClS_Config config { get; set; } = default!;
    [Parameter]
    public ClS_Hotels hotel { get; set; } = default!;
    [Parameter]
    public int HotelID { get; set; }
    [Inject]
    protected IToaster Toaster { get; set; } = default!;

    PagedResult<HotelUsersInfo> PaginatedItems=PagedResult<HotelUsersInfo>.EmptyPagedResult();
    string Search="";
    MudTable<HotelUsersInfo>? table;
    private async Task<TableData<HotelUsersInfo>> GetPaginatedItems(TableState state)
    {
        PaginatedItems = await config.GetGridPaging<HotelUsersInfo>(
            SelectPro: 4,
            ValID:HotelID,
            PageNumber: state.Page + 1,
            PageSize: state.PageSize,
            Search:Search.ToEmptyOnNull(),
            SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "htlus_Name" : state.SortLabel,
            SortDirection: Util.ResolveSort(state.SortDirection));

        return new TableData<HotelUsersInfo>() { TotalItems = PaginatedItems.TotalItems, Items = PaginatedItems.Items };
    }
    async Task OnSearch(string e)
    {
        Search = e;
        await table!.ReloadServerData();
    }
}