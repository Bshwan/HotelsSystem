using HotelsSystem.Pages.Configs;

namespace HotelsSystem.BL;

public class ClS_Reports
{
    private ISqlDataAccess _db { get; set; }

    int SessionValue = 0;

    public ClS_Reports(ISqlDataAccess db, SPResult session)
    {
        SessionValue = session.Result;
        _db = db;
    }
    public async Task<PagedResult<T>> SearchList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10,string FullName="",string Mobile="",string MotherName="", int GuestID=0, int  DirectorateID=0,int WorkPlaceID=0,int HotelID=0,  int RoomID = 0, int GenderID = 0,  int NationalityID = 0, string FromCheckInDate = "", string ToCheckInDate = "",string FromCheckOutDate="",string ToCheckOutDate="", string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("Pro_SearchList", new { Select = SelectPro, FullName= FullName, Mobile= Mobile, MotherName= MotherName, DirectorateID = DirectorateID , GuestID= GuestID, WorkPlaceID = WorkPlaceID, HotelID= HotelID, RoomID = RoomID, GenderID = GenderID, NationalityID = NationalityID, FromCheckInDate = FromCheckInDate, ToCheckInDate = ToCheckInDate, FromCheckOutDate= FromCheckOutDate, ToCheckOutDate= ToCheckOutDate, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize);
    }
    public async Task<PagedResult<T>> Pro_ReportActionLog<T>(int SelectPro = 0, int UserType = 0, string? UserID = "", int ActionType = 0, string? TableName = "", string? FieldValue = "", string FromDate = "", string ToDate = "", int PageNumber = 1, int PageSize = 10, string? SortColumn = "", string? SortDirection = "", int ExportToExcel = 0)
    {

        var objParameters = new { Select = SelectPro, UserType = ClS_Config.ReturnEmptyOnZero(UserType), UserID = UserID, ActionType = ClS_Config.ReturnEmptyOnZero(ActionType), TableName = TableName, FieldValue = FieldValue, FromDate = FromDate, ToDate = ToDate, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue };
        if (ExportToExcel == 0)
        {
            return await _db.GetGridResult<T, dynamic>("Pro_ReportActionLog", objParameters, PageNumber: PageNumber, PageSize: PageSize);
        }
        PageSize = Util.DefaultExportSize;
        objParameters = new { Select = SelectPro, UserType = ClS_Config.ReturnEmptyOnZero(UserType), UserID = UserID, ActionType = ClS_Config.ReturnEmptyOnZero(ActionType), TableName = TableName, FieldValue = FieldValue, FromDate = FromDate, ToDate = ToDate, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue };
        PagedResult<T> Result = await _db.GetGridResult<T, dynamic>("Pro_ReportActionLog", objParameters, PageNumber: PageNumber, PageSize: PageSize);
        int totalPages = Result.TotalItems / PageSize;
        if (Result.TotalItems % PageSize > 0)
        {
            totalPages++;
        }
        for (int i = 2; i <= totalPages; i++)
        {
            PageNumber = PageNumber + 1; Result.Items = Result.Items.Concat((await _db.GetGridResult<T, dynamic>("Pro_ReportActionLog", objParameters, PageNumber: PageNumber, PageSize: PageSize)).Items);
        }
        return Result;
    }


}
