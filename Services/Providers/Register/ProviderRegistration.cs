using TestTask.Contract.Providers;

namespace TestTask.Services.Providers.Register;

public static class ProviderRegistration
{
    public static IServiceCollection AddProviders(this IServiceCollection serviceCollection) {
        serviceCollection.AddScoped<ProviderOneService>();
        serviceCollection.AddScoped<ProviderTwoService>();

        return serviceCollection;
    }

    public static IEnumerable<Type> GetProviders() {
        return typeof(ISearchProvider).Assembly
            .GetTypes()
            .Where(x => !x.IsAbstract && !x.IsInterface && typeof(ISearchProvider).IsAssignableFrom(x));
    }
}