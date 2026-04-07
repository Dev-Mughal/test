namespace Infrastructure.Pagination
{
    public record PaginationRequest(
        int PageNumber = 1,
        int PageSize = 10)
    {
        public int ValidPageNumber => PageNumber > 0 ? PageNumber : 1;
        public int ValidPageSize => PageSize > 0 && PageSize <= 100 ? PageSize : 10;
    }
}
