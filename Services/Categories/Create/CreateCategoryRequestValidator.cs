using App.Repositories.Categories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Categories.Create
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;
        public CreateCategoryRequestValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x.Name)
                //.NotNull().WithMessage("Kategori ismi gereklidir.")
                .NotEmpty().WithMessage("Kategori ismi gereklidir.")
                .MustAsync(MustUniqueCategoryNameAsync).WithMessage("Kategori ismi veritabanında bulunmaktadır.");
        }

        private async Task<bool> MustUniqueCategoryNameAsync(string name, CancellationToken cancellationToken)
        {
            var anyCategory = await _categoryRepository.Where(x => x.Name == name).AnyAsync();

            return !anyCategory;
        }
    }
}
