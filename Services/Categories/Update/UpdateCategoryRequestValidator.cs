using App.Repositories.Categories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Categories.Update
{
    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        private readonly ICategoryRepository _categoryRepository;
        public UpdateCategoryRequestValidator(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            RuleFor(x => x)
                .MustAsync(MustUniqueCategoryNameAsync).WithMessage("Kategori ismi veritabanında bulunmaktadır.");

            RuleFor(x => x.Name)
                //.NotNull().WithMessage("Kategori ismi gereklidir.")
                .NotEmpty().WithMessage("Kategori ismi gereklidir.");
        }

        private async Task<bool> MustUniqueCategoryNameAsync(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var anyCategory = await _categoryRepository.Where(x => x.Name == request.Name).AnyAsync();

            return !anyCategory;
        }
    }
}
