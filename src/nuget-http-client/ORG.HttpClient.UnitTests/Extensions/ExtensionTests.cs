using System;
using System.Net;
using System.Threading.Tasks;
using ORG.HttpClient.Exceptions;
using ORG.HttpClient.Extensions;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace ORG.HttpClient.UnitTests.Extensions
{
    public class ExtensionTests
    {
        [Fact]
        public async Task GetAsync_CorrectResult()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            var mockResponse = TestJson.LoadTestJson();

            var request = mockHttp.When("http://localhost/test")
                .Respond("application/json", mockResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            var expected = TestJson.ExampleTestJson();

            // Act
            var result = await client.GetAsync<TestJson>("/test");

            // Assert
            result.ShouldBeEquivalentTo(expected);
            mockHttp.GetMatchCount(request).ShouldBe(1);
        }

        [Fact]
        public async Task PostAsync_WithRequestResponse_CorrectResult()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            var mockResponse = TestJson.LoadTestJson();

            var request = mockHttp.Expect("http://localhost/test")
                .WithContent(TestJson.LoadTestJson())
                .Respond("application/json", mockResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            var expected = TestJson.ExampleTestJson();

            // Act
            var result = await client.PostAsync<TestJson, TestJson>("/test", TestJson.ExampleTestJson());

            // Assert
            result.ShouldBeEquivalentTo(expected);
            mockHttp.GetMatchCount(request).ShouldBe(1);
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task PostAsync_WithRequestOnly_CorrectResult()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            var request = mockHttp.Expect("http://localhost/test")
                .WithContent(TestJson.LoadTestJson())
                .Respond(HttpStatusCode.Accepted);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            await client.PostAsync<TestJson>("/test", TestJson.ExampleTestJson());

            // Assert
            mockHttp.GetMatchCount(request).ShouldBe(1);
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task PostAsync_WithResponseOnly_CorrectResult()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            var mockResponse = TestJson.LoadTestJson();

            var request = mockHttp.Expect("http://localhost/test")
                .Respond("application/json", mockResponse);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            var expected = TestJson.ExampleTestJson();

            // Act
            var response = await client.PostAsync<TestJson>("/test");

            // Assert
            response.ShouldBeEquivalentTo(expected);
            mockHttp.GetMatchCount(request).ShouldBe(1);
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task PostAsync_WithNoRequestOrResponse_CorrectResult()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            var request = mockHttp.Expect("http://localhost/test")
                .Respond(HttpStatusCode.Accepted);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            await client.PostAsync("/test");

            // Assert
            mockHttp.GetMatchCount(request).ShouldBe(1);
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task PutAsync_WithRequest_CorrectResult()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            var request = mockHttp.Expect("http://localhost/test")
                .WithContent(TestJson.LoadTestJson())
                .Respond(HttpStatusCode.Accepted);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            await client.PutAsync<TestJson>("/test", TestJson.ExampleTestJson());

            // Assert
            mockHttp.GetMatchCount(request).ShouldBe(1);
            mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task GetAsync_Non200Status_Throws()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/test")
                .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            await client.GetAsync<TestJson>("/test").ShouldThrowAsync(typeof(ApiException));
        }

        [Fact]
        public async Task GetAsync_204Status_ReturnsDefault()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/test")
                .Respond(HttpStatusCode.NoContent);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            var response = await client.PostAsync<TestJson>("/test");

            // Assert
            response.ShouldBeNull();
        }

        [Fact]
        public async Task PostAsync_Non200Status_Throws()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/test")
                .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            await client.PostAsync<TestJson>("/test").ShouldThrowAsync(typeof(ApiException));
            await client.PostAsync("/test").ShouldThrowAsync(typeof(ApiException));
            await client.PostAsync<TestJson, TestJson>("/test", TestJson.ExampleTestJson()).ShouldThrowAsync(typeof(ApiException));
            await client.PostAsync("/test", TestJson.ExampleTestJson()).ShouldThrowAsync(typeof(ApiException));
        }

        [Fact]
        public async Task PostAsync_204Status_ReturnsNull()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/test")
                .Respond(HttpStatusCode.NoContent);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            var response = await client.GetAsync<TestJson>("/test");

            // Assert
            response.ShouldBeNull();
        }

        [Fact]
        public async Task PutAsync_Non200Status_Throws()
        {
            // Arrange
            using var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://localhost/test")
                .Respond(HttpStatusCode.BadRequest);

            var client = mockHttp.ToHttpClient();
            client.BaseAddress = new Uri("http://localhost/");

            // Act
            await client.PutAsync<TestJson>("/test", TestJson.ExampleTestJson()).ShouldThrowAsync(typeof(ApiException));
        }
    }
}