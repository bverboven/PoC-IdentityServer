using System.ComponentModel.DataAnnotations;

namespace Identity.Sts.Quickstart.Account
{
    public class LoginWithRecoveryCodeModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}