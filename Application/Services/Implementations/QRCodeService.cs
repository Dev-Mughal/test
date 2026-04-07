using Application.Constants;
using Application.Services.Interfaces;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using QRCoder;

namespace Application.Services.Implementations
{
    /// <summary>
    /// Generates and parses incentive codes in the format <c>{tableCode}-{id}</c>.
    /// Register as a singleton — the service is stateless.
    ///
    /// DB storage: <c>QRCode</c> column stores the code string (e.g. "10A-452", "10B-901").
    /// API responses: Include both the code string and the full QR image URL.
    /// </summary>
    public class QRCodeService : IQRCodeService
    {
        private const char   Separator  = '-';
        private const string QREndpoint = "/api/qr/";

        // ??? GenerateIncentiveCode ????????????????????????????????????????????
        // Stored in the QRCode column — the raw code string.
        public string GenerateIncentiveCode(string tableCode, long id)
        {
            if (!IncentiveTableCode.All.Contains(tableCode))
                throw new InvalidIncentiveCodeException(tableCode);

            return $"{tableCode}{Separator}{id}";
        }

        // ??? GenerateQRCodeImageUrl ????????????????????????????????????????????
        // Builds the absolute URL to the GET /api/qr/{code} endpoint.
        // Follows the same request-based pattern as IImageService.GetPublicImageUrl.
        public string GenerateQRCodeImageUrl(string code, HttpRequest request)
            => $"{request.Scheme}://{request.Host}{QREndpoint}{code}";

        // ??? RenderQRCodeImage ????????????????????????????????????????????????
        // Called only by GET /api/qr/{code} — generates PNG on-demand.
        // pixelsPerModule = 20 produces a ~200×200 px image at ECCLevel Q.
        public byte[] RenderQRCodeImage(string code)
        {
            using var generator = new QRCodeGenerator();
            using var data      = generator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            using var qr        = new PngByteQRCode(data);
            return qr.GetGraphic(20);
        }

        // ??? ParseIncentiveCode ??????????????????????????????????????????????
        // Expected format: {tableCode}-{id} (exactly 2 dash-separated parts)
        public (string tableCode, long id) ParseIncentiveCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new InvalidIncentiveCodeException(code ?? string.Empty, "code is empty");

            var parts = code.Split(Separator);

            if (parts.Length != 2)
                throw new InvalidIncentiveCodeException(code, $"expected 2 segments, got {parts.Length}");

            var tableCode = parts[0];
            if (!IncentiveTableCode.All.Contains(tableCode))
                throw new InvalidIncentiveCodeException(tableCode);

            if (!long.TryParse(parts[1], out var id) || id <= 0)
                throw new InvalidIncentiveCodeException(code, $"id segment '{parts[1]}' is not a valid positive integer");

            return (tableCode, id);
        }

        // ??? IsValidIncentiveCode ????????????????????????????????????????????
        // Wraps ParseIncentiveCode — safe to call for pre-validation without catching.
        public bool IsValidIncentiveCode(string code)
        {
            try   { ParseIncentiveCode(code); return true; }
            catch { return false; }
        }
    }
}
