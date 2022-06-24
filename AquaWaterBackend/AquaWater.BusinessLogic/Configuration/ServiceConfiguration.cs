using AquaWater.BusinessLogic.Services.Implementations;
using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.Data.Repository;
using AquaWater.Data.Repository.Implementations;
using AquaWater.Data.Repository.Interfaces;
using AquaWater.Data.Services.Implementations;
using AquaWater.Data.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AquaWater.Data.Configuration
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped<ICompanyManagerService, CompanyManagerService>();
            services.AddScoped<IConfirmationMailService, ConfirmationMailService>();
            services.AddScoped<IMailService, MailService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAuthenticationServices, AuthenticationService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFindApplicationUser, FindApplicationUser>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}
