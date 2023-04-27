namespace HotelsSystem.Models;
public class NavInfo{
    public int nav_Id { get; set; }
    public string? nav_NameOption { get; set; }
    public string? nav_controller { get; set; }
    public string? nav_action { get; set; }
    public int nav_parentId { get; set; }
    public bool nav_isParent { get; set; }
    public string? nav_Icon { get; set; }
    public int nav_Seq { get; set; }
}