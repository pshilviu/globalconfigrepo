namespace ORG.HttpClient.Options
{
    /// <summary>
    /// Base http client options
    /// </summary>
    public interface IBaseClientOptions
    {
        /// <summary>
        /// Base url for client
        /// </summary>
        public string? BaseUrl { get; set; }

        /// <summary>
        /// Timeout value (defaults to 30 seconds)
        /// </summary>
        public double? Timeout { get; set; }
    }
}