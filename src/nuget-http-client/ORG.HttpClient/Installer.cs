using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using ORG.HttpClient.Options;

namespace ORG.HttpClient
{
    /// <summary>
    /// Installer class for IServiceCollection extensions
    /// </summary>
    public static class Installer
    {
        /// <summary>
        /// Add Service with a Typed HTTP client that is secured by azure AD DI
        /// </summary>
        /// <param name="services"></param>
        /// <typeparam name="TOptions">Configuration inherited from <see cref="AzureAdClientOptions"/></typeparam>
        /// <typeparam name="TInterface">Interface of service</typeparam>
        /// <typeparam name="TConcrete">Concrete implementation of service</typeparam>
        public static void AddAzureAdSecureClientApi<TOptions, TInterface, TConcrete>(this IServiceCollection services)
            where TOptions : AzureAdClientOptions
            where TInterface : class
            where TConcrete : class, TInterface
        {
            var optionsName = typeof(TOptions).Name;

            // Add named options based on the implementation type of TOptions
            services.AddOptions<TOptions>(optionsName)
                .Configure<IConfiguration>((options, configuration) =>
                {
                    configuration.GetSection(optionsName).Bind(options);
                    ScopeOverrideCheck(configuration, options);
                });

            // Add Client
            services.AddTransient<TInterface, TConcrete>();

            // Add typed client for service
            services.AddHttpClient<TInterface, TConcrete>((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetService<IOptionsMonitor<TOptions>>();

                    client.Timeout = TimeSpan.FromSeconds(options!.Get(optionsName).Timeout ?? 30);
                    client.BaseAddress = new Uri(options.Get(optionsName).BaseUrl!);
                })

                // Add Microsoft Identity Handler for service to service calls
                .AddHttpMessageHandler(serviceProvider => new MicrosoftIdentityUserAuthenticationMessageHandler(
                    serviceProvider.GetRequiredService<ITokenAcquisition>(),
                    serviceProvider.GetRequiredService<IOptionsMonitor<TOptions>>(),
                    serviceProvider.GetRequiredService<IOptionsMonitor<MicrosoftIdentityOptions>>(),
                    optionsName));
        }

        private static void ScopeOverrideCheck(IConfiguration configuration, AzureAdClientOptions options)
        {
            if (options.Scopes != null)
            {
                return;
            }

            // Take global config
            var scopesConfig = configuration.GetSection("AzureAd:EnabledScopes").GetChildren();
            StringBuilder scopeBuilder = new();
            foreach (var section in scopesConfig)
            {
                scopeBuilder.Append(section.Value).Append(' ');
            }

            options.Scopes = scopeBuilder.ToString().Trim();
        }
    }
}