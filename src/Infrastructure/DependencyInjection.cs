using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Solution.Application.Services;
using Solution.Domain.Interfaces.Repositories;
using Solution.Infrastructure.Authentication;
using Solution.Infrastructure.Repositories;
using System.IO;
using System.Text;

namespace Solution.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
            => services
                .AddPersistence(configuration)
                .AddAuthentication(configuration);

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var companies = File.Open(configuration.GetValue<string>("Storage:Companies"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var groups = File.Open(configuration.GetValue<string>("Storage:Groups"), FileMode.OpenOrCreate, FileAccess.ReadWrite);

            return services
                .AddSingleton<ICompanyRepository>(new CompanyRepository(companies))
                .AddSingleton<IGroupRepository>(new GroupRepository(groups));
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptions = configuration.GetSection("Jwt").Get<JwtOptions>();

            services
                .Configure<JwtOptions>(configuration.GetSection("Jwt"))
                .AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret))
                });

            return services;
        }
    }
}
