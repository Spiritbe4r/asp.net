using System.ComponentModel.DataAnnotations;

namespace api_rest.ViewModels
{
    public class ConfirmEmailViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}