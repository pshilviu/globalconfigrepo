using Microsoft.Extensions.Options;

namespace ORG.HttpClient.UnitTests.Installer
{
    public class MockService : IMockService
    {
        public MockService(System.Net.Http.HttpClient client, IOptionsMonitor<MockOptions> options)
        {
            Client = client;
            Options = options.Get(nameof(MockOptions));
        }

        public System.Net.Http.HttpClient Client { get; set; }

        public MockOptions Options { get; set; }
    }
}