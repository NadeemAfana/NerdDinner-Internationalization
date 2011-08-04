using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NerdDinner.Models
{
    public class LogOnModel
    {
        [Required(ErrorMessageResourceName = "YouMustSpecifyUsername", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelUsername", ResourceType=typeof(Resources.Resources)) ]        
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecifyPassword", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.Password)]
        [Display(Name = "ModelPassword", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [Display(Name = "ModelRememberMe", ResourceType = typeof(Resources.Resources))]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessageResourceName = "YouMustSpecifyUsername", ErrorMessageResourceType = typeof(Resources.Resources))]
        [Display(Name = "ModelUsername", ResourceType = typeof(Resources.Resources))]   
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecifyEmailAddress", ErrorMessageResourceType = typeof(Resources.Resources))]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "ModelEmail", ResourceType=typeof(Resources.Resources)) ]                
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "YouMustSpecifyPassword", ErrorMessageResourceType = typeof(Resources.Resources))]
        [StringLength(100, ErrorMessageResourceName = "PasswordTooShort", ErrorMessageResourceType = typeof(Resources.Resources), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "ModelPassword", ResourceType = typeof(Resources.Resources))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ModelConfirmPassword", ResourceType = typeof(Resources.Resources))]
        [Compare("Password", ErrorMessageResourceName = "NewPasswordAndConfirmationMismatch", ErrorMessageResourceType = typeof(Resources.Resources))]
        public string ConfirmPassword { get; set; }
    }

}