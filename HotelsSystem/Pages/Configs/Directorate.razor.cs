namespace HotelsSystem.Pages.Configs
{
    public partial class Directorate
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

        private PagedResult<DirectorateInfo> PaginatedDirectorate = PagedResult<DirectorateInfo>.EmptyPagedResult();
        //private string SelectedColumnToSort = "peo_DirectorateID";
        //private SortDirections sort = SortDirections.ASC;
        public DirectorateInfo SelectedDirectorate = new DirectorateInfo();
        public DirectorateInfo Filter = new DirectorateInfo();

        private MudTable<DirectorateInfo>? tableRef = new MudTable<DirectorateInfo>();

        private IEnumerable<UserInfo> combo = Enumerable.Empty<UserInfo>();
        private ClS_Config config = default!;
        MudForm? AddForm;


        protected override async Task OnInitializedAsync()
        {
            var session = await Protection.GetDecryptedSession(JSRuntime, DB);
            config = new ClS_Config(DB, session);

            await GetCombo();
        }

        private async Task GetDirectorateByID(int id)
        {
            SelectedDirectorate = await config.GetOneInfo<DirectorateInfo>(SelectPro: 3, ValID: id);
        }

        private async Task GetCombo()
        {
            combo = await config.GetCMB<UserInfo>(SelectPro: 6);
        }

        private async Task<IEnumerable<UserInfo>> SearchUser(string val)
        {
            return await Task.FromResult(combo.Where(x => x.peo_UserName.ToEmptyOnNull().Contains(val.ToEmptyOnNull())));
        }

        private void OnUserChanged(UserInfo e)
        {
            if (e == null)
            {
                SelectedDirectorate.peo_UserName = string.Empty;
                SelectedDirectorate.peo_dirAdminUserID = 0;
            }
            else
            {
                SelectedDirectorate.peo_UserName = e.peo_UserName;
                SelectedDirectorate.peo_dirAdminUserID = e.peo_UserID;
            }
        }

        public async Task InsertUpdateDirectorate()
        {
            await AddForm!.Validate();
            if (!AddForm.IsValid)
                return;

            SPResult result = await config.InsertUpdateConfig<SPResult>(
            SelectPro: 1,
            ValName: SelectedDirectorate.peo_DirectorateName.ToEmptyOnNull(),
            ValID: SelectedDirectorate.peo_DirectorateID,
            ValueID: SelectedDirectorate.peo_dirAdminUserID);

            if (result.Result == 1)
            {
                SelectedDirectorate = new DirectorateInfo();
                if (tableRef != null)
                    await tableRef.ReloadServerData();
                Toaster.Success(".", result.MSG);
                return;
            }
            Toaster.Error(".", result.MSG);
        }
        private async Task<TableData<DirectorateInfo>> GetPaginatedItems(TableState state)
        {
            PaginatedDirectorate = await config.GetGridPaging<DirectorateInfo>(
                    SelectPro: 2,
                    PageNumber: state.Page + 1,
                    PageSize: state.PageSize,
                    SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "peo_DirectorateID" : state.SortLabel,
                    Search: Filter.peo_DirectorateName.ToEmptyOnNull(),
                    SortDirection: Util.ResolveSort(state.SortDirection));

            return new TableData<DirectorateInfo>() { TotalItems = PaginatedDirectorate.TotalItems, Items = PaginatedDirectorate.Items };
        }
    }
}
