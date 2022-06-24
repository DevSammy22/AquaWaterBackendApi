using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Repository;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region Consructor 
        private readonly IMapper _mapper;
        private readonly IGenericRepo<CustomerFavourite> _customerFavouriteRepo;
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<Customer> _customerRepo;
        private readonly IGenericRepo<Company> _companyRepo;
        private readonly IFindApplicationUser _findApplicationUser;
        private readonly IGenericRepo<ProductGallery> _productGalleryRepo;

        public ProductService(IMapper mapper, IGenericRepo<Product> productRepo, IGenericRepo<CustomerFavourite> customerFavouriteRepo,IGenericRepo<Customer> customerRepo,
 IGenericRepo<Company> companyRepo, IFindApplicationUser findApplicationUser, IGenericRepo<ProductGallery> productGalleryRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _customerFavouriteRepo = customerFavouriteRepo;
            _customerRepo = customerRepo;
            _companyRepo = companyRepo;
            _findApplicationUser = findApplicationUser;
            _productGalleryRepo = productGalleryRepo;
            _companyRepo = companyRepo;
        }
        #endregion
        #region Implimentation

        public async Task RemoveFavouriteProduct(Guid productId, Guid custommerId)
        {
            var customerFavourite = _customerFavouriteRepo.Table.FirstOrDefault(x => x.ProductId == productId && x.CustomerId == custommerId);

            if (customerFavourite == null)
            {
                return;
            }
            await _customerFavouriteRepo.DeleteAsync(customerFavourite.Id);
        }

        public async Task<SearchResponse<IEnumerable<ProductDTO>>> GetProductsByCompanyID(SearchRequest<ProductSearchDTO> search)
        {
            var checkCompany = await _companyRepo.GetByIdAysnc(search.Data.CompanyId);

            if (checkCompany == null)
            {
                throw new ArgumentException($"Company with {search.Data.CompanyId} does not exist");
            }

            var companyProducts = _productRepo.TableNoTracking.Where(x => x.CompanyId == search.Data.CompanyId)
                            .Include(x => x.ProductGallery).Include(x => x.Ratings).OrderBy(x => x.Id);
            return Paginator.Pagination<Product, ProductDTO>(companyProducts, _mapper, search.PageSize, search.Page);
        }

        public async Task<Response<ProductResponseDTO>> GetProductByIdAsync(string productId)
        {
            var product = await _productRepo.TableNoTracking.Where(x => x.Id == Guid.Parse(productId))
                                                    .Include(x => x.Reviews)
                                                    .Include(x => x.ProductGallery)
                                                    .AsNoTracking().FirstOrDefaultAsync();
            if (product != null)
            {
                var response = new Response<ProductResponseDTO>
                {
                    Data = _mapper.Map<ProductResponseDTO>(product),
                    Success = true
                };
                return response;
            }

            throw new ArgumentException($"Product with {productId} Not Found");
        }

        public async Task<Response<string>> AddFavoriteProductAsync(string productId, string userId)
        {
            var customer = _customerRepo.Table.Where(x => x.UserId == userId).FirstOrDefault();
            var customerFavourite = _customerFavouriteRepo.Table.FirstOrDefault(x => x.CustomerId == customer.Id && x.ProductId == Guid.Parse(productId));
            if (customerFavourite != null)
            {
                throw new ArgumentException("The product is already a favourite");
            }
            if (customer != null)
            {
                await _customerFavouriteRepo.InsertAsync(new CustomerFavourite
                {
                    CustomerId = customer.Id,
                    ProductId = Guid.Parse(productId)
                });

                return new Response<string>()
                {
                    Message = $"The product with {productId} has been succesfully added to your favourite",
                    Success = true
                };
            }
            throw new ArgumentException("Customer does not exit");
        }

        public async Task<Response<ProductCreatedResponseDTO>> CreateProductAsync(ProductCreateRequestDTO createProduct, string userId)
        {
            var companyManager = await _findApplicationUser.GetCompanyManagerByUserIdAsync(userId);
            if (companyManager == null)
            {
                throw new ArgumentException("Company manager does not exist");
            }

            var company = await _companyRepo.Table.Where(x => x.Id == createProduct.CompanyId).FirstOrDefaultAsync();
            if (company == null)
            {
                throw new ArgumentException("Company does not exist");
            }

            if (company.Id != companyManager.CompanyId)
            {
                throw new ArgumentException("Company manager does not belong to this company");
            }

            using (var transaction = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeAsyncFlowOption.Enabled))
            {
                var productToBeCreated = _mapper.Map<Product>(createProduct);
                await _productRepo.InsertAsync(productToBeCreated);

                var response = new Response<ProductCreatedResponseDTO>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = _mapper.Map<ProductCreatedResponseDTO>(productToBeCreated)
                };

                return response;
            }
        }

        public async Task<Response<UpdateProductResponse>> UpdateProductAsync(ProductUpdateRequest productUpdateRequest, string userId)
        {
            var companyManager = await _findApplicationUser.GetCompanyManagerByUserIdAsync(userId);
            if (companyManager == null) throw new ArgumentException("Company Manger not found");

            var product = await _productRepo.TableNoTracking.Where(x => x.Id == productUpdateRequest.Id).Include(x => x.ProductGallery).FirstOrDefaultAsync();
            if (product == null) throw new ArgumentException("Product does not exist");

            if (companyManager.CompanyId != product.CompanyId) throw new ArgumentException("Product does not belong to Company Manager");

            foreach (var gallery in product.ProductGallery)
            {
                await _productGalleryRepo.DeleteAsync(gallery);
            }

            await _productRepo.DeleteAsync(product);
            var newUpdatedProduct = _mapper.Map<Product>(productUpdateRequest);

            await _productRepo.UpdateAsync(newUpdatedProduct);

            return new Response<UpdateProductResponse>()
            {
                Errors = null,
                Message = "Successful",
                Success = true,
                Data = _mapper.Map<UpdateProductResponse>(productUpdateRequest)
            };

        }


        public async Task<Response<string>> DeleteProduct(string productId, string userId)
        {
            var companyManager = await _findApplicationUser.GetCompanyManagerByUserIdAsync(userId);
            if (companyManager == null)
            {
                throw new ArgumentException($"Company Manager with UserId: {userId} not found");
            }
            var product = await _productRepo.GetByIdAysnc(Guid.Parse(productId));
            if (product == null)
            {
                throw new ArgumentException($"Product with Id: {productId} not found");
            }
            if (companyManager.CompanyId == product.CompanyId)
            {
                await _productRepo.DeleteAsync(product);
            }
            return new Response<string>()
            {
                Success = true,
                Message = "Product deleted Successfully",

            };
        }
        #endregion

        public async Task<Response<string>> RemoveProductImage(string productId, string imageId)
        {
            var product =await _productRepo.Table.Where(x => x.Id == Guid.Parse(productId)).FirstOrDefaultAsync();
            if (product != null)
            {
                var image = await _productGalleryRepo.Table.Where(x => x.Id == Guid.Parse(imageId) && x.ProductId == Guid.Parse(productId)).FirstOrDefaultAsync();
                if (image != null)
                {
                    await _productGalleryRepo.DeleteAsync(image);
                    return new Response<string>()
                    {
                        Success = true,
                        Message = "Product image deleted successfully",
                        Data = null,
                        Errors = null
                    };
                }
                return new Response<string>()
                {
                    Success = false,
                    Message = "Product image not found",
                    Data = null,
                    Errors = null
                };

            }

            return new Response<string>()
            {
                Success = false,
                Message = "Product image not found",
                Data = null,
                Errors = null
            };
        }
    }
}