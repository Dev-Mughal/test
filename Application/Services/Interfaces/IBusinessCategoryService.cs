using Common.Features.BusinessCategory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Application.Services.Interfaces
{
    public interface IBusinessCategoryService
    {
        Task<IEnumerable<SelectListItem>> GetBusinessCategoriesLookupAsync();
        Task<IEnumerable<BusinessCategoryColumnDto>> GetBusinessCategoriesGroupedAsync();
    }
}