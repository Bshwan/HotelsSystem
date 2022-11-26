namespace HotelsSystem.Models;

public class WorkingPointInfo
{
    public int wp_ID { get; set; }
    public int wp_DirectorateID { get; set; }
    public string? peo_DirectorateName { get; set; }
    public string? wp_workpointName { get; set; }
    public int wp_AdminID { get; set; }
    public int HasRole { get; set; }
    public int per_ID { get; set; }
    public string? peo_UserName { get; set; }
    public string? wp_locationText { get; set; }

}
