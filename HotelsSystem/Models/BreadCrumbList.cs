namespace HotelsSystem.Models
{
    public static class BreadCrumbList
    {
        public static IEnumerable<BreadCrumbItem> Items = new List<BreadCrumbItem>(){
            new BreadCrumbItem { ID = 0, Name = "dashboard", ParentID = 0, Path = Routing.dashboard },
            new BreadCrumbItem { ID = 1, Name = "config", ParentID = 0, Path = Routing.config },
            new BreadCrumbItem { ID = 2, Name = "role", ParentID = 1, Path = Routing.roles },




        };
    }
}