namespace HotelsSystem.BL;

public class ClS_UserManagement
{
    private ISqlDataAccess _db { get; set; }

    int SessionValue=0;

    public ClS_UserManagement(ISqlDataAccess db, SPResult session)
    {
        SessionValue=session.Result;
        _db = db;
    }

    public async Task<T> InsertUpdateUser<T>(int SelectPro = 0, int ValID = 0, string UserName = "", string userFullName = "", string UserMobile="", int UserTypeID=0,int UserDirectorateID=0,int UserWorkPointID=0, string UserPassword = "", bool UserActive = true,  string Note = "", int Language = 0)
    {
        return await _db.SaveData<T, dynamic>("Pro_InsertUpdateUser", new { select = SelectPro, ID = ValID, UserName = UserName, userFullName = userFullName, UserMobile= UserMobile, UserTypeID= UserTypeID, UserDirectorateID= UserDirectorateID, UserWorkPointID= UserWorkPointID, UserPassword = UserPassword, UserActive = UserActive,  Note = Note, Language = Language, EntryBy = SessionValue });
    }

    public async Task<T> InsertDeletePermissions<T>(int SelectPro = 0, string PermissionID = "", int UsersID = 0, string GroupName = "")
    {
        return await _db.SaveData<T, dynamic>("Pro_InsertDeletePermissions", new { Type = SelectPro, PermissionID = PermissionID, UserID = UsersID, GroupName = GroupName, EntryBy = SessionValue });
    }

    public async Task<T> Login<T>(int SelectPro = 0, string UserName = "", string UserPass = "")
    {
        return await _db.GetOneInfo<T, dynamic>("pro_Login", new { username = UserName, password = UserPass, Select = SelectPro, UserID = SessionValue });
    }
    public async Task<PagedResult<T>> UserList<T>(int SelectPro = 1, int PageNumber = 1, int PageSize = 10,int UserTypeID=0, string FullName = "", int DirectorateID = 0, int WorkPlaceID = 0, string SortColumn = "", string SortDirection = "Asc")
    {
        return await _db.GetGridResult<T, dynamic>("Pro_GridUserList", new { Select = SelectPro, UserTypeID= UserTypeID, FullName = FullName, DirectorateID = DirectorateID, WorkPlaceID = WorkPlaceID, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize);
    }

    public async Task<T> HotelInsertUpdateUser<T>(int SelectPro = 0, int ValID = 0, string UserName = "", string userFullName = "", string UserMobile = "", int UserTypeID = 0,  string UserPassword = "", bool UserActive = true, string Note = "", int Language = 0)
    {
        return await _db.SaveData<T, dynamic>("HTPro_InsertUpdateUser", new { select = SelectPro, ID = ValID, UserName = UserName, userFullName = userFullName, UserMobile = UserMobile, UserTypeID = UserTypeID, UserPassword = UserPassword, UserActive = UserActive, Note = Note, Language = Language, CreateBy = SessionValue });
    }

    public async Task<UserCombos> UserCombos(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
    {
        var result = await _db.GetMultiple<GroupInfo, DataAccessPermissions, LanguageInfo,DirectorateInfo,UserTypesInfo>(
                "Pro_GetCMB",
                new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
        return new UserCombos()
        {
            Groups = result.Item1,
            Permissions = result.Item2,
            Languages = result.Item3,
            Directorates=result.Item4,
            UserTypes=result.Item5
        };
    }
    public async Task<WorkPointCombo> WorkPointComboBox(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
    {
        var result = await _db.GetMultiple<DirectorateInfo, UserInfo>(
                "Pro_GetCMB",
                new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
        return new WorkPointCombo()
        {
            Directorates = result.Item1,
            UserCombo = result.Item2,
        
        };
    }
}