using Microsoft.JSInterop;

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
        private string SelectedColumnToSort = "peo_DirectorateID";
        private SortDirections sort = SortDirections.ASC;
        public DirectorateInfo SelectedDirectorate = new DirectorateInfo();
        public DirectorateInfo Filter = new DirectorateInfo();

        private MudTable<DirectorateInfo>? tableRef = new MudTable<DirectorateInfo>();

        private IEnumerable<UserAutoCombo> combo = Enumerable.Empty<UserAutoCombo>();
        private ClS_Config config = default!;

        protected override async Task OnInitializedAsync()
        {

            var session = await Protection.GetDecryptedSession(JSRuntime, DB);
            config = new ClS_Config(DB, session);


            await GetPaginatedDirectorate();
            await GetCombo();
       
        }

        private async Task GetDirectorateByID(int id)
        {
            SelectedDirectorate = (await config.GetAllInfo<DirectorateInfo>(
                        SelectPro: 3,
                        ValID: id)).First();
            
        }

        private async Task GetCombo()
        {
            combo = await config.GetCMB<UserAutoCombo>(SelectPro: 6);
        }

        private async Task<IEnumerable<UserAutoCombo>> SearchIncomes(string val)
        {
            return await Task.FromResult(combo.Where(x => x.peo_UserName.ToEmptyOnNull().Contains(val.ToEmptyOnNull())));
        }

        private void OnIncomeChanged(UserAutoCombo e)
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

        private async Task GetPaginatedDirectorate(int page = 1, string ColumnName = "", SortDirection sort = SortDirection.None)
        {
            if (!string.IsNullOrWhiteSpace(ColumnName))
            {
                // if (sort == SortDirections.ASC)
                //     sort = SortDirections.DESC;
                // else
                //     sort = SortDirections.ASC;

                SelectedColumnToSort = ColumnName;
            }

            PaginatedDirectorate = await config.GetGridPaging<DirectorateInfo>(
                    SelectPro: 2,
                    PageNumber: page,
                    SortColumn: SelectedColumnToSort,
                    Search:Filter.peo_DirectorateName.ToEmptyOnNull(),
                    SortDirection: Util.ResolveSort(sort)
                    );
        }

        public async Task InsertUpdateDirectorate()
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
                SelectPro: 1,
                ValName: SelectedDirectorate.peo_DirectorateName.ToEmptyOnNull(),
                ValID: SelectedDirectorate.peo_DirectorateID,
                ValueID: SelectedDirectorate.peo_dirAdminUserID);
                
                if (result.Result == 1)
                {
                    await GetPaginatedDirectorate();
                    Toaster.Success(".", result.MSG);
                    SelectedDirectorate = new DirectorateInfo();
                    if (tableRef != null)
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
        private async Task<TableData<DirectorateInfo>> ServerReload(TableState state)
        {
            config.RowNumber = state.PageSize;
            await GetPaginatedDirectorate(page: state.Page + 1, ColumnName: state.SortLabel,sort:state.SortDirection);
            return new TableData<DirectorateInfo>() { TotalItems = PaginatedDirectorate.TotalItems, Items = PaginatedDirectorate.Items };
        }

        private bool EnableDisableButton()
        {
            return string.IsNullOrWhiteSpace(SelectedDirectorate.peo_DirectorateName)
            || SelectedDirectorate.peo_dirAdminUserID <= 0;
        }
    }
}
