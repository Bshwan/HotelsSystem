
using HotelsSystem.Pages.Hotels;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using PdfReport;
using System.IO;
using System.Reflection;
using static MudBlazor.CategoryTypes;

namespace HotelsSystem.Pages.Report;
public partial class ReportActionLogs
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
    protected IStringLocalizer<App> L { get; set; } = default!;
    [Inject]
    protected IDialogService DialogService { get; set; } = default!;
    [Inject]
    public ISessionStorageService storage { get; set; } = default!;
    [Inject]
    public IBlazorDownloadFileService Downloader { get; set; } = default!;

    PagedResult<ActionlogInfo> PaginatedItems = PagedResult<ActionlogInfo>.EmptyPagedResult();
    private MudTable<ActionlogInfo>? table;
    ClS_Reports? report;
    ClS_Config config = default!;
    ActionlogInfo Filter = new ActionlogInfo();
    SPResult? session;
    private PdfExport pdf = default!;
    IEnumerable<UserInfo> Users = Enumerable.Empty<UserInfo>();
    IEnumerable<ActionTypeInfo> ActionTypes = Enumerable.Empty<ActionTypeInfo>();
    List<UserTypesInfo> UserTypes = new List<UserTypesInfo> { new UserTypesInfo { usT_ID = 1, usT_userType = "Admin" }, new UserTypesInfo { usT_ID = 2, usT_userType = "Reciption" } };

    protected override async Task OnInitializedAsync()
    {
        session = await Protection.GetDecryptedSession(jSRuntime, DB, storage);
        report = new ClS_Reports(DB, session);
        config = new ClS_Config(DB, session);
        pdf = new PdfExport(Toaster, L, config, nav, isRTL: !Util.CurrentLang());
        await GetActionTypes();
    }
    async Task GetUsersByType()
    {
        if (Filter.UserTypeID <= 0)
        {
            Users = Enumerable.Empty<UserInfo>();
            return;
        }
        Users = await config.GetCMB<UserInfo>(SelectPro: 17, ValID: Filter.UserTypeID);
    }
    async Task OnSelectedUserTypeChanged(UserTypesInfo e)
    {
        if (e != null)
        {
            Filter.UserTypeID = e.usT_ID;
            Filter.UserTypeName = e.usT_userType;
        }
        else
        {
            Filter.UserTypeID = 0;
            Filter.UserTypeName = "";
        }
        await GetUsersByType();
    }
    void OnSelectedUserChanged(UserInfo e)
    {
        if (e != null)
        {
            Filter.UserID = e.peo_UserID;
            Filter.actionlog_UserName = e.peo_UserName;
        }
        else
        {
            Filter.UserID = 0;
            Filter.actionlog_UserName = "";
        }
    } void OnSelectedActionTypeChanged(ActionTypeInfo e)
    {
        if (e != null)
        {
            Filter.actionlog_ActionType = e.actionlogtype_ID;
            Filter.actionlogtype_Name = e.actionlogtype_Name;
        }
        else
        {
            Filter.actionlog_ActionType = 0;
            Filter.actionlogtype_Name = "";
        }
    }
    async Task ClearFilter()
    {
        Filter = new ActionlogInfo();
        await OnSelectedUserTypeChanged(null!);
        await table.ReloadServerData();
    }
    async Task GetActionTypes()
    {
        ActionTypes = await config.GetCMB<ActionTypeInfo>(SelectPro: 18);
    }
    SortDirection sort = SortDirection.Ascending;
    string SelectedColumnToSort = "actionlog_ID";
    private async Task<TableData<ActionlogInfo>> GetPaginatedItems(TableState state)
    {
        sort = state.SortDirection;
        SelectedColumnToSort = state.SortLabel.IsStringNullOrWhiteSpace() ? "actionlog_ID" : state.SortLabel;
        PaginatedItems = await report!.Pro_ReportActionLog<ActionlogInfo>(
            SelectPro: 1,
            PageNumber: state.Page + 1,
            PageSize: state.PageSize,
            SortColumn: SelectedColumnToSort,
            UserType: Filter.UserTypeID,
            UserID: Filter.UserID.ToString(),
            ActionType: Filter.actionlog_ActionType,
            TableName: Filter.actionlogtype_Name.ToEmptyOnNull(),
            FieldValue: Filter.actionlog_Value.ToEmptyOnNull(),
            FromDate: Filter.actionlog_EntryDate.ToyyyyMMddElseEmpty(),
            ToDate: Filter.actionlog_EntryDate2.ToyyyyMMddElseEmpty(),
            SortDirection: Util.ResolveSort(sort));

        return new TableData<ActionlogInfo>() { TotalItems = PaginatedItems.TotalItems, Items = PaginatedItems.Items };
    }
    private async Task ExportToPdf()
    {
        var Columns = new string[] {
             L["Username"],
    L["Action Type"],
    L["Table Name"],
    L["Field Name"],
    L["Field Value"],
    L["Old Value"],
    L["Entry Date"]
        };
        string FileName = L["action-log-report"];

        var items = await report!.Pro_ReportActionLog<ActionlogInfo>(
            SelectPro: 1,
            ExportToExcel:1,
            SortColumn: SelectedColumnToSort,
            UserType: Filter.UserTypeID,
            UserID: Filter.UserID.ToString(),
            ActionType: Filter.actionlog_ActionType,
            TableName: Filter.actionlogtype_Name.ToEmptyOnNull(),
            FieldValue: Filter.actionlog_Value.ToEmptyOnNull(),
            FromDate: Filter.actionlog_EntryDate.ToyyyyMMddElseEmpty(),
            ToDate: Filter.actionlog_EntryDate2.ToyyyyMMddElseEmpty(),
            SortDirection: Util.ResolveSort(sort));

        if (!items.Items.Any())
        {
            Toaster.Error(".", L["no-data-available"]);
            return;
        }
        var SelectedItems = items.Items.Select(x => new
        {
            UserName = x.actionlog_UserName.ToEmptyOnNull(),
            ActionType = x.actionlogtype_Name.ToEmptyOnNull(),
            TableName = x.actionlog_TableName.ToEmptyOnNull(),
            FieldName = x.actionlog_FieldName.ToEmptyOnNull(),
            FieldValue = x.actionlog_Value.ToEmptyOnNull(),
            OldValue = x.actionlog_OldValue.ToEmptyOnNull(),
            EntryDate = x.actionlog_EntryDate.ToddMMyyyyhhmmsstt()
        }).ToList();

        var stream = await pdf.PdfTable(

        ReportName: FileName,
        Items: SelectedItems,
        Totals: new List<PdfTotalInfo>(),
        ColumnNames: Columns,
        wp:0);

        await Downloader.DownloadFile("Report Action Log", stream, contentType: Util.PdfContentType);

    }
}