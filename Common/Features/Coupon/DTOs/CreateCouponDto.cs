using Microsoft.AspNetCore.Http;
using System.ComponentModel;

namespace Common.Features.Coupon.DTOs
{
    public record CreateCouponDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? Photo { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        [Description("Optional. If omitted, defaults to EndDateTime.")]
        public DateTime? ExpirationTime { get; set; }
    }
}


