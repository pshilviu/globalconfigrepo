using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Shouldly;
using Xunit;

namespace ORG.HttpClient.UnitTests.Installer
{
    public class InstallerTests
    {
        [Fact]
        public void AddAzureAdSecureClientApi_WhenCalled_AddsRequiredServices()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "MockOptions:BaseUrl", "https://google.co.uk" },
                    { "AzureAd:EnabledScopes:[0]", "12345" },
                    { "AzureAd:EnabledScopes:[1]", "6789" }
                })
                .Build();

            var sp = BuildProviderWithConfig(configuration);

            // Act
            var service = sp.GetRequiredService<IMockService>();

            // Assert
            service.ShouldNotBeNull();
            service.Client.BaseAddress.ShouldBe(new Uri("https://google.co.uk"));
            service.Options.Scopes.ShouldBe("12345 6789");
        }

        [Fact]
        public void AddAzureAdSecureClientApi_WithScopeOverride_ScopeIsOverridden()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "MockOptions:Scopes", "MyScope" },
                    { "MockOptions:BaseUrl", "https://google.co.uk" }
                })
                .Build();

            var sp = BuildProviderWithConfig(configuration);

            // Act
            var service = sp.GetRequiredService<IMockService>();

            // Assert
            service.Options.Scopes.ShouldBe("MyScope");
        }

        private static ServiceProvider BuildProviderWithConfig(IConfiguration configuration)
        {
            var sc = new ServiceCollection();
            sc.AddSingleton(configuration);
            sc.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi()
                .AddInMemoryTokenCaches();
            sc.AddAzureAdSecureClientApi<MockOptions, IMockService, MockService>();
            var sp = sc.BuildServiceProvider();

            return sp;
        }
    }
}