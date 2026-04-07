using Application.Utilities.Cache;
using Common.Features.BusinessCategory;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class BusinessCategoryService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        ICacheService cache) : IBusinessCategoryService
    {
        // Business categories are managed by admins and change very rarely —
        // a 24-hour TTL keeps the payload fresh enough without extra DB hits.
        private static readonly TimeSpan LookupTtl = TimeSpan.FromHours(24);

        public Task<IEnumerable<SelectListItem>> GetBusinessCategoriesLookupAsync() =>
            cache.GetOrCreateAsync(
                CacheKeys.BizCategoryLookup,
                () => contextFactory.QueryWithDbContextAsync(async context =>
                    (IEnumerable<SelectListItem>) await context.BusinessCategories
                        .AsNoTracking()
                        .Select(x => new SelectListItem
                        {
                            Text  = x.CategoryName,
                            Value = x.CategoryId.ToString(),
                        })
                        .ToListAsync()
                        .ConfigureAwait(false) ?? []),
                LookupTtl);

        public Task<IEnumerable<BusinessCategoryColumnDto>> GetBusinessCategoriesGroupedAsync() =>
            cache.GetOrCreateAsync(
                CacheKeys.BizCategoryGrouped,
                () => contextFactory.QueryWithDbContextAsync(async context =>
                {
                    var categories = await context.BusinessCategories
                        .AsNoTracking()
                        .Where(x => x.IsActive)
                        .OrderBy(x => x.DisplayColumn)
                        .ThenBy(x => x.DisplayOrder)
                        .Select(x => new { x.CategoryId, x.CategoryName, x.CategorySlug, x.DisplayOrder, x.DisplayColumn })
                        .ToListAsync()
                        .ConfigureAwait(false);

                    return (IEnumerable<BusinessCategoryColumnDto>) categories
                        .GroupBy(x => x.DisplayColumn)
                        .Select(g => new BusinessCategoryColumnDto(
                            g.Key,
                            g.Select(x => new BusinessCategoryItemDto(x.CategoryId, x.CategoryName, x.CategorySlug, x.DisplayOrder))
                             .ToList()
                             .AsReadOnly()));
                }),
                LookupTtl);
    }
}
