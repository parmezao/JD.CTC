using FluentValidation;
using JD.CTC.Shared.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JD.CTC.Shared.Validator
{
    public class LegadoValidator : AbstractValidator<Legado>
    {
        public LegadoValidator()
        {
            RuleFor(x => x.CdLegado).NotEmpty().WithMessage("*campo obrigatório");
            RuleFor(x => x.NomeLegado).NotEmpty().WithMessage("*campo obrigatório");
            RuleFor(x => x.SitLegado).NotEmpty().WithMessage("*campo obrigatório");
        }
    }
}
