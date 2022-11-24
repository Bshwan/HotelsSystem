namespace HotelsSystem.BL;

public class ClS_UserManagement
{
    private ISqlDataAccess _db { get; set; }

    private SPResult SessionResult;

    public ClS_UserManagement(ISqlDataAccess db, SPResult session)
    {
        SessionResult = session;
        _db = db;
    }

    public async Task<T> InsertUpdateUser<T>(int SelectPro = 0, int ValID = 0, string UserName = "", string userFullName = "", string UserMobile="", int UserTypeID=0,int UserDirectorateID=0,int UserWorkPointID=0, string UserPassword = "", bool UserActive = true,  string Note = "", int Language = 0)
    {
        return await _db.SaveData<T, dynamic>("pro_InsertAndUpdateUsers", new { select = SelectPro, ID = ValID, UserName = UserName, userFullName = userFullName, UserMobile= UserMobile, UserTypeID= UserTypeID, UserDirectorateID= UserDirectorateID, UserWorkPointID= UserWorkPointID, UserPassword = UserPassword, UserActive = UserActive,  Note = Note, Language = Language, CreateBy = SessionResult.Result });
    }

    public async Task<T> InsertDeletePermissions<T>(int SelectPro = 0, string PermissionID = "", int UsersID = 0, string GroupName = "")
    {
        return await _db.SaveData<T, dynamic>("Pro_InsertDeletePermissions", new { Type = SelectPro, PermissionID = PermissionID, UserID = UsersID, GroupName = GroupName, EntryBy = SessionResult.Result });
    }

    public async Task<T> Login<T>(int SelectPro = 0, string UserName = "", string UserPass = "")
    {
        return await _db.GetOneInfo<T, dynamic>("pro_Login", new { username = UserName, password = UserPass, Select = SelectPro, UserID = SessionResult.Result });
    }
    public async Task<PagedResult<T>> UserList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10,int UserTypeID=0, string FullName = "", int DirectorateID = 0, int WorkPlaceID = 0, string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("Pro_GridUserList", SessionResult.CNSTR.ToEmptyOnNull(), new { Select = SelectPro, UserTypeID= UserTypeID, FullName = FullName, DirectorateID = DirectorateID, WorkPlaceID = WorkPlaceID, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionResult.Result }, PageNumber: PageNumber, PageSize: PageSize);
    }

    // public async Task<LoginSystemOptions> Login(int SelectPro = 0, string UserName = "", string UserPass = "")
    // {
    //     var result = await _db.GetMultiple<SPResult, ConfigData, SystemOptions,UserSystemOptions>(
    //             "pro_Login",
    //             SessionResult.CNSTR.ToEmptyOnNull(),
    //             new { username = UserName, password = UserPass, Select = SelectPro, UserID = SessionResult.Result });
    //     return new LoginSystemOptions()
    //     {
    //         SPResult = result.Item1.Any() ? result.Item1.First() : new SPResult(),
    //         ConfigData = result.Item2.Any() ? result.Item2 : Enumerable.Empty<ConfigData>(),
    //         SystemOptions = result.Item3,
    //         UserOptions=result.Item4
    //     };
    //     // return await _db.Login<dynamic>("pro_Login", SessionResult.CNSTR.ToEmptyOnNull(), new { username = UserName, password = UserPass, Select = SelectPro, UserID = SessionResult.Result });
    // }
}