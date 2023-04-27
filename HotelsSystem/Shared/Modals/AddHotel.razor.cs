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
    MyFunctions.myLogin.MyFunctions func = new MyFunctions.myLogin.MyFunctions();
    HotelRoomsInfo SelectedHotelRoom = new HotelRoomsInfo();

    int SelectedTab = 0;
    private TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
    private TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;
    private TableEditTrigger editTrigger = TableEditTrigger.RowClick;
    PagedResult<HotelRoomsInfo> PaginatedRooms = PagedResult<HotelRoomsInfo>.EmptyPagedResult();
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
            await GetHotelUser();
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

    async Task GetHotelByID()
    {
        SelectedHotel = await config.GetOneInfo<HotelsInfo>(SelectPro: 5, ValID: HotelID);
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
            SelectedHotel.HTT_Type = string.Empty;
            return;
        }
        SelectedHotel.htl_TypeID = e.HTT_ID;
        SelectedHotel.HTT_Type = e.HTT_Type;
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
            SelectedHotelRoom.htr_Price = 0;
            return;
        }
        SelectedHotelRoom.htr_Type = e.cfg_HTR_ID;
        SelectedHotelRoom.cfg_HTR_Type = e.cfg_HTR_Type;
        SelectedHotelRoom.htr_Price = e.cfg_HTR_Price;
    }
    void InLineOnSelectedRoomTypeChanged(HotelRoomsTypesInfo e, HotelRoomsInfo context)
    {
        if (e == null)
        {
            context.htr_Type = 0;
            context.cfg_HTR_Type = "";
            context.htr_Price = 0;
            return;
        }
        context.htr_Type = e.cfg_HTR_ID;
        context.cfg_HTR_Type = e.cfg_HTR_Type;
        context.htr_Price = e.cfg_HTR_Price;
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
        System.Console.WriteLine(context.htr_FloorID);
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

        SPResult result = await hotel.InsertUpdateHotels<SPResult>(SelectPro: 1,
        ValID: SelectedHotel.htl_ID,
        HotelTypeID: SelectedHotel.htl_TypeID,
        HotelName: SelectedHotel.htl_Name.ToEmptyOnNull(),
        HotelAddress: SelectedHotel.htl_Address.ToEmptyOnNull(),
        StarNumber: SelectedHotel.htl_Star,
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
            await OnAdd.InvokeAsync();
            // MudDialog.Close(DialogResult.Ok(true));
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
    async Task GetHotelUser()
    {
        SelectedHotelUser = await config.GetOneInfo<HotelUsersInfo>(SelectPro: 8, ValID: SelectedHotel.htl_ID);

        if (!SelectedHotelUser.htlus_Password.IsStringNullOrWhiteSpace())
            SelectedHotelUser.htlus_Password = func.decr_pass(SelectedHotelUser.htlus_Password);
    }
    void OnSelectedLanguageChange(LanguageInfo e)
    {
        if (e == null)
        {
            SelectedHotelUser.htlus_LanguageID = 0;
            return;
        }
        SelectedHotelUser.htlus_LanguageID = e.lang_ID;
    }
    void OnSelectedHotelTypeChangeChange(HotelUserTypeComboBox e)
    {
        if (e == null)
        {
            SelectedHotelUser.htlus_TypeID = 0;
            return;
        }
        SelectedHotelUser.htlus_TypeID = e.htlustype_ID;
    }
    async Task InsertUpdateHotelUser()
    {
        await AddHotelUserForm!.Validate();
        if (!AddHotelUserForm.IsValid)
            return;

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
            await GetHotelUser();
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
            ((HotelRoomsInfo)element).htr_Detail=BackupUpitem.htr_Detail;
            ((HotelRoomsInfo)element).htr_FloorID=BackupUpitem.htr_FloorID;
            ((HotelRoomsInfo)element).htr_Type=BackupUpitem.htr_Type;
            // ((HotelRoomsInfo)element).htr_Type=BackupUpitem.htr_Type;
            ((HotelRoomsInfo)element).cfg_HTR_Type=BackupUpitem.cfg_HTR_Type;
            ((HotelRoomsInfo)element).htf_FloorName=BackupUpitem.htf_FloorName;
            ((HotelRoomsInfo)element).htr_Price=BackupUpitem.htr_Price;
            // System.Console.WriteLine("reseting"+JsonConvert.SerializeObject(BackupUpitem));
            // element = BackupUpitem;
        // }
    }
    HotelRoomsInfo BackupUpitem = new HotelRoomsInfo();
    private void BackupItem(object element)
    {
       BackupUpitem=new(){
        htr_Detail=((HotelRoomsInfo)element).htr_Detail,
        htr_Type=((HotelRoomsInfo)element).htr_Type,
        cfg_HTR_Type=((HotelRoomsInfo)element).cfg_HTR_Type,
        htf_FloorName=((HotelRoomsInfo)element).htf_FloorName,
        htr_FloorID=((HotelRoomsInfo)element).htr_FloorID,
        htr_Price=((HotelRoomsInfo)element).htr_Price,
        // htr_Type=((HotelRoomsInfo)element).htr_Type

       };
    }
    private async Task ItemHasBeenCommitted(object element)
    {
        if (element is HotelRoomsInfo room)
        {
            System.Console.WriteLine(JsonConvert.SerializeObject(room));
            await InLineUpdateRooms(room);
        }
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
        System.Console.WriteLine(SelectedItems.Any());
        var select = JsonConvert.SerializeObject(SelectedItems.Select(x => x.htr_ID));
        SPResult result = await hotel.InsertUpdateHotels<SPResult>(
            SelectPro: 3,
            ValID: HotelID,
            HotelTypeID: SelectedHotelRoom.htr_Type,
            StarNumber: SelectedHotelRoom.htr_FloorID,
            HotelName: SelectedItems.Count==1? SelectedHotelRoom.htr_Detail.ToEmptyOnNull():"",
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
        System.Console.WriteLine(JsonConvert.SerializeObject(room));
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