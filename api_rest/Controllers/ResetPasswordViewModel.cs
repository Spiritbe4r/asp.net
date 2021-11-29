using System.ComponentModel.DataAnnotations;

namespace api_rest.Controllers
{
    public class ResetPasswordViewModel


    {
        [Required]
        public string Email { get; set; }

    }
}