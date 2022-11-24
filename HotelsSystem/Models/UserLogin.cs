using System.ComponentModel.DataAnnotations;

namespace HotelsSystem.Models
{
    public class UserLogin
    {
        [Required(ErrorMessageResourceType = typeof(Resources.App), ErrorMessageResourceName = "enter_username")]
        public string? UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.App), ErrorMessageResourceName = "enter_password")]
        public string? Password { get; set; }
    }
}