namespace HotelsSystem.BL;

public class ClS_Hotels
{
    private ISqlDataAccess _db { get; set; }

    // private SPResult SessionResult;
    int SessionValue=0;

    public ClS_Hotels(ISqlDataAccess db, SPResult session)
    {
        SessionValue=session.Result;
        _db = db;
    }
    public async Task<PagedResult<T>> HotelList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10, int HotelTypes = 0, string FullName = "", int DirectorateID = 0, int WorkPlaceID = 0, string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("Pro_HotelList", new { Select = SelectPro, HotelTypes = HotelTypes, FullName = FullName, DirectorateID = DirectorateID, WorkPlaceID = WorkPlaceID, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize);
    }

    public async Task<T> InsertUpdateHotels<T>(int SelectPro = 0, int ValID = 0, int HotelTypeID = 0, string HotelName = "", string HotelAddress = "", int StarNumber = 0,int NumberOfRooms=0, int DirectorateID = 0, int WorkPointID = 0, string Note = "",decimal Price=0)
    {
        return await _db.SaveData<T, dynamic>("Pro_InsertUpdateHotels", new { select = SelectPro, ID = ValID, HotelTypeID = HotelTypeID, HotelName = HotelName, HotelAddress = HotelAddress, StarNumber = StarNumber, NumberOfRooms= NumberOfRooms, DirectorateID = DirectorateID,Price=Price, WorkPointID = WorkPointID, Note = Note, EntryBy = SessionValue });
    }

    public async Task<T> HotelInsertUpdateHotels<T>(int SelectPro = 0, int ValID = 0,  string RoomName = "", int RoomType = 0, int FloorID = 0, string FloorName = "", int NumberOfBed = 0,string Note = "")
    {
        return await _db.SaveData<T, dynamic>("HTPro_InsertUpdateHotels", new { select = SelectPro, ID = ValID, RoomName = RoomName, RoomType = RoomType, FloorID = FloorID, FloorName = FloorName, NumberOfBed = NumberOfBed, Note = Note, EntryBy = SessionValue });
    }
    public async Task<AddHotelCombos> AddHotelCombos(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
    {
        var result = await _db.GetMultiple<HotelTypesComboBox, DirectorateInfo,HotelRoomsTypesInfo,HotelFloorInfo>(
                "Pro_GetCMB",
                new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
        return new AddHotelCombos()
        {
            HotelTypes = result.Item1,
            Directorates = result.Item2,
            RoomTypes=result.Item3,
            FloorTypes=result.Item4
        };
    }
    public async Task<AddHotelAddUserCombos> AddHotelAddUserCombos(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
    {
        var result = await _db.GetMultiple<HotelUserTypeComboBox, LanguageInfo>(
                "Pro_GetCMB",
                new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
        return new AddHotelAddUserCombos()
        {
           HotelUserTypes  = result.Item1,
            Languages = result.Item2,
        
        };
    }
}
