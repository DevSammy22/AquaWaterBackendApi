using System;
using System.ComponentModel.DataAnnotations;

namespace AquaWater.Dto.Request
{
    public class VerifyOtpDTO
    {
        [Required]
        [Range(100000, double.MaxValue)]
        public long OTP { get; set; }

        [Required]
        public string OrderId { get; set; }
    }
}
