using Microsoft.AspNetCore.Components.Routing;
using System.Text.RegularExpressions;

namespace HotelsSystem.Shared.Layouts.MainLayoutItems
{
    public partial class BreadCrumbItems
    {
        [Inject]
        protected NavigationManager UriHelper { get; set; }=default!;
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>();
        protected override Task OnInitializedAsync()
        {
            UriHelper.LocationChanged += LocationChanged;
            return Task.CompletedTask;
        }

        private void LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            BreadCrumbResult.items.Clear();
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            UriHelper.LocationChanged -= LocationChanged;
        }

        public int FindPage()
        {
            string uri = UriHelper.Uri;
            if (uri == null)
            {
                return 0;
            }
            else
            {
                foreach (var item in BreadCrumbList.Items)
                {
                    Regex rgx = new Regex(@"\b" + item.Path.ToEmptyOnNull().ToLower() + @"\b($|/)");
                    
                    // System.Diagnostics.Debug.WriteLine(item + " is equal:" + uri + "--?");
                    if (rgx.IsMatch(uri.ToLower()))
                    {
                        return item.ID;
                    }
                }
            }

            return 0;
        }
    }
}