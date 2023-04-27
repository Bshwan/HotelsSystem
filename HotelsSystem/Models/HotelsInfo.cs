namespace HotelsSystem.Models;

public class HotelsInfo
{
    public int htl_ID { get; set; }
    public int htl_TypeID { get; set; }
    // public string? congltype_Name { get; set; }
    public bool HotelHas0Stars { get; set; }

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
    public string? congltype_Name { get; set; }
    public int congltype_StarNumber { get; set; }
    public int congltype_ID { get; set; }
    public decimal congltype_Price { get; set; }
    public string congltype_PriceStr
    {
        get { return congltype_Price.ToString("#,##0.##").Replace(",",""); }
        set { congltype_Price = decimal.TryParse(value, out decimal result) ? result : 0; }
    }
    public string? congltype_EntryBy { get; set; }
    public string? congltype_EntryByName { get; set; }

}
