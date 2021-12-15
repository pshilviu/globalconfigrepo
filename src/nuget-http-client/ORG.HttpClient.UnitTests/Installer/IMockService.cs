namespace ORG.HttpClient.UnitTests.Installer
{
    public interface IMockService
    {
        public System.Net.Http.HttpClient Client { get; set; }

        public MockOptions Options { get; set; }
    }
}