namespace HotelsSystem.Models;

public class HotelUsersInfo
{
    public int htlus_ID { get; set; }
    public string htlus_Name { get; set; }
    public string htlus_FullName { get; set; }
    public string htlus_Password { get; set; }
    public int htlus_HotelID { get; set; }
    public string htl_Name { get; set; }
    public int htlus_TypeID { get; set; }
    public string htlustype_Name { get; set; }
    public int htlus_LanguageID { get; set; }
    public bool htlus_Active { get; set; }
    public string htlus_EntryByName { get; set; }
    public string htlus_EntryDate { get; set; }

}
