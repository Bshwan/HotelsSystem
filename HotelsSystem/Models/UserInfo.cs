namespace HotelsSystem.Models;

public class UserInfo
{
    public int Us_Code { get; set; }
    public string? Us_Name { get; set; }
    public string? Us_FullName { get; set; }
    public string? Us_password { get; set; }
    public bool Us_active { get; set; } = true;
    public DateTime Us_createDate { get; set; }
    public string? Us_createBy { get; set; }
    public string? Us_phoneNumber { get; set; }
    public string? Us_note { get; set; }
    public int Us_Language { get; set; }
    public int Us_WpID { get; set; }
    public string? Wp_Name { get; set; }
    public string? UserSecKey { get; set; }
    public int Us_PINCode { get; set; }
    public int Us_RightOfDelegate { get; set; }
    public decimal Us_SellDiscountRatio { get; set; }

    public string Us_sellDiscountRatio
    {
        get { return Us_SellDiscountRatio.ToString("#,##0.##"); }
        set { Us_SellDiscountRatio = decimal.TryParse(value, out decimal result) ? result : 0; }
    }

    public string? Us_modifyBy { get; set; }
    public DateTime Us_modifyDate { get; set; }
    public byte[]? Us_PersonalPhoto { get; set; }
}