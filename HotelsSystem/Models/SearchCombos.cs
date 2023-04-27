namespace HotelsSystem.Models;

public class SearchCombos{
    public IEnumerable<DirectorateInfo> Directorates=Enumerable.Empty<DirectorateInfo>();
    public IEnumerable<GenderInfo> Genders=Enumerable.Empty<GenderInfo>();
    public IEnumerable<NationalityInfo> Nationalities=Enumerable.Empty<NationalityInfo>();
    public IEnumerable<WorkingPointInfo> WorkingPoints=Enumerable.Empty<WorkingPointInfo>();
    public IEnumerable<HotelsInfo> Hotels=Enumerable.Empty<HotelsInfo>();
    public IEnumerable<HotelRoomsInfo> Rooms=Enumerable.Empty<HotelRoomsInfo>();
}