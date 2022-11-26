namespace HotelsSystem.BL;

public class ClS_Guests
{
    private ISqlDataAccess _db { get; set; }

    int SessionValue = 0;

    public ClS_Guests(ISqlDataAccess db, SPResult session)
    {
        SessionValue = session.Result;
        _db = db;
    }

    public async Task<T> InsertUpdateGuest<T>(int SelectPro = 0, int ValID = 0,int GusetMasterID=0,int RoomID=0, string FullName = "", string SurName = "", string MotherName = "", string Mobile = "",string IDNumber="",int Gender=0, string DateOfBirth = "", int NationalityID = 0,  string Note = "")
    {
        return await _db.SaveData<T, dynamic>("HTPro_InsertUpdateGuest", new { select = SelectPro, ID = ValID, GusetMasterID= GusetMasterID, RoomID= RoomID, FullName = FullName, SurName = SurName, MotherName = MotherName, Mobile = Mobile, IDNumber= IDNumber, Gender= Gender, DateOfBirth = DateOfBirth, NationalityID = NationalityID,  Note = Note, EntryBy = SessionValue });
    }

    public async Task<PagedResult<T>> GuestList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10, int RoomID = 0,int FloorID=0, string FullName = "", int Nationality = 0, string FromDate = "",string ToDate="", string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("HTPro_GuestDetailList", new { Select = SelectPro, RoomID = RoomID, FloorID= FloorID , FullName = FullName, Nationality = Nationality, FromDate = FromDate, ToDate= ToDate, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize);
    }
}
