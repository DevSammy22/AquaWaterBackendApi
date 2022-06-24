using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaWater.Dto.Request
{
    public class ResetPasswordDTO
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        [Compare("NewPassword", ErrorMessage = "The fields New Password and Confirm Passwords should be equal")]
        public string ConfirmPassword { get; set; }
    }
}
