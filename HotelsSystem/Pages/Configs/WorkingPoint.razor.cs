namespace HotelsSystem.Pages.Configs
{
    public partial class WorkingPoint
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

        ClS_UserManagement mgmt = default!;

        private PagedResult<WorkingPointInfo> PaginatedWorkPoint = PagedResult<WorkingPointInfo>.EmptyPagedResult();
        public WorkingPointInfo SelectedWorkPoint = new WorkingPointInfo();
        public WorkingPointInfo Filter = new WorkingPointInfo();

        private WorkPointCombo combo = new WorkPointCombo();
        private ClS_Config config = default!;
        private MudTable<WorkingPointInfo>? tableRef;
        MudForm? AddForm;


        protected override async Task OnInitializedAsync()
        {

            var session = await Protection.GetDecryptedSession(JSRuntime, DB);
            mgmt = new ClS_UserManagement(DB, session);
            config = new ClS_Config(DB, session);
            await GetCombo();

        }

        private async Task GetWorkPointByID(int id)
        {
            SelectedWorkPoint = (await config.GetAllInfo<WorkingPointInfo>(SelectPro: 4, ValID: id)).First();
        }

        private async Task GetCombo()
        {
            combo = await mgmt.WorkPointComboBox(SelectPro: 7);
            // combo.UserCombo = await config.GetCMB<UserAutoCombo>(SelectPro: 6);
        }

        private async Task<IEnumerable<UserInfo>> SearchUser(string val)
        {
            return await Task.FromResult(combo.UserCombo.Where(x => x.peo_UserName.ToEmptyOnNull().Contains(val.ToEmptyOnNull())));
        }

        private void OnUserChanged(UserInfo e)
        {
            if (e == null)
            {
                SelectedWorkPoint.peo_UserName = string.Empty;
                SelectedWorkPoint.wp_AdminID = 0;
            }
            else
            {
                SelectedWorkPoint.peo_UserName = e.peo_UserName;
                SelectedWorkPoint.wp_AdminID = e.peo_UserID;
            }
        }

        private async Task<IEnumerable<DirectorateInfo>> SearchDirectorate(string val)
        {
            return await Task.FromResult(combo.Directorates.Where(x => x.peo_DirectorateName.ToEmptyOnNull().Contains(val.ToEmptyOnNull())));
        }

        private void OnDirectorateChanged(DirectorateInfo e)
        {
            if (e == null)
            {
                SelectedWorkPoint.peo_DirectorateName = string.Empty;
                SelectedWorkPoint.wp_DirectorateID = 0;
            }
            else
            {
                SelectedWorkPoint.peo_DirectorateName = e.peo_DirectorateName;
                SelectedWorkPoint.wp_DirectorateID = e.peo_DirectorateID;
            }
        }

        public async Task InsertUpdateWorkPoint()
        {
            await AddForm!.Validate();
            if (!AddForm.IsValid)
                return;

            SPResult result = await config.InsertUpdateConfig<SPResult>(
            SelectPro: 2,
            ValName: SelectedWorkPoint.wp_workpointName.ToEmptyOnNull(),
            ValueIDTwo: SelectedWorkPoint.wp_DirectorateID,
            ValueID: SelectedWorkPoint.wp_AdminID,
            ValueName: SelectedWorkPoint.location.ToEmptyOnNull(),
            ValID: SelectedWorkPoint.wp_ID);

            if (result.Result == 1)
            {
                await tableRef!.ReloadServerData();
                SelectedWorkPoint = new WorkingPointInfo();
                Toaster.Success(".", result.MSG);

                return;
            }
            Toaster.Error(".", result.MSG);
        }
        private async Task<TableData<WorkingPointInfo>> GetPaginatedItems(TableState state)
        {
            config.RowNumber = state.PageSize;

            PaginatedWorkPoint = await config.GetGridPaging<WorkingPointInfo>(
                    SelectPro: 3,
                    PageNumber: state.Page + 1,
                    PageSize: state.PageSize,
                    SortColumn: state.SortLabel.IsStringNullOrWhiteSpace() ? "wp_ID" : state.SortLabel,
                    Search: Filter.peo_DirectorateName.ToEmptyOnNull(),
                    SortDirection: Util.ResolveSort(state.SortDirection));

            return new TableData<WorkingPointInfo>() { TotalItems = PaginatedWorkPoint.TotalItems, Items = PaginatedWorkPoint.Items };
        }

        private string ValidateUser(UserInfo val)
        {
            if (val.peo_UserID <= 0)
                return L["required"];
            return null!;
        }
        
        private string ValidateDirectorate(DirectorateInfo val)
        {
            if (val.peo_DirectorateID <= 0)
                return L["required"];
            return null!;
        }
    }
}
