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
}
