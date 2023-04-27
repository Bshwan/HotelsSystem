namespace HotelsSystem.Pages.Configs
{
    public partial class HotelType
    {
        [Parameter, EditorRequired]
        public ISqlDataAccess DB { get; set; } = default!;

        [Parameter, EditorRequired]
        public NavigationManager nav { get; set; } = default!;

        [Parameter, EditorRequired]
        public IJSRuntime JSRuntime { get; set; } = default!;

        [Parameter, EditorRequired]
        public IToaster Toaster { get; set; } = default!;

        [Inject]
        protected IDialogService DialogService { get; set; } = default!;


        private PagedResult<HotelsInfo> PaginatedHotelType = PagedResult<HotelsInfo>.EmptyPagedResult();
        //private string SelectedColumnToSort = "peo_DirectorateID";
        //private SortDirections sort = SortDirections.ASC;
        public HotelsInfo SelectedHotelTYpe = new HotelsInfo();
        public HotelsInfo Filter = new HotelsInfo();

        private MudTable<HotelsInfo>? tableReff = new MudTable<HotelsInfo>();

      
        private ClS_Config config = default!;
        MudForm? AddForm;


        protected override async Task OnInitializedAsync()
        {
            var session = await Protection.GetDecryptedSession(JSRuntime, DB);
            config = new ClS_Config(DB, session);

            
        }

        private async Task GetHotelTypeByID(int id)
        {
            SelectedHotelTYpe = await config.GetOneInfo<HotelsInfo>(SelectPro: 13, ValID: id);
        }

     

    

  

        public async Task InsertUpdateHotelType()
        {
            await AddForm!.Validate();
            if (!AddForm.IsValid)
                return;

            SPResult result = await config.InsertUpdateConfig<SPResult>(
            SelectPro: 5,
            ValName: SelectedHotelTYpe.congltype_Name.ToEmptyOnNull(),
            ValID: SelectedHotelTYpe.congltype_ID,
            ValueID: SelectedHotelTYpe.congltype_StarNumber,
            Price:SelectedHotelTYpe.congltype_Price);

            if (result.Result == 1)
            {
                SelectedHotelTYpe = new HotelsInfo();
                if (tableReff != null)
                    await tableReff.ReloadServerData();
                Toaster.Success(".", result.MSG);
                return;
            }
            Toaster.Error(".", result.MSG);
        }
        private async Task<TableData<HotelsInfo>> GetPaginatedhoteltype(TableState state)
        {
            PaginatedHotelType = await config.GetGridPaging<HotelsInfo>(
                    SelectPro: 7,
                    PageNumber: state.Page + 1,
                    PageSize: state.PageSize,
                    SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "congltype_Name" : state.SortLabel,
                    Search: Filter.peo_DirectorateName.ToEmptyOnNull(),
                    SortDirection: Util.ResolveSort(state.SortDirection));

            return new TableData<HotelsInfo>() { TotalItems = PaginatedHotelType.TotalItems, Items = PaginatedHotelType.Items };
        }









    }
}
