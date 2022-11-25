namespace HotelsSystem.BL;

public class ClS_Hotels
{
    private ISqlDataAccess _db { get; set; }

    private SPResult SessionResult;

    public ClS_Hotels(ISqlDataAccess db, SPResult session)
    {
        SessionResult = session;
        _db = db;
    }
    public async Task<PagedResult<T>> HotelList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10, int HotelTypes = 0, string FullName = "", int DirectorateID = 0, int WorkPlaceID = 0, string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("Pro_HotelList", new { Select = SelectPro, HotelTypes = HotelTypes, FullName = FullName, DirectorateID = DirectorateID, WorkPlaceID = WorkPlaceID, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionResult.Result }, PageNumber: PageNumber, PageSize: PageSize);
    }

    public async Task<T> InsertUpdateHotels<T>(int SelectPro = 0, int ValID = 0, int HotelTypeID = 0, string HotelName = "", string HotelAddress = "", int StarNumber = 0,int NumberOfRooms=0, int DirectorateID = 0, int WorkPointID = 0, string Note = "")
    {
        return await _db.SaveData<T, dynamic>("Pro_InsertUpdateHotels", new { select = SelectPro, ID = ValID, HotelTypeID = HotelTypeID, HotelName = HotelName, HotelAddress = HotelAddress, StarNumber = StarNumber, NumberOfRooms= NumberOfRooms, DirectorateID = DirectorateID, WorkPointID = WorkPointID, Note = Note, EntryBy = SessionResult.Result });
    }

    public async Task<T> HotelInsertUpdateHotels<T>(int SelectPro = 0, int ValID = 0,  string RoomName = "", int RoomType = 0, int FloorID = 0, string FloorName = "", int NumberOfBed = 0,string Note = "")
    {
        return await _db.SaveData<T, dynamic>("HTPro_InsertUpdateHotels", new { select = SelectPro, ID = ValID, RoomName = RoomName, RoomType = RoomType, FloorID = FloorID, FloorName = FloorName, NumberOfBed = NumberOfBed, Note = Note, EntryBy = SessionResult.Result });
    }
}
