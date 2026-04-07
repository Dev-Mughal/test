namespace Common.Features.BusinessCategory
{
    public record BusinessCategoryItemDto(
        int CategoryId,
        string CategoryName,
        string CategorySlug,
        short DisplayOrder
    );

    public record BusinessCategoryColumnDto(
        short Column,
        IReadOnlyList<BusinessCategoryItemDto> Categories
    );
}
