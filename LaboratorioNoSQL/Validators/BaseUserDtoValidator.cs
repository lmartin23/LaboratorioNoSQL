using FluentValidation;
using LaboratorioNoSQL.Dtos;
using LaboratorioNoSQL.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioNoSQL.Validators
{
    public class BaseUserDtoValidator : AbstractValidator<BaseUserDto>
    {
        public BaseUserDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("{PropertyName} no puede ser vacio")
            .EmailAddress().WithMessage("{PropertyName} debe tener formato de email");
            RuleFor(x => x.Password).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("{PropertyName} no puede ser vacio");

        }
    }
}
