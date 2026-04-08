using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using SA.Accounting.Services.OptionClasses;

namespace SA.Accounting.Application;

public static class ApplicationRegistrations
{
    public static void AddApplicationRegistrations(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddMediatR(o=>o.RegisterServicesFromAssembly(typeof(ApplicationRegistrations).Assembly));
        services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(typeof(ApplicationRegistrations).Assembly);

        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(typeof(ApplicationRegistrations).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfiguration));
    }
}
