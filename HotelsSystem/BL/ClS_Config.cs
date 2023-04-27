namespace HotelsSystem.BL.Classes
{
    public class ClS_Config
    {
        public static int debounce = 700;
        public int RowNumber { get; set; } = 10;
        public int RowNumber2 { get; set; } = 10;
        public int RowNumber3 { get; set; } = 10;
        public bool IsInValidated1 { get; set; }
        public bool IsInValidated2 { get; set; }
        public bool IsInValidated3 { get; set; }
        public bool IsInValidated4 { get; set; }
        public bool IsInValidated5 { get; set; }
        public bool IsSaveClicked1 { get; set; }
        public bool IsSaveClicked2 { get; set; }
        public bool IsSaveClicked3 { get; set; }
        public bool IsSaveClicked4 { get; set; }
        public bool IsSaveClicked5 { get; set; }

        private ISqlDataAccess _db { get; set; }

        // public ICookieService CookieService { get; set; }

        private int SessionValue = 0;
        private SPResult session;

        public ClS_Config(ISqlDataAccess db, SPResult session)
        {
            SessionValue = session.Result;
            this.session=session;
            _db = db;
        }

        public async Task<IEnumerable<T>> GetCMB<T>(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
        {
            return await _db.GetDataTable<T, dynamic>("Pro_GetCMB", new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
        }

        public async Task<IEnumerable<T>> GetAllInfo<T>(int SelectPro = 0, int ValID = 0)
        {
            return await _db.GetDataTable<T, dynamic>("Pro_GetAllInfo", new { Select = SelectPro, ID = ValID,  EntryBy = SessionValue });
        }

        public async Task<T> GetOneInfo<T>(int SelectPro = 0, int ValID = 0)
        {
            return await _db.GetOneInfo<T, dynamic>("Pro_GetAllInfo", new { Select = SelectPro, ID = ValID,  EntryBy = SessionValue });
        }

        public async Task<IEnumerable<T>> GetGrid<T>(int SelectPro = 0, int ValID = 0, string Search = "")
        {
            return await _db.GetDataTable<T, dynamic>("Pro_GetGrid", new { Select = SelectPro, ID = ValID, Search = Search, EntryBy = SessionValue });
        }

        public async Task<PagedResult<T>> GetGridPaging<T>(int SelectPro = 0, int ValID = 0, string Search = "", int PageNumber = 1, string SortColumn = "", string SortDirection = "Asc", int MaxNavigationPage = 5,int PageSize=10)
        {
            return await _db.GetGridResult<T, dynamic>("Pro_GridPaging", new { Select = SelectPro, ID = ValID, Search = Search, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize, MaxNavigationPages: MaxNavigationPage);
        }

        public async Task<T> InsertUpdateConfig<T>(int SelectPro = 0, int ValID = 0, string ValName = "", string ValueName = "", int ValueID = 0, int ValueIDTwo = 0,decimal Price=0)
        {
            return await _db.SaveData<T, dynamic>("Pro_InsertUpdateConfig", new { Select = SelectPro, ID = ValID, Name = ValName, ValueName = ValueName, ValueID = ValueID, ValueIDTwo = ValueIDTwo, Price= Price,  EntryBy = 1 });
        }


        public async Task<PagedResult<T>> HotelGetGridPaging<T>(int SelectPro = 0, int ValID = 0, string Search = "", int PageNumber = 1, string SortColumn = "", string SortDirection = "Asc", int MaxNavigationPage = 5, int PageSize = 10)
        {
            return await _db.GetGridResult<T, dynamic>("HTPro_GridPaging", new { Select = SelectPro, ID = ValID, Search = Search, PageNumber = PageNumber, PageSize = PageSize, SortColumn = SortColumn, SortDirection = SortDirection, EntryBy = SessionValue }, PageNumber: PageNumber, PageSize: PageSize, MaxNavigationPages: MaxNavigationPage);
        }

        public async Task<IEnumerable<T>> HotelGetCMB<T>(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
        {
            return await _db.GetDataTable<T, dynamic>("HTPro_GetCMB", new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
        }

        public async Task<IEnumerable<T>> HotelGetAllInfo<T>(int SelectPro = 0, int ValID = 0)
        {
            return await _db.GetDataTable<T, dynamic>("HTPro_GetAllInfo", new { Select = SelectPro, ID = ValID, EntryBy = SessionValue });
        }

        public async Task<SearchCombos> SearchCombos(int SelectPro = 0, int ValID = 0, int IDTwo = 0)
        {
            var result = await _db.GetMultiple<DirectorateInfo,GenderInfo,NationalityInfo>(
                "Pro_GetCMB", new { Select = SelectPro, ID = ValID, IDTwo = IDTwo, EntryBy = SessionValue });
            return new SearchCombos()
            {
              Directorates=result.Item1,
              Genders=result.Item2,
              Nationalities=result.Item3
            };
        }

        //public bool HasPermission(int ID, bool ShouldMoreThanOnePermissionsBeAllTrue = false)
        //{
        //    // if(ID==9)
        //    // return true;
        //    IEnumerable<ConfigData> permission = Enumerable.Empty<ConfigData>();

        //    if (ID == 5)
        //        permission = session.ConfigData.Where(x => x.confg_ID == ID || x.confg_ID == 25 || x.confg_ID==16);
        //    else
        //        permission = session.ConfigData.Where(x => x.confg_ID == ID);

        //    if (ID == 5 || ID == 25)
        //        return true;

        //    if (!ShouldMoreThanOnePermissionsBeAllTrue)
        //        return permission.Any(x => x.confg_Value.TryParseTo0() == 1);
        //    else
        //        return permission.All(x => x.confg_Value.TryParseTo0() == 1);
        //}

        public bool HasPermissionRole(int ID)
        {
           return session.Roles.Any(x => x.pergroup_PerID == ID || x.pergroup_PerID == 1);
        }

        //public bool HasPermissionSystemOptions(int ID)
        //{
        //    var permission = session.SystemOptions.Where(x => x.so_ID == ID);
        //    if (permission.Any())
        //        return permission.First().so_Value;
        //    else
        //        return false;
        //}

        public string ValidateField(bool bool1, string? val)
        {
            if (bool1 && string.IsNullOrWhiteSpace(val))
                return "custom-validation";
            else
                return "";
        }
        public string ValidateField(bool bool1, byte[]? val)
        {
            if (bool1 && (val == null || val.Length <= 0))
                return "custom-validation";
            else
                return "";
        }
        public string ValidateField(bool bool1, bool val)
        {
            if (bool1 && val)
                return "custom-validation";
            else
                return "";
        }
        public string ValidateField(bool bool1, DateTime? val)
        {
            if (bool1 && !val.HasValue)
                return "custom-validation";
            else
                return "";
        }
        public string ValidateField(bool bool1, int val)
        {
            if (bool1 && val <= 0)
                return "custom-validation";
            else
                return "";
        }
        public string ValidateFieldAccCode(bool bool1, int val)
        {
            if (bool1 && val <= -1)
                return "custom-validation";
            else
                return "";
        }
        public string ValidateField(bool bool1, decimal val)
        {
            if (bool1 && val <= decimal.Zero)
                return "custom-validation";
            else
                return "";
        }

        public string ChangeAddEditIcon(int id)
        {
            return id > 0 ? "Edit" : "Add";
        }
        public async Task<IEnumerable<CheckRoles>> GetAllRoles()
        {
            var Roles = await GetAllInfo<CheckRoles>(SelectPro: 10, ValID: SessionValue);
            return Roles;
        }
    }
}