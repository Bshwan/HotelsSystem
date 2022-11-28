namespace HotelsSystem.Shared.Modals;
public partial class SearchFilter
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter]
    public GuestDetailsInfo Filter { get; set; } = default!;
    [Parameter]
    public SearchCombos combos { get; set; } = default!;
    [Parameter]
    public ClS_Config config { get; set; } = default!;

    void OnSelectedNationalityChange(NationalityInfo e)
    {
        if (e == null)
        {
            Filter.NationalityID = 0;
            Filter.nat_Name = string.Empty;
            return;
        }
        Filter.NationalityID = e.nat_ID;
        Filter.nat_Name = e.nat_Name;

    }

    void OnSelectedGenderChange(GenderInfo e)
    {
        if (e == null)
        {
            Filter.GenderID = 0;
            Filter.gen_Name = string.Empty;
            return;
        }
        Filter.GenderID = e.gen_ID;
        Filter.gen_Name = e.gen_Name;
    }

    async Task<IEnumerable<DirectorateInfo>> SearchDirectoaret(string e)
    {
        return await Task.FromResult(combos.Directorates.SearchAll<DirectorateInfo>(e.ToEmptyOnNull(), "peo_DirectorateName"));
    }
    async Task<IEnumerable<WorkingPointInfo>> SearchWorkpoint(string e)
    {
        return await Task.FromResult(combos.WorkingPoints.SearchAll<WorkingPointInfo>(e.ToEmptyOnNull(), "wp_workpointName"));
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
        Filter.HotelID = 0;
        Filter.RoomID = 0;
        Filter.htl_Name = "";
        Filter.RoomName = "";
        Filter.WorkplaceID = 0;
        Filter.wp_workpointName = string.Empty;

        if (e == null)
        {
            Filter.DirectorateID = 0;
            Filter.peo_DirectorateName = string.Empty;
            combos.WorkingPoints = Enumerable.Empty<WorkingPointInfo>();
            combos.Hotels = Enumerable.Empty<HotelsInfo>();
            combos.Rooms = Enumerable.Empty<HotelRoomsInfo>();
            return;
        }
        Filter.DirectorateID = e.peo_DirectorateID;
        Filter.peo_DirectorateName = e.peo_DirectorateName;
        await GetWorkpointByDirectorate(e.peo_DirectorateID);
    }
    async Task OnWorkpointChange(WorkingPointInfo e)
    {
        Filter.HotelID = 0;
        Filter.RoomID = 0;
        Filter.htl_Name = "";
        Filter.RoomName = "";
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
        Filter.RoomID = 0;
        Filter.RoomName = string.Empty;
        if (e == null)
        {
            Filter.HotelID = 0;
            Filter.htl_Name = string.Empty;
            combos.Rooms = Enumerable.Empty<HotelRoomsInfo>();
            return;
        }
        Filter.HotelID = e.htl_ID;
        Filter.htl_Name = e.htl_Name;
        await GetRoomsByHotel(e.htl_ID);
    }
    void OnRoomChange(HotelRoomsInfo e)
    {
        if (e == null)
        {
            Filter.RoomID = 0;
            Filter.RoomName = string.Empty;
            return;
        }
        Filter.RoomID = e.htr_ID;
        Filter.RoomName = e.htr_Detail;
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
    void Submit() => MudDialog.Close(DialogResult.Ok(Filter));
    void Cancel() => MudDialog.Close(DialogResult.Ok(new GuestDetailsInfo()));
}