namespace HotelsSystem.Models;

public class HotelRoomsInfo
{
    public int htr_ID { get; set; }
    public string htr_Detail { get; set; }
    public int htr_HotelID { get; set; }
    public int htl_TypeID { get; set; }
    public string HTT_Type { get; set; }
    public int htr_FloorID { get; set; }
    public string hrf_FloorName { get; set; }
    public string htl_Name { get; set; }
    public int htr_Type { get; set; }
    public string cfg_HTR_Type { get; set; }
    public int htr_NumberOfBed { get; set; }
    public string htr_EntryByName { get; set; }
    public DateTime htr_EntryDate { get; set; }

}
