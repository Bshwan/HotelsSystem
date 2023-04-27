namespace HotelsSystem.Models
{
    public static class BreadCrumbList
    {
        public static IEnumerable<BreadCrumbItem> Items = new List<BreadCrumbItem>(){
            new BreadCrumbItem { ID = 0, Name = "hotelery", ParentID = 0, Path = Routing.dashboard },
            new BreadCrumbItem { ID = 1, Name = "config", ParentID = 0, Path = Routing.config },
            new BreadCrumbItem { ID = 2, Name = "hotels", ParentID = 0, Path = Routing.hotels },
            new BreadCrumbItem { ID = 4, Name = "role", ParentID = 0, Path = Routing.roles },
            new BreadCrumbItem { ID = 5, Name = "add-user", ParentID = 0, Path = Routing.adduser },
            new BreadCrumbItem { ID = 6, Name = "user-list", ParentID = 0, Path = Routing.userlist },
            new BreadCrumbItem { ID = 7, Name = "user-list", ParentID = 0, Path = Routing.userlist },
            new BreadCrumbItem { ID = 8, Name = "search", ParentID = 0, Path = Routing.search },




        };
    }
}