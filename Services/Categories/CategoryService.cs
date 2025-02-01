using App.Repositories;
using App.Repositories.Categories;
using App.Services.Categories.Create;
using App.Services.Categories.Dto;
using App.Services.Categories.Update;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryRequest> _createCategoryRequestValidator;
        private readonly IValidator<UpdateCategoryRequest> _updateCategoryRequestValidator;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork, IValidator<CreateCategoryRequest> createCategoryRequestValidator, IValidator<UpdateCategoryRequest> updateCategoryRequestValidator, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _createCategoryRequestValidator = createCategoryRequestValidator;
            _updateCategoryRequestValidator = updateCategoryRequestValidator;
            _mapper = mapper;
        }

        public async Task<ServiceResult<List<CategoryDto>>> GetAllListAsync()
        {
            var categories = await _categoryRepository.GetAll().ToListAsync();

            var categoriesAsDto = _mapper.Map<List<CategoryDto>>(categories);

            return ServiceResult<List<CategoryDto>>.Success(categoriesAsDto);
        }

        public async Task<ServiceResult<CategoryDto>> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if(category is null)
            {
                return ServiceResult<CategoryDto>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
            }

            var categoryAsDto = _mapper.Map<CategoryDto>(category);

            return ServiceResult<CategoryDto>.Success(categoryAsDto);
        }

        public async Task<ServiceResult<CategoryWithProductsDto>> GetCategoryWithProductsAsync(int categoryId)
        {
            var categoriesWithProducts = await _categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if(categoriesWithProducts is null)
            {
                return ServiceResult<CategoryWithProductsDto>.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
            }

            var categoryDtoWithProducts = _mapper.Map<CategoryWithProductsDto>(categoriesWithProducts);

            return ServiceResult<CategoryWithProductsDto>.Success(categoryDtoWithProducts);
        }

        public async Task<ServiceResult<List<CategoryWithProductsDto>>> GetCategoryWithProductsAsync()
        {
            var categoriesWithProducts = await _categoryRepository.GetCategoryWithProducts().ToListAsync();

            var categoryDtoWithProducts = _mapper.Map<List<CategoryWithProductsDto>>(categoriesWithProducts);

            return ServiceResult<List<CategoryWithProductsDto>>.Success(categoryDtoWithProducts);
        }

        public async Task<ServiceResult<int>> CreateAsync(CreateCategoryRequest request)
        {
            var validationResult = await _createCategoryRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return ServiceResult<int>.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }
            
            var newCategory = _mapper.Map<Category>(request);

            await _categoryRepository.AddAsync(newCategory);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult<int>.SuccessAsCreated(newCategory.Id, $"api/categories/{newCategory.Id}");
        }

        public async Task<ServiceResult> UpdateAsync(UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if(category is null)
            {
                return ServiceResult.Fail("Güncellenecek kategori bulunamadı", HttpStatusCode.NotFound);
            }

            var validationResult = await _updateCategoryRequestValidator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                return ServiceResult.Fail(validationResult.Errors.Select(x => x.ErrorMessage).ToList());
            }

            category = _mapper.Map(request, category);

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if(category is null)
            {
                return ServiceResult.Fail("Kategori bulunamadı", HttpStatusCode.NotFound);
            }

            _categoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return ServiceResult.Success(HttpStatusCode.NoContent);
        }
    }
}
