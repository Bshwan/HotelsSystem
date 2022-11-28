using System.ComponentModel.DataAnnotations;

namespace HotelsSystem.Models;

public class ChangePasswordInfo
{
   [Required(ErrorMessageResourceName = "enter_old_password", ErrorMessageResourceType = typeof(Resources.App))]
    public string? OldPassword { get; set; }
   [Required(ErrorMessageResourceName = "enter_new_password", ErrorMessageResourceType = typeof(Resources.App))]
    public string? NewPassword { get; set; }
    [Required(ErrorMessageResourceName = "enter_verify_password", ErrorMessageResourceType = typeof(Resources.App))]
    [Compare(nameof(NewPassword), ErrorMessageResourceName = "new_verify_password_not_same", ErrorMessageResourceType = typeof(Resources.App))]
    public string? verifyPassword { get; set; }
}
