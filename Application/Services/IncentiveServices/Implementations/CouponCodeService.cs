using Application.Services.IncentiveServices.Interfaces;
using System.Text;

namespace Application.Services.IncentiveServices.Implementations
{
    public class CouponCodeService : ICouponCodeService
    {
        private const string TrackCodeChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789"; // no ambiguous chars (0,O,1,I)
        private const int TrackCodeLength = 8;
        private const string RandomSegmentChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        private const int RandomSegmentLength = 10;

        /// <summary>
        /// Generates a unique QR code content identifier embedding the incentive type code.
        /// Format: BZP-{typeCode}01-{randomSegment}
        /// Example: BZP-B01-A3X9K2MNPQ
        /// </summary>
        public string GenerateQRCode(string typeCode)
        {
            var randomSegment = GenerateRandomSegment(RandomSegmentLength);
            return $"BZP-{typeCode}01-{randomSegment}";
        }

        /// <summary>
        /// Generates a short human-readable unique TrackCode for manual entry.
        /// Format: XXX-XXXXX (8 unambiguous alphanumeric characters)
        /// </summary>
        public string GenerateTrackCode()
        {
            var random = Random.Shared;
            var sb = new StringBuilder(TrackCodeLength + 1);

            for (int i = 0; i < TrackCodeLength; i++)
            {
                if (i == 3) sb.Append('-');
                sb.Append(TrackCodeChars[random.Next(TrackCodeChars.Length)]);
            }

            return sb.ToString();
        }

        private static string GenerateRandomSegment(int length)
        {
            var random = Random.Shared;
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(RandomSegmentChars[random.Next(RandomSegmentChars.Length)]);
            return sb.ToString();
        }
    }
}

