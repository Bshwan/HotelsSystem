using MudBlazor;

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
        private string SelectedColumnToSort = "wp_ID";
        private SortDirections sort = SortDirections.ASC;
        public WorkingPointInfo SelectedWorkPoint = new WorkingPointInfo();
        public WorkingPointInfo Filter = new WorkingPointInfo();

        private WorkPointCombo combo = new WorkPointCombo();
        private ClS_Config config = default!;
        private MudTable<WorkingPointInfo>? tableRef =new MudTable<WorkingPointInfo>();
        protected override async Task OnInitializedAsync()
        {

            var session = await Protection.GetDecryptedSession(JSRuntime, DB);
            mgmt = new ClS_UserManagement(DB, session);
            config = new ClS_Config(DB, session);


            await GetPaginatedWorkPoint();
            await GetCombo();

        }

        private async Task GetWorkPointByID(int id)
        {
            SelectedWorkPoint = (await config.GetAllInfo<WorkingPointInfo>(
                        SelectPro: 4,
                        ValID: id)).First();

        }

        private async Task GetCombo()
        {
            combo = await mgmt.WorkPointComboBox(SelectPro: 7);
           // combo.UserCombo = await config.GetCMB<UserAutoCombo>(SelectPro: 6);
        }

        private async Task<IEnumerable<UserAutoCombo>> SearchUser(string val)
        {
            return await Task.FromResult(combo.UserCombo.Where(x => x.peo_UserName.ToEmptyOnNull().Contains(val.ToEmptyOnNull())));
        }

        private void OnUserChanged(UserAutoCombo e)
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

        private async Task GetPaginatedWorkPoint(int page = 1, string ColumnName = "", SortDirection sort = SortDirection.None)
        {
            if (!string.IsNullOrWhiteSpace(ColumnName))
            {
                // if (sort == SortDirections.ASC)
                //     sort = SortDirections.DESC;
                // else
                //     sort = SortDirections.ASC;

                SelectedColumnToSort = ColumnName;
            }

            PaginatedWorkPoint = await config.GetGridPaging<WorkingPointInfo>(
                    SelectPro: 3,
                    PageNumber: page,
                    SortColumn: SelectedColumnToSort,
                    Search: Filter.peo_DirectorateName.ToEmptyOnNull(),
                    SortDirection: Util.ResolveSort(sort)
                    );
        }

        public async Task InsertUpdateWorkPoint()
        {
            if (config.IsSaveClicked1)
                return;
            try
            {

                config.IsSaveClicked1 = true;

                if (EnableDisableButton())
                {
                    config.IsInValidated1 = true;
                    return;
                }
                config.IsInValidated1 = false;

                SPResult result = await config.InsertUpdateConfig<SPResult>(
                SelectPro: 2,
                ValName: SelectedWorkPoint.wp_workpointName.ToEmptyOnNull(),
                ValueIDTwo: SelectedWorkPoint.wp_DirectorateID,
                ValueID: SelectedWorkPoint.wp_AdminID,
                ValueName:SelectedWorkPoint.location.ToEmptyOnNull(),
                ValID:SelectedWorkPoint.wp_ID);

                if (result.Result == 1)
                {
                    await GetPaginatedWorkPoint();
                    Toaster.Success(".", result.MSG);
                    SelectedWorkPoint = new WorkingPointInfo();
                    if(tableRef!=null)
                        await tableRef.ReloadServerData();

                    return;
                }
                Toaster.Error(".", result.MSG);
            }
            finally
            {
                config.IsSaveClicked1 = false;
            }
        }
        private async Task<TableData<WorkingPointInfo>> ServerReload(TableState state)
        {
            config.RowNumber = state.PageSize;
            await GetPaginatedWorkPoint(page: state.Page + 1, ColumnName: state.SortLabel, sort: state.SortDirection);
            return new TableData<WorkingPointInfo>() { TotalItems = PaginatedWorkPoint.TotalItems, Items = PaginatedWorkPoint.Items };
        }

        private bool EnableDisableButton()
        {
            return string.IsNullOrWhiteSpace(SelectedWorkPoint.wp_workpointName)
            || SelectedWorkPoint.wp_AdminID <= 0
            || SelectedWorkPoint.wp_DirectorateID <= 0;
        }
    }
}
