namespace HotelsSystem.Models;

public class HotelsInfo
{
    public int htl_ID { get; set; }
    public int htl_TypeID { get; set; }
    public string? HTT_Type { get; set; }
    public string? htl_Name { get; set; }
    public string? htl_Note { get; set; }
    public string? htl_Address { get; set; }
    public int htl_Star { get; set; }
    public int htl_NumberOfRooms { get; set; }
    public int htl_DirectorateID { get; set; }
    public string? peo_DirectorateName { get; set; } 
    public int htl_WorkPointID { get; set; }
    public string? wp_workpointName { get; set; }
    public DateTime? htl_EntryDate { get; set; }
    public string? htl_EntryByName { get; set; }

}
