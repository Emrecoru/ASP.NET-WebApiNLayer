using App.Repositories.Products;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products.Create
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.Name)
                //.NotNull().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır.")
                //.Must(MustUniqueProductName).WithMessage("Ürün ismi veritabanında bulunmaktadır.");
                .MustAsync(MustUniqueProductNameAsync).WithMessage("Ürün ismi veritabanında bulunmaktadır.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Ürün kategori değeri 0'dan büyük olmalıdır");
        }

        // Sync validation
        //private bool MustUniqueProductName(string name)
        //{
        //    var anyProduct = _productRepository.Where(x => x.Name == name).Any();

        //    return !anyProduct;
        //}

        // Async validation
        private async Task<bool> MustUniqueProductNameAsync(string name, CancellationToken cancellationToken)
        {
            var anyProduct = await _productRepository.Where(x => x.Name == name).AnyAsync();

            return !anyProduct;
        }
    }
}
