using Common.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BizyPopAPIs.Utilities.CustomAttribute
{
    public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter where T : class
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var dto = context.Arguments.OfType<T>().FirstOrDefault();

            if (dto == null)
                return await next(context);

            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                return Results.UnprocessableEntity<ApiErrorResponseModel>(ApiResponse.ErrorResponse(errors, message: "Validation failed"));
            }
            return await next(context);
        }
    }
}