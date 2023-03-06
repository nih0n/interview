using Microsoft.Extensions.DependencyInjection;
using Solution.Application.UseCases.AddCompanyCost;
using Solution.Application.UseCases.AddCompanyToGroup;
using Solution.Application.UseCases.CloseCompany;
using Solution.Application.UseCases.CreateCompany;
using Solution.Application.UseCases.CreateGroup;
using Solution.Application.UseCases.GetCompany;
using Solution.Application.UseCases.GetGroup;
using Solution.Application.UseCases.GetGroupCompanies;
using Solution.Application.UseCases.GetGroupCosts;
using Solution.Domain.Interfaces;

namespace Solution.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
            => services
                .AddUseCases()
                .AddScoped<INotificationContext, NotificationContext>();
        private static IServiceCollection AddUseCases(this IServiceCollection services)
            => services
                .AddScoped<IGetCompanyUseCase,  GetCompanyUseCase>()
                .AddScoped<ICreateCompanyUseCase, CreateCompanyUseCase>()
                .AddScoped<IAddCompanyCostUseCase, AddCompanyCostUseCase>()
                .AddScoped<ICloseCompanyUseCase, CloseCompanyUseCase>()
                .AddScoped<IGetGroupUseCase, GetGroupUseCase>()
                .AddScoped<IGetGroupCompaniesUseCase, GetGroupCompaniesUseCase>()
                .AddScoped<ICreateGroupUseCase, CreateGroupUseCase>()
                .AddScoped<IAddCompanyToGroupUseCase, AddCompanyToGroupUseCase>()
                .AddScoped<IGetGroupCostsUseCase, GetGroupCostsUseCase>();
    }
}
