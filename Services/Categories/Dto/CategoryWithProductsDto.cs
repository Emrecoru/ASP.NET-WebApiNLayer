using App.Services.Products.Dto;

namespace App.Services.Categories.Dto
{
    public record CategoryWithProductsDto(int Id, string Name, List<ProductDto> Products);
}
