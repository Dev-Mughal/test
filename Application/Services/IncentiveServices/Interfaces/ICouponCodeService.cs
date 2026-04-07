namespace Application.Services.IncentiveServices.Interfaces
{
    public interface ICouponCodeService
    {
        /// <summary>
        /// Generates a unique QR code identifier embedding the incentive type code.
        /// Format: BZP-{typeCode}01-{randomSegment}
        /// </summary>
        string GenerateQRCode(string typeCode);

        /// <summary>
        /// Generates a short unique TrackCode for manual entry (e.g. "BZP-A3X9K2").
        /// </summary>
        string GenerateTrackCode();
    }
}

