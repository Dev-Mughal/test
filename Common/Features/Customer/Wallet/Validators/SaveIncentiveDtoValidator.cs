using Common.Features.Customer.Wallet.DTOs;
using FluentValidation;

namespace Common.Features.Customer.Wallet.Validators
{
    public class SaveIncentiveDtoValidator : AbstractValidator<SaveIncentiveDto>
    {
        public SaveIncentiveDtoValidator()
        {
            RuleFor(x => x.IncentiveTypeId)
                .GreaterThan(0).WithMessage("A valid Incentive Type ID is required.");

            RuleFor(x => x.IncentiveId)
                .GreaterThan(0).WithMessage("A valid Incentive ID is required.");
        }
    }
}
