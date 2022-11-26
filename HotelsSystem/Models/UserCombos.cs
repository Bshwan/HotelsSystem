namespace HotelsSystem.Models;

public class UserCombos{
    public IEnumerable<GroupInfo> Groups=Enumerable.Empty<GroupInfo>();
    public IEnumerable<DataAccessPermissions> Permissions=Enumerable.Empty<DataAccessPermissions>();
    public IEnumerable<LanguageInfo> Languages=Enumerable.Empty<LanguageInfo>();
    public IEnumerable<DirectorateInfo> Directorates=Enumerable.Empty<DirectorateInfo>();
    public IEnumerable<UserTypesInfo> UserTypes=Enumerable.Empty<UserTypesInfo>();
    public IEnumerable<WorkingPointInfo> WorkingPointsPerUser=Enumerable.Empty<WorkingPointInfo>();
    public IEnumerable<WorkingPointInfo> WorkingPoints=Enumerable.Empty<WorkingPointInfo>();
}