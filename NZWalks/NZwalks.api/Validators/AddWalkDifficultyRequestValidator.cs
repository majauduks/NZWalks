using FluentValidation;
using NZwalks.api.Models.DTO;

namespace NZwalks.api.Validators
{
    public class AddWalkDifficultyRequestValidator : AbstractValidator <Models.DTO.AddWalkDifficultyRequest>
    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x=>x.Code).NotEmpty();
        }
    }
}
