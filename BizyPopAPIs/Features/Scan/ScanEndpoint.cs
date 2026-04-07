using Application.Services.Interfaces;
using Common.Features.Scan;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Scan
{
    public static class ScanEndpoint
    {
        public static IEndpointRouteBuilder MapScanEndpoints(this IEndpointRouteBuilder app)
        {
            // ??? POST /api/scan — unified endpoint for QR scan + manual track code entry ????
            app.MapPost("/api/scan", async (
                [FromBody] IncentiveScanRequest request,
                [FromServices] IIncentiveScanService scanService) =>
            {
                var result = await scanService.ScanAsync(request.Code).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Incentive record retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<IncentiveScanResult>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription(
                "Accepts an incentive code ({tableCode}-{id}) from either a QR code scan or manual " +
                "track code entry and returns the matching incentive definition record.")
            .WithName("ScanIncentive");

            // ??? GET /api/scan/{code} — direct scan URL target for QR payload ????????????????????
            // This allows QR payload to point to a full endpoint URL while using the same scan logic.
            app.MapGet("/api/scan/{code}", async (
                string code,
                [FromServices] IIncentiveScanService scanService) =>
            {
                var result = await scanService.ScanAsync(code).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Incentive record retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<IncentiveScanResult>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription(
                "Accepts an incentive code in the route and returns the matching incentive record. " +
                "Used as the direct URL target embedded inside QR images.")
            .WithName("ScanIncentiveByCode");

            // ??? GET /api/qr/{code} — render PNG QR image on-demand ????????????????????
            // DTOs send the full URL to this endpoint in the QRCodeImageUrl field.
            // Clients embed it as <img src="https://domain/api/qr/BZ-10A-452" />.
            app.MapGet("/api/qr/{code}", (
                string code,
                HttpRequest request,
                [FromServices] IQRCodeService qrCodeService) =>
            {
                if (!qrCodeService.IsValidIncentiveCode(code))
                    return Results.NotFound();

                var scanUrl = $"{request.Scheme}://{request.Host}/api/scan/{code}";
                var imageBytes = qrCodeService.RenderQRCodeImage(scanUrl);
                return Results.File(imageBytes, "image/png");
            })
            .AllowAnonymous()
            .Produces<byte[]>(StatusCodes.Status200OK, "image/png")
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription(
                "Returns a server-rendered PNG QR code image for the given incentive code. " +
                "The QRCodeImageUrl field in API responses points to this endpoint.")
            .WithName("GetQRCodeImage");

            return app;
        }
    }
}
