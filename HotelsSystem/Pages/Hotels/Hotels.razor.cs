
namespace HotelsSystem.Pages.Hotels;
public partial class Hotels
{
    [Inject]
    protected ISqlDataAccess DB { get; set; } = default!;
    [Inject]
    protected IJSRuntime jSRuntime { get; set; } = default!;
    [Inject]
    protected IToaster Toaster { get; set; } = default!;
    [Inject]
    protected NavigationManager nav { get; set; } = default!;
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;

    PagedResult<HotelsInfo> PaginatedItems = PagedResult<HotelsInfo>.EmptyPagedResult();

    ClS_Hotels hotel = default!;
    ClS_Config config = default!;
    HotelsInfo Filter = new HotelsInfo();
    private MudTable<HotelsInfo>? table;
    SPResult? session;

    protected override async Task OnInitializedAsync()
    {
        session = await Protection.GetDecryptedSession(jSRuntime, DB);
        hotel = new ClS_Hotels(DB, session);
        config = new ClS_Config(DB, session);
    }


    private async Task<TableData<HotelsInfo>> GetPaginatedItems(TableState state)
    {
        PaginatedItems = await hotel!.HotelList<HotelsInfo>(
            SelectPro: 1,
            PageNumber: state.Page + 1,
            PageSize: state.PageSize,
            SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "htl_EntryDate" : state.SortLabel,
            SortDirection: Util.ResolveSort(state.SortDirection),
            DirectorateID: Filter.htl_DirectorateID,
            WorkPlaceID: Filter.htl_WorkPointID,
            FullName: Filter.htl_Name.ToEmptyOnNull());

        return new TableData<HotelsInfo>() { TotalItems = PaginatedItems.TotalItems, Items = PaginatedItems.Items };
    }
    async Task OnSearch(string e)
    {
        Filter.htl_Name = e;
        await table!.ReloadServerData();
    }

    private async Task OpenDialog(int id)
    {
        var options = new DialogOptions
        {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,MaxWidth=MaxWidth.Medium
        };
        
        var parameters = new DialogParameters();
        parameters.Add("config", config);
        parameters.Add("hotel", hotel);
        parameters.Add("HotelID", id);
        parameters.Add("OnAdd", EventCallback.Factory.Create(this, (async()=>await table.ReloadServerData())));

        var modal = DialogService.Show<AddHotel>(L["add-hotel"], parameters, options);
        var ModalResult = await modal.Result;
        
        if (!ModalResult.Cancelled)
            await table!.ReloadServerData();
    }
    private void OpenUsersModal(int id){
		var options = new DialogOptions {
            CloseOnEscapeKey = true,
            CloseButton = true,
            Position = DialogPosition.TopCenter,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
        };
        var parameters = new DialogParameters();
        parameters.Add("config", config);
        parameters.Add("hotel", hotel);
        parameters.Add("HotelID", id);

        DialogService.Show<HotelUsers>(L["users"],parameters, options);
    }
}