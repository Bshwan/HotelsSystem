namespace HotelsSystem.Models;

public class GuestMasterInfo
{
    public int GM_ID { get; set; }
    public int GM_Hotel { get; set; }
    public string? htl_Name { get; set; }
    public int GM_Room { get; set; }
    public string? htr_Detail { get; set; }
    public int htl_TypeID { get; set; }
    public string? congltype_Name { get; set; } 
    public int htr_FloorID { get; set; }
    public string? htf_FloorName { get; set; }
    public DateTime? GM_CheckIn { get; set; }
    public DateTime? GM_CheckOut { get; set; }
    public string? GM_Notes { get; set; }
    public string? GM_EntryByName { get; set; }
    public DateTime? GM_EntryDate { get; set; }
    public string? GM_UpdatedByName { get; set; }
    public DateTime? GM_UpdatedDate { get; set; }

}
