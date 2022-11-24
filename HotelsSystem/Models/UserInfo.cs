namespace HotelsSystem.Models;

public class UserInfo
{
    public int peo_UserID { get; set; }
    public int peo_UserTypeID { get; set; }
    public string? usT_userType { get; set; }
    public string? peo_UserName { get; set; }
    public string? peo_userFullName { get; set; }
    public string? peo_UserMobile { get; set; }
    public int peo_DirectorateID { get; set; }
    public string? peo_DirectorateName { get; set; } 
    public string? peo_UserPassword { get; set; }
    public string? wp_workpointName { get; set; }

    public string? Us_password { get; set; }
    public bool peo_UserActive { get; set; } = true;
    public int peo_Language { get; set; } 
    public DateTime peo_createdDate { get; set; }
   


    public string? Us_modifyBy { get; set; }
    public DateTime Us_modifyDate { get; set; }
    public byte[]? Us_PersonalPhoto { get; set; }

}