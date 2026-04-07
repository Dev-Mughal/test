namespace Common.Features.Business.DTOs
{
    public record GetBusinessesRequest(
        string? CategoryId = null,
        string? SearchQuery = null,
        int PageNumber = 1,
        int PageSize = 10);
}
