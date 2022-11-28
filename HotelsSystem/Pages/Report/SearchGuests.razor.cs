
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
    ClS_Config config = default!;
    GuestDetailsInfo Filter = new GuestDetailsInfo();
    SPResult? session;
    SearchCombos combos = new SearchCombos();



    protected override async Task OnInitializedAsync()
    {
        session = await Protection.GetDecryptedSession(jSRuntime, DB);
        report = new ClS_Reports(DB, session);
        config = new ClS_Config(DB, session);
        await GetCombos();
    }
    async Task GetCombos()
    {
        combos = await config.SearchCombos(SelectPro: 11);
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
    async Task OnSelectedNationalityChange(NationalityInfo e)
    {
        if (e == null)
        {
            Filter.NationalityID = 0;
            Filter.nat_Name = string.Empty;
            return;
        }
        Filter.NationalityID = e.nat_ID;
        Filter.nat_Name = e.nat_Name;
        await table!.ReloadServerData();

    }
    async Task OnSelectedGenderChange(GenderInfo e)
    {
        if (e == null)
        {
            Filter.GenderID = 0;
            Filter.gen_Name = string.Empty;
            return;
        }
        Filter.GenderID = e.gen_ID;
        Filter.gen_Name = e.gen_Name;
        await table!.ReloadServerData();
    }
    async Task<IEnumerable<DirectorateInfo>> SearchDirectoaret(string e)
    {
        return await Task.FromResult(combos.Directorates.SearchAll<DirectorateInfo>(e.ToEmptyOnNull(), "peo_DirectorateName"));
    }
    async Task<IEnumerable<HotelsInfo>> SearchHotels(string e)
    {
        return await Task.FromResult(combos.Hotels.SearchAll<HotelsInfo>(e.ToEmptyOnNull(), "htl_Name"));
    }
    async Task<IEnumerable<HotelRoomsInfo>> SearchRooms(string e)
    {
        return await Task.FromResult(combos.Rooms.SearchAll<HotelRoomsInfo>(e.ToEmptyOnNull(), "htr_Detail"));
    }
    async Task OnDirectoarateChange(DirectorateInfo e)
    {
        if (e == null)
        {
            Filter.DirectorateID = 0;
            Filter.peo_DirectorateName = string.Empty;
            combos.WorkingPoints = Enumerable.Empty<WorkingPointInfo>();
            return;
        }
        Filter.DirectorateID = e.peo_DirectorateID;
        Filter.peo_DirectorateName = e.peo_DirectorateName;
        await GetWorkpointByDirectorate(e.peo_DirectorateID);
    }
    async Task OnWorkpointChange(WorkingPointInfo e)
    {
        if (e == null)
        {
            Filter.WorkplaceID = 0;
            Filter.wp_workpointName = string.Empty;
            combos.Hotels = Enumerable.Empty<HotelsInfo>();
            combos.Rooms = Enumerable.Empty<HotelRoomsInfo>();
            return;
        }
        Filter.WorkplaceID = e.wp_ID;
        Filter.wp_workpointName = e.wp_workpointName;
        await GetHotelByWorkpoint(e.wp_ID);
    }
    async Task OnHotelChange(HotelsInfo e)
    {
        if (e == null)
        {
            Filter.HotelID = 0;
            Filter.htl_Name = string.Empty;
            combos.Hotels = Enumerable.Empty<HotelsInfo>();
            combos.Rooms = Enumerable.Empty<HotelRoomsInfo>();
            return;
        }
        Filter.HotelID = e.htl_ID;
        Filter.htl_Name = e.htl_Name;
        await GetRoomsByHotel(e.htl_ID);
    }

    async Task GetWorkpointByDirectorate(int id)
    {
        combos.WorkingPoints = await config.GetCMB<WorkingPointInfo>(SelectPro: 5, ValID: id);
    }
    async Task GetHotelByWorkpoint(int id)
    {
        combos.Hotels = await config.GetCMB<HotelsInfo>(SelectPro: 12, ValID: id);
    }
    async Task GetRoomsByHotel(int id)
    {
        combos.Rooms = await config.GetCMB<HotelRoomsInfo>(SelectPro: 13, ValID: id);
    }
}