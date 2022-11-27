namespace HotelsSystem.Models;

public class GuestDetailsInfo
{
    public int GD_ID { get; set; }
    public int GD_GM { get; set; }
    public string? GD_Fullname { get; set; }
    public string? GD_Surname { get; set; }
    public string? GD_MotherName { get; set; }
    public string? GD_Mobile { get; set; }
    public string? GD_IdNumber { get; set; }
    public int GD_Gender { get; set; }
    public string? gen_Name { get; set; }
    public DateTime GD_DOB { get; set; }
    public int GD_Nationality { get; set; }
    public string? nat_Name { get; set; }
    public DateTime GD_CheckIn { get; set; }
    public DateTime GD_ChechOut { get; set; }
    public string? GD_Note { get; set; }
    public string? GD_EntryByName { get; set; }
    public DateTime GD_EntryDate { get; set; } 
    public string? GD_UpdatedByName { get; set; }
    public DateTime GD_UpdatedDate { get; set; }
    public string? htl_Name { get; set; }
    public string? htl_Address { get; set; }
    public int GM_Room { get; set; }
    public string? htr_Detail { get; set; }
    public int htr_Type { get; set; }
    public string? cfg_HTR_Type { get; set; }
    public int htr_FloorID { get; set; }
    public string? htf_FloorName { get; set; }
    public string? peo_DirectorateName { get; set; }
    public string? wp_workpointName { get; set; }
    public int htr_NumberOfBed { get; set; }
    public int DirectorateID { get; set; }
    public int WorkplaceID { get; set; }
    public int HotelID { get; set; }
    public int RoomID { get; set; }
    public int GenderID { get; set; }
    public int NationalityID { get; set; }
    public DateTime? FromCheckInDate { get; set; }
    public DateTime? ToCheckInDate { get; set; }
    public DateTime? FromCheckOutDate { get; set; }
    public DateTime? ToCheckOutDate { get; set; }
    //public DateTime GM_CheckIn { get; set; }
    //public DateTime GM_CheckOut { get; set; }



     
}
