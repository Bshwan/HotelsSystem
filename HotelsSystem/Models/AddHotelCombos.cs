namespace HotelsSystem.Models;

public class AddHotelCombos{
    public IEnumerable<HotelTypesComboBox> HotelTypes=Enumerable.Empty<HotelTypesComboBox>();
    public IEnumerable<DirectorateInfo> Directorates=Enumerable.Empty<DirectorateInfo>();
    public IEnumerable<WorkingPointInfo> WorkingPoints=Enumerable.Empty<WorkingPointInfo>();
    public IEnumerable<HotelRoomsTypesInfo> RoomTypes=Enumerable.Empty<HotelRoomsTypesInfo>();
    public IEnumerable<HotelFloorInfo> FloorTypes=Enumerable.Empty<HotelFloorInfo>();
}