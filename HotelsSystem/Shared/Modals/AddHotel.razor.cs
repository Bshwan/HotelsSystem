namespace HotelsSystem.Shared.Modals;
public partial class AddHotel
{
    [CascadingParameter]
    MudDialogInstance MudDialog { get; set; } = default!;
    [Parameter]
    public ClS_Config config { get; set; } = default!;
    [Parameter]
    public ClS_Hotels hotel { get; set; } = default!;
    [Parameter]
    public int HotelID { get; set; }
    [Inject]
    protected IToaster Toaster { get; set; } = default!;

    MudForm? AddHotelForm;
    HotelsInfo SelectedHotel = new HotelsInfo();
    AddHotelCombos combos = new AddHotelCombos();

    protected override async Task OnParametersSetAsync()
    {
        await GetCombos();
        if (HotelID > 0)
            await GetHotelByID();
    }
    async Task GetCombos()
    {
        combos = await hotel.AddHotelCombos(SelectPro: 8);
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

        combos.WorkingPoints = await config.GetCMB<WorkingPointInfo>(SelectPro: 5, ValID: e.peo_DirectorateID);
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
            MudDialog.Close(DialogResult.Ok(true));
            Toaster.Success(".", result.MSG);
            return;
        }
        Toaster.Error(".", result.MSG);
    }
}