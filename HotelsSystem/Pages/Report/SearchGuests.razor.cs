
using HotelsSystem.Pages.Hotels;
using Microsoft.Extensions.Localization;
using System.Reflection;

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
    [Inject]
        public ISessionStorageService storage { get; set; } = default!;

    PagedResult<GuestDetailsInfo> PaginatedItems = PagedResult<GuestDetailsInfo>.EmptyPagedResult();
    private MudTable<GuestDetailsInfo>? table;
    ClS_Reports? report;
    ClS_Config config = default!;
    GuestDetailsInfo Filter = new GuestDetailsInfo();
    SPResult? session;
    SearchCombos combos = new SearchCombos();



    protected override async Task OnInitializedAsync()
    {
        session = await Protection.GetDecryptedSession(jSRuntime, DB,storage);
        report = new ClS_Reports(DB, session);
        config = new ClS_Config(DB, session);
        await GetCombos();
    }
    async Task GetCombos()
    {
        combos = await config.SearchCombos(SelectPro: 11);
    }
    async Task<IEnumerable<GuestDetailsInfo>> SearchGuestsCombo(string? e)
    {
        return await Task.FromResult(await config.GetCMB<GuestDetailsInfo>(SelectPro: 16, ValueName: e.ToEmptyOnNull()));
    }
    async Task OnGuestSelected(GuestDetailsInfo e)
    {
        if (e == null)
        {
            Filter.GD_ID = 0;
            Filter.GD_Fullname = "";
        }
        else
        {
            Filter.GD_ID = e.GD_ID;
            Filter.GD_Fullname = e.GD_Fullname;
        }
        await table.ReloadServerData();
    }
    private async Task<TableData<GuestDetailsInfo>> GetPaginatedItems(TableState state)
    {
        if (Filter.GD_Fullname.IsStringNullOrWhiteSpace() && Filter.DirectorateID <= 0 && Filter.GD_Mobile.IsStringNullOrWhiteSpace() && Filter.GD_MotherName.IsStringNullOrWhiteSpace()
        && Filter.WorkplaceID <= 0 && Filter.HotelID <= 0 && Filter.RoomID <= 0 && Filter.GenderID <= 0 && Filter.NationalityID <= 0 && !Filter.FromCheckInDate.HasValue && Filter.GD_ID <= 0
        && !Filter.FromCheckOutDate.HasValue && !Filter.ToCheckInDate.HasValue && !Filter.ToCheckOutDate.HasValue)
        {
            PaginatedItems = PagedResult<GuestDetailsInfo>.EmptyPagedResult();
            return new TableData<GuestDetailsInfo>() { TotalItems = 0, Items = Enumerable.Empty<GuestDetailsInfo>() };
        }
        
        PaginatedItems = await report!.SearchList<GuestDetailsInfo>(
            SelectPro: 1,
            PageNumber: state.Page + 1,
            PageSize: state.PageSize,
            SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "GD_Fullname" : state.SortLabel,
            SortDirection: Util.ResolveSort(state.SortDirection),
            FullName: Filter.GD_Fullname.ToEmptyOnNull(),
            DirectorateID: Filter.DirectorateID,
            GuestID:Filter.GD_ID,
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
    void ShowGuestDocumentsModal(GuestDetailsInfo guest)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
        };
        var parameters = new DialogParameters();
        parameters.Add("config", config);
        parameters.Add("GuestID", guest.GD_ID);
        var modal = DialogService.Show<GuestDocumentsModal>(L["files"], parameters, options);
        //var res = await modal.Result;
        // StateHasChanged();
    }

    private async Task OpenFilterModal()
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
        };
        var param = new DialogParameters();
        param.Add("Filter", Filter);
        param.Add("combos", combos);
        param.Add("config", config);
        var modal = DialogService.Show<SearchFilter>(L["filter"], parameters: param, options);
        var res = await modal.Result;
        if (res.Cancelled)
            return;
        if (res.Data is GuestDetailsInfo filter)
        {
            Filter = filter;
            await table!.ReloadServerData();
        }
    }
    async Task OnSearch(string e)
    {
        Filter.htl_Name = e;
        await table!.ReloadServerData();
    }
    // async Task OnSelectedNationalityChange(NationalityInfo e)
    // {
    //     if (e == null)
    //     {
    //         Filter.NationalityID = 0;
    //         Filter.nat_Name = string.Empty;
    //         return;
    //     }
    //     Filter.NationalityID = e.nat_ID;
    //     Filter.nat_Name = e.nat_Name;
    //     await table!.ReloadServerData();

    // }
    // async Task OnSelectedGenderChange(GenderInfo e)
    // {
    //     if (e == null)
    //     {
    //         Filter.GenderID = 0;
    //         Filter.gen_Name = string.Empty;
    //         return;
    //     }
    //     Filter.GenderID = e.gen_ID;
    //     Filter.gen_Name = e.gen_Name;
    //     await table!.ReloadServerData();
    // }
    // async Task<IEnumerable<DirectorateInfo>> SearchDirectoaret(string e)
    // {
    //     return await Task.FromResult(combos.Directorates.SearchAll<DirectorateInfo>(e.ToEmptyOnNull(), "peo_DirectorateName"));
    // }
    // async Task<IEnumerable<WorkingPointInfo>> SearchWorkpoint(string e)
    // {
    //     return await Task.FromResult(combos.WorkingPoints.SearchAll<WorkingPointInfo>(e.ToEmptyOnNull(), "wp_workpointName"));
    // }
    // async Task<IEnumerable<HotelsInfo>> SearchHotels(string e)
    // {
    //     return await Task.FromResult(combos.Hotels.SearchAll<HotelsInfo>(e.ToEmptyOnNull(), "htl_Name"));
    // }
    // async Task<IEnumerable<HotelRoomsInfo>> SearchRooms(string e)
    // {
    //     return await Task.FromResult(combos.Rooms.SearchAll<HotelRoomsInfo>(e.ToEmptyOnNull(), "htr_Detail"));
    // }
    // async Task OnDirectoarateChange(DirectorateInfo e)
    // {
    //     if (e == null)
    //     {
    //         Filter.DirectorateID = 0;
    //         Filter.peo_DirectorateName = string.Empty;
    //         combos.WorkingPoints = Enumerable.Empty<WorkingPointInfo>();
    //         return;
    //     }
    //     Filter.DirectorateID = e.peo_DirectorateID;
    //     Filter.peo_DirectorateName = e.peo_DirectorateName;
    //     await GetWorkpointByDirectorate(e.peo_DirectorateID);
    // }
    // async Task OnWorkpointChange(WorkingPointInfo e)
    // {
    //     if (e == null)
    //     {
    //         Filter.WorkplaceID = 0;
    //         Filter.wp_workpointName = string.Empty;
    //         combos.Hotels = Enumerable.Empty<HotelsInfo>();
    //         combos.Rooms = Enumerable.Empty<HotelRoomsInfo>();
    //         return;
    //     }
    //     Filter.WorkplaceID = e.wp_ID;
    //     Filter.wp_workpointName = e.wp_workpointName;
    //     await GetHotelByWorkpoint(e.wp_ID);
    // }
    // async Task OnHotelChange(HotelsInfo e)
    // {
    //     if (e == null)
    //     {
    //         Filter.HotelID = 0;
    //         Filter.htl_Name = string.Empty;
    //         combos.Hotels = Enumerable.Empty<HotelsInfo>();
    //         combos.Rooms = Enumerable.Empty<HotelRoomsInfo>();
    //         return;
    //     }
    //     Filter.HotelID = e.htl_ID;
    //     Filter.htl_Name = e.htl_Name;
    //     await GetRoomsByHotel(e.htl_ID);
    // }
    // void OnRoomChange(HotelRoomsInfo e)
    // {
    //     if (e == null)
    //     {
    //         Filter.RoomID = 0;
    //         Filter.RoomName = string.Empty;
    //         return;
    //     }
    //     Filter.RoomID = e.htr_ID;
    //     Filter.RoomName = e.htr_Detail;
    // }

    // async Task GetWorkpointByDirectorate(int id)
    // {
    //     combos.WorkingPoints = await config.GetCMB<WorkingPointInfo>(SelectPro: 5, ValID: id);
    // }
    // async Task GetHotelByWorkpoint(int id)
    // {
    //     combos.Hotels = await config.GetCMB<HotelsInfo>(SelectPro: 12, ValID: id);
    // }
    // async Task GetRoomsByHotel(int id)
    // {
    //     combos.Rooms = await config.GetCMB<HotelRoomsInfo>(SelectPro: 13, ValID: id);
    // }
}