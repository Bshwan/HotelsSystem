namespace HotelsSystem.Permissions
{
    public static class Permissions
    {
        public static IEnumerable<PermissionsInfo> AllPermissions = new List<PermissionsInfo>()
        {
            new PermissionsInfo{PermissionName="admin",PermissionsID=new List<int>{1}}
            // new PermissionsInfo{PermissionName=Routing.generalconfigurations,PermissionsID=new List<int>{1,28}},
            
        };
    }
}
