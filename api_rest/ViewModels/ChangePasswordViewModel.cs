using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_rest.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
