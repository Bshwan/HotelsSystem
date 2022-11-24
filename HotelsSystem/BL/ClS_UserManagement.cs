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

    public async Task<T> InsertUpdateUser<T>(int SelectPro = 0, int Code = 0, string Name = "", string FullName = "", string Password = "", bool Active = true, string Phone = "", string Note = "", int Language = 0, int WorkingPlace = 0, int PinCode = 0, string SecKey = "", int Delegate = 0, decimal SellDiscountRatio = 0, int WPWarehouse = 0, string CompanyName = "", string CompanyJob = "", string CompanyAddress = "", string CompanyEmail = "", string CompanyWebsite = "", string CompanyPhoneOne = "", string CompanyPhoneTwo = "", string CompanyPhoneThree = "", string CompanyPhoneFour = "", byte[]? RightLogo = null, byte[]? LeftLogo = null, byte[]? CenterLogo = null, byte[]? TitleLogo = null)
    {
        return await _db.SaveData<T, dynamic>("pro_InsertAndUpdateUsers", new { select = SelectPro, Code = Code, Name = Name, FullName = FullName, Password = Password, Active = Active, Phone = Phone, Note = Note, Language = Language, WorkingPlace = WorkingPlace, PinCode = PinCode, SecKey = SecKey, WPWarehouse = WPWarehouse, CompanyName = CompanyName, CompanyJob = CompanyJob, CompanyAddress = CompanyAddress, CompanyEmail = CompanyEmail, CompanyWebsite = CompanyWebsite, CompanyPhoneOne = CompanyPhoneOne, CompanyPhoneTwo = CompanyPhoneTwo, CompanyPhoneThree = CompanyPhoneThree, CompanyPhoneFour = CompanyPhoneFour, Delegate = Delegate, SellDiscountRatio = SellDiscountRatio, RightLogo = RightLogo, LeftLogo = LeftLogo, CenterLogo = CenterLogo, TitleLogo = TitleLogo, CreateBy = SessionResult.Result });
    }

    public async Task<T> InsertDeletePermissions<T>(int SelectPro = 0, string PermissionID = "", int UsersID = 0, string GroupName = "")
    {
        return await _db.SaveData<T, dynamic>("Pro_InsertDeletePermissions", new { Type = SelectPro, PermissionID = PermissionID, UserID = UsersID, GroupName = GroupName, EntryBy = SessionResult.Result });
    }

    public async Task<T> Login<T>(int SelectPro = 0, string UserName = "", string UserPass = "")
    {
        return await _db.GetOneInfo<T, dynamic>("pro_Login", new { username = UserName, password = UserPass, Select = SelectPro, UserID = SessionResult.Result });
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