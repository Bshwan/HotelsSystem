
using Microsoft.Extensions.Localization;

namespace HotelsSystem.Pages.Report;
public partial class SearchGuests
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
    protected IStringLocalizer<App> L { get; set; } = default!;
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    PagedResult<GuestDetailsInfo> PaginatedItems = PagedResult<GuestDetailsInfo>.EmptyPagedResult();
    private MudTable<GuestDetailsInfo>? table;
    ClS_Reports? report;
    GuestDetailsInfo Filter = new GuestDetailsInfo();



    protected override async Task OnInitializedAsync()
    {
        var session = await Protection.GetDecryptedSession(jSRuntime, DB);
        report = new ClS_Reports(DB, session);
    }
    private async Task<TableData<GuestDetailsInfo>> GetPaginatedItems(TableState state)
    {
        PaginatedItems = await report!.SearchList<GuestDetailsInfo>(
            SelectPro: 1,
            PageNumber: state.Page + 1,
            PageSize: state.PageSize,
            SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "GD_Fullname" : state.SortLabel,
            SortDirection: Util.ResolveSort(state.SortDirection),
            FullName: Filter.GD_Fullname.ToEmptyOnNull(),
            DirectorateID: Filter.DirectorateID,
            Mobile: Filter.GD_Mobile.ToEmptyOnNull(),
            MotherName: Filter.GD_MotherName.ToEmptyOnNull(),
            WorkPlaceID: Filter.WorkplaceID,
            HotelID: Filter.HotelID,
            RoomID: Filter.RoomID,
            GenderID: Filter.GenderID,
            NationalityID: Filter.NationalityID,
            FromCheckInDate: Filter.FromCheckInDate.ToyyyyMMddElseEmpty(),
            ToCheckInDate: Filter.ToCheckInDate.ToyyyyMMddElseEmpty(),
            FromCheckOutDate: Filter.FromCheckOutDate.ToyyyyMMddElseEmpty(),
            ToCheckOutDate: Filter.ToCheckOutDate.ToyyyyMMddElseEmpty());

        return new TableData<GuestDetailsInfo>() { TotalItems = PaginatedItems.TotalItems, Items = PaginatedItems.Items };
    }
    async Task OnSearch(string e)
    {
        Filter.htl_Name = e;
        await table!.ReloadServerData();
    }
    private void OpenFilterModal()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
        };
        DialogService.Show<SearchFilter>(L["users"], options);
    }
}