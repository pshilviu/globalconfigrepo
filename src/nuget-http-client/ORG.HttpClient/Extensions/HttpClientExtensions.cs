using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ORG.HttpClient.Exceptions;

namespace ORG.HttpClient.Extensions
{
    /// <summary>
    /// HTTP Client Extensions
    /// </summary>
    public static class HttpClientExtensions
    {
        private static JsonSerializerOptions SerializerOptions { get; } = new(JsonSerializerDefaults.Web);

        /// <summary>
        /// Get with deserialized Json response
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="route"></param>
        /// <typeparam name="TResponse">Response body model</typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException">Api Exception</exception>
        public static async Task<TResponse?> GetAsync<TResponse>(this System.Net.Http.HttpClient httpClient, string route)
        {
            var httpResponse = await httpClient.GetAsync(route);
            var result = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApiException(
                    $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}");
            }

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TResponse>(result, SerializerOptions)!;
        }

        /// <summary>
        /// Post with JSON serialised request body and response body
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="route"></param>
        /// <param name="requestModel"></param>
        /// <typeparam name="TRequest">Request body model</typeparam>
        /// <typeparam name="TResponse">Response body model</typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException">Api Exception</exception>
        public static async Task<TResponse?> PostAsync<TRequest, TResponse>(this System.Net.Http.HttpClient httpClient, string route, TRequest requestModel)
        {
            using var content = new StringContent(JsonSerializer.Serialize(requestModel, SerializerOptions), Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);
            var result = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApiException(
                    $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}");
            }

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TResponse>(result, SerializerOptions)!;
        }

        /// <summary>
        /// Post with JSON serialized request body, no response body
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="route"></param>
        /// <param name="requestModel"></param>
        /// <typeparam name="TRequest">Request body model</typeparam>
        /// <exception cref="ApiException">Api Exception</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task PostAsync<TRequest>(this System.Net.Http.HttpClient httpClient, string route, TRequest requestModel)
        {
            using var content = new StringContent(JsonSerializer.Serialize(requestModel, SerializerOptions), Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PostAsync(route, content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApiException(
                    $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}");
            }
        }

        /// <summary>
        /// Post with no body or response body
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="route"></param>
        /// <exception cref="ApiException">Api Exception</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task PostAsync(this System.Net.Http.HttpClient httpClient, string route)
        {
            using var content = new StringContent(string.Empty);

            var httpResponse = await httpClient.PostAsync(route, content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApiException(
                    $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}");
            }
        }

        /// <summary>
        /// Post with no request body, deserialized JSON response body
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="route"></param>
        /// <typeparam name="TResponse">Response body model</typeparam>
        /// <returns></returns>
        /// <exception cref="ApiException">Api Exception</exception>
        public static async Task<TResponse?> PostAsync<TResponse>(this System.Net.Http.HttpClient httpClient, string route)
        {
            using var content = new StringContent(string.Empty);

            var httpResponse = await httpClient.PostAsync(route, content);
            var result = await httpResponse.Content.ReadAsStringAsync();

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApiException(
                    $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}");
            }

            if (httpResponse.StatusCode == HttpStatusCode.NoContent)
            {
                return default;
            }

            return JsonSerializer.Deserialize<TResponse>(result, SerializerOptions)!;
        }

        /// <summary>
        /// Put with JSON serialized
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="route"></param>
        /// <param name="requestModel"></param>
        /// <typeparam name="TRequest">Request body model</typeparam>
        /// <exception cref="ApiException">Api Exception</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task PutAsync<TRequest>(this System.Net.Http.HttpClient httpClient, string route, TRequest requestModel)
        {
            using var content = new StringContent(JsonSerializer.Serialize(requestModel, SerializerOptions), Encoding.UTF8, "application/json");

            var httpResponse = await httpClient.PutAsync(route, content);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ApiException(
                    $"Response for requested URI: {route} did not return successfully. Status Code: {httpResponse.StatusCode}");
            }
        }
    }
}