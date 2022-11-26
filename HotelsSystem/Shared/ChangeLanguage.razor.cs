namespace HotelsSystem.Shared;
    public partial class ChangeLanguage
    {
        private Dictionary<string, string> cultures=new Dictionary<string, string>();

        [Inject]
        protected NavigationManager nav { get; set; }=default!;
        [Inject]
        protected ISqlDataAccess DB{get;set;}=default!;
        [Inject]
        protected IJSRuntime jSRuntime{get;set;}=default!;
        ClS_UserManagement mgmt=default!;

        protected override async Task OnInitializedAsync(){
            var session=await Protection.GetDecryptedSession(jSRuntime,DB);
            mgmt=new ClS_UserManagement(DB,session);
        }

        private async Task RequestCultureChange(string Culture, int CultureID)
        {
            if (string.IsNullOrWhiteSpace(Culture))
            {
                return ;
            }
            SPResult result=await mgmt.InsertUpdateUser<SPResult>(SelectPro:3,Language:CultureID);

            var uri = new Uri(nav.Uri)
                .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            var query = $"?culture={Uri.EscapeDataString(Culture)}&" +
                $"redirectUri={Uri.EscapeDataString(uri)}";

            nav.NavigateTo("/Culture/SetCulture" + query, forceLoad: true);
        }
        
    }
