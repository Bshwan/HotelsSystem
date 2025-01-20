using System.Reflection;

namespace HotelsSystem.Shared.Modals;
public partial class AddHotel
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter]
    public ClS_Config config { get; set; } = default!;
    [Parameter]
    public EventCallback OnAdd { get; set; } = default!;
    [Parameter]
    public ClS_Hotels hotel { get; set; } = default!;
    [Parameter]
    public int HotelID { get; set; }
    [Inject]
    protected IToaster Toaster { get; set; } = default!;

    MudForm? AddHotelForm;
    MudForm? AddHotelUserForm;
    HotelsInfo SelectedHotel = new HotelsInfo();
    AddHotelCombos combos = new AddHotelCombos();
    AddHotelAddUserCombos AddUserCombos = new AddHotelAddUserCombos();
    HotelUsersInfo SelectedHotelUser = new HotelUsersInfo();
    HotelUsersInfo SelectedHotelUserBeforeChange = new HotelUsersInfo();
    MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
    HotelRoomsInfo SelectedHotelRoom = new HotelRoomsInfo();

    int SelectedTab = 0;
    private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
    private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;
    private TableEditTrigger editTrigger = TableEditTrigger.RowClick;
    PagedResult<HotelRoomsInfo> PaginatedRooms = PagedResult<HotelRoomsInfo>.EmptyPagedResult();
    PagedResult<HotelUsersInfo> PaginatedHotelUsers = PagedResult<HotelUsersInfo>.EmptyPagedResult();
    HashSet<HotelRoomsInfo> SelectedItems = new HashSet<HotelRoomsInfo>();
    MudAutocomplete<HotelFloorInfo>? FloorAutocomplete;
    MudAutocomplete<HotelRoomsTypesInfo>? RoomTypeAutocomplete;
    MudAutocomplete<HotelRoomsTypesInfo>? InLineRoomTypeAutocomplete;
    MudAutocomplete<HotelFloorInfo>? InLineFloorAutocomplete;

    protected override async Task OnParametersSetAsync()
    {
        if (HotelID > 0)
            await GetHotelByID();

        if (SelectedHotel.htl_ID > 0)
        {
            //await GetHotelUser();
            await AddUserGetCombos();
        }
        await GetCombos();

        if (SelectedHotel.htl_ID <= 0)
        {
            if (combos.Directorates.Any())
            {
                var first = combos.Directorates.First();
                SelectedHotel.htl_DirectorateID = first.peo_DirectorateID;
                SelectedHotel.peo_DirectorateName = first.peo_DirectorateName;
                await GetWorkpointByDirectorate();
            }
            if (combos.WorkingPoints.Any())
            {
                var first = combos.WorkingPoints.First();
                SelectedHotel.wp_workpointName = first.wp_workpointName;
                SelectedHotel.htl_WorkPointID = first.wp_ID;
            }
        }
    }
    async Task GetCombos()
    {
        combos = await hotel.AddHotelCombos(SelectPro: 8, ValID: HotelID);
    }
    async Task AddUserGetCombos()
    {
        AddUserCombos = await hotel.AddHotelAddUserCombos(SelectPro: 9);
    }
    HotelsInfo SelectedHotelBeforeChange = new HotelsInfo();
    async Task GetHotelByID()
    {
        SelectedHotel = await config.GetOneInfo<HotelsInfo>(SelectPro: 5, ValID: HotelID);
        SelectedHotel.HotelHas0Stars = SelectedHotel.congltype_StarNumber == 0;
        SelectedHotelBeforeChange = Util.Clone(SelectedHotel);
    }
    async Task InsertLog()
    {
        var changes = Util.GetChangedProperties(SelectedHotel,SelectedHotelBeforeChange);
        changes.Remove("htl_TypeID");
        changes.Remove("htl_DirectorateID");
        changes.Remove("htl_WorkPointID");
        string keys = string.Join(",", changes.Keys);
        string NewVlaues = string.Join(",", changes.Values.Select(v => v.NewValue?.ToString() ?? "null"));
        string OldValues = string.Join(",", changes.Values.Select(v => v.OldValue?.ToString() ?? "null"));

        if (keys.Any())
            await config.Pro_InsertActionLog<SPResult>(SelectPro: 1, ActionType: 6, ProfileID: HotelID, UserID: config.session.Result, UserName: config.session.LastValue, UserType: config.session.MSG.ToEmptyOnNull(),
            FieldName: keys, Value: NewVlaues,OldValue:OldValues,TableName:"hotel admin");
    }
    async Task InsertLogSingleRoom(HotelRoomsInfo e)
    {
        var changes = Util.GetChangedProperties(e, BackupUpitem);
        changes.Remove("htr_Type");
        changes.Remove("htr_FloorID");
        string keys = string.Join(",", changes.Keys);
        string NewVlaues = string.Join(",", changes.Values.Select(v => v.NewValue?.ToString() ?? "null"));
        string OldValues = string.Join(",", changes.Values.Select(v => v.OldValue?.ToString() ?? "null"));

        if (keys.Any())
            await config.Pro_InsertActionLog<SPResult>(SelectPro: 1, ActionType: 6, ProfileID: HotelID, UserID: config.session.Result, UserName: config.session.LastValue, UserType: config.session.MSG.ToEmptyOnNull(),
            FieldName: keys, Value: NewVlaues, OldValue: OldValues, TableName: "hotel admin");
    }
    async Task InsertLogUser()
    {
        try
        {
            var changes = Util.GetChangedProperties(SelectedHotelUser, SelectedHotelUserBeforeChange);
            changes.Remove("htlus_TypeID");
            changes.Remove("htlus_LanguageID");
            var HasKey = changes.TryGetValue("peo_UserPassword", out var findPassword);
            if (HasKey && findPassword.OldValue != null && findPassword.NewValue != null)
            {
                if (!string.IsNullOrWhiteSpace(findPassword.OldValue.ToString()))
                    findPassword.OldValue = func.encr_pass(findPassword.OldValue.ToString());
                if (!string.IsNullOrWhiteSpace(findPassword.NewValue.ToString()))
                    findPassword.NewValue = func.encr_pass(findPassword.NewValue.ToString());
                changes["htlus_Password"] = findPassword;
            }
            string keys = string.Join(",", changes.Keys);
            string NewVlaues = string.Join(",", changes.Values.Select(v => v.NewValue?.ToString() ?? "null"));
            string OldValues = string.Join(",", changes.Values.Select(v => v.OldValue?.ToString() ?? "null"));

            if (keys.Any())
                await config.Pro_InsertActionLog<SPResult>(SelectPro: 1, ActionType: 6, ProfileID: SelectedHotelUser.htlus_ID, UserID: config.session.Result, UserName: config.session.LastValue, UserType: config.session.MSG.ToEmptyOnNull(),
                    FieldName: keys, Value: NewVlaues, OldValue: OldValues, TableName: "user admin");
        }
        catch(Exception ex)
        {
            await Console.Out.WriteLineAsync(ex.ToString());
        }
    }
    async Task InsertLogUpdateRoomsBulk()
    {
        var changes = new Dictionary<string, (object? OldValue, object? NewValue)>();

        foreach (var item in SelectedItems)
        {
            var find = PaginatedRooms.Items.Where(x => x.htr_ID == item.htr_ID).FirstOrDefault();
            if (find != null)
            {
                if(SelectedItems.Count==1 && find.htr_Detail!= SelectedHotelRoom.htr_Detail)
                changes.Add("htr_Detail", (OldValue: find.htr_Detail, NewValue: SelectedHotelRoom.htr_Detail));

                if(find.htr_Price != SelectedHotelRoom.htr_Price)
                changes.Add("htr_Price", (OldValue: find.htr_Price, NewValue: SelectedHotelRoom.htr_Price));

                //if (find.htr_FloorID != SelectedHotelRoom.htr_FloorID)
                //    changes.Add("htr_FloorID", (OldValue: find.htr_FloorID, NewValue: SelectedHotelRoom.htr_FloorID));

                if (find.htr_Type != SelectedHotelRoom.htr_Type)
                    changes.Add("htr_Type", (OldValue: find.htr_Type, NewValue: SelectedHotelRoom.htr_Type));

                if (find.htf_FloorName != SelectedHotelRoom.htf_FloorName)
                    changes.Add("htf_FloorName", (OldValue: find.htf_FloorName, NewValue: SelectedHotelRoom.htf_FloorName));
            }
        }

        string keys = string.Join(",", changes.Keys);
        string NewVlaues = string.Join(",", changes.Values.Select(v => v.NewValue?.ToString() ?? "null"));
        string OldValues = string.Join(",", changes.Values.Select(v => v.OldValue?.ToString() ?? "null"));

        if (keys.Any())
            await config.Pro_InsertActionLog<SPResult>(SelectPro: 1, ActionType: 6, UserID: config.session.Result, ProfileID: HotelID, UserName: config.session.LastValue, UserType: config.session.MSG.ToEmptyOnNull(),
                FieldName: keys, Value: NewVlaues, OldValue: OldValues, TableName: "hotelrooms admin");
    }
    async Task OnDirectorateChange(DirectorateInfo e)
    {
        SelectedHotel.wp_workpointName = string.Empty;
        SelectedHotel.htl_WorkPointID = 0;
        combos.WorkingPoints = Enumerable.Empty<WorkingPointInfo>();

        if (e == null)
        {
            SelectedHotel.peo_DirectorateName = string.Empty;
            SelectedHotel.htl_DirectorateID = 0;
            return;
        }

        SelectedHotel.peo_DirectorateName = e.peo_DirectorateName;
        SelectedHotel.htl_DirectorateID = e.peo_DirectorateID;

        await GetWorkpointByDirectorate();
    }
    async Task GetWorkpointByDirectorate()
    {
        combos.WorkingPoints = await config.GetCMB<WorkingPointInfo>(SelectPro: 5, ValID: SelectedHotel.htl_DirectorateID);
    }
    void OnWorkpointChange(WorkingPointInfo e)
    {
        if (e == null)
        {
            SelectedHotel.wp_workpointName = string.Empty;
            SelectedHotel.htl_WorkPointID = 0;
            return;
        }
        SelectedHotel.wp_workpointName = e.wp_workpointName;
        SelectedHotel.htl_WorkPointID = e.wp_ID;
    }
    void OnHotelTypeChange(HotelTypesComboBox e)
    {
        if (e == null)
        {
            SelectedHotel.htl_TypeID = 0;
            SelectedHotel.congltype_Name = string.Empty;
            // SelectedHotel.htl_Star = 0;
            SelectedHotel.HotelHas0Stars = false;
            SelectedHotel.htl_Star = 0;
            SelectedHotel.congltype_StarNumber=0;
            return;
        }
        SelectedHotel.htl_TypeID = e.congltype_ID;
        SelectedHotel.congltype_Name = e.congltype_Name;
        // SelectedHotel.htl_Star = e.congltype_StarNumber;
        SelectedHotel.HotelHas0Stars = e.congltype_StarNumber == 0;
        SelectedHotel.htl_Star=e.congltype_StarNumber;
        SelectedHotel.congltype_StarNumber=e.congltype_StarNumber;
    }
    async Task<IEnumerable<DirectorateInfo>> SearchDirectorate(string e)
    {
        return await Task.FromResult(combos.Directorates.SearchAll<DirectorateInfo>(e, "peo_DirectorateName"));
    }
    async Task<IEnumerable<HotelRoomsTypesInfo>> SearchRoomType(string e)
    {
        // System.Console.WriteLine("search"+e);
        var search = combos.RoomTypes.SearchAll<HotelRoomsTypesInfo>(e, "cfg_HTR_Type");
        // if (!search.Any() && !e.IsStringNullOrWhiteSpace())
        // {
        //     RoomTypeAutocomplete!.Value = new HotelRoomsTypesInfo { cfg_HTR_ID = 0, cfg_HTR_Type = e.ToEmptyOnNull(),cfg_HTR_Price=0 };
        // }
        return await Task.FromResult(search);
    }
    async Task<IEnumerable<HotelRoomsTypesInfo>> InLineSearchRoomType(string e)
    {
        var search = combos.RoomTypes.SearchAll<HotelRoomsTypesInfo>(e, "cfg_HTR_Type");
        // if (!search.Any() && !e.IsStringNullOrWhiteSpace())
        // {
        //     InLineRoomTypeAutocomplete!.Value = new HotelRoomsTypesInfo { cfg_HTR_ID = 0, cfg_HTR_Type = e.ToEmptyOnNull(),cfg_HTR_Price=0 };
        // }
        return await Task.FromResult(search);
    }


    async Task<IEnumerable<HotelFloorInfo>> SearchFloorType(string e)
    {
        var search = combos.FloorTypes.SearchAll<HotelFloorInfo>(e, "htf_FloorName");
        if (!search.Any() && !e.IsStringNullOrWhiteSpace())
        {
            FloorAutocomplete!.Value = new HotelFloorInfo { htf_FloorID = 0, htf_FloorName = e.ToEmptyOnNull() };
            SelectedHotelRoom.htr_FloorID = 0;
            SelectedHotelRoom.htf_FloorName = e.ToEmptyOnNull();
        }
        return await Task.FromResult(search);
    }
    async Task<IEnumerable<HotelFloorInfo>> SearchFloorTypeInLine(string e, HotelRoomsInfo context)
    {
        var search = combos.FloorTypes.SearchAll<HotelFloorInfo>(e, "htf_FloorName");
        if (!search.Any() && !e.IsStringNullOrWhiteSpace())
        {
            InLineFloorAutocomplete!.Value = new HotelFloorInfo { htf_FloorID = 0, htf_FloorName = e.ToEmptyOnNull() };
            context.htr_FloorID = 0;
            context.htf_FloorName = e.ToEmptyOnNull();
        }
        return await Task.FromResult(search);
    }
    void OnSelectedRoomTypeChanged(HotelRoomsTypesInfo e)
    {
        if (e == null)
        {
            SelectedHotelRoom.htr_Type = 0;
            SelectedHotelRoom.cfg_HTR_Type = "";
            // SelectedHotelRoom.htr_Price = 0;
            return;
        }
        SelectedHotelRoom.htr_Type = e.cfg_HTR_ID;
        SelectedHotelRoom.cfg_HTR_Type = e.cfg_HTR_Type;
        // SelectedHotelRoom.htr_Price = e.cfg_HTR_Price;
    }
    void InLineOnSelectedRoomTypeChanged(HotelRoomsTypesInfo e, HotelRoomsInfo context)
    {
        if (e == null)
        {
            context.htr_Type = 0;
            context.cfg_HTR_Type = "";
            // context.htr_Price = 0;
            return;
        }
        context.htr_Type = e.cfg_HTR_ID;
        context.cfg_HTR_Type = e.cfg_HTR_Type;
        // context.htr_Price = e.cfg_HTR_Price;
    }
    void OnSelectedFloorName(HotelFloorInfo e)
    {
        if (e == null)
        {
            SelectedHotelRoom.htr_FloorID = 0;
            SelectedHotelRoom.htf_FloorName = "";
            return;
        }
        SelectedHotelRoom.htr_FloorID = e.htf_FloorID;
        SelectedHotelRoom.htf_FloorName = e.htf_FloorName;
    }
    void InLineOnSelectedFloorName(HotelFloorInfo e, HotelRoomsInfo context)
    {
        if (e == null)
        {
            context.htr_FloorID = 0;
            context.htf_FloorName = "";
            return;
        }

        context.htr_FloorID = e.htf_FloorID;
        context.htf_FloorName = e.htf_FloorName;
    }
    async Task<IEnumerable<WorkingPointInfo>> SearchWorkpoint(string e)
    {
        return await Task.FromResult(combos.WorkingPoints.SearchAll<WorkingPointInfo>(e, "wp_workpointName"));
    }

    async Task InsertUpdateHotel()
    {

        await AddHotelForm!.Validate();
        if (!AddHotelForm.IsValid)
            return;

        if (SelectedHotel.htl_ID>0)
        {
            _ = Task.Run(async () => await InsertLog());
        }

        SPResult result = await hotel.InsertUpdateHotels<SPResult>(SelectPro: 1,
        ValID: SelectedHotel.htl_ID,
        HotelTypeID: SelectedHotel.htl_TypeID,
        HotelName: SelectedHotel.htl_Name.ToEmptyOnNull(),
        HotelAddress: SelectedHotel.htl_Address.ToEmptyOnNull(),
        // StarNumber: SelectedHotel.HotelHas0Stars ? 0 : SelectedHotel.htl_Star,
        NumberOfRooms: SelectedHotel.htl_NumberOfRooms,
        DirectorateID: SelectedHotel.htl_DirectorateID,
        WorkPointID: SelectedHotel.htl_WorkPointID,
        Note: SelectedHotel.htl_Note.ToEmptyOnNull());

        if (result.Result == 1)
        {
            if (int.TryParse(result.LastValue, out int val) && val > 0)
            {
                HotelID = val;
                await GetHotelByID();
                await AddUserGetCombos();
            }
            else
            {
                await GetHotelByID();
            }
            await OnAdd.InvokeAsync();
            // MudDialog.Close(DialogResult.Ok(true));
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    //async Task GetHotelUser()
    //{
    //    SelectedHotelUser = await config.GetOneInfo<HotelUsersInfo>(SelectPro: 8, ValID: SelectedHotel.htl_ID);

    //    if (!SelectedHotelUser.htlus_Password.IsStringNullOrWhiteSpace())
    //        SelectedHotelUser.htlus_Password = func.decr_pass(SelectedHotelUser.htlus_Password);
    //}
    async Task GetHotelUserByUserID(int id)
    {
        SelectedHotelUser = await config.GetOneInfo<HotelUsersInfo>(SelectPro: 8, ValID: id);

        if (!SelectedHotelUser.htlus_Password.IsStringNullOrWhiteSpace())
            SelectedHotelUser.htlus_Password = func.decr_pass(SelectedHotelUser.htlus_Password);

        SelectedHotelUserBeforeChange = Util.Clone(SelectedHotelUser);
    }
    void OnSelectedLanguageChange(LanguageInfo e)
    {
        if (e == null)
        {
            SelectedHotelUser.htlus_LanguageID = 0;
            SelectedHotelUser.lang_Name = "";
            return;
        }
        SelectedHotelUser.htlus_LanguageID = e.lang_ID;
        SelectedHotelUser.lang_Name = e.lang_Name;
    }
    void OnSelectedHotelTypeChangeChange(HotelUserTypeComboBox e)
    {
        if (e == null)
        {
            SelectedHotelUser.htlus_TypeID = 0;
            SelectedHotelUser.htlustype_Name = "";
            return;
        }
        SelectedHotelUser.htlus_TypeID = e.htlustype_ID;
        SelectedHotelUser.htlustype_Name = e.htlustype_Name;
    }
    async Task InsertUpdateHotelUser()
    {
        await AddHotelUserForm!.Validate();
        if (!AddHotelUserForm.IsValid)
            return;

        if (SelectedHotelUser.htlus_ID > 0)
        {
            _ = Task.Run(async () => await InsertLogUser());
        }

        SPResult result = await hotel.InsertUpdateHotels<SPResult>(
            SelectPro: 2,
            ValID: SelectedHotelUser.htlus_ID,
            HotelName: SelectedHotelUser.htlus_Name.ToEmptyOnNull(),
            HotelAddress: SelectedHotelUser.htlus_FullName.ToEmptyOnNull(),
            Note: func.encr_pass(SelectedHotelUser.htlus_Password.ToEmptyOnNull()),
            HotelTypeID: SelectedHotel.htl_ID,
            StarNumber: SelectedHotelUser.htlus_LanguageID,
            DirectorateID: SelectedHotelUser.htlus_Active ? 1 : 0);

        if (result.Result == 1)
        {
            //await GetHotelUserByUserID(SelectedHotelUser.htlus_ID);
            SelectedHotelUser = new HotelUsersInfo();
            await UsersTable.ReloadServerData();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    private void ResetItemToOriginalValues(object element)
    {
        // System.Console.WriteLine("reseting");
        // if (element is HotelRoomsInfo room)
        // {
        // System.Console.WriteLine("backup:"+BackupUpitem.htr_Detail);
        ((HotelRoomsInfo)element).htr_Detail = BackupUpitem.htr_Detail;
        ((HotelRoomsInfo)element).htr_FloorID = BackupUpitem.htr_FloorID;
        ((HotelRoomsInfo)element).htr_Type = BackupUpitem.htr_Type;
        // ((HotelRoomsInfo)element).htr_Type=BackupUpitem.htr_Type;
        ((HotelRoomsInfo)element).cfg_HTR_Type = BackupUpitem.cfg_HTR_Type;
        ((HotelRoomsInfo)element).htf_FloorName = BackupUpitem.htf_FloorName;
        ((HotelRoomsInfo)element).htr_Price = BackupUpitem.htr_Price;
        // System.Console.WriteLine("reseting"+JsonConvert.SerializeObject(BackupUpitem));
        // element = BackupUpitem;
        // }
    }
    HotelRoomsInfo BackupUpitem = new HotelRoomsInfo();
    private void BackupItem(object element)
    {
        BackupUpitem = new()
        {
            htr_Detail = ((HotelRoomsInfo)element).htr_Detail,
            htr_Type = ((HotelRoomsInfo)element).htr_Type,
            htl_TypeID = ((HotelRoomsInfo)element).htl_TypeID,
            htr_ID = ((HotelRoomsInfo)element).htr_ID,
            cfg_HTR_Type = ((HotelRoomsInfo)element).cfg_HTR_Type,
            htf_FloorName = ((HotelRoomsInfo)element).htf_FloorName,
            htr_FloorID = ((HotelRoomsInfo)element).htr_FloorID,
            htr_Price = ((HotelRoomsInfo)element).htr_Price,
            // htr_Type=((HotelRoomsInfo)element).htr_Type

        };
    }
    private async Task ItemHasBeenCommitted(object element)
    {
        if (element is HotelRoomsInfo room)
        {
            await InLineUpdateRooms(room);
        }
    }
    MudTable<HotelUsersInfo>? UsersTable;
    private async Task<TableData<HotelUsersInfo>> GetPaginatedUser(TableState state)
    {
        if (HotelID > 0)
        {
            PaginatedHotelUsers = await config!.GetGridPaging<HotelUsersInfo>(
                SelectPro: 4,
                ValID: HotelID,
                PageNumber: state.Page + 1,
                PageSize: state.PageSize,
                SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "htlus_ID" : state.SortLabel,
                SortDirection: Util.ResolveSort(state.SortDirection));
        }
        //StateHasChanged();

        return new TableData<HotelUsersInfo>() { TotalItems = PaginatedHotelUsers.TotalItems, Items = PaginatedHotelUsers.Items };
    }

    MudTable<HotelRoomsInfo>? table;
    private async Task<TableData<HotelRoomsInfo>> GetPaginatedRooms(TableState state)
    {
        if (HotelID > 0)
        {
            PaginatedRooms = await config!.GetGridPaging<HotelRoomsInfo>(
                SelectPro: 5,
                ValID: HotelID,
                PageNumber: state.Page + 1,
                PageSize: state.PageSize,
                SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "htr_Detail" : state.SortLabel,
                SortDirection: Util.ResolveSort(state.SortDirection));
        }
        StateHasChanged();

        return new TableData<HotelRoomsInfo>() { TotalItems = PaginatedRooms.TotalItems, Items = PaginatedRooms.Items };
    }
    async Task UpdateRooms()
    {
        var select = JsonConvert.SerializeObject(SelectedItems.Select(x => x.htr_ID));

        //_ = Task.Run(async () => await InsertLogUpdateRoomsBulk());

        SPResult result = await hotel.InsertUpdateHotels<SPResult>(
            SelectPro: 3,
            ValID: HotelID,
            HotelTypeID: SelectedHotelRoom.htr_Type,
            StarNumber: SelectedHotelRoom.htr_FloorID,
            HotelName: SelectedItems.Count == 1 ? SelectedHotelRoom.htr_Detail.ToEmptyOnNull() : "",
            Note: SelectedHotelRoom.htf_FloorName.ToEmptyOnNull(),
            Price: SelectedHotelRoom.htr_Price,
            HotelAddress: select.ToEmptyOnNull().Replace("[", "").Replace("]", ""));

        if (result.Result == 1)
        {
            SelectedHotelRoom = new HotelRoomsInfo();
            await table!.ReloadServerData();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    async Task InLineUpdateRooms(HotelRoomsInfo room)
    {
        _ = Task.Run(async () => await InsertLogSingleRoom(room));
        SPResult result = await hotel.InsertUpdateHotels<SPResult>(
            SelectPro: 3,
            ValID: HotelID,
            HotelTypeID: room.htr_Type,
            StarNumber: room.htr_FloorID,
            HotelName: room.htr_Detail.ToEmptyOnNull(),
            Note: room.htf_FloorName.ToEmptyOnNull(),
            Price: room.htr_Price,
            HotelAddress: room.htr_ID.ToString());

        if (result.Result == 1)
        {
            SelectedHotelRoom = new HotelRoomsInfo();
            await table!.ReloadServerData();
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
}