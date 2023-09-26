using BLOGPESSOAL.Model;
using FluentValidation;

namespace BLOGPESSOAL.Validator
{
    public class TemaValidator : AbstractValidator<Tema>
    {

        public TemaValidator()
        {
            RuleFor(t => t.Descricao)
                .NotEmpty()
                .MaximumLength(255);

        }

    }
}
