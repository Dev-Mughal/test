using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Canonical service for generating and parsing incentive codes.
    /// Format: <c>{tableCode}-{id}</c>  e.g. <c>10A-452</c>, <c>10B-452</c>
    ///
    /// <c>QRCode</c> field in DB stores the code string itself.
    /// API responses include both the code string and the full QR image URL.
    /// </summary>
    public interface IQRCodeService
    {
        /// <summary>
        /// Builds the canonical incentive code: <c>{tableCode}-{id}</c>.
        /// </summary>
        string GenerateIncentiveCode(string tableCode, long id);

        /// <summary>
        /// Builds the absolute URL to the QR image endpoint: <c>https://domain/api/qr/{code}</c>.
        /// Used in API response DTOs so clients can embed the URL in an &lt;img&gt; tag.
        /// </summary>
        string GenerateQRCodeImageUrl(string code, HttpRequest request);

        /// <summary>
        /// Renders a PNG QR code image whose payload is the given <paramref name="code"/> string.
        /// Called only by the <c>GET /api/qr/{code}</c> endpoint.
        /// </summary>
        byte[] RenderQRCodeImage(string code);

        /// <summary>
        /// Parses a code produced by <see cref="GenerateIncentiveCode"/>.
        /// </summary>
        /// <returns>A tuple of the validated <c>tableCode</c> and the record <c>id</c>.</returns>
        /// <exception cref="Common.Exceptions.InvalidIncentiveCodeException">
        /// Thrown when the format is invalid or the table code is not recognised.
        /// </exception>
        (string tableCode, long id) ParseIncentiveCode(string code);

        /// <summary>
        /// Returns <c>true</c> if <paramref name="code"/> is a valid incentive code;
        /// <c>false</c> otherwise. Never throws.
        /// </summary>
        bool IsValidIncentiveCode(string code);
    }
}
