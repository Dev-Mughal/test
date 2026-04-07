namespace Common.Features.Coupon.DTOs
{
    public record CouponResponseDto
    (
        long Id,
        string Title,
        string Description,
        string? PhotoUrl,
        string TrackCode,      // The code string itself (e.g. 10A-452 or 10B-452) for manual entry / scanning
        string? QRCodeImageUrl, // Full URL to GET /api/qr/{code} for QR display
        DateTime StartDateTime,
        DateTime EndDateTime,
        DateTime ExpirationTime,
        bool IsActive,
        bool IsFeatured,
        DateTime CreatedOn
    );
}

