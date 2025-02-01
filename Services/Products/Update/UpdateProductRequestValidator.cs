using App.Repositories.Products;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Products.Update
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        private readonly IProductRepository _productRepository;
        public UpdateProductRequestValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x).MustAsync(MustUniqueProductNameAsync).WithMessage("Ürün ismi veritabanında bulunmaktadır.");

            RuleFor(x => x.Name)
                //.NotNull().WithMessage("Ürün ismi gereklidir.")
                .NotEmpty().WithMessage("Ürün ismi gereklidir.")
                .Length(3, 10).WithMessage("Ürün ismi 3 ile 10 karakter arasında olmalıdır.");
                //.Must(MustUniqueProductName).WithMessage("Ürün ismi veritabanında bulunmaktadır.");
                //.MustAsync(MustUniqueProductNameAsync).WithMessage("Ürün ismi veritabanında bulunmaktadır.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Ürün fiyatı 0'dan büyük olmalıdır.");

            RuleFor(x => x.Stock)
                .InclusiveBetween(1, 100).WithMessage("Stok adedi 1 ile 100 arasında olmalıdır.");

            RuleFor(x => x.CategoryId)
               .GreaterThan(0).WithMessage("Ürün kategori değeri 0'dan büyük olmalıdır");
        }

        private async Task<bool> MustUniqueProductNameAsync(UpdateProductRequest updateProductRequest, CancellationToken cancellationToken)
        {
            var anyProduct = await _productRepository.Where(x => x.Name == updateProductRequest.Name && x.Id != updateProductRequest.Id).AnyAsync();

            return !anyProduct;
        }
    }
}
