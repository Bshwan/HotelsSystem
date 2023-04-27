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
            BreadCrumbFun(ParentID: FindPage(), FindPageResult: FindPage(), NotFirstTime: false);
            StateHasChanged();
        }

        void IDisposable.Dispose()
        {
            UriHelper.LocationChanged -= LocationChanged;
        }


        public void BreadCrumbFun(int ParentID , int FindPageResult , bool NotFirstTime)
        {

            if (NotFirstTime)
            {
                ParentID = BreadCrumbList.Items.Where(x => x.ID == ParentID).Select(x => x.ParentID).FirstOrDefault();

            }
            if (ParentID > 0)
            {

                BreadCrumbFun(ParentID: ParentID, FindPageResult: FindPageResult, NotFirstTime: true);
            }
            if (BreadCrumbList.Items.Where(x => x.ID == ParentID).Select(x => x.ID).FirstOrDefault() == FindPageResult)
            {
                //The Active Page
                BreadCrumbResult.items.Add(new BreadcrumbItem(L[BreadCrumbList.Items.Where(x => x.ID == ParentID).Select(x => x.Name.ToEmptyOnNull()).First()], href: null, disabled: true));

            }
            else
            {
                //Parent Pages
                BreadCrumbResult.items.Add(new BreadcrumbItem(L[BreadCrumbList.Items.Where(x => x.ID == ParentID).Select(x => x.Name.ToEmptyOnNull()).First()],disabled:BreadCrumbList.Items.Where(x => x.ID == ParentID).Select(x => x.Path.ToEmptyOnNull()).First().Equals(Routing.dashboard), href: @BreadCrumbList.Items.Where(x => x.ID == ParentID).Select(x => x.Path).FirstOrDefault()));

            }

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