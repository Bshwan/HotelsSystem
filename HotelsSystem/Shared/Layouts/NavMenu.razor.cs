namespace HotelsSystem.Shared.Layouts;
public partial class NavMenu
{
    [Inject]
    protected IJSRuntime jSRuntime { get; set; } = default!;
    [Inject]
    protected ISqlDataAccess DB { get; set; } = default!;
    bool _isNavExpanded = true;
    private bool collapseNavMenu = true;

    private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }
    ClS_Config config = default!;
    SPResult? session;
    IEnumerable<NavInfo> Nav = Enumerable.Empty<NavInfo>();
    protected override async Task OnInitializedAsync()
    {
        session = await Protection.GetDecryptedSession(jSRuntime, DB);
        config = new ClS_Config(DB, session);
        await GetNav();
    }
    async Task GetNav()
    {
        Nav = await config.GetAllInfo<NavInfo>(SelectPro: 11);
    }
    Dictionary<string, string> IconMapper = new Dictionary<string, string>(){
        {"Domain",Icons.Filled.Domain},
        {"Settings",Icons.Filled.Settings},
        {"AdminPanelSettings",Icons.Filled.AdminPanelSettings},
        {"People",Icons.Filled.People},
        {"Security",Icons.Filled.Security},
        {"search",Icons.Filled.Search},
    };
    string GetIconByText(string text)
    {
        var find = IconMapper.Where(x => x.Key.ContainsIgnoreCase(text));
        if (find.Any())
            return find.First().Value;
        return "";
    }

}