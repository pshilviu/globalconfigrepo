using System.Diagnostics.CodeAnalysis;
using Microsoft.Identity.Web;

namespace ORG.HttpClient.Options
{
    /// <summary>
    /// Http client options for azure ad secure endpoints
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class AzureAdClientOptions : MicrosoftIdentityAuthenticationMessageHandlerOptions, IBaseClientOptions
    {
        /// <inheritdoc />
        public string? BaseUrl { get; set; }

        /// <inheritdoc />
        public double? Timeout { get; set; }
    }
}