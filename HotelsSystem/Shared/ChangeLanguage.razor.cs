namespace HotelsSystem.Shared;
    public partial class ChangeLanguage
    {
        private Dictionary<string, string> cultures=new Dictionary<string, string>();

        [Inject]
        protected NavigationManager nav { get; set; }=default!;

        private Task RequestCultureChange(string Culture, int CultureID)
        {
            if (string.IsNullOrWhiteSpace(Culture))
            {
                return Task.CompletedTask;
            }

            var uri = new Uri(nav.Uri)
                .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
            System.Diagnostics.Debug.WriteLine("uri:" + uri);
            var query = $"?culture={Uri.EscapeDataString(Culture)}&" +
                $"redirectUri={Uri.EscapeDataString(uri)}";

            nav.NavigateTo("/Culture/SetCulture" + query, forceLoad: true);
            return Task.CompletedTask;
        }
    }
